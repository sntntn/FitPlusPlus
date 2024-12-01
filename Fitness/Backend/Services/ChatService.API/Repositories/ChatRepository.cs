using ChatService.API.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ChatService.API.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly IMongoCollection<Message> _messages;

        public ChatRepository(IMongoClient mongoClient, IOptions<MongoDBSettings> settings)
        {
            var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _messages = database.GetCollection<Message>("Messages"); // "Messages" je ime kolekcije
        }

        public async Task<IEnumerable<Message>> GetAllMessagesAsync()
        {
            return await _messages.Find(message => true).ToListAsync(); // Vrati sve poruke
        }

        public async Task<Message> GetMessageByIdAsync(string id)
        {
            return await _messages.Find(message => message.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateMessageAsync(Message message)
        {
            await _messages.InsertOneAsync(message); // Ubaci novu poruku
        }
    }
}