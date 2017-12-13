﻿using AmpCacheRefreshLibrary;
using Bikewale.Utility;
using BikewaleOpr.Cache;
using BikewaleOpr.common;
using BikewaleOpr.DALs.Bikedata;
using BikewaleOpr.Interface.BikeData;
using BikeWaleOpr.Common;
using Microsoft.Practices.Unity;
using MySql.CoreDAL;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace BikeWaleOpr.Content
{
    public class ExpectedLaunches : Page
    {
        protected HtmlGenericControl spnError;
        protected DataGrid dtgrdLaunches;
        protected int serialNo = 0;
        protected Button btnSave;
        string selModelId = "";

        private readonly IBikeMakes _makes = null;

        public ExpectedLaunches()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IBikeMakesRepository, BikeMakesRepository>()
                    .RegisterType<IBikeMakes, BikewaleOpr.BAL.BikeMakes>();
                _makes = container.Resolve<IBikeMakes>();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
            dtgrdLaunches.PageIndexChanged += new DataGridPageChangedEventHandler(Page_Change);
            dtgrdLaunches.DeleteCommand += new DataGridCommandEventHandler(dtgrdLaunches_Delete);
            btnSave.Click += new EventHandler(btnSave_Click);
        }

        void Page_Load(object Sender, EventArgs e)
        {

            Ajax.Utility.RegisterTypeForAjax(typeof(AjaxFunctions));

            if (!IsPostBack)
            {
                AjaxFunctions aj = new AjaxFunctions();
                /* string result = aj.UpdateVerPhotos("10");
                spnError.InnerHtml = result;*/
                //aj.SavePhoto("","", "335");
                Trace.Warn(ConfigurationManager.AppSettings["imgHostURL"]);
                BindGrid(false);
            }
        } // Page_Load

        /// <summary>
        /// Modified by : Sajal Gupta on 15-12-16
        /// Desc : Refreshed modeldetails key for new bike launch.
        /// Modified by : Sangram Nandkhile on 5th Jan 2017
        /// Desc : Refreshed modeldetails, launch Details, PopularBikesByMake_ keys for new bike launch.
        /// Modified by : Sajal Gupta on 9-1-2017
        /// Desc : Refreshed popularbikekeys for new bike launch.
        /// Modified by : Aditi Srivastava on 12 Jan 2017
        /// Desc        : Refreshed upcoming bikes key on new bike launch
        /// Modified by :   Sumit Kate on 10 Feb 2017
        /// Description :   Clear BW_NewLaunchedBikes memcache object
        /// Modified by : Vivek Singh Tomar on 27th Sep 2017
        /// Summary : Changed version of cache key
        /// Modified by : Ashutosh Sharma on 29 Sep 2017 
        /// Description : Changed cache key from 'BW_ModelDetail_' to 'BW_ModelDetail_V1_'.
        /// Modified by : Ashutosh Sharma on 28 Nov 2017
        /// Description : Added call to ClearSeriesCache.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string selId = string.Empty;
                string makeIdList = string.Empty;
                for (int i = 0; i < dtgrdLaunches.Items.Count; i++)
                {
                    CheckBox chkLaunched = (CheckBox)dtgrdLaunches.Items[i].FindControl("chkLaunched");
                    if (chkLaunched.Checked)
                    {
                        Label lblId = (Label)dtgrdLaunches.Items[i].FindControl("lblId");
                        Label lblModelId = (Label)dtgrdLaunches.Items[i].FindControl("lblModelId");
                        Label lblMakeId = (Label)dtgrdLaunches.Items[i].FindControl("lblMakeId");
                        Label lblSeriesId = (Label)dtgrdLaunches.Items[i].FindControl("lblSeriesId");

                        if (lblId != null)
                            selId += lblId.Text + ",";
                        if (lblMakeId != null)
                            selModelId += lblMakeId.Text + ",";

                        UInt32 makeId, modelId;
                        UInt32.TryParse(lblModelId.Text, out modelId);
                        UInt32.TryParse(lblMakeId.Text, out makeId);
                        //Refresh memcache object for newbikelaunches
                        if (modelId > 0)
                        {
                            selModelId += modelId + ",";
                            MemCachedUtility.Remove(String.Format("BW_ModelDetails_{0}", modelId));
                            MemCachedUtility.Remove(String.Format("BW_ModelDetail_V1_{0}", modelId));
                            MemCachedUtility.Remove(String.Format("BW_GenericBikeInfo_MO_{0}_V1", modelId));
                        }
                        if (makeId > 0)
                        {
                            MemCachedUtility.Remove(String.Format("BW_PopularBikesByMake_{0}", lblMakeId.Text));
                            //CLear popularBikes key

                            BwMemCache.ClearPopularBikesCacheKey(null, makeId);
                            BwMemCache.ClearPopularBikesCacheKey(6, makeId);
                            BwMemCache.ClearPopularBikesCacheKey(9, makeId);
                            BwMemCache.ClearPopularBikesCacheKey(9, null);

                            RefreshAmpContent(makeId);

                        }
                        if (modelId > 0 && makeId > 0)
                        {
                            //Clear upcoming bikes key

                            for (int so = 0; so < 5; so++)
                            {
                                BwMemCache.ClearUpcomingBikesCacheKey(9, so, null, null);
                                BwMemCache.ClearUpcomingBikesCacheKey(9, so, makeId, null);
                                BwMemCache.ClearUpcomingBikesCacheKey(9, so, null, modelId);
                                BwMemCache.ClearUpcomingBikesCacheKey(9, so, makeId, modelId);
                            }
                        }
                        if (lblMakeId != null && lblSeriesId != null)
                        {
                            BwMemCache.ClearSeriesCache(Convert.ToUInt32(lblSeriesId.Text), Convert.ToUInt32(lblMakeId.Text));
                        }
                    }
                }
                if (selId.Length > 0 && selModelId.Length > 0)
                {
                    selId = selId.Substring(0, selId.Length - 1);
                    selModelId = selModelId.Substring(0, selModelId.Length - 1);
                    UpdateBikeIsLaunched(selId, selModelId);
                    BindGrid(false);
                }
                //Refresh memcache object for newbikelaunches
                MemCachedUtility.Remove("BW_NewLaunchedBikes_SI_1_EI_10");
                MemCachedUtility.Remove("BW_NewBikeLaunches");
                MemCachedUtility.Remove("BW_NewLaunchedBikes");

            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, string.Format("Error at ExpectedLaunches.btnSave_Click() ==> {0}", selModelId));
            }

        }   // End btn_Save_click function


        /// <summary>
        /// Written By  : Ashwini Todkar on 14 Feb 2014
        /// Summary     : Method deletes upcoming bike model from expected bike launches
        /// </summary>
        /// <param name="bikeModelId"></param>

        public void DeleteExpectedLaunchBike(string bikeModelId)
        {
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("deleteexpectedlaunchbike"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_bikemodelid", DbType.Int32, bikeModelId));
                    MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase);
                }
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);
            }
        }//End of DeleteExpectedLaunchBike


        /// <summary>
        /// Modified by : Ashutosh Sharma on 28 Nov 2017
        /// Description : Added cmo.BikeSeriesId in sql query.
        /// </summary>
        /// <param name="IsPaging"></param>
        void BindGrid(bool IsPaging)
        {
            string sql = "";
            int pageSize = dtgrdLaunches.PageSize;

            sql = @" SELECT ec.Id, ec.BikeMakeId, ec.LaunchDate, ec.ExpectedLaunch, ec.BikeModelId, concat(cma.Name ,'-', cmo.Name) AS BikeName 
                , ec.EstimatedPriceMin, ec.EstimatedPriceMax, ec.HostURL, ec.SmallPicImagePath ,ec.LargePicImagePath, cmo.BikeSeriesId
                from expectedbikelaunches ec 
                left join bikemodels cmo on ec.bikemodelid = cmo.id 
                left join bikemakes cma on cmo.bikemakeid = cma.id 
                where islaunched=0 and ec.isdeleted = 0 
                order by ec.launchdate desc ";

            try
            {

                using (DataSet ds = MySqlDatabase.SelectAdapterQuery(sql, ConnectionType.ReadOnly))
                {
                    if (ds != null && ds.Tables != null)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            spnError.InnerHtml = " ";
                        }
                        else
                        {
                            spnError.InnerHtml = "Sorry! No Record Exists. ";
                        }

                        if (ds.Tables[0].Rows.Count > pageSize)
                        {
                            dtgrdLaunches.AllowPaging = true;
                            dtgrdLaunches.PageSize = pageSize;
                        }
                        else
                            dtgrdLaunches.AllowPaging = false;

                        if (!IsPaging)
                            dtgrdLaunches.CurrentPageIndex = 0;

                        dtgrdLaunches.DataSource = ds;
                        dtgrdLaunches.DataBind();

                        if (dtgrdLaunches.Items.Count > 0)
                        {
                            if (dtgrdLaunches.CurrentPageIndex == 0)
                                serialNo = 0;
                            else
                                serialNo = pageSize * dtgrdLaunches.CurrentPageIndex;
                        }
                    }
                }
            }
            catch (SqlException err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);

            }
            catch (Exception err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);

            }
        }

        void Page_Change(object sender, DataGridPageChangedEventArgs e)
        {
            // Set CurrentPageIndex to the page the user clicked.
            dtgrdLaunches.CurrentPageIndex = e.NewPageIndex;
            BindGrid(true);
        }

        void dtgrdLaunches_Delete(object sender, DataGridCommandEventArgs e)
        {
            string modelId = string.Empty;

            modelId = ((Label)e.Item.FindControl("lblModelId")).Text;
            Trace.Warn("+++", modelId);
            DeleteExpectedLaunchBike(modelId);

            BindGrid(false);
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 17 Feb 2014
        /// Summary    : method updates New ,Used and futuristic flags of bikemodels and bikeversion table
        ///              also update isLaunched flags in ExpectedBikeLaunch table
        /// Modified By : Sushil Kumar on 9th July 2017
        /// Description : Change input parametres as per carwale mysql master base conventions         
        /// </summary>
        /// <param name="launchBikeIds"></param>
        /// <param name="launchBikeModelIds"></param>
        protected void UpdateBikeIsLaunched(string launchBikeIds, string launchBikeModelIds)
        {
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("updatebikeislaunched"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_sellaunchbikeids", DbType.String, 30, launchBikeIds));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_sellauchmodelids", DbType.String, 30, launchBikeModelIds));
                    MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase);

                    if (!string.IsNullOrEmpty(launchBikeModelIds))
                    {
                        foreach (string modelId in launchBikeModelIds.Split(','))
                        {
                            NameValueCollection nvc = new NameValueCollection();
                            nvc.Add("v_MakeId", null);
                            nvc.Add("v_ModelName", null);
                            nvc.Add("v_ModelMaskingName", null);
                            nvc.Add("v_HostUrl", null);
                            nvc.Add("v_OriginalImagePath", null);
                            nvc.Add("v_IsUsed", "1");
                            nvc.Add("v_IsNew", "1");
                            nvc.Add("v_IsFuturistic", "0");
                            nvc.Add("v_IsDeleted", null);
                            nvc.Add("v_ModelId", modelId);
                            SyncBWData.PushToQueue("BW_UpdateBikeModels", DataBaseName.CW, nvc);

                        }
                    }

                }
            }
            catch (SqlException sqlEx)
            {
                Trace.Warn("Sql Exception ", sqlEx.Message);
                ErrorClass.LogError(sqlEx, HttpContext.Current.Request.ServerVariables["URL"]);

            }
            catch (Exception ex)
            {
                Trace.Warn("Exception ", ex.Message);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

            }
        }//End UpdateBikeIsLaunched

        private void RefreshAmpContent(uint makeId)
        {
            var makeDetails = _makes.GetMakeDetailsById(makeId);
            string makeUrl = string.Format("{0}/m/{1}amp", BWConfiguration.Instance.BwHostUrl, Bikewale.Utility.UrlFormatter.CreateMakeUrl(makeDetails.MaskingName));
            string privateKeyPath = HttpContext.Current.Server.MapPath("~/App_Data/private-key.pem");
            GoogleAmpCacheRefreshCall.UpdateAmpCache(makeUrl, privateKeyPath);
        }
    } // class
} // namespace