using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace UbytkacBackend {

    internal class Functions {

        public static void LoadSettings() {
            //if (!fn_check_directory(Program.setting_folder)) {
            //    fn_create_path(Program.setting_folder);
            //    fn_copy_file(Path.Combine(Program.startup_path, Program.DataPath, Program.ConfigFile), Path.Combine(Program.setting_folder, Program.ConfigFile));
            //    fn_copy_file(Path.Combine(Program.startup_path, Program.DataPath, "Log4Net.config"), Path.Combine(Program.setting_folder, "Log4Net.config"));
            //}
        }

        public static bool fn_check_directory(string directory) {
            return Directory.Exists(directory);
        }

        public static bool fn_check_file(string file) {
            return System.IO.File.Exists(file);
        }

        public static bool fn_copy_file(string from, string to) {
            try
            {
                System.IO.File.Copy(from, to, true);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool fn_create_path(string path) {
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

        public static Encoding Fn_file_detect_encoding(string FileName) {
            string enc = "";
            if (System.IO.File.Exists(FileName))
            {
                FileStream filein = new(FileName, FileMode.Open, FileAccess.Read);
                if ((filein.CanSeek))
                {
                    byte[] bom = new byte[5];
                    filein.Read(bom, 0, 4);
                    // EF BB BF = utf-8 FF FE = ucs-2le, ucs-4le, and ucs-16le FE FF = utf-16 and
                    // ucs-2 00 00 FE FF = ucs-4
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

        public static byte[] ReadFile(string fileName) {
            FileStream f = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            int size = (int)f.Length;
            byte[] data = new byte[size];
            size = f.Read(data, 0, size);
            f.Close();
            return data;
        }

        public static bool IsValidEmail(string email) {
            var trimmedEmail = email.Trim();
            if (trimmedEmail.EndsWith(".")) { return false; }
            try {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            } catch { return false; }
        }

        /// <summary>
        /// Randoms the string.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public static string RandomString(int length) {
            Random random = new Random();
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// Separate ByteArray from 64 encode file
        /// For inserted file types
        /// </summary>
        /// <param name="strContent">Content of the string.</param>
        /// <returns></returns>
        public static byte[] GetByteArrayFrom64Encode(string strContent) {
            try {
                var base64Data = Regex.Match(strContent, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
                base64Data = base64Data.Length > 0 ? base64Data : Regex.Match(strContent, @"data:text/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
                base64Data = base64Data.Length > 0 ? base64Data : Regex.Match(strContent, @"data:video/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
                base64Data = base64Data.Length > 0 ? base64Data : Regex.Match(strContent, @"data:application/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
                return Convert.FromBase64String(base64Data);
            } catch { }
            byte[] result = new byte[] { };
            return result;
        }


        /// <summary>
        /// Gets the self signed certificate For API Security HTTPS.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public static X509Certificate2 GetSelfSignedCertificate(string password) {
            var commonName = "127.0.0.1,localhost";// ServerConfigSettings.ConfigCertificateDomain;
            var rsaKeySize = 2048;
            var years = 10;
            var hashAlgorithm = HashAlgorithmName.SHA256;

            using (var rsa = RSA.Create(rsaKeySize)) {
                var request = new CertificateRequest($"cn={commonName}", rsa, hashAlgorithm, RSASignaturePadding.Pkcs1);

                SubjectAlternativeNameBuilder subjectAlternativeNameBuilder = new();
                subjectAlternativeNameBuilder.AddDnsName(Assembly.GetExecutingAssembly().GetName().FullName);

                X509BasicConstraintsExtension extension = new();

                request.CertificateExtensions.Add(
                  new X509KeyUsageExtension(X509KeyUsageFlags.DataEncipherment | X509KeyUsageFlags.KeyEncipherment | X509KeyUsageFlags.DigitalSignature | X509KeyUsageFlags.KeyCertSign, false)
                );

                request.CertificateExtensions.Add(
                  new X509EnhancedKeyUsageExtension(
                    new OidCollection { new Oid("1.3.6.1.5.5.7.3.1"), new Oid("1.3.6.1.5.5.7.3.2") }, false)
                );

                var notAfter = DateTimeOffset.Now.AddYears(years);
                var certificate = request.CreateSelfSigned(DateTimeOffset.Now.AddDays(-1), notAfter);
                if (OperatingSystem.IsWindows()) { certificate.FriendlyName = Assembly.GetExecutingAssembly().GetName().FullName; }

                try { //Saving Autogenerate Certificate
                    byte[] exportedData = certificate.Export(X509ContentType.Pfx, password);
                    File.WriteAllBytes(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Data", "ServerAutoCertificate.pfx"), exportedData);
                } catch { }

                return new X509Certificate2(certificate.Export(X509ContentType.Pfx, password), password, X509KeyStorageFlags.Exportable);
            }
        }


        /// <summary>
        /// Set Imported Certificate from file on Server for Https 
        /// File must has Valid path from Startup Data Path
        /// </summary>
        /// <returns></returns>
        public static X509Certificate2 GetSelfSignedCertificateFromFile(string FileNameFromDataPath) {
            byte[]? certificate = null;
            string? password = null;
            try {
                certificate = File.ReadAllBytes(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Data", FileNameFromDataPath));
                password = "CertPassword";//ServerConfigSettings.ConfigCertificatePassword;
                return new X509Certificate2(certificate, password);
            } catch (Exception Ex) { ServerCoreFunctions.SendEmail(new MailRequest() { Content = "Incorrect Certificate Path or Password, " + ServerCoreFunctions.GetSystemErrMessage(Ex) }); }
            return GetSelfSignedCertificate("CertPassword");
        }

    }
}