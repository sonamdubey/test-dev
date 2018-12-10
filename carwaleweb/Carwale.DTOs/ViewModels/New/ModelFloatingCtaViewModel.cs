using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Carwale.DTOs.ViewModels.New
{
    public class ModelFloatingCtaViewModel
    {
        public string CarName { get; set; }
        public string ModelName { get; set; }
        public string MakeName { get; set; }
        public string VersionName { get; set; }
        public string CityName { get; set; }
        public string CityTrackingLabel { get; set; }
        public int CityId { get; set; }
        public int PageId { get; set; }
        public bool IsShowFloatingCta { get; set; }
        public bool IsAdAvailable { get; set; }
        public bool ShowCampaignLink { get; set; }
        public double PredicationScore { get; set; }
        public string PredicationLabel { get; set; }
        public string ModelImagePath { get; set; }
        public string PriceText { get; set; }
        public string PriceLabel { get; set; }
    }
}