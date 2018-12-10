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
using Carwale.Notifications;
using Carwale.DAL.CoreDAL;
using System.Data.Common;
using Carwale.DAL.CoreDAL.MySql;

namespace Carwale.UI.Community
{
    public class Default : Page
    {
        protected Repeater rptHotForumDiscussions;
        protected HtmlGenericControl divHotForumDiscussions, divFeaturedAlbums;
        protected DataList dtgrdViewAlbum;

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
            if (!IsPostBack)
            {
                GetHotForumDiscussions();            
            }
        } // Page_Load

        // will fetch hot discussions in Forums
        private void GetHotForumDiscussions()
        {          
            try
            {
                DataSet ds = new DataSet();
                CommonOpn op = new CommonOpn();
                using(DbCommand cmd = DbFactory.GetDBCommand("GetHotDiscussionForums_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    ds = MySqlDatabase.SelectAdapterQuery(cmd, DbConnections.ForumsMySqlReadConnection);
                    op.BindRepeaterReaderDataSet(ds, rptHotForumDiscussions);                   
                }
                if (rptHotForumDiscussions.Items.Count <= 0)
                    divHotForumDiscussions.Visible = false;
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception
        }       
    } // class
} // namespace