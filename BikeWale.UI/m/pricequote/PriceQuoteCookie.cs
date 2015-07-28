using System;
using System.Collections.Generic;
using System.Web;

namespace Bikewale.Mobile.PriceQuote
{
    #region PriceQuoteCookie class Defination
    /// <summary>
    /// Created By : Ashwini Todkar on 28 April 2014
    /// Summary    : class to manage PriceQuoteCookie
    /// </summary>
    public static class PriceQuoteCookie
    {
        #region Properties
        private static string _pqId = "0";
        public static string PQId 
        { 
            get 
            {
                if (HttpContext.Current.Request.Cookies["_MPQ"] != null)
                    return HttpContext.Current.Request.Cookies["_MPQ"]["PQId"].ToString();
                else
                    return _pqId;
            }
            set
            {
                HttpContext.Current.Request.Cookies["_MPQ"]["PQId"] = _pqId;
            }
        }

        private static string _city = "0";
        public static string CityId
        {
            get
            {
                if (HttpContext.Current.Request.Cookies["_MPQ"] != null)
                    return HttpContext.Current.Request.Cookies["_MPQ"]["CityId"].ToString();
                else
                    return _city;
            }
            set 
            {
                HttpContext.Current.Request.Cookies["_MPQ"]["CityId"] = _city;
            }

        }

        private static string _area = "0";
        public static string AreaId
        {
            get
            {
                if (HttpContext.Current.Request.Cookies["_MPQ"] != null)
                    return HttpContext.Current.Request.Cookies["_MPQ"]["AreaId"].ToString();
                else
                    return _area;
            }
            set
            {
                HttpContext.Current.Request.Cookies["_MPQ"]["AreaId"] = _area;
            }

        }

        private static string _versionId = "0";
        public static string VersionId
        {
            get
            {
                if (HttpContext.Current.Request.Cookies["_MPQ"] != null)
                    return HttpContext.Current.Request.Cookies["_MPQ"]["VersionId"].ToString();
                else
                    return _versionId;
            }
            set
            {
                HttpContext.Current.Request.Cookies["_MPQ"]["VersionId"] = _versionId;
            }

        }

        private static string _dealerId = "0";
        public static string DealerId
        {
            get
            {
                if (HttpContext.Current.Request.Cookies["_MPQ"] != null)
                    return HttpContext.Current.Request.Cookies["_MPQ"]["DealerId"].ToString();
                else
                    return _dealerId;
            }
            set
            {
                HttpContext.Current.Request.Cookies["_MPQ"]["DealerId"] = _dealerId;
            }

        }
        #endregion


        #region Methods
        /// <summary>
        /// Writen By : Ashwini Todkar on 28 April 2014
        /// Summary   : Set  cookie for customer who takes price quote
        /// Modified By : Sadhana Upadhyay on 28 Oct 2014 to remove name , email, mobile no
        /// </summary>
        public static void SavePQCookie(string cityId, string pqId, string areaId, string versionId,string dealerId)
        {

            HttpCookie objPQCookie = new HttpCookie("_MPQ");
            objPQCookie.Values["CityId"] = cityId;
            objPQCookie.Values["AreaId"] = areaId;
            objPQCookie.Values["PQId"] = pqId;
            objPQCookie.Values["VersionId"] = versionId;
            objPQCookie.Values["DealerId"] = dealerId;

            HttpContext.Current.Response.Cookies.Add(objPQCookie);

        }

        /// <summary>
        /// written By : Ashwini Todkar on 28 april 2014
        /// Summary    : method to check price cookie exist 
        /// </summary>
        /// <returns>if cookie exist it returns true otherwise returns false</returns>
        public static bool IsPQCoockieExist()
        {
            bool isCookieExist = false;

            if (HttpContext.Current.Request["_MPQ"] != null)
                isCookieExist = true;

            return isCookieExist;
        }
        #endregion
    }

    #endregion
}