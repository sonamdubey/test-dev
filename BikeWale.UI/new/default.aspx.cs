using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Bikewale.Common;
using Bikewale.Controls;
using Bikewale.Memcache;

namespace Bikewale.New
{
	public class Default : Page
	{
		protected Repeater rptWallpapers, rptBodyStyles;
		protected DataList dltMakes;
		protected DropDownList drpRevMake;
		//protected Button btnRev;
		//protected QuickResearch ucQuickResearch;
	
		protected override void OnInit( EventArgs e )
		{
			InitializeComponent();
		}
		
		void InitializeComponent()
		{
			base.Load += new EventHandler( Page_Load );
		}
		
		void Page_Load( object Sender, EventArgs e )
		{			
			if (!Page.IsPostBack)
			{
                DeviceDetection dd = new DeviceDetection(Request.ServerVariables["HTTP_X_REWRITE_URL"].ToString());
                dd.DetectDevice();

				LoadMakes();
				//LoadBodyStyles();
				//LoadWallpapers();
				//LoadUpcomingBikes();
			}
		}
		
        //private void LoadBodyStyles()
        //{
        //   DataSet ds = new DataSet();
        //   ds = CWCommon.GetStaticBodyStyles();
        //   rptBodyStyles.DataSource = ds;
        //   rptBodyStyles.DataBind();
        //}
		
		private void LoadMakes()
		{
			// Bind New Bike Makes
           //DataSet ds = new DataSet();
           //ds = BWCommon.GetStaticMakes();
           //dltMakes.DataSource = ds;
           //dltMakes.DataBind();
           //ucQuickResearch.MakeContents = ds;		   

            //Database db = null;
            DataSet ds = null;

            try
            {
                BikeMakes objMakes = new BikeMakes();
                ds = objMakes.GetNewBikeMakes();

                /*db = new Database();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "GetMakeModelVersion";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@condition",SqlDbType.VarChar, 10).Value = "Make";

                    ds = db.SelectAdaptQry(cmd);

                    dltMakes.DataSource = ds;
                    dltMakes.DataBind();
                }*/
                dltMakes.DataSource = ds;
                dltMakes.DataBind();
            }
            catch (SqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, "new.default.LoadMakes");
                objErr.SendMail();
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "new.default.LoadMakes");
                objErr.SendMail();
            }
	    }
		
        //private void LoadWallpapers()
        //{
        //    string sql = " SELECT TOP 3 RandomString, HostUrl FROM Wallpapers ORDER BY NEWID() ";
        //    try
        //    {
        //        CommonOpn op = new CommonOpn();
        //        op.BindRepeaterReader( sql, rptWallpapers );
        //    }
        //    catch(Exception err )
        //    {
        //        ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
        //        objErr.SendMail();
        //    } // catch Exception
        //}
	}
}		