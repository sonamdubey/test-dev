using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Bikewale.Common;
using Bikewale;
using System.Data;
using System.Data.SqlClient;
using Enyim.Caching;
using System.Configuration;
using System.Data.Common;
using Bikewale.Notifications.CoreDAL;

namespace Bikewale.MyBikeWale
{
    /// <summary>
    ///     Created By : Ashish G. Kamble on 9/9/2012
    ///     Class to remove the current listing from bikewale
    /// </summary>
    public class RemoveFromListing : System.Web.UI.Page
    {
        protected DropDownList drpStatus;
        protected Button btnSave;
        protected Label lblMsg, lblRemoveStatus;
        protected TextBox txtComments;
        protected HtmlContainerControl div_RemoveInquiry;

        public string inquiryId = "",inquiryType = "";
        public string isRemovedListing = "0";
        
        bool _isMemcachedUsed;
        protected static MemcachedClient _mc = null;

        public RemoveFromListing()
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

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
            this.btnSave.Click += new EventHandler(btnSave_Click);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["Id"] == null || !CommonOpn.CheckId(Request["Id"]))
            {
                btnSave.Enabled = false;
                return;
            }
            else
                inquiryId = Request["Id"].ToString();

            if (Request["type"] == null || !CommonOpn.CheckId(Request["type"]))
            {
                btnSave.Enabled = false;
                return;
            }
            else
                inquiryType = Request["type"].ToString();

            if(!IsPostBack)
            {
                FillStatusSell();
                lblRemoveStatus.Visible = false;
            }
        }

        protected void FillStatusSell()
        {
            drpStatus.Items.Add(new ListItem("-- Reason to remove from listing --", "0"));
            drpStatus.Items.Add(new ListItem("Sold through bikewale.com", "1"));
            drpStatus.Items.Add(new ListItem("Sold through other dealer/individual", "2"));
            drpStatus.Items.Add(new ListItem("I am not satisfied with bikewale.com services", "3"));
            drpStatus.Items.Add(new ListItem("I am not selling it any more", "4"));

            lblMsg.Text = "Please choose a reason to remove the bike from listing and then click 'Remove My Bike' button.";
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            UpdateClassifiedInquirySoldStatus();
            UpdateSoldStatus();

            //div_RemoveInquiry.Visible = false;
            
            //lblRemoveStatus.Text = "Your inquiry has been removed from bikewale listing.";
            //lblRemoveStatus.Visible = true;

            //this line invalidate memcache to get updated used bike count from live listing
            _mc.Remove("BW_ModelWiseUsedBikesCount");
            isRemovedListing = "1";
        }

        protected void UpdateClassifiedInquirySoldStatus()
        {
            string sql = "";

            sql = " insert into classifiedinquirysoldstatus (reason, comments, inquiryid) values (@reason, @comments, @inquiryid) ";

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand(sql))
                {
                    cmd.Parameters.Add(DbFactory.GetDbParam("@reason", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 100, drpStatus.SelectedItem.Text));
                    cmd.Parameters.Add(DbFactory.GetDbParam("@comments", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 100, txtComments.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("@inquiryid", DbParamTypeMapper.GetInstance[SqlDbType.BigInt], inquiryId)); 

                    MySqlDatabase.InsertQuery(cmd);
                }
            }
            catch (SqlException err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception

        }   // End of UpdateClassifiedInquirySoldStatus method

        /// <summary>
        /// Modified By : Sadhana Upadhyay on 15 Oct 2014
        /// Summary : set IsApproved 1 to maintain count in bikewale opr
        /// </summary>
        protected void UpdateSoldStatus()
        {
            string sql = " update dbo.classifiedindividualsellinquiries set statusid=3, isapproved = 1 where id = @inquiryid ";

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand(sql))
                {
                    //cmd.Parameters.Add("@InquiryId", SqlDbType.BigInt).Value = inquiryId;
                    cmd.Parameters.Add(DbFactory.GetDbParam("@inquiryid", DbParamTypeMapper.GetInstance[SqlDbType.BigInt], inquiryId));
                    MySqlDatabase.UpdateQuery(cmd);                                                                                                        
                }
            }
            catch (SqlException err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception

        }   // End of UpdateSoldStatus method

    }   // End of class
}   // End of namespace