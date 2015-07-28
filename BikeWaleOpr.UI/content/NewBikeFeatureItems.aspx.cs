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
	public class NewBikeFeatureItems : Page
	{
		protected HtmlGenericControl spnError;
		protected DropDownList cmbCategories, cmbMake;
		protected TextBox txtItems;
		protected Button btnSave, btnFind;
		protected DataGrid dtgrdMembers;
		protected RequiredFieldValidator req1;
				
		private string SortCriteria
		{
			get { return ViewState["SortCriteria"].ToString(); }
			set { ViewState["SortCriteria"] = value; }
		} // SortCriteria
		
		private string SortDirection
		{
			get { return ViewState["SortDirection"].ToString(); }
			set { ViewState["SortDirection"] = value; }
		} // SortDirection
						 
		protected override void OnInit( EventArgs e )
		{
			InitializeComponent();
		}
		
		void InitializeComponent()
		{
			base.Load += new EventHandler( Page_Load );
			btnSave.Click += new EventHandler( btnSave_Click );
			dtgrdMembers.PageIndexChanged += new DataGridPageChangedEventHandler(Page_Change);
			dtgrdMembers.SortCommand += new DataGridSortCommandEventHandler(Sort_Grid);
			dtgrdMembers.EditCommand += new DataGridCommandEventHandler( dtgrdMembers_Edit );
			dtgrdMembers.UpdateCommand += new DataGridCommandEventHandler( dtgrdMembers_Update );
			dtgrdMembers.CancelCommand += new DataGridCommandEventHandler( dtgrdMembers_Cancel );
			dtgrdMembers.DeleteCommand += new DataGridCommandEventHandler( dtgrdMembers_Delete );
			
			btnFind.Click += new EventHandler( btnFind_Click );
		}
		
		void Page_Load( object Sender, EventArgs e )
		{
			string sql = string.Empty;
			CommonOpn op = new CommonOpn();
			
            //if( HttpContext.Current.User.Identity.IsAuthenticated != true) 
            //        Response.Redirect("../users/Login.aspx?ReturnUrl=../Contents/NewCarFeatureItems.aspx");
				
            //if ( Request.Cookies["Customer"] == null )
            //    Response.Redirect("../Users/Login.aspx?ReturnUrl=../Contents/NewCarFeatureItems.aspx");
				
            //int pageId = 38;
            //if ( !op.verifyPrivilege( pageId ) )
            //    Response.Redirect("../NotAuthorized.aspx");
			
			if ( !IsPostBack )
			{
				try
				{
                    sql = "SELECT Id, Name FROM NewBikeFeatureCategories WHERE IsActive=1";
                    op.FillDropDown(sql, cmbCategories, "Name", "Id");
                    cmbCategories.Items.Insert(0, new ListItem("Select", "0"));
					
					sql = "SELECT ID, Name FROM BikeMakes WHERE IsDeleted <> 1 ORDER BY NAME";
					op.FillDropDown( sql, cmbMake, "Name", "ID" );
					ListItem item = new ListItem( "--Select--", "0" );
					cmbMake.Items.Insert( 0, item );
				}
				catch ( SqlException ex )
				{
					Trace.Warn(ex.Message + ex.Source);
					ErrorClass objErr = new ErrorClass(ex,Request.ServerVariables["URL"]);
					objErr.SendMail();
				}
                catch (Exception ex)
                {
                    Trace.Warn(ex.Message + ex.Source);
                    ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                    objErr.SendMail();
                }

				SortDirection = "";
				SortCriteria = "Category, Feature";		
			}
			
			sql = "SELECT ID, Name, BikeMakeId FROM BikeModels WHERE IsDeleted <> 1 ORDER BY Name";
            string Script = op.GenerateChainScript("cmbMake", "cmbModel", sql, "Select Model");
			//RegisterStartupScript( "ChainScript", Script );
			ClientScript.RegisterStartupScript(this.GetType(), "ChainScript", Script );
		} // Page_Load
		
		void btnFind_Click( object Sender, EventArgs e )
		{
			BindGrid();
		}
		
		void btnSave_Click( object Sender, EventArgs e )
		{
			string sql;
			
			string[] features = txtItems.Text.Split( '\n' );
			Database db = new Database();
			
			for ( int i = 0; i < features.Length; i++ )
			{
				sql = "INSERT INTO NewBikeFeatureItems( Name, CategoryId, BikeModelId ) "
					+ " VALUES( '" + features[i].Trim().Replace( "'", "''" ) + "', " 
					+ cmbCategories.SelectedValue + ", "
					+ Request.Form["cmbModel"] + " )";

				try
				{
					db.InsertQry( sql );
				}
				catch( SqlException ex )	
				{
					Trace.Warn(ex.Message + ex.Source);
					ErrorClass objErr = new ErrorClass(ex,Request.ServerVariables["URL"]);
					objErr.SendMail();
				}
			}
			
			//Update Changes Log
            //ContentCommon cc = new ContentCommon();
            //cc.LogUpdates("Available Features Add", "ModelId", Request.Form["cmbModel"]);
					
			txtItems.Text = "";
			
			BindGrid();
		}
		
		///<summary>
		///This function gets the list of the sell inquiries made according to the 
		///model
		///</summary>
		void BindGrid()
		{
			string sql = "";
						
			sql = " SELECT NI.ID, NI.Name Feature, NC.Id AS CategoryId, NC.Name As Category "
				+ " FROM NewBikeFeatureItems NI, NewBikeFeatureCategories NC "
				+ " WHERE NI.CategoryId=NC.Id AND BikeModelId=" + Request.Form["cmbModel"];
			
			if(SortCriteria != "")
				sql += " ORDER BY " + SortCriteria + " " + SortDirection ; 
			
			Trace.Warn(sql);
			CommonOpn objCom = new CommonOpn();			
			try
			{
				objCom.BindGridSet( sql, dtgrdMembers );
			}
			catch(Exception err)
			{
				Trace.Warn(err.Message + err.Source);
				ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
				objErr.SendMail();
			}
			
		}
		
		void dtgrdMembers_Edit( object sender, DataGridCommandEventArgs e )
		{
			dtgrdMembers.EditItemIndex = e.Item.ItemIndex;
			BindGrid();
			btnSave.Enabled = false;
		}
		
		void dtgrdMembers_Update( object sender, DataGridCommandEventArgs e )
		{
			Page.Validate();
			if ( !Page.IsValid ) return;
			
			string sql;
			TextBox txt = (TextBox) e.Item.FindControl( "txtItem" );
			sql = "UPDATE NewBikeFeatureItems SET "
				+ " Name='" + txt.Text.Trim().Replace("'","''") + "',"
				+ " CategoryId=" + Request.Form["cmbGridCategory"]
				+ " WHERE Id=" + dtgrdMembers.DataKeys[ e.Item.ItemIndex ];
			
			Database db = new Database();
			
			try
			{
				db.InsertQry( sql );
				//Update Changes Log
                //ContentCommon cc = new ContentCommon();
                //cc.LogUpdates("Available Features Update", "ModelId", Request.Form["cmbModel"]);
			}
			catch( SqlException ex )	
			{
				Trace.Warn(ex.Message + ex.Source);
				ErrorClass objErr = new ErrorClass(ex,Request.ServerVariables["URL"]);
				objErr.SendMail();
			}
			dtgrdMembers.EditItemIndex = -1;
			btnSave.Enabled = true;
			BindGrid();
		}
		
		void dtgrdMembers_Cancel( object sender, DataGridCommandEventArgs e )
		{
			dtgrdMembers.EditItemIndex = -1;
			BindGrid();
			btnSave.Enabled = true;
		}
		
		void dtgrdMembers_Delete( object sender, DataGridCommandEventArgs e )
		{
			string sql;
			
			sql = "DELETE NewBikeFeatureItems WHERE Id=" + dtgrdMembers.DataKeys[ e.Item.ItemIndex ];
			
			Database db = new Database();
			
			try
			{
				db.DeleteQry( sql );
				
				//Update Changes Log
                //ContentCommon cc = new ContentCommon();
                //cc.LogUpdates("Available Features Delete", "ModelId", Request.Form["cmbModel"]);
			}
			catch( SqlException ex )	
			{
				Trace.Warn(ex.Message + ex.Source);
				ErrorClass objErr = new ErrorClass(ex,Request.ServerVariables["URL"]);
				objErr.SendMail();
			}
			BindGrid();
		}
		
		void Page_Change(object sender,DataGridPageChangedEventArgs e)
		{
			// Set CurrentPageIndex to the page the user clicked.
			dtgrdMembers.CurrentPageIndex = e.NewPageIndex;
			BindGrid();
		}
		
		
		 // <summary>
		/// this function sorts the dataset based on given criteria
		/// </summary>
		/// <paramname="sender"></param>
		/// <paramname="e"></param>
		protected void Sort_Grid(Object sender, DataGridSortCommandEventArgs e)
		{
			if ( SortCriteria == e.SortExpression )
			{
				SortDirection = SortDirection == "DESC" ? "ASC" : "DESC"; 
			}
			else
			{
				SortDirection = "ASC";
			}
			SortCriteria = e.SortExpression;
			
			BindGrid();
			
		} 
		
	
	} // class
} // namespace