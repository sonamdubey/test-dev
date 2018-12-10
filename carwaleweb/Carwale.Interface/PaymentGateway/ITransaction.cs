using Carwale.Entity.PaymentGateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.PaymentGateway
{
    public interface ITransaction
    {
        string BeginTransaction(TransactionDetails _details);
        bool CompleteBillDeskTransaction();
        bool CompleteTransactionUsed(GatewayResponse response);
    }
}
