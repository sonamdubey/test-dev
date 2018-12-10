using System;
using Carwale.Entity.PaymentGateway;
using Carwale.Interfaces.PaymentGateway;
using System.Configuration;
using Carwale.Interfaces.Classified.SellCar;
using Carwale.Notifications.Logs;
using Carwale.Utility;

namespace Carwale.BL.PaymentGateway
{
    public class Transaction : ITransaction
    {
        private readonly IPaymentGateway _pg;
        private readonly ITransactionRepository _transactionRepo;
        private readonly ITransactionValidator _transactionVal;
        private readonly IPackageRepository _pkgRepo;
        private readonly ISellCarRepository _sellCarRepository;

        public Transaction(IPaymentGateway pg, ITransactionRepository transactionRepo, ITransactionValidator transactionVal, IPackageRepository pkgRepo, ISellCarRepository sellCarRepository)
        {
            _pg = pg;
            _transactionRepo = transactionRepo;
            _transactionVal = transactionVal;
            _pkgRepo = pkgRepo;
            _sellCarRepository = sellCarRepository;
        }

        public string BeginTransaction(TransactionDetails transaction)
        {
            if (_transactionVal.ValidateTransDetails(transaction))
            {   
                transaction.PGRecordId = _transactionRepo.BeginTransaction(transaction);
            }
            else
            {
                return "Invalid information!";
            }
            // Websrever ==> CWL
            // Production ==> CW
            // Staging ==> CWUAT

            transaction.UniqueTransactionId += transaction.PGRecordId;

            if (transaction.PGRecordId > 0)
            {
                if (!(transaction.PlatformId == 74 && !BrowserUtils.IsWebView()))
                {
                    PGCookie.PGRecordId = transaction.PGRecordId.ToString();
                    _pg.Request(transaction);
                }
                return "Transaction initiation Successfull!";
            }

            return "Transaction Failure";
        }

        public bool CompleteBillDeskTransaction()
        {
            var pgResponse = _pg.GetResponse();

            PGCookie.PGRespCode = pgResponse.PGRespCode;
            PGCookie.PGMessage = pgResponse.PGMessage;
            PGCookie.PGTransId = pgResponse.PGTransId;
            pgResponse.PGId = Convert.ToUInt64(PGCookie.PGCarId);

            if (pgResponse.PGId > 0)
            {
                CompleteTransaction(pgResponse);
                return true;
            }
            return false;
        }

        //It is used from cptcwl.aspx for approving used car that are not live to payment failure
        public bool CompleteTransactionUsed(GatewayResponse response)
        {
            int consumerType = 2;
            CompleteTransaction(response);
            int consumerId = _sellCarRepository.GetCustomerSellInquiryData(Convert.ToInt32(response.PGId)).Id;
            if (consumerId > 0 && response.PGRespCode == "0")
            {
                Logger.LogInfo(String.Format("Transaction.cs - call package change SP for consumer id {0}, package Id = {1}", consumerId, PGCookie.PGPkgId));
                _pkgRepo.ChangePackage(response.PGId, Convert.ToUInt64(consumerId), Convert.ToInt32(PGCookie.PGPkgId));
                if (_pkgRepo.UpgradePackageTypeToListingType(consumerType, response.PGId, Convert.ToUInt64(consumerId)) == true)
                {
                    PGCookie.SendInqNotification = "1";
                }
                return true;
            }
            return false;
        }

        private void CompleteTransaction(GatewayResponse pgResponse)
        {
            TransactionDetails transaction = null;
            transaction = _transactionRepo.CompleteTransaction(pgResponse);
            if (transaction != null)
            {
                if (transaction.ConsumerType != 1)
                    _pkgRepo.GetCustomerInfoByCustomerID(transaction);
                _transactionRepo.InsertConsumerInvoice(Convert.ToInt32(pgResponse.PGTransId), _pkgRepo.InsertConsumerPackageRequests(transaction, Convert.ToInt32(pgResponse.PGRespCode)), transaction);
            }
        }
    }
}

