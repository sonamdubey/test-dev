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
        public CarTradeVoucherStatus Status { get; set; }
    }
    internal enum CarTradeVoucherStatus
    {
        Pre_Approved = 3,
        Rejected = 4,
        Credit_Refer = 5
    }
}
