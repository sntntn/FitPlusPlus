using ChatService.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChatService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly IChatService _chatService;

    public ChatController(IChatService chatService)
    {
        _chatService = chatService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllMessages()
    {
        var messages = await _chatService.GetAllMessagesAsync();
        return Ok(messages);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetMessageById(int id)
    {
        var message = await _chatService.GetMessageByIdAsync(id);
        if (message == null)
            return NotFound();
        return Ok(message);
    }

    [HttpPost]
    public async Task<IActionResult> CreateMessage([FromBody] string content)
    {
        var message = await _chatService.CreateMessageAsync(content);
        return CreatedAtAction(nameof(GetMessageById), new { id = message.Id }, message);
    }
}