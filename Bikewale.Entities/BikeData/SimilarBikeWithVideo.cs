
using System;
namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// created By :- Subodh Jain 3 feb 2017
    /// Summary:- Entity for similar bike videos
    /// </summary>
    [Serializable]
    public class SimilarBikeWithVideo : BasicBikeEntityBase
    {
        public uint VideoCount { get; set; }
    }
}
