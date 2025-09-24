using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using ChatService.API.Services;

namespace ChatService.API.Middleware;

public class WebSocketMiddleware
{
    private readonly RequestDelegate _next;
    private readonly WebSocketHandler _webSocketHandler;

    public WebSocketMiddleware(RequestDelegate next, WebSocketHandler webSocketHandler)
    {
        _next = next;
        _webSocketHandler = webSocketHandler;
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/ws") && context.WebSockets.IsWebSocketRequest)
        {
            var trainerId = context.Request.Query["trainerId"];
            var clientId = context.Request.Query["clientId"];

            if (string.IsNullOrEmpty(trainerId) || string.IsNullOrEmpty(clientId))
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Missing trainerId or clientId");
                return;
            }

            var webSocket = await context.WebSockets.AcceptWebSocketAsync();
            await _webSocketHandler.HandleConnection(webSocket, trainerId, clientId);
        }
        else
        {
            await _next(context);
        }
    }
}