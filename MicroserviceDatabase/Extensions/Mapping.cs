using MicroserviceDatabase.DTO;
using MicroserviceDatabase.Models;

namespace MicroserviceDatabase.Extensions
{
    public static class Mapping
    {
        public static ElectricityData ToEntity(this PriceInfo priceInfo)
        {
            return new ElectricityData
            {
                StartDate = priceInfo.startDate,
                EndDate = priceInfo.endDate,
                Price = priceInfo.price
            };
        }
    }
}
