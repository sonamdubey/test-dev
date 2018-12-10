using Carwale.Entity.CarData;
using Carwale.Entity.CMS.Articles;
using Carwale.Entity.Enum;
using Carwale.Notifications;
using Carwale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Carwale.DTOs.CMS.ThreeSixtyView; 

namespace Carwale.BL.CMS
{
    /// <summary>
    /// author : sachin bharti on 25/11/15
    /// </summary>
    public class CMSCommon
    {
        /// <summary>
        /// author : sachin bharti on 25/11/15
        /// purpose : return all distinct models tagged to the artcile
        /// </summary>
        /// <param name="vehicleTagList"></param>
        /// <returns></returns>
        public static IEnumerable<int> GetDistinctModels(List<VehicleTag> vehicleTagList)
        {
            return (from item in vehicleTagList
                    where item.ModelBase.ModelId > 0
                    select item.ModelBase.ModelId).ToList().Distinct();
        }

        public static string GetArticleUrl(int categoryId, string makeName, string modelMaskingName, string articleMaskingName, bool isMsite = false)
        {
            string url = string.Empty;
            try
            {
                switch(categoryId)
                {
                    case 2:
                        url = string.Format("{0}/expert-reviews/{1}/", isMsite ? "/m" : string.Empty, articleMaskingName);
                        break;
                    case 8:
                        url = string.Format("{0}/{1}-cars/{2}/expert-reviews/{3}/", isMsite ? "/m" : string.Empty, Format.FormatSpecial(makeName), modelMaskingName, articleMaskingName);
                        break;
                }
                
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "Carwale.BL.CMS");
                objErr.LogException();
            }
           return url;
        }

        public static bool IsThreeSixtyViewAvailable(CarModelDetails modelDetails)
        {
            return (modelDetails != null && (modelDetails.Is360ExteriorAvailable || modelDetails.Is360OpenAvailable || modelDetails.Is360InteriorAvailable));
        }

        public static string Get360ModelCarouselLinkageImageUrl(CarModelDetails modelDetails, bool forceInterior=false)
        {
            if (IsThreeSixtyViewAvailable(modelDetails))
            {
                string rootPath = string.Format("/cw/360/{0}/{1}/", Format.FormatSpecial(modelDetails.MakeName), modelDetails.ModelId);

                string exteriorImageUrl = rootPath + string.Format("{0}-door/15.jpg", modelDetails.Is360ExteriorAvailable ? "closed" : modelDetails.Is360OpenAvailable ? "open" : string.Empty);
                string interiorImageUrl = rootPath + "interior/m/1.jpg";
                if (forceInterior) return modelDetails.Is360InteriorAvailable ? interiorImageUrl : string.Empty;
                return ((modelDetails.Is360ExteriorAvailable || modelDetails.Is360OpenAvailable) ? exteriorImageUrl : interiorImageUrl);
            }
            else
                return null;
        }

        public static string GetImageUrl(string makeName, string maskingName, string imageName = null, uint imageId = 0, bool isMsite = false)
        {
            string imgUrl = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(makeName) || string.IsNullOrEmpty(maskingName))
                    return string.Format("{0}/images/", isMsite ? "/m" : string.Empty);

                makeName = Format.RemoveSpecialCharacters(makeName);
                if (!string.IsNullOrEmpty(imageName))
                {
                    int ind = imageName.IndexOf(".jpg");
                    imageName = RemoveSpecialChars(imageName);

                    if (ind > 0)
                    {
                        imageName = imageName.Substring(0, ind);
                        var hyphenIndex = imageName.IndexOf('-');
                        if (hyphenIndex < 0)
                            imageName = string.Format("-{0}", imageId);
                        imgUrl = string.Format("{0}/{1}-cars/{2}/images/{3}/", isMsite ? "/m" : string.Empty, makeName, maskingName, imageName.ToLower());
                    }
                    else
                        imgUrl = string.Format("{0}/{1}-cars/{2}/images/{3}-{4}/", isMsite ? "/m" : string.Empty, makeName, maskingName, imageName.ToLower(), imageId);
                }
                else
                    imgUrl = string.Format("{0}/{1}-cars/{2}/images/", isMsite ? "/m" : string.Empty, makeName, maskingName);
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CMSContent.GetImageUrl");
                objErr.LogException();
            }
            return imgUrl;
        }

        public static string RemoveAnchorTag(string str)
        {
            str = Regex.Replace(str, @"<a [^>]+>(.*?)<\/a>", "$1");
            return str;
        }

        public static bool IsModelColorPhotosPresent(List<ModelColors> modelColors)
        {
            if (modelColors != null && modelColors.Count > 0)
            {
                int count = 0;
                modelColors.ForEach(x => count = (!string.IsNullOrEmpty(x.OriginalImgPath) ? count + 1 : count));
                if ((double)count / modelColors.Count >= 0.7)
                    return true;
            }
            return false;
        }

        public static string GetVideoUrl(string makeName, string maskingName, string title, int basicId, bool isMsite = false)
        {
            if (string.IsNullOrEmpty(makeName) && string.IsNullOrEmpty(maskingName))
                return string.Format("{0}/videos/", isMsite ? "/m" : string.Empty);

            if (string.IsNullOrEmpty(title) || basicId <= 0)
                return string.Format("{0}{1}videos/", isMsite ? "/m" : string.Empty, ManageCarUrl.CreateModelUrl(makeName, maskingName));
            
            title = RemoveSpecialCharsAndExtraSpace(title);
            return string.Format("{0}{1}videos/{2}-{3}/", isMsite ? "/m" : string.Empty,ManageCarUrl.CreateModelUrl(makeName, maskingName), title.Trim().ToLower().Replace(" ", "-"), basicId);
        }

        public static ThreeSixtyViewCategory Get360DefaultCategory(ThreeSixtyAvailabilityDTO threeSixtyDetails)
        {
                return threeSixtyDetails.Is360ExteriorAvailable ? ThreeSixtyViewCategory.Closed : threeSixtyDetails.Is360OpenAvailable ? ThreeSixtyViewCategory.Open : ThreeSixtyViewCategory.Interior;
        }

        public static bool CheckCategoryAvailable(CarModelDetails modelDetails, ThreeSixtyViewCategory category)
        {
            return (category == null && modelDetails.Is360ExteriorAvailable) || (category != null && ((category == ThreeSixtyViewCategory.Closed && modelDetails.Is360ExteriorAvailable) || (category == ThreeSixtyViewCategory.Open && modelDetails.Is360OpenAvailable) || (category == ThreeSixtyViewCategory.Interior && modelDetails.Is360InteriorAvailable)));
        }

        public static string RemoveSpecialChars(string str)
        {
            str = Regex.Replace(str, @"[!@#$%^&*(),.?:{}|<>]", "");
            return str;
        }
        public static string RemoveSpecialCharsAndExtraSpace(string str)
        {
            str = Regex.Replace(str, "[^a-zA-Z 0-9]+", "");
            str = Regex.Replace(str, "\\s+", " ");
            return str;
        }
    }
}