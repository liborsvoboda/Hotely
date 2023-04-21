using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Transactions;
using TravelAgencyBackEnd.DBModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using TravelAgencyBackEnd.WebPages;
using Microsoft.Extensions.Options;
using Stripe;
using System.Text;
using System.IO;
using System.Net.Mail;

namespace TravelAgencyBackEnd.Controllers
{
    [ApiController]
    [Route("WebApi/Properties")]
    public class PropertiesApi : ControllerBase
    {
        private readonly hotelsContext _dbContext = new();


        [HttpGet("/WebApi/Properties/{language}")]
        public async Task<string> GetProperties(string language = null) {

            List<PropertyOrServiceTypeList> result;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
                result = new hotelsContext().PropertyOrServiceTypeLists
                    .Include(a => a.PropertyOrServiceUnitType)
                    .ToList();
            }

            result.ForEach(item => { 
                item.SystemName = DBOperations.DBTranslate(item.SystemName, language);
                item.PropertyOrServiceUnitType.SystemName = DBOperations.DBTranslate(item.PropertyOrServiceUnitType.SystemName, language);
            });

            return JsonSerializer.Serialize(result, new JsonSerializerOptions()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true,
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }

    }
}
