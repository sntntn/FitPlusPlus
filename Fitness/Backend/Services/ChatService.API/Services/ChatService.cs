using System.Text.Json;
using ChatService.API.Models;
using ChatService.API.Publishers;
using ChatService.API.Repositories;

namespace ChatService.API.Services;

public class ChatService: IChatService
{
    private readonly IChatRepository _chatRepository;
    private readonly WebSocketHandler _webSocketHandler;
    private readonly INotificationPublisher _notificationPublisher;

    public ChatService(IChatRepository chatRepository, WebSocketHandler webSocketHandler, INotificationPublisher notificationPublisher)
    {
        _chatRepository = chatRepository;
        _webSocketHandler = webSocketHandler;
        _notificationPublisher = notificationPublisher;
    }

    public async Task<IEnumerable<object>> GetBasicInfoForSessionsAsync(string userId)
    {
        return await _chatRepository.GetBasicInfoForSessionsAsync(userId);
    }

    public async Task AddMessageToSessionAsync(string trainerId, string clientId, string content, string senderType)
    {
        if (senderType != "trainer" && senderType != "client")
        {
            throw new ArgumentException("Invalid sender type. Must be either 'trainer' or 'client'.");
        }

        var session = await _chatRepository.GetChatSessionAsync(trainerId, clientId);

        if (session == null)
        {
            throw new InvalidOperationException("Chat session not found.");
        }

        if (session.ExpirationDate.HasValue && session.ExpirationDate.Value < DateTime.UtcNow)
        {
            throw new InvalidOperationException("Chat session has expired. Please pay if you want to send a message again.");
        }

        var newMessage = new Message
        {
            Content = content,
            Timestamp = DateTime.UtcNow,
            SenderType = senderType
        };

        var sessionKey = _webSocketHandler.GetSessionKey(trainerId, clientId);

        await Task.WhenAll(
            _webSocketHandler.BroadcastMessage(sessionKey, JsonSerializer.Serialize(newMessage)),
            _chatRepository.AddMessageToChatSessionAsync(session.Id.ToString(), newMessage)
        );
    }

    public async Task<IEnumerable<Message>> GetMessagesFromSessionAsync(string trainerId, string clientId)
    {
        var session = await _chatRepository.GetChatSessionAsync(trainerId, clientId);
        return session?.Messages ?? throw new InvalidOperationException("No messages found.");
    }

    public async Task CreateChatSessionAsync(string trainerId, string clientId)
    {
        await _chatRepository.CreateChatSessionAsync(trainerId, clientId);

        await Task.WhenAll(
            _notificationPublisher.PublishNotification("Chat Session Created", "New chat session created!", "Information", true, clientId, "Client"),
            _notificationPublisher.PublishNotification("Chat Session Created", "New chat session created!", "Information", true, trainerId, "Trainer")
        );    
    }

    public async Task<ChatSession?> GetChatSessionAsync(string trainerId, string clientId)
    {
        return await _chatRepository.GetChatSessionAsync(trainerId, clientId);
    }

    public async Task<bool> DeleteChatSessionAsync(string trainerId, string clientId)
    {
        return await _chatRepository.DeleteChatSessionAsync(trainerId, clientId);
    }

    public async Task<bool> UnlockChatSessionAsync(string sessionId)
    {
        return await _chatRepository.UnlockChatSessionAsync(sessionId);
    }
}