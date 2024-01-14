namespace Cinema.DTO
{
    public class ReviewsDTO
    {
        public int movieId { get; set; }
        public string? userId { get; set; }
        public int rating {  get; set; }
        public string content { get; set; }
        public DateTime dateUpdate { get; set; } = DateTime.UtcNow;
    }
}
