using HtmlAgilityPack;
using System.IO.Compression;
using UbytkacBackend.ServerCoreStructure;

namespace UbytkacBackend.ServerCoreDBSettings {

    /*
    /// <summary>
    /// Server Root Controller Used by Server Webpages
    /// </summary>
    /// <seealso cref="ControllerBase"/>
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ServerWebPagesExportsApi : ControllerBase {

        /// <summary>
        /// Server Root "/" Redirection to "server" Folder
        /// </summary>
        /// <returns></returns>
        [HttpGet("/")]
        public IActionResult Index() {
            return new RedirectResult("/Index");
        }
    }*/

    [Authorize]
    [ApiController]
    [Route("ServerCoreExport")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ServerWebPagesExportApi : ControllerBase {

        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        public ServerWebPagesExportApi(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment) {
            _hostingEnvironment = hostingEnvironment;
        }



        [HttpGet("/ServerCoreExport/ExportStaticWebPages")]
        public async Task<IActionResult> ExportStaticWebPages() {
            try {
                string data = null;
                //TODO copy wwwroot

                ZipFile.CreateFromDirectory(Path.Combine(ServerRuntimeData.Startup_path, "Export", "Webpages"), Path.Combine(ServerRuntimeData.Startup_path, "Export", "Webpages.zip"));
                var zipData = await System.IO.File.ReadAllBytesAsync(Path.Combine(ServerRuntimeData.Startup_path, "Export", "Webpages.zip"));

                if (data != null) { return File(zipData, "application/x-zip-compressed", "Webpages.zip"); }
                else { return BadRequest(new { message = DbOperations.DBTranslate("BadRequest", "en") }); }
            } catch (Exception ex) { return BadRequest(new { message = DataOperations.GetSystemErrMessage(ex) }); }
        }

        /// <summary>
        /// Update Translation Table By All Tables and Field Names For Export Offline Language
        /// Dictionary CZ for System
        /// </summary>
        /// <returns></returns>
        [HttpGet("/ServerCoreExport/XamlCz")]
        public async Task<IActionResult> ExportXamlCz() {
            try {
                new hotelsContext().EasyITCenterCollectionFromSql<CustomString>("EXEC SpOperationFillTranslationTableList;");

                List<SystemTranslationList> data = null;
                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) { data = new hotelsContext().SystemTranslationLists.OrderBy(a => a.SystemName).ToList(); }

                string xmlExport = "<ResourceDictionary\r\n    xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"\r\n    xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"\r\n    xmlns:system=\"clr-namespace:System;assembly=mscorlib\">";

                data.ForEach(translation => {
                    if (!translation.SystemName.Any(Char.IsWhiteSpace)) { xmlExport += Environment.NewLine + "<system:String x:Key=\"" + DataOperations.FirstCharToLowerCase(translation.SystemName) + "\" xml:space=\"preserve\">" + (translation.DescriptionCz != null && translation.DescriptionCz.Length > 0 ? translation.DescriptionCz : translation.SystemName) + "</system:String>"; }
                });
                xmlExport += Environment.NewLine + "</ResourceDictionary>";

                return File(Encoding.UTF8.GetBytes(xmlExport), "application/xml", "StringResources.cs-CZ.xaml");
            } catch (Exception ex) { return BadRequest(new { message = DataOperations.GetSystemErrMessage(ex) }); }
        }

        /// <summary>
        /// Update Translation Table By All Tables and Field Names For Export Offline Language
        /// Dictionary EN for System
        /// </summary>
        /// <returns></returns>
        [HttpGet("/ServerCoreExport/XamlEn")]
        public async Task<IActionResult> ExportXamlEn() {
            try {
                new hotelsContext().EasyITCenterCollectionFromSql<CustomString>("EXEC SpOperationFillTranslationTableList;");

                List<SystemTranslationList> data = null;
                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) { data = new hotelsContext().SystemTranslationLists.OrderBy(a => a.SystemName).ToList(); }

                string xmlExport = "<ResourceDictionary\r\n    xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"\r\n    xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"\r\n    xmlns:system=\"clr-namespace:System;assembly=mscorlib\">";

                data.ForEach(translation => {
                    if (!translation.SystemName.Any(Char.IsWhiteSpace)) { xmlExport += Environment.NewLine + "<system:String x:Key=\"" + DataOperations.FirstCharToLowerCase(translation.SystemName) + "\" xml:space=\"preserve\">" + (translation.DescriptionEn != null && translation.DescriptionEn.Length > 0 ? translation.DescriptionEn : translation.SystemName) + "</system:String>"; }
                });
                xmlExport += Environment.NewLine + "</ResourceDictionary>";

                return File(Encoding.UTF8.GetBytes(xmlExport), "application/xml", "StringResources.xaml");
            } catch (Exception ex) { return BadRequest(new { message = DataOperations.GetSystemErrMessage(ex) }); }
        }



        /// <summary>
        /// Minimal Export of Page for Running
        /// on every Web servers Without Needs anythink
        /// </summary>
        /// <returns></returns>
        [HttpGet("/ServerCoreExport/ExportMinimalStaticWebPages")]
        public async Task<IActionResult> ExportMinimalStaticWebPages() {
            try {

                FileOperations.CreatePath(Path.Combine(ServerRuntimeData.Startup_path, "Export"));
                FileOperations.ClearFolder(Path.Combine(ServerRuntimeData.Startup_path, "Export"));
                FileOperations.CreatePath(Path.Combine(ServerRuntimeData.Startup_path, "Export", "Webpages", "metro", "managed", "storage"));
                FileOperations.CopyDirectory(Path.Combine(ServerRuntimeData.Startup_path, ServerConfigSettings.DefaultStaticWebFilesFolder, "metro", "managed", "storage"), Path.Combine(ServerRuntimeData.Startup_path, "Export", "Webpages", "metro", "managed", "storage"));

                string json = System.IO.File.ReadAllText(Path.Combine(ServerRuntimeData.Startup_path, "Export", "Webpages", "metro", "managed", "storage", "globalStorage.js"));
                FileOperations.WriteToFile(Path.Combine(ServerRuntimeData.Startup_path, "Export", "Webpages", "metro", "managed", "storage", "globalStorage.js"), json.Replace("window.location.origin",ServerConfigSettings.ServerPublicUrl));

                HtmlWeb hw = new HtmlWeb();
                HtmlDocument doc = hw.Load((Request.IsHttps ? "https" : "http") + "://" + Request.Host + "/");
                string index = doc.Text.Replace("../..", ServerConfigSettings.ServerPublicUrl);
                 System.IO.File.WriteAllText(Path.Combine(ServerRuntimeData.Startup_path, "Export", "Webpages", "Index.html"), index);

                ZipFile.CreateFromDirectory(Path.Combine(ServerRuntimeData.Startup_path, "Export", "Webpages"), Path.Combine(ServerRuntimeData.Startup_path, "Export", "Webpages.zip"));
                var zipData = await System.IO.File.ReadAllBytesAsync(Path.Combine(ServerRuntimeData.Startup_path, "Export", "Webpages.zip"));

                return File(zipData, "application/x-zip-compressed", "Webpages.zip");
            } catch (Exception ex) { return BadRequest(new { message = DataOperations.GetSystemErrMessage(ex) }); }
        }


    }
}