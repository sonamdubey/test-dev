using Bikewale.Common;
using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Pager;
using System;
using System.Web;

namespace Bikewale.Models.ExpertReviews
{
    /// <summary>
    /// Created by: Vivek Singh Tomar on 17th Aug 2017
    /// Summary: Model for expert reviews for scooters landing page
    /// </summary>
    public class ExpertReviewsScootersPage
    {
        #region Variables for dependency injection
        private readonly ICMSCacheContent _cmsCache = null;
        private readonly IPager _pager;
        private readonly IBikeModelsCacheRepository<int> _models = null;
        private readonly IUpcoming _upcoming = null;
        private readonly IBikeModels<BikeModelEntity, int> _bikeModels = null;
        #endregion

        #region Page level variables
        private const int pageSize = 10, pagerSlotSize = 5;
        private int curPageNo = 1;
        string make = string.Empty, model = string.Empty;
        private uint MakeId, ModelId, CityId, pageCatId = 0;
        public string redirectUrl;
        public StatusCodes status;
        private MakeHelper makeHelper = null;
        private ModelHelper modelHelper = null;
        private GlobalCityAreaEntity currentCityArea;
        private BikeModelEntity objModel = null;
        private BikeMakeEntityBase objMake = null;
        private EnumBikeType bikeType = EnumBikeType.All;
        private bool showCheckOnRoadCTA = false;
        private PQSourceEnum pqSource = 0;

        public ModelHelper ModelHelper
        {
            get
            {
                return modelHelper;
            }

            set
            {
                modelHelper = value;
            }
        }
        #endregion

        #region Public properties
        public bool IsMobile { get; set; }
        #endregion

        #region Constructor
        public ExpertReviewsScootersPage(ICMSCacheContent cmsCache, IPager pager, IBikeModelsCacheRepository<int> models, IBikeModels<BikeModelEntity, int> bikeModels, IUpcoming upcoming)
        {
            _cmsCache = cmsCache;
            _pager = pager;
            _models = models;
            _bikeModels = bikeModels;
            _upcoming = upcoming;
            ProcessQueryString();
        }
        #endregion

        #region Functions
        /// <summary>
        /// Created by : Vivek Singh Tomar on 17th Aug 2017
        /// Summary    : Function to get the expert reviews landing page data
        /// </summary>
        public ExpertReviewsScootersPageVM GetData(int widgetTopCount)
        {
            ExpertReviewsScootersPageVM objData = new ExpertReviewsScootersPageVM();

            try
            {
                if (objMake != null)
                    objData.Make = objMake;
                if (objModel != null)
                    objData.Model = objModel;

                int _startIndex = 0, _endIndex = 0;
                _pager.GetStartEndIndex(pageSize, curPageNo, out _startIndex, out _endIndex);

                objData.StartIndex = _startIndex;
                objData.EndIndex = _endIndex;

                objData.Articles = _cmsCache.GetArticlesByCategoryList(Convert.ToString((int)EnumCMSContentType.RoadTest), _startIndex, _endIndex, Convert.ToString((int)EnumBikeBodyStyles.Scooter), (int)MakeId, (int)ModelId);

                if (objData.Articles != null && objData.Articles.RecordCount > 0)
                {
                    status = StatusCodes.ContentFound;
                    objData.StartIndex = _startIndex;
                    objData.EndIndex = _endIndex > objData.Articles.RecordCount ? Convert.ToInt32(objData.Articles.RecordCount) : _endIndex;
                    //BindLinkPager(objData);
                    //SetPageMetas(objData);
                    //CreatePrevNextUrl(objData);
                    //GetWidgetData(objData, widgetTopCount);
                }
                else
                {
                    status = StatusCodes.ContentNotFound;
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "Bikewale.Models.ExpertReviewsIndexPage.GetData");
            }

            return objData;
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 17th Aug 2017
        /// Summary    : Process query string for expert reviews page
        /// </summary>
        private void ProcessQueryString()
        {
            var request = HttpContext.Current.Request;
            var queryString = request != null ? request.QueryString : null;

            if (queryString != null)
            {

                if (!string.IsNullOrEmpty(queryString["pn"]))
                {
                    string _pageNo = queryString["pn"];
                    if (!string.IsNullOrEmpty(_pageNo))
                    {
                        int.TryParse(_pageNo, out curPageNo);
                    }
                }
                make = queryString["make"];
                model = queryString["model"];

                //ProcessMakeMaskingName(request, make);
                //ProcessModelMaskingName(request, model);
            }
        }
        #endregion
    }
}