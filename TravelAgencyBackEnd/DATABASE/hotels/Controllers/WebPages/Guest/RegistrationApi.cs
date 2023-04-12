using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Transactions;
using TravelAgencyBackEnd.CoreClasses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using TravelAgencyBackEnd.DBModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Text.Json.Serialization;
using TravelAgencyBackEnd.WebPages;

namespace TravelAgencyBackEnd.Controllers
{
    [Authorize]
    [ApiController]
    [Route("WebRegistration")]
    public class WebRegistrationApi : ControllerBase
    {

        [HttpPost("/WebRegistration")]
        [Consumes("application/json")]
        public async Task<string> SaveWebRegistration([FromBody] GuestList record, [FromBody] PageLanguage language)
        {
            try
            {
                //check email exist
                int count;
                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                { count = new hotelsContext().GuestLists.Where(a => a.Email == record.Email).Count(); }
                if (count > 0)
                {
                    return JsonSerializer.Serialize(new DBResultMessage()
                    {
                        status = DBWebApiResponses.emailExist.ToString(),
                        message = Functions.Translate(lang, DBWebApiResponses.emailExist.ToString())
                    });
                }


                var data = new hotelsContext().GuestLists.Add(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { insertedId = record.Id, status = DBResult.success.ToString(), recordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { status = DBResult.error.ToString(), recordCount = result, ErrorMessage = string.Empty });
            }
            catch (Exception ex)
            { return JsonSerializer.Serialize(new DBResultMessage() { status = DBResult.error.ToString(), recordCount = 0, ErrorMessage = SystemFunctions.GetUserApiErrMessage(ex) }); }
        }

    }
}
