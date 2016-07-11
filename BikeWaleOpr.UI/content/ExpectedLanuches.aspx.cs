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
using System.Drawing.Imaging;
using Ajax;
using System.IO;
using System.Configuration;
using System.Data.Common;
using BikeWaleOPR.Utilities;
using MySql.CoreDAL;

namespace BikeWaleOpr.Content
{
    public class ExpectedLaunches : Page
    {
        protected HtmlGenericControl spnError;
        protected DataGrid dtgrdLaunches;
        protected int serialNo = 0;
        protected Button btnSave;

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

        void btnSave_Click(object sender, EventArgs e)
        {
            string selId = "";
            string selModelId = "";
            
            for (int i = 0; i < dtgrdLaunches.Items.Count; i++)
            {
                CheckBox chkLaunched = (CheckBox)dtgrdLaunches.Items[i].FindControl("chkLaunched");
                Label lblId = (Label)dtgrdLaunches.Items[i].FindControl("lblId");
                Label lblModelId = (Label)dtgrdLaunches.Items[i].FindControl("lblModelId");

                if (chkLaunched.Checked)
                {
                    selId += lblId.Text + ",";
                    selModelId += lblModelId.Text + ",";
                }
            }

            if (selId != "" && selModelId != "")
            {
                selId = selId.Substring(0, selId.Length - 1);
                selModelId = selModelId.Substring(0,selModelId.Length - 1);
            
                UpdateBikeIsLaunched(selId, selModelId);

                BindGrid(false);             
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
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_bikemodelid", DbParamTypeMapper.GetInstance[SqlDbType.Int], bikeModelId));
                    MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase);
                }
            }
            catch (SqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }//End of DeleteExpectedLaunchBike
		
        void BindGrid(bool IsPaging)
        {
            string sql = "";
            int pageSize = dtgrdLaunches.PageSize;

            sql = @" SELECT ec.Id, ec.LaunchDate, ec.ExpectedLaunch, ec.BikeModelId, concat(cma.Name ,'-', cmo.Name) AS BikeName 
                , ec.EstimatedPriceMin, ec.EstimatedPriceMax, ec.HostURL, ec.SmallPicImagePath ,ec.LargePicImagePath
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
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
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
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_sellaunchbikeids", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 30, launchBikeIds));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_sellauchmodelids", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 30, launchBikeModelIds));
                    MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase);
                }
            }
            catch (SqlException sqlEx)
            {
                Trace.Warn("Sql Exception ", sqlEx.Message);
                ErrorClass errObj = new ErrorClass(sqlEx, HttpContext.Current.Request.ServerVariables["URL"]);
                errObj.SendMail();
            }
            catch (Exception ex)
            {
                Trace.Warn("Exception ", ex.Message);
                ErrorClass errObj = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                errObj.SendMail();
            }
        }//End UpdateBikeIsLaunched
    } // class
} // namespace