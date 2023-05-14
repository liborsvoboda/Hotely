using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TravelAgencyAdmin.Classes;

namespace TravelAgencyAdmin.GlobalOperations {

    /// <summary>
    /// Centralised System Functions for work with Types, methods, Formats, Logic, supported methods
    /// </summary>
    internal class SystemOperations {

        /// <summary>
        /// Settings Local Application Translation dictionaries (Resources Files) for Pages Will be
        /// replaced by DBDictionary, but for Offline Running must be possible
        /// </summary>
        /// <param name="Resources">      </param>
        /// <param name="languageDefault"></param>
        /// <returns></returns>
        public static ResourceDictionary SetLanguageDictionary(ResourceDictionary Resources, string languageDefault) {
            if (languageDefault == "system") languageDefault = Thread.CurrentThread.CurrentCulture.ToString();
            App.appLanguage = languageDefault;

            ResourceDictionary dict = new ResourceDictionary();
            switch (languageDefault) {
                case "en-US":
                    dict.Source = new Uri("..\\Languages\\StringResources.xaml", UriKind.Relative);
                    break;

                case "cs-CZ":
                    dict.Source = new Uri("..\\Languages\\StringResources.cs-CZ.xaml", UriKind.Relative);
                    break;
            }
            Resources.MergedDictionaries.Add(dict);
            return Resources;
        }

        /// <summary>
        /// System Mail sending
        /// </summary>
        /// <param name="address"></param>
        /// <param name="subject"></param>
        /// <param name="body">   </param>
        /// <param name="attach"> </param>
        public static void SendMailWithMailTo(string address, string subject, string body, string attach) {
            string mailto =
                $"mailto:{address}?Subject={subject}&Body={body}&Attach={attach}";
            Process.Start(mailto);
        }

        /// <summary>
        /// Automatic Increase version System Ideal for small systems with more release in 1 day
        /// Increase Windows Correct 3 position for Widows Installation In Debug is increase last 4 position
        /// </summary>
        /// <returns></returns>
        public static bool IncreaseFileVersionBuild() {
            if (Debugger.IsAttached) {
                try {
                    var fi = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.GetDirectories("Properties")[0].GetFiles("AssemblyInfo.cs")[0];
                    var ve = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
                    string ol = ve.FileMajorPart.ToString() + "." + ve.FileMinorPart.ToString() + "." + ve.FileBuildPart.ToString() + "." + ve.FilePrivatePart.ToString();

                    AppVersion appVersion = new AppVersion() { Major = ve.FileMajorPart, Minor = ve.FileMinorPart, Build = ve.FileBuildPart, Private = ve.FilePrivatePart + 1 };
                    string ne = appVersion.Major.ToString() + "." + appVersion.Minor.ToString() + "." + appVersion.Build.ToString() + "." + appVersion.Private.ToString();

                    File.WriteAllText(fi.FullName, File.ReadAllText(fi.FullName).Replace("[assembly: AssemblyVersion(\"" + ol + "\")]", "[assembly: AssemblyVersion(\"" + ne + "\")]"));
                    File.WriteAllText(fi.FullName, File.ReadAllText(fi.FullName).Replace("[assembly: AssemblyFileVersion(\"" + ol + "\")]", "[assembly: AssemblyFileVersion(\"" + ne + "\")]"));
                    return true;
                } catch { return false; }
            } else {
                try {
                    var fi = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.GetDirectories("Properties")[0].GetFiles("AssemblyInfo.cs")[0];
                    var ve = FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
                    string ol = ve.FileMajorPart.ToString() + "." + ve.FileMinorPart.ToString() + "." + ve.FileBuildPart.ToString() + "." + ve.FilePrivatePart.ToString();

                    bool moveNext = false;
                    AppVersion appVersion = new AppVersion() { Major = ve.FileMajorPart, Minor = ve.FileMinorPart, Build = ve.FileBuildPart, Private = ve.FilePrivatePart };
                    if (appVersion.Build + 1 > 99) { appVersion.Build = 0; moveNext = true; } else { appVersion.Build++; }
                    if (appVersion.Minor + 1 > 99) { appVersion.Minor = 0; moveNext = true; } else if (moveNext) { appVersion.Minor++; moveNext = false; }
                    if (moveNext) { appVersion.Major++; }

                    string ne = appVersion.Major.ToString() + "." + appVersion.Minor.ToString() + "." + appVersion.Build.ToString() + "." + appVersion.Private.ToString();
                    File.WriteAllText(fi.FullName, File.ReadAllText(fi.FullName).Replace("[assembly: AssemblyVersion(\"" + ol + "\")]", "[assembly: AssemblyVersion(\"" + ne + "\")]"));
                    File.WriteAllText(fi.FullName, File.ReadAllText(fi.FullName).Replace("[assembly: AssemblyFileVersion(\"" + ol + "\")]", "[assembly: AssemblyFileVersion(\"" + ne + "\")]"));
                    return true;
                } catch { return false; }
            }
        }

        /// <summary>
        /// Mining All Exception Information For Central System Logger Ignore Some selected Fails is
        /// possible by Ignored Exception Settings
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="msgCount"> </param>
        /// <returns></returns>
        public static async Task<string> GetExceptionMessages(Exception exception, int msgCount = 1) {
            if (exception != null && App.IgnoredExceptionList.FirstOrDefault(a => a.ErrorNumber == exception.HResult.ToString() && a.Active == true) != null && !bool.Parse(await DataOperations.ParameterCheck("DeactivateIgnoredException"))) return null;
            return exception != null ? string.Format("{0}: {1}\n{2}", msgCount, (exception.Message + Environment.NewLine + exception.HResult.ToString() + Environment.NewLine + exception.StackTrace + Environment.NewLine), GetExceptionMessages(exception.InnerException, ++msgCount)) : string.Empty;
        }

        /// <summary>
        /// Mining All Exception Information For Local System Logger EveryTime Show All fails for
        /// Best Developing On Expert Level
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="msgCount"> </param>
        /// <returns></returns>
        public static string GetExceptionMessagesAll(Exception exception, int msgCount = 1) {
            return exception != null ? string.Format("{0}: {1}\n{2}", msgCount, (exception.Message + Environment.NewLine + exception.HResult.ToString() + Environment.NewLine + exception.StackTrace + Environment.NewLine), GetExceptionMessagesAll(exception.InnerException, ++msgCount)) : string.Empty;
        }

        /// <summary>
        /// SYSTEM Advanced Filter Conversion for API return existing filter for saving to string in
        /// selected Page
        /// </summary>
        /// <param name="filterBox"></param>
        /// <returns></returns>
        public static string FilterToString(ComboBox filterBox) {
            string advancedFilter = null;
            int index = 0;
            try {
                foreach (WrapPanel filterItem in filterBox.Items) {
                    if (index > 0) {
                        if (filterItem.Name.Split('_')[0] == "condition") {
                            index = 0;
                            foreach (var item in filterItem.Children) {
                                if (index == 1) { advancedFilter += "[!]" + ((Label)item).Content; }
                                if (index > 1) { advancedFilter += "{!}" + ((Label)item).Content; }
                                index++;
                            }
                        } else if (filterItem.Name.Split('_')[0] == "filter") {
                            advancedFilter += "[!]" + ((ComboBox)filterItem.Children[0]).SelectionBoxItem;
                            advancedFilter += "{!}" + ((ComboBox)filterItem.Children[2]).SelectionBoxItem;
                            advancedFilter += "{!}'" + ((TextBox)filterItem.Children[3]).Text + "'";
                        }
                        index = 1;
                    }
                    index++;
                }
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
            return advancedFilter;
        }

        /// <summary>
        /// Its Solution for this is a solution for demanding and multiplied servers Or Running
        /// SHARP and Test System By One Backend Server Service API Urls with Namespaces in Name are
        /// for Backend model with More Same Database Schemas Backend Databases count in One Server
        /// Service is Unlimited
        /// </summary>
        /// <param name="stringForRemoveNamespace"></param>
        public static string RemoveAppNamespaceFromString(string stringForRemoveNamespace) {
            string appNameSpace = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace.Split('.')[0];
            return stringForRemoveNamespace.Replace(appNameSpace, string.Empty);
        }
    }
}