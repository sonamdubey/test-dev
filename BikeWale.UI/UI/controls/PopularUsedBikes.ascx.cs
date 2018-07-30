using Bikewale.BindViewModels.Controls;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace Bikewale.Controls
{
    public class PopularUsedBikes : System.Web.UI.UserControl
    {
        protected Repeater rptPopularUsedBikes;
        public uint TotalRecords { get; set; }
        public int FetchedRecordsCount { get; set; }
        public int PQSourceId { get; set; }

        protected string cityName = "India";
        protected int? cityId = null;
        protected string usedCityMaskingName = String.Empty;

        public string header { get; set; }


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
            string location = string.Empty;
            cityId = null;
            cityName = string.Empty;
            if (this.Context.Request.Cookies.AllKeys.Contains("location") && this.Context.Request.Cookies["location"].Value != "0")
            {
                location = this.Context.Request.Cookies["location"].Value;
                var _locArray = location.Split('_');
                if (_locArray != null && _locArray.Length > 0)
                {
                    cityId = Convert.ToInt32(_locArray[0]);
                    if (_locArray.Length > 1)
                    {
                        cityName = (_locArray[1]).Replace('-', ' ');
                    }
                }
            }

        }

        protected string FormatUsedBikeUrl(string makeMaskingName, string cityMaskingName)
        {
            string url = String.Empty;
            usedCityMaskingName = cityMaskingName.Trim();
            if (cityId.HasValue)
            {
                url = String.Format("/used/{0}-bikes-in-{1}/", makeMaskingName, usedCityMaskingName);
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
                url = String.Format("/used/bikes-in-{0}/", usedCityMaskingName);
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

        public override void Dispose()
        {
            rptPopularUsedBikes.DataSource = null;
            rptPopularUsedBikes.Dispose();

            base.Dispose();
        }
    }
}