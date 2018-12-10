using Carwale.Entity.Geolocation;

namespace Carwale.Entity.CarData
{
    public class CarDataTrackingEntity
    {
        public int ModelId { get; set; }
        public int Platform { get; set; }
        public long OnRoadPrice { get; set; }
        public int CampaignType { get; set; }
        public int PageId { get; set; }
        public int VersionId { get; set; }
        public string Category { get; set; }
        public string Action { get; set; }
        public CustLocation Location { get; set; }

        public CarDataTrackingEntity()
        {
            this.Location = new CustLocation();
        }
    }
}
