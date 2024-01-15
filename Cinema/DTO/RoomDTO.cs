namespace Cinema.DTO
{
    public class RoomDTO
    {
        public int roomNo { get; set; }
        public int rows { get; set; }
        public int seatsInRow { get; set; }
        public DateTime dateUpdate { get; set; } = DateTime.UtcNow;
    }
}
