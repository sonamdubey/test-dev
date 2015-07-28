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
	public class BikeMakes : Page
	{
		protected HtmlGenericControl spnError;
        protected TextBox txtMake,txtMaskingName;
		protected Button btnSave;
		protected DataGrid dtgrdMembers;
        protected Label lblStatus;
				
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
		}
		
		void Page_Load( object Sender, EventArgs e )
		{
            //CommonOpn op = new CommonOpn();
			
            //if( HttpContext.Current.User.Identity.IsAuthenticated != true) 
            //        Response.Redirect("../users/Login.aspx?ReturnUrl=../Contents/CarMakes.aspx");
				
            //if ( Request.Cookies["Customer"] == null )
            //    Response.Redirect("../Users/Login.aspx?ReturnUrl=../Contents/CarMakes.aspx");
				
            //int pageId = 38;
            //if ( !op.verifyPrivilege( pageId ) )
            //    Response.Redirect("../NotAuthorized.aspx");
            lblStatus.Text = "";
			if ( !IsPostBack )
			{
				SortDirection = "";
				SortCriteria = "Name";
                			
				BindGrid();
			}
		} // Page_Load
		
		void btnSave_Click( object Sender, EventArgs e )
		{
			Page.Validate();
			if ( !Page.IsValid ) return;
			
			string sql;
			
			sql = "INSERT INTO BikeMakes( Name,MaskingName,IsDeleted,MaCreatedOn,MaUpdatedBy) "
                + " VALUES( '" + txtMake.Text.Trim().Replace("'", "''") + "','" + txtMaskingName.Text.Trim() + "', 0 , getdate(),'" + BikeWaleAuthentication.GetOprUserId() + "')";
			Database db = new Database();
			Trace.Warn("save sql : ",sql);
			try
			{
				db.InsertQry( sql );
			}
			catch( SqlException ex )	
			{
				Trace.Warn("Error", ex.Message + ex.Source);
				ErrorClass objErr = new ErrorClass(ex,Request.ServerVariables["URL"]);
				objErr.SendMail();

                // Error code Unique key constraint in the database.
                if (ex.Number == 2627)
                {
                    lblStatus.Text = "Make name or make masking name already exists. Can not insert duplicate name.";
                }
                else
                    lblStatus.Text = "";
			}
            catch(Exception ex)
            {
                Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
			BindGrid();
		}
		
		///<summary>
		///This function gets the list of the sell inquiries made according to the 
		///model
		///</summary>
		void BindGrid()
		{
			string sql = "";
			
			int pageSize = dtgrdMembers.PageSize;

            sql = " SELECT BM.ID, BM.Name,BM.MaskingName,BM.Used,BM.New, BM.Futuristic,BM.MaCreatedOn AS CreatedOn, BM.MaUpdatedOn As UpdatedOn,OU.UserName AS UpdatedBy FROM BikeMakes BM LEFT JOIN OprUsers OU ON BM.MaUpdatedBy = OU.id WHERE IsDeleted=0";
			
			if(SortCriteria != "")
                sql += " ORDER BY BM.Futuristic DESC,BM.New DESC,BM.Used DESC," + SortCriteria + " " + SortDirection; 
			
			Trace.Warn(sql);
			CommonOpn objCom = new CommonOpn();			
			try
			{
				objCom.BindGridSet( sql, dtgrdMembers, pageSize);
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
			//req1.Enabled = false;
			btnSave.Enabled = false;
		}
		
		void dtgrdMembers_Update( object sender, DataGridCommandEventArgs e )
		{
			Page.Validate();
			if ( !Page.IsValid ) return;
			
			string sql;
            CheckBox chkFuturistic = (CheckBox)e.Item.FindControl("chkFut");
            CheckBox chkUsed = (CheckBox)e.Item.FindControl("chkUsed");
            CheckBox chkNew = (CheckBox)e.Item.FindControl("chkNew");

			TextBox txt = (TextBox) e.Item.FindControl( "txtMake" );
			sql = "UPDATE BikeMakes SET "
				+ " Name='" + txt.Text.Trim().Replace("'","''") + "',"
                + " Futuristic=" + Convert.ToInt16(chkFuturistic.Checked) + ","
                + " Used = " + Convert.ToInt16(chkUsed.Checked) + ","
                + " New = " + Convert.ToInt16(chkNew.Checked) + ","
                + " MaUpdatedOn=getdate(),"
                + " MaUpdatedBy='" + BikeWaleAuthentication.GetOprUserId() + "'"
				+ " WHERE Id=" + dtgrdMembers.DataKeys[ e.Item.ItemIndex ];
			
			Database db = new Database();

            try
            {
                db.InsertQry(sql);
            }
            catch (SqlException ex)
            {
                Trace.Warn(ex.Message + ex.Source);                
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();

                Trace.Warn("number : " + ex.Number + " : error code : " + ex.ErrorCode);
                // Error code Unique key constraint in the database.
                if (ex.Number == 2627)
                {
                    lblStatus.Text = "Make name or make masking name already exists. Can not insert duplicate name";
                }
            }
            catch (Exception ex)
            {
                Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally 
            { 
                db.CloseConnection();
            }
			dtgrdMembers.EditItemIndex = -1;
			//req1.Enabled = true;
			btnSave.Enabled = true;
			BindGrid();
		}
		
		void dtgrdMembers_Cancel( object sender, DataGridCommandEventArgs e )
		{
			dtgrdMembers.EditItemIndex = -1;
			BindGrid();
			//req1.Enabled = true;
			btnSave.Enabled = true;
		}
		
		void dtgrdMembers_Delete( object sender, DataGridCommandEventArgs e )
		{
            MakeModelVersion mmv = new MakeModelVersion();
            mmv.DeleteMakeModelVersion(dtgrdMembers.DataKeys[e.Item.ItemIndex].ToString(), BikeWaleAuthentication.GetOprUserId());
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