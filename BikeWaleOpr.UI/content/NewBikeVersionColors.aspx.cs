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
	public class NewBikeVersionColors : Page
	{
		protected HtmlGenericControl spnError;
		protected Button btnSave;
		protected Label lblBike;
		protected DataGrid dtgrdColors;
		protected CheckBoxList chkModelColors;
		protected TextBox txtColor, txtCode, txtHexCode;
		
		string qryStrModel = "";
		string qryStrVersion = "";
										 
		protected override void OnInit( EventArgs e )
		{
			InitializeComponent();
		}
		
		void InitializeComponent()
		{
			base.Load += new EventHandler( Page_Load );
			btnSave.Click += new EventHandler( btnSave_Click );
			dtgrdColors.EditCommand += new DataGridCommandEventHandler( dtgrdColors_Edit );
			dtgrdColors.UpdateCommand += new DataGridCommandEventHandler( dtgrdColors_Update );
			dtgrdColors.CancelCommand += new DataGridCommandEventHandler( dtgrdColors_Cancel );
			dtgrdColors.DeleteCommand += new DataGridCommandEventHandler( dtgrdColors_Delete );
		}
		
		void Page_Load( object Sender, EventArgs e )
		{
			//CommonOpn op = new CommonOpn();
			
            //if( HttpContext.Current.User.Identity.IsAuthenticated != true) 
            //        Response.Redirect("../users/Login.aspx?ReturnUrl=../Contents/NewCarVersionColorsStep1.aspx");
				
            //if ( Request.Cookies["Customer"] == null )
            //    Response.Redirect("../Users/Login.aspx?ReturnUrl=../Contents/NewCarVersionColorsStep1.aspx");
				
            //int pageId = 38;
            //if ( !op.verifyPrivilege( pageId ) )
            //    Response.Redirect("../NotAuthorized.aspx");
		
			if( Request.QueryString["Model"] != null && Request.QueryString["Model"].ToString() != "")
			{
				if( Request.QueryString["Version"] != null && Request.QueryString["Version"].ToString() != "")
				{
					qryStrModel = Request.QueryString["model"].ToString();
					qryStrVersion = Request.QueryString["version"].ToString();
					
					if( !CommonOpn.CheckId(qryStrModel) && !CommonOpn.CheckId(qryStrVersion) )
					{
						Response.Redirect("bikeversions.aspx");
					}
				}
				else
				{
                    Response.Redirect("bikeversions.aspx");
				}
			}
			else
			{
                Response.Redirect("bikeversions.aspx");
			}
			
			if ( !IsPostBack )
			{
				lblBike.Text = GetBikeName( qryStrVersion );
				BindGrid();
				BindCheckList();
			}

		} // Page_Load
		
		void btnSave_Click( object Sender, EventArgs e )
		{
			bool dataSaved = false;
			
			if ( txtColor.Text.Trim().Length > 0 )
			{
				SaveColor( txtColor.Text, txtCode.Text, txtHexCode.Text );
				dataSaved = true;
				
				txtColor.Text = "";
				txtCode.Text = "";
				txtHexCode.Text = "";
			}
			
			for ( int i = 0; i < chkModelColors.Items.Count; i++ )
			{
				if ( chkModelColors.Items[i].Selected )
				{
					string color = "", code = "", HexCode = "";
					color = chkModelColors.Items[i].Text.Split( ':' )[0];
					code = chkModelColors.Items[i].Text.Split( ':' )[1];
					HexCode = chkModelColors.Items[i].Text.Split( ':' )[2];
					
					SaveColor( color, code, HexCode );
					
					dataSaved = true;
				}
			}
			
			if ( dataSaved )
			{
				spnError.InnerHtml = "Data Saved Successfully.";
				BindGrid();
				BindCheckList();
			}
		}
		
		void SaveColor( string color, string code, string HexCode )
		{
			string sql = "";
			Database db = new Database();
					
			sql = " INSERT INTO VersionColors ( Color, Code, HexCode, BikeVersionId ) "
				+ " VALUES('" 
				+ color.Trim().Replace("'","''") + "','" 
				+ code.Trim().Replace("'","''") + "','" 
				+ HexCode.Trim().Replace("'","''") + "'," + qryStrVersion + " )";
				
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
				
		void BindGrid()
		{
			string sql = "";
						
			sql = " SELECT ID, Color, Code, HexCode "
				+ " FROM VersionColors WHERE IsActive=1 AND BikeVersionId=" + qryStrVersion;
			
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
		
		void BindCheckList()
		{
			string sql = "";
						
			sql = " SELECT DISTINCT Id, Color + ':' + Code + ':' + HexCode AS ColorValue "
				+ " FROM ModelColors WHERE IsActive=1 "
				+ " AND Color NOT IN ( SELECT Color FROM VersionColors "
				+ " WHERE IsActive=1 AND BikeVersionId=" + qryStrVersion + " ) "
				+ " AND BikeModelId=" + qryStrModel;
			
			Trace.Warn(sql);
			
			Database db = new Database();
			
			DataSet ds = new DataSet();
			SqlConnection cn = new SqlConnection( db.GetConString() );
						
			try
			{
				cn.Open();
				
				SqlDataAdapter adp = new SqlDataAdapter( sql, cn );
				adp.Fill( ds, "Categories" );				
					
				chkModelColors.DataSource = ds.Tables["Categories"];	
				chkModelColors.DataTextField = "ColorValue";
				chkModelColors.DataValueField = "ID";
				chkModelColors.DataBind();
				
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
			
			sql = "UPDATE VersionColors SET "
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
			BindCheckList();
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
			
			sql = "UPDATE VersionColors SET IsActive=0 WHERE Id=" + dtgrdColors.DataKeys[ e.Item.ItemIndex ];
			
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
			BindCheckList();
		}
		
		private string GetBikeName( string VersionId )
		{
			string bikeName = "";
			string sql = "";
			Database db = new Database();
			
			sql = "SELECT ( Ma.Name + ' ' + Mo.Name + ' ' + Ve.Name ) BikeMake"
				+ " FROM BikeMakes Ma, BikeModels Mo, BikeVersions Ve "
				+ " WHERE Ma.ID=Mo.BikeMakeId AND Mo.ID=Ve.BikeModelId "
				+ " AND Ve.Id = " + VersionId;
			
			try
			{
				SqlDataReader dr = db.SelectQry( sql );
				while ( dr.Read() )
				{
					bikeName = dr[ 0 ].ToString();
				}
				dr.Close();
			}
			catch ( SqlException ex )
			{
				Trace.Warn( ex.Message );
			}
			finally
			{
				db.CloseConnection();
			}
			return bikeName;
		} // GetBikeName
	} // class
} // namespace