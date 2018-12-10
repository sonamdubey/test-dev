using Carwale.DTOs.NewCars;
using Carwale.Entity.CarData;
using Carwale.Entity.Dealers;
using Carwale.DTOs.Campaigns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.CarData
{
    public class VariantListDTO
    {
        public CarModelDetails ModelDetails { get; set; }
        public CarOverviewDTOV2 MobileOverviewDetails { get; set; }
        public List<string> FuelTypes { get; set; }
        public List<string> TransmissionTypes { get; set; }
        public DealerAdDTO DealerAd { get; set; }
        public string Summary { get; set; }
        public bool ShowCampaignLink { get; set; }
        public List<NewCarVersionsDTOV2> NewCarVersions { get; set; }
        public int PageId { get; set; }
        public bool ShowVariantList { get; set; }
    }
}
