using System;
using System.IO;
using System.Web.Script.Serialization;
using System.Collections.Specialized;
using System.Diagnostics;
using TravelAgencyAdmin.Classes;
using System.Windows;
using System.Threading;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Text;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Microsoft.Win32;

namespace TravelAgencyAdmin.GlobalFunctions
{
    class MediaFunctions
    {
        public static ResourceDictionary SetLanguageDictionary(ResourceDictionary Resources, string languageDefault)
        {
            if (languageDefault == "system") languageDefault = Thread.CurrentThread.CurrentCulture.ToString();

            ResourceDictionary dict = new ResourceDictionary();
            switch (languageDefault)
            {
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



        public static void SendMailWithMailTo(string address, string subject, string body, string attach)
        {
            string mailto =
                $"mailto:{address}?Subject={subject}&Body={body}&Attach={attach}";
            Process.Start(mailto);
        }


        public static bool IncreaseFileVersionBuild()
        {
            if (Debugger.IsAttached)
            {
                try
                {
                    var fi = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.GetDirectories("Properties")[0].GetFiles("AssemblyInfo.cs")[0];
                    var ve = FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
                    string ol = ve.FileMajorPart.ToString() + "." + ve.FileMinorPart.ToString() + "." + ve.FileBuildPart.ToString() + "." + ve.FilePrivatePart.ToString();

                    AppVersion appVersion = new AppVersion() { Major = ve.FileMajorPart, Minor = ve.FileMinorPart, Build = ve.FileBuildPart, Private = ve.FilePrivatePart + 1 };
                    string ne = appVersion.Major.ToString() + "." + appVersion.Minor.ToString() + "." + appVersion.Build.ToString() + "." + appVersion.Private.ToString();

                    File.WriteAllText(fi.FullName, File.ReadAllText(fi.FullName).Replace("[assembly: AssemblyVersion(\"" + ol + "\")]", "[assembly: AssemblyVersion(\"" + ne + "\")]"));
                    File.WriteAllText(fi.FullName, File.ReadAllText(fi.FullName).Replace("[assembly: AssemblyFileVersion(\"" + ol + "\")]", "[assembly: AssemblyFileVersion(\"" + ne + "\")]"));
                    return true;
                } catch { return false; }
            } else {
                try
                {
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

        public static ImageSource ByteToImage(byte[] imageData)
        {
            BitmapImage biImg = new BitmapImage();
            MemoryStream ms = new MemoryStream(imageData);
            biImg.BeginInit();
            biImg.StreamSource = ms;
            biImg.EndInit();

            ImageSource imgSrc = biImg as ImageSource;

            return imgSrc;
        }

        public async static void SaveAppScreenShot(MainWindow mainWindow)
        {
            try
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                RenderTargetBitmap bmp = new RenderTargetBitmap((int)mainWindow.ActualWidth, (int)mainWindow.ActualHeight, 96, 96, PixelFormats.Pbgra32);
                bmp.Render(mainWindow);
                encoder.Frames.Add(BitmapFrame.Create(bmp));

                SaveFileDialog dlg = new SaveFileDialog() { DefaultExt = ".png", Filter = "Image files |*.png;" };
                if (dlg.ShowDialog() == true) { 
                    FileStream fs = new FileStream(dlg.FileName, FileMode.Create);
                    encoder.Save(fs);
                    fs.Close();
                }
            } catch (Exception ex) { await MainWindow.ShowMessage(true, ex.Message + Environment.NewLine + ex.StackTrace); }

        }

    }
}
