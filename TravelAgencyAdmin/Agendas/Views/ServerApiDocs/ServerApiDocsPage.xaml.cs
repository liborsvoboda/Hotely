using CefSharp;
using CefSharp.Wpf;
using Newtonsoft.Json;
using System.Windows;
using System.Windows.Controls;
using TravelAgencyAdmin.Classes;
using TravelAgencyAdmin.GlobalOperations;

//using CefSharp;
//using CefSharp.Wpf;

namespace TravelAgencyAdmin.Pages {

    public partial class ServerApiDocsPage : UserControl {
        public static DataViewSupport dataViewSupport = new DataViewSupport();
        public static TemplateClassList selectedRecord = new TemplateClassList();

        public static CefSettings CeffSettings = new CefSettings() { LogSeverity = LogSeverity.Disable, IgnoreCertificateErrors = true };

        public ServerApiDocsPage() {
            InitializeComponent();
            _ = SystemOperations.SetLanguageDictionary(Resources, JsonConvert.DeserializeObject<Language>(App.Setting.DefaultLanguage).Value);

            webViewer.Address = "http://nomad.ubytkac.cz:5000/AdminApiDocs";
            webViewer.FrameLoadStart += WebViewer_FrameLoadStart;
            webViewer.FrameLoadEnd += WebViewer_FrameLoadEnd;
            webViewer.LifeSpanHandler = new MyCustomLifeSpanHandler();

            if (!Cef.IsInitialized) { Cef.Initialize(CeffSettings, performDependencyCheck: true, browserProcessHandler: null); }

            CefSharpSettings.ShutdownOnExit = true;
            CefSharpSettings.SubprocessExitIfParentProcessClosed = true;
        }

        private void WebViewer_FrameLoadStart(object sender, FrameLoadStartEventArgs e) {
            MainWindow.ProgressRing = Visibility.Visible;
        }

        private void WebViewer_FrameLoadEnd(object sender, FrameLoadEndEventArgs e) {
            MainWindow.ProgressRing = Visibility.Hidden;
        }

        private class MyCustomLifeSpanHandler : ILifeSpanHandler {

            public bool DoClose(IWebBrowser chromiumWebBrowser, IBrowser browser) {
                return true;
            }

            public void OnAfterCreated(IWebBrowser chromiumWebBrowser, IBrowser browser) {
            }

            public void OnBeforeClose(IWebBrowser chromiumWebBrowser, IBrowser browser) {
            }

            /// <summary>
            /// Block open New Solo Window Frame as popup
            /// </summary>
            /// <param name="chromiumWebBrowser"></param>
            /// <param name="browser">           </param>
            /// <param name="frame">             </param>
            /// <param name="targetUrl">         </param>
            /// <param name="targetFrameName">   </param>
            /// <param name="targetDisposition"> </param>
            /// <param name="userGesture">       </param>
            /// <param name="popupFeatures">     </param>
            /// <param name="windowInfo">        </param>
            /// <param name="browserSettings">   </param>
            /// <param name="noJavascriptAccess"></param>
            /// <param name="newBrowser">        </param>
            /// <returns></returns>
            public bool OnBeforePopup(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, string targetUrl, string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IPopupFeatures popupFeatures, IWindowInfo windowInfo, IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser) {
                browser.MainFrame.LoadUrl(targetUrl);
                newBrowser = null;
                return true;
            }
        }
    }
}