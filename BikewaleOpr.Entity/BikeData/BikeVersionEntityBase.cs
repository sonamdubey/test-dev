
using System;

namespace BikewaleOpr.Entities.BikeData
{
    /// <summary>
    /// Created By : Ashish G. Kamble
    /// </summary>
	[Serializable]
    public class BikeVersionEntityBase
    {
        public int VersionId { get; set; }
        public string VersionName { get; set; }

    }
}
