using Carwale.Entity.Classified;
using Carwale.Entity.PaymentGateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.PaymentGateway
{
    public interface IPackageRepository
    {
        bool ChangePackage(ulong inquiryId, ulong consumerId, int packageId);
        bool UpgradePackageTypeToListingType(int consumerType, ulong carId, ulong consumerId);
        void GetCustomerInfoByCustomerID(TransactionDetails transaction);
        Package GetPackageDetails(int packageId);
        int InsertConsumerPackageRequests(TransactionDetails transaction,int responseCode);
        List<MyPaymentsEntity> GetPaymentsDetails(int customerId);
        InvoiceDetails GetInvoiceDetails(int customerId, int invoiceId);
    }
}
