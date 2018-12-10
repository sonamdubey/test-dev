using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Campaigns
{
    [Serializable]
    public class PageTemplates
    {
        public int PropertyId { get; set; }
        public int TemplateId { get; set; }
        public int CampaignId { get; set; }
    }
}
