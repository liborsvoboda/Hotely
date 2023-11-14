using System;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using UbytkacBackend.CoreClasses;

namespace UbytkacBackend {

    internal static class ServerCoreFunctions {

        public static string GetUserApiErrMessage(Exception exception, int msgCount = 1) {
            return exception != null ? string.Format("{0}: {1}\n{2}", msgCount, exception.Message, GetUserApiErrMessage(exception.InnerException, ++msgCount)) : string.Empty;
        }

        public static string GetSystemErrMessage(Exception exception, int msgCount = 1) {
            return exception != null ? string.Format("{0}: {1}\n{2}", msgCount, (exception.Message + Environment.NewLine + exception.StackTrace + Environment.NewLine), GetSystemErrMessage(exception.InnerException, ++msgCount)) : string.Empty;
        }

        /// <summary>
        /// Sends the mass mail.
        /// </summary>
        /// <param name="mailRequests">The mail requests.</param>
        public static void SendMassEmail(List<MailRequest> mailRequests) {
            mailRequests.ForEach(mailRequest => { SendEmail(mailRequest); });
        }

        /// <summary>
        /// Creates the path recursively.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static bool CreatePath(string path) {
            try {
                string[] pathParts = path.Split('\\');

                for (int i = 0; i < pathParts.Length; i++) {
                    if (i > 0)
                        pathParts[i] = Path.Combine(pathParts[i - 1], pathParts[i]);

                    if (!Directory.Exists(pathParts[i]))
                        Directory.CreateDirectory(pathParts[i]);
                }
                return true;
            } catch {
                return false;
            }
        }

        /// <summary>
        /// System Mailer sending Emails To service email with detected fail for analyze and
        /// corrections on the Way provide better services every time !!! This You can implement
        /// everywhere, !!! In Debug mode is written to Console, in Release will be sent email
        /// Empty Sender And Recipients set email for Service Recipient
        /// </summary>
        /// <param name="mailRequest"></param>
        /// <param name="sendImmediately"></param>
        public static string SendEmail(MailRequest mailRequest) {
            try {

                    MailMessage Email = new() { From = new MailAddress(mailRequest.Sender ?? ServerConfigSettings.EmailerSMTPLoginUsername) };

                    if (mailRequest.Recipients != null && mailRequest.Recipients.Any()) { mailRequest.Recipients.ForEach(email => { Email.To.Add(email); }); }
                    else { Email.To.Add(ServerConfigSettings.EmailerServiceEmailAddress); }

                    Email.Subject = mailRequest.Subject ?? ServerConfigSettings.SpecialServerServiceName;
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

                return DBResult.success.ToString();
            } catch (Exception ex) {
                return DBResult.error.ToString();
            }
        }

        /// <summary>
        /// Removes the whitespace from the String.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static string RemoveWhitespace(this string input) {
            return new string(input.ToCharArray()
                .Where(c => !Char.IsWhiteSpace(c))
                .ToArray());
        }


        /// <summary>
        /// Server Function For Running External Processes
        /// </summary>
        /// <param name="processDefinition">The process definition.</param>
        /// <returns></returns>
        public static bool RunProcess(ProcessClass processDefinition) {
            string resultOutput = "", resultError = "";

            try {
                using (Process proc = new Process()) {
                    proc.StartInfo.FileName = processDefinition.Command;
                    if (OperatingSystem.IsWindows()) { proc.StartInfo.WorkingDirectory = processDefinition.WorkingDirectory + "\\" ?? null; }
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
        /// Full Clear Folder
        /// </summary>
        /// <param name="FolderName">Name of the folder.</param>
        public static void ClearFolder(string FolderName) {
            if (Directory.Exists(FolderName)) Directory.Delete(FolderName, true);
            if (!Directory.Exists(FolderName)) CreatePath(FolderName);
        }


        /// <summary>
        /// Extension For Checking Operation System 
        /// of Server Running
        /// </summary>
        public static class OperatingSystem {
            public static bool IsWindows() =>
                RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

            public static bool IsMacOS() =>
                RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

            public static bool IsLinux() =>
                RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        }
    }
}