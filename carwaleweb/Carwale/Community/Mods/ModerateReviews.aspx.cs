using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Carwale.UI.Common;
using Carwale.DAL.Forums;
using System.Data.Common;
using Carwale.DAL.CoreDAL.MySql;
using MySql.Data.MySqlClient;
using Carwale.Cache.Forums;

namespace Carwale.UI.Community.Mods
{
    public class ModerateReviews : Page
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
                //BindGrid();
                BindUnverifiedReviews();
            }
        } // Page_Load

      
        void BindUnverifiedReviews()
        {   DataSet ds = new DataSet();
            ForumsModeratorDAL moderatorDal = new ForumsModeratorDAL();
            ds = moderatorDal.BindUnverifiedReviews();
            if (ds != null && ds.Tables.Count > 0)
            {
                dtgrdForums.DataSource = ds.Tables[0];
                dtgrdForums.DataBind();
            }
        }
    } // class
} // namespace