using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.Entity.PaymentGateway;

namespace Carwale.Interfaces.PaymentGateway
{
    public interface ITransactionValidator
    {
        bool ValidateTransDetails(TransactionDetails transaction);
    }
}


