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
        Task<bool> ExtendChatSessionAsync(string sessionId);
        Task AddMessageToChatSessionAsync(string sessionId, Message message);
        Task updateChatSessionStatusAsync(ChatSession session);
        
    }
}
