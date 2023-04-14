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

namespace TravelAgencyBackEnd.Controllers
{
    [ApiController]
    [Route("ParameterList")]
    public class ParameterListApi : ControllerBase
    {

        [HttpGet("/ParameterList")]
        public async Task<string> GetParameterList()
        {
            List<ParameterList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                data = new hotelsContext().ParameterLists.Where(a => a.UserId == null).ToList();
            }
            return JsonSerializer.Serialize(data, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true });
        }

        [HttpGet("/ParameterList/Filter/{filter}")]
        public async Task<string> GetParameterListByFilter(string filter)
        {
            List<ParameterList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            }))
            {
                if (Request.HttpContext.User.IsInRole("Admin"))
                { data = new hotelsContext().ParameterLists.FromSqlRaw("SELECT * FROM ParameterList WHERE 1=1 AND " + filter.Replace("+", " ")).AsNoTracking().ToList(); }
                else
                {
                    data = new hotelsContext().ParameterLists.FromSqlRaw("SELECT * FROM ParameterList WHERE 1=1 AND " + filter.Replace("+", " "))
                        .Include(a => a.User).Where(a => a.User.UserName == Request.HttpContext.User.Claims.First().Issuer)
                        .AsNoTracking().ToList();
                }
            }

            return JsonSerializer.Serialize(data, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true });
        }

        [Authorize]
        [HttpGet("/ParameterList/{userId}")]
        public async Task<string> GetParameterListKey(int userId)
        {
            List<ParameterList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {

                if (Request.HttpContext.User.IsInRole("Admin"))
                {
                    data = new hotelsContext().ParameterLists.ToList();
                }
                else
                {
                    data = new hotelsContext().ParameterLists.Where(a => a.UserId == userId || a.UserId == null).ToList();
                }
            }

            return JsonSerializer.Serialize(data, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true });
        }

        [Authorize]
        [HttpPut("/ParameterList")]
        [Consumes("application/json")]
        public async Task<string> InsertParameterList([FromBody] ParameterList record)
        {
            try
            {
                record.User = null;  //EntityState.Detached IDENTITY_INSERT is set to OFF
                var data = new hotelsContext().ParameterLists.Add(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { insertedId = record.Id, status = DBResult.success.ToString(), recordCount = result, message = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { status = DBResult.error.ToString(), recordCount = result, message = string.Empty });
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new DBResultMessage() { status = DBResult.error.ToString(), recordCount = 0, message = SystemFunctions.GetUserApiErrMessage(ex) });
            }
        }

        [Authorize]
        [HttpPost("/ParameterList")]
        [Consumes("application/json")]
        public async Task<string> UpdateParameterList([FromBody] ParameterList record)
        {
            try
            {
                var data = new hotelsContext().ParameterLists.Update(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { insertedId = record.Id, status = DBResult.success.ToString(), recordCount = result, message = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { status = DBResult.error.ToString(), recordCount = result, message = string.Empty });
            }
            catch (Exception ex)
            { return JsonSerializer.Serialize(new DBResultMessage() { status = DBResult.error.ToString(), recordCount = 0, message = SystemFunctions.GetUserApiErrMessage(ex) }); }
        }

        [Authorize]
        [HttpDelete("/ParameterList/{id}")]
        [Consumes("application/json")]
        public async Task<string> DeleteParameterList(string id)
        {
            try
            {
                if (!int.TryParse(id, out int Ids)) return JsonSerializer.Serialize(new DBResultMessage() { status = DBResult.error.ToString(), recordCount = 0, message = "Id is not set" });

                ParameterList record = new() { Id = int.Parse(id) };

                var data = new hotelsContext().ParameterLists.Remove(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { insertedId = record.Id, status = DBResult.success.ToString(), recordCount = result, message = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { status = DBResult.error.ToString(), recordCount = result, message = string.Empty });

            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new DBResultMessage() { status = DBResult.error.ToString(), recordCount = 0, message = SystemFunctions.GetUserApiErrMessage(ex) });
            }
        }
    }
}
