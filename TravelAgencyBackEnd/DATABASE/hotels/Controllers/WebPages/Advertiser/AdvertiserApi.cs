using UbytkacBackend.DBModel;

namespace UbytkacBackend.Controllers {

    [Authorize]
    [ApiController]
    [Route("WebApi/Advertiser")]
    public class AdvertiserApi : ControllerBase {

        [HttpGet("/WebApi/Advertiser/GetAdvertisementList/{language}")]
        public async Task<string> GetAdvertisementList(string language = null) {

            string userId = User.FindFirst(ClaimTypes.GroupSid.ToString()).Value;
            List<HotelList> result = new List<HotelList>();
            try {

                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                    result = new hotelsContext().HotelLists
                        .Include(a => a.HotelRoomLists)
                        .Include(a => a.City)
                        .Include(a => a.Country)
                        .Include(a => a.DefaultCurrency)

                        /*
                        .Include(a => a.HotelReservationLists)
                        .ThenInclude(a => a.HotelReservationDetailLists)
                        .Include(a => a.HotelReservationLists)
                        .ThenInclude(a => a.HotelReservedRoomLists.Where(a => a.Count > 0))
                        */

                        //.Include(a => a.GuestAdvertiserNoteLists)
                        //.Include(a=> a.HotelReservationReviewLists)
                        .Where(a => a.UserId == int.Parse(userId))
                        .AsNoTracking()
                        .IgnoreAutoIncludes()
                        .ToList();
                }


                result.ForEach(hotel => {

                    // load props to each hotel by include is extremelly slow
                    using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                        List<HotelImagesList> images = new hotelsContext().HotelImagesLists.Where(a => a.HotelId == hotel.Id).ToList();
                        hotel.HotelImagesLists = images;

                        List<HotelPropertyAndServiceList> props = new hotelsContext().HotelPropertyAndServiceLists
                        .Include(a => a.PropertyOrService)
                        .Include(a => a.PropertyOrService.PropertyOrServiceUnitType)
                        .Where(a => a.HotelId == hotel.Id && a.IsAvailable).ToList();

                        props.ForEach(property => {
                            property.PropertyOrService.SystemName = DBOperations.DBTranslate(property.PropertyOrService.SystemName, language);
                        });
                        hotel.HotelPropertyAndServiceLists = props;

                        List<HotelReservationList> reservations = new hotelsContext().HotelReservationLists
                        .Include(a => a.HotelReservationDetailLists)
                        .Include(a => a.HotelReservedRoomLists.Where(a => a.Count > 0))
                        .Where(a => a.HotelId == hotel.Id).OrderByDescending(a => a.Timestamp).ToList();
                        hotel.HotelReservationLists = reservations;

                        List<GuestAdvertiserNoteList> notes = new hotelsContext().GuestAdvertiserNoteLists.Where(a => a.HotelId == hotel.Id)
                        .OrderByDescending(a => a.TimeStamp).ToList();
                        hotel.GuestAdvertiserNoteLists = notes;

                        List<HotelReservationReviewList> reviews = new hotelsContext().HotelReservationReviewLists.Where(a => a.HotelId == hotel.Id && a.Approved == true)
                        .OrderByDescending(a => a.Timestamp).ToList();
                        hotel.HotelReservationReviewLists = reviews;

                    }

                    //clean datasets
                    hotel.DescriptionCz = hotel.DescriptionCz.Replace("<HTML><BODY>", "").Replace("</BODY></HTML>", "");
                    hotel.DescriptionEn = hotel.DescriptionEn != null ? hotel.DescriptionEn.Replace("<HTML><BODY>", "").Replace("</BODY></HTML>", "") : "";

                    hotel.HotelImagesLists.ToList().ForEach(attachment => {
                        attachment.Hotel = null;
                        //attachment.Attachment = null;  need for wizard
                    });

                    hotel.City.HotelLists = null;
                    hotel.City.Country = null;
                    hotel.Country.CityLists = null;
                    hotel.Country.HotelLists = null;
                    hotel.DefaultCurrency.HotelLists = null;
                    hotel.HotelRoomLists.ToList().ForEach(room => {
                        room.Hotel = null;
                        //room.Image = null; need for wizard
                    });

                    //hotel.HotelReservationLists = hotel.HotelReservationLists.OrderByDescending(a => a.StartDate).ToList();
                    hotel.HotelReservationLists.ToList().ForEach(reservation => {
                        reservation.HotelReservationDetailLists = reservation.HotelReservationDetailLists.OrderByDescending(a => a.Timestamp).ToList();
                    });

                    //hotel.GuestAdvertiserNoteLists = hotel.GuestAdvertiserNoteLists.OrderByDescending(a => a.TimeStamp).ToList();
                    //hotel.HotelReservationReviewLists = hotel.HotelReservationReviewLists.OrderByDescending(a => a.Timestamp).ToList();
                });

                //result.ForEach(item => { item.SystemName = DBOperations.DBTranslate(item.SystemName, language); });
            } 
            catch (Exception ex) { }
            return JsonSerializer.Serialize(result, new JsonSerializerOptions() {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true,
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }


        [HttpPost("/WebApi/Advertiser/SetHotel")]
        [Consumes("application/json")]
        public async Task<string> InsertOrUpdateSetHotelList([FromBody] AvertiserHotel record) {
            try {

                string userId = User.FindFirst(ClaimTypes.GroupSid.ToString()).Value;
                HotelList hotelRec = new(); int result = 0;

                if (userId != null) {
                    if (record.HotelRecId == null) {
                        hotelRec = new() {
                            Name = record.HotelName,
                            CountryId = record.CountryId,
                            CityId = record.CityId,
                            DescriptionCz = record.Description,
                            DefaultCurrencyId = record.CurrencyId,
                            UserId = int.Parse(userId), 
                            ApproveRequest = false,
                            Approved = false,
                            EnabledCommDaysBeforeStart = record.LimitGuestCommDays

                        };
                        var data = new hotelsContext().HotelLists.Add(hotelRec);
                        result = await data.Context.SaveChangesAsync();

                        //Create Properties for new Hotel
                        List<SqlParameter> parameters = new();
                        parameters = new List<SqlParameter> {
                        new SqlParameter { ParameterName = "@HotelId", Value = hotelRec.Id },
                        new SqlParameter { ParameterName = "@PropertyId", IsNullable = true, DbType = System.Data.DbType.Int32, Value = DBNull.Value },
                        };
                        new hotelsContext().Database.ExecuteSqlRaw("exec GenerateHotelProperties @HotelId, @PropertyId", parameters.ToArray()).ToString();

                    }
                    else {
                        hotelRec = new hotelsContext().HotelLists.Where(a => a.Id == (int)record.HotelRecId).FirstOrDefault();
                        hotelRec.Name = record.HotelName; hotelRec.CountryId = record.CountryId; hotelRec.CityId = record.CityId;
                        hotelRec.DescriptionCz = record.Description; hotelRec.DefaultCurrencyId = record.CurrencyId; hotelRec.ApproveRequest = false; hotelRec.Approved = false;
                        hotelRec.EnabledCommDaysBeforeStart = record.LimitGuestCommDays;
                        var data = new hotelsContext().HotelLists.Update(hotelRec);
                        result = await data.Context.SaveChangesAsync();
                    }
                }

                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = hotelRec.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            } catch (Exception ex) {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = SystemFunctions.GetUserApiErrMessage(ex) });
            }
        }


        [HttpPost("/WebApi/Advertiser/SetHotelImages")]
        [Consumes("application/json")]
        public async Task<string> InsertOrUpdateOrDeleteHotelImagesList([FromBody] AvertiserImages record) {
            try {

                string userId = User.FindFirst(ClaimTypes.GroupSid.ToString()).Value;

                //clean exists
                List<HotelImagesList> existImages = new hotelsContext().HotelImagesLists.Where(a => a.HotelId == (int)record.HotelRecId).ToList();
                hotelsContext data1 = new(); data1.HotelImagesLists.RemoveRange(existImages);
                int result = await data1.SaveChangesAsync();

                //insert all news
                HotelImagesList imageRecord = new();
                record.Images.ForEach(async image => {
                    imageRecord = new() { HotelId = record.HotelRecId, FileName = image.FileName, Attachment = Functions.GetByteArrayFrom64Encode(image.Attachment), IsPrimary = image.IsPrimary, UserId = int.Parse(userId) };
                    var data = new hotelsContext().HotelImagesLists.Add(imageRecord);
                    int result = await data.Context.SaveChangesAsync();
                });

                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = imageRecord.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            } catch (Exception ex) {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = SystemFunctions.GetUserApiErrMessage(ex) });
            }
        }


        [HttpPost("/WebApi/Advertiser/SetHotelRooms")]
        [Consumes("application/json")]
        public async Task<string> InsertOrUpdateOrDeleteHotelRoomList([FromBody] AvertiserRooms record) {
            try {

                string userId = User.FindFirst(ClaimTypes.GroupSid.ToString()).Value;

                //clean exists
                List<HotelRoomList> existRooms = new hotelsContext().HotelRoomLists.Where(a => a.HotelId == (int)record.HotelRecId).ToList();
                hotelsContext data1 = new(); data1.HotelRoomLists.RemoveRange(existRooms);
                int result = await data1.SaveChangesAsync();

                //insert all news
                HotelRoomList roomRecord = new();
                record.Rooms.ForEach(async room => {
                    roomRecord = new() { HotelId = record.HotelRecId, RoomTypeId = room.RoomTypeId, Name = room.RoomName,DescriptionCz = room.Description, ImageName = room.FileName, 
                        Image = Functions.GetByteArrayFrom64Encode(room.Attachment), Price = room.Price, MaxCapacity = room.MaxCapacity, ExtraBed = room.ExtraBed, RoomsCount = room.RoomsCount, UserId = int.Parse(userId) };
                    var data = new hotelsContext().HotelRoomLists.Add(roomRecord);
                    int result = await data.Context.SaveChangesAsync();
                });

                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = roomRecord.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            } catch (Exception ex) {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = SystemFunctions.GetUserApiErrMessage(ex) });
            }
        }


        [HttpPost("/WebApi/Advertiser/SetHotelProperties")]
        [Consumes("application/json")]
        public async Task<string> UpdateHotelPropertyAndServiceList([FromBody] AvertiserProperties record) {
            try {
                string userId = User.FindFirst(ClaimTypes.GroupSid.ToString()).Value;
                int result = 0; List<int> availableProps = new();
                HotelPropertyAndServiceList propertyRecord = new();
                record.Properties.ForEach(async property => {
                    availableProps.Add(property.id);

                    propertyRecord = new hotelsContext().HotelPropertyAndServiceLists.Where(a => a.HotelId == (int)record.HotelRecId && a.PropertyOrServiceId == property.id).FirstOrDefault();
                    propertyRecord.IsAvailable = property.isAvailable;
                    propertyRecord.Value = property.value; propertyRecord.ValueRangeMin = property.valueRangeMin; propertyRecord.ValueRangeMax = property.valueRangeMax;
                    propertyRecord.Fee = property.fee; propertyRecord.FeeValue = property.feeValue; propertyRecord.FeeRangeMin = property.feeRangeMin; propertyRecord.FeeRangeMax = property.feeRangeMax;
                    propertyRecord.Timestamp = DateTimeOffset.Now.DateTime;

                    var data = new hotelsContext().HotelPropertyAndServiceLists.Update(propertyRecord);
                    result = await data.Context.SaveChangesAsync();
                });

                //disable availableProperties missing in API
                List<HotelPropertyAndServiceList> removedproperties = new hotelsContext().HotelPropertyAndServiceLists.Where(a => a.HotelId == (int)record.HotelRecId && !availableProps.Contains(a.Id)).ToList();
                removedproperties.ForEach(property => { property.IsAvailable = false; });
                var data = new hotelsContext().HotelPropertyAndServiceLists.Update(propertyRecord);
                result = await data.Context.SaveChangesAsync();


                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = propertyRecord.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = SystemFunctions.GetUserApiErrMessage(ex) }); }
        }


        // failed on used
        [HttpGet("/WebApi/Advertiser/DeleteAdvertisement/{id}")]
        public async Task<string> DeleteUnusedHotel(int id) {
            try {
                string userId = User.FindFirst(ClaimTypes.GroupSid.ToString()).Value;
                int result = 0;

                HotelList hotel = new hotelsContext().HotelLists.Where(a => a.Id == id && a.UserId == int.Parse(userId)).FirstOrDefault();
                if (hotel != null) {
                    var data = new hotelsContext().HotelLists.Remove(hotel);
                    result = await data.Context.SaveChangesAsync();
                }

                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = hotel.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = SystemFunctions.GetUserApiErrMessage(ex) }); }
        }


        [HttpGet("/WebApi/Advertiser/ActivationHotel/{id}")]
        public async Task<string> ActivateDeactivateHotel(int id) {
            try {
                string userId = User.FindFirst(ClaimTypes.GroupSid.ToString()).Value;
                int result = 0;

                HotelList hotel = new hotelsContext().HotelLists.Where(a => a.Id == id && a.UserId == int.Parse(userId)).FirstOrDefault();
                if (hotel != null) {
                    hotel.Deactivated = !hotel.Deactivated;
                    var data = new hotelsContext().HotelLists.Update(hotel);
                    result = await data.Context.SaveChangesAsync();
                }
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = hotel.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = SystemFunctions.GetUserApiErrMessage(ex) }); }
        }


        [HttpGet("/WebApi/Advertiser/ApprovalRequest/{id}")]
        public async Task<string> SetApprovalRequest(int id) {
            try {
                string userId = User.FindFirst(ClaimTypes.GroupSid.ToString()).Value;
                int result = 0;

                HotelList hotel = new hotelsContext().HotelLists.Where(a => a.Id == id && a.UserId == int.Parse(userId)).FirstOrDefault();
                if (hotel != null) {
                    hotel.ApproveRequest = true;
                    var data = new hotelsContext().HotelLists.Update(hotel);
                    result = await data.Context.SaveChangesAsync();
                }
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = hotel.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = SystemFunctions.GetUserApiErrMessage(ex) }); }
        }

    }
}