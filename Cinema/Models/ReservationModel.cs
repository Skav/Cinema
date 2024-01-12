namespace Cinema.Models
{
    public class ReservationModel : BasicModel
    {
        public string userId { get; set; }
        public int movieShowId { get; set; }
        public int seatRow {  get; set; }
        public int seatColumn { get; set; }
        public string email { get; set; }
        public string fullName {  get; set; }
        public bool isPaid {  get; set; }
        public string status {  get; set; }
        public DateTime claimDate { get; set; }
        public virtual UsersModel User { get; set; }
        public virtual MovieShowModel MovieShow { get; set; }
    }
}
