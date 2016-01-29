using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Bikewale.Utility
{
    public static class BikeBookingCookie
    {
        #region Properties
        private static string _pqId = "0";
        public static string PQId
        {
            get
            {
                if (HttpContext.Current.Request.Cookies["_BBC"] != null)
                    return HttpContext.Current.Request.Cookies["_BBC"]["PQId"].ToString();
                else
                    return _pqId;
            }
            set
            {
                HttpContext.Current.Request.Cookies["_BBC"]["PQId"] = _pqId;
            }
        }

        private static string _city = "0";
        public static string CityId
        {
            get
            {
                if (HttpContext.Current.Request.Cookies["_BBC"] != null)
                    return HttpContext.Current.Request.Cookies["_BBC"]["CityId"].ToString();
                else
                    return _city;
            }
            set
            {
                HttpContext.Current.Request.Cookies["_BBC"]["CityId"] = _city;
            }

        }

        private static string _area = "0";
        public static string AreaId
        {
            get
            {
                if (HttpContext.Current.Request.Cookies["_BBC"] != null)
                    return HttpContext.Current.Request.Cookies["_BBC"]["AreaId"].ToString();
                else
                    return _area;
            }
            set
            {
                HttpContext.Current.Request.Cookies["_BBC"]["AreaId"] = _area;
            }

        }

        private static string _versionId = "0";
        public static string VersionId
        {
            get
            {
                if (HttpContext.Current.Request.Cookies["_BBC"] != null)
                    return HttpContext.Current.Request.Cookies["_BBC"]["VersionId"].ToString();
                else
                    return _versionId;
            }
            set
            {
                HttpContext.Current.Request.Cookies["_BBC"]["VersionId"] = _versionId;
            }

        }

        private static string _dealerId = "0";
        public static string DealerId
        {
            get
            {
                if (HttpContext.Current.Request.Cookies["_BBC"] != null)
                    return HttpContext.Current.Request.Cookies["_BBC"]["DealerId"].ToString();
                else
                    return _dealerId;
            }
            set
            {
                HttpContext.Current.Request.Cookies["_BBC"]["DealerId"] = _dealerId;
            }

        }
        #endregion


        #region Methods
        public static void SaveBBCookie(string cityId, string pqId, string areaId, string versionId, string dealerId)
        {

            HttpCookie objBBCookie = new HttpCookie("_BBC");
            objBBCookie.Values["CityId"] = cityId;
            objBBCookie.Values["AreaId"] = areaId;
            objBBCookie.Values["PQId"] = pqId;
            objBBCookie.Values["VersionId"] = versionId;
            objBBCookie.Values["DealerId"] = dealerId;
            objBBCookie.Expires = DateTime.Now.AddMinutes(20);

            HttpContext.Current.Response.Cookies.Add(objBBCookie);

        }

        public static bool IsBBCoockieExist()
        {
            bool isCookieExist = false;

            if (HttpContext.Current.Request["_BBC"] != null)
                isCookieExist = true;

            return isCookieExist;
        }

        public static void DeleteBBCookie()
        {
            HttpCookie objBBCookie = new HttpCookie("_BBC");
            objBBCookie.Value = string.Empty;
            objBBCookie.Expires = DateTime.Now.AddDays(-1);
            HttpContext.Current.Response.Cookies.Add(objBBCookie);
        }
        #endregion
    }
}
