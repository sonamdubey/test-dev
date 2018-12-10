using Carwale.DTOs.PriceQuote;
using Carwale.Entity.CarData;
using Carwale.Entity.Deals;
using Carwale.Entity.Enum;
using System;
using System.Collections.Generic;

namespace Carwale.DTOs.CarData
{
    /*
      *This DTO is refactored on 15 may 2017.We removed all unnecessary fields which are not required for make page.
     * If anybody wants some more property at any other page then see the previous version and add here and if u think anyone of the field is not required then remove 
     */
    public class CarModelSummaryDTOV2
    {
        public string ModelName { get; set; }
        public string MaskingName { get; set; }
        public double ModelRating { get; set; }
        public int ReviewCount { get; set; }
        public string HostUrl { get; set; }
        public string OriginalImage { get; set; }
        public int ModelId { get; set; }
        public bool New { get; set; }
        public int MakeId { get; set; }
        public double MinPrice { get; set; }
        public double MaxPrice { get; set; }
        public DealsStock DiscountSummary { get; set; }
        public PriceOverviewDTOV2 CarPriceOverview { get; set; }
        public CarBodyStyle BodyStyleId { get; set; }
        public int Rank { get; set; }
        public int NoOfTopCarsInBodyType { get; set; }
        public DateTime LaunchDate { get; set; }
        public bool ShowDate { get; set; }
        public int CWConfidence { get; set; }
        public List<ModelColors> Colours { get; set; }
        public bool IsModelColorPhotosAvailable { get; set; }
        public int VersionCount { get; set; }
    }
}
