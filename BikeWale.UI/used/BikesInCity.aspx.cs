using Bikewale.Common;
using System;
using System.Web.UI.WebControls;

namespace Bikewale.Used
{
    /// <summary>
    /// Created By : Ashwini Todkar on 3 April 2014
    /// </summary>

    public class BikesInCity : System.Web.UI.Page
    {
        protected Repeater rptCity;
        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }
        protected void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
        }

        /// <summary>
        /// Modified by : Sajal Gupta on 06-10-2016
        /// Desc : Added device detection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Device Detection
            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];
            DeviceDetection dd = new DeviceDetection(originalUrl);
            dd.DetectDevice();

            if (!IsPostBack)
            {
                BindCities();
            }
        }

        private void BindCities()
        {
            SearchCommon objSC = new SearchCommon();

            rptCity.DataSource = objSC.GetUsedBikeByCityWithCount();
            rptCity.DataBind();
        }
    }
}