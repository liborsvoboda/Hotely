using TravelAgencyAdmin.Api;
using TravelAgencyAdmin.Classes;
using TravelAgencyAdmin.GlobalClasses;
using TravelAgencyAdmin.Helper;
using TravelAgencyAdmin.Pages;
using log4net.Config;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace TravelAgencyAdmin
{
    public partial class App : Application
    {
        /// <summary>
        /// Global Application Startup Settings
        /// </summary>
        public static log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static Version AppVersion = Assembly.GetExecutingAssembly().GetName().Version;
        public static List<Parameters> Parameters = new List<Parameters>();
        public static UserData UserData = new UserData();

        internal static string appName = Assembly.GetEntryAssembly().GetName().FullName.Split(',')[0];
        internal static string startupPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        internal static string settingFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), appName);
        internal static string reportFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), appName, "Reports");
        internal static string updateFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), appName, "Update");
        internal static string tempFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), appName, "Temp");
        internal static string settingFile = "config.json";
        public static Config Setting = GlobalFunctions.FileFunctions.LoadSettings();

        /// <summary>
        /// Application Error handlers
        /// </summary>
        public App()
        {
            log4net.GlobalContext.Properties["RollingFileAppender"] = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), Assembly.GetEntryAssembly().GetName().FullName.Split(',')[0], "Log4NetApplication.log");
            _ = XmlConfigurator.Configure();
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

        private void CurrentDomain_FirstChanceException(object sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
        {
            log.Warn(e.Exception);
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            log.Fatal(e.Exception);
            e.Handled = true;

            //CrashReporterGlobalField._ReportCrash.Send(e.Exception);
            //if (CrashReporterGlobalField._ReportCrash.IsQuit)
            //{
                Current.Shutdown(-1);
            //}

        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {

            Exception exception = e.ExceptionObject as Exception;
            log.Fatal(exception);

            _ = MessageBox.Show(exception.Message + "\n" + "Application must be close !!!", "CurrentDomain UnhandledException", MessageBoxButton.OK, MessageBoxImage.Error);

            if (e.IsTerminating)
            {
                Environment.Exit(1);
            }
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            log.Fatal(e.Exception);
            _ = MessageBox.Show(e.Exception.Message, "TaskScheduler UnobservedTaskException", MessageBoxButton.OK, MessageBoxImage.Error);
            e.SetObserved();
        }

        private void WinFormApplication_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            log.Fatal(e.Exception);
            //CrashReporterGlobalField._ReportCrash.Send(e.Exception);
        }

        /// <summary>
        /// Connected Starting Video
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            if (!Setting.HideStartVideo)
            {
                MetroWindow startupAnimation = new WelcomePage();
                MainWindow = startupAnimation;
                startupAnimation.Closing += StartupAnimation_Closing;
            } else {
                MetroWindow mainView = new MainWindow();
                MainWindow = mainView;
                mainView.Show(); mainView.Focus();
            }
        }


        private void StartupAnimation_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MetroWindow mainView = new MainWindow();
            MainWindow = mainView;
            mainView.Show(); mainView.Focus();
        }
    }
}
