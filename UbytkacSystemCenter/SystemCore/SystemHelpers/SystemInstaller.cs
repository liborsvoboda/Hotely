using EasyITSystemCenter.GlobalOperations;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Web.WebView2.Core;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;


namespace EasyITSystemCenter.SystemHelper {

    /// <summary>
    /// Webvirew2 Add Installer
    /// </summary>
    public class WebView2AutoInstaller {

        /// <summary>
        /// Check WebView2 runtime is installed and download then install it.
        /// </summary>
        /// <returns></returns>
        public static async Task<bool> CheckAndInstallWebView() {
            try {
                var version = CoreWebView2Environment.GetAvailableBrowserVersionString();
                if (!string.IsNullOrEmpty(version))
                    return true;
            } catch (WebView2RuntimeNotFoundException) {}

            MessageDialogResult result = await MainWindow.ShowMessageOnMainWindow(false, await DBOperations.DBTranslation("WebViewNotInstalledInstallItQuestion"), true);
            if (result == MessageDialogResult.Affirmative) { InstallWebView(); }
            return true;
        }


        private async static void InstallWebView() {
            try {
                var process = Process.Start(new ProcessStartInfo() {
                    FileName = Path.Combine(App.appRuntimeData.startupPath, "Data", "Install", "MicrosoftEdgeWebview2Setup.exe"),
                    Arguments = $"/install", Verb = "runas", UseShellExecute = true
                });

                process.WaitForExit();
            } catch (Exception e) {
                await MainWindow.ShowMessageOnMainWindow(true, await DBOperations.DBTranslation("WebViewCannotBeInstalledCheckRights"), true);
            }
        }
    }
}
