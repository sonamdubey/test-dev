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
        protected int curPageNo = 1, currentYear = DateTime.Now.Year;
        protected uint makeId;
        protected string pageTitle = string.Empty, prevPageUrl = string.Empty, nextPageUrl = string.Empty, makeMaskingName = string.Empty, makeName = string.Empty;
        protected bool isMake = false;

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

        /// <summary>
        /// Written By : Sangram Nandkhile on 23 Nov 2016
        /// Summary : To create meta and title
        /// </summary>
        private void CreateMetaTags()
        {
            meta = new PageMetaTags();
            if (isMake)
            {
                meta.Title = string.Format("Upcoming {1} Bikes in India - Expected {1} Bike New Launches in {0}", currentYear, makeName);
                meta.Description = string.Format("Check out upcoming {1} bikes in {0} in India. From small to super-luxury, from announced to highly speculated models, from near future to end of year, know about every upcoming bike launch in India this year.", currentYear, makeName);
                meta.Keywords = string.Format("Upcoming {1} bikes, new upcoming {1} launches, upcoming {1} bike launches, upcoming {1} models, future {1} bikes, future {1} bike launches, {0} {1} bikes, speculated {1} launches, futuristic {1} models", currentYear, makeName);
                meta.CanonicalUrl = string.Format("https://www.bikewale.com/{0}-bikes/upcoming/", makeMaskingName);
                meta.PreviousPageUrl = string.IsNullOrEmpty(prevPageUrl) ? string.Empty : "https://www.bikewale.com" + prevPageUrl;
                meta.NextPageUrl = string.IsNullOrEmpty(nextPageUrl) ? string.Empty : "https://www.bikewale.com" + nextPageUrl;
                pageTitle = string.Format("Upcoming {0} Bikes in India", makeName);
            }
            else
            {
                meta.Title = string.Format("Upcoming Bikes in India - Expected Launches in {0}", currentYear);
                meta.Description = string.Format("Find out upcoming new bikes in {0} in India. From small to super-luxury, from announced to highly speculated models, from near future to end of year, know about every upcoming bike launch in India this year.", currentYear);
                meta.Keywords = string.Format("Upcoming bikes, new upcoming launches, upcoming bike launches, upcoming models, future bikes, future bike launches, {0} bikes, speculated launches, futuristic models", currentYear);
                meta.CanonicalUrl = "https://www.bikewale.com/upcoming-bikes/";
                meta.PreviousPageUrl = string.IsNullOrEmpty(prevPageUrl) ? string.Empty : "https://www.bikewale.com" + prevPageUrl;
                meta.NextPageUrl = string.IsNullOrEmpty(nextPageUrl) ? string.Empty : "https://www.bikewale.com" + nextPageUrl;
                pageTitle = "Upcoming Bikes in India";
            }
        }

        /// <summary>
        /// Written By : Sangram Nandkhile on 23 Nov 2016
        /// Summary : To process query string objects
        /// </summary>

        public void ProcessQueryString()
        {
            string pageNo = Request.QueryString["pn"];
            if (!String.IsNullOrEmpty(pageNo))
            {
                if (!Int32.TryParse(pageNo, out curPageNo))
                    curPageNo = 1;
            }
            makeMaskingName = Request.QueryString["make"];
            if (!string.IsNullOrEmpty(makeMaskingName))
            {
                MakeMaskingResponse objMake = new MakeHelper().GetMakeByMaskingName(makeMaskingName);
                makeId = HandleMakeRedirection(objMake);
                isMake = objMake != null && objMake.StatusCode == 200 ? true : false;
            }
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

                    int startIndex = 0, endIndex = 0, totalCount = 0, pageSize = 10;
                    objPager.GetStartEndIndex(pageSize, curPageNo, out startIndex, out endIndex);
                    objInput.PageNo = startIndex;
                    objInput.PageSize = endIndex;
                    if (isMake)
                        objInput.MakeId = (int)makeId;
                    objList = objModel.GetUpcomingBikesList(objInput, EnumUpcomingBikesFilter.Default, out totalCount);
                    int recordCount = objList.Count();
                    if (recordCount > 0 && isMake)
                    {
                        var firstModel = objList.FirstOrDefault();
                        makeName = firstModel != null ? firstModel.MakeBase.MakeName : string.Empty;
                    }

                    int totalPages = objPager.GetTotalPages(totalCount, pageSize);
                    //if current page number exceeded the total pages count i.e. the page is not available
                    if (curPageNo > totalPages)
                    {
                        UrlRewrite.Return404();

                    }
                    else
                    {
                        PagerEntity pagerEntity = new PagerEntity();
                        pagerEntity.BaseUrl = !isMake ? "/m/upcoming-bikes/" : string.Format("/m/{0}-bikes/upcoming/", makeMaskingName);
                        pagerEntity.PageNo = curPageNo;
                        pagerEntity.PagerSlotSize = totalPages;
                        pagerEntity.PageUrlType = "page/";
                        pagerEntity.TotalResults = totalCount;
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
                ErrorClass.LogError(ex, "UpcomingbikesList.GetUpcomingBikeList()");
                
            }
        }

        /// <summary>
        /// Created by: Sangram Nandkhile on 24 Nov 2016
        /// Desc: Common code to handle make masking name redirections
        /// </summary>
        /// <param name="objMakeResponse"></param>
        private uint HandleMakeRedirection(MakeMaskingResponse objMakeResponse)
        {
            uint makeID = 0;
            if (objMakeResponse != null)
            {
                if (objMakeResponse.StatusCode == 200)
                {
                    makeID = objMakeResponse.MakeId;
                }
                else if (objMakeResponse.StatusCode == 301)
                {
                    CommonOpn.RedirectPermanent(Request.RawUrl.Replace(makeMaskingName, objMakeResponse.MaskingName));
                }
                else
                {
                    UrlRewrite.Return404();

                }
            }
            else
            {
                UrlRewrite.Return404();

            }
            return makeID;
        }
    }   //End of Class
}   //End of namespace