using Bikewale.Entities.PriceQuote;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 04-Sep-2017
    /// </summary>
    public class PopupLeadCaptureVM : ModelBase
    {
        public LeadCaptureEntity LeadCapture { get; set; }
        public string PopupJson { get; set; }
    }
}
