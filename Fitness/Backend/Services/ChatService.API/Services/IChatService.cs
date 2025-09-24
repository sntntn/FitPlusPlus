using ChatService.API.Models;

namespace ChatService.API.Services;

public interface IChatService
{
    Task<IEnumerable<object>> GetBasicInfoForSessionsAsync(string userId);
    Task AddMessageToSessionAsync(string trainerId, string clientId, string content, string senderType);
    Task<IEnumerable<Message>> GetMessagesFromSessionAsync(string trainerId, string clientId);
    Task CreateChatSessionAsync(string trainerId, string clientId);
    Task<ChatSession?> GetChatSessionAsync(string trainerId, string clientId);
    Task<bool> DeleteChatSessionAsync(string trainerId, string clientId);
    Task<bool> ExtendChatSessionAsync(string trainerId, string clientId);
}
