
using Bikewale.Entities.BikeData;
using System;
using System.Runtime.Serialization;
namespace Bikewale.Entities.Compare
{
    /// <summary>
    /// Created By :   Sushil Kumar on 2nd Feb 2017
    /// Description :   To Store bike comparisions versions
    /// </summary>
    [Serializable, DataContract]
    public class BikeVersionCompareEntity : BikeVersionEntityBase
    {
        public uint ModelId { get; set; }
    }
}
