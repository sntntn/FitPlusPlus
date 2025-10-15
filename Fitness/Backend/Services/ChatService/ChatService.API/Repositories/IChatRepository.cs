using ChatService.API.Models;
using MongoDB.Bson;

/// <summary>
/// Defines the data access operations for chat sessions and messages
/// stored in MongoDB.
/// </summary>
namespace ChatService.API.Repositories
{
    /// <summary>
    /// Retrieves basic information about all chat sessions
    /// associated with the specified user (as trainer or client).
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>
    /// A collection of objects containing summary data for each chat session,
    /// including trainer, client, expiration date, and unlock status.
    /// </returns>
    public interface IChatRepository
    {
        /// <summary>
        /// Retrieves basic information about all chat sessions
        /// associated with the specified user (as trainer or client).
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>
        /// A collection of objects containing summary data for each chat session,
        /// including trainer, client, expiration date, and unlock status.
        /// </returns>
        Task<IEnumerable<object>> GetBasicInfoForSessionsAsync(string userId);
        /// <summary>
        /// Retrieves the chat session between a given trainer and client.
        /// </summary>
        /// <param name="trainerId">Identifier of the trainer.</param>
        /// <param name="clientId">Identifier of the client.</param>
        /// <returns>
        /// The chat session if found; otherwise, <c>null</c>.
        /// </returns>
        Task<ChatSession?> GetChatSessionAsync(string trainerId, string clientId);
        /// <summary>
        /// Inserts a new chat session document into the MongoDB collection.
        /// </summary>
        /// <param name="session">The chat session entity to insert.</param>
        Task InsertChatSessionAsync(ChatSession session);
        /// <summary>
        /// Deletes the chat session for the given trainer and client.
        /// </summary>
        /// <param name="trainerId">Identifier of the trainer.</param>
        /// <param name="clientId">Identifier of the client.</param>
        /// <returns>
        /// <c>true</c> if the session was successfully deleted;
        /// otherwise, <c>false</c>.
        /// </returns>
        Task<bool> DeleteChatSessionAsync(string trainerId, string clientId);
        /// <summary>
        /// Extends the expiration date of a chat session by 30 days
        /// and ensures it remains unlocked.
        /// </summary>
        /// <param name="sessionId">The unique identifier of the chat session.</param>
        /// <returns>
        /// <c>true</c> if the session was successfully extended;
        /// otherwise, <c>false</c>.
        /// </returns>
        Task<bool> ExtendChatSessionAsync(string sessionId);
        /// <summary>
        /// Adds a new message to the message list of an existing chat session.
        /// </summary>
        /// <param name="sessionId">The unique identifier of the chat session.</param>
        /// <param name="message">The message object to add.</param>
        Task AddMessageToChatSessionAsync(string sessionId, Message message);
        /// <summary>
        /// Updates the unlocked status of an existing chat session.
        /// </summary>
        /// <param name="session">The chat session entity with updated status.</param>
        Task updateChatSessionStatusAsync(ChatSession session);
        
    }
}
