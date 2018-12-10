using Carwale.Entity.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.PaymentGateway
{
    public class GatewayResponse : CustomerMinimal
    {
        public string PGRespCode { get; set; }
        public string PGMessage { get; set; }
        public string PGEPGTransId { get; set; }
        public string PGAuthIdCode { get; set; }
        public string PGTransId { get; set; }
        public ulong PGId { get; set; }
        public long PGRecordId { get; set; }
        public bool IsTransactionCompleted { get; set; }
    }
}





