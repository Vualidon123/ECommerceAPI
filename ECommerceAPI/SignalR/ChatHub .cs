using ECommerceAPI.Services;
using Microsoft.AspNetCore.SignalR;

public class ChatHub : Hub
{
    private readonly IChatbotService _chatbotService;

    public ChatHub(IChatbotService chatbotService)  
    {
        _chatbotService = chatbotService;
    }

    public async Task SendMessage(int user, string message)
    {
        var reply = await _chatbotService.GetAnswerAsync(message, user);
        await Clients.Caller.SendAsync("ReceiveMessage", "Bot", reply);
    }
}
