
using System;
namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Created By : Sushil Kumar on 5th Jan 2016
    /// Description : To get similar bikes with photos count
    /// Modified by :Snehal Dange on 07-09-2017
    /// Description : Added ExShowroomPriceMumbai ,OnRoadPriceInCity , BodyStyle
    /// </summary>
    [Serializable]
    public class SimilarBikesWithPhotos //: BasicBikeEntityBase
    {
        public uint PhotosCount { get; set; }
        public BikeMakeEntityBase Make { get; set; }
        public BikeModelEntityBase Model { get; set; }
        public string OriginalImagePath { get; set; }
        public string HostUrl { get; set; }
        public bool IsDiscontinued { get; set; }
        public bool IsUpcoming { get; set; }
        public uint ExShowroomPriceMumbai { get; set; }
        public uint OnRoadPriceInCity { get; set; }

        public sbyte BodyStyle { get; set; }
    }
}
