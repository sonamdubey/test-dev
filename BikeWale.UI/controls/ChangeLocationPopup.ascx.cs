using Bikewale.Entities.Location;
using System;

namespace Bikewale.Controls
{
    /// <summary>
    /// Created By : 
    /// Description : 
    /// </summary>
    public class ChangeLocationPopup : System.Web.UI.UserControl
    {
        public uint UrlCityId { get; set; }
        public string UrlCityName { get; set; }
        public string CustomMessage { get; set; }

        protected uint CookieCityId;
        protected string CookieCityName = string.Empty;
        protected bool IsLocationChange = false;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            GlobalCityAreaEntity objLocation = Bikewale.Utility.GlobalCityArea.GetGlobalCityArea();
            CookieCityId = objLocation.CityId;
            CookieCityName = objLocation.City;
            IsLocationChange = (UrlCityId > 0 && UrlCityId != CookieCityId);
        }
    }
}