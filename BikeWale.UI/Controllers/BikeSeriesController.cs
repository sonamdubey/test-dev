using Bikewale.Common;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Compare;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Compare;
using Bikewale.Interfaces.UsedBikes;
using Bikewale.Interfaces.Videos;
using Bikewale.Models.BikeSeries;
using System;
using System.Web.Mvc;

namespace Bikewale.Controllers
{
    /// <summary>
    /// Created by : Ashutosh Sharma on 15 Nov 2017
    /// Description : UI controller for bike series page.
    /// </summary>
    public class BikeSeriesController : Controller
    {
        private readonly IBikeSeries _bikeSeries = null;
        private readonly IUsedBikesCache _usedBikesCache;
        private readonly ICMSCacheContent _articles = null;
        private readonly IVideos _videos = null;
        private readonly IBikeSeriesCacheRepository _seriesCache = null;
        private readonly IBikeCompare _compareScooters = null;
        private readonly ModelController _modelController = null;

        /// <summary>
        /// Modified by :   Ashish Kamble on 22 Nov 2017
        /// Description :   Resolve ModelController
        /// </summary>
        /// <param name="seriesCache"></param>
        /// <param name="usedBikesCache"></param>
        /// <param name="bikeSeries"></param>
        /// <param name="articles"></param>
        /// <param name="videos"></param>
        /// <param name="compareScooters"></param>
        public BikeSeriesController(IBikeSeriesCacheRepository seriesCache, IUsedBikesCache usedBikesCache, IBikeSeries bikeSeries, ICMSCacheContent articles, IVideos videos, IBikeCompare compareScooters)
        {
            _bikeSeries = bikeSeries;
            _usedBikesCache = usedBikesCache;
            _articles = articles;
            _videos = videos;
            _seriesCache = seriesCache;
            _compareScooters = compareScooters;

            try
            {
                //This is to initialize _modelController member variable. So that we can invoke action method on ModelController.
                if (DependencyResolver.Current != null)
                    _modelController = DependencyResolver.Current.GetService<ModelController>();
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "BikeSeriesController.Ctor - Error occured while getting model controller");
            }
        }
        /// <summary>
        /// Created by : Ashutosh Sharma on 15 Nov 2017
        /// Description : Action method for desktop.
        /// Modified by :   Ashish Kamble on 22 Nov 2017
        /// Description :   Masking Name processing for Series and model. If given masking name is not series masking name then redirect to model page
        /// </summary>
        /// <returns></returns>
        [Route("make/{makeMaskingName}/series/{maskingName}/"), Filters.DeviceDetection]
        public ActionResult Index(string makeMaskingName, string maskingName, uint? versionId)
        {
            ActionResult objResult = null;
            bool isMakePermanantRedirect = false;
            makeMaskingName = ProcessMakeMaskingName(makeMaskingName, out isMakePermanantRedirect);
            SeriesMaskingResponse objResponse = _seriesCache.ProcessMaskingName(String.Format("{0}_{1}", makeMaskingName, maskingName));

            if (objResponse != null)
            {
                if (!(objResponse.IsSeriesPageCreated || isMakePermanantRedirect))
                {
                    if (_modelController != null)
                    {
                        //forward Controller context for Model Controller. This is require as ModelController refer Request variables e.g. Request.RawUrl/Request.Cookies
                        _modelController.ControllerContext = new ControllerContext(this.Request.RequestContext, _modelController);

                        //Call Action Method of Model Controller
                        objResult = _modelController.Index(makeMaskingName, maskingName, versionId);
                    }
                    else
                    {
                        objResult = HttpNotFound();
                    }
                }
                else if (objResponse.StatusCode == 301 || isMakePermanantRedirect)
                {
                    string url = string.Format("/{0}-bikes/{1}/", makeMaskingName, objResponse.NewMaskingName);
                    objResult = RedirectPermanent(url);
                }
                else
                {
                    SeriesPageVM obj;

                    SeriesPage seriesPage = new SeriesPage(_seriesCache, _usedBikesCache, _bikeSeries, _articles, _videos, _compareScooters);
                    seriesPage.CompareSource = CompareSources.Desktop_SeriesPage;
                    seriesPage.IsMobile = false;
                    seriesPage.MaskingName = maskingName;
                    obj = seriesPage.GetData(objResponse.SeriesId);
                    objResult = View(obj);
                }
            }
            else
            {
                objResult = HttpNotFound();
            }
            return objResult;
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 15 Nov 2017
        /// Description : Action method for mobile.
        /// Modified by :   Ashish Kamble on 22 Nov 2017
        /// Description :   Masking Name processing for Series and model. If given masking name is not series masking name then redirect to model page
        /// </summary>
        /// <returns></returns>
        [Route("m/make/{makeMaskingName}/series/{maskingName}/")]
        public ActionResult Index_Mobile(string makeMaskingName, string maskingName, uint? versionId)
        {
            ActionResult objResult = null;
            bool isMakePermanantRedirect = false;
            makeMaskingName = ProcessMakeMaskingName(makeMaskingName, out isMakePermanantRedirect);
            SeriesMaskingResponse objResponse = _seriesCache.ProcessMaskingName(String.Format("{0}_{1}", makeMaskingName, maskingName));

            if (objResponse != null)
            {
                //If Model Page, pass control to model controller and get the view result
                if (!(objResponse.IsSeriesPageCreated || isMakePermanantRedirect))
                {
                    if (_modelController != null)
                    {
                        //forward Controller context for Model Controller. This is require as ModelController refer Request variables e.g. Request.RawUrl/Request.Cookies
                        _modelController.ControllerContext = new ControllerContext(this.Request.RequestContext, _modelController);

                        //Call Action Method of Model Controller
                        objResult = _modelController.Index_Mobile(makeMaskingName, maskingName, versionId);
                    }
                    else
                    {
                        objResult = HttpNotFound();
                    }
                }
                else if (objResponse.StatusCode == 301 || isMakePermanantRedirect)
                {
                    string url = string.Format("/m/{0}-bikes/{1}/", makeMaskingName, objResponse.NewMaskingName);
                    objResult = RedirectPermanent(url);
                }
                else
                {
                    SeriesPageVM obj;

                    SeriesPage seriesPage = new SeriesPage(_seriesCache, _usedBikesCache, _bikeSeries, _articles, _videos, _compareScooters);
                    seriesPage.IsMobile = true;
                    seriesPage.MaskingName = maskingName;
                    seriesPage.CompareSource = CompareSources.Mobile_SeriesPage;
                    obj = seriesPage.GetData(objResponse.SeriesId);
                    objResult = View(obj);
                }
            }
            else
            {
                objResult = HttpNotFound();
            }

            return objResult;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 12 Dec 2017
        /// Description :   Processes Make Masking name
        /// </summary>
        /// <param name="makeMaskingName"></param>
        /// <param name="is301"></param>
        /// <returns></returns>
        private string ProcessMakeMaskingName(string makeMaskingName, out bool is301)
        {
            MakeMaskingResponse makeResponse = null;
            is301 = false;
            if (!string.IsNullOrEmpty(makeMaskingName))
            {
                MakeHelper makeHelper = new MakeHelper();
                makeResponse = makeHelper.GetMakeByMaskingName(makeMaskingName);
                if (makeResponse != null)
                {
                    if (makeResponse.StatusCode == 200)
                    {
                        return makeResponse.MaskingName;
                    }
                    else if (makeResponse.StatusCode == 301)
                    {
                        is301 = true;
                        return makeResponse.MaskingName;
                    }
                    else
                    {
                        return "";
                    }
                }
            }
            return "";
        }
    }

}