using EasyITSystemCenter.GlobalClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Controls;

namespace EasyITSystemCenter.GlobalOperations {

    /// <summary>
    /// Centralized DataOperations as Cleaning dataset Language Dictionary Auto filing
    /// </summary>
    public static class DataOperations {

        /// <summary> Convert Dictionary<string,string> To Json Suportted Values Bool, Int, String
        /// </summary> <returns></returns>
        public static string ConvertDataSetToJson(Dictionary<string, string> keyList) {
            bool tempBool; int tempInt = 0;
            Dictionary<string, object> exportJsonList = new Dictionary<string, object>();
            keyList.ToList().ForEach(key => {
                string valueType = bool.TryParse(key.Value, out tempBool) ? "bool" : int.TryParse(key.Value, out tempInt) ? "int" : "string";
                exportJsonList.Add(key.Key, (valueType == "bool" ? (object)tempBool : valueType == "int" ? (object)tempInt : (object)key.Value));
            });
            return new JavaScriptSerializer().Serialize(exportJsonList);
        }

        /// <summary>
        /// !!! SYSTEM RULE: ClassList with joining fields names must be nullable before API
        /// operation !!! ClassName must contain: "Extended" WORD Extension field in Class - Dataset
        /// must be set as null before Database Operation else is joining to other dataset is valid
        /// and can be blocked by fail key Its Check Extended in ClassName - SYSTEM RULE
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static void NullSetInExtensionFields<T>(ref T dataset) {
            if (dataset.GetType().Name.ToLower().Contains("extended")) {
                foreach (PropertyInfo prop in dataset.GetType().GetProperties()) { if (prop.DeclaringType.Name.Contains("Extended")) prop.SetValue(dataset, null); }
            }
        }


        /// <summary>
        /// Object To Json Serializer
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ObjectToJson(this object obj) {
            return JsonSerializer.Serialize(obj, App.appRuntimeData.JsonSerializeOptions);
        }


        /// <summary>
        /// Return Requested User or if not exist default DB parameter CamelCase Ignored
        /// </summary>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        public static async Task<string> ParameterCheck(string parameterName) {
            string result = null;
            try {
                if (App.UserData.Authentification != null) {
                    result = App.ParameterList.Where(a => a.SystemName.ToLower() == parameterName.ToLower() && a.UserId == App.UserData.Authentification.Id).Select(a => a.Value).FirstOrDefault();
                }
            } catch (Exception Ex) { App.ApplicationLogging(Ex); }

            try {
                if (result == null) {
                    result = App.ParameterList.Where(a => a.SystemName.ToLower() == parameterName.ToLower() && a.UserId == null).Select(a => a.Value).FirstOrDefault();
                    if (result == null) App.ApplicationLogging(new Exception(), $"{await DBOperations.DBTranslation("Missing Server Parameter")}: {parameterName}");
                }
                return result;
            } catch { return result; }
        }


        /// <summary>
        /// Convert String To ByteArray
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static byte[] StringToByteArray(string input) {
            return Encoding.UTF8.GetBytes(input);
        }



        /// <summary>
        /// Convert UTF8 to UNICODE
        /// </summary>
        /// <param name="utf8String"></param>
        /// <returns></returns>
        public static string Utf8toUnicode(this string utf8String) {
            byte[] utf8Bytes = new byte[utf8String.Length];
            for (int i = 0; i < utf8String.Length; ++i) { utf8Bytes[i] = (byte)utf8String[i]; }
            return Encoding.UTF8.GetString(utf8Bytes, 0, utf8Bytes.Length);
        }

        /// <summary>
        /// Remove Diactritics
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static string RemoveDiacritism(string Text) {
            string stringFormD = Text.Normalize(NormalizationForm.FormD);
            StringBuilder retVal = new StringBuilder();
            for (int index = 0; index < stringFormD.Length; index++) {
                if (System.Globalization.CharUnicodeInfo.GetUnicodeCategory(stringFormD[index]) != System.Globalization.UnicodeCategory.NonSpacingMark)
                    retVal.Append(stringFormD[index]);
            }
            return retVal.ToString().Normalize(NormalizationForm.FormC);
        }

        /// <summary>
        /// Convert Generic Tabla To Standard Table For Use Standard System Fields
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static List<GenericTable> ConvertGenericClassListToStandard<T>(T data) {
            return JsonSerializer.Deserialize<List<GenericTable>>(JsonSerializer.Serialize(data));
        }

        /// <summary>
        /// Convert Generic Tabla To Standard Table For Use Standard System Fields
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static GenericTable ConvertGenericClassToStandard<T>(T data) {
            return JsonSerializer.Deserialize<GenericTable>(JsonSerializer.Serialize(data));
        }

        /// <summary>
        /// Convert String to Enum
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ToEnum<T>(this string value) {
            try {
                Type enumType = typeof(T);
                if (!enumType.IsEnum) {
                    App.ApplicationLogging(new Exception("DataOperation ToEnum Method DataOperations  T ToEnum<T>: T must be an Enumeration type." + enumType.ToString()));
                }
            } catch (Exception Ex) { App.ApplicationLogging(Ex); }
            return (T)Enum.Parse(typeof(T), value, true);
        }


    
    }
}