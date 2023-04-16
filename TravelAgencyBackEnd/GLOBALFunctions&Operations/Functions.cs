using System.Text;
using System.IO;

namespace TravelAgencyBackEnd
{
    class Functions
    {
        public static void LoadSettings()
        {
            //if (!fn_check_directory(Program.setting_folder)) { 
            //    fn_create_path(Program.setting_folder);
            //    fn_copy_file(Path.Combine(Program.startup_path, Program.DataPath, Program.ConfigFile), Path.Combine(Program.setting_folder, Program.ConfigFile));
            //    fn_copy_file(Path.Combine(Program.startup_path, Program.DataPath, "Log4Net.config"), Path.Combine(Program.setting_folder, "Log4Net.config")); 
            //}
        }

        public static bool fn_check_directory(string directory)
        {
            return Directory.Exists(directory);
        }

        public static bool fn_check_file(string file)
        {
            return File.Exists(file);
        }

        public static bool fn_copy_file(string from, string to)
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

        public static bool fn_create_path(string path)
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

        public static Encoding Fn_file_detect_encoding(string FileName)
        {
            string enc = "";
            if (File.Exists(FileName))
            {
                FileStream filein = new(FileName, FileMode.Open, FileAccess.Read);
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

        public static byte[] ReadFile(string fileName)
        {
            FileStream f = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            int size = (int)f.Length;
            byte[] data = new byte[size];
            size = f.Read(data, 0, size);
            f.Close();
            return data;
        }

    }
}