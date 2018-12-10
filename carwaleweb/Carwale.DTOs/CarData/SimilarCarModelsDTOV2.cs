using Carwale.DTOs.OffersV1;
using Carwale.DTOs.PriceQuote;
using Carwale.Entity.CarData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.CarData
{
    public class SimilarCarsDTO
    {
        public List<SimilarCarModelsDTOV2> SimilarCarModels { get; set; }
        public string SourceModelName { get; set; }
        public int SourceModelId { get; set; }
        public int WidgetPageSource { get; set; }
        public string PageName { get; set; }
        public int PQPageId { get; set; }
        public UpcomingCarModel SimilarUpcomingCar { get; set; }
    }
    public class SimilarCarModelsDTOV2
    {
        public int ReviewCount { get; set; }
        public int PopularVersionId { get; set; }
        public string HostUrl { get; set; }
        public string ModelImageOriginal { get; set; }
        public string MakeName { get; set; }
        public string ModelName { get; set; }
        public int ModelId { get; set; }
        public string ReviewRateNew;
        public string CarModelUrl { get; set; }
        public string MaskingName { get; set; }
        public decimal ReviewRate { get; set; }
        public bool IsFeatured { get; set; }
        public PriceOverviewDTOV2 PricesOverview { get; set; }
        public string CompareCarUrl { get; set; }
        public string SpotlightUrl { get; set; }
        public DateTime LaunchDate { get; set; }
        public OfferLinkDto OfferDetails { get; set; }
    }
}
