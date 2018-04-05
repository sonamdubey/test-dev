using BikeWaleOpr.Common;
using MySql.CoreDAL;
/*******************************************************************************************************
IN THIS CLASS THE NEW MEMBEERS WHO HAVE REQUESTED FOR REGISTRATION ARE SHOWN
*******************************************************************************************************/
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

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

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
            btnSave.Click += new EventHandler(btnSave_Click);
            dtgrdColors.EditCommand += new DataGridCommandEventHandler(dtgrdColors_Edit);
            dtgrdColors.UpdateCommand += new DataGridCommandEventHandler(dtgrdColors_Update);
            dtgrdColors.CancelCommand += new DataGridCommandEventHandler(dtgrdColors_Cancel);
            dtgrdColors.DeleteCommand += new DataGridCommandEventHandler(dtgrdColors_Delete);
        }

        void Page_Load(object Sender, EventArgs e)
        {

            if (Request.QueryString["Model"] != null && Request.QueryString["Model"].ToString() != "")
            {
                if (Request.QueryString["Version"] != null && Request.QueryString["Version"].ToString() != "")
                {
                    qryStrModel = Request.QueryString["model"].ToString();
                    qryStrVersion = Request.QueryString["version"].ToString();

                    if (!CommonOpn.CheckId(qryStrModel) && !CommonOpn.CheckId(qryStrVersion))
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

            if (!IsPostBack)
            {
                lblBike.Text = GetBikeName(qryStrVersion);
                BindGrid();
                BindCheckList();
            }

        } // Page_Load

        void btnSave_Click(object Sender, EventArgs e)
        {
            bool dataSaved = false;

            if (txtColor.Text.Trim().Length > 0)
            {
                SaveColor(txtColor.Text, txtCode.Text, txtHexCode.Text);
                dataSaved = true;

                txtColor.Text = "";
                txtCode.Text = "";
                txtHexCode.Text = "";
            }

            for (int i = 0; i < chkModelColors.Items.Count; i++)
            {
                if (chkModelColors.Items[i].Selected)
                {
                    string color = "", code = "", HexCode = "";
                    color = chkModelColors.Items[i].Text.Split(':')[0];
                    code = chkModelColors.Items[i].Text.Split(':')[1];
                    HexCode = chkModelColors.Items[i].Text.Split(':')[2];

                    SaveColor(color, code, HexCode);

                    dataSaved = true;
                }
            }

            if (dataSaved)
            {
                spnError.InnerHtml = "Data Saved Successfully.";
                BindGrid();
                BindCheckList();
            }
        }

        void SaveColor(string color, string code, string HexCode)
        {
            string sql = "";


            sql = string.Format(" insert into versioncolors ( color, code, hexcode, bikeversionid ,isactive)	values('{0}','{1}','{2}','{3}',1)", color.Trim().Replace("'", "''"), code.Trim().Replace("'", "''"), HexCode.Trim().Replace("'", "''"), qryStrVersion);

            try
            {
                MySqlDatabase.InsertQuery(sql, ConnectionType.MasterDatabase);
            }
            catch (SqlException err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);
                
            }
        }

        void BindGrid()
        {
            string sql = "";

            sql = " SELECT ID, Color, Code, HexCode   from versioncolors where isactive=1 and bikeversionid=" + qryStrVersion;

            try
            {
                using (DataSet ds = MySqlDatabase.SelectAdapterQuery(sql, ConnectionType.ReadOnly))
                {
                    if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                    {
                        dtgrdColors.DataSource = ds.Tables[0];
                        dtgrdColors.DataBind();
                    }
                }

            }
            catch (Exception err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);
                
            }
        }

        void BindCheckList()
        {
            string sql = "";

            sql = string.Format(@" select distinct Id, concat(color,':' ,code ,':' , hexcode) as ColorValue 
				from modelcolors where isactive=1 
				and color not in ( select color from versioncolors
				where isactive=1 and bikeversionid={0} ) and bikemodelid={1}", qryStrVersion, qryStrModel);

            try
            {
                using (DataSet ds = MySqlDatabase.SelectAdapterQuery(sql, ConnectionType.ReadOnly))
                {
                    if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                    {
                        chkModelColors.DataSource = ds.Tables[0];
                        chkModelColors.DataTextField = "ColorValue";
                        chkModelColors.DataValueField = "ID";
                        chkModelColors.DataBind();
                    }
                }

            }
            catch (Exception err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);
                
            }
        }

        void dtgrdColors_Edit(object sender, DataGridCommandEventArgs e)
        {
            dtgrdColors.EditItemIndex = e.Item.ItemIndex;
            BindGrid();
            btnSave.Enabled = false;
        }

        void dtgrdColors_Update(object sender, DataGridCommandEventArgs e)
        {
            Page.Validate();
            if (!Page.IsValid) return;

            string sql;

            TextBox txtCol = (TextBox)e.Item.FindControl("txtColor");
            TextBox txtCod = (TextBox)e.Item.FindControl("txtCode");
            TextBox txtHCod = (TextBox)e.Item.FindControl("txtHexCode");

            sql = string.Format("update versioncolors set color='{0}',code='{1}',hexcode='{2}' where id={3} ", txtCol.Text.Trim().Replace("'", "''"), txtCod.Text.Trim().Replace("'", "''"), txtHCod.Text.Trim().Replace("'", "''"), dtgrdColors.DataKeys[e.Item.ItemIndex]);

            try
            {
                MySqlDatabase.InsertQuery(sql, ConnectionType.MasterDatabase);
            }
            catch (SqlException ex)
            {
                Trace.Warn(ex.Message + ex.Source);
                ErrorClass.LogError(ex, Request.ServerVariables["URL"]);
                
            }
            dtgrdColors.EditItemIndex = -1;
            btnSave.Enabled = true;
            BindGrid();
            BindCheckList();
        }

        void dtgrdColors_Cancel(object sender, DataGridCommandEventArgs e)
        {
            dtgrdColors.EditItemIndex = -1;
            BindGrid();
            btnSave.Enabled = true;
        }

        void dtgrdColors_Delete(object sender, DataGridCommandEventArgs e)
        {
            string sql;

            sql = "update versioncolors set isactive=0 where id=" + dtgrdColors.DataKeys[e.Item.ItemIndex];

            try
            {
                MySqlDatabase.InsertQuery(sql, ConnectionType.MasterDatabase);
            }
            catch (SqlException ex)
            {
                Trace.Warn(ex.Message + ex.Source);
                ErrorClass.LogError(ex, Request.ServerVariables["URL"]);
                
            }
            BindGrid();
            BindCheckList();
        }

        private string GetBikeName(string VersionId)
        {
            string bikeName = "";
            string sql = "";


            sql = @"select concat( ve.makename,' ' ,ve.modelname , ' ' , ve.name ) bikemake
				from bikeversions ve 
				where ve.id = " + VersionId;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand(sql))
                {
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                bikeName = dr[0].ToString();
                            }
                        }
                    }
                }

            }
            catch (SqlException ex)
            {
                Trace.Warn(ex.Message);
            }

            return bikeName;
        } // GetBikeName
    } // class
} // namespace