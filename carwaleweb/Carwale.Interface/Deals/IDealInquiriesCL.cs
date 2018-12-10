using Carwale.DTOs.Deals;
using Carwale.Entity;
using Carwale.Entity.Deals;
using Carwale.Entity.PaymentGateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.Deals
{
    public interface IDealInquiriesCL
    {
        DealsStockDTO ThankYou(string respcd, string transId, bool paymentSuccess, string sourceIdForAutobiz);
        int PushLead(DealsInquiryDetailDTO dealsInquiry);
        bool PushMultipleLeads(DealsInquiryDetailDTO dealsInquiry);
        string BeginTransaction(DealsInquiryDetail dealsInquiry, int responseId, int sourceId);
        bool UpdateCustInfo(int inquiryId, DealsInquiryDetail dealsInquiry);
        void ProcessTransactionResults_App(GatewayResponse pgResponse);
        TransactionDetails GetTransactionDetails(DealsInquiryDetailDTO dealsInquiry);
        string GetSDKMessage(DealsInquiryDetailDTO dealsInquiry, TransactionDetails transaction);
    }
}
