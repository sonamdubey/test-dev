using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Campaigns
{
    [Serializable]
    public class ClientCampaignMapping
    {
        public int ClientId { get; set; }        
        public int CampaignId { get; set; }
    }
}
