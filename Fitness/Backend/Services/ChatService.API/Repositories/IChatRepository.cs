using ChatService.API.Models;
using MongoDB.Bson;

namespace ChatService.API.Repositories
{
    public interface IChatRepository
    {

        Task<IEnumerable<object>> GetBasicInfoForTrainerSessionsAsync(string trainerId);
        Task<ChatSession?> GetChatSessionAsync(string trainerId, string clientId);
        Task CreateChatSessionAsync(ChatSession session);
        Task<bool> DeleteChatSessionAsync(string trainerId, string clientId);

        Task<bool> UnlockChatSessionAsync(string sessionId);
        Task AddMessageToChatSessionAsync(string sessionId, Message message);
        Task<IEnumerable<Message>> GetMessagesFromChatSessionAsync(string sessionId);
    }
}
