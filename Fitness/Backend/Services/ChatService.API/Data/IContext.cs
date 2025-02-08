using ChatService.API.Models;
using MongoDB.Driver;

namespace ChatService.API.Data
{
    public interface IContext
    {
        IMongoCollection<ChatSession> ChatSessions { get; }
    }
}