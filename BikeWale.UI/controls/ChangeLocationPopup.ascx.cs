using Bikewale.Entities.Location;
using Bikewale.Notifications;
using System;

namespace Bikewale.Controls
{
    /// <summary>
    /// Created By :  Sushil Kumar on 15th Jan 2017
    /// Description : Control to handle user location for url and cookie city mismatch
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
            try
            {
                GlobalCityAreaEntity objLocation = Bikewale.Utility.GlobalCityArea.GetGlobalCityArea();
                CookieCityId = objLocation.CityId;
                CookieCityName = objLocation.City;
                IsLocationChange = (UrlCityId > 0 && UrlCityId != CookieCityId);
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, "Bikewale.Controls.ColorCount.ChangeLocationPopup.Page_Load");
            }
        }
    }
}