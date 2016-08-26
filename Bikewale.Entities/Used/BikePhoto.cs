using System;

namespace Bikewale.Entities.Used
{
    /// <summary>
    /// Created By : Sushil Kumar on 27th August 2016
    /// Description : Bike Photos entity for used bikes
    /// </summary>
    [Serializable]
    public class BikePhoto
    {
        public string HostUrl { get; set; }
        public string OriginalImagePath { get; set; }
        public bool IsMain { get; set; }
    }
}
