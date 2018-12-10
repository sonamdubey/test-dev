using Carwale.DTOs.Campaigns;
namespace Carwale.DTOs.PriceQuote
{
    public class EmiCalculatorModelLink 
    {
        public EmiCalculatorModelData EmiCalculatorModelData { get; set; }
        public EmiCalculatorDealerAdDto DealerData { get; set; }
        public bool ShowEmiCalculator { get; set; }
        public bool ShowLinkOnBottomLeft { get; set; }
        public bool ShowToolTip { get; set; }
        public bool ShowRedSolidButton { get; set; }
        public bool ShowChangeTextLink { get; set; }
        public bool ShowGetEmiOffersButton { get; set; }
    }
}
