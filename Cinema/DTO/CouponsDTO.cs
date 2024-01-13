namespace Cinema.DTO
{
    public class CouponsDTO
    {
        public string userId {  get; set; }
        public int discount { get; set; }
        public string discountType { get; set; }
        public bool active { get; set; }
        public DateTime expDate { get; set; }
        public DateTime dateUpdate { get; set; } = DateTime.UtcNow;
    }
}
