using Carwale.DAL.CoreDAL;
using Carwale.DAL.Interface.Classified.ElasticSearch;
using Carwale.Entity.Classified;
using Carwale.Entity.Elastic;
using Carwale.Interfaces.Classified.ElasticSearch;
using Carwale.Utility.Classified;
using Nest;
using System;
using System.Collections.Generic;

namespace Carwale.DAL.Classified.ElasticSearch
{
    public class NearbyCityRepository : INearbyCityRepository
    {
        private static readonly ElasticClient _elasticClient = ElasticClientInstance.GetInstance();
        private const int _size = 1000;

        private readonly IQueryContainerRepository<City> _queryContainerRepository;

        public NearbyCityRepository(IQueryContainerRepository<City> queryContainerRepository)
        {
            _queryContainerRepository = queryContainerRepository;
        }

        public List<City> GetFromLatLong(double minLat, double maxLat, double minLong, double maxLong, int cityId, ElasticOuptputs filterInputs)
        {
            List<City> citiesList = new List<City>();
            var cities = _elasticClient.Search<City>(qq => qq
                    .Index(Constants.ClassifiedElasticIndex)
                                .Type("stock")
                                    .Query(q => q
                                        .Range(y => y.Field("lattitude").GreaterThanOrEquals(minLat).LessThanOrEquals(maxLat)) &&
                                        q.Range(y => y.Field("longitude").GreaterThanOrEquals(minLong).LessThanOrEquals(maxLong)) &&
                                        q.Bool(b => b
                                            .Filter(ff => ff
                                                .Bool(bb => bb
                                                    .Must(m => _queryContainerRepository.GetCommonQueryContainerForSearchPage(filterInputs, filterInputs.carsWithPhotos, m, isNearByCity: true))
                                                    .MustNot(mm => mm.Term("cityId", cityId))
                                                )
                                            )
                                        )
                                    )
                                    .Aggregations(aa => aa
                                    .Terms("CityId", h => h.Size(_size).Field("cityId"))
                                    .Terms("CityName", h => h.Size(_size).Field("cityName"))
                                )
                            );

            var cityIdTermAggregateBucket = cities.Aggs.Terms("CityId").Buckets;
            var cityNameTermAggregateBucket = cities.Aggs.Terms("CityName").Buckets;

            using (var cityIdListEnumerator = cityIdTermAggregateBucket.GetEnumerator())
            using (var cityNameListEnumerator = cityNameTermAggregateBucket.GetEnumerator())
            {
                while (cityIdListEnumerator.MoveNext() && cityNameListEnumerator.MoveNext())
                {
                    var currCityIdObject = cityIdListEnumerator.Current;
                    var currCityNameObject = cityNameListEnumerator.Current;

                    City city = new City
                    {
                        CityId = Convert.ToInt32(currCityIdObject.Key),
                        CityName = currCityNameObject.Key,
                        CityCount = Convert.ToInt32(currCityIdObject.DocCount)
                    };
                    citiesList.Add(city);
                }
            }
            return citiesList;
        }
    }
}
