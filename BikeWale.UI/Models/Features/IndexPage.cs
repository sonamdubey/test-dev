
using Bikewale.Entities;
using Bikewale.Entities.CMS;
using Bikewale.Entities.Pager;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Pager;
using Bikewale.Notifications;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace Bikewale.Models.Features
{

    /// <summary>
    /// Created By :- Subodh Jain 31 March 2017
    /// Summary :- Model For Index Page
    /// </summary>
    public class IndexPage
    {
        private readonly ICMSCacheContent _Cache = null;
        private readonly IPager _objPager = null;

        public uint curPageNo = 1;
        private const int _pageSize = 10, _pagerSlotSize = 5;
        public StatusCodes status;
        public IndexPage(ICMSCacheContent Cache, IPager objPager)
        {
            _Cache = Cache;
            _objPager = objPager;
        }

        public IndexFeatureVM GetData()
        {
            IndexFeatureVM objIndex = new IndexFeatureVM();
            GetFeaturesList(objIndex);
            return objIndex;
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 30 Sept 2014
        /// Summary    : PopulateWhere to get features list from web api asynchronously
        /// </summary>
        public void GetFeaturesList(IndexFeatureVM objIndex)
        {
            try
            {
                int _startIndex = 0, _endIndex = 0;

                _objPager.GetStartEndIndex(_pageSize, (int)curPageNo, out _startIndex, out _endIndex);

                IList<EnumCMSContentType> categorList = new List<EnumCMSContentType>();
                categorList.Add(EnumCMSContentType.Features);
                categorList.Add(EnumCMSContentType.SpecialFeature);
                string _featuresCategoryId = CommonApiOpn.GetContentTypesString(categorList);

                var _objFeaturesList = _Cache.GetArticlesByCategoryList(_featuresCategoryId, _startIndex, _endIndex, 0, 0);

                if (_objFeaturesList != null && _objFeaturesList.Articles.Count() > 0)
                {

                    objIndex.ArticlesList = _objFeaturesList.Articles;
                    objIndex.TotalArticles = _objFeaturesList.RecordCount;
                    status = StatusCodes.ContentFound;

                }
                else
                {
                    status = StatusCodes.ContentNotFound;
                }

            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "Bikewale.GetFeaturesList");
            }
        }

        /// <summary>
        /// Created By : Subodh Jain 31 March 2017
        /// Summary    : Bind link pager
        /// </summary>
        private void BindLinkPager(IndexFeatureVM objIndex)
        {
            try
            {
                string _baseUrl = RemoveTrailingPage(HttpContext.Current.Request.RawUrl.ToLower());
                objIndex.PagerEntity = new PagerEntity();

                objIndex.PagerEntity.PageNo = (int)curPageNo;
                objIndex.PagerEntity.PagerSlotSize = _pagerSlotSize;
                objIndex.PagerEntity.BaseUrl = _baseUrl;
                objIndex.PagerEntity.PageUrlType = "page/";
                objIndex.PagerEntity.TotalResults = (int)objIndex.TotalArticles;
                objIndex.PagerEntity.PageSize = _pageSize;
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "Exception :Bikewale.Models.Features.IndexPage.BindLinkPager");
            }
        }
        private string RemoveTrailingPage(string rawUrl)
        {
            string retUrl = rawUrl;
            if (rawUrl.Contains("/page/"))
            {
                string[] urlArray = rawUrl.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                retUrl = string.Format("/{0}/", string.Join("/", urlArray.Take(urlArray.Length - 2).ToArray()));
            }
            return retUrl;
        }


    }
}