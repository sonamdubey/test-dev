using Carwale.Entity.Classified.ListingPayment;

namespace Carwale.Interfaces.Classified
{
    public interface IClassifiedMails
    {
        void SendUpgradeNotification<T>(T model, string email);
        void SendReceiptMail(Receipt receipt, string packageName);
    }
}
