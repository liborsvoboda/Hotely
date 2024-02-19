
//Golbal System Logger Window For Develoer Support
function GlobalConsoleLogWindow(startNow) {
    if ((startNow || Metro.storage.getItem('LoggerToolShown', null) == true) && $("#GlobalConsoleLogWindow")[0] == undefined) {
        
        Metro.window.create({
            id: 'GlobalConsoleLogWindow', title: "System Logger",
            width: 350, height: 250, clsWindow: "supertop pos-absolute",
            icon: "<span class='mif-windows'></span>",
            content: "<div id=\"log-container\"><pre id=\"log\"></pre></div>",
            draggable: true, shadow: true, modal: false, place: "top-left",
            onShow: function () { window.scrollTo(0, 0); Metro.storage.setItem('LoggerToolShown', true);}, onClose: function () { Metro.storage.setItem("LoggerToolShown", null) },
            clsCaption: "bg-red", dragArea: "#body",
        });

        rewireLoggingToElement(() => document.getElementById("log"), () => document.getElementById("log-container"), true);
        function rewireLoggingToElement(eleLocator, eleOverflowLocator, autoScroll) {
            fixLoggingFunc('log'); fixLoggingFunc('debug'); fixLoggingFunc('warn'); fixLoggingFunc('error'); fixLoggingFunc('info');

            function fixLoggingFunc(name) {
                console['old' + name] = console[name];
                console[name] = function (...arguments) {
                    const output = produceOutput(name, arguments); const eleLog = eleLocator();

                    if (autoScroll) {
                        const eleContainerLog = eleOverflowLocator();
                        const isScrolledToBottom = eleContainerLog.scrollHeight - eleContainerLog.clientHeight <= eleContainerLog.scrollTop + 1;
                        eleLog.innerHTML += output + "<br>";
                        if (isScrolledToBottom) {
                            eleContainerLog.scrollTop = eleContainerLog.scrollHeight - eleContainerLog.clientHeight;
                        }
                    } else { eleLog.innerHTML += output + "<br>"; }
                    
                    window.onunhandledrejection = event => { 
                    	console.warn(`UNHANDLED PROMISE REJECTION: ${(typeof event == "object" ? JSON.stringify(event) : event.reason) }`);
                    };
                    window.onerror = (error) => { 
                    	console.error(`UNHANDLED ERROR: ${(typeof error == "object" ? JSON.stringify(error) : error.log)  }`); 
                    }; 
                    window.onerror = (event, url, line, column, error) => { 
                    	let msg = ""; msg += error; console.error(error, msg + "\n" + msg + "\n" + url + "\n" + line); 
                    };
                };
            }
            function produceOutput(name, args) { return args.reduce((output, arg) => {
                    return output + "<span class=\"log-" + (typeof arg) + " log-" + name + "\">" + (typeof arg === "object" && (JSON || {}).stringify ? JSON.stringify(arg) : arg) + "</span>&nbsp;";
                }, '');
            }
        }
    } else { Metro.storage.setItem('LoggerToolShown', null); }
}

