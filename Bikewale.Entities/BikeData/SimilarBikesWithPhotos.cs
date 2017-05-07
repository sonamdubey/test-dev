
using System;
namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Created By : Sushil Kumar on 5th Jan 2016
    /// Description : To get similar bikes with photos count
    /// </summary>
    [Serializable]
    public class SimilarBikesWithPhotos : BasicBikeEntityBase
    {
        public uint PhotosCount { get; set; }
    }
}
