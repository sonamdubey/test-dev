using Bikewale.Models;
using System;
using System.Collections.Specialized;

namespace Bikewale.Utility
{
    /// <summary>
    /// Created By : Deepak Israni on 21 March 2018
    /// Description: Class to provide helper functions related to Google Ads.
    /// </summary>
    public static class GoogleAdsHelper
    {
        /// <summary>
        /// Created By : Deepak Israni on 21 March 2018
        /// Description: Function to generate string that defines the adslot for an ad with the position property.
        /// </summary>
        /// <param name="adId"></param>
        /// <param name="adPath"></param>
        /// <param name="divId"></param>
        /// <param name="size"></param>
        /// <param name="viewSize"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static string GenerateSlotDefinition(string adId, string adPath, uint divId, string size, string viewSize, string position)
        {
            return String.Format("googletag.defineSlot('{0}_{1}_{2}', {3}, 'div-gpt-ad-{4}-{5}').addService(googletag.pubads());", adPath, position, size, viewSize, adId, divId);
        }

        /// <summary>
        /// Created By : Deepak Israni on 21 March 2018
        /// Description: Function to generate string that defines the adslot for an ad when no position property declared.
        /// </summary>
        /// <param name="adId"></param>
        /// <param name="adPath"></param>
        /// <param name="divId"></param>
        /// <param name="size"></param>
        /// <param name="viewSize"></param>
        /// <returns></returns>
        public static string GenerateSlotDefinition(string adId, string adPath, uint divId, string size, string viewSize)
        {
            return String.Format("googletag.defineSlot('{0}_{1}', {2}, 'div-gpt-ad-{3}-{4}').addService(googletag.pubads());", adPath, size, viewSize, adId, divId);
        }

        /// <summary>
        /// Created by : Snehal Dange on 22nd March 2018
        /// Description : Method to set all the ad slot properties
        /// </summary>
        /// <param name="adInfo"></param>
        /// <param name="viewSizes"></param>
        /// <param name="divId"></param>
        /// <param name="width"></param>
        /// <param name="size"></param>
        /// <param name="position"></param>
        /// <param name="loadImmediate"></param>
        /// <returns></returns>
        public static AdSlotModel SetAdSlotProperties(NameValueCollection adInfo, string[] viewSizes, uint divId, uint width, string size, string position, bool loadImmediate = false)
        {
            AdSlotModel adSlotObj = new AdSlotModel(viewSizes)
                {
                    AdId = adInfo["adId"],
                    AdPath = adInfo["adPath"],
                    DivId = divId,
                    Width = width,
                    LoadImmediate = loadImmediate,
                    Position = position,
                    Size = size
                };
            return adSlotObj;
        }

        /// <summary>
        /// Created by : Snehal Dange on 22nd March 2018
        /// Description: Overloaded method to set all the ad slot properties when position property is not applicable.
        /// </summary>
        /// <param name="adInfo"></param>
        /// <param name="viewSizes"></param>
        /// <param name="divId"></param>
        /// <param name="width"></param>
        /// <param name="size"></param>
        /// <param name="loadImmediate"></param>
        /// <returns></returns>
        public static AdSlotModel SetAdSlotProperties(NameValueCollection adInfo, string[] viewSizes, uint divId, uint width, string size, bool loadImmediate = false)
        {
            AdSlotModel adSlotObj = new AdSlotModel(viewSizes)
            {
                AdId = adInfo["adId"],
                AdPath = adInfo["adPath"],
                DivId = divId,
                Width = width,
                LoadImmediate = loadImmediate,
                Size = size
            };
            return adSlotObj;
        }
    }
}
