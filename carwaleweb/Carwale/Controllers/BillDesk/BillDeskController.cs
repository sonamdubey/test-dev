using Carwale.Entity.PaymentGateway;
using Carwale.Interfaces.Deals;
using Carwale.Interfaces.PaymentGateway;
using Carwale.Notifications;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Carwale.UI.Controllers.BillDesk
{
    public class BillDeskController : Controller
    {
        // GET: BillDesk
        private readonly IPaymentGateway _paymentGateway;
        private readonly ITransactionRepository _transactionRepo;
        private readonly IDealInquiriesCL _dealsInquiriesCL;
        private readonly IPackageRepository _pkgRepo;

        public BillDeskController(IPaymentGateway paymentGateway, ITransactionRepository transactionRepo, IDealInquiriesCL dealsInquiriesCL, IPackageRepository packageRepository)
        {
            _paymentGateway =  paymentGateway;
            _transactionRepo = transactionRepo;
            _dealsInquiriesCL = dealsInquiriesCL;
            _pkgRepo = packageRepository;
        }

        [Route("billdesk/response.aspx")]
        public ActionResult Index()
        {
            var pgResponse = _paymentGateway.GetResponse();
            pgResponse.PGTransId = pgResponse.PGTransId.Replace(ConfigurationManager.AppSettings["OfferUniqueTransaction"].ToString(), string.Empty);
            TransactionDetails transaction = null;
            transaction = _transactionRepo.CompleteTransaction(pgResponse);
            if (transaction != null)
            {
                if (transaction.ConsumerType != 1)
                    _pkgRepo.GetCustomerInfoByCustomerID(transaction);
                _transactionRepo.InsertConsumerInvoice(Convert.ToInt32(pgResponse.PGTransId), _pkgRepo.InsertConsumerPackageRequests(transaction, Convert.ToInt32(pgResponse.PGRespCode)), transaction);
            }
            _dealsInquiriesCL.ProcessTransactionResults_App(pgResponse);
            return View();
        }
    }
}