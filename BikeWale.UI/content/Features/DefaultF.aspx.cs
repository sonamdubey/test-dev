﻿using Bikewale.BAL.EditCMS;
using Bikewale.BAL.Pager;
using Bikewale.Cache.CMS;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.Controls;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.Pager;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.EditCMS;
using Bikewale.Interfaces.Pager;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Bikewale.Content
{
    /// <summary>
    /// Modified By : Ashwini Todkar on 25 Sept 2014
    /// Retrieved features from carwale web api
    /// </summary>
    public class DefaultF : System.Web.UI.Page
    {
        protected Repeater rptFeatures;
        protected Bikewale.Mobile.Controls.LinkPagerControl ctrlPager;
        protected string prevUrl = string.Empty, nextUrl = string.Empty;
        protected UpcomingBikesCMS ctrlUpcomingBikes;
        private int _pageNo = 1;
        private const int _pageSize = 10, _pagerSlotSize = 5;
        private bool _isContentFound = true;  
        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);
        }

        private void Page_Load(object sender, EventArgs e)
        {
            // Modified By :Lucky Rathore on 12 July 2016.
            Form.Action = Request.RawUrl;
            //code for device detection added by Ashwini Todkar
            // Modified By :Ashish Kamble on 5 Feb 2016
            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];

            DeviceDetection dd = new DeviceDetection(originalUrl);
            dd.DetectDevice();


            CommonOpn op = new CommonOpn();

            if (Request["pn"] != null && Request.QueryString["pn"] != "")
            {
                if (CommonOpn.CheckId(Request.QueryString["pn"]) == true)
                    _pageNo = Convert.ToInt32(Request.QueryString["pn"]);
            }

            GetFeaturesList();
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 25 Sept 2014
        /// Summary    : method to fetch features list and total features count from carwale api
        /// Modified By: Aditi Srivastava on 8 Nov 2016
        /// Summay     : Added function to bind upcoming bikes widget 
        /// </summary>      
        private async void GetFeaturesList()
        {
            try
            {
                // get pager instance
                IPager objPager = GetPager();

                int _startIndex = 0, _endIndex = 0;// _featuresCategoryId = (int)EnumCMSContentType.Features;

                List<EnumCMSContentType> categorList = new List<EnumCMSContentType>();
                categorList.Add(EnumCMSContentType.Features);
                categorList.Add(EnumCMSContentType.SpecialFeature);
                string _featuresCategoryId = CommonApiOpn.GetContentTypesString(categorList);

                objPager.GetStartEndIndex(_pageSize, _pageNo, out _startIndex, out _endIndex);
                CMSContent _objFeaturesList = null;

                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IArticles, Articles>()
                            .RegisterType<ICMSCacheContent, CMSCacheRepository>()
                            .RegisterType<ICacheManager, MemcacheManager>();
                    ICMSCacheContent _cache = container.Resolve<ICMSCacheContent>();

                    _objFeaturesList = _cache.GetArticlesByCategoryList(_featuresCategoryId, _startIndex, _endIndex,0,0);                

                    if (_objFeaturesList != null && _objFeaturesList.Articles.Count > 0)
                    {

                        BindFeatures(_objFeaturesList);
                        BindLinkPager(objPager, Convert.ToInt32(_objFeaturesList.RecordCount));
                        
                        BindUpcoming();

                    }
                    else
                    {
                        _isContentFound = false;
                    }
                }


            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                if (!_isContentFound)
                    Response.Redirect("/pagenotfound.aspx", false);
            }
        }

        //PopulateWhere to create Pager instance
        private IPager GetPager()
        {
            IPager _objPager = null;
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IPager, Pager>();
                _objPager = container.Resolve<IPager>();
            }
            return _objPager;
        }


        /// <summary>
        /// Written By : Ashwini Todkar on 24 Sept 2014
        /// PopulateWhere to bind link pager control 
        /// </summary>
        /// <param name="objPager"> Pager instance </param>
        /// <param name="recordCount"> total news available</param>
        private void BindLinkPager(IPager objPager, int recordCount)
        {
            PagerOutputEntity _pagerOutput = null;
            PagerEntity _pagerEntity = null;

            try
            {
                _pagerEntity = new PagerEntity();
                _pagerEntity.BaseUrl = "/features/";
                _pagerEntity.PageNo = _pageNo; //Current page number
                _pagerEntity.PagerSlotSize = _pagerSlotSize; // 5 links on a page
                _pagerEntity.PageUrlType = "page/";
                _pagerEntity.TotalResults = recordCount; //total News count
                _pagerEntity.PageSize = _pageSize;        //No. of news to be displayed on a page

                _pagerOutput = objPager.GetPager<PagerOutputEntity>(_pagerEntity);

                // for RepeaterPager
                ctrlPager.PagerOutput = _pagerOutput;
                ctrlPager.CurrentPageNo = _pageNo;
                ctrlPager.TotalPages = objPager.GetTotalPages(recordCount, _pageSize);
                ctrlPager.BindPagerList();

                //For SEO
                //CreatePrevNextUrl(linkPager.TotalPages);
                prevUrl = String.IsNullOrEmpty(_pagerOutput.PreviousPageUrl) ? "" : "http://www.bikewale.com" + _pagerOutput.PreviousPageUrl;
                nextUrl = String.IsNullOrEmpty(_pagerOutput.NextPageUrl) ? "" : "http://www.bikewale.com" + _pagerOutput.NextPageUrl;
            }
            catch (Exception ex)
            {
                Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        private void BindFeatures(CMSContent _objFeaturesList)
        {
            rptFeatures.DataSource = _objFeaturesList.Articles;
            rptFeatures.DataBind();
        }
        /// <summary>
        /// Created by : Aditi Srivastava on 8 Nov 2016
        /// Summary  : Bind upcoming bikes list
        /// </summary>
        private void BindUpcoming()
        {
            var cookies = this.Context.Request.Cookies;
            string city = cookies["location"].Value.Substring(cookies["location"].Value.IndexOf('_') + 1);
            if (String.IsNullOrEmpty(city))
                city = BWConfiguration.Instance.DefaultName;
            ctrlUpcomingBikes.sortBy = (int)EnumUpcomingBikesFilter.Default;
            ctrlUpcomingBikes.pageSize = 4;
            ctrlUpcomingBikes.cityName = city;
           // ctrlUpcomingBikes.MakeId = 1;
        }
    }
}