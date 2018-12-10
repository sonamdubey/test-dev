using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carwale.Notifications;
using Carwale.UI.Common;
using Carwale.DAL.Forums;
using System.Data.Common;
using Carwale.DAL.CoreDAL.MySql;
using System.Data;
using Carwale.Cache.Forums;

namespace Carwale.UI.Community.Mods
{
    public class PostsInModeration : Page
    {
        public int counter = 1;
        protected Repeater rptReport;
        protected string modid = "-1";

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }
        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            ForumsCache threadInfo = new ForumsCache();
            bool isModerator = threadInfo.IsModerator(CurrentUser.Id);
            if (!(isModerator))
            {
                UrlRewrite.Return404();
            }
            modid = CurrentUser.Id;
            FillRepeater();
        }

        protected void FillRepeater()
        {
            CommonOpn op = new CommonOpn();
            DataSet ds = new DataSet();
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("GetPostsInModeration_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    ds = MySqlDatabase.SelectAdapterQuery(cmd, DbConnections.ForumsMySqlReadConnection);
                }
                op.BindRepeaterReaderDataSet(ds, rptReport);
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }
        protected void rptReport_ItemCommand(Object Sender, RepeaterCommandEventArgs e)
        {
            Trace.Warn("The " + ((Button)e.CommandSource).Text + " button has just been clicked");
            //Trace.Warn(rptReport.Items[Convert.ToInt32(e.Item.ItemIndex)].FindControl("ID").ToString());
            Trace.Warn((e.Item.ItemIndex).ToString());
            Trace.Warn(rptReport.Items[1].FindControl("ID").ToString());
        }
        protected void btn_ApproveClick(object sender, EventArgs e)
        {
            Trace.Warn("Button Clicked on click event");
        }
    }
}