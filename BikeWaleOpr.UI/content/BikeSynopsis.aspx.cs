using BikewaleOpr.common;
using BikeWaleOpr.Common;
using FreeTextBoxControls;
using MySql.CoreDAL;
// C# Document
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BikeWaleOpr.Content
{
    public class AddBikeSynopsis : Page
    {
        protected Button btnSave;
        protected FreeTextBox ftbDescription;
        protected Label lblMessage, lbl_success_msg;
        protected TextBox txtPros, txtCons, txtSmallDesc;
        protected DropDownList drpLooks, drpPerformance, drpFuel, drpComfort, drpSafety, drpInteriors,
                    drpRide, drpHandling, drpBraking, drpOverall;

        public string qryStrModel = "";
        public string bikeName = "";

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
            this.btnSave.Click += new System.EventHandler(btnSave_OnClick);
        }

        void Page_Load(object Sender, EventArgs e)
        {

            if (Request.QueryString["model"] != null && Request.QueryString["model"].ToString() != "")
            {
                qryStrModel = Request.QueryString["model"].ToString();
                if (!CommonOpn.CheckId(qryStrModel))
                {
                    Response.Redirect("bikemodels.aspx");
                }
            }
            else
            {
                Response.Redirect("bikemodels.aspx");
            }

            GetBikeName(qryStrModel);

            if (!IsPostBack)
            {
                FillRatings(drpLooks);
                FillRatings(drpPerformance);
                FillRatings(drpFuel);
                FillRatings(drpComfort);
                FillRatings(drpSafety);
                FillRatings(drpInteriors);
                FillRatings(drpRide);
                FillRatings(drpHandling);
                FillRatings(drpBraking);
                FillRatings(drpOverall);

                //Fill Existing data if Exist
                //GetBikeName(qryStrModel);
                FillExistingData(qryStrModel);
            }

        }
        /// <summary>
        /// Modified by : Vivek Singh Tomar on 27th Sep 2017
        /// Summary : Changed version of cache key
        /// Modified by : Ashutosh Sharma on 29 Sep 2017 
        /// Description : Changed cache key from 'BW_ModelDetail_' to 'BW_ModelDetail_V1_'.
        /// Modified by : Rajan Chauhan on 06 Feb 2018.
        /// Description : Changed version of key from 'BW_ModelDetail_V1_' to 'BW_ModelDetail_'.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSave_OnClick(object sender, EventArgs e)
        {
            string saveId = "";

            if (lblMessage.Text.Trim() != "")
            {
                saveId = SaveData(lblMessage.Text);
            }
            else
            {
                saveId = SaveData("-1");
            }

            if (saveId != "" && saveId != "0")
            {

                lbl_success_msg.Text = "Data Saved Successfully";
                lbl_success_msg.Visible = true;
            }
            FillExistingData(Request.QueryString["model"]);

            //Refresh memcache object for ModelDescription change for app
            MemCachedUtility.Remove(string.Format("BW_ModelDesc_{0}", Request.QueryString["model"]));

            //Refresh memcache object for ModelDescription change for desktop site
            MemCachedUtility.Remove(string.Format("BW_ModelDetails_{0}", Request.QueryString["model"]));
            MemCachedUtility.Remove(string.Format("BW_ModelDetail_V1_{0}", Request.QueryString["model"]));
        }

        void FillRatings(DropDownList drpName)
        {
            for (int i = 10; i > 0; i--)
            {
                drpName.Items.Insert(0, i.ToString());
            }

            drpName.SelectedValue = "5";
        }

        //Fill the data if exist for the current model
        void FillExistingData(string modelId)
        {
            string sql = "";

            int _modelid = default(int);
            if (!string.IsNullOrEmpty(modelId) && int.TryParse(modelId, out _modelid))
            {
                sql = " select * from bikesynopsis where modelid = " + _modelid + " and isactive = 1";

            }

            try
            {
                if (!string.IsNullOrEmpty(sql))
                {
                    using (DbCommand cmd = DbFactory.GetDBCommand(sql))
                    {
                        using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                        {
                            if (dr != null && dr.Read())
                            {
                                ftbDescription.Text = dr["FullDescription"].ToString();
                                txtSmallDesc.Text = dr["SmallDescription"].ToString();
                                txtPros.Text = dr["Pros"].ToString();
                                txtCons.Text = dr["Cons"].ToString();
                                drpLooks.SelectedValue = dr["Looks"].ToString();
                                drpPerformance.SelectedValue = dr["Performance"].ToString();
                                drpFuel.SelectedValue = dr["FuelEfficiency"].ToString();
                                drpComfort.SelectedValue = dr["Comfort"].ToString();
                                drpSafety.SelectedValue = dr["Safety"].ToString();
                                drpInteriors.SelectedValue = dr["Interiors"].ToString();
                                drpRide.SelectedValue = dr["RideQuality"].ToString();
                                drpHandling.SelectedValue = dr["Handling"].ToString();
                                drpBraking.SelectedValue = dr["Braking"].ToString();
                                drpOverall.SelectedValue = dr["Overall"].ToString();
                                lblMessage.Text = dr["Id"].ToString();
                            }
                        }
                    }
                }
            }
            catch (SqlException err)
            {
                Trace.Warn(err.Message);
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);
                
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);
                
            }
        }

        //Function To Get the Bike Name
        void GetBikeName(string modelId)
        {
            string sql = "";
            int _modelid = default(int);
            if (!string.IsNullOrEmpty(modelId) && int.TryParse(modelId, out _modelid))
            {
                sql = @" select concat(cma.name,' ',cmo.name) as bikename
				 from bikemakes as cma, bikemodels as cmo
				 where cma.id = cmo.bikemakeid and cmo.id = " + _modelid + " and cma.isdeleted = 0";

            }
            try
            {
                if (!string.IsNullOrEmpty(sql))
                {
                    using (DbCommand cmd = DbFactory.GetDBCommand(sql))
                    {
                        using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                        {
                            if (dr != null && dr.Read())
                            {
                                bikeName = dr["BikeName"].ToString();
                            }
                        }
                    }
                }
            }
            catch (SqlException err)
            {
                Trace.Warn(err.Message);
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);
                
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);
                
            }
        }

        //Function to read features and descriptions from the saved text file
        string ReadOtherDetails(string filePath)
        {
            string strRead = "";

            StreamReader sr = new StreamReader(filePath);
            strRead = sr.ReadToEnd();
            sr.Close();

            return strRead;
        }

        string SaveData(string updateId)
        {
            string lastSavedId = "";

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("con_addbikesynopsis"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_id", DbType.Int64, updateId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int64, qryStrModel));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_fulldescription", DbType.String, ftbDescription.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_smalldescription", DbType.String, 8000, txtSmallDesc.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_pros", DbType.String, 500, txtPros.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cons", DbType.String, 500, txtCons.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_looks", DbType.Int16, drpLooks.SelectedItem.Value));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_performance", DbType.Int16, drpPerformance.SelectedItem.Value));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_fuel", DbType.Int16, drpFuel.SelectedItem.Value));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_comfort", DbType.Int16, drpComfort.SelectedItem.Value));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_safety", DbType.Int16, drpSafety.SelectedItem.Value));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_interiors", DbType.Int16, drpInteriors.SelectedItem.Value));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_ride", DbType.Int16, drpRide.SelectedItem.Value));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_handling", DbType.Int16, drpHandling.SelectedItem.Value));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_braking", DbType.Int16, drpBraking.SelectedItem.Value));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_overall", DbType.Int16, drpOverall.SelectedItem.Value));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isactive", DbType.Boolean, 1));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_entrydatetime", DbType.DateTime, DateTime.Now));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_lastupdated", DbType.DateTime, DateTime.Now));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_lastsavedid", DbType.Int64, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_userid", DbType.Int32, CurrentUser.Id));


                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);

                    if (cmd.Parameters["par_lastsavedid"].Value.ToString() != "")
                        lastSavedId = cmd.Parameters["par_lastsavedid"].Value.ToString();
                }

            }
            catch (SqlException err)
            {
                Trace.Warn(err.Message);
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);
                
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);
                
            }

            return lastSavedId;
        }

        //Function to save description in the seperate file for each new inserted part.
        bool SaveDescription(string itemId)
        {
            bool isSaved = false;
            string fullPath = "";
            string mainDir = "";

            try
            {
                //string mainDir = CommonOpn.ResolvePhysicalPath("/Contents/Modeldescriptions/");
                if (HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToLower().IndexOf("carwale.com") >= 0)
                {
                    mainDir = CommonOpn.ResolvePhysicalPath("/CarSynopsis/ModelDescriptions/");
                }
                else if (HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToLower().IndexOf("localhost") >= 0)
                {
                    mainDir = CommonOpn.ResolvePhysicalPath("/content/ModelDescription/");
                }
                else
                {
                    mainDir = CommonOpn.ResolvePhysicalPath("/content/ModelDescription/").Replace("carwale", "bikewaleopr");
                }

                //check whether the directory for the make exists or not, if not then create the directory
                if (Directory.Exists(mainDir) == false)
                    Directory.CreateDirectory(mainDir);

                //create file to store description
                fullPath = mainDir + "\\" + itemId + ".txt";

                Trace.Warn(fullPath);
                StreamWriter sw = File.CreateText(fullPath);
                sw.Write(ftbDescription.Text.Trim());
                sw.Flush();
                sw.Close();
                isSaved = true;
            }
            catch (Exception err)
            {
                //catch the sql exception. if it is equal to 2627, then say that it is for duplicate entry 
                Trace.Warn(err.Message);
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);
                
                isSaved = false;
            } // catch Exception

            return isSaved;
        }

    }//class
}// namespace