namespace ChatService.API.Models;

public class ChatSessionEvent
{
    public string TrainerId { get; set; }
    public string ClientId { get; set; }
    public DateTime Timestamp { get; set; }
}