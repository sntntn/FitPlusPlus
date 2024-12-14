using MongoDB.Driver;
using ChatService.API.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;

namespace ChatService.API.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly IMongoCollection<Message> _messages;
        private readonly IMongoCollection<ChatSession> _chatSessions;

        public ChatRepository(IMongoClient mongoClient, IOptions<MongoDBSettings> settings)
        {
            var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _messages = database.GetCollection<Message>("Messages");
            _chatSessions = database.GetCollection<ChatSession>("ChatSessions");
        }

        // Implementacija metoda za poruke
        public async Task<IEnumerable<Message>> GetAllMessagesAsync()
        {
            return await _messages.Find(Builders<Message>.Filter.Empty).ToListAsync();
        }

        public async Task<Message?> GetMessageByIdAsync(string id)
        {
            return await _messages.Find(message => message.Id == ObjectId.Parse(id)).FirstOrDefaultAsync();
        }


        public async Task CreateMessageAsync(Message message)
        {
            await _messages.InsertOneAsync(message);
        }

        // Implementacija metoda za sesije
        public async Task<ChatSession?> GetChatSessionAsync(string trainerId, string clientId)
        {
            return await _chatSessions
                .Find(session => session.TrainerId == trainerId && session.ClientId == clientId)
                .FirstOrDefaultAsync();
        }

        public async Task CreateChatSessionAsync(ChatSession session)
        {
            if (session.Id != ObjectId.Empty)
            {
                throw new InvalidOperationException("ID should not be provided. It will be automatically generated.");
            }
            
            var existingSession = await _chatSessions
                .Find(s => s.TrainerId == session.TrainerId && s.ClientId == session.ClientId)
                .FirstOrDefaultAsync();

            if (existingSession != null)
            {
                throw new InvalidOperationException("Unable to create. A session between this trainer and client already exists.");
            }
            
            await _chatSessions.InsertOneAsync(session);
        }

        public async Task UnlockChatSessionAsync(string sessionId)
        {
            var filter = Builders<ChatSession>.Filter.Eq(s => s.Id, new ObjectId(sessionId));
            var update = Builders<ChatSession>.Update.Set(s => s.IsUnlocked, true);
            await _chatSessions.UpdateOneAsync(filter, update);
        }

        public async Task AddMessageToChatSessionAsync(string sessionId, Message message)
        {
            var filter = Builders<ChatSession>.Filter.Eq(s => s.Id, new ObjectId(sessionId));
            var update = Builders<ChatSession>.Update.Push(s => s.Messages, message);
            await _chatSessions.UpdateOneAsync(filter, update);
        }

        public async Task<IEnumerable<Message>> GetMessagesFromChatSessionAsync(string sessionId)
        {
            var session = await _chatSessions
                .Find(s => s.Id == ObjectId.Parse(sessionId))
                .FirstOrDefaultAsync();

            return session?.Messages ?? Enumerable.Empty<Message>();
        }
    }
    
}
