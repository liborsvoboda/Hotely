using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TravelAgencyBackEnd.Services;

namespace TravelAgencyBackEnd.Controllers
{
    [ApiController]
    [Route("BackendCheck")]
    public class BackendCheckApiService : ControllerBase
    {

        [HttpGet("/BackendCheck")]
        public Task<string> GetBackendCheckApi()
        { return Task.FromResult(DBOperations.DBTranslate("ServerRunning", "cz")); }

    }

}
