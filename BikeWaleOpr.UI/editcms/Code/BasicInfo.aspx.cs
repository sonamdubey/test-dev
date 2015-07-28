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
using System.Text.RegularExpressions;


namespace BikeWaleOpr.EditCms
{
	public class BasicInfo : Page
	{		
		protected DropDownList ddlCategory, ddlAuthor, ddlHours, ddlMins;
        protected CheckBox chk;		
        protected HtmlInputButton btnUpdate, btnSave;
        protected TextBox txtTitle, txtAuthor, txtPhotosBy;
		protected RichTextEditor rteDescription;
		protected DateControl dtDate;
		protected string bid = string.Empty;
		protected HtmlInputHidden hdnSubCat, hdnBasicId;
        protected HtmlGenericControl subCatContainer, alertObj, divDynamicControl;
        protected CheckBox chkIsFeatured;
        //HtmlTable table1 = new HtmlTable();
        //HtmlTableRow row;
        //HtmlTableCell cell;
		
		protected DisplayBasicInfo BasicInfoControl;
		protected EditCmsCommon EditCmsCommon;
									 
		protected override void OnInit( EventArgs e )
		{
			InitializeComponent();
		}
		
		void InitializeComponent()
		{
			base.Load += new EventHandler( Page_Load );
			//btnSave.Click += new EventHandler( btnSave_Click );
            //btnUpdate.Click += new EventHandler(btnUpdate_Click);            
		}

		void Page_Load( object Sender, EventArgs e )
		{
            //CommonOpn op = new CommonOpn();

            //if( HttpContext.Current.User.Identity.IsAuthenticated != true) 
            //        Response.Redirect("../users/Login.aspx?ReturnUrl=../editcms/basicinfo.aspx");
				
            //if ( Request.Cookies["Customer"] == null )
            //    Response.Redirect("../Users/Login.aspx?ReturnUrl=../editcms/basicinfo.aspx");
					
            int pageId = 53;
            
            //if ( !op.verifyPrivilege( pageId ) )
            //    Response.Redirect("../NotAuthorized.aspx");

			if (!Page.IsPostBack)
			{
                LoadCategory();
                LoadHoursMins();
                LoadAuthors(); // autor drop down fill 

                if (Request.QueryString["bid"] != null)
                {
                    bid = CommonOpn.CheckId(Request.QueryString["bid"]) ? Request.QueryString["bid"] : "0";
                    //createOtherInfoRows();
                    //divDynamicControl.Controls.Add(table1);
                    //GetFieldData(bid);

                    LoadData(bid);
                }		               
			}

            if (Request.QueryString["bid"] != null)
            {
                EditCmsCommon.BasicId = bid;
                EditCmsCommon.pageId = pageId;
                EditCmsCommon.PageName = "Basic Information";
            }
            // dynamic genrated control for other info       
            
            /*if (divDynamicControl.Controls.Count == 0)
            {
                if (ddlCategory.SelectedValue != "0")
                {   createOtherInfoRows();
                    divDynamicControl.Controls.Add(table1);
                }
            }*/
            //alertObj.Visible = false;
		} // Page_Load       

        private bool AllowBikeSelection()
		{
            Trace.Warn("AllowBikeSelection");
            bool returnVal = true;
            Trace.Warn("ddlCategory.SelectedItem.Value : ", ddlCategory.SelectedItem.Value);
			string sql = "SELECT AllowBikeSelection FROM Con_EditCms_Category WHERE ID = " + ddlCategory.SelectedItem.Value;
			SqlDataReader dr = null;
			Database db = new Database();
			try
			{
				dr = db.SelectQry(sql);
				if (dr.Read())
				{
					returnVal = Convert.ToBoolean(dr[0]);
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
			return returnVal;
		}
		
		private void LoadData(string bid)
		{
            Trace.Warn("LoadData");
            string sql = "SELECT ID, CategoryId,Title,DisplayDate, AuthorName," +
                        "AuthorId,Description, IsFeatured FROM Con_EditCms_Basic " +                       
                        "WHERE ID = @ID";
			SqlDataReader dr = null;
			Database db = new Database();
			DateTime dateTimeVal = new DateTime();
			SqlParameter [] param = 
			{
				new SqlParameter("@ID", bid)
			};            
			try
			{
				
				dr = db.SelectQry(sql, param);            
                 if (dr.Read())
                    {
                        ddlCategory.SelectedValue = dr["CategoryId"].ToString();
                        Trace.Warn("ddlCategory.SelectedValue : " + dr["CategoryId"].ToString());
                        if (txtTitle.Text == "")
                        {
                            dateTimeVal = Convert.ToDateTime(dr["DisplayDate"].ToString());
                            txtTitle.Text = dr["Title"].ToString();
                            ddlAuthor.SelectedValue = dr["AuthorId"].ToString();
                            rteDescription.Text = dr["Description"].ToString();
                            dtDate.Value = dateTimeVal;
                            ddlHours.SelectedValue = int.Parse(dateTimeVal.ToString("HH")).ToString();
                            ddlMins.SelectedValue = int.Parse(dateTimeVal.ToString("mm")).ToString();
                            chkIsFeatured.Checked = !String.IsNullOrEmpty(dr["IsFeatured"].ToString()) ? Convert.ToBoolean(dr["IsFeatured"].ToString()) : false;
                        }
                    }                
				LoadSubCategories(bid);
			}
			catch(Exception err)
			{
                Trace.Warn(err.Message);
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
		
		private void LoadSubCategories(string bid)
		{
            Trace.Warn("LoadSubCategories");
			bool found = false;
			string sql = string.Empty;
			Database db = new Database();
			SqlDataReader dr = null;
			StringBuilder sb = new StringBuilder();
            Trace.Warn("Category = " + ddlCategory.SelectedItem.Value);
			sql = " Select Id, Name, IsNull(SubCategoryId, 0) As SubCategoryId From Con_EditCms_SubCategories A "
				+ " Left Join Con_EditCms_BasicSubCategories B On B.SubCategoryId = A.Id  And BasicId = @BasicId "
				+ " Where CategoryId = @CategoryId And IsActive = 1 Order By Name";
			
			SqlParameter[] param = { new SqlParameter( "@CategoryId", ddlCategory.SelectedItem.Value ), new SqlParameter( "@BasicId", bid )  };

            Trace.Warn("sql : " + sql);
			
			
			try
			{
				string isChecked = string.Empty;
				string idValue = string.Empty;
				string nameValue = string.Empty;
				string subCatValue = string.Empty;
                string strSubCatIds = string.Empty;
				dr = db.SelectQry( sql, param );
				
				sb.Append("<ul>");
				while( dr.Read() )
				{
					found = true;
					idValue = dr["Id"].ToString();
					nameValue = dr["Name"].ToString();
					subCatValue = dr["SubCategoryId"].ToString();
					
					if( subCatValue != "0" )
					{
						isChecked = "checked=\"checked\"";
                        if (strSubCatIds == string.Empty)
                        {
                            strSubCatIds = idValue;
                        }
                        else
                        {
                            strSubCatIds = strSubCatIds + "," + idValue;                            
                        }
                        Trace.Warn("strSubCatIds: " + strSubCatIds);
					}		
					else
					{
						isChecked = "";
					}
					sb.AppendFormat("<li><input id=\"{0}\" type=\"checkbox\" {2} name=\"chk\" /><label for=\"{0}\">{1}</label></li>", idValue, nameValue, isChecked);					
				}
                Trace.Warn("strSubCatIds: " + strSubCatIds);
                if (!hdnSubCat.Value.Contains(strSubCatIds))
                {
                    hdnSubCat.Value = strSubCatIds;
                }

				sb.Append("</ul>");
                Trace.Warn("2nd load " + hdnSubCat.Value);
				subCatContainer.InnerHtml = found == true ? sb.ToString() : "";
			} 
			catch(SqlException err)
			{
				Trace.Warn("LoadSubCatSqlErr: " + err.Message);
				ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
				objErr.SendMail();
			}
			catch(Exception err)
			{
				Trace.Warn("LoadSubCatErr: " + err.Message);
				ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
				objErr.SendMail();
			}
			finally
			{
				if( dr!=null )
					dr.Close();				
				db.CloseConnection();
			}
		}
		
		private void LoadCategory()
		{
            Trace.Warn("LoadCategory");

            string sql = "SELECT Id, Name FROM Con_EditCms_Category WHERE IsActive = 1 ORDER BY NAME";
		
			CommonOpn op = new CommonOpn();
			try
			{
				op.FillDropDown(sql, ddlCategory, "Name", "ID");
			}
			catch(SqlException err)
			{
				ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
				objErr.SendMail();
			} // catch Exception
			
			ListItem item = new ListItem("--Select--", "0");
			ddlCategory.Items.Insert(0, item);
		}        
		
		private void LoadHoursMins()
		{
			ListItem item;
			for( int i=0; i<24; ++i )
			{
				if( i<10 )
					item = new ListItem("0"+i.ToString(), i.ToString());
				else
					item = new ListItem(i.ToString(), i.ToString());
			
				ddlHours.Items.Insert(i, item);
			}
			for( int i=0; i<60; ++i )
			{
				if( i<10 )
					item = new ListItem("0"+i.ToString(), i.ToString());
				else
					item = new ListItem(i.ToString(), i.ToString());
			
				ddlMins.Items.Insert(i, item);
			}			
				
			ddlHours.SelectedValue = int.Parse(DateTime.Now.ToString("HH")).ToString();
			ddlMins.SelectedValue = int.Parse(DateTime.Now.ToString("mm")).ToString();
		}

        /* this function fill the dropdown of the author from the table OprUser join with Con_Edit_Cms */
        private void LoadAuthors()
        {
            Trace.Warn("LoadAuthors");
            string sql = "SELECT AuthorId, UserName FROM Con_EditCms_Author A "
                       + "Inner Join OprUsers B On A.AuthorId = B.Id "
                       + "Where B.IsActive = 1 Order By UserName";
            
            CommonOpn op = new CommonOpn();
            try
            {
                op.FillDropDown(sql, ddlAuthor, "UserName", "AuthorId");
            }
            catch (SqlException err)
            {
                Trace.Warn("LoadAuthorsSqlErr: " + err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                Trace.Warn("LoadAuthorsErr: " + err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception

            ListItem item = new ListItem("--Select Author--", "0");
            ddlAuthor.Items.Insert(0, item);
            ddlAuthor.SelectedValue = CurrentUser.Id;
        }

        private void SaveFieldData(string _CatFieldId, string _ValueType, string _Value,string BasicId)
        {
            Trace.Warn("Save Other Info");
            
            SqlConnection con;
            SqlCommand cmd;
            SqlParameter prm;
            Database db = new Database();

            string conStr = db.GetConString();
            con = new SqlConnection(conStr);

            try
            {
                cmd = new SqlCommand("Con_EditCms_OtherInfoSave", con);
                cmd.CommandType = CommandType.StoredProcedure;

                prm = cmd.Parameters.Add("@BasicId", SqlDbType.BigInt);
                prm.Value = BasicId;

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
                    prm.Value = _Value;
                }

                prm = cmd.Parameters.Add("@LastUpdatedBy", SqlDbType.BigInt);
                prm.Value = CurrentUser.Id;

               
                con.Open();

                cmd.ExecuteNonQuery();
            
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }     

        private void saveOtherInfo(HtmlTable tbl,string BasicId)
        {
            try
            {              
                string id = "", valType = "0", catFieldId = "0";
                for (int i = 0; i < tbl.Rows.Count; i++)
                {
                    if ((Control)tbl.Rows[i].Cells[1].Controls[0] is TextBox)
                    {
                        TextBox txt = (TextBox)tbl.Rows[i].Cells[1].Controls[0];
                        id = txt.ID;
                        string[] idSplitted = id.Split('_');
                        valType = idSplitted[0];
                        catFieldId = idSplitted[1];
                        Trace.Warn("valeue:- " + catFieldId + "," + valType + "," + txt.Text.Trim() + "," + BasicId);
                        SaveFieldData(catFieldId, valType, txt.Text.Trim(), BasicId);
                    }
                    else if ((Control)tbl.Rows[i].Cells[1].Controls[0] is CheckBox)
                    {
                        CheckBox chk = (CheckBox)tbl.Rows[i].Cells[1].Controls[0];
                        id = chk.ID;
                        string[] idSplitted = id.Split('_');
                        valType = idSplitted[0];
                        catFieldId = idSplitted[1];
                        if (chk.Checked)
                        {
                            SaveFieldData(catFieldId, valType, "1", BasicId);
                        }
                        else
                        {
                            SaveFieldData(catFieldId, valType, "0", BasicId);
                        }
                    }
                    else if ((Control)tbl.Rows[i].Cells[2].Controls[0] is DateControl)
                    {
                        DateControl dt = (DateControl)tbl.Rows[i].Cells[1].Controls[0];
                        id = dt.ID;
                        string[] idSplitted = id.Split('_');
                        valType = idSplitted[0];
                        catFieldId = idSplitted[1];
                        SaveFieldData(catFieldId, valType, dt.Value.ToString(), BasicId);
                    }
                }
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
            }
        }
	} // class
} // namespace