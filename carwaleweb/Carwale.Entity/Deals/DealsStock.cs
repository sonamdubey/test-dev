using Carwale.Entity.CarData;
using Carwale.Entity.Geolocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Carwale.Entity.Deals
{
    [Serializable]
    public class DealsStock
    {
        public MakeEntity Make { get; set; }
        public ModelEntity Model { get; set; }
        public VersionBase Version { get; set; }
        public int OnRoadPrice { get; set; }
        public int Savings { get; set; }
        public int OfferPrice { get; set; }
        public CarImageBase CarImageDetails { get; set; }
        public string Offers { get; set; }
        public string TermsConditions { get; set; }
        public int StockCount { get; set; }
        public ColorEntity Color { get; set; }
        public int ManufacturingYear { get; set; }
        public City City { get; set; }
        public int DealerId { get; set; }
        public int StockId { get; set; }
        public int SelectedYear { get; set; }
        public string MaskingNumber { get; set; }
        public int FuelType { get; set; }
        public int TransmissionType { get; set; }
        public bool ?IsSimilarCar { get; set; }
        public bool PriceUpdated { get; set; }
        public int OfferValue { get; set; }
        public int PriceBreakUpId { get; set; }
        public bool ShowExtraSavings { get; set; }
        public int ExtraSavings { get; set; }
        public int DeliveryTimeline { get; set; }
        public bool IsNew { get; set; }
        public int VersionCount { get; set; }
        public string SpecificationsOverview { get; set; }
        public DealsDealers DealerDetails { get; set; }
        public AreaCode Area { get; set; }
        
    }
}
