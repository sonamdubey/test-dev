
using Carwale.Entity.Enum;
using Carwale.Entity.Common;
using Carwale.Notifications.Logs;
using Carwale.Utility;
using System;
using System.Collections.Generic;
using System.Web;

namespace Carwale.BL.Experiments
{
    public static class ProductExperiments
    {
        private static bool IsInRange(Array range, int value)
        {
            try
            {
                return value >= Convert.ToInt32(range.GetValue(0)) && value <= Convert.ToInt32(range.GetValue(1));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "ProductExperiments.IsInRange");
            }
            return false;
        }

        public static bool IsEligibleForORP()
        {
            try
            {
                return Convert.ToBoolean(HttpContext.Current.Items["IsEligibleForORP"]);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "ProductExperiments.IsEligibleForORP()");
            }
            return false;
        }

        public static bool IsShowExperimentalColor(int abTestKeyValue)
        {
            return true;
        }

        public static ProductExperiment GetMyListingMenuType(int abTestKeyValue)
        {
            if (IsInRange(CWConfiguration.AbTestValueForClassifiedMyListingsRename, abTestKeyValue))
            {
                return new ProductExperiment
                {
                    TestVersion = ProductExperiment.TestVersions.Test1,
                    Description = "Rename MyListing to Edit Ad"
                };
            }
            else
            {
                return new ProductExperiment
                {
                    IsControl = true,
                    Description = "Control"
                };
            }
        }

        public static bool IsNewRecommendationLogic(int abTestKeyValue)
        {
            return abTestKeyValue >= 81 && abTestKeyValue <= 100;
        }

        public static bool IsPicPqMerger(int makeId)
        {
            return (makeId == 7 || makeId == 9 || makeId == 8 || makeId == 16 || makeId == 10);
        }
        public static bool IsPicTableFormat(int modelId, int cityId)
        {
            return (modelId == 976) && (cityId == 13);
        }

        public static bool ShowGetEmiAssistanceLink()
        {
            var abTest = CustomerCookie.AbTest;
            if (abTest >= 1 && abTest <= 50)
            {
                return true;
            }
            return false;
        }

        public static bool ShowChangeTextLink()
        {
            var abTest = CustomerCookie.AbTest;
            if (abTest >= 51 && abTest <= 100)
            {
                return true;
            }
            return false;
        }

        public static bool ShowCustomiseYourEmiButton()
        {
            var abTest = CustomerCookie.AbTest;
            if (abTest >= 1 && abTest <= 100)
            {
                return true;
            }
            return false;
        }

        public static string VoucherPrice(int platformId)
        {
            if (platformId == 74 || platformId == 83)
            {
                return "2,000";
            }
            return (DateTime.Now.Date.Month == 10 && DateTime.Now.Date.Day >= 8 && DateTime.Now.Date.Day <= 14) ? "4,000" : "2,000";
        }
        public static ExperimentAdSlot GetCompareCarsExperimentAdSlot(int abTest)
        {
            ExperimentAdSlot adSlot = new ExperimentAdSlot();
            if (abTest <= 30)
            {
                adSlot.Name = "Carwale_Mob_compare_320x50";
                adSlot.Size = "[320, 50]";
            }
            else if (abTest <= 60)
            {
                adSlot.Name = "Carwale_Mob_compare_320x100";
                adSlot.Size = "[320, 100]";
            }
            else
            {
                adSlot.Name = "CarWale_Mobile_CompareCar_BTF";
                adSlot.Size = "[[320, 100], [320, 50]]";
            }
            return adSlot;
        }

        public static string GetH1ForTopCarsPage(CarBodyStyle bodyType)
        {
            return bodyType == CarBodyStyle.Hatchback ? "BEST" : "TOP";
        }        
        public static bool showCustomNotificationPopup(bool isMobile)
        {
            return (!isMobile && CustomerCookie.AbTest >= 1 && CustomerCookie.AbTest <= 10);
        }

        #region Desktop New Pq Page Experiments

        public static bool ShowLinkOnBottomLeft()
        {
            var abTest = CustomerCookie.AbTest;
            return abTest > 0 && abTest < 41;
        }
        public static bool ShowToolTip()
        {
            var abTest = CustomerCookie.AbTest;
            return abTest > 20 && abTest < 41;
        }
        public static bool ShowRedSolidButton()
        {
            var abTest = CustomerCookie.AbTest;
            return abTest > 40 && abTest < 61;
        }
        public static bool ShowChangeTextLinkOnDesk()
        {
            var abTest = CustomerCookie.AbTest;
            return abTest > 60 && abTest < 81;
        }
        public static bool ShowGetEmiOffersButton()
        {
            var abTest = CustomerCookie.AbTest;
            return abTest > 60 && abTest < 81;
        }
        #endregion
    }
    public class ProductExperiment
    {
        public string Description { get; set; }
        public bool IsControl { get; set; }
        public TestVersions TestVersion { get; set; }

        public enum TestVersions
        {
            Test1 = 1,
            Test2 = 2,
            Test3 = 3,
            Test4 = 4,
            Test5 = 5,
        }
    }
}
