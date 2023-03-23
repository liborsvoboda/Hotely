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
        public static Config LoadSettings()
        {
            try
            {
                if (!CheckDirectory(App.reportFolder))
                {
                    CreatePath(App.reportFolder);
                    _ = CopyFile(Path.Combine(App.startupPath, "Data", App.settingFile), Path.Combine(App.settingFolder, App.settingFile));
                    _ = CopyFile(Path.Combine(App.startupPath, "Data/no_photo.png"), Path.Combine(App.settingFolder, "no_photo.png"));
                }
                CopyFiles(Path.Combine(App.startupPath, "Data", "SubReports"), App.reportFolder);
                if (!CheckDirectory(App.updateFolder)) { CreatePath(App.updateFolder); }
                if (!CheckDirectory(App.tempFolder)) { CreatePath(App.tempFolder); }
                string json = File.ReadAllText(Path.Combine(App.settingFolder, App.settingFile), FileDetectEncoding(Path.Combine(App.settingFolder, App.settingFile)));
                Config SettingData = JsonConvert.DeserializeObject<Config>(json);
                return SettingData;
            }
            catch (Exception ex)
            {
                App.log.Fatal("configuration load error: " + ex.Message + Environment.NewLine + ex.StackTrace);
                return new Config();
            }
        }

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
            catch
            {
                return false;
            }
        }

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

        public static bool CreateFile(string file)
        {
            if (!File.Exists(file))
                File.Create(file).Close();

            return CheckFile(file);

        }

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
            try
            {
                string[] pathParts = path.Split('\\');

                for (int i = 0; i < pathParts.Length; i++)
                {
                    if (i > 0)
                        pathParts[i] = Path.Combine(pathParts[i - 1], pathParts[i]);

                    if (!Directory.Exists(pathParts[i]))
                        Directory.CreateDirectory(pathParts[i]);
                }
                return true;
            }
            catch
            {
                return false;
            }

        }

        public static void CreateDirectory(string directory)
        {
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
        }

        public static bool DeleteFile(string file)
        {
            File.Delete(file);

            if (!CheckFile(file))
                return true;
            else
                return false;
        }

        public static void ClearFolder(string FolderName)
        {
            DirectoryInfo dir = new DirectoryInfo(FolderName);

            foreach (FileInfo fi in dir.GetFiles())
            {
                fi.Delete();
            }
        }

        public static bool ByteArrayToFile(string fileName, byte[] byteArray)
        {
            try
            {
                using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(byteArray, 0, byteArray.Length);
                    return true;
                }
            }
            catch (Exception ex)
            {
                App.log.Fatal("Report saving error: " + ex.Message + Environment.NewLine + ex.StackTrace);
                return false;
            }
        }

        public static void Fn_WriteToFile(string file, string Message)
        {
            CreateFile(file);
            StreamWriter objWriter = new StreamWriter(file, true);
            objWriter.WriteLine(Message);
            objWriter.Close();
        }



    }
}
