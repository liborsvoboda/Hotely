using TABackend.DBModel;
using TABackend.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TABackend.Services
{
    public class ReservationService
    {
        private readonly hotelsContext _db;
        public ReservationService()
        {
            _db = new hotelsContext();
        }

        public IEnumerable<Reservation> FindReservation(int id)
        {
            return _db.Reservations.Where(x => x.GuestId == id).AsEnumerable();
        }
        
       
        public IEnumerable<ReservationsDetail> FindReservationDetails(int id)
        {
            return _db.ReservationsDetails.Where(x => x.ReservationId == id).AsEnumerable();
        }

        
    }
}
