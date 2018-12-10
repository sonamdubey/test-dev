using Carwale.Entity.Classified;
using Carwale.Entity.Elastic;
using Carwale.Entity.Enum;
using Carwale.Interfaces.Classified.ElasticSearch;
using Carwale.Utility.Classified;
using Nest;
using System;
using System.Configuration;

namespace Carwale.DAL.Classified.ElasticSearch
{
    public class AggregationQueryDescriptor : IAggregationQueryDescriptor
    {
        private const int _maxStocksPerDealer = 2;
        private const int _countAggsCount = 1000;
        private static string[] _mumbaiAndAroundCityIds = ConfigurationManager.AppSettings["MumbaiAroundCityIds"].Split(',');
        private static string[] _delhiNcrCityIds = ConfigurationManager.AppSettings["DelhiNCRCityIds"].Split(',');
        private readonly IQueryContainerRepository<StockBaseEntity> _queryContainerDescriptor;
        public AggregationQueryDescriptor(IQueryContainerRepository<StockBaseEntity> queryContainerDescriptor)
        {
            _queryContainerDescriptor = queryContainerDescriptor;
        }

        public FilterAggregationDescriptor<StockBaseEntity> GetTotalStockCountDescriptor(ElasticOuptputs elasticInputs)
        {
            
            FilterAggregationDescriptor<StockBaseEntity> filterAggregationDescriptor = new FilterAggregationDescriptor<StockBaseEntity>();
            filterAggregationDescriptor.Filter(f =>
            {
                return _queryContainerDescriptor.GetCommonQueryContainerForSearchPage(elasticInputs, elasticInputs.carsWithPhotos, f);
            });
            return filterAggregationDescriptor;
        }

        public FilterAggregationDescriptor<StockBaseEntity> GetSellerTypeCountDescriptor(ElasticOuptputs elasticInputs)
        {
            int sellerTypes = 2;           //two types:- dealer, individual
            var sellers = elasticInputs.sellers;
            elasticInputs.sellers = null;
            FilterAggregationDescriptor<StockBaseEntity> filterAggregationDescriptor = new FilterAggregationDescriptor<StockBaseEntity>();
            filterAggregationDescriptor.Filter(f =>
            {
                return _queryContainerDescriptor.GetCommonQueryContainerForSearchPage(elasticInputs, elasticInputs.carsWithPhotos, f);
            })
            .Aggregations(k => k
                .Terms("SellerIdCount", l => l
                    .Size(sellerTypes)
                    .Field("sellerType")
                )
            );
            elasticInputs.sellers = sellers;
            return filterAggregationDescriptor;
        }

        public FilterAggregationDescriptor<StockBaseEntity> GetCityCountDescriptor(ElasticOuptputs elasticInputs)
        {
            var cities = elasticInputs.cities;
            //Sending blank array instead of null, because in query, for null cities array we are excluding diamond package
            //null cities array implies all india case
            elasticInputs.cities = new string[] { };
            FilterAggregationDescriptor<StockBaseEntity> filterAggregationDescriptor = new FilterAggregationDescriptor<StockBaseEntity>();
            filterAggregationDescriptor.Filter(f =>
            {
                return _queryContainerDescriptor.GetCommonQueryContainerForSearchPage(elasticInputs, elasticInputs.carsWithPhotos, f);
            })
            .Aggregations(a => a
                            .Terms("CityIdCount", t => t
                                .Size(_countAggsCount)
                                .Field("citiesMapping.keyword")
                            )
                        );
            elasticInputs.cities = cities;
            return filterAggregationDescriptor;
        }

        public FilterAggregationDescriptor<StockBaseEntity> GetFuelTypeCountDescriptor(ElasticOuptputs elasticInputs)
        {
            var fuels = elasticInputs.fuels;
            elasticInputs.fuels = null;
            FilterAggregationDescriptor<StockBaseEntity> filterAggregationDescriptor = new FilterAggregationDescriptor<StockBaseEntity>();
            filterAggregationDescriptor.Filter(f =>
            {
                return _queryContainerDescriptor.GetCommonQueryContainerForSearchPage(elasticInputs, elasticInputs.carsWithPhotos, f);
            })
            .Aggregations(a => a
                            .Terms("FuelIdCount", t => t
                                .Size(_countAggsCount)
                                .Field("fuelType")
                            )
                        );
            elasticInputs.fuels = fuels;
            return filterAggregationDescriptor;
        }

        /// <summary>
        /// This method returns all makes count for a filter applied excluding the make and root filter
        /// The root count is result is returned if any make is selected
        /// </summary>
        /// <param name="elasticInputs"></param>
        /// <returns></returns>
        public FilterAggregationDescriptor<StockBaseEntity> GetMakeAndItsRootCountDescriptor(ElasticOuptputs elasticInputs)
        {
            var tempMakes = elasticInputs.NewMakes;
            var tempRoots = elasticInputs.NewRoots;
            elasticInputs.NewMakes = null;
            elasticInputs.NewRoots = null;
            FilterAggregationDescriptor<StockBaseEntity> filterAggregationDescriptor = new FilterAggregationDescriptor<StockBaseEntity>();
            filterAggregationDescriptor.Filter(f =>
            {
                return _queryContainerDescriptor.GetCommonQueryContainerForSearchPage(elasticInputs, elasticInputs.carsWithPhotos, f);
            })
            .Aggregations(a => a
                            .Terms("MakeIdCount", t => t
                                .Size(_countAggsCount)
                                .Field("makeMapping.keyword")
                                .Aggregations(aa => aa
                                     .Filter("Root_Count", fil2 =>
                                     {
                                         //If make is selected then add its root count
                                         return elasticInputs.cars != null ?
                                         fil2.Filter(ff => ff
                                            .Bool(bb => bb
                                                .Must(mu => mu
                                                    .Terms(terms => terms
                                                        .Field("makeId").Terms<string>(elasticInputs.cars)
                                                    )
                                                )
                                            )
                                        )
                                        .Aggregations(aaa => aaa
                                            .Terms("RootIdCount", f2 => f2
                                                .Size(_countAggsCount).Field("rootMapping.keyword")
                                            )
                                        )
                                        : fil2;
                                     })
                                )
                            )
                        );
            elasticInputs.NewMakes = tempMakes;
            elasticInputs.NewRoots = tempRoots;
            return filterAggregationDescriptor;
        }

        public FilterAggregationDescriptor<StockBaseEntity> GetBodyTypeCountDescriptor(ElasticOuptputs elasticInputs)
        {
            var bodyTypes = elasticInputs.bodytypes;
            elasticInputs.bodytypes = null;
            FilterAggregationDescriptor<StockBaseEntity> filterAggregationDescriptor = new FilterAggregationDescriptor<StockBaseEntity>();
            filterAggregationDescriptor.Filter(f =>
            {
                return _queryContainerDescriptor.GetCommonQueryContainerForSearchPage(elasticInputs, elasticInputs.carsWithPhotos, f);
            })
            .Aggregations(a => a
                            .Terms("BodyIdCount", t => t
                                .Size(_countAggsCount)
                                .Field("bodyStyle")
                            )
                        );
            elasticInputs.bodytypes = bodyTypes;
            return filterAggregationDescriptor;
        }

        public FilterAggregationDescriptor<StockBaseEntity> GetColorCountDescriptor(ElasticOuptputs elasticInputs)
        {
            var colors = elasticInputs.colors;
            elasticInputs.colors = null;
            FilterAggregationDescriptor<StockBaseEntity> filterAggregationDescriptor = new FilterAggregationDescriptor<StockBaseEntity>();
            filterAggregationDescriptor.Filter(f =>
            {
                return _queryContainerDescriptor.GetCommonQueryContainerForSearchPage(elasticInputs, elasticInputs.carsWithPhotos, f);
            })
            .Aggregations(a => a
                            .Terms("ColorIdCount", t => t
                                .Size(_countAggsCount)
                                .Field("usedCarMasterColorsId")
                            )
                        );
            elasticInputs.colors = colors;
            return filterAggregationDescriptor;
        }

        public FilterAggregationDescriptor<StockBaseEntity> GetTransmissionCountDescriptor(ElasticOuptputs elasticInputs)
        {
            var transmissions = elasticInputs.transmissions;
            elasticInputs.transmissions = null;
            FilterAggregationDescriptor<StockBaseEntity> filterAggregationDescriptor = new FilterAggregationDescriptor<StockBaseEntity>();
            filterAggregationDescriptor.Filter(f =>
            {
                return _queryContainerDescriptor.GetCommonQueryContainerForSearchPage(elasticInputs, elasticInputs.carsWithPhotos, f);
            })
            .Aggregations(a => a
                            .Terms("TransmissionIdCount", t => t
                                .Size(_countAggsCount)
                                .Field("transmission")
                            )
                        );
            elasticInputs.transmissions = transmissions;
            return filterAggregationDescriptor;
        }

        public FilterAggregationDescriptor<StockBaseEntity> GetOwnersCountDescriptor(ElasticOuptputs elasticInputs)
        {
            var owners = elasticInputs.owners;
            elasticInputs.owners = null;
            FilterAggregationDescriptor<StockBaseEntity> filterAggregationDescriptor = new FilterAggregationDescriptor<StockBaseEntity>();
            filterAggregationDescriptor.Filter(f =>
            {
                return _queryContainerDescriptor.GetCommonQueryContainerForSearchPage(elasticInputs, elasticInputs.carsWithPhotos, f);
            })
            .Aggregations(a => a
                            .Terms("OwnerTypeCount", t => t
                                .Size(_countAggsCount)
                                .Field("owners")
                            )
                        );
            elasticInputs.owners = owners;
            return filterAggregationDescriptor;
        }

        public FilterAggregationDescriptor<StockBaseEntity> GetCarsWithPhotoCountDescriptor(ElasticOuptputs elasticInputs)
        {
            var carsWithPhoto = elasticInputs.carsWithPhotos;
            elasticInputs.carsWithPhotos = "1";// we want all the stock with atleast 1 photo
            FilterAggregationDescriptor<StockBaseEntity> filterAggregationDescriptor = new FilterAggregationDescriptor<StockBaseEntity>();
            filterAggregationDescriptor.Filter(f =>
            {
                return _queryContainerDescriptor.GetCommonQueryContainerForSearchPage(elasticInputs, elasticInputs.carsWithPhotos, f);
            })
            .Aggregations(a => a
                            .Terms("CarsWithPhotosCount", t => t
                                .Size(_countAggsCount)
                                .Field("carWithPhoto")
                            )
                        );
            elasticInputs.carsWithPhotos = carsWithPhoto;
            return filterAggregationDescriptor;
        }

        public FilterAggregationDescriptor<StockBaseEntity> GetMumbaiAroundCitiesCountDescriptor(ElasticOuptputs elasticInputs)
        {
            var cities = elasticInputs.cities;
            elasticInputs.cities = _mumbaiAndAroundCityIds;// only mumbai and around
            FilterAggregationDescriptor<StockBaseEntity> filterAggregationDescriptor = new FilterAggregationDescriptor<StockBaseEntity>();
            filterAggregationDescriptor.Filter(f =>
            {
                return _queryContainerDescriptor.GetCommonQueryContainerForSearchPage(elasticInputs, elasticInputs.carsWithPhotos, f);
            });
            elasticInputs.cities = cities;
            return filterAggregationDescriptor;
        }

        public FilterAggregationDescriptor<StockBaseEntity> GetDelhiAroundCitiesCountDescriptor(ElasticOuptputs elasticInputs)
        {
            var cities = elasticInputs.cities;
            elasticInputs.cities = _delhiNcrCityIds;// only delhi and around
            FilterAggregationDescriptor<StockBaseEntity> filterAggregationDescriptor = new FilterAggregationDescriptor<StockBaseEntity>();
            filterAggregationDescriptor.Filter(f =>
            {
                return _queryContainerDescriptor.GetCommonQueryContainerForSearchPage(elasticInputs, elasticInputs.carsWithPhotos, f);
            });
            elasticInputs.cities = cities;
            return filterAggregationDescriptor;
        }


        public FilterAggregationDescriptor<StockBaseEntity> GetCarTradeCertificationCountDescriptor(ElasticOuptputs elasticInputs)
        {
            var isCartradeCertified = elasticInputs.IsCarTradeCertifiedCars;
            elasticInputs.IsCarTradeCertifiedCars = true;// we want all certified cars
            FilterAggregationDescriptor<StockBaseEntity> filterAggregationDescriptor = new FilterAggregationDescriptor<StockBaseEntity>();
            filterAggregationDescriptor.Filter(f =>
            {
                return _queryContainerDescriptor.GetCommonQueryContainerForSearchPage(elasticInputs, elasticInputs.carsWithPhotos, f);
            })
            .Aggregations(a => a
                            .Terms("CarTradeCertifiedCarsCount", t => t
                                .Size(_countAggsCount)
                                .Field("certificationId")
                            )
                        );
            elasticInputs.IsCarTradeCertifiedCars = isCartradeCertified;
            return filterAggregationDescriptor;
        }

        public FilterAggregationDescriptor<StockBaseEntity> GetFranchiseCarsCountDescriptor(ElasticOuptputs elasticInputs)
        {
            bool IsFranchiseCarsAvailable = elasticInputs.IsFranchiseCars;
            elasticInputs.IsFranchiseCars = true;
            FilterAggregationDescriptor<StockBaseEntity> filterAggregationDescriptor = new FilterAggregationDescriptor<StockBaseEntity>();
            filterAggregationDescriptor.Filter(f =>
            {
                return _queryContainerDescriptor.GetCommonQueryContainerForSearchPage(elasticInputs, elasticInputs.carsWithPhotos, f);
            } );
            elasticInputs.IsFranchiseCars = IsFranchiseCarsAvailable;
            return filterAggregationDescriptor;
        }

        /// <summary>
        /// Returns an AggregationContainerDescriptor for the first featured slot which can either be a diamond slot or a franchise slot.
        /// </summary>
        /// <returns>A  List ot type PQ</returns>
        public AggregationContainerDescriptor<StockBaseEntity> GetAggregationContainerDescriptorForFirstSlot(int randNum, int size, AggregationContainerDescriptor<StockBaseEntity> descriptor)
        {
            //firstSlotPackageId contains either diamond's or franchise's packageId(50-50), which will be shown on the first featured slot.
            CwBasePackageId firstSlotPackageId = new Random().Next(10) > 4 ? CwBasePackageId.Franchise : CwBasePackageId.Diamond;
            return descriptor.Terms("dealers", taggd => taggd
                    .Script(script => script
                        //if the stock's cwBasePackageId doesn't match the firstSlotPackageId, stock is pushed down(as ordering is set as 'ascending') by adding an offset of 3, else 0 is added as
                        //offset. Since the offset is 0 for the stocks that arent' pushed down, the value generated by the script will lie in [-1, 1],
                        //adding an offset of 3 ensures that the minimum value(generated by the script) for the pushed down stocks will always be greated than 1.
                        .Inline(@"int offset = doc['cwBasePackageId'].value == params.firstSlotPackageId ? 0 : 3 ;
                                  return (offset + Math.sin(params.randomNum*doc['dealerId'].value)) + '_' + doc['dealerId'].value;")
                        .Params(par => par.Add("randomNum", randNum)
                                          .Add("firstSlotPackageId", firstSlotPackageId))
                        .Lang("painless")
                    )
                    .OrderAscending("_term")
                    .Size(size)
                    .Aggregations(ag => ag
                        .TopHits("stocks", desc => desc.Size(size)))
                );
        }

        public AggregationContainerDescriptor<StockBaseEntity> GetAggregationContainerForFeaturedStocks(int randNum, int size, AggregationContainerDescriptor<StockBaseEntity> descriptor, double latitude, double longitude, bool shouldFetchNearbyCars)
        {
            string inlineAggScript = "(Math.sin(params.randomNum*doc['dealerId'].value) + 1)+'_'+doc['dealerId'].value";
            string nearbyInlineAggScript = "double diff = doc['location'].planeDistance(params.targetLat, params.targetLong) ; return (diff/1000 <= params.bucketRange ? 1 : 2)";

            return shouldFetchNearbyCars ?
                descriptor.Terms("dealers", t => t
                    .Script(script => script
                       .Inline($"{nearbyInlineAggScript}+'_'+{inlineAggScript}")
                       .Params(par => par
                            .Add("randomNum", randNum)
                            .Add("targetLat", latitude)
                            .Add("targetLong", longitude)
                            .Add("bucketRange", Constants.CarsNearMeBucketRange)
                        )
                       .Lang("painless"))
                    .OrderAscending("_term")
                    .Size(size)
                    .Aggregations(ag => ag
                       .TopHits("stocks", t11 => t11
                          .Size(_maxStocksPerDealer)
                       )

                    )
                )
                
                : descriptor.Terms("dealers", t => t
                    .Script(script => script
                       .Inline(inlineAggScript)
                       .Params(par => par.Add("randomNum", randNum))
                       .Lang("painless"))
                    .OrderAscending("_term")
                    .Size(size)
                    .Aggregations(ag => ag
                       .TopHits("stocks", t11 => t11
                          .Size(_maxStocksPerDealer)
                       )
                    )
                );
        }
    }
}
