using Carwale.DAL.CoreDAL;
using Carwale.Entity.Classified;
using Carwale.Entity.Elastic;
using Carwale.Interfaces.Classified.ElasticSearch;
using Carwale.Interfaces.Elastic;
using Carwale.Utility.Classified;
using Nest;

namespace Carwale.DAL.Classified.ElasticSearch
{
    public class AggregationsRepository : IAggregationsRepository
    {
        private readonly IAggregationQueryDescriptor _aggregationQueryDescriptor;
        private readonly IProcessFilters _processFilters;
        private readonly IProcessAggregationElasticJson _processAggregationElasticJson;
        private static readonly ElasticClient _elasticClient = ElasticClientInstance.GetInstance();

        public AggregationsRepository(
            IAggregationQueryDescriptor aggregationQueryDescriptor, 
            IProcessFilters processFilters,
            IProcessAggregationElasticJson processAggregationElasticJson
            )
        {
            _aggregationQueryDescriptor = aggregationQueryDescriptor;
            _processFilters = processFilters;
            _processAggregationElasticJson = processAggregationElasticJson;
        }

        public SellerType GetSellerTypeCount(FilterInputs filterInputs)
        {
            ElasticOuptputs elasticOutputs= _processFilters.ProcessFilterParams(filterInputs);
            var searchResponse = _elasticClient.Search<StockBaseEntity>(s => s
                     .Type(Constants.ClassifiedElasticIndexType)
                     .Index(Constants.ClassifiedElasticIndex)
                     .Size(0)
                     .Aggregations(agg => agg.Filter("SellerCount", filter =>
                                    _aggregationQueryDescriptor.GetSellerTypeCountDescriptor(elasticOutputs)))
                    );
            return _processAggregationElasticJson.GetSellerTypeCount(searchResponse);
        }

        public CountData GetAllFilterCount(ElasticOuptputs elasticInputs)
        {
            var searchResponse = _elasticClient.Search<StockBaseEntity>(s => s
                .Type(Constants.ClassifiedElasticIndexType)
                .Index(Constants.ClassifiedElasticIndex)
                .Take(0)
                .Aggregations(agg => agg
                    .Filter("TotalStockCount", fil =>
                        _aggregationQueryDescriptor.GetTotalStockCountDescriptor(elasticInputs)
                    )
                    .Filter("CityCount", fil =>
                        _aggregationQueryDescriptor.GetCityCountDescriptor(elasticInputs)
                    )
                    .Filter("FuelTypeCount", fil =>
                        _aggregationQueryDescriptor.GetFuelTypeCountDescriptor(elasticInputs)
                    )
                    .Filter("MakeCount", fil =>
                        _aggregationQueryDescriptor.GetMakeAndItsRootCountDescriptor(elasticInputs)
                    )

                    .Filter("BodyTypeCount", fil =>
                        _aggregationQueryDescriptor.GetBodyTypeCountDescriptor(elasticInputs)
                    )

                    .Filter("ColorCount", fil =>
                        _aggregationQueryDescriptor.GetColorCountDescriptor(elasticInputs)
                    )

                    .Filter("TransmissionCount", fil =>
                        _aggregationQueryDescriptor.GetTransmissionCountDescriptor(elasticInputs)
                    )

                    //Not adding seller count descriptor

                    .Filter("OwnersCount", fil =>
                        _aggregationQueryDescriptor.GetOwnersCountDescriptor(elasticInputs)
                    )

                    //Cars with photo
                    .Filter("FilterBy2", fil =>
                        _aggregationQueryDescriptor.GetCarsWithPhotoCountDescriptor(elasticInputs)
                    )

                    //Cars with certification
                    .Filter("FilterBy1", fil =>
                        _aggregationQueryDescriptor.GetCarTradeCertificationCountDescriptor(elasticInputs)
                    )

                    //Franchies Cars Count Starts
                    .Filter("FilterBy3", i =>
                        _aggregationQueryDescriptor.GetFranchiseCarsCountDescriptor(elasticInputs)
                    )

                    .Filter("MumbaiAroundCities", fil =>
                        _aggregationQueryDescriptor.GetMumbaiAroundCitiesCountDescriptor(elasticInputs)
                    )

                    .Filter("DelhiAroundCities", fil =>
                        _aggregationQueryDescriptor.GetDelhiAroundCitiesCountDescriptor(elasticInputs)
                    )
                )
            );

            var filtersCount = _processAggregationElasticJson.GetAllFilterCount(searchResponse);

            return filtersCount;
        }
    }
}
