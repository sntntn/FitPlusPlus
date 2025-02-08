using ChatService.API.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace ChatService.API.Data
{
    public class Context : IContext
    {
        public Context(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetValue<string>("MongoDB:ConnectionString"));
            var database = client.GetDatabase(configuration.GetValue<string>("MongoDB:DatabaseName"));
            
            ChatSessions = database.GetCollection<ChatSession>(configuration.GetValue<string>("MongoDB:ChatSessionsCollection"));
        }

        public IMongoCollection<ChatSession> ChatSessions { get; }
    }
}