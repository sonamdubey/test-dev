using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewaleopr.ManufacturerCampaign.Entities
{
   public class ManufacturerCampaignPopup
    {
        public uint CampaignId { get; set; }
        public string PopupHeading { get; set; }
        public string PopupDescription { get; set; }

        public string PopupSuccessMessage { get; set; }

        public ushort EmailRequired { get; set; }
        public ushort PinCodeRequired { get; set; }
        public ushort DealerRequired { get; set; }
        
    }
}
