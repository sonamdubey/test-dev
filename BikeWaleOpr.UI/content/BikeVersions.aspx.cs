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
    public class BikeVersions : System.Web.UI.Page
    {
        protected HtmlGenericControl spnError;
        protected DropDownList cmbMakes, cmbModels, cmbBodyStyles, cmbFuelType, cmbTransmission, cmbSegments, cmbSubSegments;
        protected TextBox txtVersion;
        protected HiddenField hdnSelectedModelId; 
        protected Button btnSave, btnShow;
        protected DataGrid dtgrdMembers;
        protected CheckBox chkUsed, chkNew, chkIndian, chkImported,
                            chkClassic, chkModified, chkFuturistic;
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

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
            btnSave.Click += new EventHandler(btnSave_Click);
            dtgrdMembers.PageIndexChanged += new DataGridPageChangedEventHandler(Page_Change);
            dtgrdMembers.SortCommand += new DataGridSortCommandEventHandler(Sort_Grid);
            dtgrdMembers.EditCommand += new DataGridCommandEventHandler(dtgrdMembers_Edit);
            dtgrdMembers.UpdateCommand += new DataGridCommandEventHandler(dtgrdMembers_Update);
            dtgrdMembers.CancelCommand += new DataGridCommandEventHandler(dtgrdMembers_Cancel);
            dtgrdMembers.DeleteCommand += new DataGridCommandEventHandler(dtgrdMembers_Delete);
            btnShow.Click += new EventHandler(btnShow_Click);
        }

        void Page_Load(object Sender, EventArgs e)
        {
            string sql;
            CommonOpn op = new CommonOpn();

            //if (HttpContext.Current.User.Identity.IsAuthenticated != true)
            //    Response.Redirect("../users/Login.aspx?ReturnUrl=../Contents/CarVersions.aspx");

            //if (Request.Cookies["Customer"] == null)
            //    Response.Redirect("../Users/Login.aspx?ReturnUrl=../Contents/CarVersions.aspx");

            //int pageId = 38;
            //if (!op.verifyPrivilege(pageId))
            //    Response.Redirect("../NotAuthorized.aspx");

            if (!IsPostBack)
            {
                sql = "SELECT Id,Name FROM BikeMakes WHERE IsDeleted=0 ORDER BY Name";

                try
                {
                    op.FillDropDown(sql, cmbMakes, "Name", "Id");
                    cmbMakes.Items.Insert(0, new ListItem("Select Make", "0"));

                    sql = "SELECT Id, Name FROM BikeSegments";
                    op.FillDropDown(sql, cmbSegments, "Name", "Id");
                    cmbSegments.Items.Add(new ListItem("--Select--","0"));

                    sql = "SELECT Id, Name FROM BikeSubSegments";
                    op.FillDropDown(sql, cmbSubSegments, "Name", "Id");
                    cmbSubSegments.Items.Add(new ListItem("--Select--", "0"));

                    sql = "SELECT Id, Name FROM BikeBodyStyles";
                    op.FillDropDown(sql, cmbBodyStyles, "Name", "Id");
                    cmbBodyStyles.Items.Add(new ListItem("--Select--", "0"));
                }
                catch (SqlException ex)
                {
                    Trace.Warn(ex.Message + ex.Source);
                    ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                    objErr.SendMail();
                }

                SortDirection = "";
                SortCriteria = "BV.Name";
            }

            sql = " SELECT ID, Name, BikeMakeId FROM BikeModels "
                + " WHERE IsDeleted=0 ORDER BY Name";

            string script = op.GenerateChainScript("cmbMakes", "cmbModels", sql, Request["cmbModels"]);
            //RegisterStartupScript( "Chain", script );
            ClientScript.RegisterStartupScript(this.GetType(), "ChainScript", script);

        } // Page_Load

        void btnShow_Click(object Sender, EventArgs e)
        {
            BindGrid();
        }

        void btnSave_Click(object Sender, EventArgs e)
        {
            Page.Validate();
            if (!Page.IsValid) return;

            string currentVerId = SaveData("-1");

            BindGrid();
        }

        string SaveData(string id)
        {

            string currentId = "-1";

            SqlConnection con;
            SqlCommand cmd;
            SqlParameter prm;
            Database db = new Database();
            CommonOpn op = new CommonOpn();

            string conStr = db.GetConString();

            con = new SqlConnection(conStr);

            try
            {
                Trace.Warn("Saving Data");

                cmd = new SqlCommand("Con_SaveBikeVersion", con);
                cmd.CommandType = CommandType.StoredProcedure;

                prm = cmd.Parameters.Add("@ID", SqlDbType.BigInt);
                prm.Value = id;
                Trace.Warn("id : ", id);

                prm = cmd.Parameters.Add("@Name", SqlDbType.VarChar, 50);
                prm.Value = txtVersion.Text.Trim();

                prm = cmd.Parameters.Add("@BikeModelId", SqlDbType.BigInt);
                prm.Value = Request["cmbModels"];

                prm = cmd.Parameters.Add("@SegmentId", SqlDbType.BigInt);
                prm.Value = cmbSegments.SelectedValue == "0" ? Convert.DBNull : cmbSegments.SelectedValue;

                prm = cmd.Parameters.Add("@BodyStyleId", SqlDbType.BigInt);
                prm.Value = cmbBodyStyles.SelectedValue == "0" ? Convert.DBNull : cmbBodyStyles.SelectedValue;

                prm = cmd.Parameters.Add("@FuelType", SqlDbType.BigInt);
                prm.Value = cmbFuelType.SelectedValue;

                prm = cmd.Parameters.Add("@Transmission", SqlDbType.BigInt);
                prm.Value = cmbTransmission.SelectedValue;

                prm = cmd.Parameters.Add("@Used", SqlDbType.Bit);
                prm.Value = Convert.ToInt16(chkUsed.Checked);

                prm = cmd.Parameters.Add("@New", SqlDbType.Bit);
                prm.Value = Convert.ToInt16(chkNew.Checked);

                prm = cmd.Parameters.Add("@Indian", SqlDbType.Bit);
                prm.Value = Convert.ToInt16(chkIndian.Checked);

                prm = cmd.Parameters.Add("@Imported", SqlDbType.Bit);
                prm.Value = Convert.ToInt16(chkImported.Checked);

                prm = cmd.Parameters.Add("@Classic", SqlDbType.Bit);
                prm.Value = Convert.ToInt16(chkClassic.Checked);

                prm = cmd.Parameters.Add("@Modified", SqlDbType.Bit);
                prm.Value = Convert.ToInt16(chkModified.Checked);

                prm = cmd.Parameters.Add("@Futuristic", SqlDbType.Bit);
                prm.Value = Convert.ToInt16(chkFuturistic.Checked);

                prm = cmd.Parameters.Add("@IsDeleted", SqlDbType.Bit);
                prm.Value = 0;

                prm = cmd.Parameters.Add("@SubSegmentId", SqlDbType.BigInt);
                prm.Value = cmbSubSegments.SelectedValue == "0" ? Convert.DBNull : cmbSubSegments.SelectedValue;

                prm = cmd.Parameters.Add("@CreatedOn", SqlDbType.DateTime);
                prm.Value = DateTime.Now;

                prm = cmd.Parameters.Add("@UpdatedBy", SqlDbType.VarChar);
                prm.Value = BikeWaleAuthentication.GetOprUserId();

                prm = cmd.Parameters.Add("@CurrentId", SqlDbType.BigInt);
                prm.Direction = ParameterDirection.Output;

                con.Open();
                //run the command
                cmd.ExecuteNonQuery();

                currentId = cmd.Parameters["@CurrentId"].Value.ToString();
                Trace.Warn("CurrentId : " + currentId);
            }
            catch (SqlException err)
            {
                //catch the sql exception. if it is equal to 2627, then say that it is for duplicate entry 
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch SqlException
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception
            finally
            {
                //close the connection	
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return currentId;
        }


        ///<summary>
        ///This function gets the list of the sell inquiries made according to the 
        ///model
        ///</summary>
        void BindGrid()
        {
            string sql = "";

            int pageSize = dtgrdMembers.PageSize;

            sql = "SELECT BV.id,BV.name,Se.id AS SegmentId,Se.name AS Segment,BS.id AS BodyStyleId,BS.name AS BodyStyle, BV.bikefueltype, BV.biketransmission, "
                + "bikemodelid, BV.used, BV.new, BV.indian, BV.imported, BV.classic, BV.modified, BV.futuristic, SSe.id AS SubSegmentId, SSe.name AS SubSegmentName, "
                + "CONVERT(VARCHAR(24), BV.vcreatedon, 113) AS CreatedOn, CONVERT(VARCHAR(24), BV.vupdatedon, 113) AS UpdatedOn, OU.username AS UpdatedBy "
                + "FROM BikeVersions BV LEFT JOIN BikeBodyStyles Bs ON Bs.Id = BV.BodyStyleId LEFT JOIN BikeSegments Se ON Se.Id = BV.SegmentId "
		        + "LEFT JOIN BikeSubSegments SSe ON SSe.Id = BV.SubSegmentId LEFT JOIN OprUsers OU ON BV.vupdatedby = OU.id "
                + "WHERE BV.isdeleted = 0 AND BV.BikeModelId = " + Request.Form["cmbModels"];

            if (SortCriteria != "")
                sql += " ORDER BY " + SortCriteria + " " + SortDirection;

            Trace.Warn(sql);
            CommonOpn objCom = new CommonOpn();
            try
            {
                objCom.BindGridSet(sql, dtgrdMembers);
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

        }

        void dtgrdMembers_Edit(object sender, DataGridCommandEventArgs e)
        {
            dtgrdMembers.EditItemIndex = e.Item.ItemIndex;
            BindGrid();
            btnSave.Enabled = false;
        }

        void dtgrdMembers_Update(object sender, DataGridCommandEventArgs e)
        {
            Page.Validate();
            if (!Page.IsValid) return;

            string sql;

            TextBox txt = (TextBox)e.Item.FindControl("txtVersion");

            CheckBox chkUsed1 = (CheckBox)e.Item.FindControl("chkUsed");
            CheckBox chkNew1 = (CheckBox)e.Item.FindControl("chkNew");
            CheckBox chkIndian1 = (CheckBox)e.Item.FindControl("chkIndian");
            CheckBox chkImported1 = (CheckBox)e.Item.FindControl("chkImported");
            CheckBox chkClassic1 = (CheckBox)e.Item.FindControl("chkClassic");
            CheckBox chkModified1 = (CheckBox)e.Item.FindControl("chkModified");
            CheckBox chkFuturistic1 = (CheckBox)e.Item.FindControl("chkFuturistic");

            sql = "UPDATE BikeVersions SET "
                + " Name='" + txt.Text.Trim().Replace("'", "''") + "',"
                + " SegmentId=" + Request.Form["cmbGridSegment"] + ","
                + " SubSegmentId=" + Request.Form["cmbGridSubSegment"] + ","
                + " BodyStyleId=" + Request.Form["cmbGridBodyStyle"] + ","
                + " BikeFuelType=" + Request.Form["cmbGridFuelType"] + ","
                + " BikeTransmission=" + Request.Form["cmbGridBikeTrans"] + ","
                + " Used=" + Convert.ToInt16(chkUsed1.Checked) + ","
                + " New=" + Convert.ToInt16(chkNew1.Checked) + ","
                + " Indian=" + Convert.ToInt16(chkIndian1.Checked) + ","
                + " Imported=" + Convert.ToInt16(chkImported1.Checked) + ","
                + " Classic=" + Convert.ToInt16(chkClassic1.Checked) + ","
                + " Modified=" + Convert.ToInt16(chkModified1.Checked) + ","
                + " Futuristic=" + Convert.ToInt16(chkFuturistic1.Checked) + ","
                + " VUpdatedOn=getdate(),"
                + " VUpdatedBy='" + BikeWaleAuthentication.GetOprUserId() + "'"
                + " WHERE Id=" + dtgrdMembers.DataKeys[e.Item.ItemIndex];

            Database db = new Database();
            Trace.Warn("sql=" + sql);

            try
            {
                db.InsertQry(sql);
            }
            catch (SqlException ex)
            {
                Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            dtgrdMembers.EditItemIndex = -1;

            btnSave.Enabled = true;
            BindGrid();
        }

        void dtgrdMembers_Cancel(object sender, DataGridCommandEventArgs e)
        {
            dtgrdMembers.EditItemIndex = -1;
            BindGrid();

            btnSave.Enabled = true;
        }

        void dtgrdMembers_Delete(object sender, DataGridCommandEventArgs e)
        {
            string sql;

            sql = "UPDATE BikeVersions SET IsDeleted=1,VUpdatedOn=getdate(),VUpdatedBy='" + BikeWaleAuthentication.GetOprUserId() + "' WHERE Id=" + dtgrdMembers.DataKeys[e.Item.ItemIndex];

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
            }
            BindGrid();
        }

        void Page_Change(object sender, DataGridPageChangedEventArgs e)
        {
            // Set CurrentPageIndex to the page the user clicked.
            dtgrdMembers.CurrentPageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void Sort_Grid(Object sender, DataGridSortCommandEventArgs e)
        {
            if (SortCriteria == e.SortExpression)
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
    }
}