using BikeWaleOpr.Common;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BikeWaleOpr.Content
{
    public class FeaturedBikes : Page
	{

		protected Repeater rptFeaturedBikes;
	
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
			LoadFeaturedBikes();
		}
		
		private void LoadFeaturedBikes()
		{
			string  sql = " SELECT CMA.ID AS MakeId, CMO.ID AS ModelId, CV.ID AS VersionId, CMA.Name + ' ' + CMO.Name + ' ' + CV.Name AS FeaturedBike "
						+ " FROM BikeVersions CV, BikeModels CMO, BikeMakes CMA "
						+ " WHERE CV.ID IN (SELECT DISTINCT FeaturedVersionId FROM CompareFeaturedBike WHERE IsActive = 1)"
						+ " AND CV.BikeModelId = CMO.ID"	
						+ " AND CMO.BikeMakeId = CMA.ID";
		
			CommonOpn op = new CommonOpn();
			try
			{
				op.BindRepeaterReader(sql, rptFeaturedBikes);
			}
			catch(Exception err)
			{
				ErrorClass.LogError(err,Request.ServerVariables["URL"]);
				
			}
		}
		
	}
}		