using ChatService.API.Data;
using MongoDB.Driver;
using ChatService.API.Models;
using MongoDB.Bson;

namespace ChatService.API.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly IMongoCollection<ChatSession> _chatSessions;

        public ChatRepository(IContext context)
        {
            _chatSessions = context.ChatSessions;
        }
        
        public async Task<IEnumerable<object>> GetBasicInfoForSessionsAsync(string userId)
        {
            var filter = Builders<ChatSession>.Filter.Or(
                Builders<ChatSession>.Filter.Eq(s => s.TrainerId, userId),
                Builders<ChatSession>.Filter.Eq(s => s.ClientId, userId)  
            );
            
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

        public async Task InsertChatSessionAsync(ChatSession session)
        {
            await _chatSessions.InsertOneAsync(session);
        }
        
        public async Task<bool> DeleteChatSessionAsync(string trainerId, string clientId)
        {
            var filter = Builders<ChatSession>.Filter.Eq(s => s.TrainerId, trainerId) &
                         Builders<ChatSession>.Filter.Eq(s => s.ClientId, clientId);

            var result = await _chatSessions.DeleteOneAsync(filter);

            return result.DeletedCount > 0;
        }

        public async Task<bool> ExtendChatSessionAsync(string sessionId)
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

        public async Task updateChatSessionStatusAsync(ChatSession session)
        {
            var filter = Builders<ChatSession>.Filter.Eq(s => s.Id, session.Id);
            var update = Builders<ChatSession>.Update.Set(s => s.IsUnlocked, session.IsUnlocked);
            await _chatSessions.UpdateOneAsync(filter, update);
        }
    }
    
}
