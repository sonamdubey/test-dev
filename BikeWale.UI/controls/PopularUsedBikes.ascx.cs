﻿using Bikewale.BindViewModels.Controls;
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
        public int TotalRecords { get; set; }
        protected int FetchedRecordsCount { get; set; }
        

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
            BindUsedBikesControl.TotalRecords = TotalRecords;
            BindUsedBikesControl.CityId = cityId;
            BindUsedBikesControl.BindRepeater(rptPopularUsedBikes);
            this.FetchedRecordsCount = BindUsedBikesControl.FetchedRecordsCount;
        }

        private void CheckCityCookie(out int? cityId, out string cityName)
        {
            string location = String.Empty;
            int locationValue;
            if (this.Context.Request.Cookies.AllKeys.Contains("location") && int.TryParse(this.Context.Request.Cookies["location"].Value, out locationValue) && locationValue > 0)
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
            return String.Format("{0} used bikes",makeName);
        }        
    }
}