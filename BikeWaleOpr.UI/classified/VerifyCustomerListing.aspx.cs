using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using BikeWaleOpr.Common;
using BikeWaleOpr.Interfaces.Pager;
using BikeWaleOpr.BAL.Pager;
using BikeWaleOpr.Entities.Pager;
using BikeWaleOpr.Controls;

namespace BikeWaleOpr.Classified
{
    public class VerifyCustomerListing : System.Web.UI.Page
    {
        //asp control variable
        protected Repeater rptCustomerList;
        protected TextBox txtProfileId;
        protected Label lblErrorMessage;
        //custom control variable 
        protected LinkPagerControl linkPager;

        protected int recordCount = 0, currentPageNo = 0;

        protected override void OnInit( EventArgs e )
		{
			InitializeComponent();
		}
		
		void InitializeComponent()
		{
			base.Load += new EventHandler( Page_Load );
		}

        void Page_Load(object Sender, EventArgs e)
        {
            lblErrorMessage.Visible = false;
            ProcessQS();
            BindRepeater();
        }

        /// <summary>
        /// Created By : Sanjay Soni ON 30/9/2014
        /// Description : To Bind All Listings
        /// </summary>
        protected void BindRepeater()
        {
            DataSet ds = null;
            string inquiryId = String.Empty;
            try
            {
                    Pager objPager = new Pager();

                    int startIndex = 0, endIndex = 0, pageSize = 20, totalPages = 0;

                    objPager.GetStartEndIndex(pageSize, currentPageNo, out startIndex, out endIndex);


                    ClassifiedCommon cc = new ClassifiedCommon();
                    if (IsPostBack)                    
                    {
                        startIndex = 0;
                        endIndex = 20;
                        inquiryId = txtProfileId.Text;
                    }

                    ds = cc.CustomerListingDetail(startIndex, endIndex, inquiryId);

                    recordCount = Convert.ToInt32(ds.Tables[0].Rows[0]["recordCount"]);
                    Trace.Warn("Record Count", recordCount.ToString());

                    totalPages = objPager.GetTotalPages(recordCount, pageSize);
                    if (ds.Tables[1].Rows != null && ds.Tables[1].Rows.Count > 0)
                    {
                        lblErrorMessage.Visible = false;
                    }
                    else{
                        lblErrorMessage.Visible = true;                        
                    }
                    rptCustomerList.DataSource = ds.Tables[1];
                    rptCustomerList.DataBind(); // Data Bind

                    PagerEntity pagerEntity = new PagerEntity();
                    pagerEntity.BaseUrl = "/classified/verifycustomerlisting.aspx?";
                    pagerEntity.PageNo = currentPageNo;
                    pagerEntity.PagerSlotSize = 30;
                    pagerEntity.PageUrlType = "pn=";
                    pagerEntity.TotalResults = recordCount;
                    pagerEntity.PageSize = pageSize;

                    PagerOutputEntity pagerOutput = objPager.GetPager<PagerOutputEntity>(pagerEntity);

                    // for RepeaterPager
                    linkPager.PagerOutput = pagerOutput;
                    linkPager.CurrentPageNo = currentPageNo;
                    linkPager.TotalPages = totalPages;
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("BindRepeater ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }   //End of BindRepeater

        /// <summary>
        /// To get page number from query string
        /// 
        /// </summary>
        protected void ProcessQS()
        {
            try
            {
                currentPageNo = Convert.ToInt32(Request.QueryString["pn"]);
                Trace.Warn(currentPageNo.ToString());
                if (currentPageNo == 0)
                    currentPageNo = 1;
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("ProcessQS ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }
    }   //End of Class
}   //End of namespace