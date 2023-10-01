using GlobalClasses;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using UbytkacAdmin.Classes;
using UbytkacAdmin.GlobalOperations;
using UbytkacAdmin.Pages;
using UbytkacAdmin.SystemStructure;

namespace UbytkacAdmin {

    public partial class App : Application {

        /// <summary>
        /// Global Application Startup Settings Central Parameters / Languages / User / Config
        /// </summary>
        public static Version AppVersion = Assembly.GetExecutingAssembly().GetName().Version;

        public static List<ParameterList> ParameterList = null;
        public static List<LanguageList> LanguageList = null;
        public static UserData UserData = new UserData();
        public static List<IgnoredExceptionList> IgnoredExceptionList = new List<IgnoredExceptionList>();

        internal static string appName = Assembly.GetEntryAssembly().GetName().FullName.Split(',')[0];
        internal static string startupPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        internal static string settingFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), appName);
        internal static string reportFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), appName, "Reports");
        internal static string updateFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), appName, "Update");
        internal static string tempFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), appName, "Temp");
        internal static string galleryFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), appName, "Gallery");
        internal static string settingFile = "config.json";
        public static Config Setting = FileOperations.LoadSettings();
        internal static string appLanguage = Thread.CurrentThread.CurrentCulture.ToString();

        /// <summary>
        /// Application Error handlers
        /// </summary>
        public App() {
            SystemOperations.SetLanguageDictionary(Resources, JsonConvert.DeserializeObject<Language>(Setting.DefaultLanguage).Value);
            //log4net.GlobalContext.SystemConfiguration["RollingFileAppender"] = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), Assembly.GetEntryAssembly().GetName().FullName.Split(',')[0], "Log4NetApplication.log");
            //_ = XmlConfigurator.Configure();
            Dispatcher.UnhandledException += App_DispatcherUnhandledException;
            DispatcherUnhandledException += App_DispatcherUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            System.Windows.Forms.Application.ThreadException += WinFormApplication_ThreadException;
            AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            // enabled ssl connections
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
        }

        /// <summary>
        /// Connected Starting Video
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e) {

            base.OnStartup(e);
            if (!Setting.HideStartVideo) {
                MetroWindow startupAnimation = new WelcomePage();
                MainWindow = startupAnimation;
                startupAnimation.Closing += StartupAnimation_Closing;
            } else { StartupAnimation_Closing(new object(), new CancelEventArgs()); }
        }

        /// <summary>
        /// Close Start Animation and run Application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void StartupAnimation_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            MetroWindow mainView = new MainWindow();
            MainWindow = mainView;
            mainView.Show(); mainView.Focus();
        }

        public static void AppRestart() {
            SystemWindowDataModel.SaveTheme();
            Process.Start(Assembly.GetEntryAssembly().EscapedCodeBase);
            ApplicationQuit();
            Process.GetCurrentProcess().Kill();
        }

        /// <summary>
        /// App Quit
        /// </summary>
        /// <param name="silent"></param>
        public static async void AppQuitRequest(bool silent) {
            if (!silent) {
                MetroWindow metroWindow = Current.MainWindow as MetroWindow;
                MetroDialogSettings settings = new MetroDialogSettings() { AffirmativeButtonText = metroWindow.Resources["yes"].ToString(), NegativeButtonText = metroWindow.Resources["no"].ToString() };
                MessageDialogResult result = await metroWindow.ShowMessageAsync(metroWindow.Resources["closeAppTitle"].ToString(), metroWindow.Resources["closeAppQuestion"].ToString(), MessageDialogStyle.AffirmativeAndNegative, settings);
                if (result == MessageDialogResult.Affirmative) ApplicationQuit();
            } else { ApplicationQuit(); }
        }

        /// <summary>
        /// MainWindow Closing Handler for Cleaning TempData, disable Addons / Tool and Third Party
        /// Software Closing Third Party processes
        /// </summary>
        private static void ApplicationQuit() {
            try {
                MainWindow mainWindow = (MainWindow)Current.MainWindow;
                mainWindow.AppSystemTimer.Enabled = false;
                if (mainWindow.vncProccess != null && !mainWindow.vncProccess.HasExited) { mainWindow.vncProccess.Kill(); };
            } catch { }

            try { FileOperations.ClearFolder(reportFolder); } catch { }
            try { FileOperations.ClearFolder(galleryFolder); } catch { }
            try { FileOperations.ClearFolder(tempFolder); } catch { }

            SystemWindowDataModel.SaveTheme();
            Current.Shutdown();
        }

        /// <summary>
        /// Full Application System logging Running If is AppSystemTimer is Enabled for disable
        /// other processes exceptions
        /// </summary>
        /// <param name="ex">           </param>
        /// <param name="customMessage"></param>
        public static void ApplicationLogging(Exception ex, string customMessage = null) {
            try {
                Current?.Invoke(async () => {
                    if (Current.MainWindow != null && Current.MainWindow.Name == "XamlMainWindow" && UserData.Authentification != null) {
                        if (string.IsNullOrWhiteSpace(customMessage)) DBOperations.SaveSystemFailMessage(await SystemOperations.GetExceptionMessages(ex));
                        else DBOperations.SaveSystemFailMessage(customMessage);
                        //if (Setting.WriteToLog) log.Error(ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + customMessage);
                    }
                });
            } catch { }
        }

        /// <summary>
        /// FullSystem Logging Every Exeption types are monitored for maximalize correct running all
        /// processes, addons, systems, communications, threads, network All detail of application
        /// system add all used posibilities
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void CurrentDomain_FirstChanceException(object sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e) => ApplicationLogging(e.Exception);

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e) {
            e.Handled = true; ApplicationLogging(e.Exception);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e) {
            Exception exception = e.ExceptionObject as Exception; ApplicationLogging(exception); if (e.IsTerminating) Environment.Exit(1);
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e) {
            ApplicationLogging(e.Exception); e.SetObserved();
        }

        private void WinFormApplication_ThreadException(object sender, ThreadExceptionEventArgs e) {
            ApplicationLogging(e.Exception); /*CrashReporterGlobalField._ReportCrash.Send(e.Exception);*/
        }

        /// <summary>
        /// Keyboard Pointer to Central Keyboard Reaction Definitions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void RootAppKeyDownController(object sender, KeyEventArgs e) => HardwareOperations.ApplicationKeyboardMaping(e);
    }
}