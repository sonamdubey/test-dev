using Carwale.Entity.CarData;
using Carwale.Entity.Dealers;
using System.Collections.Generic;
using Carwale.DTOs.CarData;
using Carwale.Entity.Geolocation;
using Carwale.Entity.Offers;
using Carwale.Entity.PriceQuote;
using Carwale.DTOs.Geolocation;
using Carwale.DTOs.OffersV1;

namespace Carwale.DTOs.PriceQuote
{
    /// <summary>
    /// Dto For MObileAndroid PriceQuote
    /// Written By : Ashish Verma on 2/6/2014
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public class PQAndroid
    {
        public ulong InquiryId { get; set; }
        public List<PQItem> priceQuoteList { get; set; }
        public List<CarVersionEntity> otherVersions { get; set; }
        public List<PriceQuoteCityDTO> otherCities { get; set; }
        public SponsoredDealer sponsoredDealer { get; set; }
        public string largePicUrl { get; set; }
        public string smallPicUrl { get; set; }
        public int cityId { get; set; }
        public string ZoneId { get; set; }
        public int AreaId { get; set; }
        public string cityName { get; set; }
        public string zoneName { get; set; }
        public string AreaName { get; set; }
        public string formatedonRoadPrice { get; set; }
        public long onRoadPrice { get; set; }
        public List<SimilarCarsAndroidDTO> alternativeCars { get; set; }
        public string otherCityUrl { get; set; }
        public string versionDetailUrl { get; set; }
        public string modelDetailUrl { get; set; }
        public string specsSummery { get; set; }
        public string carName { get; set; }
        public decimal reviewRate { get; set; }
        public string HostUrl { get; set; }
        public string OriginalImgPath { get; set; }

        public int makeId { get; set; }
        public int modelId { get; set; }
        public int versionId { get; set; }

        public string MakeName { get; set; }
        public string modelName { get; set; }
        public string versionName { get; set; }
        public string maskingName { get; set; }
        public PQOfferEntity offers { get; set; }
        public List<NewCarDealersList> dealerList { get; set; }
        public List<PuneThaneZones> SpecialZones { get; set; }
        public List<CityZonesDTO> Zones { get; set; }
        public bool IsSponsoredCar { get; set; }
        public OfferDto OfferV1 { get; set; }
    }
}
