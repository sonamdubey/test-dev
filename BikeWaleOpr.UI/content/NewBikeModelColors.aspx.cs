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
	public class NewBikeModelColors : Page
	{
		protected HtmlGenericControl spnError;
		protected Button btnSave, btnFind;
		protected DataGrid dtgrdColors;
		protected DropDownList cmbMake,cmbModel;
		protected TextBox txtColor, txtCode, txtHexCode;

        public string SelectedModel
        {
            get
            {
                if (Request.Form["cmbModel"] != null)
                    return Request.Form["cmbModel"].ToString();
                else
                    return "-1";
            }
        }

        public string ModelContents
        {
            get
            {
                if (Request.Form["hdn_cmbModel"] != null)
                    return Request.Form["hdn_cmbModel"].ToString();
                else
                    return "";
            }
        }

		protected override void OnInit( EventArgs e )
		{
			InitializeComponent();
		}
		
		void InitializeComponent()
		{
			base.Load += new EventHandler( Page_Load );
			btnSave.Click += new EventHandler( btnSave_Click );
			btnFind.Click += new EventHandler( btnFind_Click );
			dtgrdColors.EditCommand += new DataGridCommandEventHandler( dtgrdColors_Edit );
			dtgrdColors.UpdateCommand += new DataGridCommandEventHandler( dtgrdColors_Update );
			dtgrdColors.CancelCommand += new DataGridCommandEventHandler( dtgrdColors_Cancel );
			dtgrdColors.DeleteCommand += new DataGridCommandEventHandler( dtgrdColors_Delete );
		}
		
		void Page_Load( object Sender, EventArgs e )
		{
            Ajax.Utility.RegisterTypeForAjax(typeof(AjaxFunctions));
			CommonOpn op = new CommonOpn();
			string sql;
			
			if ( !IsPostBack )
			{
				sql = "SELECT ID, Name FROM BikeMakes WHERE IsDeleted <> 1 ORDER BY NAME";
				op.FillDropDown( sql, cmbMake, "Name", "ID" );
				ListItem item = new ListItem( "--Select--", "0" );
				cmbMake.Items.Insert( 0, item );
			}
            else
            {
                AjaxFunctions aj = new AjaxFunctions();
                aj.UpdateContents(cmbModel, ModelContents, SelectedModel);
            }
            //sql = "SELECT ID, Name, BikeMakeId FROM BikeModels WHERE IsDeleted <> 1 ORDER BY Name";
            //string Script = op.GenerateChainScript("cmbMake", "cmbModel", sql, "Select Model");
            ////RegisterStartupScript( "ChainScript", Script );
            //ClientScript.RegisterStartupScript(this.GetType(), "ChainScript", Script);			
		} // Page_Load
	
		void btnFind_Click( object Sender, EventArgs e )
		{
			BindGrid();
		}
		
		void btnSave_Click( object Sender, EventArgs e )
		{            
			SaveColor( Request["cmbModel"].ToString() );
            
			BindGrid();
			
			txtColor.Text = "";
			txtCode.Text = "";
		}	
		
		void SaveColor( string modelId )
		{
			string sql = "";
			Database db = new Database();
					
			sql = " INSERT INTO ModelColors ( Color, Code, HexCode, BikeModelId ) "
				+ " VALUES('" 
				+ txtColor.Text.Trim().Replace("'","''") + "','" 
				+ txtCode.Text.Trim().Replace("'","''") + "','" 
				+ txtHexCode.Text.Trim().Replace("'","''") + "', " + modelId + " )";
				
			try
			{
				db.InsertQry( sql );
			}
			catch(SqlException err)
			{
				Trace.Warn(err.Message + err.Source);
				ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
				objErr.SendMail();
			}
		}
		
		///<summary>
		///This function gets the list of the sell inquiries made according to the 
		///model
		///</summary>
		void BindGrid()
		{
			string sql = "";
						
			sql = " SELECT ID, Color, Code, HexCode "
				+ " FROM ModelColors WHERE IsActive=1 AND BikeModelId=" + Request["cmbModel"];
			
			Trace.Warn(sql);
			
			Database db = new Database();
			
			DataSet ds = new DataSet();
			SqlConnection cn = new SqlConnection( db.GetConString() );
						
			try
			{
				cn.Open();
				
				SqlDataAdapter adp = new SqlDataAdapter( sql, cn );
				adp.Fill( ds, "Categories" );				
					
				dtgrdColors.DataSource = ds.Tables["Categories"];	
				dtgrdColors.DataBind();
				
			}
			catch(Exception err)
			{
				Trace.Warn(err.Message + err.Source);
				ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
				objErr.SendMail();
			}
			finally
			{
				if ( cn.State == ConnectionState.Open ) cn.Close();
			}
		}
		
		void dtgrdColors_Edit( object sender, DataGridCommandEventArgs e )
		{
			dtgrdColors.EditItemIndex = e.Item.ItemIndex;
			BindGrid();
			btnSave.Enabled = false;
		}
		
		void dtgrdColors_Update( object sender, DataGridCommandEventArgs e )
		{
			Page.Validate();
			if ( !Page.IsValid ) return;
			
			string sql;
			
			TextBox txtCol = (TextBox) e.Item.FindControl( "txtColor" );
			TextBox txtCod = (TextBox) e.Item.FindControl( "txtCode" );
			TextBox txtHCod = (TextBox) e.Item.FindControl( "txtHexCode" );
			
			sql = "UPDATE ModelColors SET "
				+ " Color='" + txtCol.Text.Trim().Replace("'","''") + "',"
				+ " Code='" + txtCod.Text.Trim().Replace("'","''") + "',"
				+ " HexCode='" + txtHCod.Text.Trim().Replace("'","''") + "'"
				+ " WHERE Id=" + dtgrdColors.DataKeys[ e.Item.ItemIndex ];
				
			Trace.Warn( sql );
			Database db = new Database();
			
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
			
			dtgrdColors.EditItemIndex = -1;
			btnSave.Enabled = true;
			BindGrid();
		}
		
		void dtgrdColors_Cancel( object sender, DataGridCommandEventArgs e )
		{
			dtgrdColors.EditItemIndex = -1;
			BindGrid();
			btnSave.Enabled = true;
		}
		
		void dtgrdColors_Delete( object sender, DataGridCommandEventArgs e )
		{
			string sql;
			
			sql = "UPDATE ModelColors SET IsActive=0 WHERE Id=" + dtgrdColors.DataKeys[ e.Item.ItemIndex ];
			
			Database db = new Database();
			
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
			BindGrid();
		}
	} // class
} // namespace