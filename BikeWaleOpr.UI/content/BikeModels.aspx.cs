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
using System.IO;
using System.Configuration;
using Enyim.Caching;
using Enyim.Caching.Memcached;

namespace BikeWaleOpr.Content
{
    public class BikeModels : System.Web.UI.Page
    {
        protected HtmlGenericControl spnError;
        protected DropDownList cmbMakes, ddlUpdateSeries, ddlSeries, ddlSegment, ddlUpdateSegment;
        protected TextBox txtModel,txtMaskingName;
        protected Button btnSave;
        protected HtmlInputButton btnUpdateSeries, btnUpdateSegment;
        protected DataGrid dtgrdMembers;
        protected Label lblStatus;
        protected HiddenField hdnModelIdList, hdnModelIdsList;
 
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
            cmbMakes.SelectedIndexChanged += new EventHandler(cmbMakes_SelectedIndexChanged);
            btnUpdateSeries.ServerClick += new EventHandler(btnUpdateSeries_ServerClick);
            btnUpdateSegment.ServerClick += new EventHandler(UpdateModelSegments);
        }

        bool _isMemcachedUsed;
        protected static MemcachedClient _mc = null;

        public BikeModels()
        {
            _isMemcachedUsed = bool.Parse(ConfigurationManager.AppSettings.Get("IsMemcachedUsed"));
            if (_mc == null)
            {
                InitializeMemcached();
            }
        }

        #region Initialize Memcache
        private void InitializeMemcached()
        {
            _mc = new MemcachedClient("memcached");
        }
        #endregion

        void Page_Load(object Sender, EventArgs e)
        {
            CommonOpn op = new CommonOpn();
            //if (HttpContext.Current.User.Identity.IsAuthenticated != true)
            //    Response.Redirect("../users/Login.aspx?ReturnUrl=../Contents/CarModels.aspx");

            //if (Request.Cookies["Customer"] == null)
            //    Response.Redirect("../Users/Login.aspx?ReturnUrl=../Contents/CarModels.aspx");

            //int pageId = 38;
            //if (!op.verifyPrivilege(pageId))
            //    Response.Redirect("../NotAuthorized.aspx");

            if (!IsPostBack)
            {
                DataTable dt = null;

                try
                {
                    MakeModelVersion mmv = new MakeModelVersion();

                    dt = mmv.GetMakes("ALL");
                    cmbMakes.DataSource = dt;
                    cmbMakes.DataValueField = "value";
                    cmbMakes.DataTextField = "text";
                    cmbMakes.DataBind();
                    cmbMakes.Items.Insert(0, new ListItem("Select", "0"));
                }
                catch (SqlException ex)
                {
                    Trace.Warn(ex.Message + ex.Source);
                    ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                    objErr.SendMail();
                }

                SortDirection = "";
                SortCriteria = "";
            }
           /* if (SortCriteria == "New")
            {
                SortDirection = "DESC";
            }*/
        } // Page_Load

        void btnSave_Click(object Sender, EventArgs e)
        {
            Page.Validate();
            if (!Page.IsValid) return;

            string sql = "", sqlId = "";
            string currentId = "-1";
            SqlDataReader dr = null;
            Trace.Warn("masking name ", txtMaskingName.Text.Trim());
            sql = "INSERT INTO BikeModels( Name,MaskingName,BikeMakeId,BikeSeriesId,BikeClassSegmentsId, IsDeleted,MoCreatedOn,MoUpdatedBy ) "
                + " VALUES( '" + txtModel.Text.Trim().Replace("'", "''") + "', '" + txtMaskingName.Text.Trim() + "'," + cmbMakes.SelectedValue + " ," + ddlSeries.SelectedValue + ","+ ddlSegment.SelectedValue +",0,getdate(),'" + BikeWaleAuthentication.GetOprUserId() + "' )";

            sqlId = "SELECT Id FROM BikeModels"
                + " WHERE Name = '" + txtModel.Text.Trim().Replace("'", "''") + "'"
                + " AND BikeMakeId = " + cmbMakes.SelectedValue + " AND IsDeleted = 0";

            Database db = new Database();

            try
            {
                db.InsertQry(sql);
                dr = db.SelectQry(sqlId);
                if (dr.Read())
                {
                    currentId = dr["Id"].ToString();
                }
                if (currentId != "-1")
                {
                    Trace.Warn("Writing File" + currentId);
                    WriteFileModel(currentId, txtModel.Text.Trim().Replace("'", "''"));
                }

                if (_mc.Get("BW_ModelMapping") != null)
                    _mc.Remove("BW_ModelMapping");

                if (_mc.Get("BW_NewModelMaskingNames") != null)
                    _mc.Remove("BW_NewModelMaskingNames");

                if(_mc.Get("BW_OldModelMaskingNames") != null)
                    _mc.Remove("BW_OldModelMaskingNames");

            }
            catch (SqlException ex)
            {         
                Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
                Trace.Warn("exception number : ", ex.Number.ToString());

                if (ex.Number == 2601)
                {
                    lblStatus.Text = "Model Name already exists. Can not insert duplicate name.";
                }
                else
                {  // Error code Unique key constraint in the database.
                    if(ex.Number == 2627)
                        lblStatus.Text = "Model Masking Name already exists. Can not insert duplicate name.";
                    else
                     lblStatus.Text = "";
                }
                   
            }
            finally 
            {
                if(dr != null)
                    dr.Close();
                db.CloseConnection();
            }
            BindGrid();
        }

        void cmbMakes_SelectedIndexChanged(object Sender, EventArgs e)
        {
            FillSeries();
            FillSegments();
            BindGrid();
            //ddlUpdateSeries.Items.Remove("0");
            //ddlSeries.Items.Remove("0");
        
        }
        ///<summary>
        ///This function gets the list of the sell inquiries made according to the 
        ///model
        ///</summary>
        void BindGrid()
        {            
            string sql = "";

            int pageSize = dtgrdMembers.PageSize;

            sql = " SELECT Mo.ID, Mo.Name, MO.Used, MO.New, MO.Indian,MO.MaskingName,BS.Name AS SeriesName , BS.MaskingName AS SeriesMaskingName, "
                + " MO.Imported, MO.Classic, MO.Modified, MO.Futuristic, MO.BikeMakeId,CONVERT(VARCHAR(24), Mo.MoCreatedOn, 113) AS CreatedOn,CONVERT(VARCHAR(24), Mo.MoUpdatedOn, 113) AS UpdatedOn,OU.UserName AS UpdatedBy "
                + " ,BCS.ClassSegmentName " 
                + " FROM BikeModels Mo LEFT JOIN OprUsers OU "
                + " ON Mo.MoUpdatedBy = OU.id "
                + " LEFT JOIN BikeSeries BS ON Mo.BikeSeriesId = BS.ID "
                + " LEFT JOIN BikeClassSegments BCS ON Mo.BikeClassSegmentsId = BCS.BikeClassSegmentsId "
                + " WHERE MO.IsDeleted=0 "
                + " AND Mo.BikeMakeId=" + cmbMakes.SelectedItem.Value;

            if (SortCriteria != "")
                sql += " ORDER BY MO.Futuristic DESC ,MO.New  DESC , MO.Used DESC, " + SortCriteria + " " + SortDirection;
            else
                sql += " ORDER BY MO.Futuristic DESC ,MO.New  DESC , MO.Used DESC, Name ASC " ;

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
            Database db = null;

            Page.Validate();
            if (!Page.IsValid) return;
            
            try
            {
                string sql = string.Empty;
                TextBox txt = (TextBox)e.Item.FindControl("txtModelName");
                CheckBox chkUsed1 = (CheckBox)e.Item.FindControl("chkUsed");
                CheckBox chkNew1 = (CheckBox)e.Item.FindControl("chkNew");
                CheckBox chkIndian1 = (CheckBox)e.Item.FindControl("chkIndian");
                CheckBox chkImported1 = (CheckBox)e.Item.FindControl("chkImported");
                CheckBox chkClassic1 = (CheckBox)e.Item.FindControl("chkClassic");
                CheckBox chkModified1 = (CheckBox)e.Item.FindControl("chkModified");
                CheckBox chkFuturistic1 = (CheckBox)e.Item.FindControl("chkFuturistic");                
                Label lblMakeId = (Label)e.Item.FindControl("lblMakeId");
                Trace.Warn("txt : ", txt.Text);
                sql = "UPDATE BikeModels SET "
                    + " Name='" + txt.Text.Trim().Replace("'", "''") + "',"
                    + " Used=" + Convert.ToInt16(chkUsed1.Checked) + ","
                    + " New=" + Convert.ToInt16(chkNew1.Checked) + ","
                    + " Indian=" + Convert.ToInt16(chkIndian1.Checked) + ","
                    + " Imported=" + Convert.ToInt16(chkImported1.Checked) + ","
                    + " Classic=" + Convert.ToInt16(chkClassic1.Checked) + ","
                    + " Modified=" + Convert.ToInt16(chkModified1.Checked) + ","
                    + " Futuristic=" + Convert.ToInt16(chkFuturistic1.Checked) + ","
                    + " MoUpdatedOn=getdate(),"
                    + " MoUpdatedBy='" + BikeWaleAuthentication.GetOprUserId() + "'"
                    + " WHERE Id=" + dtgrdMembers.DataKeys[e.Item.ItemIndex];

                db = new Database();
            
                db.InsertQry(sql);

                //Update Upcoming Bike
                if (chkFuturistic1.Checked == true)
                    MakeUpcomingBike(dtgrdMembers.DataKeys[e.Item.ItemIndex].ToString(), lblMakeId.Text);

                //Write URL
                if (dtgrdMembers.DataKeys[e.Item.ItemIndex].ToString() != "-1")
                {
                    Trace.Warn("Writing File" + dtgrdMembers.DataKeys[e.Item.ItemIndex]);
                    WriteFileModel(dtgrdMembers.DataKeys[e.Item.ItemIndex].ToString(), txt.Text.Trim().Replace("'", "''"));
                }

                if (chkUsed1.Checked && !chkFuturistic1.Checked && !chkNew1.Checked)
                {
                    try
                    {
                        Trace.Warn("++++model Id : ", dtgrdMembers.DataKeys[e.Item.ItemIndex].ToString());
                        MakeModelVersion mmv = new MakeModelVersion();
                        mmv.DiscontinueBikeModel(dtgrdMembers.DataKeys[e.Item.ItemIndex].ToString());
                    }
                    catch (Exception ex)
                    {
                        Trace.Warn(ex.Message);
                        ErrorClass errObj = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                        errObj.SendMail();
                    }
                }
                    
            }
            catch (SqlException ex)
            {
                Trace.Warn(ex.StackTrace);
                Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch(Exception ex)
            {
                Trace.Warn(ex.StackTrace);
                Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                if(db != null)
                    db.CloseConnection();
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
            //string sql;

            //sql = "UPDATE BikeModels SET IsDeleted=1,MoUpdatedOn=getdate(),MoUpdatedBy='" + BikeWaleAuthentication.GetOprUserId() + "' WHERE Id=" + dtgrdMembers.DataKeys[e.Item.ItemIndex];

            //Database db = new Database();

            //try
            //{
            //    db.InsertQry(sql);
            //}
            //catch (SqlException ex)
            //{
            //    Trace.Warn(ex.Message + ex.Source);
            //    ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}

            MakeModelVersion mmv = new MakeModelVersion();
            mmv.DeleteModelVersions(dtgrdMembers.DataKeys[e.Item.ItemIndex].ToString(), BikeWaleAuthentication.GetOprUserId());
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

        void MakeUpcomingBike(string modelId, string makeId)
        {
            SqlDataReader dr = null;
            Database db = new Database();
            string sql = "", sqlSave = "";

            sql = "SELECT Id FROM ExpectedBikeLaunches WHERE BikeModelId = " + modelId + "";

            sqlSave = "INSERT INTO ExpectedBikeLaunches(BikeMakeId, BikeModelId, IsLaunched) VALUES(" + makeId + ", " + modelId + ", 0)";

            try
            {
                dr = db.SelectQry(sql);
                if (dr.Read())
                {
                    Trace.Warn("Exist");
                }
                else
                {
                    db.InsertQry(sqlSave);
                }

            }
            catch (SqlException err)
            {
                //catch the sql exception. if it is equal to 2627, then say that it is for duplicate entry 
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                //catch the sql exception. if it is equal to 2627, then say that it is for duplicate entry 
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception
            finally
            {
                if(dr != null)
                    dr.Close();
                db.CloseConnection();
            }
        }   // End of make upcoming bike method

        //Function to save data in the URL redirecting mapping file.
        bool WriteFileModel(string urlModelId, string urlModelName)
        {
            bool isSaved = false;
            string fullPath = "";
            string mainDir = "";
            string txtToWrite = "";

            try
            {
                if (urlModelName.Length > 0 && CommonOpn.CheckId(urlModelId))
                {
                    txtToWrite = urlModelName.Trim().ToLower().Replace(" ", "").Replace("-", "") + "\t" + urlModelId;
                    mainDir = CommonOpn.ResolvePhysicalPath("/Research/urlMapping/");

                    //check whether the directory for the make exists or not, if not then create the directory
                    if (Directory.Exists(mainDir) == false)
                        Directory.CreateDirectory(mainDir);

                    //create file to store description
                    fullPath = mainDir + "\\map_models.txt";
                    Trace.Warn(fullPath);

                    if (File.Exists(fullPath) != false)
                    {
                        Trace.Warn("Appending File=" + txtToWrite);
                        StreamWriter sw = File.AppendText(fullPath);
                        sw.WriteLine(txtToWrite);
                        sw.Flush();
                        sw.Close();

                        //Save the same data in table
                        //SaveModelURL(urlModelId, txtToWrite);
                    }
                    else
                    {
                        Trace.Warn("Writing File=" + txtToWrite);
                        StreamWriter sw = File.CreateText(fullPath);
                        sw.Write(txtToWrite);
                        sw.Flush();
                        sw.Close();

                        //Save the same data in table
                        //SaveModelURL(urlModelId, txtToWrite);
                    }
                    isSaved = true;
                }
            }
            catch (Exception err)
            {
                //catch the sql exception. if it is equal to 2627, then say that it is for duplicate entry 
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
                isSaved = false;
            } // catch Exception

            return isSaved;
        }

        //void SaveModelURL(string urlModelId, string modelURL)
        //{
        //    string sql = "";
        //    Database db = new Database();

        //    sql = "INSERT INTO URLRedirect(ContentType, ContentId, ContentURL, CreatedOn)"
        //        + " VALUES(1, " + urlModelId + ", '" + modelURL + "', '" + DateTime.Now + "')";

        //    try
        //    {
        //        Trace.Warn("sql=" + sql);
        //        db.InsertQry(sql);
        //    }
        //    catch (SqlException ex)
        //    {
        //        Trace.Warn(ex.Message + ex.Source);
        //        ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
        //        objErr.SendMail();
        //    }
        //}

        /// <summary>
        /// Created by : Sadhana Upadhyay on 26th Feb 2014
        /// Summary : To fill Series Dropdownlist
        /// </summary>
        void FillSeries()
        {
            string makeId = cmbMakes.SelectedValue;
            DataTable dt = null;

            try
            {
                ManageBikeSeries ms = new ManageBikeSeries();

                dt = ms.GetSeriesDdl(makeId);
                ddlUpdateSeries.DataSource = dt;
                ddlUpdateSeries.DataValueField = "value";
                ddlUpdateSeries.DataTextField = "text";
                ddlUpdateSeries.DataBind();

                ddlSeries.DataSource = dt;
                ddlSeries.DataValueField = "value";
                ddlSeries.DataTextField = "text";
                ddlSeries.DataBind();

                ListItem item = new ListItem("--Select Series--", "-1");
                ddlUpdateSeries.Items.Insert(0, item);
                ddlSeries.Items.Insert(0, item);
            }
            catch (SqlException err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception
        }   //End of FillSeries

        /// <summary>
        /// Written By : Ashwini Todkar on 20 March 2014
        /// Summary    : Method to get cc segment classes and ids
        /// </summary>
        private void FillSegments()
        {

            DataTable dt = GetModelCCSegments();

            if (dt.Rows.Count > 0)
            {
                ListItem li = new ListItem("---Select Segment---", "-1");

                ddlSegment.DataSource = dt;
                ddlSegment.DataTextField = "Text";
                ddlSegment.DataValueField = "Value";
                ddlSegment.DataBind();

                ddlSegment.Items.Insert(0, li);

                ddlUpdateSegment.DataSource = dt;
                ddlUpdateSegment.DataTextField = "Text";
                ddlUpdateSegment.DataValueField = "Value";
                ddlUpdateSegment.DataBind();

                ddlUpdateSegment.Items.Insert(0, li);
            }
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 20 March 2014
        /// Summary    : Method to get cc segment classes and ids
        /// </summary>
        /// <returns></returns>
        private DataTable GetModelCCSegments()
        {
            Database db = null;
            DataTable dt = null;

            try
            {
                using (SqlCommand cmd = new SqlCommand("GetModelCCSegments"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    db = new Database();
                    dt = db.SelectAdaptQry(cmd).Tables[0];
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
          
            return dt;
        }//End of GetModelCCSegments


        /// <summary>
        /// Created by : Sadhana Upadhyay on 26th Feb 2014
        /// Summary : To update Model series
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdateSeries_ServerClick(object sender, EventArgs e)
        {
            string ModelIdList = hdnModelIdList.Value;
            if (ModelIdList.Length > 0)
                ModelIdList = ModelIdList.Substring(0, ModelIdList.Length - 1);
            Trace.Warn("ModelIdList" + ModelIdList);
            try
            {
                ManageBikeSeries ms = new ManageBikeSeries();
                ms.UpdateModelSeries(ddlUpdateSeries.SelectedValue, ModelIdList);
            }

            catch (Exception err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            BindGrid();
            Page.ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Series Updated Successfully.');", true);
        }   //End of btnUpdateSeries_Click

        /// <summary>
        /// Written By : Ashwini Todkar on 20 March 2014
        /// Summary    : Method to update cc segment classes for models
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateModelSegments(object sender, EventArgs e)
        {
            string ModelIdsList = hdnModelIdsList.Value;

            if (ModelIdsList.Length > 0)
                ModelIdsList = ModelIdsList.Substring(0 , ModelIdsList.Length - 1);

            Trace.Warn("++ model ids ",ModelIdsList);

            try
            {
                UpdateModelSegments(ddlUpdateSegment.SelectedValue,ModelIdsList);
            }
            catch (Exception ex)
            {
                Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            BindGrid();
            Page.ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Segment Updated Successfully.');", true);
        }   // End of UpdateModelSegments


        /// <summary>
        /// Written By : Ashwini Todkar on 20 March 2014
        /// Summary    : Method to update cc segment classes for models
        /// </summary>
        /// <param name="segmentId">segment class id</param>
        /// <param name="modelIdsList">List of model ids</param>
        private void UpdateModelSegments(string segmentId, string modelIdsList)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    Database db = new Database();
                    cmd.CommandText = "UpdateModelSegments";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@SegmentId", SqlDbType.Int).Value = segmentId;
                    cmd.Parameters.Add("@ModelIdsList", SqlDbType.VarChar, 500).Value = modelIdsList;

                    db.UpdateQry(cmd);
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }   //End of UpdateModelSegments
    }
}