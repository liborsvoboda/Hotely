namespace TravelAgencyBackEnd.Models.ViewModels {

    public class ReviewModel {
        public int HotelId { get; set; }
        public int ReservationId { get; set; }
        public int GuestId { get; set; }
        public float Rating { get; set; }
        public string Description { get; set; }
    }
}