using System;

namespace Bikewale.Entities.Customer
{
    public class CancelledBikeCustomer : CustomerEntityBase
    {
        public string BikeName { get; set; }
        public string BookingDate { get; set; }
        public UInt16 isCancellable { get; set; }
        public string DealerName { get; set; }
        public string BWId { get; set; }
        public uint TransactionId { get; set; }
        public string CityName { get; set; }
        public uint PQId { get; set; }
    }
}
