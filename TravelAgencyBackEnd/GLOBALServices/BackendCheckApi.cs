using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Threading.Tasks;
using System.Transactions;

namespace BACKENDCORE.Controllers
{
    [ApiController]
    [Route("BackendCheck")]
    public class BackendCheckApi : ControllerBase
    {

        [HttpGet("/BackendCheck")]
        public Task<string> GetBackendCheckApi()
        { return Task.FromResult("Backend Api Server is connected."); }

    }

}
