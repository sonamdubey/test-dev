using Bikewale.Entities.BikeData;
using System;
using System.Web.UI;


namespace Bikewale.Mobile.Controls
{
    /// <summary>
    /// Created By : Aditi Srivastava on 2 Dec 2016
    /// Summary    : Show cities by make for dealers and service centers in a city
    /// </summary>
    public class BrandCityPopUp : UserControl
    {
        public EnumBikeType requestType;
        public uint cityId, makeId;
        protected string staticUrl = Bikewale.Utility.BWConfiguration.Instance.StaticUrl;
        protected string staticFileVersion = Bikewale.Utility.BWConfiguration.Instance.StaticFileVersion;
        protected bool isOperaBrowser = false;
        protected void Page_Load(object sender, EventArgs e)
        {

            
            DetectOperaBrowser();
        }

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(this.Page_Load);
        }


        private void DetectOperaBrowser()
        {
            System.Web.HttpBrowserCapabilities browserDetection = Request.Browser;
            if (browserDetection != null)
            {
                string browserType = browserDetection.Type;
                string browser = browserDetection.Browser;
                string browserVersion = browserDetection.Version;

                if (!string.IsNullOrEmpty(browserType) && browserType.ToLower().Contains("mini"))
                    isOperaBrowser = true;
            }

        }

    }
}
