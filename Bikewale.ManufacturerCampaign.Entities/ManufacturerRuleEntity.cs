namespace Bikewale.ManufacturerCampaign.Entities
{
    /// <summary>
    /// Created by : Aditi Srivastava on 23 Jun 2017
    /// Summary    : Entity for individual rules 
    /// </summary>
    public class ManufacturerRuleEntity
    {
        public uint ModelId { get; set; }
        public uint MakeId { get; set; }
        public uint StateId { get; set; }
        public uint CityId { get; set; }
        public string ModelName { get; set; }
        public string MakeName { get; set; }
        public string StateName { get; set; }
        public string CityName { get; set; }
    }
}
