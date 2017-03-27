using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.EditCMS;
using Bikewale.Interfaces.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.Pager;
using Bikewale.Notifications;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.CMS;
using Bikewale.Entities;

namespace Bikewale.Models
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 21 Mar 2017
    /// Summary : Model for the expert reviews landing page
    /// </summary>
    public class ExpertReviewsIndexPage
    {
        #region Variables for dependency injection
        private readonly ICMSCacheContent cache = null;
        private readonly IArticles articles = null;
        private readonly IBikeModels<BikeModelEntity, int> bikeModels = null;
        private readonly IBikeMakesCacheRepository<int> bikeMakesCache = null;
        private readonly IBikeMaskingCacheRepository<BikeModelEntity, int> bikeMaskingCache = null;
        private readonly IPager pager; 
        #endregion

        #region Page level variables
        private const int pageSize = 10, pagerSlotSize = 5;
        private int curPageNo = 1;

        public int MakeId, ModelId;

        protected string makeName = string.Empty, modelName = string.Empty, makeMaskingName = string.Empty, modelMaskingName = string.Empty;
        public string redirectUrl;        
        public StatusCodes status;
        #endregion


        /// <summary>
        /// Constructor for the dependency injection
        /// </summary>
        /// <param name="_cache"></param>
        /// <param name="_articles"></param>
        /// <param name="_bikeModels"></param>
        /// <param name="_bikeMakesCache"></param>
        /// <param name="_bikeMaskingCache"></param>
        /// <param name="_pager"></param>
        public ExpertReviewsIndexPage(ICMSCacheContent _cache, IArticles _articles, IBikeModels<BikeModelEntity, int> _bikeModels, IBikeMakesCacheRepository<int> _bikeMakesCache, IBikeMaskingCacheRepository<BikeModelEntity, int> _bikeMaskingCache, IPager _pager)
        {
            cache = _cache;
            articles = _articles;
            bikeModels = _bikeModels;
            bikeMakesCache = _bikeMakesCache;
            bikeMaskingCache = _bikeMaskingCache;
            pager = _pager;

            ProcessQueryString();
        }

        /// <summary>
        /// Function to get the expert reviews landing page data
        /// </summary>
        /// <returns></returns>
        public ExpertReviewsIndexPageVM GetData()
        {
            ExpertReviewsIndexPageVM objData = null;

            // Write business logic to get the page data
            objData = new ExpertReviewsIndexPageVM();

            try
            {                
                int _startIndex = 0, _endIndex = 0;
                pager.GetStartEndIndex(pageSize, curPageNo, out _startIndex, out _endIndex);

                objData.StartIndex = _startIndex;
                objData.EndIndex = _endIndex;

                objData.Articles = cache.GetArticlesByCategoryList(Convert.ToString((int)EnumCMSContentType.RoadTest), _startIndex, _endIndex, 7, 59);

                if (objData.Articles != null && objData.Articles.RecordCount > 0)
                {
                    //objData.IsContentFound = true;
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "Bikewale.BindViewModels.Webforms.EditCMS.RoadTestListing.GetRoadTestList");
            }

            return objData;
        }   // ExpertReviewsIndexPageVM ends

        /// <summary>        
        /// Function to parse the query string. Function checks if url is valid or need to redirect to new url.
        /// </summary>
        /// <returns></returns>
        private void ProcessQueryString()
        {
            try
            {
                modelMaskingName = HttpContext.Current.Request.QueryString["model"];
                if (!String.IsNullOrEmpty(modelMaskingName))
                {
                    ModelMaskingResponse objResponse = null;

                    objResponse = bikeMaskingCache.GetModelMaskingResponse(modelMaskingName);
                    if (objResponse != null && objResponse.StatusCode == 200)
                    {
                        ModelId = (int)objResponse.ModelId;

                        BikeModelEntity bikemodelEnt = bikeModels.GetById(ModelId);
                        modelName = bikemodelEnt.ModelName;
                    }
                    else
                    {
                        if (objResponse.StatusCode == 301)
                        {
                            status = StatusCodes.RedirectPermanent;                            
                            redirectUrl = HttpContext.Current.Request.RawUrl.Replace(HttpContext.Current.Request.QueryString["model"], objResponse.MaskingName);
                        }
                        else
                        {
                            status = StatusCodes.ContentNotFound;                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "Bikewale.BindViewModels.Webforms.EditCMS.RoadTestListing.ProcessQueryString");
                status = StatusCodes.ContentNotFound;                
            }

            makeMaskingName = HttpContext.Current.Request.QueryString["make"];
            if (!String.IsNullOrEmpty(makeMaskingName))
            {
                MakeMaskingResponse objResponse = null;

                try
                {
                    objResponse = bikeMakesCache.GetMakeMaskingResponse(makeMaskingName);
                }
                catch (Exception ex)
                {
                    Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "Bikewale.BindViewModels.Webforms.EditCMS.RoadTestListing.ParseQueryString");
                    status = StatusCodes.ContentNotFound;                    
                }
                finally
                {
                    if (objResponse != null)
                    {
                        if (objResponse.StatusCode == 200)
                        {
                            MakeId = (int)objResponse.MakeId;
                        }
                        else if (objResponse.StatusCode == 301)
                        {
                            status = StatusCodes.RedirectPermanent;                            
                            redirectUrl = HttpContext.Current.Request.RawUrl.Replace(makeMaskingName, objResponse.MaskingName);
                        }
                        else
                        {
                            status = StatusCodes.ContentNotFound;                            
                        }
                    }
                    else
                    {
                        status = StatusCodes.ContentNotFound;                        
                    }
                }


                BikeMakeEntityBase objMMV = bikeMakesCache.GetMakeDetails((uint)MakeId);
                makeName = objMMV.MakeName;

                if (MakeId <= 0)
                {
                    status = StatusCodes.ContentNotFound;                    
                }
            }

            if (!String.IsNullOrEmpty(HttpContext.Current.Request.QueryString["pn"]))
            {
                if (!Int32.TryParse(HttpContext.Current.Request.QueryString["pn"], out curPageNo))
                    curPageNo = 1;
                else
                    curPageNo = Convert.ToInt32(HttpContext.Current.Request.QueryString["pn"]);
            }

        }

    }   // class
}   // namespace