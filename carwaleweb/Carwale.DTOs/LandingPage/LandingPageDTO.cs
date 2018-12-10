using Carwale.DTOs.CarData;
using Carwale.DTOs.Geolocation;
using Carwale.Entity.CarData;
using Carwale.Entity.Geolocation;
using Carwale.Entity.LandingPage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.LandingPage
{
    public class LandingPageDTO
    {
        public LandingPageDetails CampaignDetails { get; set; }
        public List<CarModelSummary> Models { get; set; }
        public List<Cities> Cities { get; set; }
        public CarModelDetails DefaultModelsDetails { get; set; }
        public PhotoGalleryDTO PhotoGalleryDTO { get; set; }       
        public bool IncludeVersaTag { get; set; }
    }
}
