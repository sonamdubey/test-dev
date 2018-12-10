using Carwale.Entity.Classified.Leads;

namespace Carwale.Interfaces.Classified.Leads
{
    public interface ISellerRepository
    {
        Seller GetDealerSeller(int inquiryId);
        Seller GetIndividualSeller(int inquiryId);
    }
}