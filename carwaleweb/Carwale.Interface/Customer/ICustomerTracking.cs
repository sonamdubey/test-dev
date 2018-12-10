using Carwale.DTOs.PriceQuote;
using Carwale.Entity.Campaigns;
using Carwale.Entity.CarData;
using Carwale.Entity.CMS.Articles;
using Carwale.Entity.Dealers;
using Carwale.Entity.PriceQuote;
using System.Collections.Generic;

namespace Carwale.Interfaces.Customer
{
    public interface ICustomerTracking
    {
        void AppsTrackModelVersionImpressionV1(CarDataTrackingEntity carDataTrackingEntity, Campaign pqDealerAdd);
        void AppsTrackModelVersionImpression(CarDataTrackingEntity carDataTrackingEntity, SponsoredDealer pqDealerAdd);
        void TrackPqImpression(CarDataTrackingEntity carDataTrackingEntity, SponsoredDealer pqDealerAdd, List<PQItem> priceQuoteList);
        void TrackPqImpression(CarDataTrackingEntity carDataTrackingEntity, SponsoredDealerDTO pqDealerAdd, List<PQItemDTO> priceQuoteList);
        void TrackPriceQuoteImpression(CarDataTrackingEntity carData, DealerAd dealerAd);
        void TrackNewsTags(List<VehicleTag> vehicleTagsList);
    }
}
