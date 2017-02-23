using Bikewale.Utility;
using BikewaleOpr.common;
using BikeWaleOpr.Common;
using MySql.CoreDAL;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

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
                    cmbMakes.Items.Insert(0, new ListItem("Select Make", "0"));

                    sql = "select Id, Name from bikesegments";
                    op.FillDropDown(sql, cmbSegments, "Name", "Id");
                    cmbSegments.Items.Add(new ListItem("--Select--", "0"));

                    sql = "select Id, Name from bikesubsegments";
                    op.FillDropDown(sql, cmbSubSegments, "Name", "Id");
                    cmbSubSegments.Items.Add(new ListItem("--Select--", "0"));

                    sql = "select Id, Name from bikebodystyles";
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
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_id", DbType.Int64, id));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_name", DbType.String, 50, txtVersion.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_bikemodelid", DbType.Int32, Request["cmbmodels"]));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_segmentid", DbType.Int32, cmbSegments.SelectedValue == "0" ? Convert.DBNull : cmbSegments.SelectedValue));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_bodystyleid", DbType.Int64, cmbBodyStyles.SelectedValue == "0" ? Convert.DBNull : cmbBodyStyles.SelectedValue));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_fueltype", DbType.Int16, cmbFuelType.SelectedValue));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_transmission", DbType.Int32, cmbTransmission.SelectedValue));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_used", DbType.Boolean, Convert.ToInt16(chkUsed.Checked)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_new", DbType.Boolean, Convert.ToInt16(chkNew.Checked)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_indian", DbType.Boolean, Convert.ToInt16(chkIndian.Checked)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_imported", DbType.Boolean, Convert.ToInt16(chkImported.Checked)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_classic", DbType.Boolean, Convert.ToInt16(chkClassic.Checked)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modified", DbType.Boolean, Convert.ToInt16(chkModified.Checked)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_futuristic", DbType.Boolean, Convert.ToInt16(chkFuturistic.Checked)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isdeleted", DbType.Boolean, false));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_subsegmentid", DbType.Int64, cmbSubSegments.SelectedValue == "0" ? Convert.DBNull : cmbSubSegments.SelectedValue));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_createdon", DbType.DateTime, Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_updatedby", DbType.String, BikeWaleAuthentication.GetOprUserId()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_currentid", DbType.Int64, ParameterDirection.Output));
                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);

                    currentId = cmd.Parameters["par_currentid"].Value.ToString();
                    if (currentId != "-1")
                    {
                        NameValueCollection nvc = new NameValueCollection();
                        nvc.Add("versionid", currentId);
                        nvc.Add("modelId", Request["cmbmodels"].ToString());
                        nvc.Add("versionName", txtVersion.Text.Trim());
                        nvc.Add("IsNew", Convert.ToInt16(chkNew.Checked).ToString());
                        nvc.Add("IsUsed", Convert.ToInt16(chkUsed.Checked).ToString());
                        nvc.Add("Isfuturistic", Convert.ToInt16(chkFuturistic.Checked).ToString());
                        SyncBWData.PushToQueue("BW_AddBikeVersions", DataBaseName.CW, nvc);
                    }
                }
            }
            catch (SqlException err)
            {
                //catch the sql exception. if it is equal to 2627, then say that it is for duplicate entry 
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch SqlException
            catch (Exception err)
            {
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
                DATE_FORMAT(bv.vcreatedon, '%d %b %Y %T') as createdon, DATE_FORMAT(bv.vupdatedon, '%d %b %Y %T')  as updatedon, ou.username as updatedby
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
                    DbFactory.GetDbParam("@versionname", DbType.String, 30, txt.Text.Trim().Replace("'", "''")),
                    DbFactory.GetDbParam("@segmentid", DbType.Int32, Request.Form["cmbGridSegment"]),
                    DbFactory.GetDbParam("@subsegmentid", DbType.Int32, Request.Form["cmbGridSubSegment"]),
                    DbFactory.GetDbParam("@bodystyleid", DbType.Int32, Request.Form["cmbGridBodyStyle"]) ,
                    DbFactory.GetDbParam("@bikefueltype", DbType.Int32, Request.Form["cmbGridFuelType"]) ,
                    DbFactory.GetDbParam("@biketransmission", DbType.Int32, Request.Form["cmbGridBikeTrans"]) ,
                    DbFactory.GetDbParam("@used", DbType.Boolean, chkUsed1.Checked),
                    DbFactory.GetDbParam("@new", DbType.Boolean, chkNew1.Checked),
                    DbFactory.GetDbParam("@indian", DbType.Boolean, chkIndian1.Checked),
                    DbFactory.GetDbParam("@Imported", DbType.Boolean, chkImported1.Checked),
                    DbFactory.GetDbParam("@classic", DbType.Boolean, chkClassic1.Checked),
                    DbFactory.GetDbParam("@modified", DbType.Boolean, chkModified1.Checked),
                    DbFactory.GetDbParam("@futuristic", DbType.Boolean, chkFuturistic1.Checked),
                    DbFactory.GetDbParam("@vupdatedby", DbType.Int64, BikeWaleAuthentication.GetOprUserId()),
                    DbFactory.GetDbParam("@key", DbType.Int32, dtgrdMembers.DataKeys[e.Item.ItemIndex])

                };
            try
            {
                MySqlDatabase.InsertQuery(sql, param, ConnectionType.MasterDatabase);
                NameValueCollection nvc = new NameValueCollection();
                nvc.Add("VersionId", dtgrdMembers.DataKeys[e.Item.ItemIndex].ToString());
                nvc.Add("versionname", txt.Text.Trim().Replace("'", "''"));
                nvc.Add("IsNew", Convert.ToInt16(chkNew1.Checked).ToString());
                nvc.Add("IsUsed", Convert.ToInt16(chkUsed1.Checked).ToString());
                nvc.Add("IsFuturistic", Convert.ToInt16(chkFuturistic1.Checked).ToString());
                SyncBWData.PushToQueue("BW_UpdateBikeVersions", DataBaseName.CW, nvc);

                var makeId = Request.Form["cmbMakes"];

                //Refresh memcache object for popularBikes change
                MemCachedUtil.Remove(string.Format("BW_PopularBikesByMake_{0}", makeId));
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
            if (!string.IsNullOrEmpty(vid) && int.TryParse(vid, out _versionId))
            {
                sql = "update bikeversions set isdeleted=1,vupdatedon=now(),vupdatedby='" + BikeWaleAuthentication.GetOprUserId() + "' where id=" + _versionId;
            }
            try
            {
                if (!string.IsNullOrEmpty(sql))
                {
                    MySqlDatabase.InsertQuery(sql, ConnectionType.MasterDatabase);
                    NameValueCollection nvc = new NameValueCollection();
                    nvc.Add("VersionId", _versionId.ToString());
                    nvc.Add("IsDeleted", "1");
                    SyncBWData.PushToQueue("BW_UpdateBikeVersions", DataBaseName.CW, nvc);
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