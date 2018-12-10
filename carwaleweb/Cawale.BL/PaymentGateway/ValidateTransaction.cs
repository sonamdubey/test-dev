using Carwale.Entity.PaymentGateway;
using Carwale.Interfaces.PaymentGateway;

namespace Carwale.BL.PaymentGateway
{
    public class ValidateTransaction : ITransactionValidator
    {
        public bool ValidateTransDetails(TransactionDetails transaction)
        {
            if (transaction == null) return false;

            return !string.IsNullOrWhiteSpace(transaction.CustomerName) &&
                   !string.IsNullOrWhiteSpace(transaction.CustCity) &&
                   !string.IsNullOrWhiteSpace(transaction.CustEmail) &&
                   !string.IsNullOrWhiteSpace(transaction.CustMobile) &&
                   (transaction.ConsumerType >= 0) &&
                   (transaction.PackageId > 0) &&
                   (transaction.Amount > 0) &&
                   (transaction.PGId > 0) &&
                   (transaction.CustomerID > 0) &&
                   (transaction.SourceId > 0);
        }
    }
}
