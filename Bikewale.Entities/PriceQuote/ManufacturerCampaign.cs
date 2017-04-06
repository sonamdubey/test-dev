
namespace Bikewale.Entities.PriceQuote
{
    /// <summary>
    /// 
    /// </summary>
    public class ManufacturerCampaign
    {
        public string Name { get; set; }
        public uint Id { get; set; }
        public string MaskingNumber { get; set; }
        public string Ad { get; set; }
        public bool IsAdAvailable { get { return !string.IsNullOrEmpty(Ad); } }
        public bool ShowAd { get; set; }
    }
}
