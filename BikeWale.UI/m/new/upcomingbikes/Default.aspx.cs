using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bikewale.Entities.Pager;
using Bikewale.Interfaces.Pager;
using Bikewale.BAL.Pager;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.BAL.BikeData;
using Bikewale.Common;
using Bikewale.Mobile.Controls;


namespace Bikewale.Mobile.New
{
	public class UpcomingbikesList : System.Web.UI.Page
	{
        protected ListPagerControl listPager, listPager_top;
        protected Repeater rptUpcomingBikes;        

        protected int totalPages = 0;
        protected int curPageNo = 1;
        protected string prevPageUrl = String.Empty , nextPageUrl = String.Empty;

        

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

		protected void Page_Load(object sender, EventArgs e)
		{            
            if (!IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["pn"]))
                    if (!Int32.TryParse(Request.QueryString["pn"], out curPageNo))
                        curPageNo = 1;
        
                GetUpcomingBikeList();
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
                    container.RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>();
                    IBikeModels<BikeModelEntity, int> objModel = container.Resolve<IBikeModels<BikeModelEntity, int>>();

                    container.RegisterType<IPager, Pager>();
                    IPager objPager = container.Resolve<IPager>();

                    UpcomingBikesListInputEntity objInput = new UpcomingBikesListInputEntity();

                    int startIndex = 0, endIndex = 0, recordCount = 0, pageSize = 10;

                    objPager.GetStartEndIndex(pageSize, curPageNo, out startIndex, out endIndex);
                    objInput.StartIndex = startIndex;
                    objInput.EndIndex = endIndex;

                    List<UpcomingBikeEntity> objList = objModel.GetUpcomingBikesList(objInput, EnumUpcomingBikesFilter.Default, out recordCount);

                    if (objList.Count > 0)
                    {
                        rptUpcomingBikes.DataSource = objList;
                        rptUpcomingBikes.DataBind();
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
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }
	}   //End of Class
}   //End of namespace