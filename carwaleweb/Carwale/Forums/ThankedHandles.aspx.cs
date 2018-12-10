using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Carwale.UI.Common;
using Carwale.UI.Controls;
using System.Collections;
using Ajax;
using Carwale.Notifications;
using Carwale.DAL.Forums;

namespace Carwale.UI.Forums
{
    public class ThankedHandles : Page
    {
        #region Global Variables
        protected Repeater rptThankedHandles;

        #endregion

        #region On Init
        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }
        #endregion

        #region Initialize Component
        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
        }
        #endregion

        #region Page Load
        void Page_Load(object Sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadHandles();
            }
        }
        #endregion

        #region Load Handles
        private void LoadHandles()
        {
            UserDAL userDetials = new UserDAL();
            DataTable dt = userDetials.LoadHandles(Request.QueryString["postId"]);
            try
            {
                rptThankedHandles.DataSource = dt;
                rptThankedHandles.DataBind();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }
        #endregion
    }
}