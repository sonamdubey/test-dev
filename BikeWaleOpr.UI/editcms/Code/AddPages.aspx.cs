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
	public class AddPages : Page
	{
		protected Button btnSave, btnUpdate;
		protected TextBox txtPageNo, txtPageName;
		protected Label lblMessage;
		protected DataGrid dgPages;
		protected DisplayBasicInfo BasicInfo;
		protected HtmlInputHidden hdnUpdateId;
		protected EditCmsCommon EditCmsCommon;
		protected bool IsSinglePage = false;
		
		protected string basicId = "", updateId = "";
		protected override void OnInit( EventArgs e )
		{
			InitializeComponent();
		}
		
		void InitializeComponent()
		{
			base.Load += new EventHandler( Page_Load );
			btnSave.Click += new EventHandler( btnSave_Click );
			btnUpdate.Click += new EventHandler( btnUpdate_OnClick );
		}
		
		void Page_Load( object Sender, EventArgs e )
		{
            //if( HttpContext.Current.User.Identity.IsAuthenticated != true) 
            //    Response.Redirect("../users/Login.aspx?ReturnUrl=../editcms/basicinfo.aspx");
			
            //if ( Request.Cookies["Customer"] == null )
            //    Response.Redirect("../Users/Login.aspx?ReturnUrl=../editcms/basicinfo.aspx");
				
			//int pageId = 53;
            //CommonOpn op = new CommonOpn();
            //if ( !op.verifyPrivilege( pageId ) )
            //    Response.Redirect("../NotAuthorized.aspx");
			
			if ( Request["bid"] != null && Request.QueryString["bid"] != "" ) 
			{
				basicId = Request.QueryString["bid"].ToString();
				
				if ( CommonOpn.CheckId(basicId) == false )
				{
					return;
				}				
			}
			else
			{
				return;
			}
			updateId = hdnUpdateId.Value;
			if (!IsPostBack )
			{	
				CheckIsSinglePage();
				BindGrid();
			}
			EditCmsCommon.BasicId = Request.QueryString["bid"]; 
			EditCmsCommon.PageId = 5;
            EditCmsCommon.PageName = "Manage Article";
			
		} // Page_Load
					
		private void CheckIsSinglePage()
		{
			string sql = string.Empty;
			SqlDataReader dr = null;
			Database db = new Database();
			
			try
			{
				sql = " Select IsSinglePage from Con_EditCms_Category C "
					+ " Inner Join Con_EditCms_Basic B On B.CategoryId = C.Id " 
					+ " Where B.Id = @BasicId ";
				
				SqlParameter[] param = { new SqlParameter( "@BasicId", basicId ) };
				
				
				dr = db.SelectQry( sql, param );
				
				while(dr.Read())
				{
					IsSinglePage = Convert.ToBoolean(dr["IsSinglePage"].ToString());
				}
				dr.Close();
			}
			catch(SqlException err)
			{
				Trace.Warn(err.Message + err.Source);
				ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
				objErr.ConsumeError();
			} // catch SqlException
			catch(Exception err)
			{
				Trace.Warn(err.Message + err.Source);
				ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
				objErr.ConsumeError();
			}// catch Exception
			finally
			{
				if( dr != null )
					dr.Close();
				
				db.CloseConnection();
			}
		}
		
		void FillData()
		{
			string sql = "" ;
			SqlDataReader dr ;
			Database db = new Database();
			
			if ( updateId != "" )
			{
				sql = " SELECT Id, BasicId, PageName, Priority "
				+ " FROM Con_EditCms_Pages "
				+ " WHERE ID = @Id"
				+ " AND IsActive = 1";
				
				SqlParameter [] param = 
				{
					new SqlParameter("@Id", updateId)
				};
				
				Trace.Warn(sql);
				try
				{
					dr = db.SelectQry(sql, param);	
					
					if(dr.Read())
					{
						txtPageName.Text = dr["PageName"].ToString();
						txtPageNo.Text = dr["Priority"].ToString();
					}
					dr.Close();
					db.CloseConnection();
				}
				catch(Exception err)
				{
					Trace.Warn(err.Message + err.Source);
					ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
					objErr.ConsumeError();
				}
			}
		}
		
		void BindGrid()
		{
			Database db = new Database();
			DataSet ds = new DataSet(); 
			string sql = string.Empty;
			bool isNewsItem = false;
												
			sql = " SELECT Id, BasicId, PageName, Priority "
				+ " FROM Con_EditCms_Pages "
				+ " WHERE BasicId = @basicId"
				+ " AND IsActive = 1"
				+ " ORDER BY Priority ";
			SqlParameter [] param = 
			{
				new SqlParameter("@basicId", basicId)
			};
			
			Trace.Warn(sql);
					
			try
			{
				ds = db.SelectAdaptQry( sql, param );
				
				if( ds.Tables[0].Rows.Count == 1 && IsSinglePage)
				{
					isNewsItem = true;
				}
				else
				{
					if(ds.Tables[0].Rows.Count > 0)
					{
						dgPages.DataSource = ds;
						dgPages.DataBind();
					}
					else
					{
						dgPages.DataSource = null;
						dgPages.DataBind();
					}
				}				
				Trace.Warn(ds.Tables[0].Rows.Count.ToString());
			}
			catch(Exception err)
			{
				Trace.Warn(err.Message + err.Source);
				ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
				objErr.ConsumeError();
			}
			finally
			{
				db.CloseConnection();
			}
			if( isNewsItem )
			{
				Response.Redirect("/editcms/fillpages.aspx?pid=" + ds.Tables[0].Rows[0]["Id"].ToString() + "&bid=" + basicId);
			}
		}
		
		void btnSave_Click( object Sender, EventArgs e )
		{
			string status = SaveData("-1");
		
			if( status == "0")
			{
				lblMessage.Text = "Data Saved Successfully";
				ClearText();
				BindGrid();
			}
			else if(status == "-1")
			{
				lblMessage.Text = "Page already added";
			}
			else if(status == "-3")
			{
				lblMessage.Text = "Page no. already assigned";
			}
		}
		
		void btnUpdate_OnClick( object sender, EventArgs e )
		{
			string status = SaveData(updateId);
		
			if( status == "0")
			{

				lblMessage.Text = "Data Saved Successfully";
				ClearText();
				BindGrid();
			}
			else if(status == "-2")
			{
				lblMessage.Text = "Page does not exists";
			}
			else if(status == "-3")
			{
				lblMessage.Text = "Page no. already assigned";
			}
		}
		
		string SaveData(string Id)
		{
			SqlConnection con;
			SqlCommand cmd;
			SqlParameter prm;
			Database db = new Database();
			string status = "";
			
			string conStr = db.GetConString();
			
			con = new SqlConnection( conStr );
			
			try
			{
				Trace.Warn("Saving Data") ;
				
				cmd = new SqlCommand("Con_EditCms_ManagePages", con);
				cmd.CommandType = CommandType.StoredProcedure;
				
				prm = cmd.Parameters.Add("@Id", SqlDbType.BigInt);
				prm.Value = Id;
				
				prm = cmd.Parameters.Add("@BasicId", SqlDbType.BigInt);
				prm.Value = basicId;
				
				prm = cmd.Parameters.Add("@PageName", SqlDbType.VarChar,50);
				prm.Value = txtPageName.Text.Trim().ToString();
				
				prm = cmd.Parameters.Add("@Priority", SqlDbType.Int);
				prm.Value = txtPageNo.Text;
				
				prm = cmd.Parameters.Add("@IsActive", SqlDbType.Bit);
				prm.Value = 1;    //Check it
				
				prm = cmd.Parameters.Add("@UpdatedBy", SqlDbType.BigInt);
				prm.Value = CurrentUser.Id;  
				
				prm = cmd.Parameters.Add("@Status", SqlDbType.Int);
				prm.Direction = ParameterDirection.Output;
				
				con.Open();

				Trace.Warn("Execute Query");
				cmd.ExecuteNonQuery();
				con.Close();
					
				Trace.Warn(cmd.Parameters["@Status"].Value.ToString());
				if ( cmd.Parameters["@Status"].Value.ToString() != "" ) 
				status = cmd.Parameters["@Status"].Value.ToString();
				
			}
			catch(SqlException err)
			{
				Trace.Warn(err.Message + err.Source);
				ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
				objErr.ConsumeError();
			} // catch SqlException
			catch(Exception err)
			{
				Trace.Warn(err.Message + err.Source);
				ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
				objErr.ConsumeError();
			}// catch Exception
			finally
			{
				//close the connection	
			    if(con.State == ConnectionState.Open)
				{
					con.Close();
				}
			}
			return status;
		}
		
		void ClearText()
		{
			txtPageName.Text = string.Empty;
			txtPageNo.Text = string.Empty;
		}
	}
}