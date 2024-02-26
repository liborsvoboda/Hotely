
//System Controllers Definitions
namespace UbytkacBackend.ServerCoreDBSettings {
    /* Example
    API URL is: http://localhost:8000/ServerGenericAuthApi/SystemParameterList
    [Authorize]
    [Authorize(Roles="members,admin")]
    [Route("ServerGenericAuthApi/[controller]")]
    [ApiController]
    public class SystemParameterListController : AuthGenericProviderApi<hotelsContext, SystemParameterList, int> {
        public SystemParameterListController(IRepositoryAsync<hotelsContext, SystemParameterList> repo) : base(repo) { }
    }*/


    [Authorize(Roles = "admin")]
    [Route("[controller]")]
    [ApiController]
    public class ServerModuleAndServiceListController : StdAuthGenericProviderApi<hotelsContext, ServerModuleAndServiceList, int> {

        public ServerModuleAndServiceListController(IRepositoryAsync<hotelsContext, ServerModuleAndServiceList> repo) : base(repo) {
        }
    }


    [Authorize(Roles = "admin")]
    [Route("[controller]")]
    [ApiController]
    public class SystemModuleListController : StdAuthGenericProviderApi<hotelsContext, SystemModuleList, int> {

        public SystemModuleListController(IRepositoryAsync<hotelsContext, SystemModuleList> repo) : base(repo) {
        }
    }

    
}