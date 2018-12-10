using Carwale.DTOs.CMS.Photos;
using Carwale.Entity.CarData;
using Carwale.Entity.Classified;
using Carwale.Entity.CMS.Articles;
using Carwale.Entity.CompareCars;
using System;
using System.Collections.Generic;

namespace Carwale.DTOs.NewCars
{
    /// <summary>
    /// Created By : Shalini 
    /// </summary>
    [Serializable]
    public class VersionPageDTO_Desktop_Cache
    {
        public CarVersionDetails VersionDetails { get; set; }
        public List<ModelImageDTO> ModelPhotosListCarousel { get; set; }
        public UsedCarCount UsedCarsCount { get; set; }
        public bool OfferExists { get; set; }
        public List<CarVersions> OtherCarVersions { get; set; }
        public List<ArticleSummary> ModelNews { get; set; }
        public int ModelExpertReviewsCount { get; set; }
        public CarModelDetails ModelDetails { get; set; }
        public List<CCarData> VersionData { get; set; }
        public string ShowAssistancePopup { get; set; }
    }
}
