using System;
using TravelAgencyAdmin.Classes;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Linq;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using TravelAgencyAdmin.Api;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net;
using TravelAgencyAdmin.GlobalClasses;
using log4net.Util;

namespace TravelAgencyAdmin.GlobalFunctions
{
    class SystemFunctions {
        /// <summary>
        /// Return Requested User or if not exist default DB parameter
        /// CamelCase Ignored
        /// </summary>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        public static async Task<string> ParameterCheck(string parameterName) {
            string result = null;
            try
            {
                result = App.ParameterList.FirstOrDefault(a => a.SystemName.ToLower() == parameterName.ToLower() && a.UserId == App.UserData.Authentification.Id).Value;
            }
            catch (Exception Ex) {
                App.ApplicationLogging(Ex, $"{await DBFunctions.DBTranslation("Missing User Parameter")} {App.UserData.Authentification.Id} {parameterName} {await DBFunctions.DBTranslation("Will used System Default")}");
                App.ApplicationLogging(Ex);
            }

            try
            {
                if (result == null)
                {
                    result = App.ParameterList.Where(a => a.SystemName.ToLower() == parameterName.ToLower() && a.UserId == null).Select(a => a.Value).FirstOrDefault();
                    if (result == null) App.ApplicationLogging(new Exception(), $"{await DBFunctions.DBTranslation("Missing Server Parameter")}: {parameterName}");
                }
                return result;
            }
            catch (Exception Ex) { return result; }
        }


        /// <summary>
        /// Mining All Exception Information
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="msgCount"></param>
        /// <returns></returns>
        public static string GetExceptionMessages(Exception exception, int msgCount = 1) {
            if (exception != null && App.IgnoredExceptionList.FirstOrDefault(a => a.ErrorNumber == exception.HResult.ToString() && a.Active == true) != null) return null;
            return exception != null ? string.Format("{0}: {1}\n{2}", msgCount, (exception.Message + Environment.NewLine + exception.HResult.ToString() + Environment.NewLine + exception.StackTrace + Environment.NewLine), GetExceptionMessages(exception.InnerException, ++msgCount)) : string.Empty;
        }


        /// <summary>
        /// return existing filter for saving to string in selected Page
        /// </summary>
        /// <param name="filterBox"></param>
        /// <returns></returns>
        public static string FilterToString(ComboBox filterBox) {
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
            catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
            return advancedFilter;
        }


        /// <summary>
        /// Global Method for Using API Urls as Unique Generic List for
        /// All Call - logic is All standartised DataPages
        /// </summary>
        /// <param name="listOnly"></param>
        /// <param name="translatedApiList"></param>
        /// <param name="exceptionApiList"></param>
        public static async Task<List<TranslatedApiList>> GetTranslatedApiList(bool listOnly, List<string> omitApiList = null) {
            List<TranslatedApiList> translatedApiList = new List<TranslatedApiList>();
            foreach (ApiUrls apiUrl in (ApiUrls[])Enum.GetValues(typeof(ApiUrls)))
            {
                if (omitApiList == null || (omitApiList != null && !omitApiList.Contains(apiUrl.ToString()))) {
                    if (!listOnly || (listOnly && apiUrl.ToString().EndsWith("List")))
                    {
                        string translatedApiUrl = await DBFunctions.DBTranslation(RemoveAppNamespaceFromString(apiUrl.ToString()));
                        translatedApiList.Add(new TranslatedApiList() { ApiTableName = RemoveAppNamespaceFromString(apiUrl.ToString()), Translate = translatedApiUrl });
                    }
                }
            }
            return translatedApiList;
        }


        /// <summary>
        /// Its Solution for this is a solution for demanding and multiplied servers 
        /// Or Running SHARP and Test System By One Backend Server Service
        /// API Urls with Namespaces in Name are for Backend model with More Same Database Schemas
        /// Backend Databases count in One Server Service is Unlimited
        /// </summary>
        /// <param name="stringForRemovenamespace"></param>
        public static string RemoveAppNamespaceFromString(string stringForRemovenamespace) {
            string appNameSpace = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace.Split('.')[0];
            return stringForRemovenamespace.Replace(appNameSpace,string.Empty);
        }
    }
}
