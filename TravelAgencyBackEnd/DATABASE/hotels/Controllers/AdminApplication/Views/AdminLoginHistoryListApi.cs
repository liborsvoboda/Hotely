using TravelAgencyBackEnd.DBModel;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Transactions;
using TravelAgencyBackEnd.CoreClasses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;

namespace TravelAgencyBackEnd.Controllers
{
    [Authorize]
    [ApiController]
    [Route("AdminLoginHistoryList")]
    public class AdminLoginHistoryListApi : ControllerBase
    {
        [HttpGet("/AdminLoginHistoryList")]
        public async Task<string> GetLoginHistory()
        {
            List<AdminLoginHistoryList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            }))
            { data = new hotelsContext().AdminLoginHistoryLists.ToList(); }
            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/AdminLoginHistoryList/Filter/{filter}")]
        public async Task<string> GetAdminLoginHistoryListByFilter(string filter)
        {
            List<AdminLoginHistoryList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            }))
            {
                data = new hotelsContext().AdminLoginHistoryLists.FromSqlRaw("SELECT * FROM AdminLoginHistoryList WHERE 1=1 AND " + filter.Replace("+"," ")).AsNoTracking().ToList();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpPost("/AdminLoginHistoryList")]
        [Consumes("application/json")] // ([FromBody] int Id) Body is only 17 ([FromBody] IdFilter id) Body is {"Id":17}
        public async Task<string> GetAdminLoginHistoryListId([FromBody] IdFilter id)
        {
            if (!int.TryParse(id.Id.ToString(), out int Ids)) return JsonSerializer.Serialize(new DBResultMessage() { status = DBResult.error.ToString(), recordCount = 0, ErrorMessage = "Id is not set" });

            AdminLoginHistoryList data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            { IsolationLevel = IsolationLevel.ReadUncommitted }))
            { data = new hotelsContext().AdminLoginHistoryLists.Where(a => a.Id == id.Id).First(); }
            return JsonSerializer.Serialize(data);
        }

        /*
            [HttpPost("/LoginHistory/Name")]
            [Consumes("application/json")]//([FromBody] string Name) Body is only "tester" ([FromBody] NameFilter Name) Body is {"Name":"tester"}
            public async Task<string> GetLoginHistoryName([FromBody] NameFilter Name)
            {
                if (string.IsNullOrWhiteSpace(Name.Name)) return JsonSerializer.Serialize(new DBResultMessage() { status = DBResult.error.ToString(), recordCount = 0, ErrorMessage = "Id is null" });

                List<LoginHistory> data;
                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
                { IsolationLevel = IsolationLevel.ReadUncommitted }))
                { data = new Context().LoginHistories.Where(a => a.UserName == Name.Name.ToString()).ToList(); }
                return JsonSerializer.Serialize(data);
            }
        */
    }
}
