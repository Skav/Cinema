namespace Cinema.DTO
{
    public class MovieShowDTO
    {
        public int roomId { get; set; }
        public int movieId { get; set; }
        public DateTime date { get; set; }
        public string hour { get; set; }
        public DateTime dateUpdate { get; set; } = DateTime.UtcNow;
    }
}
