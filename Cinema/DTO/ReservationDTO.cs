using System.Security.Policy;

namespace Cinema.DTO
{
    public class ReservationDTO
    {
        public string? userId { get; set; }
        public int movieShowId { get; set; }
        public int seatRow {  get; set; }
        public int seatColumn {  get; set; }
        public string? email {  get; set; }
        public string? fullName { get; set; }
        public bool? isPaid { get; set; }
        public string? status { get; set; }
        public DateTime dateUpdate { get; set; } = DateTime.UtcNow;

    }
}
