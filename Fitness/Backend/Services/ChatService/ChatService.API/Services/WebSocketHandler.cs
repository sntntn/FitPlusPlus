using System.Net.WebSockets;
using System.Text;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ChatService.API.Services;

/// <summary>
/// Handles WebSocket connections and message exchange between clients and trainers.
/// Maintains active sessions, broadcasts messages within each session, and manages disconnections.
/// </summary>
public class WebSocketHandler
{
    private static readonly ConcurrentDictionary<string, List<WebSocket>> _sessions = new();

    /// <summary>
    /// Handles a new WebSocket connection between a trainer and a client.
    /// Listens for incoming messages and broadcasts them to all participants in the same session.
    /// </summary>
    /// <param name="webSocket">The connected WebSocket instance.</param>
    /// <param name="trainerId">The unique ID of the trainer.</param>
    /// <param name="clientId">The unique ID of the client.</param>
    public async Task HandleConnection(WebSocket webSocket, string trainerId, string clientId)
    {
        var sessionKey = GetSessionKey(trainerId, clientId);

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

    /// <summary>
    /// Closes a WebSocket connection and removes it from the session list.
    /// </summary>
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

    /// <summary>
    /// Sends a message to all WebSocket connections in the specified session.
    /// </summary>
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

    /// <summary>
    /// Generates a unique session key for a trainer-client pair.
    /// </summary>
    public string GetSessionKey(string trainerId, string clientId)
    {
        return $"{trainerId}:{clientId}";
    }
}
