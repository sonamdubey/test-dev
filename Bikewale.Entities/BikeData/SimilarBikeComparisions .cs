using System;

namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Created By :Snehal Dange on 24th Oct 2017
    /// Description :Entity used for similar bike comparision on compare bike page 
    /// </summary>
    [Serializable]
    public class SimilarBikeComparisions
    {
        public SimilarCompareBikeEntity Bikes { get; set; }
        public BikeMakeBase Make { get; set; }
        public BikeModelEntityBase Model { get; set; }
        public string HostUrl { get; set; }
        public string OriginalImagePath { get; set; }

    }
}
