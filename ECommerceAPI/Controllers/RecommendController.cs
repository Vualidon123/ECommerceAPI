using Algolia.Search.Clients;
using Algolia.Search.Models.Recommend;
using ECommerceAPI.Models;
using ECommerceAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

[ApiController]
[Route("api/[controller]")]
public class RecommendationController : ControllerBase
{
    private readonly RecommendClient _recommendClient;
    private readonly ProductService _productService;
    private readonly AlgoliaSyncService _algoliaSyncService;
    
    public RecommendationController(ProductService productService, AlgoliaSyncService algoliaSyncService)
    {
        string appId = "VRWCPQ5YA7";
        string apiKey = "70fef4ae5234f560ee9a3847f730dc7f";
        _recommendClient = new RecommendClient(appId, apiKey);
        _productService = productService;
        _algoliaSyncService = algoliaSyncService;
    }

  

[HttpGet("similar/{productId}")]
public async Task<IActionResult> GetSimilarProducts(int productId)
{
    try
    {
        // Step 1: Verify the product exists in our database
        var product = await _productService.GetProductByIdAsync(productId);
        if (product == null)
        {
            return NotFound($"Product with ID {productId} not found in database.");
        }

        // Step 2: Make the Algolia recommendation request
        var response = await _recommendClient.GetRecommendationsAsync(
            new GetRecommendationsParams
            {
                Requests = new List<RecommendationsRequest>
                {
                    new RecommendationsRequest(
                        new RelatedQuery
                        {
                            IndexName = "products",
                            ObjectID = product.id.ToString(),
                            Model = RelatedModel.RelatedProducts,
                            Threshold = 10.0,
                            MaxRecommendations = 5
                        }
                    ),
                },
            }
        );

        var results = response.Results.First();

        // Step 3: Map hits to Product list
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        var similarProducts = results.Hits
            .Select(hit =>
            {
                string json = JsonSerializer.Serialize(hit); // hit → JSON
                return JsonSerializer.Deserialize<Product>(json, options); // JSON → Product
            })
            .Where(p => p != null)
            .ToList();

        if (!similarProducts.Any())
        {
            return Ok(new { message = "No similar products found" });
        }

        return Ok(similarProducts);
    }
    catch (Exception ex)
    {
        return StatusCode(500, new { error = ex.Message });
    }
}

[HttpGet("debug/products")]
    public async Task<IActionResult> GetDebugProducts()
    {
        try
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(new 
            { 
                totalProducts = products.Count(),
                products = products.Select(p => new 
                {
                    id = p.id,
                    name = p.name,
                    category = p.category,
                    price = p.price
                }).ToList()
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpPost("sync-products")]
    public async Task<IActionResult> SyncProductsToAlgolia()
    {
        try
        {
            var success = await _algoliaSyncService.SyncProductsToAlgoliaAsync();
            
            if (success)
            {
                var products = await _productService.GetAllProductsAsync();
                return Ok(new 
                { 
                    message = "Products successfully synced to Algolia!",
                    count = products.Count(),
                    sample = products.Take(3).Select(p => new 
                    {
                        id = p.id,
                        name = p.name,
                        category = p.category
                    }).ToList(),
                    nextStep = "Now try calling /api/Recommendation/similar/{productId} to test similarity search"
                });
            }
            else
            {
                return StatusCode(500, new { error = "Failed to sync products to Algolia" });
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpGet("algolia-status")]
    public async Task<IActionResult> GetAlgoliaIndexStatus()
    {
        try
        {
            var status = await _algoliaSyncService.GetAlgoliaIndexStatus();
            return Ok(new { status = status });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpGet("similar-test/{productId}")]
    public async Task<IActionResult> TestSimilarProductsWithDifferentParams(int productId)
    {
        try
        {
            var product = await _productService.GetProductByIdAsync(productId);
            if (product == null)
            {
                return NotFound($"Product with ID {productId} not found in database.");
            }

            var results = new List<object>();

            // Test 1: Very low threshold, no filters
            try
            {
                var response1 = await _recommendClient.GetRecommendationsAsync(
                    new GetRecommendationsParams
                    {
                        Requests = new List<RecommendationsRequest>
                        {
                            new RecommendationsRequest(
                                new RelatedQuery
                                {
                                    IndexName = "products",
                                    ObjectID = product.id.ToString(),
                                    Model = RelatedModel.RelatedProducts,
                                    Threshold = 1.0, // Very low threshold
                                    MaxRecommendations = 5
                                }
                            ),
                        },
                    }
                );
                results.Add(new { test = "Threshold 1.0, no filters", hits = response1.Results.First().Hits?.Count ?? 0, data = response1.Results.First().Hits });
            }
            catch (Exception ex)
            {
                results.Add(new { test = "Threshold 1.0, no filters", error = ex.Message });
            }

            // Test 2: Medium threshold with category filter
            try
            {
                var response2 = await _recommendClient.GetRecommendationsAsync(
                    new GetRecommendationsParams
                    {
                        Requests = new List<RecommendationsRequest>
                        {
                            new RecommendationsRequest(
                                new RelatedQuery
                                {
                                    IndexName = "products",
                                    ObjectID = product.id.ToString(),
                                    Model = RelatedModel.RelatedProducts,
                                    Threshold = 20.0,
                                    MaxRecommendations = 5,
                                    QueryParameters = new RecommendSearchParams
                                    {
                                        Filters = $"category:\"{product.category}\""
                                    }
                                }
                            ),
                        },
                    }
                );
                results.Add(new { test = "Threshold 20.0, category filter", hits = response2.Results.First().Hits?.Count ?? 0, data = response2.Results.First().Hits });
            }
            catch (Exception ex)
            {
                results.Add(new { test = "Threshold 20.0, category filter", error = ex.Message });
            }

            return Ok(new 
            { 
                product = new { id = product.id, name = product.name, category = product.category },
                testResults = results
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }
}
