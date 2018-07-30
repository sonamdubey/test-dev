using Bikewale.BindViewModels.Controls;
using Bikewale.Entities.Location;
using Bikewale.Utility;
using System;
using System.Web.UI.WebControls;

namespace Bikewale.Mobile.Controls
{
    public class PopularUsedBikes : System.Web.UI.UserControl
    {
        protected Repeater rptPopularUsedBikes;
        public uint TotalRecords { get; set; }
        public int FetchedRecordsCount { get; set; }
        public int PQSourceId { get; set; }
        protected string cityName = "India";
        protected string cityMaskingName = string.Empty;
        protected int? cityId = null;
        public string header { get; set; }


        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            GlobalCityAreaEntity currentCityArea = GlobalCityArea.GetGlobalCityArea();
            cityId = Convert.ToInt32(currentCityArea.CityId);
            cityName = currentCityArea.City;
            BindPopularUsedBikes();
        }
        /// <summary>
        /// Modified By :-Subodh Jain on 1 Dec 2016
        /// Summary :- Added cityMaskingName
        /// </summary>
        /// <returns></returns>
        private void BindPopularUsedBikes()
        {
            BindUsedBikesControl objUsed = new BindUsedBikesControl();
            objUsed.TotalRecords = TotalRecords;
            objUsed.CityId = cityId;

            objUsed.BindRepeater(rptPopularUsedBikes);
            cityMaskingName = objUsed.cityMaskingName;
            this.FetchedRecordsCount = objUsed.FetchedRecordsCount;
        }

        protected string FormatUsedBikeUrl(string makeMaskingName, string cityMaskingName)
        {
            string url = String.Empty;
            if (cityId.HasValue)
            {
                url = String.Format("/m/used/{0}-bikes-in-{1}/", makeMaskingName, cityMaskingName.Trim());
            }
            else
            {
                url = String.Format("/m/used/{0}-bikes-in-india/", makeMaskingName);
            }
            return url;
        }
        /// <summary>
        /// Modified By :-Subodh Jain on 1 Dec 2016
        /// Summary :- changed url to cityMaskingName
        /// </summary>
        /// <returns></returns>
        protected string FormatCompleteListUrl()
        {
            string url = String.Empty;
            if (cityId.HasValue)
            {
                url = String.Format("/m/used/bikes-in-{0}/", cityMaskingName);
            }
            else
            {
                url = "/m/used/bikes-in-india/";
            }
            return url;
        }

        protected string FormatImgAltTitle(string makeName)
        {
            return String.Format("{0} used bikes", makeName);
        }
    }
}