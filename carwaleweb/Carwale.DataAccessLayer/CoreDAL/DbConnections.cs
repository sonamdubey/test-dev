using System.Configuration;
using Carwale.Utility;
using log4net;

namespace Carwale.DAL.CoreDAL.MySql
{
	public static class DbConnections
    {
		private static ILog logger = LogManager.GetLogger(typeof(DbConnections));
    private static readonly string _sqlConString = ConfigurationManager.AppSettings["connectionStringClassified"];
		private static readonly string _classifiedMySqlMasterConString;
		private static readonly string _advantageMySqlMasterConString;
		private static readonly string _newCarMySqlMasterConString;
		private static readonly string _carDataMySqlMasterConString;
		private static readonly string _esMySqlMasterConString;
		private static readonly string _editcmsMySqlMasterConString;
		private static readonly string _forumsMySqlMasterConString;
		private static readonly string _accessoriesMySqlMasterConString;
		private static readonly string _cwqMySqlMasterConString;
		private static readonly string _offersMySqlMasterConString;
    private static readonly string _pricesMySqlMasterConString;

		private static readonly string _classifiedMySqlReadConString;
		private static readonly string _advantageMySqlReadConString;
		private static readonly string _newCarMySqlReadConString;
		private static readonly string _carDataMySqlReadConString;
		private static readonly string _esMySqlReadConString;
		private static readonly string _editcmsMySqlReadConString;
		private static readonly string _forumsMySqlReadConString;
		private static readonly string _accessoriesMySqlReadConString;
		private static readonly string _cwqMySqlReadConString;
		private static readonly string _offersMySqlReadConString;
    private static readonly string _pricesMySqlReadConString;

		static DbConnections() {

			_classifiedMySqlMasterConString = GetConnectionStringFromKMS("ConnectionMySqlMasterClassified"); 
			_advantageMySqlMasterConString = GetConnectionStringFromKMS("ConnectionMySqlMasterAdvantage");
			_newCarMySqlMasterConString = GetConnectionStringFromKMS("ConnectionMySqlMasterNewCar");
			_carDataMySqlMasterConString = GetConnectionStringFromKMS("ConnectionMySqlMasterCarData");
			_esMySqlMasterConString = GetConnectionStringFromKMS("ConnectionMySqlMasterEs");
			_editcmsMySqlMasterConString = GetConnectionStringFromKMS("ConnectionMySqlMasterEditCms");
			_forumsMySqlMasterConString = GetConnectionStringFromKMS("ConnectionMySqlMasterForums");
			_accessoriesMySqlMasterConString = GetConnectionStringFromKMS("ConnectionMySqlMasterAccessories");
			_cwqMySqlMasterConString = GetConnectionStringFromKMS("ConnectionMySqlMasterCWQ");
			_offersMySqlMasterConString = GetConnectionStringFromKMS("ConnectionMySqlMasterOffers");
      _pricesMySqlMasterConString = GetConnectionStringFromKMS("ConnectionMySqlMasterPrices");

			_classifiedMySqlReadConString = GetConnectionStringFromKMS("ConnectionMySqlReadClassified");
			_advantageMySqlReadConString = GetConnectionStringFromKMS("ConnectionMySqlReadAdvantage");
			_newCarMySqlReadConString = GetConnectionStringFromKMS("ConnectionMySqlReadNewCar");
			_carDataMySqlReadConString = GetConnectionStringFromKMS("ConnectionMySqlReadCarData");
			_esMySqlReadConString = GetConnectionStringFromKMS("ConnectionMySqlReadEs");
			_editcmsMySqlReadConString = GetConnectionStringFromKMS("ConnectionMySqlReadEditCms");
			_forumsMySqlReadConString = GetConnectionStringFromKMS("ConnectionMySqlReadForums");
			_accessoriesMySqlReadConString = GetConnectionStringFromKMS("ConnectionMySqlReadAccessories");
			_cwqMySqlReadConString = GetConnectionStringFromKMS("ConnectionMySqlReadCWQ");
			_offersMySqlReadConString = GetConnectionStringFromKMS("ConnectionMySqlReadOffers");
      _pricesMySqlReadConString = GetConnectionStringFromKMS("ConnectionMySqlReadPrices");
		}

		private static string GetConnectionStringFromKMS(string key)
		{
            var connStr = AwsUtils.GetObjectFromS3(key);
            if (string.IsNullOrEmpty(connStr))
            {
                logger.Error("could not get connection string " + key + " from AWS");
                return ConfigurationManager.ConnectionStrings[key] != null ? ConfigurationManager.ConnectionStrings[key].ConnectionString : "";
            }
            return connStr;
		}

		public static string Connection
        {
            get
            {
                return _sqlConString;
            }
        }
        public static string ClassifiedMySqlReadConnection
        {
            get
            {
                return _classifiedMySqlReadConString;
            }
        }
        public static string AdvantageMySqlReadConnection
        {
            get
            {
                return _advantageMySqlReadConString;
            }
        }
        public static string NewCarMySqlReadConnection
        {
            get
            {
                return _newCarMySqlReadConString;
            }
        }
        public static string CarDataMySqlReadConnection
        {
            get
            {
                return _carDataMySqlReadConString;
            }
        }
        public static string EsMySqlReadConnection
        {
            get
            {
                return _esMySqlReadConString;
            }
        }
        public static string EditCmsMySqlReadConnection
        {
            get
            {
                return _editcmsMySqlReadConString;
            }
        }
        public static string ForumsMySqlReadConnection
        {
            get
            {
                return _forumsMySqlReadConString;
            }
        }
        public static string AccessoriesMySqlReadConnection
        {
            get
            {
                return _accessoriesMySqlReadConString;
            }
        }

        public static string CWQMySqlReadConnection
        {
            get
            {
                return _cwqMySqlReadConString;
            }
        }

        public static string ClassifiedMySqlMasterConnection
        {
            get
            {
                return _classifiedMySqlMasterConString;
            }
        }
        public static string AdvantageMySqlMasterConnection
        {
            get
            {
                return _advantageMySqlMasterConString;
            }
        }
        public static string NewCarMySqlMasterConnection
        {
            get
            {
                return _newCarMySqlMasterConString;
            }
        }
        public static string CarDataMySqlMasterConnection
        {
            get
            {
                return _carDataMySqlMasterConString;
            }
        }
        public static string EsMySqlMasterConnection
        {
            get
            {
                return _esMySqlMasterConString;
            }
        }
        public static string EditCmsMySqlMasterConnection
        {
            get
            {
                return _editcmsMySqlMasterConString;
            }
        }
        public static string ForumsMySqlMasterConnection
        {
            get
            {
                return _forumsMySqlMasterConString;
            }
        }
        public static string AccessoriesMySqlMasterConnection
        {
            get
            {
                return _accessoriesMySqlMasterConString;
            }
        }
        public static string CWQMySqlMasterConnection
        {
            get
            {
                return _cwqMySqlMasterConString;
            }
        }
        public static string OffersMySqlMasterConnection
        {
            get
            {
                return _offersMySqlMasterConString;
            }
        }
        public static string OffersMySqlReadConnection
        {
            get
            {
                return _offersMySqlReadConString;
            }
        }
        
        public static string PricesMySqlMasterConnection
        {
            get
            {
                return _pricesMySqlMasterConString;
            }
        }
        
        public static string PricesMySqlReadConnection
        {
            get
            {
                return _pricesMySqlReadConString;
            }
        }
    }
}
