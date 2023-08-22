using System.Threading.Tasks;

namespace UbytkacBackend.Controllers {

    [ApiController]
    [Route("BackendCheck")]
    public class BackendCheckApiService : ControllerBase {

        [HttpGet("/BackendCheck")]
        public Task<string> GetBackendCheckApi() { return Task.FromResult(DBOperations.DBTranslate("ServerRunning", "cz")); }
    }
}