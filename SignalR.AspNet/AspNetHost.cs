﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using SignalR.Abstractions;

namespace SignalR.AspNet
{
    public class AspNetHost : HttpTaskAsyncHandler
    {
        private readonly PersistentConnection _connection;

        private static readonly Lazy<bool> _hasAcceptWebSocketRequest =
            new Lazy<bool>(() =>
            {
                return typeof(HttpContextBase).GetMethods().Any(m => m.Name.Equals("AcceptWebSocketRequest", StringComparison.OrdinalIgnoreCase));
            });

        public AspNetHost(PersistentConnection connection)
        {
            _connection = connection;
        }

        public override Task ProcessRequestAsync(HttpContextBase context)
        {
            var request = new AspNetRequest(context.Request);
            var response = new AspNetResponse(context.Request, context.Response);
            var hostContext = new HostContext(request, response, context.User);

            // Determine if the client should bother to try a websocket request
            hostContext.Items["supportsWebSockets"] = _hasAcceptWebSocketRequest.Value;

            // Set the debugging flag
            hostContext.Items["debugMode"] = context.IsDebuggingEnabled;

            // Stick the context in here so transports or other asp.net specific logic can
            // grab at it.
            hostContext.Items["aspnet.context"] = context;

            return _connection.ProcessRequestAsync(hostContext);
        }
    }
}
