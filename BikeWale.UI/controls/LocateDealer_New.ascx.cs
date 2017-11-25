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

namespace Bikewale.Controls
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
                string _apiUrl = "/api/DealerMakes/";

                using (Utility.BWHttpClient objClient = new Utility.BWHttpClient())
                {
                    //makes = objClient.GetApiResponseSync<NewBikeDealersMakeList>(Utility.BWConfiguration.Instance.BwHostUrl, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, makes);
                    makes = objClient.GetApiResponseSync<NewBikeDealersMakeList>(Utility.APIHost.BW, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, makes);
                }

                if (makes != null && makes.Makes!=null && makes.Makes.Any())
                {
                    cmbMake.DataSource = makes.Makes;
                    cmbMake.DataBind();
                }
            }
            catch (Exception err)
            {                
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);
                
            }
        }
    }
}