namespace Cinema.DTO
{
    public class MovieDTO
    {
        public string title { get; set; }
        public int duration { get; set; }
        public string genre { get; set; }
        public string description { get; set; }
        public bool available { get; set; }
        public DateTime dateUpdate { get; set; } = DateTime.UtcNow;

    }
}
