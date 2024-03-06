class ConsoleRecorder {
    constructor() { this.overwriteConsoleMethod(); }

    overwriteConsoleMethod() {
        const originalConsoleLog = console.log;
        const originalConsoleError = console.error;
        const originalConsoleWarn = console.warn;
        const originalConsoleInfo = console.info;
        const originalConsoleDebug = console.debug;

        console.log = (message) => {
            originalConsoleLog(message);
            //this.callSystemLogger('log', message);
        };

        console.error = (message) => {
            originalConsoleError(message);
            this.callSystemLogger('error', message);
        };

        console.warn = (message) => {
            originalConsoleWarn(message);
            this.callSystemLogger('warn', message);
        };

        console.info = (message) => {
            originalConsoleInfo(message);
            //this.callSystemLogger('info', message);
        };

        console.debug = (message) => {
            originalConsoleDebug(message);
            this.callSystemLogger('debug', message);
        };
    }

    callSystemLogger(type, message) {
    	let logMessage = {"LogLevel":type, "Message": message};
    	SendWebSystemLogMessage(logMessage);
    }
}

