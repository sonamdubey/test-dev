namespace BikewaleOpr.Entity.BikePricing
{
    /// <summary>
    /// Created By : Prabhu Puredla on 22 May 2018
    /// Description : Entity to hold the Oem price entity
    /// </summary>
    public class OemPriceEntity
    {
        public string BikeName { set; get; }
        public string CityName { set; get; }
        public string Price { set; get; }
        public uint StateId { set; get; }
        public uint BikeId { set; get; }
        public uint CityId { set; get; }
        public double Insurance { set; get; }
        public double Rto { set; get; }
        public uint ModelId { set; get; }
    }
}
