using ChatService.API.Models;
using MongoDB.Bson;

namespace ChatService.API.Repositories
{
    public interface IChatRepository
    {

        Task<IEnumerable<object>> GetBasicInfoForSessionsAsync(string userId);
        Task<ChatSession?> GetChatSessionAsync(string trainerId, string clientId);
        Task InsertChatSessionAsync(ChatSession session);
        Task<bool> DeleteChatSessionAsync(string trainerId, string clientId);

        Task<bool> UnlockChatSessionAsync(string sessionId);
        Task AddMessageToChatSessionAsync(string sessionId, Message message);
        Task<IEnumerable<Message>> GetMessagesFromChatSessionAsync(string sessionId);
    }
}
