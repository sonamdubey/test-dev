using Bikewale.BindViewModels.Controls;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace Bikewale.Mobile.controls
{
    public class PopularUsedBikes : System.Web.UI.UserControl
    {
        protected Repeater rptPopularUsedBikes;
        public uint TotalRecords { get; set; }
        public int FetchedRecordsCount { get; set; }

        protected string cityName = String.Empty;
        protected static int? cityId = null;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            CheckCityCookie(out cityId, out cityName);
            BindPopularUsedBikes();
        }

        private void BindPopularUsedBikes()
        {
            BindUsedBikesControl objUsed = new BindUsedBikesControl();
            objUsed.TotalRecords = TotalRecords;
            objUsed.CityId = cityId;
            objUsed.BindRepeater(rptPopularUsedBikes);
            this.FetchedRecordsCount = objUsed.FetchedRecordsCount;
        }

        /// <summary>
        /// Modified By : Sushil Kumar on 26th August 2016
        /// Description : Replaced location name from location cookie to selected location objects for city and area respectively.
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="cityName"></param>
        private void CheckCityCookie(out int? cityId, out string cityName)
        {
            string location = String.Empty;
            cityId = null;
            cityName = String.Empty;
            if (this.Context.Request.Cookies.AllKeys.Contains("location"))
            {
                location = this.Context.Request.Cookies["location"].Value;
                var _locArray = location.Split('_');
                if (_locArray != null && _locArray.Length > 0)
                {
                    cityId = Convert.ToInt32(_locArray[0]);
                    cityName = (_locArray[1]).Replace('-', ' ');
                }
            }

        }

        protected string FormatControlHeader()
        {
            return String.Format("Popular used bikes in {0}", !String.IsNullOrEmpty(cityName) ? cityName : "India");
        }

        protected string FormatUsedBikeUrl(string makeMaskingName, string cityMaskingName)
        {
            string url = String.Empty;
            cityName = cityMaskingName.Trim();
            if (cityId.HasValue)
            {
                url = String.Format("/used/{0}-bikes-in-{1}/", makeMaskingName, cityName);
            }
            else
            {
                url = String.Format("/used/{0}-bikes-in-india/", makeMaskingName);
            }
            return url;
        }

        protected string FormatCompleteListUrl()
        {
            string url = String.Empty;
            if (cityId.HasValue)
            {
                url = String.Format("/used/bikes-in-{0}/", cityName);
            }
            else
            {
                url = "/used/bikes-in-india/";
            }
            return url;
        }

        protected string FormatImgAltTitle(string makeName)
        {
            return String.Format("{0} used bikes", makeName);
        }
    }
}