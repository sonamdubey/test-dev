using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Carwale.UI.Common;
using Carwale.DAL.CoreDAL;
using Carwale.Notifications;
using Carwale.Notifications.Logs;
using Carwale.DAL.Classified;

namespace Carwale.Used
{
    public class UCAUnsubscribe : System.Web.UI.Page
    {
        protected HtmlInputButton btnConfirm;
        protected HtmlGenericControl dMes, dReq;
        protected HtmlInputHidden hdnUCAId;
        protected HtmlInputHidden hdnUCAEmail;
        protected string CustomerEmail = string.Empty;
        protected int CustomerCity = 0;
        private string _ucaId = string.Empty;

        public string UCAId
        {
            get { return _ucaId; }
            set { _ucaId = value; }
        }


        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(UCAUnsubscribe_Load);
            btnConfirm.ServerClick += new EventHandler(btnConfirm_ServerClick);
        }

        void UCAUnsubscribe_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["uca"]))
                {
                    if (CommonOpn.IsNumeric(Request.QueryString["uca"].ToString()))
                    {
                        UCAId = Request.QueryString["uca"].ToString();
                        hdnUCAId.Value = UCAId;
                    }
                }
                if (!String.IsNullOrEmpty(Request.QueryString["email"]))
                {
                    CustomerEmail = Request.QueryString["email"].ToString();
                    hdnUCAEmail.Value = CustomerEmail;
                }
            }
        }

        void btnConfirm_ServerClick(object sender, EventArgs e)
        {
            ClassifiedEmailAlertRepository clasifiedEmailRepo = new ClassifiedEmailAlertRepository();
            clasifiedEmailRepo.UnsubscribeNdUsedCarAlertCustomer(Convert.ToInt32(hdnUCAId.Value), hdnUCAEmail.Value, out CustomerCity);
            dReq.Visible = false;
            dMes.Visible = true;
        }
    }
}