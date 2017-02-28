﻿using Bikewale.BAL.BikeData;
using Bikewale.BAL.Pager;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Pager;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
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
    public class NewBikeLaunch : System.Web.UI.Page
    {
        protected ListPagerControl listPager;
        protected Repeater rptLaunchedBikes;

        protected int totalPages = 0;
        protected int curPageNo = 1;
        protected string prevPageUrl = String.Empty, nextPageUrl = String.Empty;
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
                    container.RegisterType<IPager, Pager>()
                        .RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>()
                        .RegisterType<IBikeModelsCacheRepository<int>, BikeModelsCacheRepository<BikeModelEntity, int>>()
                        .RegisterType<ICacheManager, MemcacheManager>()
                        .RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>()
                        .RegisterType<IPager, Bikewale.BAL.Pager.Pager>();
                    IBikeModelsCacheRepository<int> objModel = container.Resolve<IBikeModelsCacheRepository<int>>();

                    container.RegisterType<IPager, Pager>();
                    IPager objPager = container.Resolve<IPager>();

                    int startIndex = 0, endIndex = 0, recordCount = 0, pageSize = 10;

                    var _objPager = container.Resolve<IPager>();

                    objPager.GetStartEndIndex(pageSize, curPageNo, out startIndex, out endIndex);
                    NewLaunchedBikesBase newLaunchedBikes = objModel.GetNewLaunchedBikesList(startIndex, endIndex);
                    IEnumerable<NewLaunchedBikeEntity> objList = newLaunchedBikes.Models;
                    recordCount = newLaunchedBikes.RecordCount;
                    if (objList != null && objList.Count() > 0)
                    {
                        rptLaunchedBikes.DataSource = objList;
                        rptLaunchedBikes.DataBind();
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
                        pagerEntity.BaseUrl = "/m/new-bike-launches/";
                        pagerEntity.PageNo = curPageNo;
                        pagerEntity.PagerSlotSize = totalPages;
                        pagerEntity.PageUrlType = "page/";
                        pagerEntity.TotalResults = recordCount;
                        pagerEntity.PageSize = pageSize;

                        PagerOutputEntity pagerOutput = objPager.GetPager<PagerOutputEntity>(pagerEntity);
                        //For SEO
                        if (!String.IsNullOrEmpty(pagerOutput.PreviousPageUrl))
                            prevPageUrl = "https://www.bikewale.com" + pagerOutput.PreviousPageUrl;
                        if (!String.IsNullOrEmpty(pagerOutput.NextPageUrl))
                            nextPageUrl = "https://www.bikewale.com" + pagerOutput.NextPageUrl;

                        // for RepeaterPager
                        listPager.PagerOutput = pagerOutput;
                        listPager.CurrentPageNo = curPageNo;
                        listPager.TotalPages = totalPages;

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
    }
}