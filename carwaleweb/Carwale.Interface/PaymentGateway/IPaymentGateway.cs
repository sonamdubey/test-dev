using Carwale.Entity.PaymentGateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.PaymentGateway
{
    public interface IPaymentGateway
    {
        void Request(TransactionDetails _details);
        GatewayResponse GetResponse();
    }
}

