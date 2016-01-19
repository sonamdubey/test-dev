using Bikewale.BindViewModels.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.controls
{
    public class PopularUsedBikes : System.Web.UI.UserControl
    {
        protected Repeater rptPopularUsedBikes;
        public uint TotalRecords { get; set; }
        public int FetchedRecordsCount { get; set; }
        public int PQSourceId { get; set; }
        

        protected string cityName = String.Empty;
        protected static int? cityId = null;
        protected static string usedCityMaskingName = String.Empty;

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

        private void CheckCityCookie(out int? cityId, out string cityName)
        {
            string location = String.Empty;
            if (this.Context.Request.Cookies.AllKeys.Contains("location") && this.Context.Request.Cookies["location"].Value!= "0")
            {
                location = this.Context.Request.Cookies["location"].Value;
                cityId = Convert.ToInt32(location.Split('_')[0]);
                cityName = location.Split('_')[1];
                return;
            }
            cityId = null;
            cityName = String.Empty;
        }

        protected string FormatControlHeader()
        {
            return String.Format("Popular used bikes in {0}", !String.IsNullOrEmpty(cityName) ? cityName : "India");
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
            return String.Format("{0} used bikes",makeName);
        }

        public override void Dispose()
        {
            rptPopularUsedBikes.DataSource = null;
            rptPopularUsedBikes.Dispose();

            base.Dispose();
        }
    }
}