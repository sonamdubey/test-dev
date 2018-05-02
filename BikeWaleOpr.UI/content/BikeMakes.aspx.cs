/*******************************************************************************************************
IN THIS CLASS THE NEW MEMBEERS WHO HAVE REQUESTED FOR REGISTRATION ARE SHOWN
*******************************************************************************************************/

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
    public class BikeMakes : Page
    {
        protected HtmlGenericControl spnError;
        protected TextBox txtMake, txtMaskingName;
        protected Button btnSave;
        protected DataGrid dtgrdMembers;
        protected Label lblStatus;

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
        }

        void Page_Load(object Sender, EventArgs e)
        {
            lblStatus.Text = "";
            if (!IsPostBack)
            {
                SortDirection = "";
                SortCriteria = "Name";

                BindGrid();
            }
        } // Page_Load

        /// <summary>
        /// Modified By : Sushil Kumar on 9th July 2017
        /// Description : Change input parametres as per carwale mysql master base conventions
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        void btnSave_Click(object Sender, EventArgs e)
        {
            Page.Validate();
            int _makeId = 0;
            if (!Page.IsValid) return;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "insertbikemake";
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_make", DbType.String, txtMake.Text.Trim().Replace("'", "''")));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makemaskingname", DbType.String, txtMaskingName.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_userid", DbType.Int32, BikeWaleAuthentication.GetOprUserId()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_ismakeexist", DbType.Boolean, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, ParameterDirection.Output));

                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);

                    // If make already exists
                    if (Convert.ToBoolean(cmd.Parameters["par_ismakeexist"].Value))
                    {
                        lblStatus.Text = "Make name or make masking name already exists. Can not insert duplicate name.";
                    }
                    _makeId = Convert.ToUInt16(cmd.Parameters["par_makeid"].Value);
                    if (_makeId > 0)
                    {
                        // Create name value collection
                        NameValueCollection nvc = new NameValueCollection();
                        nvc.Add("v_MakeId", _makeId.ToString());
                        nvc.Add("v_MakeName", txtMake.Text.Trim().Replace("'", "''"));
                        nvc.Add("v_MaskingName", txtMaskingName.Text.Trim());
                        nvc.Add("v_Futuristic", "0");
                        nvc.Add("v_Used", "1");
                        nvc.Add("v_New", "1");
                        SyncBWData.PushToQueue("BW_AddBikeMakes", DataBaseName.CW, nvc);
                    }
                }
            }
            catch (SqlException ex)
            {
                Trace.Warn("Error", ex.Message + ex.Source);
                ErrorClass.LogError(ex, Request.ServerVariables["URL"]);
                
            }
            catch (Exception ex)
            {
                Trace.Warn(ex.Message + ex.Source);
                ErrorClass.LogError(ex, Request.ServerVariables["URL"]);
                
            }
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

            sql = @" select bm.id, bm.name,bm.maskingname,
                        if(bm.used,true,false) as used ,if(bm.new,true,false) as new, if(bm.futuristic,true,false) as futuristic ,
                        bm.macreatedon as createdon,
                        bm.maupdatedon as updatedon,
                        ou.username as updatedby 
                    from bikemakes bm left join oprusers ou on bm.maupdatedby = ou.id where isdeleted=0 ";

            if (SortCriteria != "")
                sql += " order by bm.futuristic desc,bm.new desc,bm.used desc," + SortCriteria + " " + SortDirection;

            Trace.Warn(sql);
            CommonOpn objCom = new CommonOpn();
            try
            {
                objCom.BindGridSet(sql, dtgrdMembers, pageSize);
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);
                
            }

        }

        void dtgrdMembers_Edit(object sender, DataGridCommandEventArgs e)
        {
            dtgrdMembers.EditItemIndex = e.Item.ItemIndex;
            BindGrid();
            //req1.Enabled = false;
            btnSave.Enabled = false;
        }

        /// <summary>
        /// Modified By : Sushil Kumar on 9th July 2017
        /// Description : Change input parametres as per carwale mysql master base conventions
        /// Modified By : Deepak Israni on 20 April 2018
        /// Description : Versioned the cache key for bikemake description
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dtgrdMembers_Update(object sender, DataGridCommandEventArgs e)
        {
            Page.Validate();
            if (!Page.IsValid) return;

            //string sql;
            CheckBox chkFuturistic = (CheckBox)e.Item.FindControl("chkFut");
            CheckBox chkUsed = (CheckBox)e.Item.FindControl("chkUsed");
            CheckBox chkNew = (CheckBox)e.Item.FindControl("chkNew");
            TextBox txt = (TextBox)e.Item.FindControl("txtMake");
            var makeid = dtgrdMembers.DataKeys[e.Item.ItemIndex];

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("updatebikemake"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_make", DbType.String, 100, txt.Text.Trim().Replace("'", "''")));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, dtgrdMembers.DataKeys[e.Item.ItemIndex]));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isfuturistic", DbType.Byte, Convert.ToInt16(chkFuturistic.Checked)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isnew", DbType.Byte, Convert.ToInt16(chkNew.Checked)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isused", DbType.Byte, Convert.ToInt16(chkUsed.Checked)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_userid", DbType.Int32, BikeWaleAuthentication.GetOprUserId()));
                    MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase);

                    // Push the data to carwale DB
                    // Create name value collection
                    NameValueCollection nvc = new NameValueCollection();
                    nvc.Add("v_MakeId", dtgrdMembers.DataKeys[e.Item.ItemIndex].ToString());
                    nvc.Add("v_MakeName", txt.Text.Trim().Replace("'", "''"));
                    nvc.Add("v_IsNew", Convert.ToInt16(chkNew.Checked).ToString());
                    nvc.Add("v_IsUsed", Convert.ToInt16(chkUsed.Checked).ToString());
                    nvc.Add("v_IsFuturistic", Convert.ToInt16(chkFuturistic.Checked).ToString());
                    nvc.Add("v_MaskingName", null);
                    nvc.Add("v_IsDeleted", null);
                    SyncBWData.PushToQueue("BW_UpdateBikeMakes", DataBaseName.CW, nvc);

                    //Refresh memcache object for bikemake description change
                    MemCachedUtility.Remove(string.Format("BW_MakeDetails_{0}_V1", makeid));

                    //Refresh memcache object for popularBikes change
                    MemCachedUtility.Remove(string.Format("BW_PopularBikesByMake_{0}", makeid));

                    //Refresh memcache object for BikeMake change
                    MemCachedUtility.Remove("BW_BikeMakes");


                }

            }
            catch (SqlException ex)
            {
                Trace.Warn(ex.Message + ex.Source);
                ErrorClass.LogError(ex, Request.ServerVariables["URL"]);
                

                Trace.Warn("number : " + ex.Number + " : error code : " + ex.ErrorCode);
                // Error code Unique key constraint in the database.
                if (ex.Number == 2627)
                {
                    lblStatus.Text = "Make name or make masking name already exists. Can not insert duplicate name";
                }
            }
            catch (Exception ex)
            {
                Trace.Warn(ex.Message + ex.Source);
                ErrorClass.LogError(ex, Request.ServerVariables["URL"]);
                
            }

            dtgrdMembers.EditItemIndex = -1;
            //req1.Enabled = true;
            btnSave.Enabled = true;
            BindGrid();
        }

        void dtgrdMembers_Cancel(object sender, DataGridCommandEventArgs e)
        {
            dtgrdMembers.EditItemIndex = -1;
            BindGrid();
            //req1.Enabled = true;
            btnSave.Enabled = true;
        }

        /// <summary>
        /// Modified by : Sajal Gupta on 9-1-2017
        /// Desc : Refresh popular bikes memcache keys.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dtgrdMembers_Delete(object sender, DataGridCommandEventArgs e)
        {
            MakeModelVersion mmv = new MakeModelVersion();
            mmv.DeleteMakeModelVersion(dtgrdMembers.DataKeys[e.Item.ItemIndex].ToString(), BikeWaleAuthentication.GetOprUserId());
            //CLear popularBikes key
            UInt32 makeId;
            UInt32.TryParse(Convert.ToString(dtgrdMembers.DataKeys[e.Item.ItemIndex]), out makeId);
            BikewaleOpr.Cache.BwMemCache.ClearPopularBikesCacheKey(null, makeId);
            BikewaleOpr.Cache.BwMemCache.ClearPopularBikesCacheKey(6, makeId);
            BikewaleOpr.Cache.BwMemCache.ClearPopularBikesCacheKey(9, makeId);
            BikewaleOpr.Cache.BwMemCache.ClearPopularBikesCacheKey(9, null);
            BindGrid();
        }

        void Page_Change(object sender, DataGridPageChangedEventArgs e)
        {
            // Set CurrentPageIndex to the page the user clicked.
            dtgrdMembers.CurrentPageIndex = e.NewPageIndex;
            BindGrid();
        }


        // <summary>
        /// this function sorts the dataset based on given criteria
        /// </summary>
        /// <paramname="sender"></param>
        /// <paramname="e"></param>
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
    } // class
} // namespace