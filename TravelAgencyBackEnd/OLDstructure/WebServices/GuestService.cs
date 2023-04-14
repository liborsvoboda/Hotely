using TravelAgencyBackEnd.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using TravelAgencyBackEnd.Services;
using TravelAgencyBackEnd.DBModel;

namespace TravelAgencyBackEnd.Services
{
    public class GuestService
    {

        private readonly hotelsContext _db;

        public GuestService()
        {
            _db = new hotelsContext();
        }

        /*
        public IEnumerable<SavedHotel> GetSavedHotels(int id)
        {
            return _db.SavedHotels.Where(n => n.GuestId == id).AsEnumerable();
        }
        */
        public IEnumerable<GuestList> GetGuestById(int id)
        {
            return _db.GuestLists.Where(g => g.Id == id);
        }

        // OLDVERSION Replaced by New
        //public void AddGuest(AddGuestViewModel guest)
        //{

        //    var newGuest = new GuestList()
        //    {
        //        FullName = guest.FullName,
        //        Street = guest.Street,
        //        ZipCode = guest.ZipCode,
        //        City = guest.City,
        //        Country = guest.Country,
        //        Phone = guest.Phone,
        //        Email = guest.Email
        //    };

        //    _db.GuestLists.Add(newGuest);
        //    _db.SaveChanges();
        //}

        public async Task<int> EditGuest(GuestList guest)
        {
            _db.GuestLists.Update(guest);
            return await _db.SaveChangesAsync();
        }

        public int AddReview(ReviewModel model)
        {
            var newReview = new HotelReservationReviewList()
            {
                HotelId = model.HotelId,
                ReservationId = model.ReservationId,
                GuestId = model.GuestId,
                Rating = model.Rating,
                Description = model.Description
            };

            _db.HotelReservationReviewLists.Add(newReview);
            return _db.SaveChanges();
        }
        /*
        public int SaveHotel(SaveModel model)
        {
            var newSaveHotel = new SavedHotel()
            {
                HotelId = model.HotelID,
                GuestId = model.GuestID,
            };

            _db.SavedHotels.Add(newSaveHotel);
            return _db.SaveChanges();
        }

        public int RemoveSavedHotel(SaveModel model)
        {
            var newRemoveHotel = new SavedHotel()
            {
                HotelId = model.HotelID,
                GuestId = model.GuestID,
            };

            _db.SavedHotels.Remove(newRemoveHotel);
            return _db.SaveChanges();
        }
        */
        public GuestList FindGuestById(int id)
        {
            return _db.GuestLists.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<HotelReservationList> GetReservationsByID(int id)
        {
            var result = _db.HotelReservationLists.Include(r => r.Guest).Include(h => h.Hotel).Where(r => r.Id == id).AsEnumerable();

            var test = _db.HotelReservationLists.Where(r => r.GuestId == id).Include(r => r.Hotel).ThenInclude(r => r.HotelRoomLists).AsEnumerable();

            return test;
        }


        // OLDVERSION Replaced by New
        //public LoginResponseViewModel Login(LoginRequestViewModel model)
        //{
        //    GuestList user =null;
        //    bool isValid = false;
        //    if (model.UserId !=0)
        //    {
        //        user = _db.GuestLists.FirstOrDefault(x => x.Id == model.UserId);
        //        if (user != null)
        //        {
        //            return new LoginResponseViewModel(user);
        //        }
        //    }
        //    else
        //    {
        //        user = _db.GuestLists.FirstOrDefault(x => x.Email == model.Email);
                
        //        isValid = BCrypt.Net.BCrypt.Verify(model.Password, user.Password);
        //        if (isValid==false)
        //        {
        //            return null;
        //        }
        //    }

        //    return new LoginResponseViewModel(user);
        //}

        public void RemoveGuest(int id)
        {
            const string deleted = "deleted";
            var customer = _db.GuestLists.SingleOrDefault(x => x.Id == id);
            customer.FullName = deleted;
            customer.Street = deleted;
            customer.ZipCode = deleted;
            customer.City = deleted;
            customer.Country = deleted;
            customer.Phone = deleted;
            customer.Email = deleted;
            customer.Password = deleted;

            _db.SaveChanges();
        }
    }

}

