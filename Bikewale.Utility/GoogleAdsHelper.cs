using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
