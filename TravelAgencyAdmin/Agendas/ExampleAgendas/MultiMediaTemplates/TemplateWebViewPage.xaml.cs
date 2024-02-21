using CefSharp;
using EasyITSystemCenter.Classes;
using EasyITSystemCenter.GlobalOperations;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Windows.Controls;

namespace EasyITSystemCenter.Pages {

    /// <summary>
    /// Template Page For internet pages document, pictures, text and and much more file formats
    /// opened in WebViewer
    /// </summary>
    public partial class TemplateWebViewPage : UserControl {

        /// <summary>
        /// Standartized declaring static page data for global vorking with pages
        /// </summary>
        public static DataViewSupport dataViewSupport = new DataViewSupport();

        public static TemplateClassList selectedRecord = new TemplateClassList();

        /// <summary>
        /// Initialize page with loading Dictionary and direct show example file
        /// </summary>
        public TemplateWebViewPage() {
            InitializeComponent();
            _ = SystemOperations.SetLanguageDictionary(Resources, App.appRuntimeData.AppClientSettings.First(a => a.Key == "sys_defaultLanguage").Value);

            try {
                webViewer.BeginInit();
                webViewer.Address = Path.Combine("https://kliknetezde.cz");
                webViewer.FrameLoadStart += WebViewer_FrameLoadStart;
                webViewer.FrameLoadEnd += WebViewer_FrameLoadEnd;
                webViewer.LifeSpanHandler = new MyCustomLifeSpanHandler();
                MainWindow.ProgressRing = System.Windows.Visibility.Visible;
            } catch (Exception ex) { }
            MainWindow.ProgressRing = System.Windows.Visibility.Hidden;
        }

        private void WebViewer_FrameLoadStart(object sender, FrameLoadStartEventArgs e) {
            MainWindow.ProgressRing = System.Windows.Visibility.Visible;
        }

        private void WebViewer_FrameLoadEnd(object sender, FrameLoadEndEventArgs e) {
            MainWindow.ProgressRing = System.Windows.Visibility.Hidden;
        }

        private class MyCustomLifeSpanHandler : ILifeSpanHandler {

            public bool DoClose(IWebBrowser chromiumWebBrowser, IBrowser browser) {
                return true;
            }

            public void OnAfterCreated(IWebBrowser chromiumWebBrowser, IBrowser browser) {
            }

            public void OnBeforeClose(IWebBrowser chromiumWebBrowser, IBrowser browser) {
            }

            public bool OnBeforePopup(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, string targetUrl, string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IPopupFeatures popupFeatures, IWindowInfo windowInfo, IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser) {
                browser.MainFrame.LoadUrl(targetUrl);
                newBrowser = null;
                return true;
            }
        }
    }
}