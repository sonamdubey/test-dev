using Carwale.Entity.Classified.Leads;
using Carwale.Entity.Enum;
using System.Collections.Generic;

namespace Carwale.Interfaces.Classified.Leads
{
    public interface ILeadRepository
    {
        LeadStatus CheckLeadStatus(string mobile, string ip, int inquiryId, bool isDealer, out int leadId);
        int InsertLead(LeadDetail lead, int buyerCity, BuyerInfo buyerInfo, bool isDuplicate);
        void InsertUnverifiedLead(LeadDetail lead);
        bool InsertLeadLog(int leadId, ClassifiedStockSource sourceTable, UsedLeadPushApiSource apiSource);
        bool InsertLeadTracking(int leadId, UsedLeadType leadType, int abCookie);
        bool ShouldResendNotification(int leadId, bool isDealer);
        void InsertLeadNotifications(int leadId, bool isDealer);
        IEnumerable<Lead> GetStockLeads(string mobile, int count);
        IEnumerable<Lead> GetDealerStockLeads(string mobile, int count, bool isOrderAsc, int dealerId);
        List<ClassifiedRequest> GetClassifiedRequests(int inquiryId, int requestDate);
        bool UpdateInquiryStatus(int inquiryId, int inquiryType, int customerId, int statusId);
        bool IsLeadGivenToDealer(string mobile, string chatTokenId);
    }
}
