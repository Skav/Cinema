using Microsoft.AspNetCore.Identity;

namespace Cinema.Models
{
    public class UsersModel : IdentityUser
    {
        public virtual ICollection<ReviewsModel> Reviews { get; set; }
        public virtual ICollection<CouponsModel> Coupons { get; set; }
        public virtual ICollection<LoyalityPointsModel> LoyalityPoints { get; set; }
        public virtual ICollection<ReservationModel> Reservations { get; set; }

        public UsersModel()
        {
            Reviews = new HashSet<ReviewsModel>();
            Coupons = new HashSet<CouponsModel>();
            LoyalityPoints = new HashSet<LoyalityPointsModel>();
            Reservations = new HashSet<ReservationModel>();
        }
    }
}
