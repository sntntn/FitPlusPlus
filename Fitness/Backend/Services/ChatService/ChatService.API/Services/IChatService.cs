using ChatService.API.Models;

namespace ChatService.API.Services;

/// <summary>
/// Defines the contract for managing chat sessions and messages
/// between trainers and clients.
/// </summary>
public interface IChatService
{
    /// <summary>
    /// Retrieves a summary of chat sessions for the specified user (trainer or client).
    /// </summary>
    /// <param name="userId">Unique identifier of the user.</param>
    /// <returns>Collection of objects containing basic chat session information.</returns>
    Task<IEnumerable<object>> GetBasicInfoForSessionsAsync(string userId);
    /// <summary>
    /// Adds a new message to an existing chat session.
    /// </summary>
    /// <param name="trainerId">Identifier of the trainer.</param>
    /// <param name="clientId">Identifier of the client.</param>
    /// <param name="content">Text content of the message.</param>
    /// <param name="senderType">Sender type ("trainer" or "client").</param>
    /// <exception cref="ArgumentException">Thrown if senderType is invalid.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the session does not exist or has expired.</exception>
    Task AddMessageToSessionAsync(string trainerId, string clientId, string content, string senderType);
    /// <summary>
    /// Retrieves all messages from a specific chat session.
    /// </summary>
    /// <param name="trainerId">Identifier of the trainer.</param>
    /// <param name="clientId">Identifier of the client.</param>
    /// <returns>Collection of messages exchanged in the chat session.</returns>
    /// <exception cref="InvalidOperationException">Thrown if no messages are found.</exception>
    Task<IEnumerable<Message>> GetMessagesFromSessionAsync(string trainerId, string clientId);
    /// <summary>
    /// Creates a new chat session between a trainer and a client.
    /// </summary>
    /// <param name="trainerId">Identifier of the trainer.</param>
    /// <param name="clientId">Identifier of the client.</param>
    /// <exception cref="InvalidOperationException">Thrown if a session already exists.</exception>
    Task CreateChatSessionAsync(string trainerId, string clientId);
    /// <summary>
    /// Retrieves the chat session for a specific trainer and client.
    /// </summary>
    /// <param name="trainerId">Identifier of the trainer.</param>
    /// <param name="clientId">Identifier of the client.</param>
    /// <returns>The chat session if found; otherwise null.</returns>
    Task<ChatSession?> GetChatSessionAsync(string trainerId, string clientId);
    /// <summary>
    /// Deletes the chat session for a specific trainer and client.
    /// </summary>
    /// <param name="trainerId">Identifier of the trainer.</param>
    /// <param name="clientId">Identifier of the client.</param>
    /// <returns>True if deletion was successful; otherwise false.</returns>
    Task<bool> DeleteChatSessionAsync(string trainerId, string clientId);
    /// <summary>
    /// Extends the validity of an existing chat session.
    /// </summary>
    /// <param name="trainerId">Identifier of the trainer.</param>
    /// <param name="clientId">Identifier of the client.</param>
    /// <returns>True if the session was successfully extended; otherwise false.</returns>
    Task<bool> ExtendChatSessionAsync(string trainerId, string clientId);
}
