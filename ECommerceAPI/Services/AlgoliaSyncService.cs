using Algolia.Search.Clients;
using ECommerceAPI.Models;
using ECommerceAPI.Repositories;
using System.Text.Json;

namespace ECommerceAPI.Services
{
    public class AlgoliaSyncService
    {
        private readonly ProductRepository _productRepository;
        private const string APP_ID = "VRWCPQ5YA7";
        private const string API_KEY = "70fef4ae5234f560ee9a3847f730dc7f";
        private const string INDEX_NAME = "products";

        public AlgoliaSyncService(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<bool> SyncProductsToAlgoliaAsync()
        {
            try
            {
                var products = await _productRepository.GetAllProductsAsync();
                
                // Convert products to Algolia format
                var algoliaProducts = products.Select(p => new
                {
                    objectID = p.id.ToString(),
                    name = p.name,
                    description = p.description,
                    price = p.price,
                    category = p.category,
                    stock = p.stock
                }).ToList();

                // Use HTTP client to sync with Algolia
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("X-Algolia-Application-Id", APP_ID);
                httpClient.DefaultRequestHeaders.Add("X-Algolia-API-Key", API_KEY);

                // Use batch upsert instead of clear + add
                var batchUrl = $"https://{APP_ID}-dsn.algolia.net/1/indexes/{INDEX_NAME}/batch";
                var batchData = new
                {
                    requests = algoliaProducts.Select(p => new
                    {
                        action = "addObject", // This will add or update existing records       
                        body = p
                    }).ToList()
                };

                var jsonContent = JsonSerializer.Serialize(batchData);
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
                
                var batchResponse = await httpClient.PostAsync(batchUrl, content);
                
                if (batchResponse.IsSuccessStatusCode)
                {
                    var responseContent = await batchResponse.Content.ReadAsStringAsync();
                    Console.WriteLine($"Successfully synced {algoliaProducts.Count} products to Algolia");
                    Console.WriteLine($"Response: {responseContent}");
                    return true;
                }
                else
                {
                    var errorContent = await batchResponse.Content.ReadAsStringAsync();
                    Console.WriteLine($"Failed to sync products: {batchResponse.StatusCode} - {errorContent}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error syncing products to Algolia: {ex.Message}");
                return false;
            }
        }

        public async Task<string> GetAlgoliaIndexStatus()
        {
            try
            {
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("X-Algolia-Application-Id", APP_ID);
                httpClient.DefaultRequestHeaders.Add("X-Algolia-API-Key", API_KEY);

                var statusUrl = $"https://{APP_ID}-dsn.algolia.net/1/indexes/{INDEX_NAME}";
                var response = await httpClient.GetAsync(statusUrl);
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return content;
                }
                else
                {
                    return $"Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}";
                }
            }
            catch (Exception ex)
            {
                return $"Exception: {ex.Message}";
            }
        }

        public async Task<bool> AddSingleProductToAlgoliaAsync(Product product)
        {
            try
            {
                var algoliaProduct = new
                {
                    objectID = product.id.ToString(),
                    name = product.name,
                    description = product.description,
                    price = product.price,
                    category = product.category,
                    stock = product.stock
                };

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("X-Algolia-Application-Id", APP_ID);
                httpClient.DefaultRequestHeaders.Add("X-Algolia-API-Key", API_KEY);

                var url = $"https://{APP_ID}-dsn.algolia.net/1/indexes/{INDEX_NAME}";
                var jsonContent = JsonSerializer.Serialize(algoliaProduct);
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
                
                var response = await httpClient.PostAsync(url, content);
                
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Successfully added product {product.id} to Algolia");
                    return true;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Failed to add product {product.id}: {response.StatusCode} - {errorContent}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding product {product.id} to Algolia: {ex.Message}");
                return false;
            }
        }
    }
}
