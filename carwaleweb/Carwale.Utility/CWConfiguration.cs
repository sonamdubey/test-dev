using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Utility
{
    ///<summary>
    /// Created By: Jitendra solanki 10 jan 2017
    /// Summary: Class will Access to Web configuration data.
    /// </summary>
    public static class CWConfiguration
    {
        public readonly static int GrpcCampaignChannelWaitTime = Convert.ToInt32(ConfigurationManager.AppSettings["GrpcCampaignChannelWaitTime"] ?? "500");
        public readonly static int GrpcBhriguChannelWaitTime = Convert.ToInt32(ConfigurationManager.AppSettings["GrpcBhriguChannelWaitTime"] ?? "500");
        public readonly static string GrpcArticleServerList = ConfigurationManager.AppSettings["GrpcArticleServerList"] ?? string.Empty;
        public readonly static int GrpcRetryCount = Convert.ToInt32(ConfigurationManager.AppSettings["GrpcRetryCount"] ?? "1");
        public readonly static int GrpcChannelWaitTime = Convert.ToInt32(ConfigurationManager.AppSettings["GrpcChannelWaitTime"] ?? "2000");
        public readonly static int GrpcPoolSize = Convert.ToInt32(ConfigurationManager.AppSettings["GrpcPoolSize"] ?? "1");

        public readonly static int RelatedArticleCount = CustomParser.parseIntObject(ConfigurationManager.AppSettings["relatedArticleCount"]);

        public readonly static string WebContentSegmentList = ConfigurationManager.AppSettings["WebContentSegmentList"] ?? string.Empty;

        public static readonly string _imgHostUrl = ConfigurationManager.AppSettings["CDNHostURL"] ?? "https://imgd.aeplcdn.com/";

        public static readonly string ArticleViewQueue = ConfigurationManager.AppSettings["ArticleViewQueue"] ?? "EditCms_Article_Updater";

        private static readonly List<int> _ExpertCategories = (ConfigurationManager.AppSettings["ExpertCategories"] ?? string.Empty).ConvertStringToList<int>();
        public static List<int> ExpertCategories { get { return _ExpertCategories; } }

        private static readonly List<int> _FeatureCategories = (ConfigurationManager.AppSettings["FeatureCategories"] ?? string.Empty).ConvertStringToList<int>();
        public static List<int> FeatureCategories { get { return _FeatureCategories; } }

        private static readonly List<int> _NewsCategories = (ConfigurationManager.AppSettings["NewsCategories"] ?? string.Empty).ConvertStringToList<int>();
        public static List<int> NewsCategories { get { return _NewsCategories; } }

        public static readonly string SponsoredAuthorId = ConfigurationManager.AppSettings["sponsoredauthor"] ?? string.Empty;

        public static readonly string IMAGE_PATTERN = "(jpg|png|gif|bmp)";

        public static readonly string IMAGE_PATH_PATTERN = @"(.*\/)(?=.*\.(?:jpg|png|gif|bmp).*)";
        public readonly static bool LogGrpcCallTimings = Convert.ToBoolean(ConfigurationManager.AppSettings["LogGrpcCallTimings"] ?? "false");
        public readonly static int GrpcCallTimeLimitCheckValue = Convert.ToInt32(ConfigurationManager.AppSettings["GrpcCallTimeLimitCheckValue"] ?? "500");
        public static readonly string EditCMSModuleName = ConfigurationManager.AppSettings["EditCMSModuleName"];
        public static readonly string NewCarConsumerModuleName = ConfigurationManager.AppSettings["NewCarConsumerModuleName"] ?? "newcarconsumers-local";
        public static double INTEREST_RATE { get { return CustomParser.parseDoubleObject((ConfigurationManager.AppSettings["interestRate"] ?? "0").ToString()); } }
        public static int TENURE { get { return CustomParser.parseIntObject((ConfigurationManager.AppSettings["tenure"] ?? "0").ToString()); } }
        private static readonly Array _TableDescriptionFuelFirstMakes = (ConfigurationManager.AppSettings["TableDescriptionFuelFirstMakes"] ?? string.Empty).ConvertStringToArray();
        public static Array TableDescriptionFuelFirstMakes { get { return _TableDescriptionFuelFirstMakes; } }

        private static readonly Array _BestCarsBodyTypes = (ConfigurationManager.AppSettings["BestCarsBodyTypes"] ?? "1,3,6,10").ConvertStringToArray();
        public static Array BestCarsBodyTypes { get { return _BestCarsBodyTypes; } }

        public static readonly ushort TopCarByBodyTypeCount = Convert.ToUInt16(ConfigurationManager.AppSettings["TopCarByBodyTypeCount"] ?? "10");

        private static readonly Array _askingAreaCityIds = (ConfigurationManager.AppSettings["AskingAreaCityIds"] ?? "1,2,10,12").ConvertStringToArray();
        public static Array AskingAreaCityIds { get { return _askingAreaCityIds; } }

        private readonly static Array _hdfcCityList = (ConfigurationManager.AppSettings["HDFCCityList"] ?? "").ConvertStringToArray();
        public static Array HdfcCityList { get { return _hdfcCityList; } }

        public readonly static string _AdTargetingServerList = ConfigurationManager.AppSettings["GrpcAdTargetingServerList"] ?? string.Empty;

        public static int AdtargetingChannelTime { get { return CustomParser.parseIntObject((ConfigurationManager.AppSettings["GrpcUserProfilingChannelWaitTime"] ?? "20")); } }

        private readonly static string _CwRecaptchaSecretKey = ConfigurationManager.AppSettings["CWRecaptchaSecretKey"] ?? string.Empty;
        public static string CwRecaptchaSecretKey { get { return _CwRecaptchaSecretKey; } }


        private readonly static List<string> _chromeCaptchaEnabledVersions = (ConfigurationManager.AppSettings["ChromeCaptchaEnabledVersions"] ?? "").Split(',').ToList();
        public static List<string> ChromeCaptchaEnabledVersions { get { return _chromeCaptchaEnabledVersions; } }

        private readonly static List<string> _androidWebviewCaptchaEnabledVersions = (ConfigurationManager.AppSettings["AndroidWebviewCaptchaEnabledVersions"] ?? "").Split(',').ToList();
        public static List<string> AndroidWebviewCaptchaEnabledVersions { get { return _androidWebviewCaptchaEnabledVersions; } }

        private readonly static List<string> _androidBrowserCaptchaEnabledVersions = (ConfigurationManager.AppSettings["AndroidBrowserCaptchaEnabledVersions"] ?? "").Split(',').ToList();
        public static List<string> AndroidBrowserCaptchaEnabledVersions { get { return _androidBrowserCaptchaEnabledVersions; } }

        private readonly static string _adDomainName = ConfigurationManager.AppSettings["adSlotDomain"] ?? string.Empty;
        public static string AdDomainName { get { return _adDomainName; } }

        private readonly static int _renaultLeadFormModelId = Convert.ToInt32(ConfigurationManager.AppSettings["RenaultLeadFormModelId"] ?? string.Empty);
        public static int RenaultLeadFormModelId { get { return _renaultLeadFormModelId; } }

        private static readonly Array _abTestRangeForNCF = (ConfigurationManager.AppSettings["AbTestRangeForNCF"] ?? "1,19").ConvertStringToArray();
        public static Array AbTestRangeForNCF { get { return _abTestRangeForNCF; } }

        public static readonly ushort newCarApiModelCount = Convert.ToUInt16(ConfigurationManager.AppSettings["NewCarApiModelCount"] ?? "3");
        public static readonly string orpText = ConfigurationManager.AppSettings["ShowPriceInCityText"] ?? "Show price in my city";
        public static readonly string tollFreeNumber = ConfigurationManager.AppSettings["CallSlugNumber"] ?? "18002090230";
        public static readonly string ViewPriceBreakupText = ConfigurationManager.AppSettings["ViewPriceBreakupText"] ?? "View Price Breakup";
        private static readonly Array _abTestValueForClassifiedMyListingsRename = (ConfigurationManager.AppSettings["AbTestValueForClassifiedMyListingsRename"] ?? "1,50").ConvertStringToArray();
        public static Array AbTestValueForClassifiedMyListingsRename { get { return _abTestValueForClassifiedMyListingsRename; } }
        public static readonly List<int> _descriptionShowingModels = (ConfigurationManager.AppSettings["SpecsDescriptionModels"] ?? "852,930,1078").ConvertStringToList<int>();
        public static List<int> DescriptionShowingModels { get { return _descriptionShowingModels; } }
        public static readonly List<int> _descriptionShowingItems = (ConfigurationManager.AppSettings["SpecsDescriptionItems"] ?? "14,15,17,26").ConvertStringToList<int>();
        public static List<int> DescriptionShowingItems { get { return _descriptionShowingItems; } }
        public static string CMSModule { get { return ConfigurationManager.AppSettings["EditCMSModuleName"] ?? string.Empty; } }

        public static readonly int HideGetEmiAssistanceAbTestMinValue = Convert.ToInt32(ConfigurationManager.AppSettings["HideGetEmiAssistanceAbTestMinValue"] ?? "0");
        public static readonly int HideGetEmiAssistanceAbTestMaxValue = Convert.ToInt32(ConfigurationManager.AppSettings["HideGetEmiAssistanceAbTestMaxValue"] ?? "0");
        public static readonly int HideCustomiseEmiAbTestMinValue = Convert.ToInt32(ConfigurationManager.AppSettings["HideCustomiseEmiAbTestMinValue"] ?? "0");
        public static readonly int HideCustomiseEmiAbTestMaxValue = Convert.ToInt32(ConfigurationManager.AppSettings["HideCustomiseEmiAbTestMaxValue"] ?? "0");
        public static readonly int MahindraMozarroModelid = Convert.ToInt32(ConfigurationManager.AppSettings["MahindraMozarroModelid"] ?? "0");
        public static readonly int MahindraAlturasModelid = Convert.ToInt32(ConfigurationManager.AppSettings["MahindraAlturasModelid"] ?? "0");
        public static readonly int MahindraAlturasCampaignid = Convert.ToInt32(ConfigurationManager.AppSettings["MahindraAlturasCampaignid"] ?? "0");
        public static readonly string HostUrl = ConfigurationManager.AppSettings["HostUrl"] ?? "www.carwale.com";
        public static readonly int ReviewExpiryTime = Convert.ToInt32(ConfigurationManager.AppSettings["ReviewExpiryTime"] ?? "14400");
        public static readonly Array TurboMLACityIds = (ConfigurationManager.AppSettings["turboMLACityIds"] ?? "").ConvertStringToArray();
        public static readonly Array TurboMLAMakeIds = (ConfigurationManager.AppSettings["turboMLAMakeIds"] ?? "").ConvertStringToArray();
        public static readonly int TurboMLAMinABTests = CustomParser.parseIntObject(ConfigurationManager.AppSettings["turboMLAMinABTests"] ?? "");
    }
}
