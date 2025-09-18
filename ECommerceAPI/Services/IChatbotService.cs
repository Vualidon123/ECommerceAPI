using ECommerceAPI.Repositories;

namespace ECommerceAPI.Services
{
    public interface IChatbotService
    {
        Task<string> GetAnswerAsync(string message, int userId);
        Task<List<string>> GetChatHistoryAsync(int userId);
    }
}
