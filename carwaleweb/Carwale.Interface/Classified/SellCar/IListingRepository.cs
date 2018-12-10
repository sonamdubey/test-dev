
using Carwale.Entity.Classified.SellCarUsed;
using System.Collections.Generic;

namespace Carwale.Interfaces.Classified.SellCar
{
    public interface IListingRepository
    {
        bool ListingDelete(int inquiryId, int status);
        bool PatchListings(int inquiryId, SellCarInfo sellCarInfo);
        bool PatchListingsV1(int inquiryId, SellCarInfo sellCarInfo);
        IList<SellCarBasicInfo> GetExpiredListings();
    }
}
