
using System;
namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Created By : Sushil Kumar on 5th Jan 2016
    /// Description : To get similar bikes with photos count
    /// </summary>
    [Serializable]
    public class SimilarBikesWithPhotos
    {
        public BikeMakeEntityBase Make { get; set; }
        public BikeModelEntityBase Model { get; set; }
        public string HostUrl { get; set; }
        public string OriginalImagePath { get; set; }
        public uint PhotosCount { get; set; }
    }
}
