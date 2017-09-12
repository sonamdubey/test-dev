using System;

namespace Bikewale.RabbitMq.CapitalFirstLeadConsumer
{
    internal class CarTradeVoucher
    {
        public string LeadId { get; set; }
        public string AgentContactNumber { get; set; }
        public string AgentName { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string VoucherCode { get; set; }
    }
}
