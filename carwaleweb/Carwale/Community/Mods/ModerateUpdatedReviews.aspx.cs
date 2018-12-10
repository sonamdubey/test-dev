/*******************************************************************************************************
IN THIS CLASS THE NEW MEMBEERS WHO HAVE REQUESTED FOR REGISTRATION ARE SHOWN
*******************************************************************************************************/
using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Carwale.UI.Controls;
using Carwale.Notifications;
using Carwale.UI.Common;
using Carwale.DAL.Forums;
using Carwale.Cache.Forums;

namespace Carwale.UI.Community.Mods
{
    public class ModerateUpdatedReviews : Page
    {
        protected HtmlGenericControl spnError;
        protected Repeater dtgrdForums;
        public int recordNo = 0;

        public bool IsNew
        {
            get { return Convert.ToBoolean(ViewState["IsNew"]); }
            set { ViewState["IsNew"] = Convert.ToBoolean(value); }
        }

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
        }

        void Page_Load(object Sender, EventArgs e)
        {
            if (CurrentUser.Id == "-1")
            {
                Response.Redirect("/users/login.aspx?ReturnUrl=" + HttpUtility.UrlEncode(Request.ServerVariables["HTTP_X_REWRITE_URL"]));
            }

            ForumsCache threadInfo = new ForumsCache();
            bool isModerator = threadInfo.IsModerator(CurrentUser.Id);
            if (!(isModerator))
            {
                UrlRewrite.Return404();
            }

            if (!IsPostBack)
            {
                BindGrid();
            }
        } // Page_Load

        void BindGrid()
        {
            CommonOpn objCom = new CommonOpn();
            ForumsModeratorDAL moderatorDal = new ForumsModeratorDAL();    
            DataSet ds = new DataSet();
            try
            {           
                ds = moderatorDal.BindUnverifiedUpdatedReviews();
                objCom.BindRepeaterReaderDataSet(ds, dtgrdForums);
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }
    } // class
} // namespace