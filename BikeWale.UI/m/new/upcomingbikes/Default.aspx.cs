using Bikewale.BAL.BikeData;
using Bikewale.BAL.Pager;
using Bikewale.Common;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Pager;
using Bikewale.Entities.SEO;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Pager;
using Bikewale.Mobile.Controls;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;


namespace Bikewale.Mobile.New
{

    /// <summary>
    /// Modified By : Sangram Nandkhile on 23 Nov 2016
    /// Summary : To show makewise bikes on upcoming page
    /// </summary>
    public class UpcomingbikesList : System.Web.UI.Page
    {
        protected ListPagerControl listPager, listPager_top;
        protected Repeater rptUpcomingBikes;
        protected IEnumerable<UpcomingBikeEntity> objList = null;
        protected PageMetaTags meta;
        protected int totalPages = 0, curPageNo = 1, currentYear = DateTime.Now.Year, makeId;
        protected string prevPageUrl = string.Empty, nextPageUrl = string.Empty, makeMaskingName = string.Empty, makeName = string.Empty;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ProcessQueryString();
                GetUpcomingBikeList();
                CreateMetaTags();
            }
        }

        private void CreateMetaTags()
        {
            meta = new PageMetaTags();
            if (!string.IsNullOrEmpty(makeMaskingName) && makeId > 0)
            {
                meta.Title = string.Format("Upcoming Bikes in India - Expected Launches in {0}", currentYear);
                meta.Keywords = string.Format("Find out upcoming new bikes in {0} in India. From small to super-luxury, from announced to highly speculated models, from near future to end of year, know about every upcoming bike launch in India this year.", currentYear);
                meta.Description = string.Format("Upcoming bikes, new upcoming launches, upcoming bike launches, upcoming models, future bikes, future bike launches, {0} bikes, speculated launches, futuristic models", currentYear);
                meta.CanonicalUrl = string.Format("http://www.bikewale.com/{0}-bikes/upcoming/", makeMaskingName);
                meta.PreviousPageUrl = string.IsNullOrEmpty(prevPageUrl) ? "" : "http://www.bikewale.com" + prevPageUrl;
                meta.NextPageUrl = string.IsNullOrEmpty(nextPageUrl) ? "" : "http://www.bikewale.com" + nextPageUrl;

            }
            else
            {
                meta.Title = string.Format("Upcoming Bikes in India - Expected Launches in {0}", currentYear);
                meta.Keywords = string.Format("Find out upcoming new bikes in {0} in India. From small to super-luxury, from announced to highly speculated models, from near future to end of year, know about every upcoming bike launch in India this year.", currentYear);
                meta.Description = string.Format("Upcoming bikes, new upcoming launches, upcoming bike launches, upcoming models, future bikes, future bike launches, {0} bikes, speculated launches, futuristic models", currentYear);
                meta.CanonicalUrl = "http://www.bikewale.com/upcoming-bikes/";
                meta.PreviousPageUrl = string.IsNullOrEmpty(prevPageUrl) ? "" : "http://www.bikewale.com" + prevPageUrl;
                meta.NextPageUrl = string.IsNullOrEmpty(nextPageUrl) ? "" : "http://www.bikewale.com" + nextPageUrl;
            }
        }

        /// <summary>
        /// Written By : Sangram Nandkhile on 23 Nov 2016
        /// Summary : To process query string objects
        /// </summary>
        /// 
        public void ProcessQueryString()
        {
            string pageNo = Request.QueryString["pn"];

            if (!String.IsNullOrEmpty(pageNo))
            {
                if (!Int32.TryParse(pageNo, out curPageNo))
                    curPageNo = 1;
            }
            makeMaskingName = Request.QueryString["make"];
        }
        /// <summary>
        /// Written By : Ashwini Todkar on 15 May 2014
        /// Modified By : Sadhana Upadhyay on 21th May 2014
        /// Summary : To bind Pager Control
        /// </summary>
        private void GetUpcomingBikeList()
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>()
                             .RegisterType<IPager, Pager>();

                    IBikeModels<BikeModelEntity, int> objModel = container.Resolve<IBikeModels<BikeModelEntity, int>>();
                    IPager objPager = container.Resolve<IPager>();
                    UpcomingBikesListInputEntity objInput = new UpcomingBikesListInputEntity();

                    int startIndex = 0, endIndex = 0, recordCount = 0, pageSize = 10;
                    objPager.GetStartEndIndex(pageSize, curPageNo, out startIndex, out endIndex);
                    objInput.StartIndex = startIndex;
                    objInput.EndIndex = endIndex;

                    objList = objModel.GetUpcomingBikesList(objInput, EnumUpcomingBikesFilter.Default, out recordCount);

                    recordCount = objList.Count();
                    if (recordCount > 0)
                    {
                        var firstModel = objList.FirstOrDefault();
                        if (firstModel != null)
                        {
                            makeId = firstModel.MakeBase.MakeId;
                            makeName = firstModel.MakeBase.MakeName;
                        }
                    }

                    totalPages = objPager.GetTotalPages(recordCount, pageSize);
                    //if current page number exceeded the total pages count i.e. the page is not available 
                    if (curPageNo > totalPages)
                    {
                        Response.Redirect("/m/pagenotfound.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        this.Page.Visible = false;
                    }
                    else
                    {
                        PagerEntity pagerEntity = new PagerEntity();
                        pagerEntity.BaseUrl = "/m/upcoming-bikes/";
                        pagerEntity.PageNo = curPageNo;
                        pagerEntity.PagerSlotSize = totalPages;
                        pagerEntity.PageUrlType = "page/";
                        pagerEntity.TotalResults = recordCount;
                        pagerEntity.PageSize = pageSize;

                        PagerOutputEntity pagerOutput = objPager.GetPager<PagerOutputEntity>(pagerEntity);
                        //For SEO
                        prevPageUrl = pagerOutput.PreviousPageUrl;
                        nextPageUrl = pagerOutput.NextPageUrl;

                        listPager_top.PagerOutput = pagerOutput;
                        listPager_top.TotalPages = totalPages;
                        listPager_top.CurrentPageNo = curPageNo;
                        listPager_top.BindPageNumbers();

                        listPager.PagerOutput = pagerOutput;
                        listPager.TotalPages = totalPages;
                        listPager.CurrentPageNo = curPageNo;
                        listPager.BindPageNumbers();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "UpcomingbikesList.GetUpcomingBikeList()");
                objErr.SendMail();
            }
        }
    }   //End of Class
}   //End of namespace