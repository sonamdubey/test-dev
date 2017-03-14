
namespace BikewaleOpr.Entity.BikeData
{
    /// <summary>
    /// Created by : Sajal Gupta on 06-03-2017
    /// Description: Used bike model image data  
    /// </summary>
    public class UsedBikeModelImageData
    {
        public uint ModelId { get; set; }
        public uint MakeId { get; set; }
        public string MakeName { get; set; }
        public string ModelName { get; set; }
        public string HostUrl { get; set; }
        public string OriginalImagePath { get; set; }
    }
}

