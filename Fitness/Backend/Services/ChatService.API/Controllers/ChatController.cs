using ChatService.API.Models;
using ChatService.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ChatService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly IChatRepository _chatRepository;

    public ChatController(IChatRepository chatRepository)
    {
        _chatRepository = chatRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllMessages()
    {
        //var messages = await _chatRepository.GetAllMessagesAsync();
        var messages = "Hello World";
        return Ok(messages);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetMessageById(string id)
    {
        //var message = await _chatRepository.GetMessageByIdAsync(id);
        var message = "Hello World";
        if (message == null)
            return NotFound();
        return Ok(message);
    }

    [HttpPost]
    public async Task<IActionResult> CreateMessage([FromBody] string content)
    {
        var newMessage = new Message
        {
            Content = content,
            Timestamp = DateTime.UtcNow
        };

        await _chatRepository.CreateMessageAsync(newMessage);

        return CreatedAtAction(nameof(GetMessageById), new { id = newMessage.Id.ToString() }, newMessage);
    }
}