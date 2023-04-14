using System.Text;
using System.IO;
using System;
using TravelAgencyBackEnd.DBModel;
using System.Linq;

namespace TravelAgencyBackEnd
{
    class DBOperations
    {

        public static async void WriteAppLogin(string ipAddress, int userId, string userName) {
            // Save new visit
            if (!string.IsNullOrWhiteSpace(userName))
            {
                AdminLoginHistoryList record = new() { IpAddress = ipAddress, UserId = userId, UserName = userName, Timestamp = DateTimeOffset.Now.DateTime };
                var result = new hotelsContext().AdminLoginHistoryLists.Add(record);
                await result.Context.SaveChangesAsync();
            }
        }

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
            string result;

            //Check Exist AND Insert New
            try
            {
                result = new hotelsContext().LanguageLists.FirstOrDefault(a => a.SystemName == word).DescriptionCz;
            }
            catch
            {
                result = word;
                LanguageList newWord = new LanguageList() { SystemName = word, DescriptionCz = "", DescriptionEn = "", UserId = 1 };
                new hotelsContext().LanguageLists.Add(newWord).Context.SaveChanges();
                return result;
            }

            if (language == "cz")
                result = new hotelsContext().LanguageLists.FirstOrDefault(a => a.SystemName == word).DescriptionCz;
            else
                result = new hotelsContext().LanguageLists.FirstOrDefault(a => a.SystemName == word).DescriptionEn;

            if (string.IsNullOrWhiteSpace(result)) { result = word; }
            return result;
        }
    }
}