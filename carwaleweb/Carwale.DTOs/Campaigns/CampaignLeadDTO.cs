using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Campaigns
{
    public class CampaignLeadDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public int VersionId { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public int ZoneId { get; set; }
        public int DealerId { get; set; }
        public int CampaignId { get; set; }
        public string UserModel { get; set; }
    }
}
