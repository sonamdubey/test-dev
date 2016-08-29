using System;
using System.Collections.Generic;

namespace Bikewale.Entities.Used
{
    /// <summary>
    /// Created By : Sushil Kumar on 27th August 2016
    /// Description : Bike Minimum details entity for used bikes
    /// </summary>
    [Serializable]
    public class BikeDetailsMin
    {
        public IList<BikePhoto> Photo { get; set; }
        public uint AskingPrice { get; set; }
        public string ModelYear { get; set; }
        public uint KmsDriven { get; set; }
        public string OwnerType { get; set; }
        public string RegisteredAt { get; set; }
        public ushort PhotosCount { get { if (Photo != null) { return Convert.ToUInt16(Photo.Count); } return 0; } }

    }
}








