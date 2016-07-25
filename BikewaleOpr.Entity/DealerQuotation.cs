
using BikewaleOpr.Entities;
namespace BikewaleOpr.Entities
{
    public class DealerQuotation
    {
        public NewBikeDealers Dealer { get; set; }
        public uint BookingAmount { get; set; }
        public uint Availability { get; set; }
        public uint DealerId { get; set; }
    }
}
