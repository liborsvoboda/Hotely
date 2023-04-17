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

namespace TravelAgencyBackEnd.Controllers
{
    [ApiController]
    [Route("WebApi/Search")]
    public class WebSearchApi : ControllerBase
    {
        private readonly hotelsContext _dbContext = new();


        [HttpGet("/WebApi/Search/GetSearchDialList/{language}")]
        public async Task<string> GetSearchDialList(string language = "cz") {

            List<string> data = GetTranslatedSearchList(language);

            return JsonSerializer.Serialize(data, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true });
        }


        [HttpGet("/WebApi/Search/GetSearchInput/{searched}/{language}")]
        public async Task<string> GetSearchInput(string searched, string language = "cz") {

            List<int> data = GetSearchedIdList(searched, language);

            List<HotelList> result;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
                result = new hotelsContext().HotelLists
                    .Include(a => a.HotelRoomLists.Where(a => a.Approved == true)).Include(a => a.City).Include(a => a.Country)
                    .Include(a => a.DefaultCurrency).Include(a => a.HotelPropertyAndServiceLists)
                    .Where(a => data.Contains(a.Id)).ToList();
            }

            //TODO changed to old structure 
            WebPageRootSearchData rootData = new();
            result.ForEach(hotel => { rootData.HotelList.Add(new WebPageRootSearch() { Hotel = hotel, RoomList = null }); });
            


            return JsonSerializer.Serialize(rootData, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true, 
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase, PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        }


        /// <summary>
        /// Full Translate List for WebPage Search Input : Whisperer
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        private List<string> GetTranslatedSearchList(string language = "cz") {

            List<string> data;
            List<string> cityData;
            List<string> countryData;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
                countryData = _dbContext.CountryLists.
                    Join(_dbContext.HotelLists.Where(a => a.Advertised && a.Approved == true),
                    joiner => joiner.Id, joined => joined.CountryId, (_joiner, _joined) => _joiner.SystemName).ToList();

                cityData = _dbContext.CityLists.Join(_dbContext.HotelLists.Where(a => a.Advertised && a.Approved == true),
                    joiner => joiner.Id, joined => joined.CountryId, (_joiner, _joined) => _joiner.City).ToList();

                data = _dbContext.HotelLists.Where(a => a.Approved == true && a.Advertised == true).Select(a => a.Name).ToList();
            }
            countryData.ForEach(item => data.Add(DBOperations.DBTranslate(item, language)));
            cityData.ForEach(item => data.Add(DBOperations.DBTranslate(item, language)));
            data = data.Distinct().ToList();
            data.Sort();
            return data;
        }


        /// <summary>
        /// Full Translate List for WebPage Search Input 
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        private List<int> GetSearchedIdList(string searched,string language = "cz") {

            List<Tuple<int, string>> data;
            List<Tuple<int, string>> cityData;
            List<Tuple<int, string>> countryData;
            List<int> searchedIdList = new();
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
                countryData = _dbContext.CountryLists.
                    Join(_dbContext.HotelLists.Where(a => a.Advertised && a.Approved == true),
                    joiner => joiner.Id, joined => joined.CountryId, (_joiner, _joined) => new Tuple<int, string>(_joined.Id, _joiner.SystemName)).ToList();

                cityData = _dbContext.CityLists.Join(_dbContext.HotelLists.Where(a => a.Advertised && a.Approved == true),
                    joiner => joiner.Id, joined => joined.CountryId, (_joiner, _joined) => new Tuple<int, string>(_joined.Id, _joiner.City)).ToList();

                data = _dbContext.HotelLists.Where(a => a.Approved == true && a.Advertised == true).Select(a => new Tuple<int, string>(a.Id, a.Name)).ToList();
            }
            countryData.ForEach(item => data.Add(new Tuple<int,string>(item.Item1,DBOperations.DBTranslate(item.Item2, language))));
            cityData.ForEach(item => data.Add(new Tuple<int, string>(item.Item1, DBOperations.DBTranslate(item.Item2, language))));
            data = data.Distinct().ToList();
            data.Sort();

            //Check Searched Value
            data.ForEach(item => {
                if (item.Item2.ToLower().Contains(searched.ToLower())) searchedIdList.Add(item.Item1);
            });

            return searchedIdList;
        }



    }
}
