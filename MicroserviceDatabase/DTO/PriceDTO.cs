using System.IO;

namespace MicroserviceDatabase.DTO
{
    public class PriceDTO
    {
        public List<PriceInfo>? prices { get; set; }
    }

    public class PriceInfo
    {
        public decimal price { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
    }
}
