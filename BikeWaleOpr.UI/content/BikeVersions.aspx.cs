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
using System.Data.Common;
using BikeWaleOPR.Utilities;
using BikeWaleOPR.DAL.CoreDAL;

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

            if (!IsPostBack)
            {
                sql = "select Id,Name from bikemakes where isdeleted=0 order by name";

                try
                {
                    op.FillDropDown(sql, cmbMakes, "Name", "Id");
                    cmbMakes.Items.Insert(0, new ListItem("Select Make", "-1"));

                    sql = "select Id, Name from bikesegments";
                    op.FillDropDown(sql, cmbSegments, "Name", "Id");
                    cmbSegments.Items.Add(new ListItem("--Select--", "-1"));

                    sql = "select Id, Name from bikesubsegments";
                    op.FillDropDown(sql, cmbSubSegments, "Name", "Id");
                    cmbSubSegments.Items.Add(new ListItem("--Select--", "-1"));

                    sql = "select Id, Name from bikebodystyles";
                    op.FillDropDown(sql, cmbBodyStyles, "Name", "Id");
                    cmbBodyStyles.Items.Add(new ListItem("--Select--", "-1"));
                }
                catch (SqlException ex)
                {
                    Trace.Warn(ex.Message + ex.Source);
                    ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                    objErr.SendMail();
                }

                SortDirection = "";
                SortCriteria = "bv.name";
            }

            sql = " select ID, Name, Bikemakeid from bikemodels   where isdeleted=0 order by name";

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
            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand("con_savebikeversion"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_id", DbParamTypeMapper.GetInstance[SqlDbType.BigInt], id));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_name", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 50, txtVersion.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_bikemodelid", DbParamTypeMapper.GetInstance[SqlDbType.BigInt], Request["cmbmodels"]));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_segmentid", DbParamTypeMapper.GetInstance[SqlDbType.BigInt], cmbSegments.SelectedValue == "0" ? Convert.DBNull : cmbSegments.SelectedValue));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_bodystyleid", DbParamTypeMapper.GetInstance[SqlDbType.BigInt], cmbBodyStyles.SelectedValue == "0" ? Convert.DBNull : cmbBodyStyles.SelectedValue));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_fueltype", DbParamTypeMapper.GetInstance[SqlDbType.BigInt], cmbFuelType.SelectedValue));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_transmission", DbParamTypeMapper.GetInstance[SqlDbType.BigInt], cmbTransmission.SelectedValue));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_used", DbParamTypeMapper.GetInstance[SqlDbType.Bit], Convert.ToInt16(chkUsed.Checked)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_new", DbParamTypeMapper.GetInstance[SqlDbType.Bit], Convert.ToInt16(chkNew.Checked)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_indian", DbParamTypeMapper.GetInstance[SqlDbType.Bit], Convert.ToInt16(chkIndian.Checked)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_imported", DbParamTypeMapper.GetInstance[SqlDbType.Bit], Convert.ToInt16(chkImported.Checked)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_classic", DbParamTypeMapper.GetInstance[SqlDbType.Bit], Convert.ToInt16(chkClassic.Checked)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modified", DbParamTypeMapper.GetInstance[SqlDbType.Bit], Convert.ToInt16(chkModified.Checked)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_futuristic", DbParamTypeMapper.GetInstance[SqlDbType.Bit], Convert.ToInt16(chkFuturistic.Checked)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isdeleted", DbParamTypeMapper.GetInstance[SqlDbType.Bit], false));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_subsegmentid", DbParamTypeMapper.GetInstance[SqlDbType.BigInt], cmbSubSegments.SelectedValue == "0" ? Convert.DBNull : cmbSubSegments.SelectedValue));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_createdon", DbParamTypeMapper.GetInstance[SqlDbType.DateTime], DateTime.Now));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_updatedby", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], BikeWaleAuthentication.GetOprUserId()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_currentid", DbParamTypeMapper.GetInstance[SqlDbType.BigInt], ParameterDirection.Output));

                    MySqlDatabase.ExecuteNonQuery(cmd);

                    currentId = cmd.Parameters["par_currentid"].Value.ToString(); 
                }

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
            int _modelid = default(int);

            if (!string.IsNullOrEmpty(Request.Form["cmbModels"].Trim()) && int.TryParse(Request.Form["cmbModels"], out _modelid))
            {
                sql = @"select bv.id,bv.name,se.id as segmentid,se.name as segment,bs.id as bodystyleid,bs.name as bodystyle, bv.bikefueltype, bv.biketransmission,
                bikemodelid, if(bv.used,1,0) as used, if(bv.new,1,0) as new, if(bv.indian,1,0) as indian, if(bv.imported,1,0) as imported, if(bv.classic,1,0) as classic, if(bv.modified,1,0) as modified, if(bv.futuristic,1,0) as futuristic, sse.id as subsegmentid, sse.name as subsegmentname,
                cast(bv.vcreatedon as char(24)) as createdon, cast(bv.vupdatedon as char(24))  as updatedon, ou.username as updatedby
                from bikeversions bv left join bikebodystyles bs on bs.id = bv.bodystyleid left join bikesegments se on se.id = bv.segmentid 
		        left join bikesubsegments sse on sse.id = bv.subsegmentid left join oprusers ou on bv.vupdatedby = ou.id 
                where bv.isdeleted = 0 and bv.bikemodelid = " + _modelid;

                if (SortCriteria != "")
                    sql += " order by " + SortCriteria + " " + SortDirection;
            }
            CommonOpn objCom = new CommonOpn();
            try
            {

                if (!string.IsNullOrEmpty(sql))
                {
                    objCom.BindGridSet(sql, dtgrdMembers);
                }
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

            sql = @"update bikeversions set 
                     name = @versionname,
                     segmentid=  @segmentid,
                     subsegmentid= @subsegmentid,
                     bodystyleid= @bodystyleid,
                     bikefueltype= @bikefueltype,
                     biketransmission=@biketransmission,
                     used= @used,
                     new= @new,
                     indian=@indian,
                     imported=@imported,
                     classic=@classic,
                     modified=@modified,
                     futuristic=@futuristic,
                     vupdatedon=now(),
                     vupdatedby=@vupdatedby
                     where id=@key";

            DbParameter[] param = new[]
                {
                    DbFactory.GetDbParam("@versionname", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 30, txt.Text.Trim().Replace("'", "''")),
                    DbFactory.GetDbParam("@segmentid", DbParamTypeMapper.GetInstance[SqlDbType.Int], Request.Form["cmbGridSegment"]),
                    DbFactory.GetDbParam("@subsegmentid", DbParamTypeMapper.GetInstance[SqlDbType.Int], Request.Form["cmbGridSubSegment"]),
                    DbFactory.GetDbParam("@bodystyleid", DbParamTypeMapper.GetInstance[SqlDbType.Int], Request.Form["cmbGridBodyStyle"]) ,
                    DbFactory.GetDbParam("@bikefueltype", DbParamTypeMapper.GetInstance[SqlDbType.Int], Request.Form["cmbGridFuelType"]) ,
                    DbFactory.GetDbParam("@biketransmission", DbParamTypeMapper.GetInstance[SqlDbType.Int], Request.Form["cmbGridBikeTrans"]) ,
                    DbFactory.GetDbParam("@used", DbParamTypeMapper.GetInstance[SqlDbType.Bit], chkUsed1.Checked),
                    DbFactory.GetDbParam("@new", DbParamTypeMapper.GetInstance[SqlDbType.Bit], chkNew1.Checked),
                    DbFactory.GetDbParam("@indian", DbParamTypeMapper.GetInstance[SqlDbType.Bit], chkIndian1.Checked),
                    DbFactory.GetDbParam("@Imported", DbParamTypeMapper.GetInstance[SqlDbType.Bit], chkImported1.Checked),
                    DbFactory.GetDbParam("@classic", DbParamTypeMapper.GetInstance[SqlDbType.Bit], chkClassic1.Checked),
                    DbFactory.GetDbParam("@modified", DbParamTypeMapper.GetInstance[SqlDbType.Bit], chkModified1.Checked),
                    DbFactory.GetDbParam("@futuristic", DbParamTypeMapper.GetInstance[SqlDbType.Bit], chkFuturistic1.Checked),
                    DbFactory.GetDbParam("@vupdatedby", DbParamTypeMapper.GetInstance[SqlDbType.BigInt], BikeWaleAuthentication.GetOprUserId()),
                    DbFactory.GetDbParam("@key", DbParamTypeMapper.GetInstance[SqlDbType.Int], dtgrdMembers.DataKeys[e.Item.ItemIndex])

                };
            try
            {
                MySqlDatabase.InsertQuery(sql,param);
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
            string sql = string.Empty;
            int _versionId = default(int);
            string vid = dtgrdMembers.DataKeys[e.Item.ItemIndex].ToString();
            if(!string.IsNullOrEmpty(vid) && int.TryParse(vid,out _versionId))
            {
                sql = "update bikeversions set isdeleted=1,vupdatedon=now(),vupdatedby='" + BikeWaleAuthentication.GetOprUserId() + "' where id=" + _versionId;
            }            

            try
            {
                if (!string.IsNullOrEmpty(sql))
                {
                    MySqlDatabase.InsertQuery(sql); 
                }
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
                SortDirection = SortDirection == "desc" ? "asc" : "desc";
            }
            else
            {
                SortDirection = "asc";
            }
            SortCriteria = e.SortExpression;

            BindGrid();

        }
    }
}