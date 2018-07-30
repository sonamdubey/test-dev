using Bikewale.Entities.BikeBooking;
using Bikewale.Notifications;
using Carwale.BL.PaymentGateway;
using Carwale.DAL.PaymentGateway;
using Carwale.Entity.PaymentGateway;
using Carwale.Interfaces.PaymentGateway;
using Microsoft.Practices.Unity;
using System;

namespace Bikewale.BAL.BikeBooking
{
    public class BrowserBikeBooking : AbstractBikeBooking
    {
        protected override BookingResults DoBookingEx(TransactionDetails entity, string sourceType)
        {
            TransactionDetails transaction = null;
            ITransaction begintrans = null;
            IUnityContainer container = null;
            string transResponse = String.Empty;

            try
            {
                transaction = entity;
                container = new UnityContainer();
                container.RegisterType<ITransaction, Transaction>()
                .RegisterType<ITransactionRepository, TransactionRepository>()
                .RegisterType<IPackageRepository, PackageRepository>()
                .RegisterType<ITransactionValidator, ValidateTransaction>();

                if (!String.IsNullOrEmpty(sourceType) && sourceType == "3")
                {
                    container.RegisterType<IPaymentGateway, BillDesk>();
                    transaction.SourceId = Convert.ToInt16(sourceType);
                }

                begintrans = container.Resolve<ITransaction>();

                transResponse = begintrans.BeginTransaction(transaction);

                if (transResponse == "Transaction Failure")
                {
                    return BookingResults.TransactionFailure;
                }
                else if (transResponse == "Invalid information!")
                {
                    return BookingResults.InvalidInformation;
                }
                else
                {
                    return BookingResults.Success;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Ex Bikewale.BAL.BikeBooking.BrowserBikeBooking.DoBookingEx");
                
                return BookingResults.GenericFailure;
            }
        }
    }
}
