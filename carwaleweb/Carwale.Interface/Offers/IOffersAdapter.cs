using Carwale.DTOs.OffersV1;
using Carwale.Entity.OffersV1;

namespace Carwale.Interfaces.Offers
{
    public interface IOffersAdapter
    {
        OfferDto GetOffers(OfferInput offerInput);
        string GetValidityText(bool isOfferExtended, OfferDetailsDto offerDetails);
    }
}
