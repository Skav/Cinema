using Cinema.Models;

namespace Cinema.Models
{
    public class CouponsModel : BasicModel
    {
        public string userId { get; set; }
        public int discount { get; set; }
        public string discountType { get; set; }
        public bool active { get; set; }
        public DateTime expDate { get; set; }
        public virtual UsersModel Users { get; set; }
    }
}
