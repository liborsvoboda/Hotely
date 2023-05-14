using TravelAgencyAdmin.Api;
using TravelAgencyAdmin.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgencyAdmin.GlobalOperations {

    /// <summary>
    /// Centralised DBOperations as Load DB Congig, System Dials (Language, Params)
    /// Another Db functions As Saving System Loging, Language Dictionary Autofiling
    /// </summary>
    internal class DBOperations : MainWindow {

        /// <summary>
        /// Startup Load System Parameters ,Languages, System Controlling, Server Setting
        /// </summary>
        public static async void LoadStartupDBData() {
            App.IgnoredExceptionList = await ApiCommunication.GetApiRequest<List<IgnoredExceptionList>>(ApiUrls.IgnoredExceptionList, null, null);
            App.ParameterList = await ApiCommunication.GetApiRequest<List<ParameterList>>(ApiUrls.ParameterList, null, null);
            App.LanguageList = await ApiCommunication.GetApiRequest<List<LanguageList>>(ApiUrls.LanguageList, null, null);
        }

        /// <summary>
        /// Centralised Method for Refresh All UserData
        /// params, for correct App running.
        /// Thinking for remove and new Load
        /// Actualy limited by DebugingHelpSetting
        /// </summary>
        public static async void LoadOrRefreshUserData() {
            App.ParameterList = await ApiCommunication.GetApiRequest<List<ParameterList>>(ApiUrls.ParameterList, App.UserData.Authentification.Id.ToString(), App.UserData.Authentification.Token);
        }

        /// <summary>
        /// Save Exception to DB Fail List (System Log)
        /// Write to System Logger
        /// </summary>
        /// <param name="message"></param>
        public static async void SaveSystemFailMessage(string message) {
            if (ServiceRunning && UserLogged)
            {
                if (string.IsNullOrWhiteSpace(message)) return;

                SystemFailList systemFailList = new SystemFailList() { UserId = App.UserData.Authentification.Id, UserName = App.UserData.UserName, Message = message, TimeStamp = DateTimeOffset.Now.DateTime };
                string json = JsonConvert.SerializeObject(systemFailList);
                StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                await ApiCommunication.PutApiRequest(ApiUrls.SystemFailList, httpContent, null, App.UserData.Authentification.Token);
            }
        }

        /// <summary>
        /// Centralised Method for Translating by DB Dictionary
        /// Service insert the news words for translate (After translate request)
        /// to Database Automaticaly with Empty Translate.
        /// Service return translate if is possible or requested word send back
        /// CamelCase ignored
        /// </summary>
        /// <param name="systemName"></param>
        /// <param name="comaList"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        public static async Task<string> DBTranslation(string systemName, bool comaList = false, string lang = null) {
            bool dictionaryUpdated = false;
            string result = "", translated = "";
            if (comaList)
            {
                systemName.Split(',').ToList().ForEach(async word =>
                {
                    try
                    {
                        if (!string.IsNullOrWhiteSpace(word))
                        {
                            if (App.LanguageList.Where(a => a.SystemName.ToLower() == word.ToLower()).Count() == 0)
                            {
                                dictionaryUpdated = true;
                                LanguageList newWord = new LanguageList() { SystemName = word, DescriptionCz = "", DescriptionEn = "", UserId = 1 };
                                string json = JsonConvert.SerializeObject(newWord);
                                StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                                _ = await ApiCommunication.PutApiRequest(ApiUrls.LanguageList, httpContent, null, App.UserData.Authentification.Token);

                                result += word + ",";
                            } else {
                                translated = ((App.appLanguage == "cs-CZ" && lang == null) || lang == "cz") ? App.LanguageList.Where(a => a.SystemName.ToLower() == word.ToLower()).Select(a => a.DescriptionCz).FirstOrDefault() : App.LanguageList.Where(a => a.SystemName.ToLower() == word.ToLower()).Select(a => a.DescriptionEn).FirstOrDefault();
                                result += (string.IsNullOrWhiteSpace(translated) ? word : translated) + ",";
                            }
                        }
                    } catch (Exception ex) { result += word + ","; }
                });
            }
            else
            {
                try
                {
                    if (App.LanguageList.Where(a => a.SystemName.ToLower() == systemName.ToLower()).Count() == 0)
                    {
                        dictionaryUpdated = true;

                        LanguageList newWord = new LanguageList() { SystemName = systemName, DescriptionCz = "", DescriptionEn = "", UserId = 1 };
                        string json = JsonConvert.SerializeObject(newWord);
                        StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                        _ = await ApiCommunication.PutApiRequest(ApiUrls.LanguageList, httpContent, null, App.UserData.Authentification.Token);
                        result = systemName;
                    }
                    else
                    {
                        translated = ((App.appLanguage == "cs-CZ" && lang == null) || lang == "cz") ? App.LanguageList.Where(a => a.SystemName.ToLower() == systemName.ToLower()).Select(a => a.DescriptionCz).FirstOrDefault() : App.LanguageList.Where(a => a.SystemName.ToLower() == systemName.ToLower()).Select(a => a.DescriptionEn).FirstOrDefault();
                        result = string.IsNullOrWhiteSpace(translated) ? systemName : translated;
                    }
                }
                catch (Exception ex) { result = systemName; }
            }

            if (dictionaryUpdated) { App.LanguageList = await ApiCommunication.GetApiRequest<List<LanguageList>>(ApiUrls.LanguageList, null, App.UserData.Authentification.Token); }

            return result;
        }
    }
}