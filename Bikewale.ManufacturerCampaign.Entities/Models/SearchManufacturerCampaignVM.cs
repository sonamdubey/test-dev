using BikewaleOpr.Entities;
using System.Collections.Generic;

namespace Bikewale.ManufacturerCampaign.Entities.Models
{
    public class SearchManufacturerCampaignVM
    {
        public string UserId { get; set; }
        public IEnumerable<ManufacturerEntity> ManufacturerList { get; set; }
    }
}
