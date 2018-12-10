using Carwale.Entity.Classified.Leads;

namespace Carwale.Interfaces.Classified.MyListings
{
    public interface IMyListingsCacheRepository
    {
        C2BLeadResponse GetC2BLeads(int inquiryId);
        CarTradeLeadResponse GetCarTradeLeads(int inquiryId);
    }
}
