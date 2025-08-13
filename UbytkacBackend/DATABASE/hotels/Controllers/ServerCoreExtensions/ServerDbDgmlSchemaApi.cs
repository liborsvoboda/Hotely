

namespace UbytkacBackend.ServerCoreDBSettings {

    /// <summary>
    /// Database Schema Diagram Controller
    /// </summary>
    /// <seealso cref="Controller"/>
    [Authorize]
    [Route("ServerDbDgmlSchema")]
    public class ServerDbDgmlSchemaApi : Controller {
        private hotelsContext Context { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DgmlSchemaApi"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public ServerDbDgmlSchemaApi(hotelsContext context) { Context = context; }

        /// <summary>
        /// Creates a DGML class diagram of most of the entities in the project wher you go to
        /// localhost/dgml See https://github.com/ErikEJ/SqlCeToolbox/wiki/EF-Core-Power-Tools
        /// </summary>
        /// <returns>a DGML class diagram</returns>
        [HttpGet("/ServerDbDgmlSchema/dgml")]
        public IActionResult GetDgml() {
            if (ServerConfigSettings.ModuleDbDiagramGeneratorEnabled) { //"application/octet-stream"
                var response = File(Encoding.UTF8.GetBytes( new hotelsContext().AsDgml()), MimeTypes.GetMimeType("DBschema.dgml"), "DBschema.dgml");
                return response;
            } else { return null; }
        }


        /// <summary>
        /// Get Full DataBase SQL Script
        /// </summary>
        /// <returns></returns>
        [HttpGet("/ServerDbDgmlSchema/sql")]
        public IActionResult Get() {
            if (ServerConfigSettings.ModuleDbDiagramGeneratorEnabled) {
                var response = File(Encoding.UTF8.GetBytes(Context.AsSqlScript()), MimeTypes.GetMimeType("DBschema.sql"), "DBschema.sql");
                return response;
            } else { return null; }
        }
    }
}