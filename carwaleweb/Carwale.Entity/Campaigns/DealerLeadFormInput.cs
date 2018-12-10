using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Campaigns
{
    public class DealerLeadFormInput
    {
        public short PlatformId { get; set; }

        public int CampaignId { get; set; }

        public int CityId { get; set; }

        public string CityName { get; set; }

        public int ZoneId { get; set; }

        public int AreaId { get; set; }

        public int ModelId { get; set; }

        public int VersionId { get; set; }

        public int ScreenId { get; set; }

        public int LeadClickSource { get; set; }

        public int InquiryClickSource { get; set; }

        public string PredictionLabel { get; set; }

        public double PredictionScore { get; set; }

        public string AppVersionId { get; set; }

        public int PropertyId { get; set; }

    }
}
