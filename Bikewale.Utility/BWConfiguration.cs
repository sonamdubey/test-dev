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
    /// </summary>
    public sealed class BWConfiguration
    {
        private static BWConfiguration _instance = null;
        private static readonly object padlock = new object();

        private int nonReadOnlyStatic = 0;

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
            _LeadConsumerQueue = String.Empty;

        private readonly bool _IsAppTrackDayVisible = false;
        private readonly int _SecurityHashLength = 0;

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
            _bikeWaleLogo = "https://imgd1.aeplcdn.com/0x0/bw/static/design15/mailer-images/bw-logo.png";
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

    }   // class
}   // namespace