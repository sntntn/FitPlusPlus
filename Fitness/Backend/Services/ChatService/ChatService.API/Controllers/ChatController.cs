using ChatService.API.Models;
using ChatService.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatService.API.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class ChatController : ControllerBase
{
    private readonly IChatService _chatService;

    public ChatController(IChatService chatService)
    {
        _chatService = chatService;
    }

    [Authorize(Roles = "Admin, Trainer, Client")]
    [HttpGet("sessions/{userId}/my-sessions-summary")]
    public async Task<IActionResult> GetBasicInfoForSessions(string userId)
    {
        var basicInfo = await _chatService.GetBasicInfoForSessionsAsync(userId);
        if (basicInfo == null || !basicInfo.Any())
        {
            return NotFound(new { Message = "No chat sessions found for the specified trainer." });
        }

        return Ok(basicInfo);
    }

    [Authorize(Roles = "Trainer, Client")]
    [HttpPost("sessions/messages")]
    public async Task<IActionResult> AddMessageToSession([FromQuery] string trainerId, [FromQuery] string clientId, [FromBody] string content, [FromQuery] string senderType)
    {
        try
        {
            await _chatService.AddMessageToSessionAsync(trainerId, clientId, content, senderType);
            return Ok(new { Message = "Message added successfully." });
        }
        catch (Exception e)
        {
            return BadRequest(new { Message = e.Message });
        }
    }
    
    [Authorize(Roles = "Trainer, Client")]
    [HttpGet("sessions/messages")]
    public async Task<IActionResult> GetMessagesFromSession([FromQuery] string trainerId, [FromQuery] string clientId)
    {
        var session = await _chatService.GetChatSessionAsync(trainerId, clientId);
    
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

    [Authorize(Roles = "Client")]
    [HttpPost("sessions")]
    public async Task<IActionResult> CreateChatSession([FromQuery] string trainerId, [FromQuery] string clientId)
    {
        try
        {
            await _chatService.CreateChatSessionAsync(trainerId, clientId);
            return Ok(new { Message = "Session created successfully." });
        }
        catch (Exception e)
        {
            return BadRequest(new { Message = e.Message });
        }
    }

    [Authorize(Roles = "Admin, Trainer, Client")]
    [HttpGet("sessions")]
    public async Task<IActionResult> GetChatSession([FromQuery] string trainerId, [FromQuery] string clientId)
    {
        var session = await _chatService.GetChatSessionAsync(trainerId, clientId);
        return session != null ? Ok(session) : NotFound(new { Message = "Chat session not found." });
    }

    [Authorize(Roles = "Admin")]   
    [HttpDelete("sessions")]
    public async Task<IActionResult> DeleteChatSession([FromQuery] string trainerId, [FromQuery] string clientId)
    {
        return await _chatService.DeleteChatSessionAsync(trainerId, clientId)
            ? Ok(new { Message = "Session deleted successfully." })
            : NotFound(new { Message = "Session not found or already deleted." });
    }
    
    [Authorize(Roles = "Client")]  
    [HttpPost("sessions/extend")]
    public async Task<IActionResult> ExtendChatSession([FromQuery] string trainerId, [FromQuery] string clientId)
    {
        return await _chatService.ExtendChatSessionAsync(trainerId,clientId)
            ? NoContent()
            : NotFound(new { Message = "Chat session not found." });
    }
    
}