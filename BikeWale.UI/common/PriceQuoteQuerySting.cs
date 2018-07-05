using Bikewale.Utility;
using System;
using System.Web;

namespace Bikewale.Common
{

    public class PriceQuoteQueryString
    {
        #region Properties

        public static string PQId
        {
            get
            {
                if (HttpContext.Current.Request.QueryString != null && HttpContext.Current.Request.QueryString.HasKeys() && (!String.IsNullOrEmpty(HttpContext.Current.Request.QueryString["MPQ"])))
                {
                    if (!String.IsNullOrEmpty(HttpContext.Current.Request.QueryString["MPQ"]))
                    {
                        return HttpUtility.ParseQueryString(EncodingDecodingHelper.DecodeFrom64(HttpContext.Current.Request.QueryString["MPQ"])).Get("PQId");
                    }
                }
                return String.Empty;
            }
        }

        public static string CityId
        {
            get
            {
                if (HttpContext.Current.Request.QueryString != null && HttpContext.Current.Request.QueryString.HasKeys() && (!String.IsNullOrEmpty(HttpContext.Current.Request.QueryString["MPQ"])))
                {
                    if (!String.IsNullOrEmpty(HttpContext.Current.Request.QueryString["MPQ"]))
                    {
                        return HttpUtility.ParseQueryString(EncodingDecodingHelper.DecodeFrom64(HttpContext.Current.Request.QueryString["MPQ"])).Get("CityId");
                    }
                }
                return String.Empty;
            }
        }

        public static string AreaId
        {
            get
            {
                if (HttpContext.Current.Request.QueryString != null && HttpContext.Current.Request.QueryString.HasKeys() && (!String.IsNullOrEmpty(HttpContext.Current.Request.QueryString["MPQ"])))
                {
                    if (!String.IsNullOrEmpty(HttpContext.Current.Request.QueryString["MPQ"]))
                    {
                        return HttpUtility.ParseQueryString(EncodingDecodingHelper.DecodeFrom64(HttpContext.Current.Request.QueryString["MPQ"])).Get("AreaId");
                    }
                }
                return String.Empty;
            }
        }

        public static string VersionId
        {
            get
            {
                if (HttpContext.Current.Request.QueryString != null && HttpContext.Current.Request.QueryString.HasKeys() && (!String.IsNullOrEmpty(HttpContext.Current.Request.QueryString["MPQ"])))
                {
                    if (!String.IsNullOrEmpty(HttpContext.Current.Request.QueryString["MPQ"]))
                    {
                        return HttpUtility.ParseQueryString(EncodingDecodingHelper.DecodeFrom64(HttpContext.Current.Request.QueryString["MPQ"])).Get("VersionId");
                    }
                }
                return String.Empty;
            }
        }

        public static string DealerId
        {
            get
            {
                if (HttpContext.Current.Request.QueryString != null && HttpContext.Current.Request.QueryString.HasKeys() && (!String.IsNullOrEmpty(HttpContext.Current.Request.QueryString["MPQ"])))
                {
                    if (!String.IsNullOrEmpty(HttpContext.Current.Request.QueryString["MPQ"]))
                    {
                        return HttpUtility.ParseQueryString(EncodingDecodingHelper.DecodeFrom64(HttpContext.Current.Request.QueryString["MPQ"])).Get("DealerId");
                    }
                }
                return String.Empty;
            }
        }

		public static string LeadId
		{
			get
			{
				if (HttpContext.Current.Request.QueryString != null && HttpContext.Current.Request.QueryString.HasKeys() && (!String.IsNullOrEmpty(HttpContext.Current.Request.QueryString["MPQ"])))
				{
					if (!String.IsNullOrEmpty(HttpContext.Current.Request.QueryString["MPQ"]))
					{
						return HttpUtility.ParseQueryString(EncodingDecodingHelper.DecodeFrom64(HttpContext.Current.Request.QueryString["MPQ"])).Get("LeadId");
					}
				}
				return String.Empty;
			}
		}

        #endregion
        public static bool IsPQQueryStringExists()
        {
            if (HttpContext.Current.Request.QueryString != null && HttpContext.Current.Request.QueryString.HasKeys() && (!String.IsNullOrEmpty(HttpContext.Current.Request.QueryString["MPQ"])))
            {
                if (!String.IsNullOrEmpty(HttpContext.Current.Request.QueryString["MPQ"]))
                {
                    return true;
                }
            }
            return false;
        }

        public static string FormQueryString(string cityId, string pqId, string areaId, string versionId, string dealerId)
        {
            return String.Format("CityId={0}&AreaId={1}&PQId={2}&VersionId={3}&DealerId={4}", cityId, areaId, pqId, versionId, dealerId);
        }

        public static string FormBase64QueryString(string cityId, string pqId, string areaId, string versionId, string dealerId)
        {
            return EncodingDecodingHelper.EncodeTo64(String.Format("CityId={0}&AreaId={1}&PQId={2}&VersionId={3}&DealerId={4}", cityId, areaId, pqId, versionId, dealerId));
        }

        public static string QueryString
        {
            get
            {
                if (HttpContext.Current.Request.QueryString != null && HttpContext.Current.Request.QueryString.HasKeys() && (!String.IsNullOrEmpty(HttpContext.Current.Request.QueryString["MPQ"])))
                {
                    if (!String.IsNullOrEmpty(HttpContext.Current.Request.QueryString["MPQ"]))
                    {
                        return EncodingDecodingHelper.DecodeFrom64(HttpContext.Current.Request.QueryString["MPQ"]);
                    }
                }
                return String.Empty;
            }
        }
    }
}