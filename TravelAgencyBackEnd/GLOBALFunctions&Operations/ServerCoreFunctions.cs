using System.Net;
using System.Net.Mail;

namespace UbytkacBackend {

    internal class ServerCoreFunctions {

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
    }
}