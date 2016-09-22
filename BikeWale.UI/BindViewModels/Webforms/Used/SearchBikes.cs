
using Bikewale.BAL.BikeData;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Cache.Location;
using Bikewale.DAL.BikeData;
using Bikewale.DAL.Location;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Location;
using Bikewale.Memcache;
using Bikewale.Mobile.Controls;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
namespace Bikewale.BindViewModels.Webforms.Used
{
    public class SearchBikes
    {
        protected LinkPagerControl ctrlPager;
        protected Repeater rptUsedListings;
        protected uint cityId;
        bool redirectPermanent, redirectToPageNotFound;
        protected string makeId, modelId = string.Empty;
        protected string makemasking = string.Empty, citymasking = string.Empty, strTotal = string.Empty;// modelmasking = string.Empty, pageno = string.Empty;
        protected string pageTitle = string.Empty, pageDescription = string.Empty, modelName = string.Empty, makeName = string.Empty, pageKeywords = string.Empty, cityName = "India", pageCanonical = string.Empty
                  , heading = string.Empty, nextUrl = string.Empty, prevUrl = string.Empty, redirectUrl = string.Empty;
        private const int _pageSize = 20;
        private int _pageNo = 1;
        protected int _startIndex = 0, _endIndex = 0, totalListing;
        private const int _pagerSlotSize = 5;

        protected IEnumerable<CityEntityBase> cities = null;
        protected IEnumerable<BikeMakeModelBase> makeModels = null;

        public ushort MakeId { get; set; }
        public uint ModelId { get; set; }
        public uint CityId { get; set; }




        private void ProcessQueryString(System.Web.UI.Page page)
        {
            ModelMaskingResponse objModelResponse = null;
            CityMaskingResponse objCityResponse = null;
            string model = string.Empty, city = string.Empty, _make = string.Empty;
            IUnityContainer container = null;
            using (container = new UnityContainer())
            {
                container.RegisterType<IBikeMaskingCacheRepository<BikeModelEntity, int>, BikeModelMaskingCache<BikeModelEntity, int>>()
                                          .RegisterType<ICacheManager, MemcacheManager>()
                                          .RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>()
                                          .RegisterType<ICityCacheRepository, CityCacheRepository>()
                                          .RegisterType<ICityMaskingCacheRepository, CityMaskingCache>()
                                          .RegisterType<ICity, CityRepository>()
                                          .RegisterType<IBikeMakesCacheRepository<int>, BikeMakesCacheRepository<BikeMakeEntity, int>>()
                                          .RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>()
                                          .RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>();
            }
            try
            {
                city = page.Request.QueryString["city"];
                if (!string.IsNullOrEmpty(city))
                {
                    var objCache = container.Resolve<ICityMaskingCacheRepository>();
                    objCityResponse = objCache.GetCityMaskingResponse(city);
                }

                if (!string.IsNullOrEmpty(page.Request.QueryString["make"]))
                {
                    string makeMaskingName = page.Request.QueryString["make"];
                    makeId = MakeMapping.GetMakeId(makeMaskingName);
                    ushort _makeId = default(ushort);
                    //verify the id as passed in the url
                    if (ushort.TryParse(makeId, out _makeId))
                    {
                        var objCache = container.Resolve<IBikeMakesCacheRepository<int>>();
                        BikeMakeEntityBase makeDetails = objCache.GetMakeDetails(_makeId);
                        if (makeDetails != null)
                        {
                            makeName = makeDetails.MakeName;
                        }
                    }
                    else
                    {
                        redirectToPageNotFound = true;
                    }
                }

                model = page.Request.QueryString["model"];
                if (!string.IsNullOrEmpty(model))
                {
                    var objCache = container.Resolve<IBikeMaskingCacheRepository<BikeModelEntity, int>>();
                    objModelResponse = objCache.GetModelMaskingResponse(model);
                    if (objModelResponse != null && objModelResponse.ModelId > 0)
                    {
                        var objCachenew = container.Resolve<IBikeModels<BikeModelEntity, int>>();
                        BikeModelEntity modelEntity = objCachenew.GetById(Convert.ToInt32(objModelResponse.ModelId));
                        modelName = modelEntity.ModelName;
                    }
                }

                if (!String.IsNullOrEmpty(page.Request.QueryString["pn"]))
                {
                    int result;
                    int.TryParse(page.Request.QueryString["pn"], out result);
                    _pageNo = result;
                }

            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, page.Request.ServerVariables["URL"] + "ParseQueryString");
                objErr.SendMail();

                page.Response.Redirect("/customerror.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                page.Visible = false;
            }
            finally
            {
                if (objCityResponse != null)
                {
                    var objCache = container.Resolve<ICityMaskingCacheRepository>();
                    IEnumerable<CityEntityBase> GetCityDetails = null;// GetAllCities();
                    CityEntityBase cityBase = null;//(from c in GetCityDetails
                                              // where c.CityMaskingName == city
                                              // select c).FirstOrDefault();
                    if (cityBase != null)
                    {
                        cityName = cityBase.CityName;
                    }
                    // Get cityId
                    // Code to check whether masking name is changed or not. If changed redirect to appropriate url
                    if (objCityResponse.StatusCode == 200)
                    {
                        cityId = objCityResponse.CityId;
                    }
                    else if (objCityResponse.StatusCode == 301)
                    {
                        //redirect permanent to new page                         
                        redirectUrl = page.Request.RawUrl.ToLower().Replace(city, objCityResponse.MaskingName);
                        redirectPermanent = true;
                    }
                    else
                    {
                        redirectToPageNotFound = true;
                    }
                }
                if (objModelResponse != null)
                {
                    // Get ModelId
                    // Code to check whether masking name is changed or not. If changed redirect to appropriate url
                    if (objModelResponse.StatusCode == 200)
                    {
                        modelId = objModelResponse.ModelId.ToString();
                    }
                    else if (objModelResponse.StatusCode == 301)
                    {
                        //redirect permanent to new page                         
                        redirectUrl = page.Request.RawUrl.ToLower().Replace(model, objModelResponse.MaskingName);
                        redirectPermanent = true;
                    }
                    else
                    {
                        redirectToPageNotFound = true;
                    }
                }
            }
        }



    }
}