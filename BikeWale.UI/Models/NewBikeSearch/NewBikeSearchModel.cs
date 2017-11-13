using Bikewale.Entities.BikeData;
using Bikewale.Entities.NewBikeSearch;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.NewBikeSearch;
using Bikewale.Interfaces.Videos;
using Bikewale.Notifications;
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
        private readonly IBikeMakesCacheRepository _makes;
        private readonly ISearchResult _searchResult = null;
        private readonly IProcessFilter _processFilter = null;
        private readonly string _queryString;
        private readonly PQSourceEnum _pqSource;
        public NewBikeSearchModel(string queryString, ICMSCacheContent objArticles, IVideos objVideos, IBikeMakesCacheRepository makes, ISearchResult searchResult, IProcessFilter processFilter, PQSourceEnum pqSource)
        {
            _makes = makes;
            _queryString = queryString;
            _articles = objArticles;
            _videos = objVideos;
            _searchResult = searchResult;
            _processFilter = processFilter;
            _pqSource = pqSource;
        }

        public NewBikeSearchVM GetData()
        {
            NewBikeSearchVM viewModel = new NewBikeSearchVM();
            viewModel.PqSource = _pqSource;
            viewModel.BikeSearch = BindBikes();
            BindEditorialWidget(viewModel);
            BindPageMetas(viewModel.PageMetaTags);
            viewModel.News = new RecentNews(5, 0, _modelIdList, _articles).GetData();
            viewModel.Videos = new RecentVideos(1, 5, _videos).GetData();
            viewModel.ExpertReviews = new RecentExpertReviews(5, _articles).GetData();
            SetFlags(viewModel);
            BindBrands(viewModel);
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
                ErrorClass objErr = new ErrorClass(ex, "NewBikeSearchModel.MapQueryString()");
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
                objPage.CanonicalUrl = "";
                objPage.AlternateUrl = "";

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "NewBikeSearchModel.BindMetas()");
            }
        }
        /// <summary>
        /// Binds the editorial widget.
        /// </summary>
        /// <param name="objVM">The object vm.</param>
        private void BindEditorialWidget(NewBikeSearchVM objVM)
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
                ErrorClass objErr = new ErrorClass(ex, "NewBikeSearchModel.BindEditorialWidget()");
            }
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 9th Oct 2017
        /// Summary : Fetch list of other makes to bind filters
        /// </summary>
        /// <param name="objVM"></param>
        private void BindBrands(NewBikeSearchVM objVM)
        {
            try
            {
                var makes = _makes.GetMakesByType(EnumBikeType.New);
                if (makes != null && makes.Any())
                {
                    objVM.PopularBrands = makes.Take(9);

                    objVM.OtherBrands = makes.Skip(9).OrderBy(m => m.MakeName);
                }
            }
            catch(Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "NewBikeSearchModel.BindBrands");
            }
        }

        private void SetFlags(NewBikeSearchVM Model)
        {
            Model.TabCount = 0;
            Model.IsNewsActive = false;
            Model.IsExpertReviewActive = false;
            Model.IsVideoActive = false;

            if (Model.News.FetchedCount > 0)
            {
                Model.TabCount++;
                Model.IsNewsActive = true;
            }
            if (Model.ExpertReviews.FetchedCount > 0)
            {
                Model.TabCount++;
                if (!Model.IsNewsActive)
                {
                    Model.IsExpertReviewActive = true;
                }
            }
            if (Model.Videos.FetchedCount > 0)
            {
                Model.TabCount++;
                if (!Model.IsExpertReviewActive && !Model.IsNewsActive)
                {
                    Model.IsVideoActive = true;
                }
            }
        }

    }
}