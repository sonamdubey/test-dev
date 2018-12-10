using Carwale.Entity.Classified.Leads;
using Carwale.Entity.Classified.SellCar;
using System.Collections.Generic;

namespace Carwale.Interfaces.Classified.MyListings
{
    public interface IMyListingsRepository
    {
        List<CustomerSellInquiry> GetListingsByMobile(string mobileNumber);
        string GetCustomerKey(int inquiryId);
        C2BLeadResponse GetC2BLeads(int inquiryId);
        CarTradeLeadResponse GetCarTradeLeads(int inquiryId);
        string GetMobileOfActiveInquiry(int inquiryId);
        bool IsCarCustomerEditable(int inquiryId);
    }
}
