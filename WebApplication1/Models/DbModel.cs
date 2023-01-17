using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class DbModel
    {
        [Key]
        public int ID { get; set; }
        public int ticketID { get; set; }
        public string status { get; set; }
        public string category { get; set; }
        public string VisualDescription { get; set; }

    }
}
