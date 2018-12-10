using System;
using System.Collections.Generic;
using System.Configuration;

namespace Carwale.Utility
{
    public static class ManageCarUrl
    {
        private static string _hostName = string.Format("https://{0}", ConfigurationManager.AppSettings["HostUrl"]);
        public static string CreateMakeUrl(string makeName, bool isMsite = false, bool isAbsoluteUrl = false)
        {
            return !string.IsNullOrWhiteSpace(makeName) ? string.Format("{0}{1}/{2}-cars/",
                (isAbsoluteUrl ? _hostName : string.Empty), (isMsite ? "/m" : string.Empty), Format.FormatSpecial(makeName)) : string.Empty;
        }

        public static string CreateModelUrl(string makeName, string maskingName, bool isAbsoluteUrl = false)
        {
            return (!string.IsNullOrWhiteSpace(makeName) && !string.IsNullOrWhiteSpace(maskingName)) ? string.Format("{0}/{1}-cars/{2}/",
                (isAbsoluteUrl ? _hostName : string.Empty), Format.FormatURL(makeName), maskingName) : string.Empty;
        }
        public static string CreateVersionUrl(string makeName, string maskingName, string versionMaskingName, bool isMsite = false, bool isAbsoluteUrl = false)
        {
            string modelUrl = CreateModelUrl(makeName, maskingName);
            return (!string.IsNullOrWhiteSpace(modelUrl) && !string.IsNullOrWhiteSpace(versionMaskingName)) ? string.Format("{0}{1}{2}{3}/",
                (isAbsoluteUrl ? _hostName : string.Empty), (isMsite ? "/m" : string.Empty), modelUrl, versionMaskingName) : string.Empty;
        }
        public static string CreateUserReviewUrl(string makeName, string maskingName, bool isMsite = false, bool isAbsoluteUrl = false)
        {
            string modelUrl = CreateModelUrl(makeName, maskingName);
            return (!string.IsNullOrWhiteSpace(modelUrl)) ? string.Format("{0}{1}{2}userreviews/", isAbsoluteUrl ? _hostName : string.Empty, isMsite ? "/m" : string.Empty, modelUrl) : string.Empty;
        }

        public static string CreateUserReviewDetailsUrl(string makeName, string maskingName, int reviewId, bool isMsite = false, bool isAbsoluteUrl = false)
        {
            string modelUrl = CreateModelUrl(makeName, maskingName);
            return (!string.IsNullOrWhiteSpace(modelUrl)) ? string.Format("{0}{1}{2}userreviews/{3}/",
                isAbsoluteUrl ? _hostName : string.Empty, isMsite ? "/m" : string.Empty, modelUrl, reviewId) : string.Empty;
        }

        public static string CreateAdvantageUrl(string makeName, string maskingName, string src, int cityId = -1, bool isMsite = false, bool isAbsoluteUrl = false)
        {
            string modelUrl = CreateModelUrl(makeName, maskingName);
            return (!string.IsNullOrWhiteSpace(modelUrl)) ? string.Format("{0}{1}{2}advantage/?src={3}&cityid={4}",
                (isAbsoluteUrl ? _hostName : string.Empty), (isMsite ? "/m" : string.Empty), modelUrl, src, cityId > 0 ? cityId.ToString() : string.Empty) : string.Empty;
        }

        public static string CreatePriceInCityUrl(string makeName, string maskingName, string cityMaskingName, bool isAbsoluteUrl = false)
        {
            return (!string.IsNullOrWhiteSpace(makeName) && !string.IsNullOrWhiteSpace(maskingName) && !string.IsNullOrWhiteSpace(cityMaskingName)) ?
                 string.Format("{0}/{1}-cars/{2}/price-in-{3}/", (isAbsoluteUrl ? _hostName : string.Empty), Format.FormatURL(makeName),
                 maskingName, cityMaskingName) : string.Empty;
        }

       public static string CreateTopCarsByBodyTypeUrl(string bodyTypeText, bool isAbsoluteUrl = false)
        {
            return string.IsNullOrWhiteSpace(bodyTypeText) ? string.Empty : $"{(isAbsoluteUrl ? _hostName : string.Empty)}/best-{bodyTypeText}-in-india/";
       }

        public static string CreateMakeDealerUrl(string makeName)
        {
            return (!string.IsNullOrWhiteSpace(makeName)) ? string.Format("/new/{0}-dealers/", Format.FormatURL(makeName)) : string.Empty;
        }

        public static string TyreDetailPageUrl(string brandName, string modelName, string tyreSize, int tyreId)
        {
            string tyrePageUrl = string.Format("/tyres/{0}/{1}/{2}-{3}/", brandName, modelName, tyreSize.Replace('/', '-'), tyreId).Replace(' ', '-').Replace("--", "-").ToLower();
            return tyrePageUrl;
        }
        public static string TyreSrpPageUrl(string carMakeName, string carModelName, uint carModelId, string makeYear, bool isMobile, uint versionId = 0)
        {
            string tyreSrpPageUrl = string.Format("/tyres/{0}-{1}-tyres/?cmids={2}&year={3}", Format.FormatURL(carMakeName), Format.FormatURL(carModelName), carModelId, makeYear);
            if (versionId > 0)
            {
                tyreSrpPageUrl = string.Format("{0}&versionid={1}", tyreSrpPageUrl, versionId);
            }
            return isMobile ? Format.FormatTyresURL(string.Format("/m{0}", tyreSrpPageUrl)) : Format.FormatTyresURL(tyreSrpPageUrl);

        }


        public static string CreateExpertReviewUrl(string makeName, string maskingName, bool isMsite = false, bool isAbsoluteUrl = false)
        {
            string modelUrl = CreateModelUrl(makeName, maskingName);
            return (!string.IsNullOrWhiteSpace(modelUrl)) ? string.Format("{0}{1}{2}expert-reviews/", isAbsoluteUrl ? _hostName : string.Empty, isMsite ? "/m" : string.Empty, modelUrl) : string.Empty;
        }
        public static string CreateImageListingPageUrl(string makeName, string maskingName, bool isMsite = false, bool isAbsoluteUrl = false)
        {
            string modelUrl = CreateModelUrl(makeName, maskingName);
            return (!string.IsNullOrWhiteSpace(modelUrl)) ? string.Format("{0}{1}{2}images/", isAbsoluteUrl ? _hostName : string.Empty, isMsite ? "/m" : string.Empty, modelUrl) : string.Empty;
        }
        public static string CreateCompareCarUrl(List<Tuple<int, string>> data, bool isAbsoluteUrl = false, bool isMsite = false)
        {
            return string.Format("{0}{1}{2}/{3}/", isAbsoluteUrl ? _hostName : string.Empty, isMsite ? "/m" : string.Empty, "/comparecars", Format.GetCompareUrl(data));
        }
        public static string CreateBestCarUnderPriceUrl(string bodyType, int price, string unit, bool isAbsoluteUrl = false)
        {
            return string.Format("{0}/best-{1}s-under-{2}-{3}s/", isAbsoluteUrl ? _hostName : string.Empty, bodyType, price, unit);
        }
        public static string CreateMakeImagePageUrl(string makeName, bool isMsite = false, bool isAbsoluteUrl = false)
        {    
            return  string.Format("{0}{1}/images/?make={2}", isAbsoluteUrl ? _hostName : string.Empty, isMsite ? "/m" : string.Empty, Format.FormatURL(makeName));
        }

		public static string CreateRatingPageUrl(int modelId, int versionId = 0, bool isAbsoluteUrl = false)
		{
			return string.Format("{0}/userreviews/rate-car/?modelid={1}{2}", isAbsoluteUrl ? _hostName : string.Empty, modelId, versionId > 0 ? $"&versionid={versionId}" : string.Empty);
		}
        public static string CreateFuelTypeUrl(string fuelType, bool isAbsoluteUrl = false)
		{
			return string.Format("{0}/best-{1}-cars-in-india/", isAbsoluteUrl ? _hostName : string.Empty, fuelType.ToLower());
		}
	}
}
