using Carwale.Entity.Classified;
using Carwale.Entity.Elastic;
using Nest;

namespace Carwale.Interfaces.Classified.ElasticSearch
{
    public interface IAggregationQueryDescriptor
    {
        FilterAggregationDescriptor<StockBaseEntity> GetTotalStockCountDescriptor(ElasticOuptputs elasticInputs);
        FilterAggregationDescriptor<StockBaseEntity> GetSellerTypeCountDescriptor(ElasticOuptputs elasticInputs);
        FilterAggregationDescriptor<StockBaseEntity> GetCityCountDescriptor(ElasticOuptputs elasticInputs);
        FilterAggregationDescriptor<StockBaseEntity> GetFuelTypeCountDescriptor(ElasticOuptputs elasticInputs);
        FilterAggregationDescriptor<StockBaseEntity> GetMakeAndItsRootCountDescriptor(ElasticOuptputs elasticInputs);
        FilterAggregationDescriptor<StockBaseEntity> GetBodyTypeCountDescriptor(ElasticOuptputs elasticInputs);
        FilterAggregationDescriptor<StockBaseEntity> GetColorCountDescriptor(ElasticOuptputs elasticInputs);
        FilterAggregationDescriptor<StockBaseEntity> GetTransmissionCountDescriptor(ElasticOuptputs elasticInputs);
        FilterAggregationDescriptor<StockBaseEntity> GetOwnersCountDescriptor(ElasticOuptputs elasticInputs);
        FilterAggregationDescriptor<StockBaseEntity> GetCarsWithPhotoCountDescriptor(ElasticOuptputs elasticInputs);
        FilterAggregationDescriptor<StockBaseEntity> GetCarTradeCertificationCountDescriptor(ElasticOuptputs elasticInputs);
        FilterAggregationDescriptor<StockBaseEntity> GetMumbaiAroundCitiesCountDescriptor(ElasticOuptputs elasticInputs);
        FilterAggregationDescriptor<StockBaseEntity> GetDelhiAroundCitiesCountDescriptor(ElasticOuptputs elasticInputs);
        FilterAggregationDescriptor<StockBaseEntity> GetFranchiseCarsCountDescriptor(ElasticOuptputs elasticInputs);
        AggregationContainerDescriptor<StockBaseEntity> GetAggregationContainerDescriptorForFirstSlot(int randNum, int size, AggregationContainerDescriptor<StockBaseEntity> descriptor);
        AggregationContainerDescriptor<StockBaseEntity> GetAggregationContainerForFeaturedStocks(int randNum, int size, AggregationContainerDescriptor<StockBaseEntity> descriptor, double latitude, double longitude, bool ShouldFetchNearbyCars);
    }
}
