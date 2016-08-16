using System;
using System.Web.UI;

namespace Bikewale.Mobile.Controls
{
    public class LeadCaptureControl : UserControl
    {
        public uint AreaId { get; set; }
        public uint ModelId { get; set; }
        public uint CityId { get; set; }
        protected string cityName = Bikewale.Utility.GlobalCityArea.GetGlobalCityArea().City;
        protected string areaName = Bikewale.Utility.GlobalCityArea.GetGlobalCityArea().Area;
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}