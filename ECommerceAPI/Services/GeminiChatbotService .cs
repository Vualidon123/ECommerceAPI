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
    private readonly string _apiKey = "AIzaSyCrii-etsVL-coPXrXz6LEfLUtnPnz-Cks";
    private readonly string _model = "gemini-1.5-flash";

    public GeminiChatbotService(HttpClient httpClient, ChatLogRepository chatLogRepository, ProductService productService)
    {
        _httpClient = httpClient;
        _chatLogRepository = chatLogRepository;
        _productService = productService;
    }

    public async Task<string> GetAnswerAsync(string message, int userId)
    {
        // 1. Lấy dữ liệu sản phẩm từ DB
        var products = await _productService.GetAllProductsAsync();

        // Ghép danh sách sản phẩm thành text
        var productContext = string.Join("\n", products.Select(p =>
            $"{p.name} - Giá: {p.price:N0} VND - Kho: {p.stock} - Mô tả: {p.description}"
        ));

        // 2. Gửi lên Gemini API
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
                new { text =
                    "Bạn là chatbot hỗ trợ khách hàng cho hệ thống thương mại điện tử Smart E-commerce. " +
                    "Dữ liệu sản phẩm hiện có trong kho:\n" + productContext + "\n\n" +
                    "Chỉ trả lời dựa trên dữ liệu này. " +
                    "Nếu không tìm thấy thông tin, hãy trả lời: 'Xin lỗi, hiện tại tôi chưa có thông tin này.'"
                }
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

        // 3. Lưu chat log
        await SaveChatLog(userId, message, aiReply);

        return aiReply ?? "Xin lỗi, tôi chưa có câu trả lời.";
    }




    public async Task<List<ChatLog>> GetChatHistoryAsync(int userId)
    {
        var chatLogs = await _chatLogRepository.GetChatLogsByUserIdAsync(userId);
        return chatLogs;
    }


    private async Task SaveChatLog(int  userId, string userMessage, string botReply)
    {
       await _chatLogRepository.SaveChatLogAsync(userId, userMessage, botReply);
    }

  
}
