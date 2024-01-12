namespace Cinema.Models
{
    public class LoyalityPointsModel : BasicModel
    {
        public string userId {  get; set; }
        public int amountOfPoints {  get; set; }
        public virtual UsersModel User { get; set; }

    }
}
