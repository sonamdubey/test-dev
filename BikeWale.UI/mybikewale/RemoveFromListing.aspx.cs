using Bikewale.BAL.Customer;
using Bikewale.BAL.MobileVerification;
using Bikewale.BAL.Pager;
using Bikewale.BAL.UsedBikes;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.DAL.BikeData;
using Bikewale.DAL.Customer;
using Bikewale.DAL.MobileVerification;
using Bikewale.DAL.Used;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Customer;
using Bikewale.Entities.Used;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Customer;
using Bikewale.Interfaces.MobileVerification;
using Bikewale.Interfaces.Pager;
using Bikewale.Interfaces.Used;
using Enyim.Caching;
using Microsoft.Practices.Unity;
using MySql.CoreDAL;
using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

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

        public string inquiryId = "";
        public string isRemovedListing = "0";

        bool _isMemcachedUsed;
        protected static MemcachedClient _mc = null;

        protected bool isAuthorised = false;

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

        /// <summary>
        /// Modified by : Sajal Gupta on 28-11-2016
        /// Desc : Added Check of authorised user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            Form.Action = Request.RawUrl;
            inquiryId = Request["Id"].ToString();

            if (!IsUserLoggedin())
            {
                NotAuthorizedRedirect();
            }
            else
            {
                if (IsAuthorisedUser(Convert.ToInt32(inquiryId)))
                {
                    if (Request["Id"] == null || !CommonOpn.CheckId(Request["Id"]))
                    {
                        btnSave.Enabled = false;
                        return;
                    }

                    FillStatusSell();
                    lblRemoveStatus.Visible = false;
                    isAuthorised = true;
                }
            }
        }

        /// <summary>
        /// Created by : Sajal Gupta on 28-11-2016
        /// Desc       : Check if user is logged in
        /// </summary>
        /// <returns></returns>
        protected bool IsUserLoggedin()
        {
            try
            {
                if (CurrentUser.Id == null || Convert.ToInt32(CurrentUser.Id) < 0)
                    return false;
                else
                    return true;
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, "RemoveFromListing.IsUserLoggedin()");

                return false;
            }
        }

        /// <summary>
        /// Created by : Sajal Gupta on 28-11-2016
        /// Desc       : Check if user is authorised.
        /// </summary>
        /// <returns></returns>
        protected bool IsAuthorisedUser(int inquiryId)
        {
            try
            {
                ISellBikes obj;
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<Bikewale.Interfaces.Customer.ICustomerRepository<CustomerEntity, UInt32>, CustomerRepository<CustomerEntity, UInt32>>();
                    container.RegisterType<ICustomer<CustomerEntity, UInt32>, Customer<CustomerEntity, UInt32>>();
                    container.RegisterType<IMobileVerificationRepository, MobileVerificationRepository>();
                    container.RegisterType<IMobileVerification, MobileVerification>();
                    container.RegisterType<IUsedBikeBuyerRepository, UsedBikeBuyerRepository>();
                    container.RegisterType<ISellBikesRepository<SellBikeAd, int>, SellBikesRepository<SellBikeAd, int>>();
                    container.RegisterType<IUsedBikeSellerRepository, UsedBikeSellerRepository>();
                    container.RegisterType<ISellBikes, SellBikes>();
                    container.RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>();
                    container.RegisterType<IBikeModelsCacheRepository<int>, BikeModelsCacheRepository<BikeModelEntity, int>>();
                    container.RegisterType<IPager, Pager>().RegisterType<ICacheManager, MemcacheManager>(); ;
                    obj = container.Resolve<ISellBikes>();
                    if (obj != null)
                    {
                        SellBikeAd inquiryDetailsObject = obj.GetById(inquiryId, Convert.ToUInt32(CurrentUser.Id));

                        if (inquiryDetailsObject == null || !inquiryDetailsObject.Status.Equals(SellAdStatus.Approved))
                            return false;
                        else
                            return true;
                    }
                    else
                    {
                        return false;
                    }
                }

            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, "RemoveFromListing.IsAuthorisedUser()");

                return false;
            }
        }

        /// <summary>
        /// Created by : Sajal Gupta on 28-11-2016
        /// Desc       : redirect if user not logged in.
        /// </summary>
        /// <returns></returns>
        private void NotAuthorizedRedirect()
        {
            Response.Redirect(String.Format("/users/login.aspx?ReturnUrl=/used/inquiry/{0}/remove/", inquiryId));
            HttpContext.Current.ApplicationInstance.CompleteRequest();
            this.Page.Visible = false;
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

        /// <summary>
        /// Modified by :   Sumit Kate on 04 Dec 2017
        /// Description :   Added a check if Used bike inquiry is added by the user who wants to remove it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            //Check if seller wants to remove its own used bike listing
            if (IsAuthorisedUser(Convert.ToInt32(inquiryId)))
            {
                UpdateClassifiedInquirySoldStatus();
                UpdateSoldStatus();

                _mc.Remove("BW_ModelWiseUsedBikesCount");
                isRemovedListing = "200";
            }
            else
            {
                isRemovedListing = "401";
            }
        }

        protected void UpdateClassifiedInquirySoldStatus()
        {
            string sql = "";

            sql = " insert into classifiedinquirysoldstatus (reason, comments, inquiryid) values (@reason, @comments, @inquiryid) ";

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand(sql))
                {
                    cmd.Parameters.Add(DbFactory.GetDbParam("@reason", DbType.String, 100, drpStatus.SelectedItem.Text));
                    cmd.Parameters.Add(DbFactory.GetDbParam("@comments", DbType.String, 100, txtComments.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("@inquiryid", DbType.Int64, inquiryId));

                    MySqlDatabase.InsertQuery(cmd, ConnectionType.MasterDatabase);
                }
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);
            }

        }   // End of UpdateClassifiedInquirySoldStatus method

        /// <summary>
        /// Modified By : Sadhana Upadhyay on 15 Oct 2014
        /// Summary : set IsApproved 1 to maintain count in bikewale opr
        /// </summary>
        protected void UpdateSoldStatus()
        {
            string sql = " update classifiedindividualsellinquiries set statusid=3, isapproved = 1 where id = @inquiryid ";

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand(sql))
                {
                    cmd.Parameters.Add(DbFactory.GetDbParam("@inquiryid", DbType.Int64, inquiryId));
                    MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase);
                }
            }
            catch (SqlException err)
            {
                Trace.Warn(err.Message);
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);

            } // catch Exception
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);

            } // catch Exception

        }   // End of UpdateSoldStatus method

    }   // End of class
}   // End of namespace