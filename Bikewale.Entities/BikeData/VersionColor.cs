
using System;
namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Modified By : Sushil Kumar on 28th August 2016
    /// Description : Removed unused property CompanyCode
    /// </summary>
    [Serializable]
    public class VersionColor
    {
        public uint ColorId { get; set; }
        public string ColorName { get; set; }
        public string ColorCode { get; set; }
    }
}
