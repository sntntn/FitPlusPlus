using ChatService.API.Data;
using ChatService.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatService.API.Services;

public class ChatService : IChatService
{
    private readonly ChatDbContext _context;

    public ChatService(ChatDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Message>> GetAllMessagesAsync()
    {
        return await _context.Messages
            .Select(entity => new Message
            {
                Id = entity.Id,
                Content = entity.Content,
                Timestamp = entity.Timestamp
            })
            .ToListAsync();
    }

    public async Task<Message?> GetMessageByIdAsync(int id)
    {
        var entity = await _context.Messages.FindAsync(id);
        if (entity == null) return null;

        return new Message
        {
            Id = entity.Id,
            Content = entity.Content,
            Timestamp = entity.Timestamp
        };
    }

    public async Task<Message> CreateMessageAsync(string content)
    {
        var entity = new MessageEntity { Content = content, Timestamp = DateTime.UtcNow };
        _context.Messages.Add(entity);
        await _context.SaveChangesAsync();

        return new Message
        {
            Id = entity.Id,
            Content = entity.Content,
            Timestamp = entity.Timestamp
        };
    }
}