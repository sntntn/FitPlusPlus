using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using ChatService.API.Services;

namespace ChatService.API.Middleware;


/// <summary>
/// Middleware component responsible for handling WebSocket connections
/// for real-time communication between trainers and clients.
/// </summary>
/// <remarks>
/// This middleware intercepts requests to the <c>/ws</c> endpoint and establishes
/// a WebSocket connection if the request is valid.  
/// It extracts the <c>trainerId</c> and <c>clientId</c> from the query string,  
/// validates them, and delegates the handling of the connection to
/// the <see cref="WebSocketHandler"/> service.
///
/// If the request is not a WebSocket request, the middleware simply
/// forwards it to the next component in the ASP.NET Core pipeline.
/// </remarks>
public class WebSocketMiddleware
{
    private readonly RequestDelegate _next;
    private readonly WebSocketHandler _webSocketHandler;

    /// <summary>
    /// Initializes a new instance of the <see cref="WebSocketMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next delegate in the middleware pipeline.</param>
    /// <param name="webSocketHandler">The handler responsible for managing WebSocket sessions.</param>
    public WebSocketMiddleware(RequestDelegate next, WebSocketHandler webSocketHandler)
    {
        _next = next;
        _webSocketHandler = webSocketHandler;
    }

    /// <summary>
    /// Invokes the middleware logic for handling incoming HTTP requests.
    /// </summary>
    /// <param name="context">The current HTTP context of the request.</param>
    /// <remarks>
    /// - If the request path starts with <c>/ws</c> and is a WebSocket request,
    ///   the connection is upgraded to a WebSocket.  
    /// - If <c>trainerId</c> or <c>clientId</c> are missing, a <c>400 Bad Request</c> response is returned.  
    /// - Otherwise, the request is passed to the next middleware in the pipeline.
    /// </remarks>
    /// <returns>A task that represents the asynchronous middleware operation.</returns>

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