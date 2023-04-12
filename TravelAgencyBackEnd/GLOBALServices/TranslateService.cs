using System;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using TravelAgencyBackEnd.DBModel;

namespace TravelAgencyBackEnd.Services
{
    public static class TranslateService
    {

        public static string DBTranslate(string word, string language = "cz")
        {
            string result;
            if (language == "cz")
                result = new hotelsContext().LanguageLists.FirstOrDefault(a => a.SystemName == word).DescriptionCz;
            else
                result = new hotelsContext().LanguageLists.FirstOrDefault(a => a.SystemName == word).DescriptionEn;

            if (result == null)
            {
                result = word;
                LanguageList newWord = new LanguageList() { SystemName = word, DescriptionCz = "", DescriptionEn = "", UserId = 1 };
                new hotelsContext().LanguageLists.Add(newWord).Context.SaveChanges();
            }
            return result;
        }
    }
}
