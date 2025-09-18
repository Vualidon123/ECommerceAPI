using ECommerceAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Repositories
{
    public class ChatLogRepository
    {
        private readonly ECommerceDbContext _dbContext;
        public ChatLogRepository(ECommerceDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task SaveChatLogAsync(int userId, string userMessage, string botReply)
        {
            var chatLog = new ChatLog
            {
                UserId = userId,
                UserMessage = userMessage,
                BotReply = botReply,
                CreatedAt = DateTime.UtcNow
            };
            _dbContext.ChatLogs.Add(chatLog);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<List<ChatLog>> GetChatLogsByUserIdAsync(int userId)
        {
            return await _dbContext.ChatLogs
                                   .Where(cl => cl.UserId == userId)
                                   .OrderByDescending(cl => cl.CreatedAt)
                                   .ToListAsync();
        }
    }

}
