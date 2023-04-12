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
using Microsoft.Data.SqlClient;

namespace TravelAgencyBackEnd.Controllers
{
    [Authorize]
    [ApiController]
    [Route("PropertyOrServiceTypeList")]
    public class PropertyOrServiceTypeListApi : ControllerBase
    {
        [HttpGet("/PropertyOrServiceTypeList")]
        public async Task<string> GetPropertyOrServiceTypeList()
        {
            List<PropertyOrServiceTypeList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            }))
            {
                data = new hotelsContext().PropertyOrServiceTypeLists.ToList();
            }

            return JsonSerializer.Serialize(data, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true });
        }

        [HttpGet("/PropertyOrServiceTypeList/Filter/{filter}")]
        public async Task<string> GetPropertyOrServiceTypeListByFilter(string filter)
        {
            List<PropertyOrServiceTypeList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            }))
            {
                data = new hotelsContext().PropertyOrServiceTypeLists.FromSqlRaw("SELECT * FROM PropertyOrServiceTypeList WHERE 1=1 AND " + filter.Replace("+"," ")).AsNoTracking().ToList();
            }

            return JsonSerializer.Serialize(data, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true });
        }

        [HttpGet("/PropertyOrServiceTypeList/{id}")]
        public async Task<string> GetPropertyOrServiceTypeListKey(int id)
        {
            PropertyOrServiceTypeList data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            }))
            {
                data = new hotelsContext().PropertyOrServiceTypeLists.Where(a => a.Id == id).First();
            }

            return JsonSerializer.Serialize(data, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true });
        }

        [HttpPut("/PropertyOrServiceTypeList")]
        [Consumes("application/json")]
        public async Task<string> InsertPropertyOrServiceTypeList([FromBody] PropertyOrServiceTypeList record)
        {
            try
            {
                record.User = null;  //EntityState.Detached IDENTITY_INSERT is set to OFF
                var data = new hotelsContext().PropertyOrServiceTypeLists.Add(record);
                int result = await data.Context.SaveChangesAsync();

                //Create Property in All Hotels
                List<SqlParameter> parameters = new();
                parameters = new List<SqlParameter> {
                        new SqlParameter { ParameterName = "@HotelId", IsNullable = true, DbType = System.Data.DbType.Int32, Value = DBNull.Value },
                        new SqlParameter { ParameterName = "@PropertyId", Value = record.Id },
                        };
                new hotelsContext().Database.ExecuteSqlRaw("exec GenerateHotelProperties @HotelId, @PropertyId", parameters.ToArray()).ToString();

                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { insertedId = record.Id, status = DBResult.success.ToString(), recordCount = result, message = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { status = DBResult.error.ToString(), recordCount = result, message = string.Empty });
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new DBResultMessage() { status = DBResult.error.ToString(), recordCount = 0, message = SystemFunctions.GetUserApiErrMessage(ex) });
            }
        }

        [HttpPost("/PropertyOrServiceTypeList")]
        [Consumes("application/json")]
        public async Task<string> UpdatePropertyOrServiceTypeList([FromBody] PropertyOrServiceTypeList record)
        {
            try
            {
                var data = new hotelsContext().PropertyOrServiceTypeLists.Update(record);
                int result = await data.Context.SaveChangesAsync();

                //Recreate Property in All Hotels
                List<SqlParameter> parameters = new();
                parameters = new List<SqlParameter> {
                        new SqlParameter { ParameterName = "@HotelId", IsNullable = true, DbType = System.Data.DbType.Int32, Value = DBNull.Value },
                        new SqlParameter { ParameterName = "@PropertyId", Value = record.Id },
                        };
                new hotelsContext().Database.ExecuteSqlRaw("exec GenerateHotelProperties @HotelId, @PropertyId", parameters.ToArray()).ToString();


                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { insertedId = record.Id, status = DBResult.success.ToString(), recordCount = result, message = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { status = DBResult.error.ToString(), recordCount = result, message = string.Empty });
            }
            catch (Exception ex)
            { return JsonSerializer.Serialize(new DBResultMessage() { status = DBResult.error.ToString(), recordCount = 0, message = SystemFunctions.GetUserApiErrMessage(ex) }); }
        }

        [HttpDelete("/PropertyOrServiceTypeList/{id}")]
        [Consumes("application/json")]
        public async Task<string> DeletePropertyOrServiceTypeList(string id)
        {
            try
            {
                if (!int.TryParse(id, out int Ids)) return JsonSerializer.Serialize(new DBResultMessage() { status = DBResult.error.ToString(), recordCount = 0, message = "Id is not set" });

                PropertyOrServiceTypeList record = new() { Id = int.Parse(id) };

                var data = new hotelsContext().PropertyOrServiceTypeLists.Remove(record);
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
