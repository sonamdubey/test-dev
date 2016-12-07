using Bikewale.Entities.BikeData;
using System;
using System.Web.UI;


namespace Bikewale.Controls
{
    /// <summary>
    /// Created By : Aditi Srivastava on 28 Nov 2016
    /// Summary    : Show cities by make for dealers and service centers in a city
    /// </summary>
    public class BrandCityPopUp : UserControl
    {
        public EnumBikeType requestType;
        public uint makeId;
        public uint cityId;
        protected string staticUrl1 = Bikewale.Utility.BWConfiguration.Instance.StaticUrl;
        protected string staticFileVersion1 = Bikewale.Utility.BWConfiguration.Instance.StaticFileVersion;
        
        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(this.Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            string originalUrl = Request.ServerVariables["HTTPS_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];
        }

    }
}
