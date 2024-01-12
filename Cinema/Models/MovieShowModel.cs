namespace Cinema.Models
{
    public class MovieShowModel : BasicModel
    {
        public int roomId { get; set; }
        public int movieId { get; set; }
        public DateTime date { get; set; }
        public string hour { get; set; }
        public virtual ICollection<ReservationModel> Reservation { get; set; }
        public virtual RoomsModel Room { get; set; }
        public virtual MoviesModel Movie { get; set; }

        public MovieShowModel()
        {
            Reservation = new List<ReservationModel>();
        }

    }
}
