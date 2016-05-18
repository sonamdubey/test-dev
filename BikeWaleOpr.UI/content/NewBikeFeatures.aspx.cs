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
using BikeWaleOpr.Common;
using BikeWaleOpr.Controls;

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
			
            //if( HttpContext.Current.User.Identity.IsAuthenticated != true) 
            //        Response.Redirect("../users/Login.aspx?ReturnUrl=../Contents/NewCarFeaturesStep1.aspx");
				
            //if ( Request.Cookies["Customer"] == null )
            //    Response.Redirect("../Users/Login.aspx?ReturnUrl=../Contents/NewCarFeaturesStep1.aspx");
				
            //int pageId = 38;
            //if ( !op.verifyPrivilege( pageId ) )
            //    Response.Redirect("../NotAuthorized.aspx");
				
			string sql, sql1;

			if ( !IsPostBack )
			{
                //if (!String.IsNullOrEmpty(Request.QueryString["versionId"]))
                //{
                //    BindRepeater();
                //    btnSave.Visible = true;
                //}

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
            //if ( Request.Form["cmbVersion"] == null || !CommonOpn.CheckId( Request.Form["cmbVersion"] ) )
            //    return;
			
            //Trace.Warn("saving data for : " + Request.Form["cmbVersion"]);
			
            //string sql = "";
            //Database db = new Database();
			
            //try
            //{
            //    // Delete all features for this version first.
            //    sql = " DELETE FROM NewBikeFeatures WHERE BikeVersionId=" + Request.Form["cmbVersion"];
            //    db.InsertQry( sql );
            //}
            //catch (SqlException err)
            //{
            //    Trace.Warn(err.Message + err.Source);
            //    ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}
            //catch( Exception err )
            //{
            //    Trace.Warn(err.Message + err.Source);
            //    ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}
					
            //foreach ( RepeaterItem item in rptFeatures.Items )
            //{
            //    Trace.Warn("Repeater...");
            //    if( (item.ItemType == ListItemType.Item) || (item.ItemType == ListItemType.AlternatingItem) )
            //    {
            //        DataGrid dg = ( DataGrid )item.FindControl( "dgItems" );
					
            //        if ( dg != null )
            //        {
            //            foreach ( DataGridItem gridItem in dg.Items )
            //            {
            //                Trace.Warn("Grid...");
            //                CheckBox chk = (CheckBox)gridItem.FindControl( "chkFeature" );
            //                try
            //                {
            //                    if ( chk.Checked )
            //                    {	
            //                        sql = " INSERT INTO NewBikeFeatures(BikeVersionId,FeatureItemId)"
            //                            + " VALUES(" + Request.Form["cmbVersion"] + ", " + dg.DataKeys[ gridItem.ItemIndex ] + " )";
								
            //                        Trace.Warn( sql );		
            //                        db.InsertQry( sql );	
            //                    }
            //                }
            //                catch (SqlException err)
            //                {
            //                    Trace.Warn(err.Message + err.Source);
            //                    ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
            //                    objErr.SendMail();
            //                }
            //                catch(Exception err)
            //                {
            //                    Trace.Warn(err.Message + err.Source);
            //                    ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
            //                    objErr.SendMail();
            //                }
            //            }
            //        }
            //    }
            //}
            
            //db.CloseConnection();
            ////Update Changes Log
            ////ContentCommon cc = new ContentCommon();
            ////cc.LogUpdates("Link Features with version", "VersionId", Request.Form["cmbVersion"].ToString());
		}
		
		///<summary>
		///This function gets the list of the sell inquiries made according to the 
		///model
		///</summary>
		void BindRepeater()
		{
            throw new Exception("Method not used/commented");

            //string sql = "";
						
            //sql = " SELECT NC.Id AS CategoryId, NC.Name As Category "
            //    + " FROM NewBikeFeatureCategories NC ";
			
            //Trace.Warn(sql);
			
            //Database db = new Database();
			
            //DataSet ds = new DataSet();
            //SqlConnection cn = new SqlConnection( db.GetConString() );
			
            //try
            //{
            //    cn.Open();
				
            //    SqlDataAdapter adp = new SqlDataAdapter( sql, cn );
            //    adp.Fill( ds, "Categories" );
				
            //    sql = " SELECT NI.ID, NI.Name Feature, NI.CategoryId AS CategoryId,"
            //        + " (SELECT FeatureItemId FROM NewBikeFeatures "
            //        + " 	WHERE FeatureItemId=NI.ID AND BikeVersionId=" + Request.Form["cmbVersion"] + ") IsAvailable" 
            //        + " FROM NewBikeFeatureItems NI WHERE IsObsolete<>1 AND BikeModelId=" + Request.Form["cmbModel"];
					
            //    Trace.Warn("sql : ",sql);

            //    adp = new SqlDataAdapter( sql, cn );
            //    adp.Fill( ds, "Features" );	
				
            //    ds.Relations.Add( 
            //            "myRelation", 
            //            ds.Tables["Categories"].Columns["CategoryId"], 
            //            ds.Tables["Features"].Columns["CategoryId"] 
            //        );
					
            //    rptFeatures.DataSource = ds.Tables["Categories"];	
            //    rptFeatures.DataBind();
				
            //}
            //catch(Exception err)
            //{
            //    Trace.Warn(err.Message + err.Source);
            //    ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}
            //finally
            //{
            //    if ( cn.State == ConnectionState.Open ) cn.Close();
            //}
			
		}
	} // class
} // namespace