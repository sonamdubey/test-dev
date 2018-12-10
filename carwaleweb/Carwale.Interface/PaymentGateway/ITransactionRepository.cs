using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.Entity.PaymentGateway;

namespace Carwale.Interfaces.PaymentGateway
{
    public interface ITransactionRepository
    {
        long BeginTransaction(TransactionDetails _inputs);
        TransactionDetails CompleteTransaction(GatewayResponse _inputs);
        void InsertConsumerInvoice(int pgtransId, int pkgReqId, TransactionDetails transaction);
        void UpdateInquiriesPaymentMode(ulong carid, int transctionType);
    }
}
