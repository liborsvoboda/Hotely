namespace UbytkacBackend.ControllersExtensions {

    /// <summary>
    /// Softwasre Triggers For Web and System Login/Visit History View
    /// </summary>
    internal class SoftwareTriggers {

        /// <summary>
        /// Trigger User Login History
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="userId">   </param>
        /// <param name="userName"> </param>
        public static async void WriteAppLogin(string ipAddress, int userId, string userName) {
            // Save new visit
            if (!string.IsNullOrWhiteSpace(userName)) {
                SystemLoginHistoryList record = new() { IpAddress = ipAddress, UserId = userId, UserName = userName, Timestamp = DateTimeOffset.Now.DateTime };
                var result = new hotelsContext().SystemLoginHistoryLists.Add(record);
                await result.Context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Trigger Web Guest Login History
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="guestId">  </param>
        /// <param name="email">    </param>
        public static async void WriteWebLogin(string ipAddress, int guestId, string email) {
            // Save new visit
            if (!string.IsNullOrWhiteSpace(email)) {
                GuestLoginHistoryList record = new() { IpAddress = ipAddress, GuestId = guestId, Email = email, Timestamp = DateTimeOffset.Now.DateTime };
                var result = new hotelsContext().GuestLoginHistoryLists.Add(record);
                await result.Context.SaveChangesAsync();
            }
        }
    }
}