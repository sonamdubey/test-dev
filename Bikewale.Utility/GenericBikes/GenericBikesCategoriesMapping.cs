using Bikewale.Entities.GenericBikes;
using System;

namespace Bikewale.Utility.GenericBikes
{
    public class GenericBikesCategoriesMapping
    {
        public const string _BestBikes = "bikes";
        public const string _BestScooters = "scooters";
        public const string _BestSports = "sports-bikes";
        public const string _BestMileageBikes = "mileage-bikes";
        public const string _BestCruisersBikes = "cruiser-bikes";


        /// <summary>
        /// Created By : Sushil Kumar on 15th Dec 2016
        /// Deascription : Android App notification categories
        /// </summary>
        /// <param name="subId"></param>
        /// <returns></returns>
        public static EnumBikeBodyStyles GetBodyStyleByBikeType(string bikeType)
        {
            EnumBikeBodyStyles bodyStyle = EnumBikeBodyStyles.AllBikes;
            Enum.TryParse(bikeType, out bodyStyle);
            return bodyStyle;
        }

        public static string BodyStyleByType(EnumBikeBodyStyles bodyStyle)
        {
            switch (bodyStyle)
            {
                case EnumBikeBodyStyles.AllBikes:
                    return _BestBikes;
                case EnumBikeBodyStyles.Cruiser:
                    return _BestCruisersBikes;
                case EnumBikeBodyStyles.Sports:
                    return _BestSports;
                case EnumBikeBodyStyles.Scooter:
                    return _BestScooters;
                case EnumBikeBodyStyles.Mileage:
                    return _BestMileageBikes;
                default:
                    return _BestBikes;
            }
        }
    }
}
