﻿using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.PWA.Articles;
using Bikewale.Entities.Videos;
using Bikewale.Models;
using Bikewale.Utility;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace Bikewale.PWA.Utils
{
    public static class PwaCmsHelper
    {
        static ILog _logger = LogManager.GetLogger("PwaCmsHelper");
        static readonly bool _logNewsUrl = BWConfiguration.Instance.LogNewsUrl;
        /// <summary>
        /// Created by Prasad Gawde 
        /// </summary>
        /// <param name="articleSummary"></param>
        /// <returns></returns>
        public static string ReturnShareUrl(ArticleDetails articleSummary)
        {
            string _bwHostUrl = BWConfiguration.Instance.BwHostUrlForJs;
            EnumCMSContentType contentType = (EnumCMSContentType)articleSummary.CategoryId;
            string shareUrl = string.Empty;
            try
            {
                switch (contentType)
                {
                    case EnumCMSContentType.News:
                    case EnumCMSContentType.AutoExpo2016:
                        shareUrl = string.Format("{0}/news/{1}-{2}.html", _bwHostUrl, articleSummary.BasicId, articleSummary.ArticleUrl);
                        break;
                    case EnumCMSContentType.Features:
                        shareUrl = string.Format("{0}/features/{1}-{2}/", _bwHostUrl, articleSummary.ArticleUrl, articleSummary.BasicId);
                        break;
                    case EnumCMSContentType.RoadTest:
                        shareUrl = string.Format("{0}/expert-reviews/{1}-{2}.html", _bwHostUrl, articleSummary.ArticleUrl, articleSummary.BasicId);
                        break;
                    case EnumCMSContentType.SpecialFeature:
                        shareUrl = string.Format("{0}/features/{1}-{2}/", _bwHostUrl, articleSummary.ArticleUrl, articleSummary.BasicId);
                        break;
                    default:
                        break;
                }
            }
            finally
            {
                if (_logNewsUrl && shareUrl.EndsWith(@".html.html"))
                {
                    ThreadContext.Properties["ShareUrl"] = shareUrl;
                    _logger.Error("ConverterUtility.ReturnShareUrl");
                }

            }
            return shareUrl;
        }
        /// <summary>
        ///  Created by Prasad Gawde
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static string GetContentCategory(int contentType)
        {
            string _category = string.Empty;
            EnumCMSContentType _contentType = (EnumCMSContentType)contentType;

            switch (_contentType)
            {
                case EnumCMSContentType.AutoExpo2016:
                case EnumCMSContentType.News:
                    _category = "NEWS";
                    break;
                case EnumCMSContentType.Features:
                case EnumCMSContentType.SpecialFeature:
                    _category = "FEATURES";
                    break;
                case EnumCMSContentType.ComparisonTests:
                case EnumCMSContentType.RoadTest:
                    _category = "EXPERT REVIEWS";
                    break;
                case EnumCMSContentType.TipsAndAdvices:
                    _category = "Bike Care";
                    break;
                default:
                    break;
            }
            return _category;
        }
        /// <summary>
        ///  Created by Prasad Gawde
        /// </summary>
        /// <param name="contentType"></param>
        /// <param name="url"></param>
        /// <param name="basicid"></param>
        /// <returns></returns>
        public static string GetArticleUrl(int contentType, string url, int basicid)
        {
            string articleUrl = string.Empty;
            EnumCMSContentType _contentType = (EnumCMSContentType)contentType;
            switch (_contentType)
            {
                case EnumCMSContentType.AutoExpo2016:
                case EnumCMSContentType.News:
                    articleUrl = string.Format("/m/news/{0}-{1}.html", basicid, url);
                    break;
                case EnumCMSContentType.Features:
                case EnumCMSContentType.SpecialFeature:
                    articleUrl = string.Format("/m/features/{0}-{1}/", url, basicid);
                    break;
                case EnumCMSContentType.ComparisonTests:
                case EnumCMSContentType.RoadTest:
                    articleUrl = string.Format("/m/expert-reviews/{0}-{1}.html", url, basicid);
                    break;
                case EnumCMSContentType.TipsAndAdvices:
                    articleUrl = string.Format("/m/bike-care/{0}-{1}.html", url, basicid);
                    break;
                case EnumCMSContentType.Videos:
                    articleUrl = string.Format("/m/videos/{0}-{1}/", url, basicid);
                    break;
                default:
                    break;
            }
            return articleUrl;
        }
        /// <summary>
        /// Created by Prasad Gawde
        /// </summary>
        /// <param name="videos"></param>
        /// <param name="catId"></param>
        /// <returns></returns>
        public static PwaVideosBySubcategory SetPwaSubCategoryVideos(IEnumerable<BikeVideoEntity> videos, int catId)
        {
            PwaVideosBySubcategory vidInfo = new PwaVideosBySubcategory();
            if (videos != null)
            {
                vidInfo.Videos = ConverterUtility.PwaConvert(videos);
                string secTitle, moreVidUrl;
                VideoTitleDescription.VideoGetTitleAndUrl(catId, out secTitle, out moreVidUrl);
                vidInfo.SectionTitle = secTitle;
                vidInfo.MoreVideosUrl = moreVidUrl;
            }
            return vidInfo;
        }
        /// <summary>
        ///  Created by Prasad Gawde
        /// </summary>
        /// <param name="videos"></param>
        /// <param name="catId"></param>
        /// <returns></returns>
        public static PwaVideosBySubcategory SetPwaSubCategoryVideos(IEnumerable<PwaBikeVideoEntity> videos, int catId)
        {
            PwaVideosBySubcategory vidInfo = new PwaVideosBySubcategory();
            vidInfo.Videos = videos;
            string secTitle, moreVidUrl;
            VideoTitleDescription.VideoGetTitleAndUrl(catId, out secTitle, out moreVidUrl);
            vidInfo.SectionTitle = secTitle;
            vidInfo.MoreVideosUrl = moreVidUrl;
            return vidInfo;
        }
        /// <summary>
        ///  Created by Prasad Gawde
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetSha256Hash(string input)
        {
            SHA256 shaHash = new SHA256Managed();
            // Convert the input string to a byte array and compute the hash.
            byte[] data = shaHash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

    }
}
