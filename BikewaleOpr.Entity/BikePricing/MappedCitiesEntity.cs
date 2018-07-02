namespace BikewaleOpr.Entity.BikePricing
{
    /// <summary>
    /// Created By : Prabhu Puredla on 18 May 2018
    /// Description : entity for mapped cities
    /// </summary>
    /// <returns></returns>
    public class MappedCitiesEntity
    {
        public uint Id { set; get; }
        public string OemCityName { get; set; }
        public uint StateId { set; get; }
        public string StateName { get; set; }
        public string CityName { get; set; }
        public uint CityId { get; set; }
        public string LastUpdatedDate { get; set; }
        public string LastUpdatedBy { get; set; }
    }
}
