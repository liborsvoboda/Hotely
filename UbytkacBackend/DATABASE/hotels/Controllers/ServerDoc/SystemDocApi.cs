using UbytkacBackend.DBModel;


namespace UbytkacBackend.Controllers {

    [ApiController]
    [Route("/WebApi/WebDocumentation")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class SystemDocApi : ControllerBase {


        /// <summary>
        /// Documentation Code Manager Html Preview Api
        /// Startup Viewer
        /// </summary>
        /// <returns></returns>
        [HttpGet("/WebApi/WebDocumentation/MdLibraryPreview/{id}")]
        public IActionResult GetMdLibraryPreview(int id) {
            string data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted })) { data = new hotelsContext().DocumentationCodeLibraryLists.Where(a => a.Id == id).First().MdContent;
            }
            FileOperations.ClearFolder(Path.Combine(ServerRuntimeData.Startup_path, "wwwroot", "server-doc", "md-preview", "data"));
            System.IO.File.WriteAllText(Path.Combine(ServerRuntimeData.Startup_path, "wwwroot", "server-doc", "md-preview", "data", "preview.md"), data);
            return new RedirectResult("/server-doc/md-preview");
        }


        /// <summary>
        /// MD Preview Api file after Saving
        /// Its same for Library & Document
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet("/WebApi/WebDocumentation/MdPreviewFile")]
        public string GetMdPreviewFile(int id) {
            string previewMd = System.IO.File.ReadAllText(Path.Combine(ServerRuntimeData.Startup_path, "wwwroot", "server-doc", "md-preview", "data", "preview.md"));
            return previewMd.ToString(); 
        }


        /// <summary>
        /// Documentation Code Manager Html Preview Api
        /// Startup Viewer
        /// </summary>
        /// <returns></returns>
        [HttpGet("/WebApi/WebDocumentation/MdDocPreview/{id}")]
        public IActionResult GetMdDocumentPreview(int id) {
            string data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted
            })) {
                data = new hotelsContext().DocumentationLists.Where(a => a.Id == id).First().MdContent;
            }
            FileOperations.ClearFolder(Path.Combine(ServerRuntimeData.Startup_path, "wwwroot", "server-doc", "md-preview", "data"));
            System.IO.File.WriteAllText(Path.Combine(ServerRuntimeData.Startup_path, "wwwroot", "server-doc", "md-preview", "data", "preview.md"), data);
            return new RedirectResult("/server-doc/md-preview");
        }

    }
}