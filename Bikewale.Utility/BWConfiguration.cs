using System;
using System.Configuration;

namespace Bikewale.Utility
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 2 Nov 2015
    /// Summary : Class to read the web configuration data. Singleton class.
    /// Modified by :   Sumit Kate on 31 Mar 2016
    /// Summary :   Added new key in Web Config for Google Map API Key GoogleMapApiKey
    /// Modified by :   Vivek Gupta on 9-5-2016
    /// Summary :   Added new key in Web Config for using grpc UseGrpc
    /// Modified By : Lucky Rathore on 09 May 2016.
    /// Description : Added bikeWaleLogo for bikewale logo image URL.
    /// Modified by :   Sumit Kate on 11 Nov 2016
    /// Description :   Added keys for AWSAccessKey, AWSBucketName, AWSSecretKey, AWSEnvironment, AWSImageQueueName, SecurityHashLength web config
    /// Modified By :   Vishnu Teja Yalakuntla on 15 Sep 2017
    /// Summary     :   Added BestBikesMakes property
    /// Modified by :   Sumit Kate on 28 Sep 2017
    /// Description :   Added InnovationBannerModels
    /// Modified by : Ashutosh Sharma on 13 Oct 2017
    /// Description : Added SynopsisSummaryMergedMakeIds
    /// Modified by : Ashutosh Sharma on 30 Nov 2017
    /// Description : Removed SynopsisSummaryMergedMakeIds
    /// </summary>
    public sealed class BWConfiguration
    {
        private static BWConfiguration _instance = null;
        private static readonly object padlock = new object();

        private int nonReadOnlyStatic = 0;
        private readonly int _pwaLocalCahceLimit = 100;

        private readonly string _defaultCity = String.Empty,
            _bwconnectionString = String.Empty,
            _cwconnectionString = String.Empty,
            _offerUniqueTransaction = String.Empty,
            _billDeskWorkingKey = String.Empty,
            _imgHostURL = String.Empty,
            _RabbitImgHostURL = String.Empty,
            _ImgPathFolder = String.Empty,
            _SourceId = String.Empty,
            _MobileSourceId = String.Empty,
            _DefaultCity = String.Empty,
            _DefaultName = String.Empty,
            _IsMSMQ = String.Empty,
            _SendSMS = String.Empty,
            _AppPath = String.Empty,
            _IsMemcachedUsed = String.Empty,
            _MemcacheTimespan = String.Empty,
            _StaticUrl = String.Empty,
            _AutoExpo = String.Empty,
            _SMTPSERVER = String.Empty,
            _ErrorMailTo = String.Empty,
            _LocalMail = String.Empty,
            _ReplyTo = String.Empty,
            _MailFrom = String.Empty,
            _ApplicationName = String.Empty,
            _SendError = String.Empty,
            _CwApiHostUrl = String.Empty,
            _ABApiHostUrl = String.Empty,
            _BwHostUrl = String.Empty,
            _BwHostUrlForJs = String.Empty,
            _ApplicationId = String.Empty,
            _FeedbackEmailTo = String.Empty,
            _OfferClaimAlertEmail = String.Empty,
            _ImageQueueName = String.Empty,
            _ElasticHostUrl = String.Empty,
            _MMindexName = String.Empty,
            _PQindexName = String.Empty,
            _CityIndexName = String.Empty,
            _AutoSuggestType = String.Empty,
            _PageSize = String.Empty,
            _StaticFileVersion = String.Empty,
            _StaticFrameworkFileVersion = String.Empty,
            _StaticPluginFileVersion = String.Empty,
            _StaticCommonFileVersion = String.Empty,
            _apiRequestTypeJSON = String.Empty,
            _BWPrioritySmsQueue = String.Empty,
            _BWSmsQueue = string.Empty,
            _BWDataSynchQueue = string.Empty,
            _GoogleApiKey = string.Empty,
            _GoogleMapApiKey = String.Empty,
            _apiMaxWaitTime = string.Empty,
            _useGrpc = string.Empty,
            _bikeWaleLogo = string.Empty,
            _CWSAPiHostUrl = string.Empty,
            _WebsiteDomain = string.Empty,
            _grpcChannelWaitTime = string.Empty,
            _grpcArticleServerList = string.Empty,
            _grpcRetryCount = string.Empty,
            _logGrpcErrors = string.Empty,
            _AWSAccessKey = String.Empty,
            _AWSBucketName = String.Empty,
            _AWSSecretKey = String.Empty,
            _AWSEnvironment = string.Empty,
            _AWSImageQueueName = String.Empty,
            _SponsoredModelId = string.Empty,
            _AndroidGlobalTopic = string.Empty,
            _FCMSendURL = string.Empty,
            _FCMApiKey = string.Empty,
            _FCMSusbscribeUserUrl = string.Empty,
            _FCMUnSusbscribeUserUrl = string.Empty,
            _FCMEnvironment = String.Empty,
            _LeadConsumerQueue = String.Empty,
            _PinCodesIndexName = string.Empty,
            _DebugMobileSite = string.Empty,
            _CoverFoxLink = string.Empty,
            _UserReviewIndexName = string.Empty,
            _OtherBikesInMakeId = string.Empty,
            _CapitalFirstPincodeIndex = string.Empty,
            _InnovationBannerModels = String.Empty,
            _UserReviewsReadInSessionCount = string.Empty,
            _PopularCitiesId = String.Empty,
            _PopularityOrderForMake = string.Empty,
            _AmpProjectUrl = string.Empty,
            _MetasMakeId = string.Empty;

        private readonly bool _IsAppTrackDayVisible = false, _UseAPIGateway = false;
        private readonly int _SecurityHashLength = 0;
        private readonly string _AWSS3Region = String.Empty;
        private readonly uint _GrpcMaxTimeLimit = 100;
        private readonly uint _GrpcPoolSize = 1;
        private readonly uint _KawasakiCampaignId;
        private readonly bool _EnablePWALogging = false;
        private readonly string _StaticUrlPWA;
        private readonly string _StaticAppFileVersion;
        private readonly string _StaticVendorFileVersion;
        private readonly string _ServiceWorkerPath;
        private readonly string _StaticCSSBTFPWAVersion;
        private readonly bool _EnablePWA;
        private readonly string _AMPDomainForSW;
        private readonly bool _logNewsUrl;
        private readonly string _capitalFirstConsumerQueue;
        private readonly bool _IsIPSecurityEnabled;
        private readonly uint _CapitalFirstDealerId;
        private readonly ushort _MakePageOnRoadPriceBtnPct;
        private readonly string _CarTradeLeadUrl, _CarTradeLeadApiAction, _CarTradeLeadApiCode;
        private readonly string _EditCMSModuleName;
        private readonly int _minEnginePoolSize, _maxEnginePoolSize;
        private readonly bool _useV8Engine;
        private readonly int _pwaRenderedHtmlCacheLimitMins;
        private readonly bool _EnablePwdResetLogging;

        // Private constructor, so no outsiders have access.
        private BWConfiguration()
        {
            // Initialize _data member here
            _defaultCity = ConfigurationManager.AppSettings["defaultName"];
            _bwconnectionString = ConfigurationManager.AppSettings["mySqlBwConnectionString"];
            _cwconnectionString = ConfigurationManager.AppSettings["mySqlConnectionString"];
            _offerUniqueTransaction = ConfigurationManager.AppSettings["OfferUniqueTransaction"];
            _billDeskWorkingKey = ConfigurationManager.AppSettings["BillDeskWorkingKey"];
            _imgHostURL = ConfigurationManager.AppSettings["imgHostURL"];
            _RabbitImgHostURL = ConfigurationManager.AppSettings["RabbitImgHostURL"];
            _ImgPathFolder = ConfigurationManager.AppSettings["imgPathFolder"];
            _SourceId = ConfigurationManager.AppSettings["sourceId"];
            _MobileSourceId = ConfigurationManager.AppSettings["mobileSourceId"];
            _DefaultCity = ConfigurationManager.AppSettings["DefaultCity"];
            _DefaultName = ConfigurationManager.AppSettings["defaultName"];
            _IsMSMQ = ConfigurationManager.AppSettings["isMSMQ"];
            _SendSMS = ConfigurationManager.AppSettings["SendSMS"];
            _AppPath = ConfigurationManager.AppSettings["AppPath"];
            _IsMemcachedUsed = ConfigurationManager.AppSettings["IsMemcachedUsed"];
            _MemcacheTimespan = ConfigurationManager.AppSettings["MemcacheTimespan"];
            _StaticUrl = ConfigurationManager.AppSettings["staticUrl"];
            _AutoExpo = ConfigurationManager.AppSettings["AutoExpo"];
            _SMTPSERVER = ConfigurationManager.AppSettings["SMTPSERVER"];
            _ErrorMailTo = ConfigurationManager.AppSettings["errorMailTo"];
            _LocalMail = ConfigurationManager.AppSettings["localMail"];
            _ReplyTo = ConfigurationManager.AppSettings["ReplyTo"];
            _MailFrom = ConfigurationManager.AppSettings["MailFrom"];
            _ApplicationName = ConfigurationManager.AppSettings["ApplicationName"];
            _SendError = ConfigurationManager.AppSettings["sendError"];
            _CwApiHostUrl = ConfigurationManager.AppSettings["cwApiHostUrl"];
            _ABApiHostUrl = ConfigurationManager.AppSettings["ABApiHostUrl"];
            _BwHostUrl = ConfigurationManager.AppSettings["bwHostUrl"];
            _BwHostUrlForJs = ConfigurationManager.AppSettings["bwHostUrlForJs"];
            _ApplicationId = ConfigurationManager.AppSettings["applicationId"];
            _FeedbackEmailTo = ConfigurationManager.AppSettings["feedbackEmailTo"];
            _OfferClaimAlertEmail = ConfigurationManager.AppSettings["OfferClaimAlertEmail"];
            _ImageQueueName = ConfigurationManager.AppSettings["ImageQueueName"];
            _ElasticHostUrl = ConfigurationManager.AppSettings["ElasticHostUrl"];
            _MMindexName = ConfigurationManager.AppSettings["MMindexName"];
            _PQindexName = ConfigurationManager.AppSettings["PQindexName"];
            _CityIndexName = ConfigurationManager.AppSettings["cityIndexName"];
            _AutoSuggestType = ConfigurationManager.AppSettings["AutoSuggestType"];
            _PageSize = ConfigurationManager.AppSettings["PageSize"];
            _StaticFileVersion = ConfigurationManager.AppSettings["staticFileVersion"];
            _StaticFrameworkFileVersion = ConfigurationManager.AppSettings["staticFrameworkFileVersion"];
            _StaticPluginFileVersion = ConfigurationManager.AppSettings["staticPluginFileVersion"];
            _StaticCommonFileVersion = ConfigurationManager.AppSettings["staticCommonFileVersion"];
            _apiRequestTypeJSON = "application/json";
            _BWPrioritySmsQueue = ConfigurationManager.AppSettings["PrioritySmsQueue"];
            _BWSmsQueue = ConfigurationManager.AppSettings["SmsQueue"];
            _BWDataSynchQueue = ConfigurationManager.AppSettings["BWDataSynchQueue"];
            _GoogleApiKey = ConfigurationManager.AppSettings["APIKey"];
            _GoogleMapApiKey = ConfigurationManager.AppSettings["GoogleMapApiKey"];
            _apiMaxWaitTime = ConfigurationManager.AppSettings["ApiMaxWaitTime"];
            _useGrpc = ConfigurationManager.AppSettings["UseGrpc"];
            _bikeWaleLogo = "https://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/bw-logo.png";
            _CWSAPiHostUrl = ConfigurationManager.AppSettings["CwWebServiceHostUrl"];
            _WebsiteDomain = ConfigurationManager.AppSettings["WebsiteDomain"];
            _grpcChannelWaitTime = ConfigurationManager.AppSettings["GrpcChannelWaitTime"];
            _grpcArticleServerList = ConfigurationManager.AppSettings["GrpcArticleServerList"];
            _grpcRetryCount = ConfigurationManager.AppSettings["GrpcRetryCount"];
            _logGrpcErrors = ConfigurationManager.AppSettings["LogGrpcErrors"];
            _AWSAccessKey = ConfigurationManager.AppSettings["AWSAccessKey"];
            _AWSBucketName = ConfigurationManager.AppSettings["AWSBucketName"];
            _AWSSecretKey = ConfigurationManager.AppSettings["AWSSecretKey"];
            _AWSEnvironment = ConfigurationManager.AppSettings["AWSEnvironment"];
            _AWSImageQueueName = ConfigurationManager.AppSettings["AWSImageQueueName"];
            _SponsoredModelId = ConfigurationManager.AppSettings["SponsoredModelIds"];
            _SecurityHashLength = Convert.ToInt32(ConfigurationManager.AppSettings["SecurityHashLength"]);
            _AndroidGlobalTopic = ConfigurationManager.AppSettings["FCMAndroidGlobalTopic"];
            _FCMSendURL = ConfigurationManager.AppSettings["FCMSendURL"];
            _FCMApiKey = ConfigurationManager.AppSettings["FCMApiKey"];
            _FCMSusbscribeUserUrl = ConfigurationManager.AppSettings["FCMIIDBatchAddEndPoint"];
            _FCMUnSusbscribeUserUrl = ConfigurationManager.AppSettings["FCMIIDBatchRemoveEndPoint"];
            _FCMEnvironment = ConfigurationManager.AppSettings["FCMEnvironment"];
            _IsAppTrackDayVisible = !String.IsNullOrEmpty(ConfigurationManager.AppSettings["IsAppTrackDayVisible"]) ? Convert.ToBoolean(ConfigurationManager.AppSettings["IsAppTrackDayVisible"]) : false;
            _LeadConsumerQueue = Convert.ToString(ConfigurationManager.AppSettings["LeadConsumerQueue"]);
            _PinCodesIndexName = Convert.ToString(ConfigurationManager.AppSettings["PinCodesIndexName"]);
            _AWSS3Region = Convert.ToString(ConfigurationManager.AppSettings["AWSS3Region"]);
            _DebugMobileSite = Convert.ToString(ConfigurationManager.AppSettings["DebugMobileSite"]);
            _GrpcMaxTimeLimit = Convert.ToUInt32(ConfigurationManager.AppSettings["GrpcMaxTimeLimit"]);
            _GrpcPoolSize = Convert.ToUInt32(ConfigurationManager.AppSettings["GrpcPoolSize"] ?? "1");
            _CoverFoxLink = Convert.ToString(ConfigurationManager.AppSettings["CoverFox"]);
            _pwaLocalCahceLimit = string.IsNullOrEmpty(ConfigurationManager.AppSettings["PwaLocalCahceLimit"]) ? 100 : Convert.ToInt32(ConfigurationManager.AppSettings["PwaLocalCahceLimit"]);
            _UseAPIGateway = !String.IsNullOrEmpty(ConfigurationManager.AppSettings["UseAPIGateway"]) ? Convert.ToBoolean(ConfigurationManager.AppSettings["UseAPIGateway"]) : false;
            _EnablePWALogging = string.IsNullOrEmpty(ConfigurationManager.AppSettings["EnablePWALogging"]) ? false : Convert.ToBoolean(ConfigurationManager.AppSettings["EnablePWALogging"]);
            _StaticUrlPWA = ConfigurationManager.AppSettings["StaticUrlPWA"];
            _StaticAppFileVersion = ConfigurationManager.AppSettings["StaticAppFileVersion"];
            _StaticVendorFileVersion = ConfigurationManager.AppSettings["StaticVendorFileVersion"];
            _ServiceWorkerPath = ConfigurationManager.AppSettings["ServiceWorkerPath"];
            _EnablePWA = string.IsNullOrEmpty(ConfigurationManager.AppSettings["EnablePWA"]) ? false : Convert.ToBoolean(ConfigurationManager.AppSettings["EnablePWA"]);
            _StaticCSSBTFPWAVersion = ConfigurationManager.AppSettings["StaticCSSBTFPWAVersion"];
            _UserReviewIndexName = ConfigurationManager.AppSettings["UserReviewIndexName"];
            _CapitalFirstPincodeIndex = ConfigurationManager.AppSettings["CapitalFirstPincodeIndex"];
            _AMPDomainForSW = ConfigurationManager.AppSettings["AMPDomainForSW"];
            _logNewsUrl = string.IsNullOrEmpty(ConfigurationManager.AppSettings["LogNewsUrl"]) ? false : Convert.ToBoolean(ConfigurationManager.AppSettings["LogNewsUrl"]);
            _capitalFirstConsumerQueue = Convert.ToString(ConfigurationManager.AppSettings["CapitalFirstConsumerQueue"]);
            _IsIPSecurityEnabled = string.IsNullOrEmpty(ConfigurationManager.AppSettings["IsIPSecurityEnabled"]) ? false : Convert.ToBoolean(ConfigurationManager.AppSettings["IsIPSecurityEnabled"]);
            _OtherBikesInMakeId = ConfigurationManager.AppSettings["OtherBikesInMakeId"];
            _CapitalFirstDealerId = Convert.ToUInt32(ConfigurationManager.AppSettings["CapitalFirstDealerId"]);
            _CarTradeLeadUrl = Convert.ToString(ConfigurationManager.AppSettings["CarTradeLeadUrl"]);
            _CarTradeLeadApiAction = Convert.ToString(ConfigurationManager.AppSettings["CarTradeLeadApiAction"]);
            _CarTradeLeadApiCode = Convert.ToString(ConfigurationManager.AppSettings["CarTradeLeadApiCode"]);
            _InnovationBannerModels = Convert.ToString(ConfigurationManager.AppSettings["InnovationBannerModels"]);
            _UserReviewsReadInSessionCount = Convert.ToString(ConfigurationManager.AppSettings["UserReviewsReadInSessionCount"]);
            _KawasakiCampaignId = Convert.ToUInt32(ConfigurationManager.AppSettings["KawasakiCampaignId"]);
            _PopularCitiesId = ConfigurationManager.AppSettings["PopularCitiesId"];
            _EditCMSModuleName = Convert.ToString(ConfigurationManager.AppSettings["EditCMSModuleName"]);
            _minEnginePoolSize = string.IsNullOrEmpty(ConfigurationManager.AppSettings["MinEnginePoolSize"]) ? 3 : Convert.ToInt32(ConfigurationManager.AppSettings["MinEnginePoolSize"]);
            _maxEnginePoolSize = string.IsNullOrEmpty(ConfigurationManager.AppSettings["MaxEnginePoolSize"]) ? 15 : Convert.ToInt32(ConfigurationManager.AppSettings["MaxEnginePoolSize"]);
            _PopularityOrderForMake = ConfigurationManager.AppSettings["PopularityOrderForMake"];
            _AmpProjectUrl = ConfigurationManager.AppSettings["AMPProjectUrl"];
            _useV8Engine = string.IsNullOrEmpty(ConfigurationManager.AppSettings["UseV8Engine"]) ? true : Convert.ToBoolean(ConfigurationManager.AppSettings["UseV8Engine"]);
            _pwaRenderedHtmlCacheLimitMins = string.IsNullOrEmpty(ConfigurationManager.AppSettings["UseV8Engine"]) ? 60 : Convert.ToInt32(ConfigurationManager.AppSettings["PwaRenderedHtmlCacheLimitMins"]);
            _MetasMakeId = ConfigurationManager.AppSettings["MetasMakeId"];
            _EnablePwdResetLogging = String.IsNullOrEmpty(ConfigurationManager.AppSettings["EnablePwdResetLogging"]) ? false : Convert.ToBoolean(ConfigurationManager.AppSettings["EnablePwdResetLogging"]);
            _pwaRenderedHtmlCacheLimitMins = string.IsNullOrEmpty(ConfigurationManager.AppSettings["UseV8Engine"]) ? 60 : Convert.ToInt32(ConfigurationManager.AppSettings["PwaRenderedHtmlCacheLimitMins"]);
            _MakePageOnRoadPriceBtnPct = string.IsNullOrEmpty(ConfigurationManager.AppSettings["MakePageOnRoadPriceBtnPct"]) ? (ushort)0 : Convert.ToUInt16(ConfigurationManager.AppSettings["MakePageOnRoadPriceBtnPct"]);
        }

        // Static method to provide access to instance
        public static BWConfiguration Instance
        {
            get
            {
                if (_instance == null)
                {
                    // Take lock while creating the object
                    lock (padlock)
                    {
                        if (_instance == null)
                        {
                            _instance = new BWConfiguration();
                        }
                    }
                }
                return _instance;
            }
        }

        public string BikeWaleLogo { get { return _bikeWaleLogo; } }

        public int NonReadOnlyStatic { get { return nonReadOnlyStatic++; } }

        /// <summary>
        /// Get the default city name.
        /// </summary>
        public string GetDefaultCityName
        {
            get { return _defaultCity; }
        }

        /// <summary>
        /// Get the bikewale database connection string
        /// </summary>
        public string BWConnectionString
        {
            get { return _bwconnectionString; }
        }

        /// <summary>
        /// Get the carwale database connection string
        /// </summary>
        public string CWConnectionString
        {
            get { return _cwconnectionString; }
        }

        public string OfferUniqueTransaction
        {
            get { return _offerUniqueTransaction; }
        }

        public string BillDeskWorkingKey
        {
            get { return _billDeskWorkingKey; }
        }
        public string ImgHostURL { get { return _imgHostURL; } }

        public string RabbitImgHostURL { get { return _RabbitImgHostURL; } }
        public string ImgPathFolder { get { return _ImgPathFolder; } }
        public string SourceId { get { return _SourceId; } }
        public string MobileSourceId { get { return _MobileSourceId; } }
        public string DefaultCity { get { return _DefaultCity; } }
        public string DefaultName { get { return _DefaultName; } }
        public string IsMSMQ { get { return _IsMSMQ; } }
        public string SendSMS { get { return _SendSMS; } }
        public string AppPath { get { return _AppPath; } }
        public string IsMemcachedUsed { get { return _IsMemcachedUsed; } }
        public string MemcacheTimespan { get { return _MemcacheTimespan; } }
        public string StaticUrl { get { return _StaticUrl; } }
        public string AutoExpo { get { return _AutoExpo; } }
        public string SMTPSERVER { get { return _SMTPSERVER; } }
        public string ErrorMailTo { get { return _ErrorMailTo; } }
        public string LocalMail { get { return _LocalMail; } }
        public string ReplyTo { get { return _ReplyTo; } }
        public string MailFrom { get { return _MailFrom; } }
        public string ApplicationName { get { return _ApplicationName; } }
        public string SendError { get { return _SendError; } }
        public string CwApiHostUrl { get { return _CwApiHostUrl; } }
        public string ABApiHostUrl { get { return _ABApiHostUrl; } }
        public string BwHostUrl { get { return _BwHostUrl; } }
        public string BwHostUrlForJs { get { return _BwHostUrlForJs; } }
        public string ApplicationId { get { return _ApplicationId; } }
        public string FeedbackEmailTo { get { return _FeedbackEmailTo; } }
        public string OfferClaimAlertEmail { get { return _OfferClaimAlertEmail; } }
        public string ImageQueueName { get { return _ImageQueueName; } }
        public string ElasticHostUrl { get { return _ElasticHostUrl; } }
        public string MMindexName { get { return _MMindexName; } }
        public string PQindexName { get { return _PQindexName; } }
        public string CityIndexName { get { return _CityIndexName; } }
        public string AutoSuggestType { get { return _AutoSuggestType; } }
        public string PageSize { get { return _PageSize; } }
        public string StaticFileVersion { get { return _StaticFileVersion; } }
        public string StaticFrameworkFileVersion { get { return _StaticFrameworkFileVersion; } }
        public string StaticPluginFileVersion { get { return _StaticPluginFileVersion; } }
        public string StaticCommonFileVersion { get { return _StaticCommonFileVersion; } }
        public string APIRequestTypeJSON { get { return _apiRequestTypeJSON; } }
        public string BWPrioritySmsQueue { get { return _BWPrioritySmsQueue; } }
        public string BWSmsQueue { get { return _BWSmsQueue; } }
        public string BWDataSynchQueue { get { return _BWDataSynchQueue; } }
        public string GoogleApiKey { get { return _GoogleApiKey; } }
        public string GoogleMapApiKey { get { return _GoogleMapApiKey; } }
        public int ApiMaxWaitTime { get { return string.IsNullOrEmpty(_apiMaxWaitTime) ? 0 : Convert.ToInt32(_apiMaxWaitTime); } }
        public string UseGrpc { get { return _useGrpc; } }
        public string CWSApiHostUrl { get { return _CWSAPiHostUrl; } }
        public string WebsiteDomain { get { return _WebsiteDomain; } }
        public string GrpcChannelWaitTime { get { return _grpcChannelWaitTime; } }
        public string GrpcArticleServerList { get { return _grpcArticleServerList; } }
        public string GrpcRetryCount { get { return _grpcRetryCount; } }
        public string LogGrpcErrors { get { return _logGrpcErrors; } }
        public string AWSAccessKey { get { return _AWSAccessKey; } }
        public string AWSBucketName { get { return _AWSBucketName; } }
        public string AWSSecretKey { get { return _AWSSecretKey; } }
        public string AWSEnvironment { get { return _AWSEnvironment; } }
        public string AWSImageQueueName { get { return _AWSImageQueueName; } }
        public int SecurityHashLength { get { return _SecurityHashLength; } }
        public string SponsoredModelId { get { return _SponsoredModelId; } }
        public string AndroidGlobalTopic { get { return _AndroidGlobalTopic; } }
        public string FCMSendURL { get { return _FCMSendURL; } }
        public string FCMApiKey { get { return _FCMApiKey; } }
        public string FCMSusbscribeUserUrl { get { return _FCMSusbscribeUserUrl; } }
        public string FCMUnSusbscribeUserUrl { get { return _FCMUnSusbscribeUserUrl; } }
        public string FCMEnvironment { get { return _FCMEnvironment; } }
        public bool IsAppTrackDayVisible { get { return _IsAppTrackDayVisible; } }
        public String LeadConsumerQueue { get { return _LeadConsumerQueue; } }
        public string PinCodesIndexName { get { return _PinCodesIndexName; } }
        public String AWSS3Region { get { return _AWSS3Region; } }
        public bool DebugMobileSite { get { return string.IsNullOrEmpty(_DebugMobileSite) ? false : Convert.ToBoolean(_DebugMobileSite); } }
        public uint GrpcMaxTimeLimit { get { return _GrpcMaxTimeLimit; } }
        public uint GrpcPoolSize { get { return _GrpcPoolSize; } }
        public string CoverFoxLink { get { return _CoverFoxLink; } }

        public bool UseAPIGateway { get { return _UseAPIGateway; } }
        public int PwaLocalCahceLimit { get { return _pwaLocalCahceLimit; } }
        public bool EnablePWALogging { get { return _EnablePWALogging; } }

        public string StaticUrlPWA { get { return _StaticUrlPWA; } }
        public string StaticAppFileVersion { get { return _StaticAppFileVersion; } }
        public string StaticVendorFileVersion { get { return _StaticVendorFileVersion; } }
        public string ServiceWorkerPath { get { return _ServiceWorkerPath; } }
        public string StaticCSSBTFPWAVersion { get { return _StaticCSSBTFPWAVersion; } }
        public bool EnablePWA { get { return _EnablePWA; } }
        public string AMPDomainForSW { get { return _AMPDomainForSW; } }

        public string UserReviewIndexName { get { return _UserReviewIndexName; } }

        public bool LogNewsUrl { get { return _logNewsUrl; } }

        public string OtherBikesInMakeId { get { return _OtherBikesInMakeId; } }
        public string CapitalFirstConsumerQueue { get { return _capitalFirstConsumerQueue; } }
        public bool IsIPSecurityEnabled { get { return _IsIPSecurityEnabled; } }
        public uint CapitalFirstDealerId { get { return _CapitalFirstDealerId; } }
        public string CarTradeLeadUrl { get { return _CarTradeLeadUrl; } }
        public string CarTradeLeadApiAction { get { return _CarTradeLeadApiAction; } }
        public string CarTradeLeadApiCode { get { return _CarTradeLeadApiCode; } }

        public string CapitalFirstPinCode { get { return _CapitalFirstPincodeIndex; } }
        public string InnovationBannerModels { get { return _InnovationBannerModels; } }
        public string UserReviewsReadInSessionCount { get { return _UserReviewsReadInSessionCount; } }
        public uint KawasakiCampaignId { get { return _KawasakiCampaignId; } }
        public string PopularCitiesId { get { return _PopularCitiesId; } }
        public string EditCMSModuleName { get { return _EditCMSModuleName; } }
        public int MaxEnginePoolSize { get { return _maxEnginePoolSize; } }
        public int MinEnginePoolSize { get { return _minEnginePoolSize; } }
        public bool UseV8Engine { get { return _useV8Engine; } }
        public int PwaRenderedHtmlCacheLimitMins { get { return _pwaRenderedHtmlCacheLimitMins; } }
        public string PopularityOrderForMake { get { return _PopularityOrderForMake; } }
        public string AMPProjectUrl { get { return _AmpProjectUrl; } }
        public string MetasMakeId { get { return _MetasMakeId; } }
        public bool EnablePwdResetLogging { get { return _EnablePwdResetLogging; } }
        public ushort MakePageOnRoadPriceBtnPct { get { return _MakePageOnRoadPriceBtnPct;  } }
    }   // class
}   // namespace