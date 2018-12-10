using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Carwale.UI.Common;
using Carwale.Entity.Deals;
using Carwale.Utility;
using Carwale.Cache.Deals;
using Carwale.Interfaces;
using Carwale.Interfaces.Deals;
using AEPLCore.Cache;
using Carwale.DAL.Deals;
using Carwale.Entity.Enum;

namespace Carwale.UI.PresentationLogic
{
    public class Advanatge
    {
        public static bool IsCityMumbaiAndAround
        {
            get { return (CookiesCustomers.MasterCityId == 1 || CookiesCustomers.MasterCityId == 13 || CookiesCustomers.MasterCityId == 40 || CookiesCustomers.MasterCityId == 128 || CookiesCustomers.MasterCityId == 12); }
        }

        public static bool IsSelectCity
        {
            get { return CookiesCustomers.MasterCityId == -1; }
        }

        public static string GetAdvantageSlugClass(DiscountSummary discountSummary)
        {
            return discountSummary != null && (discountSummary.MaxDiscount > 0 || (discountSummary.MaxDiscount == 0 && !String.IsNullOrEmpty(discountSummary.Offers)))? string.Empty : "hide";
        }

        public static string GetAdvantageSlugClass(object discount)
        {
            int maxDiscount;
            return (Int32.TryParse(discount.ToString(), out maxDiscount) && maxDiscount > 0) ? string.Empty : "hide";
        }

        public static string GetAdvantageURL(string make, string maskingName, int cityId = 0, int versionId = 0, string src = "")
        {
            return Format.FormatSpecial(make) + "-cars/" + maskingName + "/advantage/?cityId=" + cityId + (string.IsNullOrWhiteSpace(src) ? string.Empty : "&src=" + src) + (versionId > 0 ? "&versionId=" + versionId : string.Empty);
        }

        public static bool ShowAdvantageDiscountLink
        {
            get
            {
                if (CookiesCustomers.MasterCityId == -1 || IsAdvantageCity(CookiesCustomers.MasterCityId, 3))
                    return true;
                else
                    return false;
            }
        }

        public static bool IsAdvantageCity(int cityId, int minimumStockCount)
        {
            if (cityId > 0)
            {
                DealsCache cache = new DealsCache(new CacheManager(), new DealsRepository());
                List<int> cityList = cache.GetCitiesWithMoreModels(minimumStockCount);
                if (cityList != null)
                    return cityList.Any(x => x == cityId);
                else
                    return false;
            }
            else
                return false;
        }
        
        public static string GetRedirectionUrl(int cityId, Platform platformId)
        {
            string prefix = platformId == Platform.CarwaleDesktop ? "" : "/m";
            string suffix = cityId > 0 ? "?cityid=" + cityId : "";
            return string.Format("{0}/advantage/{1}", prefix, suffix);
        }

        
    }

}