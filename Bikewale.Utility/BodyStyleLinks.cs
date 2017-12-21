using Bikewale.Entities.GenericBikes;

namespace Bikewale.Utility
{
    /// <summary>
    /// Created By Sajal Gupta on 30-01-2017
    /// Description : Class to hold functions for getting body style links
    /// </summary>
    public static class BodyStyleLinks
    {
        /// <summary>
        /// Creted by : Sajal Gupta on 30-01-2017
        /// Description : function to return footer link according to body style.
        /// </summary>
        /// <param name="bodyStyle"></param>
        /// <returns></returns>
        public static string BodyStyleFooterLink(EnumBikeBodyStyles bodyStyle)
        {
            switch (bodyStyle)
            {
                case EnumBikeBodyStyles.Mileage:
                    return "Mileage Bikes";
                case EnumBikeBodyStyles.Sports:
                    return "Sports Bikes";
                case EnumBikeBodyStyles.Cruiser:
                    return "Cruisers Bikes";
                case EnumBikeBodyStyles.Scooter:
                    return "Scooters";
                case EnumBikeBodyStyles.AllBikes:
                default:
                    return "Bikes";
            }
        }
        /// <summary>
        /// Created by : Aditi Srivastava on 3 Feb 2017
        /// Description : function to return widget heading according to body style.
        /// </summary>
        /// <param name="bodyStyle"></param>
        /// <returns></returns>
        public static string BodyStyleHeadingText(EnumBikeBodyStyles bodyStyle)
        {
            switch (bodyStyle)
            {
                case EnumBikeBodyStyles.Mileage:
                    return "Mileage bikes";
                case EnumBikeBodyStyles.Sports:
                    return "Sports bikes";
                case EnumBikeBodyStyles.Cruiser:
                    return "Cruiser bikes";
                case EnumBikeBodyStyles.Scooter:
                    return "Scooters";
                case EnumBikeBodyStyles.AllBikes:
                default:
                    return "bikes";
            }
        }
		/// <summary>
		/// Created by : Ashutosh Sharma on 20 Dec 2017
		/// Description : Method to get BodyStyle text without "bikes" at the end.
		/// </summary>
		/// <param name="bodyStyle"></param>
		/// <returns></returns>
		public static string BodyStyleText(EnumBikeBodyStyles bodyStyle)
		{
			switch (bodyStyle)
			{
				case EnumBikeBodyStyles.Mileage:
					return "Mileage";
				case EnumBikeBodyStyles.Sports:
					return "Sports Bikes";
				case EnumBikeBodyStyles.Cruiser:
					return "Cruisers";
				case EnumBikeBodyStyles.Scooter:
					return "Scooters";
				case EnumBikeBodyStyles.AllBikes:
				default:
					return "Bikes";
			}
		}
	}
}
