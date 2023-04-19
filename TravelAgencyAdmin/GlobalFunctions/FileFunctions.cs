using System;
using System.IO;
using System.Web.Script.Serialization;
using TravelAgencyAdmin.Classes;
using Newtonsoft.Json;
using System.Text;

namespace TravelAgencyAdmin.GlobalFunctions
{
    class FileFunctions
    {

        /// <summary>
        /// Application Startup Check and configure Data Structure in folder ProgramData
        /// And required files, load client configuration config.json
        /// </summary>
        /// <returns></returns>
        public static Config LoadSettings()
        {
            try
            {
                if (!CheckDirectory(App.reportFolder)) { CreatePath(App.reportFolder); }try { FileFunctions.ClearFolder(App.reportFolder); } catch { }
                if (!CheckDirectory(App.updateFolder)) { CreatePath(App.updateFolder); }try { FileFunctions.ClearFolder(App.updateFolder); } catch { }
                if (!CheckDirectory(App.tempFolder)) { CreatePath(App.tempFolder); }try { FileFunctions.ClearFolder(App.tempFolder); } catch { }
                if (!CheckDirectory(App.galleryFolder)) { CreatePath(App.galleryFolder); }try { FileFunctions.ClearFolder(App.galleryFolder); } catch { }

                if (!CheckFile(Path.Combine(App.startupPath, "Data", App.settingFile))) CopyFile(Path.Combine(App.startupPath, "Data", App.settingFile), Path.Combine(App.settingFolder, App.settingFile));
                CopyFile(Path.Combine(App.startupPath, "Data/no_photo.png"), Path.Combine(App.settingFolder, "no_photo.png"));
                CopyFiles(Path.Combine(App.startupPath, "Data", "SubReports"), App.reportFolder);

                string json = File.ReadAllText(Path.Combine(App.settingFolder, App.settingFile), FileDetectEncoding(Path.Combine(App.settingFolder, App.settingFile)));
                Config SettingData = JsonConvert.DeserializeObject<Config>(json);
                return SettingData;
            }
            catch (Exception ex) { App.ApplicationLogging(ex); 
                return new Config();
            }
        }


        /// <summary>
        /// Function for saving Application Configuration
        /// This is client configuration only
        /// </summary>
        /// <returns></returns>
        public static bool SaveSettings()
        {
            try
            {
                using (StreamWriter sw = File.CreateText(Path.Combine(App.settingFolder, App.settingFile)))
                {
                    sw.Write(new JavaScriptSerializer().Serialize(App.Setting));
                    sw.Flush();
                    sw.Close();
                }
                return true;
            }
            catch { return false; }
        }


        /// <summary>
        /// Prepared Method for Files Copy
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="destinationPath"></param>
        public static void CopyFiles(string sourcePath, string destinationPath)
        {
            string[] filePaths = Directory.GetFiles(sourcePath);
            foreach (string fullFilePath in filePaths)
            {
                string fileName = Path.GetFileName(fullFilePath);
                if (!File.Exists(Path.Combine(destinationPath, fileName)))
                {
                    File.Copy(Path.Combine(sourcePath, fileName), Path.Combine(destinationPath, fileName));
                }
            }
        }


        /// <summary>
        /// Prepared Method for Create empty file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool CreateFile(string file)
        {
            if (!File.Exists(file))
                File.Create(file).Close();

            return CheckFile(file);

        }


        /// <summary>
        /// Prepared Method for Get Information of File encoding 
        /// UTF8,WIN1250,etc
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public static Encoding FileDetectEncoding(string FileName)
        {
            string enc = "";
            if (File.Exists(FileName))
            {
                FileStream filein = new FileStream(FileName, FileMode.Open, FileAccess.Read);
                if ((filein.CanSeek))
                {
                    byte[] bom = new byte[5];
                    filein.Read(bom, 0, 4);
                    // EF BB BF       = utf-8
                    // FF FE          = ucs-2le, ucs-4le, and ucs-16le
                    // FE FF          = utf-16 and ucs-2
                    // 00 00 FE FF    = ucs-4
                    if ((((bom[0] == 0xEF) && (bom[1] == 0xBB) && (bom[2] == 0xBF)) || ((bom[0] == 0xFF) && (bom[1] == 0xFE)) || ((bom[0] == 0xFE) && (bom[1] == 0xFF)) || ((bom[0] == 0x0) && (bom[1] == 0x0) && (bom[2] == 0xFE) && (bom[3] == 0xFF))))
                        enc = "Unicode";
                    else
                        enc = "ASCII";
                    // Position the file cursor back to the start of the file
                    filein.Seek(0, SeekOrigin.Begin);
                }
                filein.Close();
            }
            if (enc == "Unicode")
                return Encoding.UTF8;
            else
                return Encoding.Default;
        }

        public static void DeleteDirectory(string directory)
        {
            if (Directory.Exists(directory))
                Directory.Delete(directory, true);
        }

        public static bool CheckDirectory(string directory)
        {
            return Directory.Exists(directory);
        }

        public static bool CheckFile(string file)
        {
            return File.Exists(file);
        }

        public static bool CopyFile(string from, string to)
        {
            try
            {
                File.Copy(from, to, true);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool CreatePath(string path)
        {
            try {
                string[] pathParts = path.Split('\\');

                for (int i = 0; i < pathParts.Length; i++)
                {
                    if (i > 0)
                        pathParts[i] = Path.Combine(pathParts[i - 1], pathParts[i]);

                    if (!Directory.Exists(pathParts[i]))
                        Directory.CreateDirectory(pathParts[i]);
                }
                return true;
            } catch { return false; }

        }

        public static void CreateDirectory(string directory)
        {
            try {
                if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            } catch { }
        }

        public static bool DeleteFile(string file)
        {
            try {
                File.Delete(file);
                if (!CheckFile(file))
                    return true;
                else return false;
            } catch {return false; }

        }

        public static void ClearFolder(string FolderName)
        {
            try {
                DirectoryInfo dir = new DirectoryInfo(FolderName);
                foreach (FileInfo fi in dir.GetFiles())
                { fi.Delete(); }
            } catch { }
        }

        public static bool ByteArrayToFile(string fileName, byte[] byteArray)
        {
            try {
                using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(byteArray, 0, byteArray.Length);
                    return true;
                }
            } catch { return false; }
        }

        public static void Fn_WriteToFile(string file, string Message)
        {
            try {
                CreateFile(file);
                StreamWriter objWriter = new StreamWriter(file, true);
                objWriter.WriteLine(Message);
                objWriter.Close();
            } catch { }
        }

        public static bool VncServerIniFile(string path)
        {
            try {
                string iniFileContent = $"[Permissions]\r\n[admin]\r\nFileTransferEnabled=1\r\nFTUserImpersonation=0\r\nBlankMonitorEnabled=1\r\nBlankInputsOnly=0\r\nDefaultScale=1\r\nUseDSMPlugin=0\r\nDSMPlugin=\r\nprimary=1\r\nsecondary=0\r\nSocketConnect=1\r\nHTTPConnect=1\r\nAutoPortSelect=1\r\nInputsEnabled=1\r\nLocalInputsDisabled=0\r\nIdleTimeout=0\r\nEnableJapInput=0\r\nEnableUnicodeInput=0\r\nEnableWin8Helper=0\r\nQuerySetting=2\r\nQueryTimeout=10\r\nQueryDisableTime=0\r\nQueryAccept=0\r\nMaxViewerSetting=0\r\nMaxViewers=128\r\nCollabo=0\r\nFrame=0\r\nNotification=1\r\nOSD=0\r\nNotificationSelection=0\r\nLockSetting=0\r\nRemoveWallpaper=0\r\nRemoveEffects=0\r\nRemoveFontSmoothing=0\r\nDebugMode=0\r\nAvilog=0\r\npath={path}\r\nDebugLevel=0\r\nAllowLoopback=1\r\nLoopbackOnly=0\r\nAllowShutdown=1\r\nAllowProperties=1\r\nAllowInjection=0\r\nAllowEditClients=1\r\nFileTransferTimeout=30\r\nKeepAliveInterval=5\r\nIdleInputTimeout=0\r\nDisableTrayIcon=0\r\nrdpmode=0\r\nnoscreensaver=0\r\nSecure=0\r\nMSLogonRequired=0\r\nNewMSLogon=0\r\nReverseAuthRequired=0\r\nConnectPriority=0\r\nservice_commandline=\r\naccept_reject_mesg=\r\ncloudServer=\r\ncloudEnabled=0\r\n[UltraVNC]\r\npasswd=B16A3A6F32CE4F0B1E\r\npasswd2=B16A3A6F32CE4F0B1E\r\n[poll]\r\nTurboMode=1\r\nPollUnderCursor=0\r\nPollForeground=0\r\nPollFullScreen=1\r\nOnlyPollConsole=0\r\nOnlyPollOnEvent=0\r\nMaxCpu2=100\r\nMaxFPS=25\r\nEnableDriver=1\r\nEnableHook=1\r\nEnableVirtual=0\r\nautocapt=1\r\n";
                File.WriteAllText(Path.Combine(path, "UltraVNC.ini"), iniFileContent);
                return true;
            }
            catch { return false; }
        }

    }
}
