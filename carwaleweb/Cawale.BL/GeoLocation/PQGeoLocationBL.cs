using AutoMapper;
using AEPLCore.Cache;
using Carwale.Cache.Geolocation;
using Carwale.DAL.Geolocation;
using Carwale.DTOs.Geolocation;
using Carwale.Entity;
using Carwale.Entity.Geolocation;
using Carwale.Entity.Geolocation.LatLongURI;
using Carwale.Interfaces;
using Carwale.Interfaces.Geolocation;
using Carwale.Notifications;
using Carwale.Notifications.Logs;
using Microsoft.Practices.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using AEPLCore.Cache.Interfaces;

namespace Carwale.BL.GeoLocation
{
    public class PQGeoLocationBL : IPQGeoLocationBL
    {
        private readonly IGeoCitiesRepository _geoRepo;
        private readonly IGeoCitiesCacheRepository _geoCache;
        private readonly ICacheManager _cache;

        public PQGeoLocationBL(IUnityContainer container)
        {
            container.RegisterType<IGeoCitiesRepository, GeoCitiesRepository>()
                .RegisterType<IGeoCitiesCacheRepository, GeoCitiesCacheRepository>()
                .RegisterType<ICacheManager, CacheManager>();

            _geoRepo = container.Resolve<IGeoCitiesRepository>();
            _geoCache = container.Resolve<IGeoCitiesCacheRepository>();
            _cache = container.Resolve<ICacheManager>();
        }

        /// <summary>
        /// Created by Rohan Sapkal
        /// 20-03-2015
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns>Returns Popular cities for PQ</returns>
        public StatesAndPopularCities GetPQStatesAndPopularCities(int modelId)
        {
            StatesAndPopularCities obj = new StatesAndPopularCities();
            try
            {
                obj.States = _geoCache.GetPQStatesByModelId(modelId);
            }
            catch (Exception err)
            {
                var exception = new ExceptionHandler(err, "GetPQStatesAndPopularCities(states) in PQGeoLocationBL url-" + HttpContext.Current.Request.ServerVariables["URL"]);
                exception.LogException();
            }
            try
            {
                obj.Cities = _geoCache.GetPQPopularCities(modelId);
            }
            catch (Exception err)
            {
                var exception = new ExceptionHandler(err, "GetPQStatesAndPopularCities(cities) in PQGeoLocationBL url-" + HttpContext.Current.Request.ServerVariables["URL"]);
                exception.LogException();
            }
            return obj;
        }

        /// <summary>
        /// Returns the City Details based on Latitude and Longitude passed
        /// Written By : Shalini Nair on 14/05/2015
        /// </summary>
        /// <param name="querystring"></param>
        /// <returns></returns>
        public City GetCityDetailsByLatLong(LatLongURI querystring)
        {
            var cityDetails = new City();
            try
            {
                var cityLatLong = new LatLongURI();

                //Converting the latitude and longitude from degrees to seconds
                var latitudeInSeconds = Convert.ToDouble(querystring.Latitude) * 3600;
                var longitudeInSeconds = Convert.ToDouble(querystring.Longitude) * 3600;

                cityLatLong.Latitude = latitudeInSeconds.ToString();
                cityLatLong.Longitude = longitudeInSeconds.ToString();

                cityDetails = _geoRepo.GetCityDetailsByLatLong(cityLatLong);
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "PQGeoLocationBL.GetCityDetailsByLatLong()");
                objErr.LogException();
            }
            return cityDetails;
        }

        public City GetCityById(int id)
        {
            if (id == -1)
                return new City() { CityId = -1, CityName = "" };
            else
            {
                var city = new City() { CityId = id };
                try
                {
					Cities cityDetails = _geoCache.GetCityDetailsById(id);
					if (cityDetails == null)
					{
						city = new City() { CityId = -1, CityName = "" };
					}
					else
					{
						city.CityName = cityDetails.CityName;
						city.CityMaskingName = cityDetails.CityMaskingName;
					}
				}
                catch (Exception ex)
                {
                    ExceptionHandler objErr = new ExceptionHandler(ex, "PQGeoLocationBL.GetCityById()");
                    objErr.LogException();
                }
                return city;
            }
        }

        /// <summary>
        /// Returns the zones and grouped cities of Mumbai and Delhi
        /// TODO:Bangalore Zone Refactoring
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        [Obsolete("Please use GetPQCityGroupsZones instead")]
        public List<CityZones> GetPQCityZonesList(int modelId)
        {
            try
            {
                var cityZoneList = new List<CityZones>();
                var zones = _geoCache.GetPQCityZonesList(modelId);

                cityZoneList.Add(new CityZones()
                {
                    Mumbai = zones.Where(x => x.GroupMasterId == 1).ToList(),
                    Bangalore = zones.Where(x => x.GroupMasterId == 3).ToList(),
                    Delhi = zones.Where(x => x.GroupMasterId == 2).ToList()
                });

                return cityZoneList;
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "PQGeoLocationBL.GetPQCityZonesList()");
                objErr.LogException();
                return null;
            }
        }

        /// <summary>
        /// This function returns the zones and groups for a modelId
        /// </summary>
        /// <param name="modelId">ModelId</param>
        /// <returns></returns>
        private List<PQGroupCity> GetPQGroupCities(int modelId)
        {
            try
            {
                var groupCities = new List<PQGroupCity>();
                var zones = _geoCache.GetPQZones(modelId);
                var groups = _geoCache.GetPQGroupCities(modelId);
                var distinctCities = GetGroupMasterCities(modelId);

                groupCities.AddRange(distinctCities.Select(c => new PQGroupCity
                {
                    CityId = c.CityId,
                    CityName = c.CityName,
                    Zones = zones.Where(z => z.CityId == c.CityId).ToList(),
                    Group = groups.Where(g => g.GroupMasterId == c.GroupMasterId).ToList(),
                    GroupName = c.GroupName
                }));

                return groupCities;
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "PQGeoLocationBL.GetPQCityGroupsZones()");
                objErr.LogException();
                return null;
            }
        }

        /// <summary>
        /// This function returns the cities, zones and city groups of a model 
        /// based on modelid, cityid and stateid passed
        /// </summary>
        /// <param name="modelId">Model Id</param>
        /// <param name="cityId">City Id</param>
        /// <param name="stateId">State Id</param>
        /// <returns></returns>
        public PQCityDTO GetPQCitiesZones(int modelId, int cityId, int stateId)
        {
            PQCityDTO pqCities = new PQCityDTO();
            try
            {
                var cities = _geoCache.GetPQCitiesByModelId(modelId);
                var groupCities = FilterCitiesV1(GetPQGroupCities(modelId), stateId, cityId);

                pqCities.Cities = Mapper.Map<List<City>, List<CityDTO>>(cities.Where(x =>
                    (stateId <= 0 || x.StateId == stateId) && (cityId <= 0 || x.CityId == cityId)).ToList());
                pqCities.GroupCities = Mapper.Map<List<PQGroupCity>, List<PQGroupCityDTO>>(groupCities);

                RemoveMasterCityFromGroup(ref pqCities.GroupCities);

                //Filter groupcities like 'Mumbai,Thane' from cities list 
                pqCities.Cities = pqCities.Cities
                               .Where(c => !pqCities.GroupCities.Any(g => g.CityId == c.Id || g.Group.Any(gc => gc.Id == c.Id)))
                               .ToList();

                return pqCities;
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "PQGeoLocationBL.GetPQCitiesZones()");
                objErr.LogException();
                return null;
            }
        }

        public bool GetPriceAvailability(int modelId, int cityId)
        {
            try
            {
                var pqCitiesZones = this.GetPQCitiesZones(modelId, 0, 0);
                if (pqCitiesZones == null || (pqCitiesZones.Cities == null && pqCitiesZones.GroupCities == null)
                                          || (pqCitiesZones.Cities.Count == 0 && pqCitiesZones.GroupCities.Count == 0))
                {
                    return false;
                }

                var groupCity = pqCitiesZones.GroupCities.FirstOrDefault(x => x.CityId == cityId);
                if (groupCity != null)
                {
                    return true;
                }

                var city = pqCitiesZones.Cities.FirstOrDefault(x => x.Id == cityId);
                if (city != null)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "PQGeoLocationBL.GetPriceAvailabiltiy()");
                objErr.LogException();
                return false;
            }

        }

        /// <summary>
        /// This function returns the cities, city groups and its zones of a model
        /// based on modelId,cityId and stateId passed
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="groupId"></param>
        /// <param name="stateId"></param>
        /// <returns></returns>
        public PQCityDTOV2 GetPQCitiesZonesV2(int modelId, int groupId, int stateId)
        {
            try
            {
                var pqCities = new PQCityDTOV2();
                var cities = _geoCache.GetPQCitiesByModelId(modelId);
                var groupCities = GetPQGroupCitiesV2(modelId).Where(x => ((groupId <= 0 || x.Cities.Any(c => c.GroupMasterId == groupId))
                                                                      && (stateId <= 0 || x.Cities.Any(c => c.StateId == stateId)))).ToList();
                if (stateId > 0)
                {
                    FilterCitiesV2(ref groupCities, stateId);
                }

                pqCities.Cities = Mapper.Map<List<City>, List<CityDTO>>(cities.Where(x => (stateId <= 0 || x.StateId == stateId) && (groupId <= 0)).ToList());
                pqCities.GroupCities = Mapper.Map<List<PQGroupCityV2>, List<PQGroupCityDTOV2>>(groupCities);
                pqCities.GroupCities.ForEach(gc => gc.Cities.Where(x => x.Zones.Count <= 0).Each(z => z.Zones = null));

                //Filter groupcities like 'Mumbai,Thane' from cities list 
                pqCities.Cities = pqCities.Cities.Where(c => !pqCities.GroupCities.Any(g => g.Cities.Any(cg => cg.Id == c.Id))).ToList();
                return pqCities;
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "PQGeoLocationBL.GetPQCitiesZonesV2()");
                objErr.LogException();
                return null;
            }
        }

        /// <summary>
        /// This function filters the cities based on stateId and cityId
        /// </summary>
        /// <param name="zones"></param>
        /// <param name="stateId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        private List<PQGroupCity> FilterCitiesV1(List<PQGroupCity> zones, int stateId, int cityId)
        {
            var groupCities = (zones.Where(x => ((stateId <= 0) || ((x.Zones.Any(z => z.StateId == stateId)) || (x.Group.Any(t => t.StateId == stateId))))
                   && ((cityId <= 0) || ((x.Zones.Any(z => z.CityId == cityId)) || (x.Group.Any(t => t.CityId == cityId))))).ToList());

            if (groupCities != null && groupCities.Count > 0)
            {
                groupCities[0].Group = groupCities[0].Group.Where(x => ((stateId <= 0 || x.StateId == stateId))).ToList();
                groupCities[0].Zones = groupCities[0].Zones.Where(x => ((stateId <= 0 || x.StateId == stateId))).ToList();
            }

            return groupCities;
        }

        /// <summary>
        /// This function filters the cities based on stateId
        /// </summary>
        /// <param name="groupCities"></param>
        /// <param name="stateId"></param>
        /// <returns></returns>
        private void FilterCitiesV2(ref List<PQGroupCityV2> groupCities, int stateId)
        {
            var groupCitiesList = groupCities;
            for (var group = 0; group < groupCities.Count; group++)
            {
                var citiesCount = groupCities[group].Cities.Count;
                for (var city = citiesCount - 1; city >= 0; city--)
                {
                    if (groupCities[group].Cities[city].StateId != stateId)
                    {
                        groupCities.Where(x => x.Cities.Remove(groupCitiesList[group].Cities[city])).ToList();
                    }
                }
            }
        }
        /// <summary>
        /// This function returns the distinct group master cities
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        private List<Entity.Geolocation.Zone> GetGroupMasterCities(int modelId)
        {
            var groupMasterCities = _geoCache.GetPQZones(modelId).GroupBy(t => t.CityId).Select(c => c.First()).ToList();
            var citiesFromGroups = _geoCache.GetPQGroupCities(modelId).GroupBy(t => t.GroupMasterId).Select(c => c.First()).ToList();

            groupMasterCities.AddRange(citiesFromGroups.Select(c => new Entity.Geolocation.Zone
            {
                GroupName = c.GroupName,
                GroupMasterId = c.GroupMasterId,
            }).Where(c => !groupMasterCities.Any(g => g.GroupMasterId == c.GroupMasterId)));
            return groupMasterCities;
        }

        public List<CityZonesDTOV2> GetZonesbyState(int stateId)
        {
            try
            {
                var zoneCities = _geoCache.GetZonesByState(stateId);
                var cities = new List<CityZonesV2>();
                if (zoneCities != null)
                {
                    var distinctCities = zoneCities.GroupBy(t => t.CityId).Select(c => c.First()).ToList();
                    cities.AddRange(distinctCities.Select(c => new CityZonesV2
                    {
                        CityId = c.CityId,
                        CityName = c.CityName,
                        Zones = zoneCities.Where(g => g.CityId == c.CityId).ToList()
                    }));
                    List<CityZonesDTOV2> cityZones = Mapper.Map<List<CityZonesV2>, List<CityZonesDTOV2>>(cities);
                    return cityZones;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return null;
            }
        }

        /// <summary>
        /// This function returns the group cities with its zones
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        private List<CityZonesV2> GetGroupCityZones(int modelId)
        {
            var cities = new List<CityZonesV2>();
            var groups = _geoCache.GetPQGroupCities(modelId);
            var zones = _geoCache.GetPQZones(modelId);

            cities.AddRange(groups.Select(c => new CityZonesV2
            {
                CityId = c.CityId,
                CityName = c.CityName,
                Zones = zones.Where(g => g.CityId == c.CityId).ToList(),
                GroupMasterId = c.GroupMasterId,
                GroupName = c.GroupName,
                StateId = c.StateId,
                StateName = c.StateName
            }));

            return cities;
        }

        /// <summary>
        /// This function returns the list of cities belonging to group 
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        private List<PQGroupCityV2> GetPQGroupCitiesV2(int modelId)
        {
            try
            {
                var groupCities = new List<PQGroupCityV2>();
                var distinctCities = GetGroupMasterCities(modelId);
                var cities = GetGroupCityZones(modelId);

                groupCities.AddRange(distinctCities.Select(c => new PQGroupCityV2
                {
                    GroupId = c.GroupMasterId,
                    GroupName = c.GroupName,
                    Cities = cities.Where(city => city.GroupMasterId == c.GroupMasterId).ToList(),
                }));
                return groupCities;
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "PQGeoLocationBL.GetPQCityGroupsZones()");
                objErr.LogException();
                return null;
            }
        }

        public List<GroupCityDTO> GetGroupCities(int stateId)
        {
            try
            {
                List<GroupCity> groupCities = FetchGroupCities(stateId);

                return Mapper.Map<List<GroupCity>, List<GroupCityDTO>>(groupCities);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return null;
            }
        }

        public List<GroupCity> FetchGroupCities(int stateId)
        {
            try
            {
                var groupCities = new List<GroupCity>();
                var masterGroupCities = _geoCache.GetMasterGroupCities();
                if (stateId > 0)
                {
                    masterGroupCities = masterGroupCities.Where(x => x.StateId == stateId).ToList();
                }

                if (masterGroupCities != null)
                {
                    List<City> allGroupCities = _geoCache.GetAllGroupCities();
                    allGroupCities.InsertRange(0, masterGroupCities);

                    if (stateId != -1 && stateId != 0)
                    {
                        allGroupCities = allGroupCities.Where(x => x.StateId == stateId).ToList();
                    }

                    groupCities.AddRange(masterGroupCities.Select(c => new GroupCity
                    {
                        GroupId = c.GroupMasterId,
                        GroupName = c.GroupName,
                        Cities = allGroupCities.Where(city => city.GroupMasterId == c.GroupMasterId).ToList(),
                    }));

                    return groupCities;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return null;
            }
        }

        /// <summary>
        /// This function removes the groupmaster city from a group
        /// </summary>
        /// <param name="pqGroupCities"></param>
        private void RemoveMasterCityFromGroup(ref List<PQGroupCityDTO> pqGroupCities)
        {
            var groupCities = pqGroupCities;
            for (var group = 0; group < pqGroupCities.Count; group++)
            {
                var citiesCount = pqGroupCities[group].Group.Count;
                for (var city = citiesCount - 1; city >= 0; city--)
                {
                    if (pqGroupCities[group].Group[city].Id == pqGroupCities[group].CityId)
                    {
                        var masterCity = groupCities[group].Group[city];
                        pqGroupCities.Where(x => x.Group.Remove(masterCity)).ToList();
                    }
                }
            }
        }

        public Location GetCityDetails(int cityId, int zoneId, int areaId)
        {
            try
            {
                Location city = null;
                if (areaId > 0)
                {
                    city = Mapper.Map<Area, Location>(new ElasticLocation().GetLocation(areaId));

                }
                else
                {
                    city = Mapper.Map<CustLocation, Location>(_geoCache.GetCustLocation(cityId, zoneId.ToString()));
                }
                return city;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw;
            }
        }


        public LocationV2 GetCityDetailsV2(int cityId, int zoneId, int areaId)
        {
            try
            {
                LocationV2 city = null;
                if (areaId > 0)
                {
                    city = Mapper.Map<Area, LocationV2>(new ElasticLocation().GetLocation(areaId));
                }
                else
                {
                    city = Mapper.Map<CustLocation, LocationV2>(_geoCache.GetCustLocation(cityId, zoneId.ToString()));
                }

                if (city == null || string.IsNullOrEmpty(city.CityName))
                {
                    return null;
                }

                city.ZoneId = (string.IsNullOrEmpty(city.ZoneId) || city.ZoneId == "0") ? "" : city.ZoneId;

                return city;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw;
            }
        }

        public Dictionary<string, CustLocation> MultiGetCityNameFromCache(List<int> cityIds)
        {

            if (cityIds == null || cityIds.Count <= 0)
            {
                return new Dictionary<string, CustLocation>();
            }

            var citiesCallbackfunction = new Dictionary<string, MultiGetCallback<CustLocation>>();

            foreach (var cityId in cityIds)
            {
                if (!citiesCallbackfunction.ContainsKey(string.Format("pq-cust-city-{0}", cityId)))
                {
                    citiesCallbackfunction.Add(string.Format("pq-cust-city-{0}", cityId), new MultiGetCallback<CustLocation>
                    {
                        CacheDuration = CacheRefreshTime.OneDayExpire(),
                        DbCallback = () => _geoRepo.GetCustLocation(cityId, "")
                    });
                }
            }
            return _cache.MultiGetFromCache(citiesCallbackfunction);
        }
    }
}