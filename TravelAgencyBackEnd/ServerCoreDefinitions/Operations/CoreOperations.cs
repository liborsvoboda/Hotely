using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;


namespace UbytkacBackend.ServerCoreStructure {

    public static class CoreOperations {

        /// <summary>
        /// Sends the mass mail.
        /// </summary>
        /// <param name="mailRequests">The mail requests.</param>
        public static void SendMassEmail(List<MailRequest> mailRequests) {
            mailRequests.ForEach(mailRequest => { SendEmail(mailRequest, true); });
        }

        /// <summary>
        /// System Mailer sending Emails To service email with detected fail for analyze and
        /// corrections on the Way provide better services every time !!! This You can implement
        /// everywhere, !!! In Debug mode is written to Console, in Release will be sent email Empty
        /// Sender And Recipients set email for Service Recipient
        /// </summary>
        /// <param name="mailRequest">    </param>
        /// <param name="sendImmediately"></param>
        public static string SendEmail(MailRequest mailRequest, bool sendImmediately = false) {
            try {
                if ((!ServerRuntimeData.DebugMode && !ServerConfigSettings.ConfigLogWarnPlusToDbEnabled) || sendImmediately) {
                    if (ServerConfigSettings.ServiceCoreCheckerEmailSenderActive || sendImmediately) {
                        MailMessage Email = new() { From = new MailAddress(mailRequest.Sender ?? ServerConfigSettings.EmailerSMTPLoginUsername) };

                        if (mailRequest.Recipients != null && mailRequest.Recipients.Any()) { mailRequest.Recipients.ForEach(email => { Email.To.Add(email); }); }
                        else { Email.To.Add(ServerConfigSettings.EmailerServiceEmailAddress); }

                        Email.Subject = mailRequest.Subject ?? ServerConfigSettings.ConfigCoreServerRegisteredName;
                        Email.Body = mailRequest.Content;
                        Email.IsBodyHtml = true;

                        SmtpClient MailClient = new(ServerConfigSettings.EmailerSMTPServerAddress, ServerConfigSettings.EmailerSMTPPort) {
                            Credentials = new NetworkCredential(ServerConfigSettings.EmailerSMTPLoginUsername, ServerConfigSettings.EmailerSMTPLoginPassword),
                            EnableSsl = ServerConfigSettings.EmailerSMTPSslIsEnabled,
                            Host = ServerConfigSettings.EmailerSMTPServerAddress,
                            Port = ServerConfigSettings.EmailerSMTPPort
                        };
                        MailClient.Timeout = 5000;
                        MailClient.SendAsync(Email, Guid.NewGuid().ToString());
                    }
                }
                else {
                    if (ServerConfigSettings.ConfigLogWarnPlusToDbEnabled && mailRequest.Content != null &&
                        !ServerRuntimeData.ServerRestartRequest && ServerRuntimeData.ServerCoreStatus == ServerStatuses.Running.ToString()) {
                        SystemFailList SolutionFailList = new SystemFailList() { UserId = null, Message = mailRequest.Content, UserName = null };
                        new hotelsContext().SystemFailLists.Add(SolutionFailList).Context.SaveChanges();
                        Console.WriteLine(mailRequest.Content); Debug.WriteLine(mailRequest.Content);
                    }
                }
                return DBResult.success.ToString();
            } catch (Exception ex) {
                return DBResult.error.ToString();
            }
        }

        /// <summary>
        /// Gets the self signed certificate For API Security HTTPS.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public static X509Certificate2 GetSelfSignedCertificate(string password) {
            var commonName = ServerConfigSettings.ConfigCertificateDomain;
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
                if (GetOperatingSystemInfo.IsWindows()) { certificate.FriendlyName = Assembly.GetExecutingAssembly().GetName().FullName; }

                try { //Saving Autogenerate Certificate
                    byte[] exportedData = certificate.Export(X509ContentType.Pfx, password);
                    File.WriteAllBytes(System.IO.Path.Combine(ServerRuntimeData.Startup_path, ServerRuntimeData.DataPath, "ServerAutoCertificate.pfx"), exportedData);
                } catch { }

                return new X509Certificate2(certificate.Export(X509ContentType.Pfx, password), password, X509KeyStorageFlags.Exportable);
            }
        }

        /// <summary>
        /// Set Imported Certificate from file on Server for Https File must has Valid path from
        /// Startup Data Path
        /// </summary>
        /// <returns></returns>
        public static X509Certificate2 GetSelfSignedCertificateFromFile(string FileNameFromDataPath) {
            byte[]? certificate = null;
            string? password = null;
            try {
                certificate = File.ReadAllBytes(System.IO.Path.Combine(ServerRuntimeData.Startup_path, ServerRuntimeData.DataPath, FileNameFromDataPath));
                password = ServerConfigSettings.ConfigCertificatePassword;
                return new X509Certificate2(certificate, password);
            } catch (Exception Ex) { SendEmail(new MailRequest() { Content = "Incorrect Certificate Path or Password, " + DataOperations.GetSystemErrMessage(Ex) }); }
            return GetSelfSignedCertificate(ServerConfigSettings.ConfigCertificatePassword);
        }

        /// <summary>
        /// Calls the GET API URL with string Response
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public static async Task<string> CallGetApiUrl(string url) {
            HttpClient httpClient = new HttpClient();
            return await httpClient.GetStringAsync(url);
        }

        /// <summary>
        /// Server Function For Running External Processes
        /// </summary>
        /// <param name="processDefinition">The process definition.</param>
        /// <returns></returns>
        public static bool RunSystemProcess(ProcessClass processDefinition) {
            string resultOutput = "", resultError = "";

            try {
                using (Process proc = new Process()) {
                    proc.StartInfo.FileName = processDefinition.Command;
                    if (GetOperatingSystemInfo.IsWindows()) { proc.StartInfo.WorkingDirectory = processDefinition.WorkingDirectory + "\\" ?? null; }
                    proc.StartInfo.Arguments = processDefinition.Arguments ?? null;
                    //proc.StartInfo.LoadUserProfile = false;
                    proc.StartInfo.CreateNoWindow = true;
                    proc.StartInfo.UseShellExecute = false;
                    proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    proc.StartInfo.RedirectStandardOutput = true;
                    proc.StartInfo.RedirectStandardError = true;
                    proc.StartInfo.Verb = (Environment.OSVersion.Version.Major >= 6) ? "runas" : "";
                    proc.Start();

                    resultOutput += proc.StandardOutput.ReadToEnd();
                    resultError += proc.StandardError.ReadToEnd();

                    if (processDefinition.WaitForExit) { proc.WaitForExit(); }
                    Console.WriteLine(resultError);
                }
                return true;
            } catch { }
            return false;
        }

        /// <summary>
        /// Server Token Validation Parameters definition For Api is Used if is ON/Off for Api is On everyTime
        /// </summary>
        /// <returns></returns>
        public static TokenValidationParameters ValidAndGetTokenParameters() {
            return new TokenValidationParameters {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(ServerConfigSettings.ConfigJwtLocalKey)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = ServerConfigSettings.ConfigTimeTokenValidationEnabled,
                ClockSkew = TimeSpan.FromMinutes(ServerConfigSettings.ConfigApiTokenTimeoutMin),
            };
        }

        /// <summary>
        /// Token Validator Extension For Server WebPages
        /// </summary>
        /// <param name="tokenString">The token string.</param>
        /// <returns></returns>
        public static ServerWebPagesToken CheckTokenValidityFromString(string tokenString) {
            try {
                JwtSecurityTokenHandler? tokenForChek = new JwtSecurityTokenHandler();
                ClaimsPrincipal userClaims = tokenForChek.ValidateToken(tokenString, ValidAndGetTokenParameters(), out SecurityToken refreshedToken);
                ServerWebPagesToken validation = new() { Data = new() { { "Token", tokenString } }, UserClaims = userClaims, stringToken = tokenString, Token = refreshedToken, IsValid = refreshedToken != null };
                return validation;
            } catch { }
            return new ServerWebPagesToken();
        }

        /// <summary>
        /// Extension For Checking Operation System of Server Running
        /// </summary>
        public static class GetOperatingSystemInfo {

            public static bool IsWindows() =>
                RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

            public static bool IsMacOS() =>
                RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

            public static bool IsLinux() =>
                RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        }
    }
}