using System.Diagnostics.CodeAnalysis;

namespace UbytkacBackend.ServerCoreStructure {

    /// <summary>
    /// !!! Implemented Server Core WebSocket System LogProvider Stream This Is Special Serice For
    /// Remote Monitoring On More Devices in OneTime
    /// </summary>
    /// <seealso cref="ILoggerProvider"/>
    public class WebSocketLogProvider : ILoggerProvider {
        public WebSocketLogger webSocketLogger;

        private bool disposedValue;

        public ILogger CreateLogger(string categoryName) {
            return webSocketLogger = new WebSocketLogger(this);
        }

        protected virtual void Dispose(bool disposing) {
            if (ServerCoreWebHelpers.DisposeSocketConnectionsWithTimeOut() == 0) { disposedValue = true; }
        }

        public void Dispose() {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

    /// <summary>
    /// Server Core WebSocket System Logger Interface
    /// </summary>
    /// <seealso cref="ILogger"/>
    public class WebSocketLogger : ILogger {
        private readonly WebSocketLogProvider _webSocketLogProvider;

        public WebSocketLogger([NotNull] WebSocketLogProvider webSocketLogProvider) {
            _webSocketLogProvider = webSocketLogProvider;
        }

        public IDisposable? BeginScope<TState>(TState state) {
            _webSocketLogProvider.Dispose(); return null;
        }

        public bool IsEnabled(LogLevel logLevel) {
            return logLevel != LogLevel.None;
        }

        /// <summary>
        /// Used to log the entry.
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="logLevel"> An instance of <see cref="LogLevel"/>.</param>
        /// <param name="eventId">  The event's ID. An instance of <see cref="EventId"/>.</param>
        /// <param name="state">    The event's state.</param>
        /// <param name="exception">The event's exception. An instance of <see cref="Exception"/></param>
        /// <param name="formatter">A delegate that formats</param>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception, string> formatter) {
            try {
                if (ServerRuntimeData.ServerCoreStatus == ServerStatusTypes.Running.ToString()) {
                    string? json = "";
                    if (IsEnabled(logLevel)) {
                        json = JsonSerializer.Serialize(logLevel.ToString()) + JsonSerializer.Serialize(eventId) + JsonSerializer.Serialize(state) + JsonSerializer.Serialize(exception);
                        if (!string.IsNullOrWhiteSpace(json)) { ServerCoreWebHelpers.SendMessageAndUpdateWebSocketsInSpecificPath("ServerCoreMonitor", json); }
                    }
                }
            } catch { _webSocketLogProvider.Dispose(); }
        }
    }
}