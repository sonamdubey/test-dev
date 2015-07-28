using BikeWaleOpr.Classified;
using BikeWaleOpr.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// Created By Sanjay Soni ON 1/10/2014
/// </summary>
namespace BikeWaleOpr.Classified
{
    public class TotalListings : System.Web.UI.Page
    {
        protected Repeater rptCustomerLiveList, rptCustomerPendingList, rptCustomerFakeList, rptCustomerUnVerifiedList, rptCustomerSoldList;
        protected int customerId = 0;
        protected DataSet ds = null;
        protected int listType = 0;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        protected void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            BindRepeater();
        }

        protected void BindRepeater()
        {
            listType = Convert.ToInt16(Request.QueryString["listtype"]);
            ClassifiedCommon cc = new ClassifiedCommon();
            ProcessQS();
            //Trace.Warn("list type :" + listType);
            switch (listType)
            {
                case 1:
                        ds = cc.CustomerTotalListings(customerId);
                        if (ds.Tables[0] != null)
                        {
                            rptCustomerLiveList.DataSource = ds.Tables[0];
                            rptCustomerLiveList.DataBind();
                        }
                        if (ds.Tables[1] != null)
                        {
                            rptCustomerPendingList.DataSource = ds.Tables[1];
                            rptCustomerPendingList.DataBind();
                        }
                        if (ds.Tables[2] != null)
                        {
                            rptCustomerFakeList.DataSource = ds.Tables[2];
                            rptCustomerFakeList.DataBind();
                        }
                        if (ds.Tables[3] != null)
                        {
                            rptCustomerUnVerifiedList.DataSource = ds.Tables[3];
                            rptCustomerUnVerifiedList.DataBind();
                        }
                        if (ds.Tables[4] != null)
                        {
                            rptCustomerSoldList.DataSource = ds.Tables[4];
                            rptCustomerSoldList.DataBind();
                        }
                    break;
                case 2:
                        ds = cc.CustomerLiveListings(customerId);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            rptCustomerLiveList.DataSource = ds.Tables[0];
                            rptCustomerLiveList.DataBind();
                        }
                    break;
                case 3:
                    ds = cc.CustomerPendingListings(customerId);
                    //Trace.Warn("count :" + ds.Tables[0].Rows.Count);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        rptCustomerPendingList.DataSource = ds.Tables[0];
                        rptCustomerPendingList.DataBind();
                    }
                    break;
                case 4:
                    ds = cc.CustomerFakeListings(customerId);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        rptCustomerFakeList.DataSource = ds.Tables[0];
                        rptCustomerFakeList.DataBind();
                    }
                    break;
                case 5:
                    ds = cc.CustomerUnVerifiedListings(customerId);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        rptCustomerUnVerifiedList.DataSource = ds.Tables[0];
                        rptCustomerUnVerifiedList.DataBind();
                    }
                    break;
                case 6:
                    ds = cc.CustomerSoldListings(customerId);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        rptCustomerSoldList.DataSource = ds.Tables[0];
                        rptCustomerSoldList.DataBind();
                    }
                    break;
            }
        } // End of BindRepeater

        protected void ProcessQS()
        {
            try
            {
                customerId = Convert.ToInt32(Request.QueryString["custid"]);
                Trace.Warn("Customer id", customerId.ToString());
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("BindRepeater ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        } // End of ProcessQS

        /// <summary>
        /// Created By : Sanjay Soni ON 30/9/2014
        /// Description : To Approve listing
        /// </summary>
        /// <param name="custid"></param>
        protected void ApproveListing(int custid)
        {
            ClassifiedCommon cc = new ClassifiedCommon();
            cc.ApproveListing(custid);
        }

        /// <summary>
        /// Created By : Sanjay Soni ON 30/9/2014
        /// Description : To Discard listing
        /// </summary>
        /// <param name="custid"></param>
        protected void DiscardListing(int custid)
        {
            ClassifiedCommon cc = new ClassifiedCommon();
            cc.DiscardListing(custid);
        }
    } // END CLASS
} // END NAMESPACE