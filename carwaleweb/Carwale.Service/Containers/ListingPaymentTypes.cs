using Carwale.BL.Classified.ListingPayment;
using Carwale.DAL.Classified.ListingPayment;
using Carwale.Interfaces.Classified.ListingPayment;
using Microsoft.Practices.Unity;

namespace Carwale.Service.Containers
{
    public static class ListingPaymentTypes
    {
        public static void RegisterTypes(UnityContainer container)
        {
            container
                .RegisterType<IReceiptRepository, ReceiptRepository>()
                .RegisterType<IReceiptLogic, ReceiptLogic>();
        }
    }
}
