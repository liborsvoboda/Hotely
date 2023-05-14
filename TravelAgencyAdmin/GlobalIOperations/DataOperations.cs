using TravelAgencyAdmin.Api;
using TravelAgencyAdmin.Classes;

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GlobalClasses;

namespace TravelAgencyAdmin.GlobalOperations {

    /// <summary>
    /// Centralized DataOperations as Cleaning dataset
    /// Language Dictionary Auto filing
    /// </summary>
    internal class DataOperations : MainWindow {


        /// <summary>
        /// !!! SYSTEM RULE: ClassList with joining fields names must be null able before API operation
        /// !!! ClassName must contain: "Extended" WORD
        /// Extension field in Class - Dataset must be set as null before Database Operation
        /// else is joining to other dataset is valid and can be blocked by fail key
        /// Its Check Extended in ClassName - SYSTEM RULE
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static void NullSetInExtensionFields<T>(ref T dataset) {
            if (dataset.GetType().Name.ToLower().Contains("extended"))
            {
                foreach (PropertyInfo prop in dataset.GetType().GetProperties())
                { if (prop.DeclaringType.Name.Contains("Extended")) prop.SetValue(dataset, null); }
            }
            
        }


        /// <summary>
        /// Global Method for Using API Uris as Unique Generic List for
        /// All API Call - logic is All standardized not only DataPages as automated Logic
        /// </summary>
        /// <param name="listOnly"></param>
        /// <param name="omitApiList"></param>
        /// <returns></returns>
        public static async Task<List<TranslatedApiList>> GetTranslatedApiList(bool listOnly, List<string> omitApiList = null) {
            List<TranslatedApiList> translatedApiList = new List<TranslatedApiList>();
            foreach (ApiUrls apiUrl in (ApiUrls[])Enum.GetValues(typeof(ApiUrls)))
            {
                if (omitApiList == null || (omitApiList != null && !omitApiList.Contains(apiUrl.ToString())))
                {
                    if (!listOnly || (listOnly && apiUrl.ToString().EndsWith("List")))
                    {
                        string translatedApiUrl = await DBOperations.DBTranslation(SystemOperations.RemoveAppNamespaceFromString(apiUrl.ToString()));
                        translatedApiList.Add(new TranslatedApiList() { ApiTableName = SystemOperations.RemoveAppNamespaceFromString(apiUrl.ToString()), Translate = translatedApiUrl });
                    }
                }
            }
            return translatedApiList;
        }


        /// <summary>
        /// Return Requested User or if not exist default DB parameter
        /// CamelCase Ignored
        /// </summary>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        public static async Task<string> ParameterCheck(string parameterName) {
            string result = null;
            try
            { result = App.ParameterList.Where(a => a.SystemName.ToLower() == parameterName.ToLower() && a.UserId == App.UserData.Authentification.Id).Select(a => a.Value).FirstOrDefault(); }
            catch (Exception Ex) { App.ApplicationLogging(Ex); }

            try
            {
                if (result == null)
                {
                    result = App.ParameterList.Where(a => a.SystemName.ToLower() == parameterName.ToLower() && a.UserId == null).Select(a => a.Value).FirstOrDefault();
                    if (result == null) App.ApplicationLogging(new Exception(), $"{await DBOperations.DBTranslation("Missing Server Parameter")}: {parameterName}");
                }
                return result;
            }
            catch (Exception Ex) { return result; }
        }
    }
}