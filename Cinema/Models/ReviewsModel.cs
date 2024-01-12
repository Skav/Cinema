namespace Cinema.Models
{
    public class ReviewsModel : BasicModel
    {
        public int movieId { get; set; }
        public string userId {  get; set; }
        public int rating { get; set; }
        public string content { get; set; }
        public virtual MoviesModel Movie { get; set; }
        public virtual UsersModel User { get; set; }

    }
}
