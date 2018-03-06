
using Bikewale.DAL.CoreDAL;
using Bikewale.ElasticSearch.Entities;
using Bikewale.Entities.NewBikeSearch;
using Bikewale.Interfaces.NewBikeSearch;
using Bikewale.Notifications;
using Bikewale.Utility;
using Bikewale.Utility.LinqHelpers;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Bikewale.BAL.BikeSearch
{
    public class BikeSearch : IBikeSearch
    {
        private readonly ElasticClient _client;

        private static readonly string _displacement = "topVersion.displacement";
        private static readonly string _exshowroom = "topVersion.exshowroom";
        private static readonly string _bikeMakeId = "bikeMake.makeId";
        private static readonly string _mileage = "topVersion.mileage";
        private static readonly string _power = "topVersion.power";
        private static readonly string _bodyStyleId = "bodyStyleId";
        private static readonly string _cityId = "city.cityId";
        private static readonly string _bikeStatus = "bikeModel.modelStatus";
        private static readonly string _topVersionStatus = "topVersion.versionStatus";
        private static readonly byte _ModelStatus = 1;// by defaut all new bikes status
        private static readonly byte _VersionStatus = 1;

        public BikeSearch()
        {
            _client = ElasticSearchInstance.GetInstance();
        }

        /// <summary>
        /// Created By :-Subodh Jain on 21 feb 2018
        /// Description :- GetBike search result according to filter passed,source and noofRecords
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filters"></param>
        /// <param name="source"></param>
        /// <param name="noOfRecords"></param>
        /// <returns></returns>
        public BikeSearchOutputEntity GetBikeSearch(SearchFilters filters)
        {
            BikeSearchOutputEntity objBikeList = new BikeSearchOutputEntity();

            try
            {
                IEnumerable<BikeModelDocument> bikeList = GetBikeSearchList(filters, BikeSearchEnum.BikeList);

                if (filters.CityId > 0)
                {
                    IEnumerable<ModelPriceDocument> bikeListWithCityPrice = null;
                    IDictionary<uint, ModelPriceDocument> bikePrices = null;

                    if (bikeList != null && bikeList.Count() > 0)
                    {
                        objBikeList.Bikes = bikeList;
                        IEnumerable<String> documentIds = bikeList.Select(bike => string.Format("{0}_{1}", bike.BikeModel.ModelId, filters.CityId));

                        bikeListWithCityPrice = GetDocuments<ModelPriceDocument>(BWConfiguration.Instance.BikeModelPriceIndex, documentIds);
                        bikePrices = bikeListWithCityPrice.ToDictionary(x => x.BikeModel.ModelId, x => x);

                    }

                    if (bikePrices != null && objBikeList.Bikes.Any())
                    {
                        var bikeCount = objBikeList.Bikes.Count();
                        for (int index = 0; index < bikeCount; index++)
                        {
                            var element = objBikeList.Bikes.ElementAt(index);
                            var modelId = element.BikeModel.ModelId;
                            if (element.TopVersion != null)
                            {
                                VersionEntity topVersion = bikePrices[modelId].VersionPrice.Where(version => version.VersionId == element.TopVersion.VersionId).First();
                                element.TopVersion.PriceList = topVersion.PriceList;
                                element.TopVersion.Exshowroom = topVersion.Exshowroom;
                                element.TopVersion.Onroad = topVersion.Onroad;
                            }
                        }
                    }
                }

                SetPrevNextFilters(filters, objBikeList);

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.BAL.BikeSearch.GetBikeSearch");
            }
            return objBikeList;
        }

        private void SetPrevNextFilters(SearchFilters filters, BikeSearchOutputEntity objBikeList)
        {

            try
            {
                objBikeList.NextFilters = filters;

                uint _totalPageSize = (uint)(objBikeList.TotalCount / filters.PageSize);
                if (objBikeList.NextFilters != null && _totalPageSize > objBikeList.CurrentPageNumber)
                {
                    objBikeList.NextFilters.PageNumber++;
                }
                objBikeList.PrevFilters = filters;
                if (objBikeList.PrevFilters != null && objBikeList.CurrentPageNumber != 1)
                {
                    objBikeList.PrevFilters.PageNumber--;
                }
            }
            catch (Exception ex)
            {

                ErrorClass.LogError(ex, "Exception : Bikewale.BAL.BikeSearch.SetPrevNextFilters");
            }
        }

        /// <summary>
        /// Created By :-Subodh Jain on 21 feb 2018
        /// Summary :- List of bikes accroding to search parameter and process filters
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filters"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        private IEnumerable<BikeModelDocument> GetBikeSearchList(SearchFilters filters, BikeSearchEnum source)
        {
            IEnumerable<BikeModelDocument> suggestionList = null;
            try
            {
                string indexName = GetIndexName(source);
                string typeName = GetTypeName(source);
                ElasticClient client = ElasticSearchInstance.GetInstance();

                if (client != null)
                {
                    // func descriptor for searching data in ES index
                    Func<SearchDescriptor<BikeModelDocument>, SearchDescriptor<BikeModelDocument>> searchDescriptor = new Func<SearchDescriptor<BikeModelDocument>, SearchDescriptor<BikeModelDocument>>(
                           sd => sd.Index(indexName).Type(typeName)
                                    .Query(q => q
                                    .Bool(bq => bq
                                     .Filter(ff => ff
                                         .Bool(bb => bb.
                                             Must(ProcessFilters(filters)))))));
                    ISearchResponse<BikeModelDocument> _result = client.Search(searchDescriptor);

                    if (_result != null && _result.Hits != null && _result.Hits.Count > 0)
                    {
                        suggestionList = _result.Documents.ToList();
                    }

                    if (suggestionList != null && suggestionList.Any() && filters.PageNumber > 0 && filters.PageSize > 0)
                    {
                        suggestionList = suggestionList.Page(filters.PageNumber, filters.PageSize);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Exception : Bikewale.BAL.BikeSearch.GetBikeSearchList "));
            }
            return suggestionList;
        }

        /// <summary>
        /// Created By :- Subodh Jain on 21 feb 2018
        /// Summary :- Process Filters according to req.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filters"></param>
        /// <returns></returns>
        private QueryContainer ProcessFilters(SearchFilters filters)
        {
            QueryContainer query = new QueryContainer();
            QueryContainerDescriptor<BikeModelDocument> FDS = new QueryContainerDescriptor<BikeModelDocument>();
            try
            {
                query &= FDS.Term(_bikeStatus, _ModelStatus);
                query &= FDS.Term(_topVersionStatus, _VersionStatus);

                if (filters.Displacement != null && filters.Displacement.Any())
                {
                    query &= Range(filters.Displacement, _displacement);
                }
                if (filters.Mileage != null && filters.Mileage.Any())
                {
                    query &= Range(filters.Mileage, _mileage);
                }
                if (filters.Power != null && filters.Power.Any())
                {
                    query &= Range(filters.Power, _power);
                }
                if (filters.Price != null && filters.Price.Any())
                {
                    query &= Range(filters.Price, _exshowroom);
                }
                if (filters.MakeId > 0)
                {
                    if (filters.ExcludeMake)
                    {
                        query &= !FDS.Term(_bikeMakeId, filters.MakeId);
                    }
                    else
                    {
                        query &= FDS.Term(_bikeMakeId, filters.MakeId);
                    }
                }
                if (filters.BodyStyle > 0)
                {
                    query &= FDS.Term(_bodyStyleId, filters.BodyStyle);
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Exception : Bikewale.BAL.BikeSearch.ProcessFilters "));
            }
            return query;
        }


        /// <summary>
        /// Expression for min max and filed 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        private Func<NumericRangeQueryDescriptor<BikeModelDocument>, INumericRangeQuery> RangeQuery(double min, double max, string fieldName)
        {
            if (max == 0)
            {
                return v => v.Field(new Field(fieldName)).GreaterThanOrEquals(min);
            }
            return v => v.Field(new Field(fieldName)).GreaterThanOrEquals(min).LessThanOrEquals(max);
        }

        /// <summary>
        /// Ranging the list of data type double from min to max
        /// </summary>
        /// <param name="List"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        private QueryContainer Range(IEnumerable<RangeEntity> List, string fieldName)
        {
            QueryContainer query = new QueryContainer();
            QueryContainerDescriptor<BikeModelDocument> FDS = new QueryContainerDescriptor<BikeModelDocument>();
            try
            {
                foreach (var obj in List)
                {
                    query |= FDS.Range(RangeQuery(obj.Min, obj.Max, fieldName));
                }
            }
            catch (Exception ex)
            {

                ErrorClass.LogError(ex, string.Format("Exception : Bikewale.BAL.BikeSearch.Range {double} "));
            }

            return query;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        private Func<NumericRangeQueryDescriptor<BikeModelDocument>, INumericRangeQuery> RangeQuery(int min, int max, string fieldName)
        {

            if (max == 0)
            {
                return v => v.Field(new Field(fieldName)).GreaterThanOrEquals(min);
            }
            return v => v.Field(new Field(fieldName)).GreaterThanOrEquals(min).LessThanOrEquals(max);
        }

        /// <summary>
        ///  Ranging the list of data type int from min to max
        /// </summary>
        /// <param name="List"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        private QueryContainer Range(IEnumerable<PriceRangeEntity> List, string fieldName)
        {
            QueryContainer query = new QueryContainer();
            QueryContainerDescriptor<BikeModelDocument> FDS = new QueryContainerDescriptor<BikeModelDocument>();
            try
            {
                foreach (var obj in List)
                {
                    query |= FDS.Range(RangeQuery(obj.Min, obj.Max, fieldName));
                }
            }
            catch (Exception ex)
            {

                ErrorClass.LogError(ex, string.Format("Exception : Bikewale.BAL.BikeSearch.Range {int} "));
            }

            return query;
        }
        /// <summary>
        /// Get index name
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private string GetIndexName(BikeSearchEnum source)
        {
            string indexName = string.Empty;
            try
            {
                switch (source)
                {

                    case BikeSearchEnum.BikeList:
                        indexName = "bikeindex";
                        break;
                    case BikeSearchEnum.PriceList:
                        indexName = "bikewalepricingindex";
                        break;
                    default:
                        indexName = "";
                        break;
                }
            }
            catch (Exception ex)
            {

                ErrorClass.LogError(ex, string.Format("Exception : Bikewale.BAL.BikeSearch.GetIndexName "));
            }
            return indexName;
        }
        /// <summary>
        /// get type of the document
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private string GetTypeName(BikeSearchEnum source)
        {
            string typeName = string.Empty;
            try
            {
                switch (source)
                {

                    case BikeSearchEnum.BikeList:
                        typeName = "bikemodeldocument";
                        break;
                    case BikeSearchEnum.PriceList:
                        typeName = "modelpricedocument";
                        break;
                    default:
                        typeName = "";
                        break;
                }
            }
            catch (Exception ex)
            {

                ErrorClass.LogError(ex, string.Format("Exception : Bikewale.BAL.BikeSearch.GetTypeName "));
            }
            return typeName;
        }

        /// <summary>
        /// Created By : Deepak Israni on 5 March 2018
        /// Description: To get the documents from ES Index according to the list/array of IDs.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="indexName"></param>
        /// <param name="documentIds"></param>
        /// <returns></returns>
        private System.Collections.Generic.IEnumerable<T> GetDocuments<T>(string indexName, System.Collections.Generic.IEnumerable<string> documentIds) where T : class
        {
            System.Collections.Generic.IEnumerable<T> documents = null;
            try
            {
                var result = _client.MultiGet(request => request.Index(indexName).GetMany<T>(documentIds));

                if (result != null && result.IsValid)
                {
                    documents = result.SourceMany<T>(documentIds);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("GetDocuments({0})", indexName));
            }
            return documents;
        }
    }
}
