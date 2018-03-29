/*******************************************************************************************************
IN THIS CLASS THE NEW MEMBEERS WHO HAVE REQUESTED FOR REGISTRATION ARE SHOWN
*******************************************************************************************************/
using BikeWaleOpr.Common;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace BikeWaleOpr.Content
{
    public class NewBikeFeatures : Page
	{
		protected HtmlGenericControl spnError;
		protected Button btnSave, btnFind;
		protected Repeater rptFeatures;
		protected DropDownList cmbMake;
										 
		protected override void OnInit( EventArgs e )
		{
			InitializeComponent();
		}
		
		void InitializeComponent()
		{
			base.Load += new EventHandler( Page_Load );
			btnSave.Click += new EventHandler( btnSave_Click );
			btnFind.Click += new EventHandler( btnFind_Click );
			rptFeatures.ItemDataBound += new RepeaterItemEventHandler( rptFeatures_ItemBind );
		}
		
		void Page_Load( object Sender, EventArgs e )
		{
			CommonOpn op = new CommonOpn();
			
          
				
			string sql, sql1;

			if ( !IsPostBack )
			{
               

				sql = "SELECT ID, Name FROM BikeMakes WHERE IsDeleted <> 1 ORDER BY NAME";
				op.FillDropDown( sql, cmbMake, "Name", "ID" );
				ListItem item = new ListItem( "--Select--", "0" );
				cmbMake.Items.Insert( 0, item );
			}
			
			sql = " SELECT DISTINCT Mo.ID, Mo.Name, BikeMakeId FROM BikeModels Mo, BikeVersions Ve"
				+ " WHERE Mo.IsDeleted <> 1 AND Mo.New=1 AND Ve.New=1 AND Mo.ID=Ve.BikeModelId "
				+ " AND Ve.ID IN (SELECT BikeVersionid FROM NewBikeSpecifications) "
				+ " ORDER BY Mo.NAME";
				
			sql1 = "SELECT DISTINCT Ve.ID, Ve.Name, BikeModelId FROM BikeVersions Ve"
				+ " WHERE Ve.IsDeleted <> 1 AND Ve.New=1 AND Ve.ID IN (SELECT BikeVersionid FROM NewBikeSpecifications) "
				+ " ORDER BY Ve.NAME";
			
			string Script = op.GenerateChainScript( "cmbMake", "cmbModel", "cmbVersion", sql, sql1 );
			//RegisterStartupScript( "ChainScript", Script );
			ClientScript.RegisterStartupScript(this.GetType(), "ChainScript", Script );

		} // Page_Load
		
		void rptFeatures_ItemBind( object sender, RepeaterItemEventArgs e )
		{
			RepeaterItem item = e.Item;
			Trace.Warn( "Item Binding..." );
			if( (item.ItemType == ListItemType.Item) || (item.ItemType == ListItemType.AlternatingItem) )
			{
				DataRowView drv = (DataRowView)item.DataItem;
				DataGrid dg = ( DataGrid )item.FindControl( "dgItems" );
				
				if ( dg != null )
				{
					dg.DataSource = drv.CreateChildView( "myRelation" ); 					
					dg.DataBind();
				}
			}
		}
		
		void btnFind_Click( object Sender, EventArgs e )
		{
			if ( Request.Form["cmbVersion"] == null || !CommonOpn.CheckId( Request.Form["cmbVersion"] ) )
				return;
				
			BindRepeater();
			btnSave.Visible = true;
			
		}
		
		void btnSave_Click( object Sender, EventArgs e )
		{
            throw new Exception("Method not used/commented");
          
		}
		
		///<summary>
		///This function gets the list of the sell inquiries made according to the 
		///model
		///</summary>
		void BindRepeater()
		{
            throw new Exception("Method not used/commented");

          
			
		}
	} // class
} // namespace