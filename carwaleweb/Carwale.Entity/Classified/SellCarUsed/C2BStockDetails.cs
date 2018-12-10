using Carwale.Entity.Classified.CarDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Classified.SellCarUsed
{
    public class C2BStockDetails
    {
        public BasicCarInfo CarInfo { get; set; }
        public Customer CustomerInfo { get; set; }
        public int HotLeadPrice { get; set; }
        public short SourceId { get; set; }
        public string Pincode { get; set; }
        public decimal? Lattitude { get; set; }
        public decimal? Longitude { get; set; }
        public string StateName { get; set; }
        public byte? IsVerified { get; set; }
        public bool? PaymentStatus { get; set; }
        public int? PaymentAmount { get; set; }
        public string PaymentDate { get; set; }
        public string TransactionId { get; set; }
    }
}
