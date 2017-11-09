using Bikewale.Entities.NewBikeSearch;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.NewBikeSearch;
using Bikewale.Interfaces.Videos;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Web;

namespace Bikewale.Models.NewBikeSearch
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 08-Nov-2017
    /// Summary: Model for New BIke Search
    /// 
    /// </summary>
    public class NewBikeSearchModel
    {
        private string _modelIdList = string.Empty;
        public uint EditorialTopCount { get; set; }
        private readonly ICMSCacheContent _articles = null;
        private readonly IVideos _videos = null;
        private readonly ISearchResult _searchResult = null;
        private readonly IProcessFilter _processFilter = null;
        private string _queryString;
        private HttpRequestBase _request;
        public NewBikeSearchModel(HttpRequestBase Request, ICMSCacheContent objArticles, IVideos objVideos, ISearchResult searchResult, IProcessFilter processFilter)
        {
            _request = Request;
            _queryString = Request.Url.Query;
            _articles = objArticles;
            _videos = objVideos;
            _searchResult = searchResult;
            _processFilter = processFilter;
        }

        public NewBikeSearchVM GetData()
        {
            NewBikeSearchVM viewModel = new NewBikeSearchVM();
            viewModel.Bikes = BindBikes();
            BindPageMetas(viewModel.PageMetaTags);
            viewModel.News = new RecentNews(5, 0, _modelIdList, _articles).GetData();
            return viewModel;
        }

        private SearchOutputEntity BindBikes()
        {
            SearchOutputEntity objResult = null;
            InputBaseEntity input = MapQueryString(_queryString);
            if (null != input)
            {
                FilterInput filterInputs = _processFilter.ProcessFilters(input);
                objResult = _searchResult.GetSearchResult(filterInputs, input);
            }
            return objResult;
        }

        /// <summary>
        /// Maps the query string into an object
        /// </summary>
        /// <param name="qs">The qs.</param>
        /// <returns></returns>
        private InputBaseEntity MapQueryString(string qs)
        {
            InputBaseEntity input = null;
            try
            {
                var dict = HttpUtility.ParseQueryString(qs);
                string jsonStr = JsonConvert.SerializeObject(
                    dict.AllKeys.ToDictionary(k => k, k => dict[k])
                );
                input =  JsonConvert.DeserializeObject<InputBaseEntity>(jsonStr);
            }
            catch (Exception ex)
            {
                new Notifications.ErrorClass(ex, "NewBikeSearchModel.MapQueryString()");
            }
            return input;
        }

        /// <summary>
        /// Created by : Sangram Nandkhile on 09-Nov-2017 
        /// Binds the page metas.
        /// </summary>
        /// <param name="objPage">The object page.</param>
        private void BindPageMetas(PageMetaTags objPage)
        {
            try
            {
                objPage.Title = "";
                objPage.Keywords = "";
                objPage.Description = "";
                objPage.CanonicalUrl = string.Format("/{0}", _request.Url.AbsoluteUri);
                objPage.AlternateUrl = "";

            }
            catch (Exception ex)
            {
                new Notifications.ErrorClass(ex, "NewBikeSearchModel.BindMetas()");
            }
        }
        /// <summary>
        /// Binds the editorial widget.
        /// </summary>
        /// <param name="objVM">The object vm.</param>
        private void BindEditorialWidget(ScootersIndexPageVM objVM)
        {
            try
            {
                RecentNews objNews = new RecentNews(EditorialTopCount, _articles);
                objNews.IsScooter = true;
                objVM.News = objNews.GetData();

                RecentExpertReviews objReviews = new RecentExpertReviews(EditorialTopCount, _articles);
                objReviews.IsScooter = true;
                objVM.ExpertReviews = objReviews.GetData();

                RecentVideos objVideos = new RecentVideos(1, (ushort)EditorialTopCount, _videos);
                objVideos.IsScooter = true;
                objVM.Videos = objVideos.GetData();

                objVM.TabCount = 0;
                objVM.IsNewsActive = false;
                objVM.IsExpertReviewActive = false;
                objVM.IsVideoActive = false;

                if (objVM.News != null && objVM.News.FetchedCount > 0)
                {
                    objVM.TabCount++;
                    objVM.IsNewsActive = true;
                }
                if (objVM.ExpertReviews.FetchedCount > 0)
                {
                    objVM.TabCount++;
                    if (!objVM.IsNewsActive)
                    {
                        objVM.IsExpertReviewActive = true;
                    }
                }
                if (objVM.Videos.FetchedCount > 0)
                {
                    objVM.TabCount++;
                    if (!objVM.IsExpertReviewActive && !objVM.IsNewsActive)
                    {
                        objVM.IsVideoActive = true;
                    }
                }
            }
            catch (Exception ex)
            {
                new Notifications.ErrorClass(ex, "NewBikeSearchModel.BindEditorialWidget()");
            }
        }

    }
}