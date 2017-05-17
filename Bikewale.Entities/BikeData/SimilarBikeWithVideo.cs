
using System;
namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// created By :- Subodh Jain 3 feb 2017
    /// Summary:- Entity for similar bike videos
    /// </summary>
    [Serializable]
    public class SimilarBikeWithVideo //: BasicBikeEntityBase
    {
        public uint VideoCount { get; set; }
        public BikeMakeEntityBase Make { get; set; }
        public BikeModelEntityBase Model { get; set; }
        public string OriginalImagePath { get; set; }
        public string HostUrl { get; set; }
        public bool IsDiscontinued { get; set; }
        public bool IsUpcoming { get; set; }
    }
}
