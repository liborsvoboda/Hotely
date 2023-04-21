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
    [Route("WebApi/Image")]
    public class WebImageApi : ControllerBase
    {
        private readonly hotelsContext _dbContext = new();


        [HttpGet("/WebApi/Image/{id}")]
        public async Task<IActionResult> GetSearchImageById(int? id = null) {

            int recId; HotelImagesList data = null;
            if (!string.IsNullOrWhiteSpace(id.ToString()) && int.TryParse(id.ToString(), out recId))
            {
                data = new hotelsContext().HotelImagesLists.Where(a => a.Id == recId).FirstOrDefault();
            }


            if (data != null) { return File(data.Attachment, MimeTypes.GetMimeType(data.FileName), data.FileName); }
            else { return BadRequest(new { message = DBOperations.DBTranslate("BadRequest", "en") }); }
        }



        [HttpGet("/WebApi/Image/{hotelId}/{filename}")]
        public async Task<IActionResult> GetSearchImageByKeys(int? hotelId = null,string fileName = null) {

            int recId; HotelImagesList data = null;
            if (!string.IsNullOrWhiteSpace(hotelId.ToString()) && !string.IsNullOrWhiteSpace(fileName) && int.TryParse(hotelId.ToString(), out recId))
            {
                data = new hotelsContext().HotelImagesLists.Where(a => a.HotelId == recId && a.FileName.ToLower() == fileName.ToLower()).FirstOrDefault();
            }

            if (data != null) { return File(data.Attachment, MimeTypes.GetMimeType(data.FileName), data.FileName); }
            else { return BadRequest(new { message = DBOperations.DBTranslate("BadRequest", "en") }); }
        }




    }
}
