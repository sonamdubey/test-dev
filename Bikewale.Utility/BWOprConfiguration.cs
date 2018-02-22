using System;
using System.Configuration;

namespace Bikewale.Utility
{
    /// <summary>
    /// Created By : Sajal Gupta on 23-12-2016
    /// Summary : Class to read the web configuration data. Singleton class.
    /// </summary>
    public sealed class BWOprConfiguration
    {
        private static BWOprConfiguration _instance = null;
        private static readonly object padlock = new object();

        private int nonReadOnlyStatic = 0;

        private readonly string _defaultCity = String.Empty,
            _ReadOnlyConnectionString = String.Empty,
            _MasterConnectionString = String.Empty,
            _imgHostURL = String.Empty,
            _RabbitImgHostURL = String.Empty,
            _ImgPathFolder = String.Empty,
            _redirectUrl = String.Empty,
            _bikewaleUrl = String.Empty,
            _isProduction = String.Empty,
            _price_a_1 = String.Empty,
            _price_a_2 = String.Empty,
            _price_a_3 = String.Empty,
            _price_b_1 = String.Empty,
            _price_b_2 = String.Empty,
            _price_b_3 = String.Empty,
            _price_d_0_5 = String.Empty,
            _price_d_1 = String.Empty,
            _price_d_2 = String.Empty,
            _price_d_3 = String.Empty,
            _price_d_4 = String.Empty,
            _price_d_5 = String.Empty,
            _price_l_1 = String.Empty,
            _price_l_2 = String.Empty,
            _price_l_3 = String.Empty,
            _ServiceTax = String.Empty,
            _DefaultName = String.Empty,
            _IsMemcachedUsed = String.Empty,
            _MemcacheTimespan = String.Empty,
            _SMTPSERVER = String.Empty,
            _ErrorMailTo = String.Empty,
            _LocalMail = String.Empty,
            _ReplyTo = String.Empty,
            _MailFrom = String.Empty,
            _ApplicationName = String.Empty,
            _SendError = String.Empty,
            _BWDataSynchQueue = String.Empty,
            _BwHostUrlForJs = String.Empty,
            _BwOprHostUrlForJs = String.Empty,
            _ABApiHostUrl = String.Empty,
            _aspnet_UseTaskFriendlySynchronizationContext = String.Empty,
            _ImageQueueName = String.Empty,
            _CwOprHostUrl = String.Empty,
            _GoogleDistanceMatrixAPIKey = String.Empty,
            _areaMaxPerCall = String.Empty,
            _CwWebServiceHostUrl = String.Empty,
            _SmsQueue = String.Empty,
            _AWSAccessKey = String.Empty,
            _AWSBucketName = String.Empty,
            _AWSSecretKey = String.Empty,
            _AWSEnvironment = string.Empty,
            _AWSImageQueueName = String.Empty,
            _notificationUserId = String.Empty,
            _notificationOtherUserId = String.Empty,
            _notificationToUserMailId = String.Empty,
            _notificationCCUserMailId = String.Empty,
            _EmailsForBikeChange = String.Empty,
            _BwHostUrl = String.Empty,
            _notificationToMailIdForPageMetas = string.Empty,
            _BWEsIndexUpdaterQueue = string.Empty;


        private readonly bool _CORSEnabled = false, _IsAppTrackDayVisible = false;
        private readonly int _SecurityHashLength = 0, _UnitSoldDataNotificationDay;
        private string _AWSS3Region = String.Empty;
        // Private constructor, so no outsiders have access.
        private BWOprConfiguration()
        {
            // Initialize _data member here            
            _ReadOnlyConnectionString = ConfigurationManager.AppSettings["ReadOnlyConnectionString"];
            _MasterConnectionString = ConfigurationManager.AppSettings["MasterConnectionString"];
            _imgHostURL = ConfigurationManager.AppSettings["imgHostURL"];
            _RabbitImgHostURL = ConfigurationManager.AppSettings["RabbitImgHostURL"];
            _ImgPathFolder = ConfigurationManager.AppSettings["imgPathFolder"];
            _redirectUrl = ConfigurationManager.AppSettings["redirectUrl"];
            _bikewaleUrl = ConfigurationManager.AppSettings["bikewaleUrl"];
            _isProduction = ConfigurationManager.AppSettings["isProduction"];
            _defaultCity = ConfigurationManager.AppSettings["defaultName"];
            _price_a_1 = ConfigurationManager.AppSettings["A:1"];
            _price_a_2 = ConfigurationManager.AppSettings["A:2"];
            _price_a_3 = ConfigurationManager.AppSettings["A:3"];
            _price_b_1 = ConfigurationManager.AppSettings["B:3"];
            _price_b_2 = ConfigurationManager.AppSettings["B:3"];
            _price_b_3 = ConfigurationManager.AppSettings["B:3"];
            _price_d_0_5 = ConfigurationManager.AppSettings["d_.5"];
            _price_d_1 = ConfigurationManager.AppSettings["d_1"];
            _price_d_2 = ConfigurationManager.AppSettings["d_2"];
            _price_d_3 = ConfigurationManager.AppSettings["d_3"];
            _price_d_4 = ConfigurationManager.AppSettings["d_4"];
            _price_d_5 = ConfigurationManager.AppSettings["d_5"];
            _price_l_1 = ConfigurationManager.AppSettings["L:1"];
            _price_l_2 = ConfigurationManager.AppSettings["L:2"];
            _price_l_3 = ConfigurationManager.AppSettings["L:3"];
            _ServiceTax = ConfigurationManager.AppSettings["ServiceTax"];
            _DefaultName = ConfigurationManager.AppSettings["defaultName"];
            _IsMemcachedUsed = ConfigurationManager.AppSettings["IsMemcachedUsed"];
            _MemcacheTimespan = ConfigurationManager.AppSettings["MemcacheTimespan"];
            _SMTPSERVER = ConfigurationManager.AppSettings["SMTPSERVER"];
            _ErrorMailTo = ConfigurationManager.AppSettings["errorMailTo"];
            _LocalMail = ConfigurationManager.AppSettings["localMail"];
            _ReplyTo = ConfigurationManager.AppSettings["ReplyTo"];
            _MailFrom = ConfigurationManager.AppSettings["MailFrom"];
            _ApplicationName = ConfigurationManager.AppSettings["ApplicationName"];
            _SendError = ConfigurationManager.AppSettings["sendError"];
            _BWDataSynchQueue = ConfigurationManager.AppSettings["BWDataSynchQueue"];
            _BwHostUrlForJs = ConfigurationManager.AppSettings["BwHostUrlForJs"];
            _BwOprHostUrlForJs = ConfigurationManager.AppSettings["BwOprHostUrlForJs"];
            _ABApiHostUrl = ConfigurationManager.AppSettings["ABApiHostUrl"];
            _aspnet_UseTaskFriendlySynchronizationContext = ConfigurationManager.AppSettings["aspnet:UseTaskFriendlySynchronizationContext"];
            _ImageQueueName = ConfigurationManager.AppSettings["ImageQueueName"];
            _CwOprHostUrl = ConfigurationManager.AppSettings["CwOprHostUrl"];
            _GoogleDistanceMatrixAPIKey = ConfigurationManager.AppSettings["GoogleDistanceMatrixAPIKey"];
            _areaMaxPerCall = ConfigurationManager.AppSettings["areaMaxPerCall"];
            _CwWebServiceHostUrl = ConfigurationManager.AppSettings["CwWebServiceHostUrl"];
            _SmsQueue = ConfigurationManager.AppSettings["SmsQueue"];
            _AWSEnvironment = ConfigurationManager.AppSettings["AWSEnvironment"];
            _notificationUserId = ConfigurationManager.AppSettings["notificationUserId"];
            _notificationOtherUserId = ConfigurationManager.AppSettings["notificationOtherUserId"];
            _notificationToUserMailId = ConfigurationManager.AppSettings["notificationToUserMailId"];
            _notificationCCUserMailId = ConfigurationManager.AppSettings["notificationCCUserMailId"];
            _AWSAccessKey = ConfigurationManager.AppSettings["AWSAccessKey"];
            _AWSBucketName = ConfigurationManager.AppSettings["AWSBucketName"];
            _AWSSecretKey = ConfigurationManager.AppSettings["AWSSecretKey"];
            _AWSEnvironment = ConfigurationManager.AppSettings["AWSEnvironment"];
            _AWSImageQueueName = ConfigurationManager.AppSettings["AWSImageQueueName"];
            _SecurityHashLength = Convert.ToInt32(ConfigurationManager.AppSettings["SecurityHashLength"]);
            _UnitSoldDataNotificationDay = Convert.ToInt32(ConfigurationManager.AppSettings["unitSoldDataNotificationDay"]);
            _AWSS3Region = Convert.ToString(ConfigurationManager.AppSettings["AWSS3Region"]);
            _EmailsForBikeChange = Convert.ToString(ConfigurationManager.AppSettings["EmailsForBikeChange"]);
            _BwHostUrl = Convert.ToString(ConfigurationManager.AppSettings["BwHostUrl"]);
            _notificationToMailIdForPageMetas = Convert.ToString(ConfigurationManager.AppSettings["notificationToMailIdForPageMetas"]);
            _BWEsIndexUpdaterQueue = Convert.ToString(ConfigurationManager.AppSettings["BWEsIndexUpdaterQueue"]);
        }

        // Static method to provide access to instance
        public static BWOprConfiguration Instance
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
                            _instance = new BWOprConfiguration();
                        }
                    }
                }
                return _instance;
            }
        }

        public int NonReadOnlyStatic { get { return nonReadOnlyStatic++; } }

        /// <summary>
        /// Get the default city name.
        /// </summary>
        public string GetDefaultCityName
        {
            get { return _defaultCity; }
        }

        public string ImgHostURL { get { return _imgHostURL; } }

        public string DefaultCity { get { return _defaultCity; } }

        public string ReadOnlyConnectionString { get { return _ReadOnlyConnectionString; } }

        public string MasterConnectionString { get { return _MasterConnectionString; } }

        public string RabbitImgHostURL { get { return _RabbitImgHostURL; } }

        public string ImgPathFolder { get { return _ImgPathFolder; } }

        public string BikewaleUrl { get { return _bikewaleUrl; } }

        public string IsProduction { get { return _isProduction; } }

        public string Price_a_1 { get { return _price_a_1; } }

        public string Price_a_2 { get { return _price_a_2; } }

        public string Price_a_3 { get { return _price_a_3; } }

        public string Price_b_1 { get { return _price_b_1; } }

        public string Price_b_2 { get { return _price_b_2; } }

        public string Price_b_3 { get { return _price_b_3; } }

        public string Price_d_0_5 { get { return _price_d_0_5; } }

        public string Price_d_1 { get { return _price_d_1; } }

        public string Price_d_2 { get { return _price_d_2; } }

        public string Price_d_3 { get { return _price_d_3; } }

        public string Price_d_4 { get { return _price_d_4; } }

        public string Price_d_5 { get { return _price_d_5; } }

        public string Price_l_1 { get { return _price_l_1; } }

        public string Price_l_2 { get { return _price_l_2; } }

        public string Price_l_3 { get { return _price_l_3; } }

        public string ServiceTax { get { return _ServiceTax; } }

        public string DefaultName { get { return _DefaultName; } }

        public string IsMemcachedUsed { get { return _IsMemcachedUsed; } }

        public string MemcacheTimespan { get { return _MemcacheTimespan; } }

        public string SMTPSERVER { get { return _SMTPSERVER; } }

        public string ErrorMailTo { get { return _ErrorMailTo; } }

        public string LocalMail { get { return _LocalMail; } }

        public string ReplyTo { get { return _ReplyTo; } }

        public string MailFrom { get { return _MailFrom; } }

        public string ApplicationName { get { return _ApplicationName; } }

        public string SendError { get { return _SendError; } }

        public string BWDataSynchQueue { get { return _BWDataSynchQueue; } }

        public string BwHostUrlForJs { get { return _BwHostUrlForJs; } }

        public string BwOprHostUrlForJs { get { return _BwOprHostUrlForJs; } }

        public string ABApiHostUrl { get { return _ABApiHostUrl; } }

        public string Aspnet_UseTaskFriendlySynchronizationContext { get { return _aspnet_UseTaskFriendlySynchronizationContext; } }

        public string ImageQueueName { get { return _ImageQueueName; } }

        public string CwOprHostUrl { get { return _CwOprHostUrl; } }

        public string GoogleDistanceMatrixAPIKey { get { return _GoogleDistanceMatrixAPIKey; } }

        public string AreaMaxPerCall { get { return _areaMaxPerCall; } }

        public string CwWebServiceHostUrl { get { return _CwWebServiceHostUrl; } }

        public string SmsQueue { get { return _SmsQueue; } }

        public string NotificationUserId { get { return _notificationUserId; } }

        public string NotificationOtherUserId { get { return _notificationOtherUserId; } }

        public string NotificationToUserMailId { get { return _notificationToUserMailId; } }

        public string NotificationToMailIdForPageMetas { get { return _notificationToMailIdForPageMetas; } }

        public string NotificationCCUserMailId { get { return _notificationCCUserMailId; } }

        public string AWSAccessKey { get { return _AWSAccessKey; } }
        public string AWSBucketName { get { return _AWSBucketName; } }
        public string AWSSecretKey { get { return _AWSSecretKey; } }
        public string AWSEnvironment { get { return _AWSEnvironment; } }
        public string AWSImageQueueName { get { return _AWSImageQueueName; } }
        public int SecurityHashLength { get { return _SecurityHashLength; } }
        public int UnitSoldDataNotificationDay { get { return _UnitSoldDataNotificationDay; } }
        public String AWSS3Region { get { return _AWSS3Region; } }
        public string EmailsForBikeChange { get { return _EmailsForBikeChange; } }
        public string BwHostUrl { get { return _BwHostUrl; } }

        public string BWEsIndexUpdaterQueue { get { return _BWEsIndexUpdaterQueue; } }
    }   // class
}   // namespace