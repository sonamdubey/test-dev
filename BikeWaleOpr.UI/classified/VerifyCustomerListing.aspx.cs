using BikeWaleOpr.BAL.Pager;
using BikeWaleOpr.Common;
using BikeWaleOpr.Controls;
using BikeWaleOpr.Entities.Pager;
using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;

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

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
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
        /// Modified By : Sushil Kumar on 11th Nov 2016
        /// Description : Fixed compare bikes issue on listing page of verified listing
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
                    startIndex = 1;
                    endIndex = 20;
                    inquiryId = txtProfileId.Text;
                }

                ds = cc.CustomerListingDetail(startIndex, endIndex, inquiryId);

                if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                {
                    recordCount = Convert.ToInt32(ds.Tables[1].Rows[0]["recordCount"]);

                    rptCustomerList.DataSource = ds.Tables[0];
                    rptCustomerList.DataBind(); // Data Bind

                    totalPages = objPager.GetTotalPages(recordCount, pageSize);

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

                    if (ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
                    {
                        lblErrorMessage.Visible = false;
                    }
                    else
                    {
                        lblErrorMessage.Visible = true;
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + "BindRepeater");
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
                if (currentPageNo == 0)
                    currentPageNo = 1;
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + "ProcessQS");
                objErr.SendMail();
            }
        }
    }   //End of Class
}   //End of namespace