namespace Cinema.Models
{
    public class RoomsModel : BasicModel
    {
        public int roomNo { get; set; }
        public int rows { get; set; }
        public int seatsInRow { get; set; }
        public virtual ICollection<MovieShowModel> MoviesShows { get; set; }

        public RoomsModel()
        {
            MoviesShows = new List<MovieShowModel>();
        }
    }
}
