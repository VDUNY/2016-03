using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.WebSockets;

namespace WebSocketServer.Controllers
{
    public class EchoController : ApiController
    {
        // We just need to implement the GET, and switch protocols:
        public HttpResponseMessage Get()
        {
            if (HttpContext.Current.IsWebSocketRequest)
            {
                // specify the handler for WebSocket requests
                HttpContext.Current.AcceptWebSocketRequest(ProcessChat);
            }

            // Upgrade from HTTP to WebSockets ;-)
            return new HttpResponseMessage(HttpStatusCode.SwitchingProtocols);
        }

        private async Task ProcessChat(AspNetWebSocketContext context)
        {
            // The context gives us the WebSocket we need.
            WebSocket socket = context.WebSocket;

            // And then we just, you know, sit around listening to it :-P
            while (true)
            {
                // eat some bytes
                ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[1024]);
                WebSocketReceiveResult result = await socket.ReceiveAsync(buffer, CancellationToken.None);

                // if the socket is closed, we're done
                if (socket.State != WebSocketState.Open) break;

                // we parse received data
                string message = Encoding.UTF8.GetString(buffer.Array, 0, result.Count);

                // ... do something with it ...

                // format and send data
                buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(message));
                await socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }
}
