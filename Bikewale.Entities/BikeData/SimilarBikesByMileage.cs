using System;


namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Created By : Snehal Dange on 3rd Nov 2017
    /// Description: entity created to display bikes with similar/better mileage
    /// </summary>
    [Serializable]
    public class SimilarBikesByMileage
    {
        public BasicBikeEntityBase SimilarMileageBikes { get; set; }
        public float AvgMileageByUsers { get; set; }
       
    }
}
