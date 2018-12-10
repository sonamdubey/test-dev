using Carwale.DTOs.Campaigns;
using Carwale.DTOs.CarData;
using Carwale.DTOs.Geolocation;
using Carwale.DTOs.OffersV1;
using Carwale.DTOs.PriceQuote;
using Carwale.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.OfferAndDealerAd
{
    public class OfferAndDealerAdDTO
    {
        public OfferDto Offer { get; set; }
        public DealerAdDTO DealerAd { get; set; }
        public Pages Page { get; set; }

        //TODO: Remove this property.
        //This is temporary. Eventually DealerAdDTO.PageProperty is to be used.
        public List<LeadSourceDTO> LeadSource { get; set; }
        public CityAreaDTO Location { get; set; }
        public CarDetailsDTO CarDetails { get; set; }
        public bool ShowCampaign { get; set; }
        public bool CampaignDetailsVisible { get; set; }
        public string Platform { get; set; }

        public OfferAndDealerAdDTO()
        {
            LeadSource = new List<LeadSourceDTO>();
            ShowCampaign = true;
            CampaignDetailsVisible = true;
        }
    }
}
