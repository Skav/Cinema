namespace Cinema.Models
{
    public class MoviesModel : BasicModel
    {
        public string title {  get; set; }
        public int duration { get; set; }
        public string genre { get; set; }
        public string description { get; set; }
        public bool available { get; set; }
        public virtual ICollection<MovieShowModel> MoviesShows { get; set; }
        public virtual ICollection<ReviewsModel> Reviews { get; set; }

        public MoviesModel()
        {
            MoviesShows = new List<MovieShowModel>();
            Reviews = new List<ReviewsModel>();
        }

        public MoviesModel(string title, int duration, string genre, bool available)
        {
            this.title = title;
            this.duration = duration;
            this.genre = genre;
            this.available = available;
            MoviesShows = new List<MovieShowModel>();
            Reviews = new List<ReviewsModel>();
        }


    }
}
