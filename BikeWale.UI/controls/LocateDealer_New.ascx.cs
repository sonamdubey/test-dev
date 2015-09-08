using Bikewale.DTO.Make;
using Bikewale.Notifications;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.controls
{
    public partial class LocateDealer_New : System.Web.UI.UserControl
    {
        protected DropDownList cmbCity, cmbMake;
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
                        
        }

        private void FillMakes()
        {
            NewBikeDealersMakeList makes = null;
            try
            {
                string _bwHostUrl = ConfigurationManager.AppSettings["bwHostUrl"];
                string _requestType = "application/json";
                string _apiUrl = "/api/DealerMakes/";

                makes = BWHttpClient.GetApiResponseSync<NewBikeDealersMakeList>(_bwHostUrl, _requestType, _apiUrl, makes);

                if (makes != null && makes.Makes!=null && makes.Makes.Count() > 0)
                {
                    cmbMake.DataSource = makes.Makes;
                    cmbMake.DataBind();
                }
            }
            catch (Exception err)
            {                
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }
    }
}