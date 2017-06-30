using BikewaleOpr.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.ManufacturerCampaign.Entities.Models
{
    public class SearchManufacturerCampaignVM
    {
        public IEnumerable<ManufacturerEntity> ManufacturerList { get; set; }
    }
}
