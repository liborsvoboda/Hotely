using UbytkacBackend.ServerCoreStructure;
using FastReport;
using FastReport.Web;
using Microsoft.AspNetCore.Mvc.RazorPages;

using static ServerCorePages.ViewerReportFileModel;
namespace ServerCorePages {


    /// <summary>
    /// Fast Report Webový Prohlížeč Reportů 
    /// V případě že je zadána Cesta nástroje Zobrazí Ukázky
    /// Cesta Nástoje: /ServerCoreTools/ViewerReportFile
    /// </summary>
    public class ViewerReportFileModel : PageModel {
        public static ServerWebPagesToken serverWebPagesToken;
        public static string result;
        public ReportList reportList = new ReportList();


        public class ReportList {
            public WebReport WebReport { get; set; }
            public List<string> ReportNameList { get; set; } = new List<string>();
        }


        public void OnGet() {


            //Standalone Check Token
            string? requestToken = HttpContext.Request.Cookies.FirstOrDefault(a => a.Key == "ApiToken").Value;
            if (!string.IsNullOrWhiteSpace(requestToken)) {
                serverWebPagesToken = CoreOperations.CheckTokenValidityFromString(requestToken);
                if (serverWebPagesToken.IsValid) { User.AddIdentities(serverWebPagesToken.UserClaims.Identities); }
            } else { serverWebPagesToken = new ServerWebPagesToken(); }


            //Show Examples OR TODO DB Report
            string? requestedUrlPath = ""; string fileContent = null;
            try {
                try { requestedUrlPath = ((string?)this.HttpContext.Items.FirstOrDefault(a => a.Key.ToString() == "FileValidUrl").Value); } catch { }
                if (requestedUrlPath != null && requestedUrlPath.ToLower().EndsWith(".frx")) { /*TODO LOAD FROM DB BY UNIQUE NAME AFTER SELECT DELETE FROM DB*/}

                else {//Show Examples
                    var exampleFiles = FileOperations.GetPathFiles(Path.Combine(ServerRuntimeData.WebRoot_path, "ServerCoreTools", "Viewers", "FastReport", "Example"), "*.*", SearchOption.TopDirectoryOnly);

                    string param = this.Request.Query.FirstOrDefault(a => a.Key == "ReportId").Value;
                    int? reportId = param != null && int.TryParse(param, out int intParam) ? intParam : null;

                    exampleFiles.ForEach(example => {
                        try {
                            if (reportList.WebReport == null && reportId != null) {
                                reportList.WebReport = new WebReport();
                                if (exampleFiles[(int)reportId].EndsWith(".frx")) { reportList.WebReport.Report.Load(exampleFiles[(int)reportId]); }
                                else { reportList.WebReport.Report.LoadPrepared(exampleFiles[(int)reportId]); }
                            }
                            else if (reportList.WebReport == null) {
                                reportList.WebReport = new WebReport();
                                if (example.EndsWith(".frx")) { reportList.WebReport.Report.Load(example); }
                                else { reportList.WebReport.Report.LoadPrepared(example); }
                            }

                        } catch (Exception ex) { }

                        reportList.ReportNameList.Add(Path.GetFileName(example)); 
                    });
                }

                //Example Show from File
                //var webReport = new WebReport();
                //webReport.Report.Load("path/to/report.frx");
                //return View(webReport);

                //Example Save
                //Stream reportForSave = Request.Body; string pathToSave = Path.Combine(webRootPath, "DesignedReports", reportName);
                //if (!Directory.Exists(pathToSave)) { Directory.CreateDirectory(Path.GetDirectoryName(pathToSave)); }
                //using (FileStream file = new FileStream(pathToSave, FileMode.Create)) { reportForSave.CopyToAsync(file); }

            } catch { }
        }


    }
}