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

        protected string _cityName = String.Empty;
        protected int? cityId = null;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            GlobalCityAreaEntity currentCityArea = GlobalCityArea.GetGlobalCityArea();
            cityId = Convert.ToInt32(currentCityArea.CityId);
            _cityName = currentCityArea.City;
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


        protected string FormatControlHeader()
        {
            return String.Format("Popular used bikes in {0}", !String.IsNullOrEmpty(_cityName) ? _cityName : "India");
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

        protected string FormatCompleteListUrl()
        {
            string url = String.Empty;
            if (cityId.HasValue)
            {
                url = String.Format("/m/used/bikes-in-{0}/", _cityName);
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