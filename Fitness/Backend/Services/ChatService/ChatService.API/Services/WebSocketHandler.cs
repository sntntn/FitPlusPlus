using System.Net.WebSockets;
using System.Text;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ChatService.API.Services;

public class WebSocketHandler
{
    private static readonly ConcurrentDictionary<string, List<WebSocket>> _sessions = new();

    public async Task HandleConnection(WebSocket webSocket, string trainerId, string clientId)
    {
        var sessionKey = GetSessionKey(trainerId, clientId);

        // ili pravimo novu listu ili samo dodajemo konekciju u postojecu
        _sessions.AddOrUpdate(sessionKey,
            _ => new List<WebSocket> { webSocket },
            (_, sockets) =>
            {
                lock (sockets)
                {
                    sockets.Add(webSocket);
                }
                return sockets;
            });

        var buffer = new byte[1024 * 4];

        try
        {
            while (webSocket.State == WebSocketState.Open)
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    await BroadcastMessage(sessionKey, message);
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    await CloseConnection(webSocket, sessionKey);
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
            await CloseConnection(webSocket, sessionKey);
        }
    }

    private async Task CloseConnection(WebSocket webSocket, string sessionKey)
    {
        if (_sessions.TryGetValue(sessionKey, out var sockets))
        {
            lock (sockets)
            {
                sockets.Remove(webSocket);
            }
        }

        if (webSocket.State == WebSocketState.Open)
        {
            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Connection closed", CancellationToken.None);
        }

        if (_sessions.TryGetValue(sessionKey, out var remainingSockets) && remainingSockets.Count == 0)
        {
            _sessions.TryRemove(sessionKey, out _);
        }
    }

    public async Task BroadcastMessage(string sessionKey, string message)
    {
        var buffer = Encoding.UTF8.GetBytes(message);
        var segment = new ArraySegment<byte>(buffer);

        if (_sessions.TryGetValue(sessionKey, out var sockets))
        {
            lock (sockets)
            {
                sockets = new List<WebSocket>(sockets); // pravim kopiju liste da bih izbegao modifikaciju tokom iteracije
            }

            foreach (var socket in sockets)
            {
                if (socket.State == WebSocketState.Open)
                {
                    await socket.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }
    }

    public string GetSessionKey(string trainerId, string clientId)
    {
        return $"{trainerId}:{clientId}";
    }
}
