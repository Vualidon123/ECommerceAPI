using ECommerceAPI.Models;
using ECommerceAPI.Repositories;
using ECommerceAPI.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text;
using System.Text.Json;

public class GeminiChatbotService : IChatbotService
{
    private readonly HttpClient _httpClient;
    private readonly ChatLogRepository _chatLogRepository;
    private readonly ProductService _productService;
    private readonly string _apiKey = "YOUR_GEMINI_API_KEY";
    private readonly string _model = "gemini-1.5-flash";

    public GeminiChatbotService(HttpClient httpClient, ChatLogRepository chatLogRepository, ProductService productService)
    {
        _httpClient = httpClient;
        _chatLogRepository = chatLogRepository;
        _productService = productService;
    }

    public async Task<string> GetAnswerAsync(string message, int userId)
    {
        // 1. Thử extract product
        var product = await ExtractProductFromMessageAsync(message);

        if (product != null)
        {
            var reply = $"Sản phẩm **{product.name}** có giá {product.price:N0} VND, " +
                        $"còn {product.stock} sản phẩm trong kho. " +
                        $"Mô tả: {product.description}";

            await SaveChatLog(userId, message, reply);
            return reply;
        }

        // 2. Nếu không thấy -> gọi Gemini
        var url = $"https://generativelanguage.googleapis.com/v1beta/models/{_model}:generateContent?key={_apiKey}";

        var requestBody = new
        {
            contents = new[]
            {
            new { role = "user", parts = new[] { new { text = message } } }
        },
            systemInstruction = new
            {
                role = "system",
                parts = new[]
                {
                new { text = "Bạn là chatbot hỗ trợ khách hàng cho hệ thống thương mại điện tử Smart E-commerce. " +
                             "Chỉ trả lời về sản phẩm, đơn hàng, thanh toán, tài khoản và chính sách. " +
                             "Nếu không có dữ liệu trong hệ thống, hãy trả lời: 'Xin lỗi, hiện tại tôi chưa có thông tin này.'" }
            }
            }
        };

        var json = JsonSerializer.Serialize(requestBody);
        var response = await _httpClient.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));
        var responseJson = await response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(responseJson);
        var aiReply = doc.RootElement
                         .GetProperty("candidates")[0]
                         .GetProperty("content")
                         .GetProperty("parts")[0]
                         .GetProperty("text")
                         .GetString();

        await SaveChatLog(userId, message, aiReply);

        return aiReply ?? "Xin lỗi, tôi chưa có câu trả lời.";
    }



    public async Task<List<string>> GetChatHistoryAsync(int userId)
    {
        var chatLogs = await _chatLogRepository.GetChatLogsByUserIdAsync(userId);
        return chatLogs.Select(cl => $"User: {cl.UserMessage}\nBot: {cl.BotReply}").ToList();
    }


    private async Task SaveChatLog(int  userId, string userMessage, string botReply)
    {
       await _chatLogRepository.SaveChatLogAsync(userId, userMessage, botReply);
    }

    private async Task<Product?> ExtractProductFromMessageAsync(string message)
    {
        // Chuẩn hoá message
        var normalizedMessage = message.ToLower().Trim();

        // Lấy tất cả sản phẩm từ DB
        var products = await _productService.GetAllProductsAsync();

        // Tìm sản phẩm có tên xuất hiện trong message
        foreach (var product in products)
        {
            if (normalizedMessage.Contains(product.name.ToLower()))
            {
                return product;
            }
        }

        return null;
    }
}
