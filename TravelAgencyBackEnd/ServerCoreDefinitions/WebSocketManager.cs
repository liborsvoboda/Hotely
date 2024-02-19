/*
* Server Core Web Helpers
* System Extensions for Correct Core working
* DataTypes Conversion Support, etc.
*/

using System.Net.WebSockets;
using System.Threading;

namespace UbytkacBackend.ServerCoreStructure {

    /// <summary>
    /// Server Web Helpers for EASY working Here are extension for Database Model, WebSocket
    /// </summary>
    public static class ServerCoreWebHelpers {

        #region WebSocketsCentralControllerMethods Helper

        /// <summary>
        /// Sends the message to client WebSocket. This Is Used by
        /// "SendMessageAndUpdateWebSocketsInSpecificPath" For Update Informaions on All Connections
        /// in Same WebSocket Path Its Solution FOR Teminals, Group Communications, etc.
        /// </summary>
        /// <param name="webSocket">The web socket.</param>
        /// <param name="message">  The message.</param>
        public static async Task SendMessageToClientSocket(WebSocket webSocket, string message) {
            await webSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(message), 0, Encoding.UTF8.GetBytes(message).Length),
                       WebSocketMessageType.Text, true, CancellationToken.None);
        }

        /// <summary>
        /// Register Listening Client WebSocket Communication Ist for Receive messages from Client
        /// </summary>
        /// <param name="webSocket">    </param>
        /// <param name="socketAPIPath"></param>
        /// <returns></returns>
        public static async Task ListenClientWebSocketMessages(WebSocket webSocket, string socketAPIPath) {
            var buffer = new byte[1024 * ServerConfigSettings.WebSocketMaxBufferSizeKb];
            var receiveResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            while (!receiveResult.CloseStatus.HasValue) {
                SendMessageAndUpdateWebSocketsInSpecificPath(socketAPIPath, "");
                receiveResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }

            await webSocket.CloseAsync(receiveResult.CloseStatus.Value, receiveResult.CloseStatusDescription, CancellationToken.None);
        }

        /// <summary>
        /// Add WeSocket Connection To Central List
        /// </summary>
        /// <param name="newWebSocket"> The new web socket.</param>
        /// <param name="socketAPIPath">The socket path.</param>
        public static async void AddSocketConnectionToCentralList(WebSocket newWebSocket, string socketAPIPath) {
            ServerRuntimeData.CentralWebSocketList.Add(new Tuple<WebSocket, WebSocketExtension>(newWebSocket, new WebSocketExtension() {
                socketAPIPath = socketAPIPath,
                SocketTimeout = DateTimeOffset.UtcNow.AddMinutes(ServerConfigSettings.WebSocketTimeoutMin)
            }));

            //welcome message
            await SendMessageToClientSocket(newWebSocket, ServerConfigSettings.ConfigCoreServerRegisteredName + " " + DbOperations.DBTranslate("welcome"));
        }

        /// <summary>
        /// Sends the message and update web sockets in specific path.
        /// </summary>
        /// <param name="socketAPIPath">The socket API path.</param>
        /// <param name="message">      The message.</param>
        public static async void SendMessageAndUpdateWebSocketsInSpecificPath(string socketAPIPath, string message) {
            //clean invalid Sockets before updating
            DisposeSocketConnectionsWithTimeOut();

            foreach (Tuple<WebSocket, WebSocketExtension> socket in ServerRuntimeData.CentralWebSocketList) {
                if (socket.Item2.socketAPIPath == socketAPIPath) {
                    await SendMessageToClientSocket(socket.Item1, message);
                    socket.Item2.SocketTimeout = DateTimeOffset.UtcNow.AddMinutes(ServerConfigSettings.WebSocketTimeoutMin);
                }
            }
        }

        /// <summary>
        /// !! Global Method for Simple Using WebSockets WebSocket Disposed - Cleaning monitored
        /// Sockets from Central List. Are Closed and Disposed Only with Timeout or with closed Connection
        /// </summary>
        public static int DisposeSocketConnectionsWithTimeOut() {
            ServerRuntimeData.CentralWebSocketList.ForEach(socket => {
                if (socket.Item2.SocketTimeout < DateTimeOffset.UtcNow) { socket.Item1.CloseAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None); }
            });
            ServerRuntimeData.CentralWebSocketList.RemoveAll(socket => socket.Item1.State != WebSocketState.Open);
            return ServerRuntimeData.CentralWebSocketList.Count;
        }

        #endregion WebSocketsCentralControllerMethods Helper
    }
}