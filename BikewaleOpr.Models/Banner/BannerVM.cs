using BikewaleOpr.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.Models
{
public class BannerVM
    {
        public BannerDetails DesktopBannerDetails { get; set; }
        public BannerDetails MobileBannerDetails { get; set; }
        [JsonProperty("startdate")]
        public DateTime? StartDate { get; set; }
        [JsonProperty("enddate")]
        public DateTime? EndDate { get; set; }
        [JsonProperty("bannerdescription")]
        public string BannerDescription { get; set; }
        [JsonProperty("campaignid")]
        public uint CampaignId { get; set; }
    }
}
