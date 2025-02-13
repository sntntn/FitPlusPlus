using ChatService.API.Data;
using MongoDB.Driver;
using ChatService.API.Models;
using Microsoft.Extensions.Options;
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

        public async Task CreateChatSessionAsync(string trainerId, string clientId)
        {
            var existingSession = await _chatSessions
                .Find(s => s.TrainerId == trainerId && s.ClientId == clientId)
                .FirstOrDefaultAsync();

            if (existingSession != null)
            {
                throw new InvalidOperationException("Unable to create. A session between this trainer and client already exists.");
            }
            
            var session = new ChatSession
            {
                TrainerId = trainerId,
                ClientId = clientId,
                IsUnlocked = true,
                ExpirationDate = DateTime.UtcNow.AddDays(30),
                Messages = new List<Message>
                {
                    new Message
                    {
                        Id = ObjectId.GenerateNewId(),
                        Content = "Ćao, ja sam tvoj trener!",
                        Timestamp = DateTime.UtcNow,
                        SenderType = "trainer"
                    },
                    new Message
                    {
                        Id = ObjectId.GenerateNewId(),
                        Content = "Upravo ti je uplaćeno online mentorstvo na 30 dana i za to vreme me možeš pitati bilo šta vezano za vežbanje ili ishranu.",
                        Timestamp = DateTime.UtcNow,
                        SenderType = "trainer"
                    },
                    new Message
                    {
                        Id = ObjectId.GenerateNewId(),
                        Content = "Stojim ti na usluzi! :)",
                        Timestamp = DateTime.UtcNow,
                        SenderType = "trainer"
                    }
                }
            };
            
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
