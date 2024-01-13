using Npgsql;

namespace Cinema.Models
{
    public class BasicModel
    {
        public int id { get; set; }
        public DateTime dateAdded { get; set; }
        public DateTime dateUpdate {  get; set; } = DateTime.UtcNow;

        public BasicModel()
        {
            
        }
    }
}
