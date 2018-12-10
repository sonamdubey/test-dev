using Carwale.Entity.Classified.Search;
using Carwale.Entity.Enum;
using Carwale.Entity.Geolocation;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.Classified;
using Carwale.Interfaces.Classified.Search;
using Carwale.Interfaces.Geolocation;
using Carwale.Utility;
using System.Configuration;
using System;
using System.Linq;

namespace Carwale.BL.Classified.Search
{
    public class SearchParamsProcesser : ISearchParamsProcesser
    {
        private const int _msitePageSize = 19;
        private const int _deskPageSize = 24;
        private const int _maxPageSize = 25;
        private const int _maxLdr = 3;
        private const int _maxLir = 1;
        private const int _maxFeaturedListings = 4;
        private readonly ICommonOperationsCacheRepository _commonOperationCache;
        private readonly ICarModelCacheRepository _carModelCacheRepository;
        private readonly ICarModelRootsCacheRepository _modelRootsCacheRepo;
        private readonly ISearchUtility _searchUtility;
        private readonly IElasticLocation _elasticLocation;
        private readonly IGeoCitiesCacheRepository _cityRepo;
        private static readonly string[] _nearByCarsCities = ConfigurationManager.AppSettings["UsedNearByCarsCities"].Split(',');

        public SearchParamsProcesser(
            ICommonOperationsCacheRepository commonOperationCache, 
            ICarModelRootsCacheRepository modelRootsCacheRepo, 
            ISearchUtility searchUtility, 
            ICarModelCacheRepository carModelCacheRepository,
            IElasticLocation elasticLocation,
            IGeoCitiesCacheRepository cityRepo
        )
        {
            _commonOperationCache = commonOperationCache;
            _modelRootsCacheRepo = modelRootsCacheRepo;
            _searchUtility = searchUtility;
            _carModelCacheRepository = carModelCacheRepository;
            _elasticLocation = elasticLocation;
            _cityRepo = cityRepo;
        }

        public void ProcessSearchParams(SearchParams searchParams, Platform source, bool isAjaxRequest, out string redirectUrl)
        {
            bool isRedirect = false;
            string rootName;
            string makeName = searchParams.MakeName;
            redirectUrl = null;

            searchParams.Ps = GetPageSize(searchParams.Ps, source);
            ProcessCity(searchParams, isAjaxRequest, out isRedirect);
            ProcessArea(searchParams, isAjaxRequest);
            searchParams.Car = ProcessMakeRoot(searchParams.Car, searchParams.Make, ref makeName, searchParams.Root, ref isRedirect, out rootName);
            if (isRedirect)
            {
                redirectUrl = _searchUtility.GetURL(makeName, rootName, searchParams.CityName, searchParams.Pn);
            }

            if (!isAjaxRequest)
            {
                ProcessListingparams(searchParams);
            }
            searchParams.ShouldFetchNearbyCars = ProcessShouldFetchNearByCars(searchParams.City, searchParams.Latitude, searchParams.Longitude
                                                    , searchParams.ShouldFetchNearbyCars);
        }

        private int GetPageSize(int pageSize, Platform source)
        {
            if (pageSize > _maxPageSize || pageSize <= 0)
            {
                return source == Platform.CarwaleMobile ? _msitePageSize : _deskPageSize;
            }
            return pageSize;
        }

        private void ProcessCity(SearchParams searchParams, bool isAjaxRequest, out bool isRedirect)
        {
            isRedirect = false;
            if (!string.IsNullOrEmpty(searchParams.CityName) && searchParams.City <= 0)
            {
                var cityObj = _cityRepo.GetCityDetailsByMaskingName(searchParams.CityName);
                if (cityObj == null || !searchParams.CityName.Equals(cityObj.CityMaskingName, StringComparison.InvariantCultureIgnoreCase))
                {
                    searchParams.CityName = cityObj?.CityMaskingName;
                    searchParams.City = cityObj == null ? 0 : cityObj.CityId;
                    isRedirect = true;
                }
                else
                {
                    searchParams.City = cityObj.CityId;
                    searchParams.CityName = cityObj.CityName;
                }
            }
            if (!isRedirect && searchParams.City <= 0 && !isAjaxRequest)
            {
                searchParams.City = CustomerCookie.MasterCityId;
            }
            if (searchParams.City == 1)    //For Mumbai case
            {
                searchParams.City = 3000;
            }
            if (searchParams.City > 0)
            {
                searchParams.FilterAppliedCount++; //in case url doesnt have city but global city present
            }
        }

        private void ProcessArea(SearchParams searchParams, bool isAjaxRequest)
        {
            int city = searchParams.City;
            if (city == 3000)
            {
                city = 1;       //mumbai case
            }
            if (!isAjaxRequest && city > 0 && city == CustomerCookie.MasterCityId)
            {
                searchParams.Area = searchParams.Area <= 0 ? CustomerCookie.CustomerAreaId : searchParams.Area;
                if (!RegExValidations.IsValidLatLong(searchParams.Latitude, searchParams.Longitude))
                {
                    searchParams.Latitude = CustomerCookie.CustomerLatitude;
                    searchParams.Longitude = CustomerCookie.CustomerLongitude;
                }
            }
            if (searchParams.Area > 0)
            {
                if(!RegExValidations.IsValidLatLong(searchParams.Latitude, searchParams.Longitude))
                {
                    Area area = _elasticLocation.GetLocation(searchParams.Area);
                    if (area != null)
                    {
                        searchParams.Latitude = area.Latitude;
                        searchParams.Longitude = area.Longitude;
                        searchParams.CustAreaName = area.name;
                    }
                }
                else
                {
                    searchParams.IsLocationDetected = true;
                    searchParams.CustAreaName = CustomerCookie.CustomerAreaName;
                }
            }
        }

        private string ProcessMakeRoot(string makeRoot, int makeId, ref string makeName, string root, ref bool isRedirect, out string rootName)
        {
            rootName = null;
            if (string.IsNullOrEmpty(makeRoot))
            {
                if (makeId > 0) //For url like "/used/make-root-cars/" without car filter
                {
                    int rootId = GetRootId(root, ref isRedirect, out rootName);
                    makeRoot = rootId > 0 ? $"{ makeId }.{ rootId }" : makeId.ToString();
                }
                else if (!string.IsNullOrEmpty(makeName))
                {
                    makeName = string.Empty;
                    isRedirect = true;
                }
            }
            return makeRoot;
        }

        private int GetRootId(string root, ref bool isRedirect, out string rootName)
        {
            int rootId = 0;
            rootName = null;
            if (!string.IsNullOrEmpty(root))
            {
                var carModelMaskingResponse = _commonOperationCache.GetMakeDetailsByRootName(root) ?? _carModelCacheRepository.GetModelByMaskingName(root);
                if (carModelMaskingResponse != null)
                {
                    if (carModelMaskingResponse.RootId > 0)
                    {
                        rootId = carModelMaskingResponse.RootId;
                        rootName = carModelMaskingResponse.RootName;
                    }
                    else if (carModelMaskingResponse.ModelId > 0)
                    {
                        var rootBase = _modelRootsCacheRepo.GetRootByModel(carModelMaskingResponse.ModelId);
                        if (rootBase != null)
                        {
                            rootId = rootBase.RootId;
                            rootName = rootBase.Name;
                        }
                        if (!string.IsNullOrEmpty(carModelMaskingResponse.RootName) && carModelMaskingResponse.RootName.ToLower() != carModelMaskingResponse.MaskingName.ToLower())
                        {
                            isRedirect = true;
                            rootName = carModelMaskingResponse.RootName;
                        }
                    }
                    else
                    {
                        isRedirect = true;
                    }
                }
            }
            return rootId;
        }

        private void ProcessListingparams(SearchParams searchParams)
        {
            if (searchParams.Pn > 1 && searchParams.Lcr == 0)
            {
                int lastPn = searchParams.Pn - 1;
                int minLcr = searchParams.Ps - _maxFeaturedListings;

                searchParams.Lcr = lastPn * minLcr;
                searchParams.Ldr = lastPn * _maxLdr;
                searchParams.Lir = lastPn * _maxLir;
            }
        }
        private static bool ProcessShouldFetchNearByCars(int city, double latitude, double longitude, bool shouldFetchNearByCars)
        {
            var isCityValid = _nearByCarsCities.Any(x => CustomParser.parseIntObject(x) == city);
            var isValidLatLong = RegExValidations.IsValidLatLong(latitude, longitude);
            if(shouldFetchNearByCars && isCityValid && isValidLatLong)
            {
                return true;
            }
            return false;
        }
    }
}
