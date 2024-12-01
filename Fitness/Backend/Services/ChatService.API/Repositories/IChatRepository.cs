using ChatService.API.Models;
using MongoDB.Bson;

public interface IChatRepository
{
    Task<IEnumerable<Message>> GetAllMessagesAsync();
    Task<Message> GetMessageByIdAsync(string id);
    Task CreateMessageAsync(Message message);
}