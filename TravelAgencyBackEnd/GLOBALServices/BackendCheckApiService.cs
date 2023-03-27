using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace TravelAgencyBackEnd.Controllers
{
    [ApiController]
    [Route("BackendCheck")]
    public class BackendCheckApiService : ControllerBase
    {

        [HttpGet("/BackendCheck")]
        public Task<string> GetBackendCheckApi()
        { return Task.FromResult("Backend Api Server is connected."); }

    }

}
