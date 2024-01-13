using Cinema.Models;

namespace Cinema.DTO
{
    public class LoyalityPointsDTO
    {
        public string userId { get; set; }
        public int amountOfPoints {  get; set; }
        public DateTime dateUpdate { get; set; } = DateTime.UtcNow;
    }
}
