﻿using Bikewale.Notifications;
using Bikewale.Utility;
using BikewaleOpr.common;
using BikewaleOpr.DALs.Bikedata;
using BikewaleOpr.Entity.BikeData;
using BikewaleOpr.Interface.BikeData;
using BikeWaleOpr.Common;
using Enyim.Caching;
using Microsoft.Practices.Unity;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace BikeWaleOpr.Content
{
    public class BikeModels : System.Web.UI.Page
    {
        protected HtmlGenericControl spnError;
        protected DropDownList cmbMakes, ddlSegment, ddlUpdateSegment, ddlSeries, ddlUpdateSeries;
        protected TextBox txtModel, txtMaskingName;
        protected Button btnSave;
        protected HtmlInputButton btnUpdateSegment, btnUpdateSeries;
        protected DataGrid dtgrdMembers;
        protected Label lblStatus;
        protected HiddenField hdnModelIdList, hdnModelIdsList, hdnModelIdsListSeries;
        private readonly IBikeSeries _series = null;

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
            btnUpdateSegment.ServerClick += new EventHandler(UpdateModelSegments);
            btnUpdateSeries.ServerClick += new EventHandler(UpdateModelSeries);
        }

        bool _isMemcachedUsed;
        protected static MemcachedClient _mc = null;

        /// <summary>
        /// Modified by : Vivek Singh Tomar on 13th Sep 2017
        /// Summary : Added initialization of BikeSeries
        /// </summary>
        public BikeModels()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IBikeSeriesRepository, BikeSeriesRepository>()
                .RegisterType<IBikeSeries, BikewaleOpr.BAL.BikeSeries>();
                _series = container.Resolve<IBikeSeries>();
            }
            _isMemcachedUsed = bool.Parse(ConfigurationManager.AppSettings.Get("IsMemcachedUsed"));
            if (_isMemcachedUsed && _mc == null)
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
            lblStatus.Text = "";
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
                    BikeWaleOpr.Common.ErrorClass objErr = new BikeWaleOpr.Common.ErrorClass(ex, Request.ServerVariables["URL"]);
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

        /// <summary>
        /// modified by : Sajal Gupta on 9-1-2017
        /// Description : Refreshed memcache popular bike makes key
        /// Modified By : Sushil Kumar on 9th July 2017
        /// Description : Change input parametres as per carwale mysql master base conventions
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        void btnSave_Click(object Sender, EventArgs e)
        {
            Page.Validate();
            if (!Page.IsValid) return;
            uint _modelId = 0;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("insertbikemodel14092017"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelname", DbType.String, 30, txtModel.Text.Trim().Replace("'", "''")));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelmaskingname", DbType.String, 50, txtMaskingName.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, cmbMakes.SelectedValue));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_segmentid", DbType.Int32, ddlSegment.SelectedValue));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_seriesid", DbType.Int32, (Convert.ToInt32(ddlSeries.SelectedValue) > 0)?ddlSeries.SelectedValue:Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_userid", DbType.Int32, BikeWaleAuthentication.GetOprUserId()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_ismodelexist", DbType.Boolean, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, ParameterDirection.Output));

                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);
                    _modelId = Convert.ToUInt32(cmd.Parameters["par_modelid"].Value);

                    if (Convert.ToBoolean(cmd.Parameters["par_ismodelexist"].Value))
                    {
                        lblStatus.Text = "Model Masking Name already exists. Can not insert duplicate name.";
                    }
                    else if (_modelId > 0)
                    {
                        // Push Data to Carwale DB
                        NameValueCollection nvc = new NameValueCollection();
                        nvc.Add("v_modelId", _modelId.ToString());
                        nvc.Add("v_MakeId", cmbMakes.SelectedValue);
                        nvc.Add("v_ModelName", txtModel.Text.Trim().Replace("'", "''"));
                        nvc.Add("v_ModelMaskingName", txtMaskingName.Text.Trim());
                        nvc.Add("v_HostUrl", null);
                        nvc.Add("v_OriginalImagePath", null);
                        nvc.Add("v_New", "1");
                        nvc.Add("v_Used", "1");
                        nvc.Add("v_Futuristic", "0");
                        SyncBWData.PushToQueue("BW_AddBikeModels", DataBaseName.CW, nvc);

                        //CLear popularBikes key                       
                        UInt32 makeId;
                        UInt32.TryParse(cmbMakes.SelectedValue, out makeId);
                        BikewaleOpr.Cache.BwMemCache.ClearPopularBikesCacheKey(null, makeId);
                        BikewaleOpr.Cache.BwMemCache.ClearPopularBikesCacheKey(6, makeId);
                        BikewaleOpr.Cache.BwMemCache.ClearPopularBikesCacheKey(9, makeId);
                        BikewaleOpr.Cache.BwMemCache.ClearPopularBikesCacheKey(9, null);
                    }

                    if (_mc != null)
                    {
                        if (_mc.Get("BW_ModelMapping") != null)
                            _mc.Remove("BW_ModelMapping");

                        if (_mc.Get("BW_NewModelMaskingNames") != null)
                            _mc.Remove("BW_NewModelMaskingNames");

                        if (_mc.Get("BW_OldModelMaskingNames") != null)
                            _mc.Remove("BW_OldModelMaskingNames");
                    }

                }

            }
            catch (SqlException ex)
            {
                BikeWaleOpr.Common.ErrorClass objErr = new BikeWaleOpr.Common.ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            BindGrid();
        }

        void cmbMakes_SelectedIndexChanged(object Sender, EventArgs e)
        {
            FillSegments();
            FillSeries(Convert.ToInt32(cmbMakes.SelectedValue));
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
            int _makeid = default(int);

            if (!string.IsNullOrEmpty(cmbMakes.SelectedItem.Value.Trim()) && int.TryParse(cmbMakes.SelectedItem.Value, out _makeid))
            {
                sql = @" select mo.id, mo.name, if(mo.used,1,0) as used, if(mo.new,1,0) as new, if(mo.indian,1,0) as indian,mo.maskingname, 
                if(mo.imported,1,0) as imported, if(mo.classic,1,0) as  classic, if(mo.modified,1,0) as  modified, if(mo.futuristic,1,0) as futuristic, mo.bikemakeid,cast( mo.mocreatedon as char(24)) as createdon,cast( mo.moupdatedon  as char(24)) as updatedon,ou.username as updatedby 
                ,bcs.classsegmentname 
                ,mo.makemaskingname as makemasking
                ,bs.id as seriesId
                ,IFNULL(bs.Name, 'N/A') as seriesName
                from bikemodels mo left join oprusers ou 
                on mo.moupdatedby = ou.id 
                left join bikeclasssegments bcs on mo.bikeclasssegmentsid = bcs.bikeclasssegmentsid
                left join bikeseries bs on mo.BikeSeriesID = bs.Id
                where mo.isdeleted=0 
                and mo.bikemakeid=" + _makeid;

                if (SortCriteria != "")
                    sql += " order by mo.futuristic desc ,mo.new  desc , mo.used desc, " + SortCriteria + " " + SortDirection;
                else
                    sql += " order by mo.futuristic desc ,mo.new  desc , mo.used desc, name asc ";
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
                BikeWaleOpr.Common.ErrorClass objErr = new BikeWaleOpr.Common.ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        void dtgrdMembers_Edit(object sender, DataGridCommandEventArgs e)
        {
            dtgrdMembers.EditItemIndex = e.Item.ItemIndex;
            BindGrid();
            btnSave.Enabled = false;
        }

        /// <summary>
        /// Modified By : Sushil Kumar on 9th July 2017
        /// Description : Change input parametres as per carwale mysql master base conventions
        /// </summary>
        /// Modified by : Vivek Singh Tomar on 31 July 2017
        /// Description : Refresh the cache when any model is updated
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dtgrdMembers_Update(object sender, DataGridCommandEventArgs e)
        {
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

                using (DbCommand cmd = DbFactory.GetDBCommand("Updatebikemodel"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelname", DbType.String, 30, txt.Text.Trim().Replace("'", "''")));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_indian", DbType.Boolean, chkIndian1.Checked));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_Imported", DbType.Boolean, chkImported1.Checked));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_classic", DbType.Boolean, chkClassic1.Checked));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modified", DbType.Boolean, chkModified1.Checked));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_futuristic", DbType.Boolean, chkFuturistic1.Checked));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_new", DbType.Boolean, chkNew1.Checked));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_used", DbType.Boolean, chkUsed1.Checked));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_moupdatedby", DbType.Int64, BikeWaleAuthentication.GetOprUserId()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_key", DbType.Int32, dtgrdMembers.DataKeys[e.Item.ItemIndex]));

                    MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase);
                    NameValueCollection nvc = new NameValueCollection();
                    nvc.Add("v_ModelName", txt.Text.Trim().Replace("'", "''"));
                    nvc.Add("v_IsUsed", Convert.ToInt16(chkUsed1.Checked).ToString());
                    nvc.Add("v_IsNew", Convert.ToInt16(chkNew1.Checked).ToString());
                    nvc.Add("v_IsFuturistic", Convert.ToInt16(chkFuturistic1.Checked).ToString());
                    nvc.Add("v_ModelId", dtgrdMembers.DataKeys[e.Item.ItemIndex].ToString());
                    nvc.Add("v_MakeId", null);
                    nvc.Add("v_ModelMaskingName", null);
                    nvc.Add("v_HostUrl", null);
                    nvc.Add("v_OriginalImagePath", null);
                    nvc.Add("v_IsDeleted", null);
                    SyncBWData.PushToQueue("BW_UpdateBikeModels", DataBaseName.CW, nvc);
                }


                if (!chkNew1.Checked)
                {
                    uint makeId, modelId;
                    uint.TryParse(lblMakeId.Text, out makeId);
                    uint.TryParse(dtgrdMembers.DataKeys[e.Item.ItemIndex].ToString(), out modelId);
                    deleteModelMostPopularBikes(modelId, makeId);
                }

                //Update Upcoming Bike
                if (chkFuturistic1.Checked)
                    MakeUpcomingBike(dtgrdMembers.DataKeys[e.Item.ItemIndex].ToString(), lblMakeId.Text);

                //Write URL
                if (dtgrdMembers.DataKeys[e.Item.ItemIndex].ToString() != "-1")
                {
                    Trace.Warn("Writing File" + dtgrdMembers.DataKeys[e.Item.ItemIndex]);
                    WriteFileModel(dtgrdMembers.DataKeys[e.Item.ItemIndex].ToString(), txt.Text.Trim().Replace("'", "''"));
                }
                //trigger mail to notify that a model has been discontinued
                if (chkUsed1.Checked && !chkFuturistic1.Checked && !chkNew1.Checked)
                {
                    try
                    {
                        MakeModelVersion mmv = new MakeModelVersion();
                        mmv.DiscontinueBikeModel(dtgrdMembers.DataKeys[e.Item.ItemIndex].ToString());
                        mmv.Make = cmbMakes.SelectedItem.Text;
                        mmv.Model = txt.Text;
                        IEnumerable<string> emails = Bikewale.Utility.GetEmailList.FetchMailList();
                        foreach (var mailId in emails)
                        {
                            SendEmailOnModelChange.SendModelDiscontinuedEmail(mailId, mmv.Make, mmv.Model, DateTime.Now);
                        }
                    }
                    catch (Exception ex)
                    {
                        BikeWaleOpr.Common.ErrorClass errObj = new BikeWaleOpr.Common.ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                        errObj.SendMail();
                    }
                }


                //Refresh memcache object for discontinuedbikes
                MemCachedUtil.Remove(string.Format("BW_DiscontinuedBikes_Make_{0}", lblMakeId.Text));

                //Refresh memcache object for bikeModelDetails
                MemCachedUtil.Remove(string.Format("BW_ModelDetails_{0}", dtgrdMembers.DataKeys[e.Item.ItemIndex].ToString()));
                MemCachedUtil.Remove(string.Format("BW_ModelDetail_{0}", dtgrdMembers.DataKeys[e.Item.ItemIndex].ToString()));
                MemCachedUtil.Remove(string.Format("BW_GenericBikeInfo_MO_{0}_V1", dtgrdMembers.DataKeys[e.Item.ItemIndex].ToString()));
                //Refresh memcache object for popularBikes change
                MemCachedUtil.Remove(string.Format("BW_PopularBikesByMake_{0}", lblMakeId.Text));

                //Refresh memcache object for upcoming bikes
                BikewaleOpr.Cache.BwMemCache.ClearUpcomingBikes();

            }
            catch (SqlException ex)
            {
                BikeWaleOpr.Common.ErrorClass objErr = new BikeWaleOpr.Common.ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                BikeWaleOpr.Common.ErrorClass objErr = new BikeWaleOpr.Common.ErrorClass(ex, Request.ServerVariables["URL"]);
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

        /// <summary>
        /// Modified by : Sajal Gupta on 9-1-2017
        /// Desc : Refresh popular bikes memcache keys.
        /// </summary>
        void dtgrdMembers_Delete(object sender, DataGridCommandEventArgs e)
        {
            MakeModelVersion mmv = new MakeModelVersion();
            mmv.DeleteModelVersions(dtgrdMembers.DataKeys[e.Item.ItemIndex].ToString(), BikeWaleAuthentication.GetOprUserId());
            //CLear popularBikes key
            UInt32 makeId;
            UInt32.TryParse(cmbMakes.SelectedValue, out makeId);

            uint modelId;
            uint.TryParse(dtgrdMembers.DataKeys[e.Item.ItemIndex].ToString(), out modelId);
            deleteModelMostPopularBikes(modelId, makeId);

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

        void MakeUpcomingBike(string modelId, string makeId)
        {
            string sql = "", sqlSave = "";

            sql = "select id from expectedbikelaunches where bikemodelid = " + modelId + "";

            sqlSave = "insert into expectedbikelaunches(bikemakeid, bikemodelid, islaunched) values(" + makeId + ", " + modelId + ", 0)";

            try
            {
                using (IDataReader dr = MySqlDatabase.SelectQuery(sql, ConnectionType.ReadOnly))
                {
                    if (!(dr != null && dr.Read()))
                    {
                        MySqlDatabase.InsertQuery(sqlSave, ConnectionType.MasterDatabase);
                    }
                }

            }
            catch (SqlException err)
            {
                //catch the sql exception. if it is equal to 2627, then say that it is for duplicate entry 
                Trace.Warn(err.Message);
                BikeWaleOpr.Common.ErrorClass objErr = new BikeWaleOpr.Common.ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                //catch the sql exception. if it is equal to 2627, then say that it is for duplicate entry 
                Trace.Warn(err.Message);
                BikeWaleOpr.Common.ErrorClass objErr = new BikeWaleOpr.Common.ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception
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
                    if (!Directory.Exists(mainDir))
                        Directory.CreateDirectory(mainDir);

                    //create file to store description
                    fullPath = mainDir + "\\map_models.txt";
                    Trace.Warn(fullPath);

                    if (!File.Exists(fullPath))
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
                BikeWaleOpr.Common.ErrorClass objErr = new BikeWaleOpr.Common.ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
                isSaved = false;
            } // catch Exception

            return isSaved;
        }


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
            DataTable dt = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getmodelccsegments"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (DataSet ds = MySqlDatabase.SelectAdapterQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                            dt = ds.Tables[0];
                    }
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                BikeWaleOpr.Common.ErrorClass objErr = new BikeWaleOpr.Common.ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                BikeWaleOpr.Common.ErrorClass objErr = new BikeWaleOpr.Common.ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return dt;
        }//End of GetModelCCSegments


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
                ModelIdsList = ModelIdsList.Substring(0, ModelIdsList.Length - 1);

            Trace.Warn("++ model ids ", ModelIdsList);

            try
            {
                UpdateModelSegments(ddlUpdateSegment.SelectedValue, ModelIdsList);
            }
            catch (Exception ex)
            {
                Trace.Warn(ex.Message + ex.Source);
                BikeWaleOpr.Common.ErrorClass objErr = new BikeWaleOpr.Common.ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            BindGrid();
            Page.ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Segment Updated Successfully.');", true);
        }   // End of UpdateModelSegments

        /// <summary>
        /// Created by sajal gupta on 24-05-2017
        /// description : This function will delete this particular model from popularbikes table and refresh the cache
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="makeId"></param>
        void deleteModelMostPopularBikes(uint modelId, uint makeId)
        {
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("deletemodelmostpopularbikes"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, modelId));
                    MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase);
                }

                MemCachedUtil.Remove(String.Format("BW_PopularBikesByMake_{0}", makeId));
                //CLear popularBikes key

                BikewaleOpr.Cache.BwMemCache.ClearPopularBikesCacheKey(null, makeId);
                BikewaleOpr.Cache.BwMemCache.ClearPopularBikesCacheKey(6, makeId);
                BikewaleOpr.Cache.BwMemCache.ClearPopularBikesCacheKey(9, makeId);
                BikewaleOpr.Cache.BwMemCache.ClearPopularBikesCacheKey(9, null);
            }
            catch (Exception ex)
            {
                BikeWaleOpr.Common.ErrorClass objErr = new BikeWaleOpr.Common.ErrorClass(ex, "deleteModelMostPopularBikes");
            }
        }

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
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "updatemodelsegments";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_segmentid", DbType.Int32, segmentId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelidslist", DbType.String, 500, modelIdsList));

                    MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase);
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                BikeWaleOpr.Common.ErrorClass objErr = new BikeWaleOpr.Common.ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn(err.Message + err.Source);
                BikeWaleOpr.Common.ErrorClass objErr = new BikeWaleOpr.Common.ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }   //End of UpdateModelSegments

        /// <summary>
        /// Created by : Vivek Singh Tomar on 13th Sep 2017
        /// Summary : bind the series drop down list
        /// </summary>
        private void FillSeries(int makeId)
        {
            IEnumerable<BikeSeriesEntityBase> SeriesList = null;
            try
            {
                if(makeId > 0)
                {
                    SeriesList = _series.GetSeriesByMake(makeId);
                    if(SeriesList != null)
                    {
                        ListItem li = new ListItem("---Select Series---", "-1");
                        ddlSeries.DataSource = SeriesList;
                        ddlSeries.DataTextField = "SeriesName";
                        ddlSeries.DataValueField = "SeriesId";
                        ddlSeries.DataBind();
                        ddlSeries.Items.Insert(0, li);

                        ddlUpdateSeries.DataSource = SeriesList;
                        ddlUpdateSeries.DataTextField = "SeriesName";
                        ddlUpdateSeries.DataValueField = "SeriesId";
                        ddlUpdateSeries.DataBind();
                        ddlUpdateSeries.Items.Insert(0, li);
                    }
                }
            }
            catch(Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "BikewaleOpr.Content.BikeModel: FillSeries");
            }
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 14th Sep 2017
        /// Summary : Update series of selected model and log model and series mapping
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateModelSeries(object sender, EventArgs e)
        {
            
            try
            {
                string ModelIdsList = hdnModelIdsListSeries.Value;

                if (ModelIdsList.Length > 0)
                    ModelIdsList = ModelIdsList.Substring(0, ModelIdsList.Length - 1);

                UpdateModelSeries(ddlUpdateSeries.SelectedValue, ModelIdsList);
                BindGrid();
                Page.ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Series Updated Successfully.');", true);
            }
            catch (Exception ex)
            {
                BikeWaleOpr.Common.ErrorClass objErr = new BikeWaleOpr.Common.ErrorClass(ex, Request.ServerVariables["URL"]);
            }
          
        }
        
        /// <summary>
        /// Created by : Ashutosh Sharma on 14-Sep-2017
        /// Description : Method to call DataBase update model series
        /// </summary>
        /// <param name="seriesId"></param>
        /// <param name="modelIdsList"></param>
        private void UpdateModelSeries(string seriesId, string modelIdsList)
        {
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "updatemodelseries";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_seriesid", DbType.Int32, seriesId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelidslist", DbType.String, 500, modelIdsList));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_updatedby", DbType.Int32, BikeWaleOpr.Common.CurrentUser.Id));


                    MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase);
                }
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn(err.Message + err.Source);
                BikeWaleOpr.Common.ErrorClass objErr = new BikeWaleOpr.Common.ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }
    }
}