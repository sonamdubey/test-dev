
using System;
namespace Bikewale.Entities.BikeData
{
    [Serializable]
    public class BasicBikeEntityBase
    {
        public int Id { get; set; }
        public BikeMakeEntityBase Make { get; set; }
        public BikeModelEntityBase Model { get; set; }
        public string OriginalImagePath { get; set; }
        public string HostUrl { get; set; }
        public bool IsDiscontinued { get; set; }
        public bool IsUpcoming { get; set; }
    }
}
