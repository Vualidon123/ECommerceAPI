namespace ECommerce.Domain.Entities
{
    public class ChatLog
    {
        public int Id { get; set; }
        public string SessionId { get; set; } = string.Empty;
        public string UserMessage { get; set; } = string.Empty;
        public string BotResponse { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
    }
}