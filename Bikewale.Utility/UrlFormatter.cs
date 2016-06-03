using System;
using System.Text.RegularExpressions;

namespace Bikewale.Utility
{
    public static class UrlFormatter
    {
        public static string BikePageUrl(string makeMaskingName, string modelMaskingName)
        {
            return String.Format("/{0}-bikes/{1}/", makeMaskingName, modelMaskingName);
        }

        public static string VideoDetailPageUrl(string videoTitleUrl, string videoBasicId)
        {
            return String.Format("/bike-videos/{0}-{1}/", videoTitleUrl, videoBasicId);
        }

        public static string VideoByCategoryPageUrl(string videoCategory, string videoCatId)
        {
            return String.Format("/bike-videos/category/{0}-{1}/", Regex.Replace(videoCategory.Trim(), @"[\(\)\s]+", "-").ToLower(), Regex.Replace(videoCatId, @"[,]+", "-"));
        }

        public static string CreateCompareUrl(string makeMasking1, string modelMasking1, string makeMasking2, string modelMasking2, string versionId1, string versionId2)
        {
            return String.Format("comparebikes/{0}-{1}-vs-{2}-{3}/?bike1={4}&bike2={5}", makeMasking1, modelMasking1, makeMasking2, modelMasking2, versionId1, versionId2);
        }

        public static string CreateCompareTitle(string make1, string model1, string make2, string model2)
        {
            return String.Format("{0} {1} vs {2} {3}", make1, model1, make2, model2);
        }

        /// <summary>
        /// Created By Vivek Gupta on 23-05-2016
        /// Desc : url format "/<make>-bikes/<model>/price-in-<city>/" for prices in city
        /// </summary>
        /// <returns></returns>
        public static string PriceInCityUrl(string make, string model, string city)
        {
            return String.Format("/{0}-bikes/{1}/price-in-{2}/", make, model, city);
        }

        /// <summary>
        /// Created By Vivek Gupta on 25-05-2016
        /// Desc : url format /<make>-bikes/<model>/specifications-features/#specs,/<make>-bikes/<model>/specifications-features/#features
        /// </summary>
        /// <returns></returns>
        public static string ViewAllFeatureSpecs(string make, string model, string hash)
        {
            return String.Format("/{0}-bikes/{1}/specifications-features/#{2}", make, model, hash);
        }

        /// <summary>
        /// Created By Vivek Gupta on 31-05-2016
        /// Desc : url format honda-bikes/dealers-in-mumbai/#{dealerId}
        /// </summary>
        /// <returns></returns>
        public static string DealerLocatorUrl(string makeMaskingName, string cityMaskingName)
        {
            return String.Format("/{0}-bikes/dealers-in-{1}/", makeMaskingName, cityMaskingName);
        }

        /// <summary>
        /// Created By Vivek Gupta on 31-05-2016
        /// Desc : url format honda-bikes/dealers-in-mumbai/#{dealerId}
        /// </summary>
        /// <returns></returns>
        public static string DealerLocatorUrl(string makeMaskingName, string cityMaskingName, string hash)
        {
            return String.Format("/{0}-bikes/dealers-in-{1}/#{2}", makeMaskingName, cityMaskingName, hash);
        }
    }
}
