using System;

namespace BikewaleOpr.Entity.Dealers
{
    /// <summary>
    /// Created by  :   Vishnu Teja Yalakuntla on 28-Jul-2017
    /// Description :   Entity for holding version and price details.
    /// </summary>
    public class DealerVersionEntity
    {
        public string MakeName { get; set; }
        public string ModelName { get; set; }
        public string VersionName { get; set; }
        public string BikeName { get { return String.Format("{0} {1} {2}", MakeName, ModelName, VersionName); } }
        public uint VersionId { get; set; }
        public uint NumberOfDays { get; set; }
        public uint BikeModelId { get; set; }
    }
}
