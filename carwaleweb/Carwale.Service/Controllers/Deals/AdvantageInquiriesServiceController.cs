using Carwale.DTOs;
using Carwale.Interfaces;
using Carwale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.Utility;
using System.Web.Http;
using Carwale.Entity.Enum;
using Carwale.Entity;
using Carwale.Service.Filters;
using Carwale.DTOs.Deals;
using System.Web;
using Carwale.Interfaces.Deals;
using AutoMapper;
using Carwale.Entity.PaymentGateway;
using System.Configuration;
using Carwale.Interfaces.PaymentGateway;
using Carwale.BL.PaymentGateway;
using System.Web.Http.Cors;

namespace Carwale.Service.Controllers.Deals
{
    [EnableCors(origins: "http://webserver:8082, http://oprst.carwale.com,http://opr.carwale.com,http://172.16.2.114:8081,http://localhost:8081,http://localhost:8082", headers: "*", methods: "*")]
    public class AdvantageInquiriesServiceController : ApiController
    {
        private readonly IDealsUserInquiry<DropOffInquiryDetailDTO> _dealsInquiry;
        private readonly  IDealInquiriesCL _dealInquiriesCL;
        private readonly ITransaction _transaction;
        private readonly IDeals _dealsBL;
        public AdvantageInquiriesServiceController(IDealsUserInquiry<DropOffInquiryDetailDTO> dealsInquiry, IDealInquiriesCL dealInquiriesCL, ITransaction transaction, IDeals dealsBL )			
        {
            _dealsBL = dealsBL;
            _dealsInquiry = dealsInquiry;
            _dealInquiriesCL = dealInquiriesCL;
            _transaction = transaction;
        }
        /// <summary>
        /// For getting Deals Inquiry Detail
        /// Created: Chetan Thambad, 12/01/2016
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("api/deals/dealinquiries/droppedoffusers")]
        public IHttpActionResult GetDroppedOffUsers()
        {
            try
            {
                // if (ValidIp.IsIPValid())
                // {
                var GetUsers = _dealsInquiry.GetDealsDroppedUsers();
                return Ok(GetUsers);
                // }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "DealsInquiryController.GetDroppedOffUsers()");
                objErr.SendMail();
            }
            return null;
        }

        [HttpPost, Route("api/advantage/inquiry")]
        [ValidateSourceFilter]
        public IHttpActionResult SubmitLeads([FromBody] DealsInquiryDetailDTO dealsInquiry)
        {
            if (dealsInquiry == null || (dealsInquiry.StockId <= 0 && dealsInquiry.CustomerName != null && dealsInquiry.CustomerMobile != null && dealsInquiry.CustomerEmail != null))
                return BadRequest("There was a descrepency in data passed.");
            int sourceId;
            Int32.TryParse(Request.Headers.GetValues("sourceid").FirstOrDefault(), out sourceId);
            if(sourceId == (int)Platform.CarwaleAndroid || sourceId == (int)Platform.CarwaleiOS )
            {
                _dealsBL.PushLeadToAutobiz(dealsInquiry, false, (int)LeadSourceIdForAutobiz.CarwaleUnpaidLead);
            }
            dealsInquiry.PlatformId = sourceId;

            var inquiryId = _dealInquiriesCL.PushLead(dealsInquiry);

            if (inquiryId > 0)
                return Ok(new { inquiryId = inquiryId });
            else
                return BadRequest("There was some error in processing the Request.");
        }


        [HttpPost, Route("api/advantage/inquiries")]
        [ValidateSourceFilter]
        public IHttpActionResult SubmitMultipleLeads([FromBody] DealsInquiryDetailDTO dealsInquiry)
        {
            if (dealsInquiry == null || (dealsInquiry.MultipleStockId ==null && dealsInquiry.CustomerName != null && dealsInquiry.CustomerMobile != null && dealsInquiry.CustomerEmail != null))
                return BadRequest("There was a descrepency in data passed.");

            IEnumerable<string> requestSource;
            int sourceId = 0;
            Request.Headers.TryGetValues("sourceid", out requestSource);
            Int32.TryParse(requestSource.ToList()[0], out sourceId);
            dealsInquiry.PlatformId = sourceId;

            var inquiryId = _dealInquiriesCL.PushMultipleLeads(dealsInquiry);
            if (inquiryId == true)
                return Ok(new { inquiryId = inquiryId });
            else
                return BadRequest("There was some error in processing the Request.");
        }
        

        [HttpPost, Route("api/advantage/beginTransaction")]
        public IHttpActionResult BeginTransaction([FromBody] DealsInquiryDetailDTO dealsInquiry)
        {
           
            string transresp = string.Empty;
              
            var transaction = _dealInquiriesCL.GetTransactionDetails(dealsInquiry);

            transresp = _transaction.BeginTransaction(transaction);

            string message = _dealInquiriesCL.GetSDKMessage(dealsInquiry, transaction);
            
            if (transresp != "Transaction Failure" && transresp != "Invalid information!")
                return Ok(new { msg = message , transactionId = transaction.PGRecordId });
            else
                return BadRequest("Transaction Failed");
        }
    }
}
