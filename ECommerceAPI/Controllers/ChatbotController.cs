using ECommerceAPI.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ChatbotController : ControllerBase
{
    private readonly IChatbotService _chatbotService;
    

    public ChatbotController(IChatbotService chatbotService)
    {
        _chatbotService = chatbotService;
    }

    [HttpPost("ask")]
    public async Task<IActionResult> Ask([FromBody] ChatRequest request)
    {
        var answer = await _chatbotService.GetAnswerAsync(request.Message, request.UserId);
        return Ok(new { reply = answer });
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetChatByUserId(int id)
    {
        var product = await _chatbotService.GetChatHistoryAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }
}

public class ChatRequest
{
    public int UserId { get; set; }
    public string Message { get; set; }
}
