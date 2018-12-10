using Carwale.DTOs.CMS;
using Carwale.Entity.CarData;
using Carwale.Entity.Enum;
using Carwale.Utility;
using MobileWeb.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Carwale.UI.PresentationLogic
{
    public static class EditorialContent
    {
        private static readonly string _imageHostUrl = (ConfigurationManager.AppSettings["CDNHostURL"]??"").ToString();

        public static string GetListingPaginationUrl(int page, bool isMobile, NewsListingDTO pageModel)
        {
            string url = string.Empty;
            if (pageModel.category == "images")
            {
                url = string.Format("/{0}{1}/{2}{3}", (isMobile ? "m/" : string.Empty), pageModel.category, (page > 1 ? string.Format("page/{0}/", page) : string.Empty), 
                                    (pageModel.make != null ? "?make=" + pageModel.make.name : string.Empty));
            }
            else
            {
                url = string.Format("/{0}{1}{2}{3}/{4}", (isMobile ? "m/" : string.Empty), (pageModel.make != null ? Format.FormatSpecial(pageModel.make.name) + "-cars/": string.Empty),
                    (pageModel.model != null ? pageModel.model.name + "/" : string.Empty), pageModel.category, (page > 1 ? string.Format("page/{0}/", page) : string.Empty));
            }
            return url;
        }

        public static string GetColorUrl(string makeName, string maskingName, string colorName, int colorId, bool isMsite)
        {
            makeName = Format.RemoveSpecialCharacters(makeName);
            string imgUrl = string.Empty;
            if (colorName != null && colorId > 0)
            {
                string colorUrl = String.Format("{0}-{1}", Format.FormatUrlColor(colorName), colorId);
                imgUrl = string.Format("{0}/{1}-cars/{2}/colours/{3}/", isMsite ? "/m" : string.Empty, makeName, maskingName, colorUrl);
            }
            else
            {
                imgUrl = string.Format("{0}/{1}-cars/{2}/colours/", isMsite ? "/m" : string.Empty, makeName, maskingName);
            }
            return imgUrl;
        }

        public static string GetReviewPageUrl(int makeId, int modelId, int versionId)
        {
            return string.Format("user-reviews/rate-car/{0}{1}{2}", (makeId > 0 ? string.Format("?makeId={0}", makeId) : string.Empty), (modelId > 0 ? string.Format("&modelId={0}", modelId) : string.Empty), (versionId > 0 ? string.Format("&versionId={0}", versionId) : "&versionId=-1"));
        }

        public static string GetCompletePageUrl(HttpRequestBase request, string url)
        {
            return string.Format("{0}{1}{2}:{3}{4}", request.Url.Scheme, Uri.SchemeDelimiter, request.Url.Host, request.Url.Port, url);
        }

        public static string GetYoutubeThumbnailUrl(string videoId)
        {
            return string.Format("https://img.youtube.com/vi/{0}/1.jpg", videoId);
        }

        public static string Get360PageUrl(string makeName, string maskingName, ThreeSixtyViewCategory category, bool isMsite)
        {
            return string.Format("{0}/{1}-cars/{2}/360-view/{3}", isMsite ? "/m" : string.Empty, Format.FormatSpecial(makeName), maskingName, (category == ThreeSixtyViewCategory.Closed) ? string.Empty : (category.ToString().ToLower() + "/"));
        }

        public static string Get360SlugImage(string makeName, int modelId, ThreeSixtyViewCategory category, string imageSize, bool isMsite)
        {
            string imageName = (category == ThreeSixtyViewCategory.Interior) ? "1_preview.jpg?ao=1" : "15.jpg?q=65";
            string imageFolderName = string.Empty;
            switch (category)
            {
                case ThreeSixtyViewCategory.Closed:
                    imageFolderName = "closed-door";
                    break;
                case ThreeSixtyViewCategory.Open:
                    imageFolderName = "open-door";
                    break;
                case ThreeSixtyViewCategory.Interior:
                    imageFolderName = "interior/" + (isMsite ? "m" : "d");
                    break;
            }

            return string.Format("{0}{1}/cw/360/{2}/{3}/{4}/{5}", _imageHostUrl, imageSize, Format.FormatSpecial(makeName), modelId, imageFolderName, imageName);
        }

        public static string FilterImageName(string imageName, string makeName, string modelName, int imageId)
        {
            if (imageName != null)
            {
                var arr = imageName.Split(new string[] { ".jpg" }, StringSplitOptions.None);
                if (arr.Length > 0)
                {
                    imageName = arr[0];
                }
                imageName= imageName.Replace('-', ' ');
                imageName = imageName.Replace(string.Format("{0} {1}", makeName, Format.FilterModelName(modelName)), string.Empty);
                imageName = imageName.Replace(imageId.ToString(), "").Trim();
            }
            return imageName;
        }

        public static string GetUserReviewUrl(string makeName, string maskingName, int reviewId = 0, bool isMsite = false)
        {
            return string.Format("{0}/{1}-cars/{2}/userreviews/{3}", isMsite ? "/m" : string.Empty, Format.FormatSpecial(makeName), maskingName, 
                reviewId > 0 ? string.Format("{0}/", reviewId) : string.Empty);
        } 
    }
}