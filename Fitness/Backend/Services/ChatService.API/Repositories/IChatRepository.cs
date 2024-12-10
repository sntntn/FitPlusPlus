using ChatService.API.Models;
using MongoDB.Bson;

namespace ChatService.API.Repositories
{
    public interface IChatRepository
    {
        // rad sa porukama
        Task<IEnumerable<Message>> GetAllMessagesAsync();
        Task<Message?> GetMessageByIdAsync(string id);
        Task CreateMessageAsync(Message message);

        // rad sa chat sesijama
        Task<ChatSession?> GetChatSessionAsync(string trainerId, string clientId);
        Task CreateChatSessionAsync(ChatSession session);
        Task UnlockChatSessionAsync(string sessionId);
        Task AddMessageToChatSessionAsync(string sessionId, Message message);
        Task<IEnumerable<Message>> GetMessagesFromChatSessionAsync(string sessionId);
    }
}
