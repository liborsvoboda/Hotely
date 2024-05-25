using System.Data;
using System.Text.RegularExpressions;

namespace UbytkacBackend.ServerCoreStructure {

    internal static class DataOperations {



        public static T ToEnum<T>(this string value) {
            Type enumType = typeof(T);
            if (!enumType.IsEnum) {
                CoreOperations.SendEmail(new SendMailRequest() { Content = "DataOperation ToEnum Method line 22: T must be an Enumeration type." + enumType.ToString() });
            }
            return (T)Enum.Parse(typeof(T), value, true);
        }


        /// <summary>
        /// Create Object Type By Type Name
        /// Its need For Generic APi For Full Database Support
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        public static object? CreateObjectTypeByTypeName(string className) {
            var assembly = Assembly.GetExecutingAssembly();
            var type = assembly.GetTypes().First(t => t.Name == className);
            return Activator.CreateInstance(type);
        }



        /// <summary>
        /// Change First Character of String
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static string? FirstCharToLowerCase(this string? str) {
            if (!string.IsNullOrEmpty(str) && char.IsUpper(str[0]))
                return str.Length == 1 ? char.ToLower(str[0]).ToString() : char.ToLower(str[0]) + str[1..];

            return str;
        }

        /// <summary>
        /// Unicodes to ut f8.
        /// </summary>
        /// <param name="strFrom">The string from.</param>
        /// <returns></returns>
        public static string UnicodeToUTF8(string strFrom) {
            byte[] bytes = Encoding.Default.GetBytes(strFrom);
            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// Mined-ed Error Message For Answer in API Error Response with detailed info about problem
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="msgCount"> </param>
        /// <returns></returns>
        public static string GetUserApiErrMessage(Exception exception, int msgCount = 1) {
            return exception != null ? string.Format("{0}: {1}\n{2}", msgCount, exception.TargetSite?.ReflectedType?.FullName + Environment.NewLine + exception.Message, GetUserApiErrMessage(exception.InnerException, ++msgCount)) : string.Empty;
        }

        /// <summary>
        /// Mined-ed Error Message For System Save to Database For Simple Solving Problem
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="msgCount"> </param>
        /// <returns></returns>
        public static string GetSystemErrMessage(Exception exception, int msgCount = 1) {
            return exception != null ? string.Format("{0}: {1}\n{2}", msgCount, (exception.TargetSite?.ReflectedType?.FullName + Environment.NewLine + exception.Message + Environment.NewLine + exception.StackTrace + Environment.NewLine), GetSystemErrMessage(exception.InnerException, ++msgCount)) : string.Empty;
        }

        /// <summary>
        /// Randoms the string.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public static string RandomString(int length) {
            Random random = new Random();
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// Separate ByteArray from 64 encode file For inserted file types
        /// </summary>
        /// <param name="strContent">Content of the string.</param>
        /// <returns></returns>
        public static byte[] GetByteArrayFrom64Encode(string strContent) {
            try {
                var base64Data = Regex.Match(strContent, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
                base64Data = base64Data.Length > 0 ? base64Data : Regex.Match(strContent, @"data:text/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
                base64Data = base64Data.Length > 0 ? base64Data : Regex.Match(strContent, @"data:video/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
                base64Data = base64Data.Length > 0 ? base64Data : Regex.Match(strContent, @"data:application/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
                return Convert.FromBase64String(base64Data);
            } catch { }
            byte[] result = new byte[] { };
            return result;
        }

        /// <summary>
        /// Determines whether [is valid email] [the specified email].
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns><c>true</c> if [is valid email] [the specified email]; otherwise, <c>false</c>.</returns>
        public static bool IsValidEmail(string email) {
            var trimmedEmail = email.Trim();
            if (trimmedEmail.EndsWith(".")) { return false; }
            try {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            } catch { return false; }
        }

        /// <summary>
        /// Removes the whitespace from the String.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static string RemoveWhitespace(this string input) {
            return new string(input.ToCharArray()
                .Where(c => !Char.IsWhiteSpace(c))
                .ToArray());
        }

        /// <summary>
        /// MarkDown Global Resolve End Spaces Characters On Insert/Update API CALS of Managing
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static string MarkDownLineEndSpacesResolve(string input) {
            input.Split(new[] { '\r', '\n' }).ToList().ForEach(line => { line = line.TrimEnd() + "    "; });
            return input;
        }

        /// <summary>
        /// Convert String to UTF8
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string StringToUTF8(this string text) {
            return Encoding.UTF8.GetString(Encoding.Default.GetBytes(text));
        }

        /// <summary>
        /// Convert UTF8 to UNICODE
        /// </summary>
        /// <param name="utf8String"></param>
        /// <returns></returns>
        public static string Utf8toUnicode(this string utf8String) {
            // copy the string as UTF-8 bytes.
            byte[] utf8Bytes = new byte[utf8String.Length];
            for (int i = 0; i < utf8String.Length; ++i) {
                //Debug.Assert( 0 <= utf8String[i] && utf8String[i] <= 255, "the char must be in byte's range");
                utf8Bytes[i] = (byte)utf8String[i];
            }

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
        public static GenericTable ConvertGenericClassToStandard<T>(T data) {
            return JsonSerializer.Deserialize<GenericTable>(JsonSerializer.Serialize(data));
        }
    }
}