using BikewaleOpr.Entities.BikeData;
using System.Collections.Generic;

namespace BikewaleOpr.Entity.BikePricing
{
    /// <summary>
    /// Created By : Prabhu Puredla on 18 May 2018
    /// Description : entity for mapped bikes
    /// </summary>
    /// <returns></returns>
    public class MappedBikesEntity
    {
        public uint Id { set; get; }
        public string OemBikeName { get; set; }
        public uint MakeId { set; get; }
        public string MakeName { get; set; }
        public string ModelName { get; set; }
        public string VersionName { get; set; }
        public uint  VersionId { get; set; }
        public uint ModelId { set; get; }
        public string LastUpdatedBy { get; set; }
        public string LastUpdatedDate { get; set; }
        public IEnumerable<BikeMakeEntityBase> Makes { get; set; }
    }
}
