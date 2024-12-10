using ChatService.API.Models;
using ChatService.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

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

    // dodavanje poruke u odredjenu chat sesiju
    [HttpPost("{sessionId}/messages")]
    public async Task<IActionResult> AddMessageToSession(string sessionId, [FromBody] string content)
    {
        var newMessage = new Message
        {
            Content = content,
            Timestamp = DateTime.UtcNow
        };

        await _chatRepository.AddMessageToChatSessionAsync(sessionId, newMessage);
        return CreatedAtAction(nameof(GetMessagesFromSession), new { sessionId }, newMessage);
    }

    // dohvatanje svih poruka iz odredjene chat sesije
    [HttpGet("{sessionId}/messages")]
    public async Task<IActionResult> GetMessagesFromSession(string sessionId)
    {
        //var messages = await _chatRepository.GetMessagesFromChatSessionAsync(sessionId);
        //HARDKODIRANO
        var messages = new List<Message>()
        {
            new Message() {Id = ObjectId.GenerateNewId(), Content = "Hello World", Timestamp = DateTime.UtcNow},
            new Message() {Id = ObjectId.GenerateNewId(), Content = "Hello World", Timestamp = DateTime.UtcNow}
        };
        
        if (messages == null || !messages.Any())
        {
            return NotFound();
        }

        return Ok(messages);
    }

    [HttpPost("sessions")]
    public async Task<IActionResult> CreateChatSession([FromBody] ChatSession session)
    {
        await _chatRepository.CreateChatSessionAsync(session);
        return CreatedAtAction(nameof(GetChatSession), new { trainerId = session.TrainerId, clientId = session.ClientId }, session);
    }

    [HttpGet("sessions")]
    public async Task<IActionResult> GetChatSession([FromQuery] string trainerId, [FromQuery] string clientId)
    {
        var session = await _chatRepository.GetChatSessionAsync(trainerId, clientId);

        if (session == null)
        {
            return NotFound();
        }

        return Ok(session);
    }

    [HttpPost("sessions/{sessionId}/unlock")]
    public async Task<IActionResult> UnlockChatSession(string sessionId)
    {
        await _chatRepository.UnlockChatSessionAsync(sessionId);
        return NoContent();
    }
}