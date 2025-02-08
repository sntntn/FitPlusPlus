using System.Net.WebSockets;
using System.Text;
using System.Collections.Concurrent;

namespace ChatService.API.Services;

public class WebSocketHandler
{
    private static readonly ConcurrentBag<WebSocket> _sockets = new();

    public async Task HandleConnection(WebSocket webSocket)
    {
        _sockets.Add(webSocket);
        var buffer = new byte[1024 * 4];

        try
        {
            while (webSocket.State == WebSocketState.Open)
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    await BroadcastMessage(message);
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    await CloseConnection(webSocket);
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Error] WebSocket exception: {ex.Message}");
        }
        finally
        {
            await CloseConnection(webSocket);
        }
    }

    private async Task CloseConnection(WebSocket webSocket)
    {
        if (webSocket.State == WebSocketState.Open)
        {
            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Connection closed", CancellationToken.None);
        }
        _sockets.TryTake(out _);
    }

    public async Task BroadcastMessage(string message)
    {
        var buffer = Encoding.UTF8.GetBytes(message);
        var segment = new ArraySegment<byte>(buffer);
        var closedSockets = new List<WebSocket>();

        foreach (var socket in _sockets)
        {
            if (socket.State == WebSocketState.Open)
            {
                await socket.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
            }
            else
            {
                closedSockets.Add(socket);
            }
        }

        foreach (var socket in closedSockets)
        {
            _sockets.TryTake(out _);
        }
    }
}
