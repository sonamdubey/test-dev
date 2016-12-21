using Bikewale.Entities.CMS;
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

        /// <summary>
        /// Modified by :   Sumit Kate on 19 Dec 2016
        /// Description :   If source is passed consider it for url qs for compare tracking
        /// </summary>
        /// <param name="makeMasking1"></param>
        /// <param name="modelMasking1"></param>
        /// <param name="makeMasking2"></param>
        /// <param name="modelMasking2"></param>
        /// <param name="versionId1"></param>
        /// <param name="versionId2"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string CreateCompareUrl(string makeMasking1, string modelMasking1, string makeMasking2, string modelMasking2, string versionId1, string versionId2, Bikewale.Entities.Compare.CompareSources? source = null)
        {
            return String.Format("comparebikes/{0}-{1}-vs-{2}-{3}/?bike1={4}&bike2={5}{6}", makeMasking1, modelMasking1, makeMasking2, modelMasking2, versionId1, versionId2, (source.HasValue ? "&source=" + (int)source.Value : ""));
        }

        public static string CreateCompareTitle(string make1, string model1, string make2, string model2)
        {
            return String.Format("{0} {1} vs {2} {3}", make1, model1, make2, model2);
        }
        public static string CreatePopularCompare(string model1, string model2)
        {
            return String.Format("{0} vs {1} ", model1, model2);
        }

        public static string CreateCompareTitle(string model1, string model2)
        {
            return String.Format("{0} vs {1}", model1, model2);
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
            return String.Format("/{0}-dealer-showrooms-in-{1}/", makeMaskingName, cityMaskingName);
        }

        /// <summary>
        /// Created By Vivek Gupta on 31-05-2016
        /// Desc : url format honda-bikes/dealers-in-mumbai/#{dealerId}
        /// </summary>
        /// <returns></returns>
        public static string DealerLocatorUrl(string makeMaskingName, string cityMaskingName, string hash)
        {
            return String.Format("/{0}-dealer-showrooms-in-{1}/#{2}", makeMaskingName, cityMaskingName, hash);
        }

        /// <summary>
        /// Created By Vivek gupta
        /// Date : 22 june 2016        
        //  Desc : used/bajaj-bikes-in-mumbai/#city=10&make=1&dist=0
        //  Modified By Sajal Gupta on 16/09/2016
        //  Desc: create links like used/bajaj-bikes-in-mumbai/  or used/bajaj-pulsar-bikes-in-mumbai/
        /// </summary>
        /// <param name="make"></param>
        /// <param name="city"></param>
        /// <returns></returns>

        public static string UsedBikesUrlNoCity(string make, string model, string city)
        {
            if (!String.IsNullOrEmpty(model))
                return String.Format("/used/{0}-{1}-bikes-in-{2}/", make, model, city);
            else
                return String.Format("/used/{0}-bikes-in-{1}/", make, city);
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
        public static string ViewMoreUsedBikes(uint cityId, string city, string make, string model)
        {
            if (cityId > 0)
            {
                if (!String.IsNullOrEmpty(model))
                    return String.Format("/used/{0}-{1}-bikes-in-{2}/", make, model, city);
                else
                    return String.Format("/used/{0}-bikes-in-{1}/", make, city);
            }
            else
            {
                if (!String.IsNullOrEmpty(model))
                    return String.Format("/used/{0}-{1}-bikes-in-india/", make, model);
                else
                    return String.Format("/used/{0}-bikes-in-india/", make);
            }
        }


        /// <summary>
        ///  Created By : Sushil Kumar 
        ///  Created On  : 16th August 2016
        ///  Description : To get article url for news,expert reviews and features listing pages
        /// Modified by :-Subodh 0n 08 nov 2016
        /// Description : To get article url for TipsAndAdvices
        /// </summary>
        /// <param name="basicId"></param>
        /// <param name="articleUrl"></param>
        /// <param name="articleType"></param>
        /// <returns></returns>
        public static string GetArticleUrl(string basicId, string articleUrl, string articleType)
        {
            string _articleUrl = string.Empty;
            EnumCMSContentType _contentType = default(EnumCMSContentType);
            if (!string.IsNullOrEmpty(articleType) && Enum.TryParse<EnumCMSContentType>(articleType, true, out _contentType))
            {
                switch (_contentType)
                {
                    case EnumCMSContentType.AutoExpo2016:
                    case EnumCMSContentType.News:
                        _articleUrl = string.Format("/news/{0}-{1}.html", basicId, articleUrl);
                        break;
                    case EnumCMSContentType.ComparisonTests:
                    case EnumCMSContentType.RoadTest:
                        _articleUrl = string.Format("/expert-reviews/{0}-{1}.html", articleUrl, basicId);
                        break;
                    case EnumCMSContentType.Features:
                    case EnumCMSContentType.SpecialFeature:
                        _articleUrl = string.Format("/features/{0}-{1}/", articleUrl, basicId);
                        break;
                    case EnumCMSContentType.TipsAndAdvices:
                        _articleUrl = string.Format("/bike-care/{0}-{1}.html", articleUrl, basicId);
                        break;
                    case EnumCMSContentType.TrackDay:
                        _articleUrl = string.Format("/trackday2016/{0}-{1}.html", basicId, articleUrl);
                        break;
                    default:
                        _articleUrl = "/";
                        break;
                }
            }
            return _articleUrl;
        }

        /// Created by: Aditi Srivastava on 27 Sep 2016
        /// Description: Replace special characters in url with hyphen and convert to lower case
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string RemoveSpecialCharUrl(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                url = (Regex.Replace(url, "[^0-9a-zA-Z]+", "-")).ToLower();
                url = (Regex.Replace(url, @"\-+", "-")).TrimEnd('-');
            }
            return url;
        }

        /// <summary>
        ///  Created By : Sajal Gupta 
        ///  Created On  : 26th sep 2016
        ///  Description : To get url for dealer.
        /// </summary>
        public static string GetDealerUrl(string makeMaskingName, string cityMaskingName, string dealerName, int dealerId)
        {
            string dealerUrl = string.Empty;
            dealerUrl = string.Format("/{0}-dealer-showrooms-in-{1}/{2}-{3}/", makeMaskingName, cityMaskingName, dealerId, RemoveSpecialCharUrl(dealerName));
            return dealerUrl;
        }

        /// <summary>
        ///  Created By : Sajal Gupta 
        ///  Created On  : 11-11-2016
        ///  Description : To get url for serviceCenter.
        /// </summary>
        public static string GetServiceCenterUrl(string makeMaskingName, string cityMaskingName, string serviceCenterName, uint serviceCenterId)
        {
            string dealerUrl = string.Empty;
            dealerUrl = string.Format("/{0}-service-center-in-{1}/{2}-{3}/", makeMaskingName, cityMaskingName, serviceCenterId, RemoveSpecialCharUrl(serviceCenterName));
            return dealerUrl;
        }

        /// <summary>
        ///  Created By : Sangram Nandkhile
        ///  Created On  : 11-11-2016
        ///  Description : To get url for serviceCenter.
        /// </summary>
        public static string GetServiceCenterUrl(string makeMaskingName, string cityMaskingName)
        {
            return string.Format("/{0}-service-center-in-{1}/", makeMaskingName, cityMaskingName);
        }
        /// <summary>
        ///  Created By : Aditi Srivastava
        ///  Created On  : 15 Dec 2016
        ///  Description : To get url for serviceCenter.
        /// </summary>
        public static string GetServiceCenterUrl(string makeMaskingName)
        {
            return string.Format("/{0}-service-center-in-india/", makeMaskingName);
        }
        /// <summary>
        ///  Created By : Subodh Jain 
        ///  Created On  : 20 Dec 2016
        ///  Description : To get url for dealershowroom.
        /// </summary>
        public static string GetDealerShowroomUrl(string makeMaskingName)
        {
            return string.Format("/{0}-dealer-showrooms-in-india/", makeMaskingName);
        }
    }
}

