using Microsoft.EntityFrameworkCore.Metadata.Internal;
using UbytkacBackend.DBModel;

namespace UbytkacBackend.Controllers {

    [ApiController]
    [Route("WebApi/Guest")]
    public class ReservationApi : ControllerBase {


        [HttpPost("/WebApi/Guest/Reservation/CheckEmail")]
        [Consumes("application/json")]
        public IActionResult PostCheckEmail([FromBody] AutoGenEmailAddress record) {
            try {
                if (!string.IsNullOrWhiteSpace(record.EmailAddress) && Functions.IsValidEmail(record.EmailAddress)) {
                    string newPassword = Functions.RandomString(10);

                    //check email exist
                    GuestList data;
                    using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                        data = new hotelsContext().GuestLists.Where(a => a.Email == record.EmailAddress && a.Active).FirstOrDefault();
                    }

                    if (data == null) {
                        
                        //Send Verify Email
                        string verifyCode = Functions.RandomString(10);
                        EmailTemplateList template = new hotelsContext().EmailTemplateLists.Where(a => a.TemplateName == "verification").FirstOrDefault();
                        MailRequest mailRequest = new MailRequest();
                        if (template != null) {
                            mailRequest = new MailRequest() {
                                Subject = (record.Language == "cz" ? template.SubjectCz : template.SubjectEn).Replace("[verifyCode]", verifyCode),
                                Recipients = new List<string>() { record.EmailAddress },
                                Content = (record.Language == "cz" ? template.EmailCz : template.EmailEn).Replace("[verifyCode]", verifyCode)
                            };
                        }
                        else {
                            mailRequest = new() {
                                Subject = "Úbytkač Registration Verification Email",
                                Recipients = new() { record.EmailAddress },
                                Content = "Your Registration Verify Code is: " + verifyCode + Environment.NewLine
                            };
                        }
                        string result = SystemFunctions.SendEmail(mailRequest);

                        return Ok(JsonSerializer.Serialize(
                        new DBResultMessage() {
                            Status = DBResult.success.ToString(),
                            ErrorMessage = verifyCode
                        })); } else {
                        return BadRequest(JsonSerializer.Serialize(new DBResultMessage() {
                            Status = DBResult.error.ToString(),
                            ErrorMessage = DBOperations.DBTranslate(DBWebApiResponses.emailExist.ToString(), record.Language)
                        })); }
                }
                else { return BadRequest(new DBResultMessage() {
                    Status = DBResult.error.ToString(),
                    ErrorMessage = DBOperations.DBTranslate("EmailAddressIsNotValid", record.Language)
                }); }
            } catch { }
            return BadRequest(new DBResultMessage() {
                Status = DBResult.error.ToString(),
                ErrorMessage = DBOperations.DBTranslate("EmailAddressIsNotValid", record.Language)
            });
        }


        [Authorize]
        [HttpPost("/WebApi/Guest/Reservation/AuthSetBooking")]
        [Consumes("application/json")]
        public IActionResult PostAuthSetBooking([FromBody] BookingRequest record) {
            try {

                string authId = User.FindFirst(ClaimTypes.PrimarySid.ToString()).Value;

                //Get ReservationNumber
                DocumentAdviceList documentAdviceList = new DocumentAdviceList();
                DateTime today = DateTimeOffset.Now.DateTime;
                documentAdviceList = new hotelsContext().DocumentAdviceLists.Where(a => a.StartDate <= today && a.EndDate >= today && a.DocumentTypeId == 1).FirstOrDefault();

                //Prepare And Save Reservation
                HotelReservationList hotelReservationList = new HotelReservationList() {
                    HotelId = record.Booking.HotelId,
                    ReservationNumber = documentAdviceList.Prefix + (int.Parse(documentAdviceList.Number) + 1).ToString("D" + documentAdviceList.Number.Length.ToString()),
                    GuestId = int.Parse(authId), StatusId = 1, HotelAccommodationActionId = null,
                    StartDate = record.Booking.StartDate, EndDate = record.Booking.EndDate,
                    TotalPrice = record.Booking.TotalPrice, CurrencyId = record.Booking.CurrencyId, Adult = record.Booking.AdultInput, Children = record.Booking.ChildrenInput,
                    FirstName = record.Booking.User.FirstName, LastName = record.Booking.User.LastName, Street = record.Booking.User.Street, Zipcode = record.Booking.User.ZipCode,
                    City = record.Booking.User.City, Country = record.Booking.User.Country, Phone = record.Booking.User.Phone, Email = record.Booking.User.Email,
                    Timestamp = DateTimeOffset.Now.DateTime
                };

                var data = new hotelsContext().HotelReservationLists.Add(hotelReservationList);
                int result = data.Context.SaveChanges();

                //Save New ReservationNumber
                if (result > 0) {
                    documentAdviceList.Number =  (int.Parse(documentAdviceList.Number) + 1).ToString("D" + documentAdviceList.Number.Length.ToString());
                    var data1 = new hotelsContext().DocumentAdviceLists.Update(documentAdviceList);
                    result = data1.Context.SaveChanges();
                } else { /* Error save Registration */}

                //Save Reservation Detail
                if (result > 0) {
                    HotelReservationDetailList hotelReservationDetailList = new HotelReservationDetailList() {
                        GuestId = int.Parse(authId), HotelId = record.Booking.HotelId, ReservationId = hotelReservationList.Id, StatusId = 1, HotelAccommodationActionId = null,
                        StartDate = record.Booking.StartDate, EndDate = record.Booking.EndDate,
                        TotalPrice = record.Booking.TotalPrice, CurrencyId = record.Booking.CurrencyId, Adult = record.Booking.AdultInput, Children = record.Booking.ChildrenInput,
                        Message = record.Booking.Message,GuestSender = true,Shown = false,
                        Timestamp = DateTimeOffset.Now.DateTime
                    };
                    var data2 = new hotelsContext().HotelReservationDetailLists.Add(hotelReservationDetailList);
                    result = data2.Context.SaveChanges();
                }
                else { /* Error save ReservationNumber */}

                //Save Reservation Rooms
                if (result > 0) {

                    record.Booking.Rooms.ForEach(room => {
                        HotelReservedRoomList hotelReservedRoomList = new() {
                            HotelId = record.Booking.HotelId, HotelRoomId = room.Id, Name = room.Name, ReservationId = hotelReservationList.Id, RoomTypeId = room.TypeId, StatusId = 1,
                            StartDate = record.Booking.StartDate, EndDate = record.Booking.EndDate,
                            Count = room.Booked, Timestamp = DateTimeOffset.Now.DateTime,
                            ExtraBed = room.Extrabed
                        };
                        var data3 = new hotelsContext().HotelReservedRoomLists.Add(hotelReservedRoomList);
                        result = data3.Context.SaveChanges();
                    });
                }
                else { /* Error save ReservationDetail */}

                //missing saving control booked rooms from foreach saving

                //Send Reservation Email
                EmailTemplateList template1 = new hotelsContext().EmailTemplateLists.Where(a => a.TemplateName == "reservations").FirstOrDefault();
                MailRequest mailRequest1 = new MailRequest();
                if (template1 != null) {
                    mailRequest1 = new MailRequest() {
                        Subject = (record.Language == "cz" ? template1.SubjectCz : template1.SubjectEn)
                        .Replace("[firstname]", record.Booking.User.FirstName).Replace("[lastname]", record.Booking.User.LastName)
                        .Replace("[email]", record.Booking.User.Email).Replace("[totalprice]", record.Booking.TotalPrice.ToString())
                        .Replace("[hotelname]", record.Booking.HotelName).Replace("[message]", record.Booking.Message).Replace("[currency]", record.Booking.Currency)
                        .Replace("[startdate]", record.Booking.StartDate.ToShortDateString()).Replace("[enddate]", record.Booking.EndDate.ToShortDateString())
                        .Replace("[adult]", record.Booking.AdultInput.ToString()).Replace("[children]", record.Booking.ChildrenInput.ToString())
                        ,
                        Recipients = new List<string>() { record.Booking.User.Email },
                        Content = (record.Language == "cz" ? template1.EmailCz : template1.EmailEn)
                        .Replace("[firstname]", record.Booking.User.FirstName).Replace("[lastname]", record.Booking.User.LastName)
                        .Replace("[email]", record.Booking.User.Email).Replace("[totalprice]", record.Booking.TotalPrice.ToString())
                        .Replace("[hotelname]", record.Booking.HotelName).Replace("[message]", record.Booking.Message).Replace("[currency]", record.Booking.Currency)
                        .Replace("[startdate]", record.Booking.StartDate.ToShortDateString()).Replace("[enddate]", record.Booking.EndDate.ToShortDateString())
                        .Replace("[adult]", record.Booking.AdultInput.ToString()).Replace("[children]", record.Booking.ChildrenInput.ToString())
                    };
                }
                else {
                    mailRequest1 = new MailRequest() {
                        Subject = "Úbytkač Reservation Email",
                        Recipients = new List<string>() { record.Booking.User.Email },
                        Content = "Reservation for " + record.Booking.User.Email + Environment.NewLine + " was success "
                    };
                }
                SystemFunctions.SendEmail(mailRequest1);


                return Ok(JsonSerializer.Serialize(
                new DBResultMessage() {
                    Status = DBResult.success.ToString(),
                    ErrorMessage = string.Empty
                }));
 
            } catch { }
            return BadRequest(new DBResultMessage() {
                Status = DBResult.error.ToString(),
                ErrorMessage = DBOperations.DBTranslate("BookingIsNotValid", record.Language)
            });
        }


        [HttpPost("/WebApi/Guest/Reservation/UnauthSetBooking")]
        [Consumes("application/json")]
        public IActionResult PostUnauthSetBooking([FromBody] BookingRequest record) {
            try {

                //Create New Guest

                GuestList checkExistDisabledGuest = new hotelsContext().GuestLists.Where(a => a.Email == record.Booking.User.Email).FirstOrDefault();

                string password = Functions.RandomString(10);
                GuestList newGuest = new GuestList() {
                    Email = record.Booking.User.Email, Password = BCrypt.Net.BCrypt.HashPassword(password),
                    FirstName = record.Booking.User.FirstName, LastName = record.Booking.User.LastName,
                    Street = record.Booking.User.Street, ZipCode = record.Booking.User.ZipCode, City = record.Booking.User.City, 
                    Country = record.Booking.User.Country, Phone = record.Booking.User.Phone,
                    Active = true, Timestamp = DateTimeOffset.Now.DateTime
                };
                int result = 0;
                if (checkExistDisabledGuest == null) {
                    var insertUser = new hotelsContext().GuestLists.Add(newGuest);
                    result = insertUser.Context.SaveChanges();
                } else {
                    newGuest.Id = checkExistDisabledGuest.Id;
                    var insertUser = new hotelsContext().GuestLists.Update(newGuest);
                    result = insertUser.Context.SaveChanges();
                }

                string authId = newGuest.Id.ToString();

                //Send Registration Email
                if (result > 0) {
                    EmailTemplateList template = new hotelsContext().EmailTemplateLists.Where(a => a.TemplateName == "registration").FirstOrDefault();
                    MailRequest mailRequest = new MailRequest();
                    if (template != null) {
                        mailRequest = new MailRequest() {
                            Subject = (record.Language == "cz" ? template.SubjectCz : template.SubjectEn)
                            .Replace("[firstname]", record.Booking.User.FirstName).Replace("[lastname]", record.Booking.User.LastName)
                            .Replace("[email]", record.Booking.User.Email).Replace("[password]", password),
                            Recipients = new List<string>() { record.Booking.User.Email },
                            Content = (record.Language == "cz" ? template.EmailCz : template.EmailEn)
                            .Replace("[firstname]", record.Booking.User.FirstName).Replace("[lastname]", record.Booking.User.LastName)
                            .Replace("[email]", record.Booking.User.Email).Replace("[password]", password),
                        };
                    }
                    else {
                        mailRequest = new MailRequest() {
                            Subject = "Úbytkač Registration Email",
                            Recipients = new List<string>() { record.Booking.User.Email },
                            Content = "Registration for " + record.Booking.User.Email + Environment.NewLine + " with password " + password
                        };
                    }
                    SystemFunctions.SendEmail(mailRequest);

                } else { /* Error save New Guest */}

                //Get ReservationNumber
                DocumentAdviceList documentAdviceList = new DocumentAdviceList();
                DateTime today = DateTimeOffset.Now.DateTime;
                documentAdviceList = new hotelsContext().DocumentAdviceLists.Where(a => a.StartDate <= today && a.EndDate >= today && a.DocumentTypeId == 1).FirstOrDefault();

                //Prepare And Save Reservation
                HotelReservationList hotelReservationList = new HotelReservationList() {
                    HotelId = record.Booking.HotelId,
                    ReservationNumber = documentAdviceList.Prefix + (int.Parse(documentAdviceList.Number) + 1).ToString("D" + documentAdviceList.Number.Length.ToString()),
                    GuestId = int.Parse(authId), StatusId = 1, HotelAccommodationActionId = null,
                    StartDate = record.Booking.StartDate, EndDate = record.Booking.EndDate,
                    TotalPrice = record.Booking.TotalPrice, CurrencyId = record.Booking.CurrencyId, Adult = record.Booking.AdultInput, Children = record.Booking.ChildrenInput,
                    FirstName = record.Booking.User.FirstName, LastName = record.Booking.User.LastName,
                    Street = record.Booking.User.Street, Zipcode = record.Booking.User.ZipCode, City = record.Booking.User.City, Country = record.Booking.User.Country,
                    Phone = record.Booking.User.Phone, Email = record.Booking.User.Email, Timestamp = DateTimeOffset.Now.DateTime
                };

                var data = new hotelsContext().HotelReservationLists.Add(hotelReservationList);
                result = data.Context.SaveChanges();

                //Save New ReservationNumber
                if (result > 0) {
                    documentAdviceList.Number = (int.Parse(documentAdviceList.Number) + 1).ToString("D" + documentAdviceList.Number.Length.ToString());
                    var data1 = new hotelsContext().DocumentAdviceLists.Update(documentAdviceList);
                    result = data1.Context.SaveChanges();
                }
                else { /* Error save Registration */}

                //Save Reservation Detail
                if (result > 0) {
                    HotelReservationDetailList hotelReservationDetailList = new HotelReservationDetailList() {
                        GuestId = int.Parse(authId), HotelId = record.Booking.HotelId, ReservationId = hotelReservationList.Id,
                        StatusId = 1, HotelAccommodationActionId = null,
                        StartDate = record.Booking.StartDate, EndDate = record.Booking.EndDate,
                        TotalPrice = record.Booking.TotalPrice, CurrencyId = record.Booking.CurrencyId,
                        Adult = record.Booking.AdultInput, Children = record.Booking.ChildrenInput,
                        Message = record.Booking.Message, GuestSender = true,
                        Shown = false, Timestamp = DateTimeOffset.Now.DateTime
                    };
                    var data2 = new hotelsContext().HotelReservationDetailLists.Add(hotelReservationDetailList);
                    result = data2.Context.SaveChanges();
                }
                else { /* Error save ReservationNumber */}

                //Save Reservation Rooms
                if (result > 0) {

                    record.Booking.Rooms.ForEach(room => {
                        HotelReservedRoomList hotelReservedRoomList = new() {
                            HotelId = record.Booking.HotelId, HotelRoomId = room.Id, Name = room.Name,
                            ReservationId = hotelReservationList.Id, RoomTypeId = room.TypeId, StatusId = 1,
                            StartDate = record.Booking.StartDate, EndDate = record.Booking.EndDate,
                            Count = room.Booked, Timestamp = DateTimeOffset.Now.DateTime,
                            ExtraBed = room.Extrabed
                        };
                        var data3 = new hotelsContext().HotelReservedRoomLists.Add(hotelReservedRoomList);
                        result = data3.Context.SaveChanges();
                    });
                }
                else { /* Error save ReservationDetail */}

                //missing saving control booked rooms from foreach saving

                //Send Reservation Email
                EmailTemplateList template1 = new hotelsContext().EmailTemplateLists.Where(a => a.TemplateName == "reservations").FirstOrDefault();
                MailRequest mailRequest1 = new MailRequest();
                if (template1 != null) {
                    mailRequest1 = new MailRequest() {
                        Subject = (record.Language == "cz" ? template1.SubjectCz : template1.SubjectEn)
                        .Replace("[firstname]", record.Booking.User.FirstName).Replace("[lastname]", record.Booking.User.LastName)
                        .Replace("[email]", record.Booking.User.Email).Replace("[totalprice]", record.Booking.TotalPrice.ToString())
                        .Replace("[hotelname]", record.Booking.HotelName).Replace("[message]", record.Booking.Message).Replace("[currency]", record.Booking.Currency)
                        .Replace("[startdate]", record.Booking.StartDate.ToShortDateString()).Replace("[enddate]", record.Booking.EndDate.ToShortDateString())
                        .Replace("[adult]", record.Booking.AdultInput.ToString()).Replace("[children]", record.Booking.ChildrenInput.ToString())
                        ,
                        Recipients = new List<string>() { record.Booking.User.Email },
                        Content = (record.Language == "cz" ? template1.EmailCz : template1.EmailEn)
                        .Replace("[firstname]", record.Booking.User.FirstName).Replace("[lastname]", record.Booking.User.LastName)
                        .Replace("[email]", record.Booking.User.Email).Replace("[totalprice]", record.Booking.TotalPrice.ToString())
                        .Replace("[hotelname]", record.Booking.HotelName).Replace("[message]", record.Booking.Message).Replace("[currency]", record.Booking.Currency)
                        .Replace("[startdate]", record.Booking.StartDate.ToShortDateString()).Replace("[enddate]", record.Booking.EndDate.ToShortDateString())
                        .Replace("[adult]", record.Booking.AdultInput.ToString()).Replace("[children]", record.Booking.ChildrenInput.ToString())
                    };
                }
                else {
                    mailRequest1 = new MailRequest() {
                        Subject = "Úbytkač Reservation Email",
                        Recipients = new List<string>() { record.Booking.User.Email },
                        Content = "Reservation for " + record.Booking.User.Email + Environment.NewLine + " was success "
                    };
                }
                SystemFunctions.SendEmail(mailRequest1);

                return Ok(JsonSerializer.Serialize(
                new DBResultMessage() {
                    Status = password,
                    ErrorMessage = string.Empty
                }));

            } catch { }
            return BadRequest(new DBResultMessage() {
                Status = DBResult.error.ToString(),
                ErrorMessage = DBOperations.DBTranslate("BookingIsNotValid", record.Language)
            });
        }
    }
}