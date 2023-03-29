using System;
using TravelAgencyAdmin.Classes;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Linq;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using TravelAgencyAdmin.Api;

namespace TravelAgencyAdmin.GlobalFunctions
{
    class SystemFunctions
    {
        /// <summary>
        /// Return User or default DB parameter
        /// </summary>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        public static string ParameterCheck(string parameterName)
        {
            string result = string.Empty;
            try {
                result = App.Parameters.Where(a => a.SystemName == parameterName && a.UserId == App.UserData.Authentification.Id).Select(a => a.Value).FirstOrDefault();

                if (result == null)
                {
                    SaveSystemFailMessage($"Parameter Name {parameterName} for userId {App.UserData.Authentification.Id} and userName {App.UserData.UserName} is missing");
                    result = App.Parameters.Where(a => a.SystemName == parameterName && a.UserId == null).Select(a => a.Value).FirstOrDefault();
                    if (result == null) SaveSystemFailMessage($"Parameter Name {parameterName} for NULL userId is missing");
                    else return result;

                    return result;
                }
                else { return result; }
            }catch { return result; }
        }

        public static string GetExceptionMessages(Exception exception, int msgCount = 1)
        {
            return exception != null ? string.Format("{0}: {1}\n{2}", msgCount, (exception.Message + Environment.NewLine + exception.StackTrace + Environment.NewLine), GetExceptionMessages(exception.InnerException, ++msgCount)) : string.Empty;
        }

        public static async void SaveSystemFailMessage(string message)
        {
            SystemFailList systemFailList = new SystemFailList() { UserId =  App.UserData.Authentification.Id, UserName = App.UserData.UserName, Message = message };
            string json = JsonConvert.SerializeObject(systemFailList);
            StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            await ApiCommunication.PutApiRequest(ApiUrls.SystemFailList, httpContent, null, App.UserData.Authentification.Token);
            
        }


        /// <summary>
        /// return existing filter for saving to string in selected Page
        /// </summary>
        /// <param name="filterBox"></param>
        /// <returns></returns>
        public static string FilterToString(ComboBox filterBox)
        {
            string advancedFilter = null;
            int index = 0;
            try
            {
                foreach (WrapPanel filterItem in filterBox.Items)
                {
                    if (index > 0)
                    {
                        if (filterItem.Name.Split('_')[0] == "condition")
                        {
                            index = 0;
                            foreach (var item in filterItem.Children)
                            {
                                if (index == 1) { advancedFilter += "[!]" + ((Label)item).Content; }
                                if (index > 1) { advancedFilter += "{!}" + ((Label)item).Content; }
                                index++;
                            }
                        }
                        else if (filterItem.Name.Split('_')[0] == "filter")
                        {
                            advancedFilter += "[!]" + ((ComboBox)filterItem.Children[0]).SelectionBoxItem;
                            advancedFilter += "{!}" + ((ComboBox)filterItem.Children[2]).SelectionBoxItem;
                            advancedFilter += "{!}'" + ((TextBox)filterItem.Children[3]).Text + "'";
                        }
                        index = 1;
                    }
                    index++;
                }
            }
            catch (Exception autoEx) {SystemFunctions.SaveSystemFailMessage(SystemFunctions.GetExceptionMessages(autoEx));}
            return advancedFilter;
        }

        public static string DBTranslation(string systemName, bool comaList = false, string lang = null)
        {
            string result = "";
            if (comaList)
            {
                systemName.Split(',').ToList().ForEach(word =>
                {
                    if (string.IsNullOrWhiteSpace(word))
                        result += ((App.appLanguage == "cs-CZ" && lang == null) || lang == "cz") ? App.LanguageList.Where(a => a.SystemName == word).Select(a => a.DescriptionCz).FirstOrDefault() : App.LanguageList.Where(a => a.SystemName == word).Select(a => a.DescriptionEn).FirstOrDefault() + ",";
                });
            }
            else
            { result = ((App.appLanguage == "cs-CZ" && lang == null) || lang == "cz") ? App.LanguageList.Where(a => a.SystemName == systemName).Select(a => a.DescriptionCz).FirstOrDefault() : App.LanguageList.Where(a => a.SystemName == systemName).Select(a => a.DescriptionEn).FirstOrDefault(); }
            return (string.IsNullOrWhiteSpace(result)) ? systemName : result;
        }
    }
}
