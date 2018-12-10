using System.Collections.Generic;
using Carwale.Entity.CMS.Photos;
using Carwale.Entity.CMS;
using Carwale.DTOs.CMS.Media;
using Carwale.Entity.CarData;
using Newtonsoft.Json;
using Carwale.DTOs.CMS.ThreeSixtyView;
using Carwale.Entity;
using Carwale.DTOs.CMS.Photos;

namespace Carwale.DTOs.CarData
{
    public class PhotoGalleryDTO
    {
        public List<ModelImage>  modelImages{ get; set; }
        public List<Video> modelVideos { get; set; }
        [JsonProperty("threeSixtyAvailability")]
        public ThreeSixtyAvailabilityDTO ThreeSixtyAvailability { get; set; }
        [JsonProperty("threeSixtyImage")]
        public ThreeSixtyImageDto ThreeSixtyImage { get; set; }
        [JsonProperty("modelColors")]
        public List<Carwale.DTOs.CMS.Photos.ModelColorsDTO> ModelColors { get; set; }
        [JsonProperty("isColorsLinkPresent")]
        public bool IsColorsLinkPresent { get; set; }
    }

    public class ThreeSixtyImageDto
    {
        [JsonProperty("exteriorImagePath")]
        public string ExteriorImagePath { get; set; }
        [JsonProperty("interiorImagePath")]
        public string InteriorImagePath { get; set; }
        [JsonProperty("imageHost")]
        public string ImageHost { get; set; }
    }

    public class PhotoGalleryDTO_V2
    {
        [JsonProperty("modelImages")]
        public List<ModelImage> ModelImages { get; set; }

        [JsonProperty("modelVideos")]
        public List<Video> ModelVideos { get; set; }

        [JsonProperty("galleryState")]
        public GalleryStateDTO GalleryState { get; set; }

        [JsonProperty("modelDetails")]
        public CarModelDetails ModelDetails { get; set; }

        [JsonProperty("recommendedVideos")]
        public List<Video> RecommendedVideos { get; set; }

        public DiscountSummaryDTO DiscountSummary { get; set; }

        public bool IsExteriorPresent { get; set; }
        public bool IsInteriorPresent { get; set; }
        public List<ModelColors> ModelColors { get; set; }
        public bool ShowModelColors { get; set; }
        public List<BreadcrumbEntity> BreadcrumbEntitylist { get; set; }
        public SubNavigationDTO SubNavigation { get; set; }
    }
}
