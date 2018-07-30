using Bikewale.Entities.PriceQuote;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 04-Sep-2017
    /// Modified by :   Sumit Kate on 17 Oct 2017
    /// Description :   Added Platform Id
    /// </summary>
    public class PopupLeadCaptureVM : ModelBase
    {
        public LeadCaptureEntity LeadCapture { get; set; }
        public string PopupJson { get; set; }
        public string Url { get; set; }
        public ushort PlatformId { get; set; }
    }
}
