using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.Common
{
    public class PriceQuoteQueryString
    {
        private static string _queryString = String.Empty;
                
        #region Properties
        private static string _pqId = "0";
        public static string PQId
        {
            //CityId=1&AreaId=59&PQId=3884&VersionId=165&DealerId=4
            get
            {
                if (!String.IsNullOrEmpty(_queryString))
                {
                    _pqId = HttpUtility.ParseQueryString(_queryString).Get("PQId");
                }
                return _pqId;
            }
            set
            {
                _pqId = value;
            }
        }

        private static string _city = "0";
        public static string CityId
        {
            get
            {
                if (!String.IsNullOrEmpty(_queryString))
                {
                    _city = HttpUtility.ParseQueryString(_queryString).Get("CityId");
                }
                return _city;
            }
            set
            {
                _city = value;
            }

        }

        private static string _area = "0";
        public static string AreaId
        {
            get
            {
                if (!String.IsNullOrEmpty(_queryString))
                {
                    _area = HttpUtility.ParseQueryString(_queryString).Get("AreaId");
                }
                return _area;
            }
            set
            {
                _area = value;
            }

        }

        private static string _versionId = "0";
        public static string VersionId
        {
            get
            {
                if (!String.IsNullOrEmpty(_queryString))
                {
                    _versionId = HttpUtility.ParseQueryString(_queryString).Get("VersionId");
                }
                return _versionId;
            }
            set
            {
                _versionId = value;
            }

        }

        private static string _dealerId = "0";
        public static string DealerId
        {
            get
            {
                if (!String.IsNullOrEmpty(_queryString))
                {
                    _dealerId = HttpUtility.ParseQueryString(_queryString).Get("DealerId");
                }
                return _dealerId;
            }
            set
            {
                _dealerId = value;
            }
        }
        #endregion
        public static bool IsPQQueryStringExists()
        {
            if (HttpContext.Current.Request.QueryString != null && HttpContext.Current.Request.QueryString.HasKeys() && (!String.IsNullOrEmpty(HttpContext.Current.Request.QueryString["MPQ"])))
            {
                if (!String.IsNullOrEmpty(HttpContext.Current.Request.QueryString["MPQ"]))
                {
                    _queryString = EncodingDecodingHelper.DecodeFrom64(HttpContext.Current.Request.QueryString["MPQ"]);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void SaveQueryString(string cityId, string pqId, string areaId, string versionId, string dealerId)
        {
            _queryString = String.Format("CityId={0}&AreaId={1}&PQId={2}&VersionId={3}&DealerId={4}", cityId, areaId, pqId, versionId, dealerId);
        }

        public static string QueryString
        {
            get
            {
                return _queryString;
            }
        }
    }
}