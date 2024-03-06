using EasyITSystemCenter.Classes;
using EasyITSystemCenter.GlobalOperations;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CefSharp;


namespace EasyITSystemCenter.Pages {

    public partial class WebModulePage : UserControl {
        public static DataViewSupport dataViewSupport = new DataViewSupport();
        public static BasicCalendarList selectedRecord = new BasicCalendarList();

        private event EventHandler ShowWebModuleChanged = delegate { };
        private SystemModuleList showWebModule = null;
        public SystemModuleList ShowWebModule {
            get => showWebModule;
            set {
                showWebModule = value;
                ShowWebModuleInBrowser();
                ShowWebModuleChanged?.Invoke(null, EventArgs.Empty);
            }
        }
        
        public WebModulePage() {

            InitializeComponent();
            _ = SystemOperations.SetLanguageDictionary(Resources, App.appRuntimeData.AppClientSettings.First(a => a.Key == "sys_defaultLanguage").Value);

            MainWindow.ProgressRing = Visibility.Visible;

            try {
                if (!App.appRuntimeData.webServerRunning) {
                    _ = MainWindow.ShowMessageOnMainWindow(true, DBOperations.DBTranslation("WebServerNotRunningCheckClientConfiguration").GetAwaiter().GetResult());
                    var test = App.Current;
                    //(this as Canvas).Children.Remove(this);
                }

            } catch (Exception ex) { App.ApplicationLogging(ex); }

            MainWindow.ProgressRing = Visibility.Hidden;
        }

        private async void ShowWebModuleInBrowser() {
            if (App.appRuntimeData.webServerRunning) {
                //Init WebBrowser
                webBrowser.BeginInit();
                webBrowser.Address = App.appRuntimeData.AppClientSettings.First(a => a.Key == "sys_localWebServerUrl").Value + showWebModule.FolderPath;
                webBrowser.FrameLoadStart += webBrowser_FrameLoadStart;
                webBrowser.FrameLoadEnd += webBrowser_FrameLoadEnd;
                webBrowser.LifeSpanHandler = new MyCustomLifeSpanHandler();

            }
            else { await MainWindow.ShowMessageOnMainWindow(true, await DBOperations.DBTranslation("WebServerNotRunningCheckClientConfiguration")); }
        }


        private void webBrowser_FrameLoadStart(object sender, FrameLoadStartEventArgs e) => MainWindow.ProgressRing = Visibility.Visible;
        private void webBrowser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e) => MainWindow.ProgressRing = Visibility.Hidden;

        private class MyCustomLifeSpanHandler : ILifeSpanHandler {

            public bool DoClose(IWebBrowser chromiumWebBrowser, IBrowser browser) { return true; }
            public void OnAfterCreated(IWebBrowser chromiumWebBrowser, IBrowser browser) { }
            public void OnBeforeClose(IWebBrowser chromiumWebBrowser, IBrowser browser) { }

            public bool OnBeforePopup(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, string targetUrl, string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IPopupFeatures popupFeatures, IWindowInfo windowInfo, IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser) {
                browser.MainFrame.LoadUrl(targetUrl); newBrowser = null; return true;
            }
        }
    }
}