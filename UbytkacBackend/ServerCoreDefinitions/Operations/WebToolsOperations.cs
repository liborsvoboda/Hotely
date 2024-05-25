/*
* Server Core Web Helpers
* System Extensions for Correct Core working
* DataTypes Conversion Support, etc.
*/

namespace UbytkacBackend.ServerCoreStructure {

    /// <summary>
    /// Server Web Helpers for EASY working Here are extension for Database Model, WebSocket
    /// </summary>
    public static class WebToolsOperations {

        #region Controls for Managed Web Source Files Helper

        /// <summary>
        /// Saving Modified Managed Web Source Files to Dev and Startup Folders Used For JS,CSS Files
        /// </summary>
        /// <param name="hostingEnvironment">The hosting environment.</param>
        /// <param name="record">            The record.</param>
        /// <returns></returns>
        public static bool SaveWebSourceFile(ref Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, ref WebCoreFileList record) {
            try {
                FileOperations.CreatePath(Path.Combine(hostingEnvironment.WebRootPath, "metro", record.MetroPath.ToLower()));
                FileOperations.CreatePath(Path.Combine(ServerRuntimeData.Startup_path, "wwwroot", "metro", record.MetroPath.ToLower()));

                string fileExt = record.FileName.Split(".").Last();

                if (!string.IsNullOrWhiteSpace(record.GuestFileContent)) {
                    if (record.SpecificationType.ToLower().EndsWith("minjs")) {
                        record.GuestFileContent = NUglify.Uglify.Js(record.GuestFileContent).Code;
                    }
                    else if (record.SpecificationType.ToLower().EndsWith("mincss")) { record.GuestFileContent = NUglify.Uglify.Css(record.GuestFileContent).Code; }
                    File.WriteAllText(Path.Combine(hostingEnvironment.WebRootPath, "metro", record.MetroPath, DataOperations.RemoveWhitespace(record.FileName)), record.GuestFileContent, Encoding.UTF8);
                    File.WriteAllText(Path.Combine(ServerRuntimeData.Startup_path, "wwwroot", "metro", record.MetroPath, record.FileName), record.GuestFileContent, Encoding.UTF8);
                }

                if (!string.IsNullOrWhiteSpace(record.UserFileContent)) {
                    if (record.SpecificationType.ToLower().EndsWith("minjs")) {
                        record.UserFileContent = NUglify.Uglify.Js(record.UserFileContent).Code;
                    }
                    else if (record.SpecificationType.ToLower().EndsWith("mincss")) { record.UserFileContent = NUglify.Uglify.Css(record.UserFileContent).Code; }
                    File.WriteAllText(Path.Combine(hostingEnvironment.WebRootPath, "metro", record.MetroPath, DataOperations.RemoveWhitespace(record.FileName).Replace(fileExt, "user." + fileExt)), record.UserFileContent, Encoding.UTF8);
                    File.WriteAllText(Path.Combine(ServerRuntimeData.Startup_path, "wwwroot", "metro", record.MetroPath, DataOperations.RemoveWhitespace(record.FileName).Replace(fileExt, "user." + fileExt)), record.UserFileContent, Encoding.UTF8);
                }

                if (!string.IsNullOrWhiteSpace(record.AdminFileContent)) {
                    if (record.SpecificationType.ToLower().EndsWith("minjs")) {
                        record.AdminFileContent = NUglify.Uglify.Js(record.AdminFileContent).Code;
                    }
                    else if (record.SpecificationType.ToLower().EndsWith("mincss")) { record.AdminFileContent = NUglify.Uglify.Css(record.AdminFileContent).Code; }
                    File.WriteAllText(Path.Combine(hostingEnvironment.WebRootPath, "metro", record.MetroPath, DataOperations.RemoveWhitespace(record.FileName).Replace(fileExt, "admin." + fileExt)), record.AdminFileContent, Encoding.UTF8);
                    File.WriteAllText(Path.Combine(ServerRuntimeData.Startup_path, "wwwroot", "metro", record.MetroPath, DataOperations.RemoveWhitespace(record.FileName).Replace(fileExt, "admin." + fileExt)), record.AdminFileContent, Encoding.UTF8);
                }

                if (!string.IsNullOrWhiteSpace(record.ProviderContent)) {
                    if (record.SpecificationType.ToLower().EndsWith("minjs")) {
                        record.ProviderContent = NUglify.Uglify.Js(record.ProviderContent).Code;
                    }
                    else if (record.SpecificationType.ToLower().EndsWith("mincss")) { record.ProviderContent = NUglify.Uglify.Css(record.ProviderContent).Code; }
                    File.WriteAllText(Path.Combine(hostingEnvironment.WebRootPath, "metro", record.MetroPath, DataOperations.RemoveWhitespace(record.FileName).Replace(fileExt, "provider." + fileExt)), record.ProviderContent, Encoding.UTF8);
                    File.WriteAllText(Path.Combine(ServerRuntimeData.Startup_path, "wwwroot", "metro", record.MetroPath, DataOperations.RemoveWhitespace(record.FileName).Replace(fileExt, "provider." + fileExt)), record.ProviderContent, Encoding.UTF8);
                }

                return true;
            } catch (Exception Ex) { CoreOperations.SendEmail(new SendMailRequest() { Content = DataOperations.GetSystemErrMessage(Ex) }); }
            return false;
        }

        /// <summary>
        /// Delete Managed Web Source Files from Dev and Startup Folders Delete Minified and Full Script
        /// </summary>
        /// <param name="hostingEnvironment">The hosting environment.</param>
        /// <param name="record">            The record.</param>
        /// <returns></returns>
        public static bool DeleteWebSourceFile(ref Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, ref WebCoreFileList record) {
            try {
                string fileExt = record.FileName.Split(".").Last();
                FileOperations.DeleteFile(Path.Combine(hostingEnvironment.WebRootPath, "metro", record.MetroPath, record.FileName));
                FileOperations.DeleteFile(Path.Combine(ServerRuntimeData.Startup_path, "wwwroot", "metro", record.MetroPath, record.FileName));

                FileOperations.DeleteFile(Path.Combine(hostingEnvironment.WebRootPath, "metro", record.MetroPath, DataOperations.RemoveWhitespace(record.FileName).Replace(fileExt, "user." + fileExt)));
                FileOperations.DeleteFile(Path.Combine(ServerRuntimeData.Startup_path, "wwwroot", "metro", record.MetroPath, DataOperations.RemoveWhitespace(record.FileName).Replace(fileExt, "user." + fileExt)));

                FileOperations.DeleteFile(Path.Combine(hostingEnvironment.WebRootPath, "metro", record.MetroPath, DataOperations.RemoveWhitespace(record.FileName).Replace(fileExt, "admin." + fileExt)));
                FileOperations.DeleteFile(Path.Combine(ServerRuntimeData.Startup_path, "wwwroot", "metro", record.MetroPath, DataOperations.RemoveWhitespace(record.FileName).Replace(fileExt, "admin." + fileExt)));

                FileOperations.DeleteFile(Path.Combine(hostingEnvironment.WebRootPath, "metro", record.MetroPath, DataOperations.RemoveWhitespace(record.FileName).Replace(fileExt, "provider." + fileExt)));
                FileOperations.DeleteFile(Path.Combine(ServerRuntimeData.Startup_path, "wwwroot", "metro", record.MetroPath, DataOperations.RemoveWhitespace(record.FileName).Replace(fileExt, "provider." + fileExt)));

                return true;
            } catch (Exception Ex) { CoreOperations.SendEmail(new SendMailRequest() { Content = DataOperations.GetSystemErrMessage(Ex) }); }
            return false;
        }

        #endregion Controls for Managed Web Source Files Helper
    }
}