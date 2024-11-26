using ChatService.API.Models;

namespace ChatService.API.Services;

public interface IChatService
{
    Task<IEnumerable<Message>> GetAllMessagesAsync();
    Task<Message?> GetMessageByIdAsync(int id);
    Task<Message> CreateMessageAsync(string content);
}