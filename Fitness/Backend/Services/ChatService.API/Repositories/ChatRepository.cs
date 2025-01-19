using MongoDB.Driver;
using ChatService.API.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;

namespace ChatService.API.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly IMongoCollection<ChatSession> _chatSessions;

        public ChatRepository(IMongoClient mongoClient, IOptions<MongoDBSettings> settings)
        {
            var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _chatSessions = database.GetCollection<ChatSession>("ChatSessions");
        }
        
        public async Task<IEnumerable<object>> GetBasicInfoForSessionsAsync(string userId)
        {
            var filter = Builders<ChatSession>.Filter.Eq(s => s.TrainerId, userId);

            // Projekcija tkd izdvajamo samo potrebna polja
            var projection = Builders<ChatSession>.Projection.Expression(s => new
            {
                TrainerId = s.TrainerId,
                ClientId = s.ClientId,
                IsUnlocked = s.IsUnlocked,
                ExpirationDate = s.ExpirationDate
            });

            return await _chatSessions.Find(filter).Project(projection).ToListAsync();
        }

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

        public async Task<bool> DeleteChatSessionAsync(string trainerId, string clientId)
        {
            var filter = Builders<ChatSession>.Filter.Eq(s => s.TrainerId, trainerId) &
                         Builders<ChatSession>.Filter.Eq(s => s.ClientId, clientId);

            var result = await _chatSessions.DeleteOneAsync(filter);

            return result.DeletedCount > 0;
        }

        public async Task<bool> UnlockChatSessionAsync(string sessionId)
        {
            var filter = Builders<ChatSession>.Filter.Eq(s => s.Id, new ObjectId(sessionId));
            var update = Builders<ChatSession>.Update
                .Set(s => s.IsUnlocked, true)
                .Set(s => s.ExpirationDate, DateTime.UtcNow.AddDays(30));
            
            var result = await _chatSessions.UpdateOneAsync(filter, update);
            
            return result.ModifiedCount > 0;
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
