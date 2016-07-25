using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using BikeWaleOpr.Common;
using BikeWaleOpr.Controls;
using Ajax;
using System.IO;

namespace BikeWaleOpr.EditCms
{
	public class PublishArticle : Page
	{
        protected string ArticleTitle = string.Empty, unpublish = string.Empty;


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
            string basicId = string.Empty;
            if (!IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["bid"]))
                {
                    basicId = Request.QueryString["bid"].ToString();
                    GetTitle(basicId);
                }
                if (!String.IsNullOrEmpty(Request.QueryString["unpublish"]))
                {
                    unpublish = Request.QueryString["unpublish"].ToString();
                } 
            }            
		}

        private void GetTitle(string basicId)
        {
            throw new Exception("Method not used/commented");

            //string sql = string.Empty;
            //SqlDataReader dr = null;
            //SqlCommand cmd = new SqlCommand();
            //Database db = new Database();

            //try
            //{
            //    sql = "Select Title From Con_EditCms_Basic Where Id = @BasicId";
            //    cmd.CommandText = sql;
            //    cmd.CommandType = CommandType.Text;
            //    cmd.Parameters.Add("@BasicId", SqlDbType.BigInt).Value = basicId;

            //    dr = db.SelectQry(cmd);
            //    while (dr.Read())
            //    {
            //        ArticleTitle = dr["Title"].ToString();
            //    }
            //}
            //catch (SqlException ex)
            //{
            //    ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}
            //catch (Exception ex)
            //{
            //    ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}
            //finally
            //{
            //    if (dr != null)
            //    {
            //        dr.Close();
            //    }
            //    db.CloseConnection();
            //}
        }		
	}
}		