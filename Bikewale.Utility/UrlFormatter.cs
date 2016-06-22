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
        public static string ViewAllFeatureSpecs(string make, string model, string hash, uint versionId)
        {
            return String.Format("/{0}-bikes/{1}/specifications-features/?vid={3}#{2}", make, model, hash, versionId);
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

        /// <summary>
        /// Created By Vivek gupta
        /// Date : 22 june 2016        
        //  Desc : used/bajaj-bikes-in-mumbai/#city=10&make=1&dist=0
        /// </summary>
        /// <param name="make"></param>
        /// <param name="city"></param>
        /// <returns></returns>

        public static string UsedBikesUrlNoCity(string make, string city, uint cityId, uint makeId)
        {
            return String.Format("/used/{0}-bikes-in-{1}/#city={2}&make={3}&dist=0", make, city, cityId, makeId);
        }


        /// <summary>
        /// Created By Vivek gupta
        /// Date : 22 june 2016
        //  Desc : used/bikes-in-mumbai/bajaj-pulsar-rs200-S42582/
        /// </summary>
        /// <param name="city"></param>
        /// <param name="make"></param>
        /// <param name="model"></param>
        /// <param name="profileId"></param>
        /// <returns></returns>
        public static string UsedBikesUrl(string city, string make, string model, string profileId)
        {
            return String.Format("/used/bikes-in-{0}/{1}-{2}-{3}/", city, make, model.Replace(" ", "-"), profileId);
        }

        /// <summary>
        /// Created By Vivek gupta
        /// Date : 22 june 2016
        /// Desc : view more used bikes url returned, /used/harleydavidson-bikes-in-pune/#city=12&make=5&dist=0
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="make"></param>
        /// <param name="city"></param>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public static string ViewMoreUsedBikes(uint cityId, string make, string city, uint makeId)
        {
            if (cityId > 0)
            {
                return String.Format("/used/{0}-bikes-in-{1}/#city={2}&make={3}&dist=0", make, city, cityId, makeId);
            }
            else
            {
                return String.Format("/used/{0}-bikes-in-india/", make);
            }
        }
    }
}
