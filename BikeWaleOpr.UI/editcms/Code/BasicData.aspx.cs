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
using System.Drawing.Imaging;

namespace BikeWaleOpr.EditCms
{
	public class BasicData : Page
	{			
		protected Repeater rptBasicData;
					 
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
				LoadBasicData();
			}
		}
		
		private void LoadBasicData()
		{
			string sql = "";
			
			sql = " SELECT ID, Title, CONVERT(VARCHAR, DisplayDate, 103) AS DisplayDate, AuthorName, description FROM Con_EditCms_Basic WHERE IsActive = 1 AND CategoryId = " + Request.QueryString["catId"] +" Order By ID Desc";
			
			CommonOpn op = new CommonOpn();
			
			try
			{
				op.BindRepeaterReader(sql, rptBasicData);
			}
			catch(Exception err)
			{
				ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
				objErr.SendMail();
			}
		}
	}
}		