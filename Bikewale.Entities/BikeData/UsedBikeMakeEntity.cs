
using System;
using System.Runtime.Serialization;
namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Used this entity to hold the data for make entities 
    /// </summary>
    [Serializable, DataContract]
    public class UsedBikeMakeEntity : BikeMakeEntityBase
    {
        // This property has been commented as new design 
        // doesn't expect to show count of bikes for makes in india

        // public uint Count { get; set; }
        public string Link { get; set; }
    }
}
