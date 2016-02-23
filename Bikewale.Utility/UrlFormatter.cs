using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bikewale.Utility
{
    public static class UrlFormatter
    {
        public static string BikePageUrl(string makeMaskingName,string modelMaskingName)
        {
            return String.Format("/{0}-bikes/{1}/",makeMaskingName,modelMaskingName);
        }

        public static string VideoDetailPageUrl(string videoTitleUrl,string videoBasicId)
        {
            return String.Format("/bike-videos/{0}-{1}/", videoTitleUrl, videoBasicId);
        }

        public static string VideoByCategoryPageUrl(string videoCategory, string videoCatId)
        {
            return String.Format("/bike-videos/category/{0}-{1}/", Regex.Replace(videoCategory, @"[\(\)\s]+", "-").ToLower(), Regex.Replace(videoCatId, @"[,]+", "-"));
        }
    }
}
