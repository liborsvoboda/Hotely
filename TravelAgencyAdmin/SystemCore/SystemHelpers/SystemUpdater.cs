using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace TravelAgencyAdmin.Helper {

    public class SystemUpdater {

        public static async void CheckUpdate(bool directUpdate) {
            MetroWindow metroWindow = Application.Current.MainWindow as MetroWindow;

            if (string.IsNullOrWhiteSpace(App.Setting.UpdateUrl)) { await MainWindow.ShowMessage(false, metroWindow.Resources["updateUrlNotSet"].ToString()); }
            Tuple<Version, string> updateResult = await LastUpdate();

            if (string.IsNullOrWhiteSpace(updateResult.Item2) && directUpdate) { await metroWindow.ShowMessageAsync(metroWindow.Resources["info"].ToString(), metroWindow.Resources["noneUpdateAvailable"].ToString(), MessageDialogStyle.Affirmative); } else if (updateResult.Item2 == "update") {
                if (App.Setting.AutomaticUpdate == "showInfo" || directUpdate) {
                    MetroDialogSettings settings = new MetroDialogSettings() { AffirmativeButtonText = metroWindow.Resources["downoladAndInstall"].ToString(), NegativeButtonText = metroWindow.Resources["ok"].ToString() };
                    MessageDialogResult result = await metroWindow.ShowMessageAsync(metroWindow.Resources["info"].ToString(),
                        metroWindow.Resources["actualVersion"].ToString() + ": " + App.AppVersion.ToString() + Environment.NewLine +
                        metroWindow.Resources["newVersionisAvaiable"].ToString() + ": " + updateResult.Item1.ToString(), MessageDialogStyle.AffirmativeAndNegative, settings);
                    if (result == MessageDialogResult.Affirmative) { DownloadUpdate(updateResult.Item1.ToString(), true); } else { MainWindow.ProgressRing = Visibility.Hidden; }
                } else if (App.Setting.AutomaticUpdate == "automaticDownload") { DownloadUpdate(updateResult.Item1.ToString(), false); } else if (App.Setting.AutomaticUpdate == "automaticInstall") { DownloadUpdate(updateResult.Item1.ToString(), true); }
            }
        }

        private static async Task<Tuple<Version, string>> LastUpdate() {
            try {
                HttpClient httpClient = new HttpClient();
                Stream resultStream = await httpClient.GetStreamAsync(App.Setting.UpdateUrl);

                string theLine = null;
                Version updateVersion = App.AppVersion;

                StreamReader theStreamReader = new StreamReader(resultStream);
                while ((theLine = theStreamReader.ReadLine()) != null) {
                    if (theLine.Split('_')[0].ToLower() == App.appName.ToLower() && Version.Parse(theLine.Split('_')[1].Remove(theLine.Split('_')[1].Length - 4, 4)) > updateVersion) { updateVersion = Version.Parse(theLine.Split('_')[1].Remove(theLine.Split('_')[1].Length - 4, 4)); }
                }

                if (updateVersion > App.AppVersion) { return new Tuple<Version, string>(updateVersion, "update"); } else { return new Tuple<Version, string>(updateVersion, null); }
            } catch (Exception ex) { return new Tuple<Version, string>(App.AppVersion, ex.Message); }
        }

        private static async void DownloadUpdate(string version, bool installNow) {
            try {
                MetroWindow metroWindow = Application.Current.MainWindow as MetroWindow;
                WebClient webClient = new WebClient();

                webClient.DownloadProgressChanged += (sender, args) => {
                    metroWindow.Dispatcher.Invoke(DispatcherPriority.DataBind, new Action(delegate () {
                        MainWindow.ProgressRing = args.ProgressPercentage > 0 && args.ProgressPercentage < 99 ? Visibility.Visible : Visibility.Hidden;
                        MainWindow.DownloadShow = args.ProgressPercentage > 0 && args.ProgressPercentage < 99 ? 200 : 0;
                        MainWindow.DownloadStatus = args.ProgressPercentage;
                    }));
                };

                webClient.DownloadFileCompleted += (sender, args) => { DownloadCompleted(version, installNow); };
                await webClient.DownloadFileTaskAsync(new Uri(App.Setting.UpdateUrl.Remove(App.Setting.UpdateUrl.Length - 6, 6) + App.appName + "_" + version + ".exe"), Path.Combine(App.updateFolder, App.appName + "_" + version + ".exe"));
            } catch (Exception ex) {
                await MainWindow.ShowMessage(false, ex.Message + Environment.NewLine + ex.StackTrace); MainWindow.ProgressRing = Visibility.Hidden;
            }
        }

        private static async void DownloadCompleted(string version, bool installNow) {
            MetroWindow metroWindow = Application.Current.MainWindow as MetroWindow;
            try {
                if (new FileInfo(Path.Combine(App.updateFolder, App.appName + "_" + version + ".exe")).Length > 1048576) {
                    if (installNow) { InstallUpdate(Path.Combine(App.updateFolder, App.appName + "_" + version + ".exe")); } else {
                        MetroDialogSettings settings = new MetroDialogSettings() { AffirmativeButtonText = metroWindow.Resources["installNow"].ToString(), NegativeButtonText = metroWindow.Resources["ok"].ToString() };
                        MessageDialogResult result = await metroWindow.ShowMessageAsync(metroWindow.Resources["info"].ToString(),
                                    metroWindow.Resources["newVersionIsDownloaded"].ToString() + ": " + version, MessageDialogStyle.AffirmativeAndNegative, settings);
                        if (result == MessageDialogResult.Affirmative) { InstallUpdate(Path.Combine(App.updateFolder, App.appName + "_" + version + ".exe")); } else { MainWindow.ProgressRing = Visibility.Hidden; }
                    }
                } else {   //Update file is under 1MB go next without info
                    metroWindow.Dispatcher.Invoke(DispatcherPriority.DataBind, new Action(delegate () { MainWindow.ProgressRing = Visibility.Hidden; MainWindow.DownloadShow = 0; MainWindow.DownloadStatus = 0; }));
                }
            } catch (Exception ex) { await MainWindow.ShowMessage(false, ex.Message + Environment.NewLine + ex.StackTrace); MainWindow.ProgressRing = Visibility.Hidden; }
        }

        private static async void InstallUpdate(string path) {
            try {
                var psi = new ProcessStartInfo() { FileName = path, Verb = (Environment.OSVersion.Version.Major >= 6) ? "runas" : "" };
                Process.Start(psi);
                App.AppQuitRequest(true);
            } catch (Exception ex) { await MainWindow.ShowMessage(false, ex.Message + Environment.NewLine + ex.StackTrace); MainWindow.ProgressRing = Visibility.Hidden; }
        }
    }
}