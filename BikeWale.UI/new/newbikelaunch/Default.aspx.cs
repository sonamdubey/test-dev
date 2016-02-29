using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bikewale.Controls;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.BAL.BikeData;
using Bikewale.Entities.Pager;
using Bikewale.Interfaces.Pager;
using Bikewale.BAL.Pager;
using Bikewale.Common;

namespace Bikewale.New
{
	public class NewLaunchBikes : System.Web.UI.Page
	{
        protected LinkPagerControl repeaterPager;
        protected Repeater rptLaunched;

        protected int totalPages = 0;
        protected int curPageNo = 1;
        protected string prevPage = String.Empty, nextPage = String.Empty;
        protected int year = DateTime.Today.Year;


        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //code for device detection added by Sadhana Upadhyay on 6 June 2014
            // Modified By :Ashish Kamble on 5 Feb 2016
            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];

            DeviceDetection dd = new DeviceDetection(originalUrl);
            dd.DetectDevice();

            if (!String.IsNullOrEmpty(Request.QueryString["pn"]))
            {
                if (!Int32.TryParse(Request.QueryString["pn"], out curPageNo))
                    curPageNo = 1;
                else
                    curPageNo = Convert.ToInt16(Request.QueryString["pn"]);
            }

            GetLaunchedBikeList();

        }   // End of Page Load

        /// <summary>
        /// Created By : Sadhana Upadhyay on 5 June 2014
        /// Summary : To get all new launched bike list
        /// </summary>
        private void GetLaunchedBikeList()
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>();
                    IBikeModels<BikeModelEntity, int> objModel = container.Resolve<IBikeModels<BikeModelEntity, int>>();

                    container.RegisterType<IPager, Pager>();
                    IPager objPager = container.Resolve<IPager>();

                    int startIndex = 0, endIndex = 0, recordCount = 0, pageSize =10;

                    objPager.GetStartEndIndex(pageSize, curPageNo, out startIndex, out endIndex);

                    List<NewLaunchedBikeEntity> objList = objModel.GetNewLaunchedBikesList(startIndex, endIndex, out recordCount);

                    if (objList.Count > 0)
                    {
                        rptLaunched.DataSource = objList;
                        rptLaunched.DataBind();
                    }


                    totalPages = objPager.GetTotalPages(recordCount, pageSize);

                    //if current page number exceeded the total pages count i.e. the page is not available 
                    if (curPageNo > totalPages)
                    {
                        Response.Redirect("/pagenotfound.aspx",false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        this.Page.Visible = false;
                    }
                    else
                    {
                        PagerEntity pagerEntity = new PagerEntity();
                        pagerEntity.BaseUrl = "/new-bikes-launches/";
                        pagerEntity.PageNo = curPageNo;
                        pagerEntity.PagerSlotSize = 5;
                        pagerEntity.PageUrlType = "page/";
                        pagerEntity.TotalResults = recordCount;
                        pagerEntity.PageSize = pageSize;

                        PagerOutputEntity pagerOutput = objPager.GetPager<PagerOutputEntity>(pagerEntity);

                        //For SEO
                        if(!String.IsNullOrEmpty(pagerOutput.PreviousPageUrl))
                            prevPage = "http://www.bikewale.com" + pagerOutput.PreviousPageUrl;
                        if (String.IsNullOrEmpty(pagerOutput.PreviousPageUrl))
                            nextPage = "http://www.bikewale.com" + pagerOutput.NextPageUrl;

                        // for RepeaterPager
                        repeaterPager.PagerOutput = pagerOutput;
                        repeaterPager.CurrentPageNo = curPageNo;
                        repeaterPager.TotalPages = totalPages;
                        repeaterPager.BindPagerList();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }
	}
}