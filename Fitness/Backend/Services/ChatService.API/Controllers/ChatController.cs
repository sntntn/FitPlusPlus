using ChatService.API.Models;
using ChatService.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ChatService.API.Controllers;

//TO DO AUTHORIZATION AND AUTHENTICATION

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly IChatRepository _chatRepository;
    private readonly IMongoClient _mongoClient;

    public ChatController(IChatRepository chatRepository, IMongoClient mongoClient)
    {
        _chatRepository = chatRepository;
        _mongoClient = mongoClient;
    }
    [HttpGet("sessions/trainer/{trainerId}/basic-info")]
    public async Task<IActionResult> GetBasicInfoForTrainerSessions(string trainerId)
    {
        var basicInfo = await _chatRepository.GetBasicInfoForTrainerSessionsAsync(trainerId);

        if (basicInfo == null || !basicInfo.Any())
        {
            return NotFound(new { Message = "No chat sessions found for the specified trainer." });
        }

        return Ok(basicInfo);
    }

    
    [HttpPost("sessions/messages")]
    public async Task<IActionResult> AddMessageToSession([FromQuery] string trainerId, [FromQuery] string clientId, [FromBody] string content, [FromQuery] string senderType)
    {
        if (senderType != "Trainer" && senderType != "Client")
        {
            return BadRequest(new { Message = "Invalid sender type. Must be either 'Trainer' or 'Client'." });
        }
        var session = await _chatRepository.GetChatSessionAsync(trainerId, clientId);

        if (session == null)
        {
            return NotFound(new { Message = "Chat session not found for the specified trainer and client." });
        }

        if (session.ExpirationDate.HasValue && session.ExpirationDate.Value < DateTime.UtcNow)
        {
            return BadRequest(new { Message = "Chat session has expired. Please pay if you want to send a message again." });
        }

        var newMessage = new Message
        {
            Content = content,
            Timestamp = DateTime.UtcNow,
            SenderType = senderType
        };

        await _chatRepository.AddMessageToChatSessionAsync(session.Id.ToString(), newMessage);

        return CreatedAtAction(nameof(GetMessagesFromSession), new { trainerId, clientId }, newMessage);
    }

    

    
    [HttpGet("sessions/messages")]
    public async Task<IActionResult> GetMessagesFromSession([FromQuery] string trainerId, [FromQuery] string clientId)
    {
        var session = await _chatRepository.GetChatSessionAsync(trainerId, clientId);
    
        if (session == null)
        {
            return NotFound(new { Message = "Chat session not found for the specified trainer and client." });
        }
    
        var messages = session.Messages;
    
        if (messages == null || !messages.Any())
        {
            return NotFound(new { Message = "No messages found in the chat session." });
        }
    
        return Ok(messages);
    }


    [HttpPost("sessions")]
    public async Task<IActionResult> CreateChatSession([FromBody] ChatSession session)
    {
        if (session.Id != ObjectId.Empty)
        {
            return BadRequest(new { Message = "ID should not be provided. It will be automatically generated." });
        }

        session.IsUnlocked = true;
        session.ExpirationDate = DateTime.UtcNow.AddDays(30);
        try
        {
            await _chatRepository.CreateChatSessionAsync(session);
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(new { Message = e.Message });
        }
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

    [HttpDelete("sessions")]
    public async Task<IActionResult> DeleteChatSession([FromQuery] string trainerId, [FromQuery] string clientId)
    {
        var success = await _chatRepository.DeleteChatSessionAsync(trainerId, clientId);

        if (success)
        {
            return Ok(new { Message = "Session deleted successfully." });
        }

        return NotFound(new { Message = "Session not found or already deleted." });
    }
    
    [HttpPost("sessions/{sessionId}/unlock")]
    public async Task<IActionResult> UnlockChatSession(string sessionId)
    {
        var success = await _chatRepository.UnlockChatSessionAsync(sessionId);
        if (!success)
        {
            return NotFound(new { Message = "Chat session not found." });
        }
        
        return NoContent();
    }

    // privremena metoda za testiranje
    [HttpGet("test-mongo-connection")]
    public IActionResult TestMongoConnection()
    {
        try
        {
            var databases = _mongoClient.ListDatabaseNames().ToList();
            return Ok(new { Message = "MongoDB connection successful", Databases = databases });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "MongoDB connection failed", Error = ex.Message });
        }
    }

    
}