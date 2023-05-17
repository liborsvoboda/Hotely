namespace TravelAgencyBackEnd {

    /// <summary>
    /// All Server Definitions of Database Operation method
    /// </summary>
    internal class DBOperations {

        #region StartupAndReloadDefinitions

        /// <summary>
        /// Method for All Server Defined Table for Local Using As OffLine AutoUpdated Tables
        /// </summary>
        /// <param name="onlyThis"></param>
        public static void LoadStaticDbDials(ServerLocalDbDials? onlyThis = null) {
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
                if (onlyThis is null || onlyThis == ServerLocalDbDials.LanguageList) Program.ServerDBLanguageList = new hotelsContext().LanguageLists.ToList();
            }
        }

        #endregion StartupAndReloadDefinitions

        #region Public definitions for standard using

        /// <summary>
        /// Trigger User Login History
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="userId">   </param>
        /// <param name="userName"> </param>
        public static async void WriteAppLogin(string ipAddress, int userId, string userName) {
            // Save new visit
            if (!string.IsNullOrWhiteSpace(userName))
            {
                AdminLoginHistoryList record = new() { IpAddress = ipAddress, UserId = userId, UserName = userName, Timestamp = DateTimeOffset.Now.DateTime };
                var result = new hotelsContext().AdminLoginHistoryLists.Add(record);
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
            if (!string.IsNullOrWhiteSpace(email))
            {
                GuestLoginHistoryList record = new() { IpAddress = ipAddress, GuestId = guestId, Email = email, Timestamp = DateTimeOffset.Now.DateTime };
                var result = new hotelsContext().GuestLoginHistoryLists.Add(record);
                await result.Context.SaveChangesAsync();
            }
        }

        public static string DBTranslate(string word, string language = "cz") {
            return Program.UseDBLocalAutoupdatedDials ? DBTranslateOffline(word, language) : DBTranslateOnline(word, language);
        }

        #endregion Public definitions for standard using

        #region Private or Online/OffLine Definitions

        /// <summary>
        /// Database LanuageList for Off-line Using Definitions
        /// </summary>
        /// <param name="word">    </param>
        /// <param name="language"></param>
        /// <returns></returns>
        private static string DBTranslateOffline(string word, string language = "cz") {
            string result;
            int index = BackendServer.ServerRuntimeData.LocalDBTableList.FindIndex(a => a.GetType() == new List<LanguageList>().GetType());

            //Check Exist AND Insert New
            try {
                if (!((List<LanguageList>)BackendServer.ServerRuntimeData.LocalDBTableList[index]).Where(a => a.SystemName.ToLower() == word.ToLower()).Any()) {
                    result = word;
                    LanguageList newWord = new() { SystemName = word, DescriptionCz = "", DescriptionEn = "", UserId = 1 };
                    new hotelsContext().LanguageLists.Add(newWord).Context.SaveChanges();
                    LoadStaticDbDials(ServerLocalDbDials.LanguageList);
                    return result;
                }
            } catch { }

            //Return From List
            if (language == "cz") result = ((List<LanguageList>)BackendServer.ServerRuntimeData.LocalDBTableList[index]).Where(a => a.SystemName.ToLower() == word.ToLower()).Select(a => a.DescriptionCz).FirstOrDefault();
            else result = ((List<LanguageList>)BackendServer.ServerRuntimeData.LocalDBTableList[index]).Where(a => a.SystemName.ToLower() == word.ToLower()).Select(a => a.DescriptionEn).FirstOrDefault();

            if (string.IsNullOrWhiteSpace(result)) { result = word; }
            return result;
        }

        /// <summary>
        /// Database LanuageList for On-line Using Definitions
        /// </summary>
        /// <param name="word">    </param>
        /// <param name="language"></param>
        /// <returns></returns>
        private static string DBTranslateOnline(string word, string language = "cz") {
            string result;

            //Check Exist AND Insert New
            try {
                if (!new hotelsContext().LanguageLists.Where(a => a.SystemName.ToLower() == word.ToLower()).Any()) {
                    result = word;
                    LanguageList newWord = new() { SystemName = word, DescriptionCz = "", DescriptionEn = "", UserId = 1 };
                    new hotelsContext().LanguageLists.Add(newWord).Context.SaveChanges();
                    return result;
                }
            } catch { }

            //Return From List
            if (language == "cz") result = new hotelsContext().LanguageLists.Where(a => a.SystemName.ToLower() == word.ToLower()).Select(a => a.DescriptionCz).FirstOrDefault();
            else result = new hotelsContext().LanguageLists.Where(a => a.SystemName.ToLower() == word.ToLower()).Select(a => a.DescriptionEn).FirstOrDefault();

            if (string.IsNullOrWhiteSpace(result)) { result = word; }
            return result;
        }

        #endregion Private or Online/OffLine Definitions
    }
}