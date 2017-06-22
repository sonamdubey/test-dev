using Bikewale.ManufacturerCampaign.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.Models.ManufacturerCampaign
{
    public class ManufacturerCampaignInformationModel
    {
        public ConfigureCampaignEntity CampaignInformation {get; set;}
        public uint CampaignId { get; set; }
    }
}
