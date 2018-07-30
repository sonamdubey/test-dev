using System;
using System.Web.UI;

namespace Bikewale.Controls
{
    public class LeadCaptureControl : UserControl
    {
        public uint AreaId { get; set; }
        public uint ModelId { get; set; }
        public uint CityId { get; set; }
        protected string cityName = string.Empty, areaName = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            var location = Bikewale.Utility.GlobalCityArea.GetGlobalCityArea();
            cityName = location.City;
            areaName = location.Area;
        }
    }
}