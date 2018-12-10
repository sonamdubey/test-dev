using Carwale.Entity.PaymentGateway;
using Carwale.Interfaces.PaymentGateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Carwale.Service.Controllers.PaymentGateway
{
    public class PaymentGateway : ApiController 
    {
        private readonly IPaymentGateway _paymentGateway;
        private readonly ITransaction _transaction;
        public PaymentGateway(IPaymentGateway paymentGateway, ITransaction transaction)
        {
            _paymentGateway = paymentGateway;
            _transaction = transaction;
        }

        [HttpGet, Route("api/paymentgateway/begintransaction")]
        public IHttpActionResult BeginTransaction(TransactionDetails transaction)
        {
            return Ok(_transaction.BeginTransaction(transaction));
        }

        [HttpGet, Route("api/paymentgateway/completetransaction")]
        public IHttpActionResult CompleteTransaction(TransactionDetails transaction)
        {
            return Ok(_transaction.CompleteTransaction());
        }
    }
}
