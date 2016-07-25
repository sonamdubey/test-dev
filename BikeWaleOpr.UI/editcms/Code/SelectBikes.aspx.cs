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
using Ajax;
using System.Text.RegularExpressions;

namespace BikeWaleOpr.EditCms
{
	public class SelectBikes : Page
	{
		protected Button btnAdd, btnContinue, btnSaveTags;
		protected TextBox txtTags;
		protected DropDownList ddlMake, ddlModel, ddlVersion;
		protected HtmlInputHidden hdn_selModel, hdn_selVersion, hdn_selModelName, hdn_selVersionName, hdnMaxBikeSel, hdnMinBikeSel , hdnVersionSelection;
		protected DataGrid dtSelBikes;
		protected DisplayBasicInfo BasicInfo;
		protected EditCmsCommon EditCmsCommon;
		protected HtmlGenericControl divBikeSelection, lblTags;
		
		protected string basicId = "", catId = "", catName = "";

		protected override void OnInit( EventArgs e )
		{
			InitializeComponent();			
		}
		
		void InitializeComponent()
		{
			base.Load += new EventHandler( Page_Load );
			btnAdd.Click += new EventHandler( btnAdd_Click );
			btnSaveTags.Click += new EventHandler( btnSaveTags_Click );
			dtSelBikes.DeleteCommand += new DataGridCommandEventHandler( dtSelBikes_Delete );	
		}
		
		public string SelectedModel
		{
			get
			{
				if(Request.Form["ddlModel"] != null && Request.Form["ddlModel"].ToString() != "")
					return Request.Form["ddlModel"].ToString();
				else
					return "-1";
			}
		}
		
		public string ModelContents
		{
			get
			{
				if(Request.Form["hdn_drpModel"] != null && Request.Form["hdn_drpModel"].ToString() != "")
					return Request.Form["hdn_drpModel"].ToString();
				else
					return "";
			}
		}
		
		public string SelectedVersion
		{
			get
			{
				if(Request.Form["ddlVersion"] != null && Request.Form["ddlVersion"].ToString() != "")
					return Request.Form["ddlVersion"].ToString();
				else
					return "-1";
			}
		}
		
		public string VersionContents
		{
			get
			{
				if(Request.Form["hdn_drpVersion"] != null && Request.Form["hdn_drpVersion"].ToString() != "")
					return Request.Form["hdn_drpVersion"].ToString();
				else
					return "";
			}
		}
		
		void Page_Load( object Sender, EventArgs e )
		{
			
			Ajax.Utility.RegisterTypeForAjax(typeof(BikeWaleOpr.AjaxFunctions));

            if (Request.QueryString["bid"] != null && Request.QueryString["bid"].ToString() != "")
            {
                basicId = Request.QueryString["bid"].ToString();
            }
				
			if ( !IsPostBack )
			{
                //if( HttpContext.Current.User.Identity.IsAuthenticated != true) 
                //    Response.Redirect("../users/Login.aspx?ReturnUrl=../editcms/basicinfo.aspx");
				
                //if ( Request.Cookies["Customer"] == null )
                //    Response.Redirect("../Users/Login.aspx?ReturnUrl=../editcms/basicinfo.aspx");
					
                //int pageId = 53;
                //CommonOpn op = new CommonOpn();
                //if ( !op.verifyPrivilege( pageId ) )
                //    Response.Redirect("../NotAuthorized.aspx");

                BikeWaleOpr.BindControls.BindAllMakes(ddlMake);
                ddlModel.Items.Insert(0,new ListItem("--Select--","-1"));
				ddlVersion.Items.Insert(0,new ListItem("--Select--","-1"));
                Trace.Warn("model and version bind");
				GetValidationData();
                Trace.Warn("validation data get");
				BindGrid();
                Trace.Warn("bind grid done");
				GetTags();
                Trace.Warn("tags get");
		        Trace.Warn("basicId : ",basicId);
			}
			else
			{
                BikeWaleOpr.AjaxFunctions aj = new BikeWaleOpr.AjaxFunctions();
				aj.UpdateContents(ddlModel, ModelContents, SelectedModel);
				aj.UpdateContents(ddlVersion, VersionContents, SelectedVersion);
			}
			EditCmsCommon.BasicId = Request.QueryString["bid"]; 
			EditCmsCommon.PageId = 2;
            EditCmsCommon.PageName = "Tag Bike(s)";
		}
		
		void GetValidationData()
		{
            throw new Exception("Method not used/commented");

            //Database db = new Database();
            //SqlDataReader dr = null ;
            //string sql;
			
            //sql = " SELECT CC.MaxBikeSelection, CC.MinBikeSelection, CC.VersionSelection, CB.CategoryId, CC.Name"
            //    + " FROM Con_EditCms_Basic CB, Con_EditCms_Category CC WHERE CC.Id = CB.CategoryId AND CB.Id = @basicId";

            //SqlParameter [] param = 
            //{
            //    new SqlParameter("@basicId", basicId)
            //};
            //Trace.Warn("GetValidationData sql : ", sql);
            //try
            //{
            //    dr = db.SelectQry( sql, param );
            //    if(dr.Read())
            //    {
            //        catId = dr["CategoryId"].ToString();
            //        catName = dr["Name"].ToString();
            //        hdnMinBikeSel.Value = dr["MinBikeSelection"].ToString();
            //        hdnMaxBikeSel.Value = dr["MaxBikeSelection"].ToString();
            //        hdnVersionSelection.Value = dr["VersionSelection"].ToString();
            //    }
            //}
            //catch(SqlException err)
            //{
            //    Trace.Warn(err.Message + err.Source);
            //    ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
            //    objErr.ConsumeError();
            //}// catch Exception
            //finally
            //{
            //    if( dr != null )
            //        dr.Close();
            //    db.CloseConnection();
            //}
		}
		
		void BindGrid()
		{
            throw new Exception("Method not used/commented");

            //Database db = new Database();
            //DataSet ds = new DataSet(); 
            //string sql;

            //sql = " SELECT CC.MakeId AS MakeId,"
            //      + " CC.ModelId AS ModelId,"
            //      + " CC.VersionId AS VersionId,"
            //      + " (Select Name FROM BikeMakes WHERE ID = CC.MakeId) As MakeName,"
            //      + " (Select Name FROM BikeModels WHERE ID = CC.ModelId) AS ModelName,"
            //      + " ISNULL((Select Name FROM BikeVersions WHERE ID = CC.VersionId),'') AS VersionName,"
            //      + " CC.Id As Id"
            //      + " FROM Con_EditCms_Bikes CC"
            //      + " WHERE CC.BasicId = @basicId"
            //      + " AND IsActive = 1";
            //Trace.Warn(sql);	
            //SqlParameter [] param = 
            //{
            //    new SqlParameter("@basicId", basicId)
            //};
            //try
            //{
            //    ds = db.SelectAdaptQry( sql, param );
            //    if(ds.Tables[0].Rows.Count > 0)
            //    {
            //        dtSelBikes.DataSource = ds;
            //        dtSelBikes.DataBind();
            //        if(ds.Tables[0].Rows.Count < int.Parse(hdnMinBikeSel.Value))
            //        {	
            //            btnAdd.Enabled = true;
            //            btnContinue.Visible = false;
            //            divBikeSelection.Style.Add("display","block");
            //        }
            //        else if(ds.Tables[0].Rows.Count >= int.Parse(hdnMinBikeSel.Value) && ds.Tables[0].Rows.Count < int.Parse(hdnMaxBikeSel.Value))
            //        {
            //            btnAdd.Enabled = true;
            //            btnContinue.Visible = true;
            //            divBikeSelection.Style.Add("display","none");
            //        }
            //        else
            //        {
            //            btnAdd.Enabled = false;
            //            btnContinue.Visible = true;
            //            divBikeSelection.Style.Add("display","none");
            //        }
            //    }
            //    else 
            //    {
            //        if(ds.Tables[0].Rows.Count == int.Parse(hdnMinBikeSel.Value) && ds.Tables[0].Rows.Count == int.Parse(hdnMaxBikeSel.Value))
            //        {
            //            btnAdd.Enabled = false;
            //            btnContinue.Visible = true;
            //            divBikeSelection.Style.Add("display","none");
            //        }
            //        else
            //        {
            //            btnAdd.Enabled = true;
            //            btnContinue.Visible = false;
            //            divBikeSelection.Style.Add("display","block");
            //        }
            //        dtSelBikes.DataSource = null;
            //        dtSelBikes.DataBind();
            //    }
            //}
            //catch(Exception err)
            //{
            //    Trace.Warn(err.Message + err.Source);
            //    ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
            //    objErr.ConsumeError();
            //}// catch Exception
            //finally
            //{
            //    db.CloseConnection();
            //    ddlMake.SelectedIndex = 0;
            //    ddlModel.SelectedIndex = 0;
            //    ddlVersion.SelectedIndex = 0;
            //}
		}

		void btnAdd_Click( object Sender, EventArgs e)
		{
            throw new Exception("Method not used/commented");

            //SqlConnection con;
            //SqlCommand cmd;
            //SqlParameter prm;
            //Database db = new Database();
			
            //string conStr = db.GetConString();
			
            //con = new SqlConnection( conStr );
			
            //try
            //{
            //    Trace.Warn("Saving Data") ;
				
            //    cmd = new SqlCommand("Con_EditCms_Bikes_AddDelete", con);
            //    cmd.CommandType = CommandType.StoredProcedure;
				
            //    prm = cmd.Parameters.Add("@BasicId", SqlDbType.BigInt);
            //    prm.Value = basicId;
				
            //    prm = cmd.Parameters.Add("@MakeId", SqlDbType.BigInt);
            //    prm.Value = ddlMake.Items[ddlMake.SelectedIndex].Value;
				
            //    prm = cmd.Parameters.Add("@ModelId", SqlDbType.BigInt);
            //    prm.Value = hdn_selModel.Value;
				
            //    prm = cmd.Parameters.Add("@VersionId", SqlDbType.BigInt);
            //    prm.Value = int.Parse(hdn_selVersion.Value) <= 0 ? "-1" : hdn_selVersion.Value;  
				
            //    prm = cmd.Parameters.Add("@UpdatedBy", SqlDbType.BigInt);
            //    prm.Value = CurrentUser.Id;  
				
            //    con.Open();

            //    Trace.Warn("Execute Query");
            //    cmd.ExecuteNonQuery();
            //    con.Close();
								
            //    string makeVal = ddlMake.SelectedItem.Text;
            //    string modelVal = hdn_selModelName.Value;
            //    string versionVal = int.Parse(hdn_selVersion.Value) <= 0 ? "" : hdn_selVersionName.Value;
				
            //    string tagsString = string.Empty;
				
            //    tagsString = txtTags.Text;
				
            //    if( tagsString != string.Empty )
            //    {	
            //        /*txtTags.Text += ", " + ddlMake.SelectedItem.Text + ", " + hdn_selModelName.Value + ", " + 
            //                                ddlMake.SelectedItem.Text + " " + hdn_selModelName.Value + ", " + 
            //                                    hdn_selModelName.Value + " " + versionVal;*/
												
            //        tagsString += ", ";
            //    }
				
            //    if( modelVal == string.Empty )
            //    {
            //        tagsString += makeVal;
            //    }
            //    else
            //    {
            //        tagsString += makeVal + ", " + modelVal + ", " + makeVal + " " + modelVal;
            //    }
				
            //    if( versionVal != string.Empty )
            //    {
            //        tagsString += ", " + modelVal + " " + versionVal;
            //    }
				
            //    txtTags.Text = tagsString;
            //    lblTags.InnerHtml = "Tags have not been saved! Please click the \"Save Tags\" button to save the tags.";
				
            //    BindGrid();			
            //}
            //catch(SqlException err)
            //{
            //    Trace.Warn(err.Message + err.Source);
            //    ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
            //    objErr.ConsumeError();
            //}// catch Exception
            //catch(Exception err)
            //{
            //    Trace.Warn(err.Message + err.Source);
            //    ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
            //    objErr.ConsumeError();
            //}// catch Exception
            //finally
            //{
            //    //close the connection	
            //    if(con.State == ConnectionState.Open)
            //    {
            //        con.Close();
            //    }
				
            //}
		}
		
		void dtSelBikes_Delete( object sender, DataGridCommandEventArgs e )
		{
            throw new Exception("Method not used/commented");

            //SqlConnection con;
            //SqlCommand cmd;
            //SqlParameter prm;
            //Database db = new Database();
			
            //string conStr = db.GetConString();
			
            //con = new SqlConnection( conStr );
			
            //try
            //{
            //    Trace.Warn("Saving Data") ;
				
            //    cmd = new SqlCommand("Con_EditCms_Bikes_AddDelete", con);
            //    cmd.CommandType = CommandType.StoredProcedure;
				
            //    prm = cmd.Parameters.Add("@Id", SqlDbType.BigInt);
            //    prm.Value = dtSelBikes.DataKeys[ e.Item.ItemIndex ];
				
            //    prm = cmd.Parameters.Add("@UpdatedBy", SqlDbType.BigInt);
            //    prm.Value = CurrentUser.Id;
				
            //    con.Open();

            //    Trace.Warn("Execute Query");
            //    cmd.ExecuteNonQuery();
            //    con.Close();
				
            //    BindGrid();
            //}
            //catch(SqlException err)
            //{
            //    Trace.Warn(err.Message + err.Source);
            //    ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
            //    objErr.ConsumeError();
            //}// catch Exception
            //catch(Exception err)
            //{
            //    Trace.Warn(err.Message + err.Source);
            //    ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
            //    objErr.ConsumeError();
            //}// catch Exception
            //finally
            //{
            //    //close the connection	
            //    if(con.State == ConnectionState.Open)
            //    {
            //        con.Close();
            //    }
            //}
		}
		
		void btnSaveTags_Click( object sender, EventArgs e )
		{
            throw new Exception("Method not used/commented");

            //SqlConnection con;
            //SqlCommand cmd;			
            //Database db = new Database();
			
            //string tags = string.Empty;
            //string slugs = string.Empty;
			
            //string conStr = db.GetConString();
			
            //con = new SqlConnection( conStr );
			
            //try
            //{
            //    Trace.Warn("Saving Data") ;
				
            //    tags = txtTags.Text.Trim();
				
            //    slugs = GetSlugs( tags );
				
            //    cmd = new SqlCommand("Con_EditCms_Tags_Save", con);
            //    cmd.CommandType = CommandType.StoredProcedure;
				
            //    cmd.Parameters.Add("@BasicId", SqlDbType.BigInt).Value = basicId;
            //    cmd.Parameters.Add("@String", SqlDbType.VarChar, 8000).Value = tags;
            //    cmd.Parameters.Add("@Delimiter", SqlDbType.Char, 1).Value = ',';				
            //    cmd.Parameters.Add("@LastUpdatedBy", SqlDbType.BigInt).Value = CurrentUser.Id;
            //    con.Open();

            //    Trace.Warn("Execute Query");
				
            //    int num = cmd.ExecuteNonQuery();
				
            //    if( num > 0 )
            //        lblTags.InnerText = "Tags Saved Successfully";
            //    else
            //        lblTags.InnerText = "Could not save Tags";
            //    con.Close();
				
            //    Trace.Warn("tags = " + tags);
            //    Trace.Warn(slugs);
				
            //    //BindGrid();
            //}
            //catch(SqlException err)
            //{
            //    Trace.Warn(err.Message + err.Source);
            //    ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}// catch Exception
            //catch(Exception err)
            //{
            //    Trace.Warn(err.Message + err.Source);
            //    ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}// catch Exception
            //finally
            //{
            //    //close the connection	
            //    if(con.State == ConnectionState.Open)
            //    {
            //        con.Close();
            //    }
            //}
		}
		
		private string GetSlugs( string tags )
		{
			string strSlug = string.Empty;
			string strDelimit = string.Empty;
			string[] slugs = tags.Split(',');
			
			for( int i=0; i<slugs.Length; ++i)
			{
				strDelimit = strSlug == string.Empty ? "" : ",";
				strSlug += strDelimit + Regex.Replace(Regex.Replace(slugs[i].Trim(), @"[^a-zA-Z 0-9]", string.Empty), @"\s+"," ").Replace(" ", "-").ToLower();				
			}	
			
			return strSlug;
		}
		
		void GetTags()
		{
            throw new Exception("Method not used/commented");
            //string sql = string.Empty;
            //Database db = new Database();
            //string strTags = string.Empty;
            //SqlDataReader dr = null;
            //sql = "Select Tag From Con_EditCms_Tags cet Inner Join Con_EditCms_BasicTags ceb On ceb.TagId = cet.Id Where ceb.BasicId = @BasicId";
			
            //SqlParameter [] param = 
            //{
            //    new SqlParameter("@BasicId", basicId)
            //};
			
            //Trace.Warn("sql = " + sql);

            //try
            //{

            //    dr = db.SelectQry(sql, param);

            //    while (dr.Read())
            //    {
            //        if (strTags == string.Empty)
            //            strTags = dr["Tag"].ToString();
            //        else
            //            strTags += ", " + dr["Tag"].ToString();
            //        Trace.Warn("Tags = " + strTags);
            //    }


            //    txtTags.Text = strTags;
            //}
            //catch (SqlException err)
            //{
            //    Trace.Warn(err.Message + err.Source);
            //    ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}
            //catch (Exception err)
            //{
            //    Trace.Warn(err.Message + err.Source);
            //    ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}
            //finally
            //{
            //    if (dr != null)
            //        dr.Close();

            //    db.CloseConnection();
            //}
		}		
	}
}	