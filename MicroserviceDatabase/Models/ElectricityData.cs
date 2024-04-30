using System.ComponentModel.DataAnnotations;

namespace MicroserviceDatabase.Models
{
    public class ElectricityData
    {
        [Key]
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Price { get; set; }
    }
}
