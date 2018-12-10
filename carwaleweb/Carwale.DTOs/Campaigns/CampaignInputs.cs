using Carwale.Entity.Dealers.URI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Campaigns
{
    public class CampaignInputs
    {
        public short PlatformId { get; set; }
        public int CampaignId { get; set; }
        public int CityId { get; set; }
        public int ModelId { get; set; }
        public int ZoneId { get; set; }
    }
}