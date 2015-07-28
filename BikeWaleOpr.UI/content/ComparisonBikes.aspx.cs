using System;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using BikeWaleOpr.Common;
using FreeTextBoxControls;
using Ajax;

namespace BikeWaleOpr.Content
{
	public class ComparisonBikes : Page
	{
		protected string selectedFeaturedBike;
		protected Repeater rptComparisonBikes;
		protected string BikesAlreadyAdded = "";
	
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
			selectedFeaturedBike = Request.QueryString["featureBike"].ToString();
			selectedFeaturedBike = selectedFeaturedBike.Replace("_"," ");
			LoadComparisonBikes();
		}
		
		private void LoadComparisonBikes()
		{
            Database db = new Database();
            SqlCommand cmd = new SqlCommand();
            string sql = " SELECT	CFC.VersionId, CMA.Name + ' ' + CMO.Name + ' ' + CV.Name AS ComparisonBike"
                       + " FROM	CompareFeaturedBike CFC, BikeVersions CV, BikeModels CMO, BikeMakes CMA"
                       + " WHERE"
                       + " CFC.VersionId = CV.ID  AND"
                       + " CV.BikeModelId = CMO.ID AND"
                       + " CMO.BikeMakeId = CMA.ID AND"
                       + " CFC.IsActive = 1 AND CFC.FeaturedVersionId IN (" + Request.QueryString["featureBikeId"].ToString() + ")";

			CommonOpn op = new CommonOpn();
			try
			{
				op.BindRepeaterReader(sql, rptComparisonBikes);
			}
			catch(Exception err)
			{
				ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
				objErr.SendMail();
			}
		}
		
		protected void GetBikesAlreadyAdded(string bike)
		{
			Response.Write("hey");
			Response.End();
			//return "hey";
		}
		
	}
}		