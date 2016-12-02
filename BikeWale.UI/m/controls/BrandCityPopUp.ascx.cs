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
        protected string staticUrl = System.Configuration.ConfigurationManager.AppSettings["staticUrl"];
        protected string staticFileVersion = System.Configuration.ConfigurationManager.AppSettings["staticFileVersion"];
        protected bool isOperaBrowser = false;
        protected void Page_Load(object sender, EventArgs e)
        {

            string originalUrl = Request.ServerVariables["HTTPS_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];  
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
