using Carwale.Entity.Elastic;
using Carwale.Entity.Geolocation;
using Carwale.Entity.PriceQuote;
using Carwale.Interfaces.Geolocation;
using Carwale.Interfaces.PriceQuote;
using Carwale.Notifications;
using ElasticClientManager;
using Microsoft.Practices.Unity;
using Nest;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.BL.NewCars
{
    public class NearbyCitiesSearch : INearbyCitiesSearch
    {
        public static readonly string indexName = ConfigurationManager.AppSettings["PricesElasticIndexName"] ?? "version_city_prices";
        private readonly IGeoCitiesCacheRepository _cityCache;

        public NearbyCitiesSearch(IUnityContainer container, IGeoCitiesCacheRepository cityCache)
        {
            _cityCache = cityCache;
        }

        public bool AddToIndex(int versionId, int cityId)
        {
            try
            {
                VersionCityPricesObj versionCity = GetVersionCityDetails(versionId,cityId);
                if (versionCity != null)
                {
                    IIndexResponse iResponse = ElasticClientOperations.AddDocument<VersionCityPricesObj>(versionCity, indexName, (versionCity.VersionId.ToString()+"_"+versionCity.CityId.ToString()));
                    return true;
                }
                return false;
            }
            catch (Exception err)
            {
                var exception = new ExceptionHandler(err, "NearbyCitiesSearch.AddToIndex()");
                exception.LogException();
                return false;
            }
        }

        public void AddToIndex(List<VehiclePrice> vehiclePriceList)
        {
            foreach (var vehiclePrice in vehiclePriceList)
            {
                AddToIndex(vehiclePrice.CarVersionId, vehiclePrice.CityId);
            }
        }

        public bool DeleteFromIndex(int versionId, int cityId)
        {
            try
            {
               return ElasticClientOperations.DeleteDocument<VersionCityPricesObj>(versionId.ToString()+"_"+cityId.ToString(), indexName);
            }
            catch (Exception err)
            {
                var exception = new ExceptionHandler(err, "NearbyCitiesSearch.DeleteFromIndex()");
                exception.LogException();
                return false;
            }
        }
        private VersionCityPricesObj GetVersionCityDetails(int versionId, int cityId)
        {
            VersionCityPricesObj versionCity = new VersionCityPricesObj();
            try
            {
                Cities cityDetails = _cityCache.GetCityDetailsById(cityId);

                if (cityDetails != null)
                {
                    versionCity = AutoMapper.Mapper.Map<Cities, VersionCityPricesObj>(cityDetails);
                    versionCity.Location.lat /= 3600;
                    versionCity.Location.lon /= 3600;
                    versionCity.VersionId = versionId;
                }
            }
            catch (Exception err)
            {
                var exception = new ExceptionHandler(err, "NearbyCitiesSearch.GetVersionCityDetails()");
                exception.LogException();
            }
            return versionCity;
        }
        public List<VersionCityPricesObj> GetNearByCities (int versionId, int cityId, int count)
        {
            List<VersionCityPricesObj> nearbyCitiesDataSameState = new List<VersionCityPricesObj>();
            try
            {
                VersionCityPricesObj versionCity = GetVersionCityDetails(versionId, cityId);
                ElasticClient client = ElasticClientInstance.GetInstance();

                QueryContainerDescriptor<VersionCityPricesObj> FDS = new QueryContainerDescriptor<VersionCityPricesObj>();

                SearchRequest<VersionCityPricesObj> searchQuerySameState = GetSearchQuery(versionCity, count);
                searchQuerySameState.Query &= FDS.Term(t => t.StateId, versionCity.StateId);
                var nearbyCitySameState = client.Search<VersionCityPricesObj>(searchQuerySameState);
                nearbyCitiesDataSameState = nearbyCitySameState.Documents.ToList();

                if (nearbyCitiesDataSameState.Count() < count)
                {
                    SearchRequest<VersionCityPricesObj> searchQueryOtherState = GetSearchQuery(versionCity, (count - nearbyCitiesDataSameState.Count()));
                    searchQueryOtherState.Query &= !FDS.Term(t => t.StateId, versionCity.StateId);
                    var nearbyCityOtherState = client.Search<VersionCityPricesObj>(searchQueryOtherState);
                    nearbyCitiesDataSameState.AddRange(nearbyCityOtherState.Documents.ToList());
                }
            }
            catch (Exception ex)
            {
                var exception = new ExceptionHandler(ex, "NearbyCitiesSearch.GetNearByCities()");
                exception.LogException();
            }
            return nearbyCitiesDataSameState;
        }

        public SearchRequest<VersionCityPricesObj> GetSearchQuery (VersionCityPricesObj versionCity, int count)
        {
            try
            {
                return new SearchRequest<VersionCityPricesObj>(indexName)
                {
                    Size = count,
                    Query = GetQueryDescriptor(versionCity),
                    Sort = new List<ISort>
                {
                    new GeoDistanceSort
                    {
                         Field = "location",
                         Order = SortOrder.Ascending,
                         Points = new[] { new Nest.GeoLocation(versionCity.Location.lat, versionCity.Location.lon) }
                    }
                }
                };
            }
            catch(Exception err)
            {
                var exception = new ExceptionHandler(err, "NearbyCitiesSearch.GetSearchQuery()");
                exception.LogException();
                throw err;
            }
        }
        public QueryContainer GetQueryDescriptor(VersionCityPricesObj versionCity) 
        {
            try
            {
                QueryContainer queryContainer = new QueryContainer();
                QueryContainerDescriptor<VersionCityPricesObj> FDS = new QueryContainerDescriptor<VersionCityPricesObj>();

                queryContainer &= FDS.Term(t => t.VersionId, versionCity.VersionId);
                queryContainer &= !FDS.Term(t => t.CityId, versionCity.CityId);
                return queryContainer;
            }
            catch(Exception err)
            {
                var exception = new ExceptionHandler(err, "NearbyCitiesSearch.GetQueryDescriptor()");
                exception.LogException();
                throw err;
            }
            
        }
    }
}
