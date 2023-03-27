using System;
using System.IO;
using System.Web.Script.Serialization;
using System.Collections.Specialized;
using System.Diagnostics;
using TravelAgencyAdmin.Classes;
using System.Windows;
using System.Threading;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Text;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using TravelAgencyAdmin.GlobalClasses;
using System.Windows.Controls;
using System.DirectoryServices.AccountManagement;
using System.Diagnostics.Eventing.Reader;
using System.Text.RegularExpressions;

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
            return exception != null ? string.Format("{0}: {1}\n{2}", msgCount, exception.Message, GetExceptionMessages(exception.InnerException, ++msgCount)) : string.Empty;
        }

        public static async void SaveSystemFailMessage(string message)
        {
            
        }


        public static string ReformatDate(string input)
        {
            try
            {
                return Regex.Replace(input,
                      "\\b(?<year>\\d{2,4})/(?<month>\\d{1,2})/(?<day>\\d{1,2})\\b",
                      "${year}-${month}-${day}",
                      RegexOptions.Multiline | RegexOptions.IgnoreCase,
                      TimeSpan.FromMilliseconds(1000));
            }
            catch (RegexMatchTimeoutException)
            {
                return input;
            }
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
            catch { }
            return advancedFilter;
        }

        public static string DBTranslation(string systemName)
        {
            string result = (App.appLanguage == "cs-CZ") ? App.LanguageList.Where(a => a.SystemName == systemName).Select(a => a.DescriptionCz).FirstOrDefault() : App.LanguageList.Where(a => a.SystemName == systemName).Select(a => a.DescriptionEn).FirstOrDefault();
            return (string.IsNullOrWhiteSpace(result)) ? systemName : result;
        }
    }
}
