using Carwale.DAL.Classified.UsedDealers;
using Carwale.Notifications;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Carwale.UI.Used.Dealers
{
    public class DealerShowroom : Page
    {
        protected DataList dlCityList;

        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.Load += new System.EventHandler(this.Page_Load);
        }

        private void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
            {
                BindControl();
            }
        }

        private void BindControl()
        {
            
            try
            {
                DealerShowroomCityRepository dealerCity = new DealerShowroomCityRepository();
                dlCityList.DataSource = dealerCity.GetDealerShowroomCity();
                dlCityList.DataBind();
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

    }
}