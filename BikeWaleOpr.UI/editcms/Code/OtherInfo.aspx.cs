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
	public class OtherInfo : Page
	{		
		protected DateControl dtDate;
		protected DisplayBasicInfo BasicInfo;
		protected Label lblIsSaved;
		protected EditCmsCommon EditCmsCommon;
		HtmlGenericControl myDiv = new HtmlGenericControl();
		
		HtmlTable table1 = new HtmlTable();	
		HtmlTableRow row;
		HtmlTableCell cell;
		
		protected string cId = "-1", basicId;
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
		
			table1.Border = 1;
			table1.ID = "tblOtherInfo";
			table1.CellPadding = 3;
			table1.CellSpacing = 3;
			table1.Border = 0;
		
			foreach(Control ctrl in Page.Controls)
   			{
				if(ctrl is HtmlForm && ctrl.HasControls() == true)
				{ 
					myDiv = (HtmlGenericControl)ctrl.FindControl("divAshish");
				}
			}	
			
			GetCategoryFields(Request.QueryString["bid"].ToString());
			GetFieldData();
		
			EditCmsCommon.BasicId = Request.QueryString["bid"]; 
			EditCmsCommon.PageId = 3;
            EditCmsCommon.PageName = "Extended Information";
			
		} // Page_Load
		
		private void GetFieldData()
		{
			string sql = "SELECT * FROM Con_EditCms_OtherInfo WHERE BasicId = " + Request.QueryString["bid"].ToString();
			SqlDataReader dr = null;
			Database db = new Database();
			try
			{
				dr = db.SelectQry(sql);
				
				while(dr.Read())
				{
					if (dr["ValueType"].ToString() == "1")
					{
						CheckBox chk = (CheckBox)this.FindControl(dr["ValueType"].ToString() + "_" + dr["CategoryFieldId"].ToString());	
						chk.Checked = Convert.ToBoolean(dr["BooleanValue"]);
					}
					else if (dr["ValueType"].ToString() == "5")
					{
						DateControl dt = (DateControl)this.FindControl(dr["ValueType"].ToString() + "_" + dr["CategoryFieldId"].ToString());	
						dt.Value = Convert.ToDateTime(dr["DateTimeValue"].ToString());
					}
					else
					{
						TextBox txt = (TextBox)this.FindControl(dr["ValueType"].ToString() + "_" + dr["CategoryFieldId"].ToString());
						switch (dr["ValueType"].ToString())
						{
							case "2"	:	txt.Text = dr["NumericValue"].ToString();
											break;
							case "3"	:	txt.Text = dr["DecimalValue"].ToString();
											break;				
							case "4"	:	txt.Text = dr["TextValue"].ToString();
											break;								
							case "5"	:	if (dr["DateTimeValue"].ToString() != "")
											{
												txt.Text = Convert.ToDateTime(dr["DateTimeValue"].ToString()).ToString("dd/MM/yyyy");
											}
											break;		
							default		:	break;														
						}
					}
					lblIsSaved.Text = "1";
				}
			}
			catch(Exception err)
			{
				ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
				objErr.SendMail();
			}
			finally
			{
				if( dr != null )
					dr.Close();
				db.CloseConnection();
			}
		}
		
		private void GetCategoryFields(string bid)
		{
			string sql  = " SELECT	CF.*"
						+ " FROM"	
						+ " Con_EditCms_Basic B, Con_EditCms_CategoryFields CF"
						+ " WHERE	"
						+ " B.CategoryId = CF.CategoryId"
						+ " AND B.ID =  " + Request.QueryString["bid"].ToString() + ""
						+ " AND CF.IsActive = 1"
						+ " ORDER BY CF.Priority ";
						
			SqlDataReader dr = null;
			Database db = new Database();
			try
			{
				dr = db.SelectQry(sql);

				while (dr.Read())
				{
					AddTableRows(dr["FieldName"].ToString(), dr["ValueType"].ToString(), dr["Id"].ToString());
					
				}
				AddButtonRow();
				AddLabelRow();
				myDiv.Controls.Add(table1);
			}
			catch(Exception err)
			{
				ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
				objErr.SendMail();
			}
			finally
			{
				if( dr != null )
					dr.Close();
				db.CloseConnection();
			}
		}
		
		private void AddTableRows(string fieldName, string valueType, string catFieldId)
		{
			row = new HtmlTableRow();
			for (int j=1; j<=3; j++)
			{
				if (j == 1)
				{
					AddLabelCell(fieldName);
				}
				else if (j == 2)
				{
					cell = new HtmlTableCell();
					cell.InnerHtml = ":";
					row.Cells.Add(cell);
				}
				else
				{
					AddControlCell(fieldName, valueType, catFieldId);
				}	
			}

			table1.Rows.Add(row);	
		}
		
		private void AddControlCell(string fieldName, string valueType, string catFieldId)
		{
			if (valueType == "1")
			{
				CheckBox chk = new CheckBox();
				//chk.ID = fieldName.ToString().Replace(" ","_") + "_" + valueType + "_" + catFieldId;
				chk.ID = valueType + "_" + catFieldId;
				cell = new HtmlTableCell();
				cell.Controls.Add(chk);
				row.Cells.Add(cell);
			}
			else if (valueType == "5")
			{
				dtDate = (DateControl)LoadControl("~/Controls/DateControl.ascx");
				dtDate.ID = valueType + "_" + catFieldId;
				cell = new HtmlTableCell();
				cell.Controls.Add(dtDate);
				row.Cells.Add(cell);
			}
			else
			{
				TextBox txt = new TextBox();
				//txt.ID = fieldName.ToString().Replace(" ","_") + "_" + valueType + "_" + catFieldId;	
				txt.ID = valueType + "_" + catFieldId;
				txt.Columns = 100;
				if (valueType == "2")
				{
					txt.Attributes.Add("onkeydown","javascript:CheckNumeric(event, this);");
				}	
				else if (valueType == "3")
				{
					txt.Attributes.Add("onkeydown","javascript:CheckDecimal(event, this);");
				}
				cell = new HtmlTableCell();
				cell.Controls.Add(txt);
				row.Cells.Add(cell);
			}
		}
		
		private void AddButtonRow()
		{
			row = new HtmlTableRow();
			cell = new HtmlTableCell();
			cell.InnerHtml = "";
			row.Cells.Add(cell);	
			cell = new HtmlTableCell();
			cell.InnerHtml = "";
			row.Cells.Add(cell);	
			cell = new HtmlTableCell();
			Button btn = new Button();
			btn.Text = "Save";
			btn.Click += new System.EventHandler(btnSave_Click);
			cell.Controls.Add(btn);
			row.Cells.Add(cell);	
			table1.Rows.Add(row);
		}
		
		private void AddLabelRow()
		{
			row = new HtmlTableRow();
			cell = new HtmlTableCell();
			cell.InnerHtml = "";
			row.Cells.Add(cell);	
			cell = new HtmlTableCell();
			cell.InnerHtml = "";
			row.Cells.Add(cell);	
			cell = new HtmlTableCell();
			Label lbl = new Label();
			lbl.Text = "Data saved successfully";
			lbl.CssClass = "errMessage";
			lbl.Visible = false;
			//btn.Click += new System.EventHandler(btnSave_Click);
			cell.Controls.Add(lbl);
			row.Cells.Add(cell);	
			table1.Rows.Add(row);
		}
		
		private void AddLabelCell(string fieldName)
		{
			cell = new HtmlTableCell();
			cell.InnerHtml = fieldName;
			row.Cells.Add(cell);	
		}
		
		void btnSave_Click( object Sender, EventArgs e )
		{
			HtmlTable tbl = new HtmlTable();	
			tbl = (HtmlTable)this.FindControl("tblOtherInfo");
           
			string id="", valType = "0", catFieldId = "0";
			for (int i=0; i< tbl.Rows.Count-2; i++)
			{
				if((Control)tbl.Rows[i].Cells[2].Controls[0] is TextBox)
				{
					TextBox txt = (TextBox)tbl.Rows[i].Cells[2].Controls[0];
					id = txt.ID;
					string[] idSplitted = id.Split('_');
					valType = idSplitted[0];
					catFieldId = idSplitted[1];
					SaveFieldData(catFieldId, valType, txt.Text.Trim());
				}
				else if((Control)tbl.Rows[i].Cells[2].Controls[0] is CheckBox)
				{
					CheckBox chk = (CheckBox)tbl.Rows[i].Cells[2].Controls[0];
					id = chk.ID;
					string[] idSplitted = id.Split('_');
					valType = idSplitted[0];
					catFieldId = idSplitted[1];
					if (chk.Checked)
					{
						SaveFieldData(catFieldId, valType, "1");
					}
					else
					{
						SaveFieldData(catFieldId, valType, "0");
					}	
				}
				else if((Control)tbl.Rows[i].Cells[2].Controls[0] is DateControl)
				{
					DateControl dt = (DateControl)tbl.Rows[i].Cells[2].Controls[0];
					id = dt.ID;
					string[] idSplitted = id.Split('_');
					valType = idSplitted[0];
					catFieldId = idSplitted[1];
					SaveFieldData(catFieldId, valType, dt.Value.ToString());
				}
			}
			lblIsSaved.Text = "1";

			Label lbl = (Label)tbl.Rows[tbl.Rows.Count-1].Cells[2].Controls[0];
			lbl.Visible = true;
			Response.Redirect("createalbum.aspx?bid=" + Request.QueryString["bid"].ToString());
		}
		
		private void SaveFieldData(string _CatFieldId, string _ValueType, string _Value)
		{
			SqlConnection con;
			SqlCommand cmd;
			SqlParameter prm;
			Database db = new Database();

			string conStr = db.GetConString();
			con = new SqlConnection( conStr );
			
			try
			{
				cmd = new SqlCommand("Con_EditCms_OtherInfoSave", con);
				cmd.CommandType = CommandType.StoredProcedure;
				
				prm = cmd.Parameters.Add("@BasicId", SqlDbType.BigInt);
				prm.Value = Request.QueryString["bid"];
				
				prm = cmd.Parameters.Add("@CategoryFieldId", SqlDbType.BigInt);
				prm.Value = _CatFieldId;
				
				prm = cmd.Parameters.Add("@ValueType", SqlDbType.BigInt);
				prm.Value = _ValueType;
				
				if (_ValueType == "1")
				{
					prm = cmd.Parameters.Add("@BooleanValue", SqlDbType.Bit);
					if (_Value == "1")
						prm.Value = true;
					else
						prm.Value = false;	
				}
				else if (_ValueType == "2" && _Value.Trim() != "")
				{
					prm = cmd.Parameters.Add("@NumericValue", SqlDbType.BigInt);
					prm.Value = _Value;	
				}
				else if (_ValueType == "3" && _Value.Trim() != "")
				{
					prm = cmd.Parameters.Add("@DecimalValue", SqlDbType.Decimal);
					prm.Value = _Value;	
				}
				else if (_ValueType == "4" && _Value.Trim() != "")
				{
					prm = cmd.Parameters.Add("@TextValue", SqlDbType.VarChar, 250);
					prm.Value = _Value;	
				}
				else if (_ValueType == "5" && _Value.Trim() != "")
				{
					prm = cmd.Parameters.Add("@DateTimeValue", SqlDbType.DateTime);
					//string[] splittedDateValue = _Value.Split('/');
					//prm.Value = splittedDateValue[2] + "-" + splittedDateValue[1] + "-" + splittedDateValue[0];	
					prm.Value = _Value;
				}
				
				prm = cmd.Parameters.Add("@LastUpdatedBy", SqlDbType.BigInt);
				prm.Value = CurrentUser.Id;
				
				con.Open();

				cmd.ExecuteNonQuery();
			}
			catch(Exception err)
			{
				ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
				objErr.SendMail();
			}
			finally
			{
			    if(con.State == ConnectionState.Open)
				{
					con.Close();
				}
			}
		}
	} // class
} // namespace