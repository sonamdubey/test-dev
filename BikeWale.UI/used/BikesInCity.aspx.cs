using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
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

        protected void Page_Load(object sender, EventArgs e)
        {
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