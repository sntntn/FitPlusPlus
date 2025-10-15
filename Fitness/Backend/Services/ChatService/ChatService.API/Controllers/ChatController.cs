using ChatService.API.Models;
using ChatService.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatService.API.Controllers;

/// <summary>
/// Provides REST API endpoints for managing chat sessions and messages between clients and trainers.
/// </summary>
/// <remarks>
/// This controller exposes operations for:
/// - Creating, retrieving, and deleting chat sessions
/// - Sending and fetching messages between participants
/// - Extending chat sessions
///
/// All routes are secured and require authorization with roles <c>Admin</c>, <c>Trainer</c>, or <c>Client</c>.
/// </remarks>
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

    /// <summary>
    /// Retrieves a summary of all chat sessions for a given user (trainer or client).
    /// </summary>
    /// <param name="userId">The ID of the user whose chat sessions are to be retrieved.</param>
    /// <returns>
    /// A list of basic session information for the specified user.
    /// Returns <c>404 Not Found</c> if no sessions exist.
    /// </returns>
    /// <response code="200">List of chat sessions found.</response>
    /// <response code="404">No chat sessions found for the user.</response>
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

    /// <summary>
    /// Adds a new message to a chat session between a trainer and a client.
    /// </summary>
    /// <param name="trainerId">The ID of the trainer participating in the chat session.</param>
    /// <param name="clientId">The ID of the client participating in the chat session.</param>
    /// <param name="content">The content of the message to be sent.</param>
    /// <param name="senderType">Indicates who sent the message ("Trainer" or "Client").</param>
    /// <returns>
    /// Returns <c>200 OK</c> if the message was added successfully.
    /// Returns <c>400 Bad Request</c> if an error occurred.
    /// </returns>
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
    
    /// <summary>
    /// Retrieves all messages exchanged between a trainer and a client in a specific chat session.
    /// </summary>
    /// <param name="trainerId">The ID of the trainer.</param>
    /// <param name="clientId">The ID of the client.</param>
    /// <returns>
    /// A list of chat messages in the session.
    /// Returns <c>404 Not Found</c> if the session or messages are missing.
    /// </returns>
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

    /// <summary>
    /// Creates a new chat session between a trainer and a client.
    /// </summary>
    /// <param name="trainerId">The ID of the trainer.</param>
    /// <param name="clientId">The ID of the client.</param>
    /// <returns>
    /// Returns <c>200 OK</c> if the session was created successfully.
    /// Returns <c>400 Bad Request</c> if a session already exists or another error occurs.
    /// </returns>
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

    /// <summary>
    /// Retrieves an existing chat session between a trainer and a client.
    /// </summary>
    /// <param name="trainerId">The ID of the trainer.</param>
    /// <param name="clientId">The ID of the client.</param>
    /// <returns>
    /// The chat session object, or <c>404 Not Found</c> if it does not exist.
    /// </returns>
    [Authorize(Roles = "Admin, Trainer, Client")]
    [HttpGet("sessions")]
    public async Task<IActionResult> GetChatSession([FromQuery] string trainerId, [FromQuery] string clientId)
    {
        var session = await _chatService.GetChatSessionAsync(trainerId, clientId);
        return session != null ? Ok(session) : NotFound(new { Message = "Chat session not found." });
    }

    /// <summary>
    /// Deletes an existing chat session (admin-only operation).
    /// </summary>
    /// <param name="trainerId">The ID of the trainer.</param>
    /// <param name="clientId">The ID of the client.</param>
    /// <returns>
    /// Returns <c>200 OK</c> if the session was deleted.
    /// Returns <c>404 Not Found</c> if the session does not exist.
    /// </returns>
    [Authorize(Roles = "Admin")]   
    [HttpDelete("sessions")]
    public async Task<IActionResult> DeleteChatSession([FromQuery] string trainerId, [FromQuery] string clientId)
    {
        return await _chatService.DeleteChatSessionAsync(trainerId, clientId)
            ? Ok(new { Message = "Session deleted successfully." })
            : NotFound(new { Message = "Session not found or already deleted." });
    }
    
    /// <summary>
    /// Extends the duration of an existing chat session (client-only operation).
    /// </summary>
    /// <param name="trainerId">The ID of the trainer.</param>
    /// <param name="clientId">The ID of the client.</param>
    /// <returns>
    /// Returns <c>204 No Content</c> if the session was extended successfully.
    /// Returns <c>404 Not Found</c> if the session does not exist.
    /// </returns>
    [Authorize(Roles = "Client")]  
    [HttpPost("sessions/extend")]
    public async Task<IActionResult> ExtendChatSession([FromQuery] string trainerId, [FromQuery] string clientId)
    {
        return await _chatService.ExtendChatSessionAsync(trainerId,clientId)
            ? NoContent()
            : NotFound(new { Message = "Chat session not found." });
    }
    
}