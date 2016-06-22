using Bikewale.Entities.Location;
using System;
using System.Linq;
using System.Web;

namespace Bikewale.Utility
{
    public class GlobalCityArea
    {
        /// <summary>
        /// Created By : Vivek Gupta
        /// Date : 22nd june 2016
        /// Desc : Get Global city area from the location cookie
        /// </summary>
        /// <returns></returns>
        public static GlobalCityAreaEntity GetGlobalCityArea()
        {
            string location = String.Empty;
            GlobalCityAreaEntity objGlobalCityArea = new GlobalCityAreaEntity();

            var cookies = HttpContext.Current.Request.Cookies;
            if (cookies.AllKeys.Contains("location"))
            {
                location = cookies["location"].Value;
                if (!String.IsNullOrEmpty(location) && location.IndexOf('_') != -1)
                {
                    string[] locArray = location.Split('_');//1_Mumbai_59_Aarey Colony	

                    objGlobalCityArea.CityId = Convert.ToUInt32(locArray[0]);
                    objGlobalCityArea.City = Convert.ToString(locArray[1]);
                    objGlobalCityArea.AreaId = locArray.Length > 2 ? Convert.ToUInt32(locArray[2]) : default(UInt32);
                    objGlobalCityArea.Area = locArray.Length > 3 ? Convert.ToString(locArray[3]) : default(string);
                }
            }

            return objGlobalCityArea;
        }
    }
}
