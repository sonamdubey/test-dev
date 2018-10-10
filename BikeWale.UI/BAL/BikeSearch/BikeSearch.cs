using AutoMapper;
using Bikewale.DAL.CoreDAL;
using Bikewale.ElasticSearch.Entities;
using Bikewale.Entities.NewBikeSearch;
using Bikewale.Interfaces.NewBikeSearch;
using Bikewale.Notifications;
using Bikewale.Utility;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.BAL.BikeSearch
{
    /// <summary>
    /// Created By : Subodh Jain on 21 feb 2018
    /// Description: To get the search result according to filter passed, source and noofRecords.
    /// Modified By: Deepak Israni on 5 March 2018
    /// Description: Added GetDocuments function and modified GetBikeSearch to get price according to the city using the bikewalepricingindex.
    /// Modified by : SnehaL Dange on 16th April 2018
    /// Description: Added 
    /// Modified By : Prabhu Puredla on 28 sept 2018
    /// Descritption : Fetching prices from elastic index
    /// </summary>
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
        private static readonly string _abs = "topVersion.abs";
        private static readonly string _brakes = "topVersion.rearBrakeType";
        private static readonly string _wheels = "topVersion.wheels";
        private static readonly string _startType = "topVersion.startType";
        private static readonly string _popular = "weight";

        private static readonly byte _modelStatus = 1;// by defaut all new bikes status
        private static readonly byte _versionStatus = 1;

        private static readonly string _modelId = "bikeModel.modelId";


        public BikeSearch()
        {
            _client = ElasticSearchInstance.GetInstance();
        }

        /// <summary>
        /// Created By : Subodh Jain on 21 feb 2018
        /// Description: GetBike search result according to filter passed,source and noofRecords
        /// Modified By: Deepak Israni on 5 March 2018
        /// Description: Fetched pricing of city from bikewalepricingindex and changed the flow of the function.
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
                if (filters != null)
                {
                    long totalResults;
                    IEnumerable<BikeModelDocument> bikeList = GetBikeSearchList(filters, BikeSearchEnum.BikeList, out totalResults);

                    if (bikeList != null && bikeList.Any())
                    {
                        objBikeList.Bikes = Convert(bikeList);
                        if (filters.CityId > 0)
                        {
                            IEnumerable<ModelPriceDocument> bikeListWithCityPrice = null;
                            IDictionary<uint, ModelPriceDocument> bikePrices = null;


                            IEnumerable<String> documentIds = bikeList.Select(bike => string.Format("{0}_{1}", bike.BikeModel.ModelId, filters.CityId));

                            bikeListWithCityPrice = GetDocuments<ModelPriceDocument>(BWConfiguration.Instance.BikeModelPriceIndex, documentIds);
                            bikePrices = bikeListWithCityPrice.ToDictionary(priceDocument => priceDocument.BikeModel.ModelId, priceDocument => priceDocument);

                            if (bikePrices != null)
                            {
                                var bikeCount = objBikeList.Bikes.Count();
                                for (int index = 0; index < bikeCount; index++)
                                {
                                    var bike = objBikeList.Bikes.ElementAt(index);
                                    var modelId = bike.BikeModel.ModelId;

                                    Bikewale.ElasticSearch.Entities.VersionEntity topVersion = null;

                                    if (bikePrices.ContainsKey(modelId))
                                    {
                                        topVersion = bikePrices[modelId].VersionPrice.FirstOrDefault(version => version.VersionId == bike.TopVersion.VersionId);
                                    }

                                    if (bike.TopVersion != null && topVersion != null)
                                    {
                                        bike.TopVersion.PriceList = ConvertPrice(topVersion.PriceList);
                                        bike.TopVersion.Exshowroom = topVersion.Exshowroom;
                                        bike.TopVersion.Onroad = topVersion.Onroad;
                                        bike.HasCityPrice = true;
                                        bike.CityName = bikePrices[modelId].City.CityName;
                                    }
                                }
                            }
                        }
                        objBikeList.TotalCount = (int)totalResults;
                        if (filters.ExcludeMake)
                        {
                            SetPrevNextFilters(filters, objBikeList);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.BAL.BikeSearch.GetBikeSearch");
            }
            return objBikeList;
        }

        /// <summary>
        /// Created By : Subodh Jain on 21 feb 2018
        /// Description: To handle the pagination of results.
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="objBikeList"></param>
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
        private IEnumerable<BikeModelDocument> GetBikeSearchList(SearchFilters filters, BikeSearchEnum source, out long totalResults)
        {
            totalResults = 0;
            IEnumerable<BikeModelDocument> bikeModelDocs = null;
            try
            {

                string indexName = GetIndexName(source);
                string typeName = GetTypeName(source);

                if (_client != null)
                {
                    Func<SearchDescriptor<BikeModelDocument>, SearchDescriptor<BikeModelDocument>> searchDescriptor = BuildSearchDescriptor<BikeModelDocument>(filters, indexName, typeName);

                    if (searchDescriptor != null)
                    {
                        ISearchResponse<BikeModelDocument> _result = _client.Search(searchDescriptor);
                        totalResults = _result.Total;
                        if (_result != null && _result.Hits != null && _result.Hits.Count > 0)
                        {
                            bikeModelDocs = _result.Documents.ToList();

                        }

                    }

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Exception : Bikewale.BAL.BikeSearch.GetBikeSearchList "));
            }

            return bikeModelDocs;
        }

        /// <summary>
        /// Created By : Sumit Kate on 6 March 2018
        /// Description: Func descriptor for searching data in ES index.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filters"></param>
        /// <param name="indexName"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        private Func<SearchDescriptor<T>, SearchDescriptor<T>> BuildSearchDescriptor<T>(SearchFilters filters, string indexName, string typeName) where T : class
        {
            Func<SearchDescriptor<T>, SearchDescriptor<T>> searchDescriptor = null;
            try
            {


                if (filters.PageNumber > 0 && filters.PageSize > 0)
                {
                    searchDescriptor = new Func<SearchDescriptor<T>, SearchDescriptor<T>>(
                            sd => sd.Index(indexName).Type(typeName)
                                     .Query(q => q
                                     .Bool(bq => bq
                                      .Filter(ff => ff
                                          .Bool(bb => bb.
                                              Must(ProcessFilters(filters)))))).From((filters.PageNumber - 1) * filters.PageSize).Sort(GetSortDescriptor<T>(filters.SortCriteria, filters.SortOrder)).Take(filters.PageSize));

                }
                else
                {
                    searchDescriptor = new Func<SearchDescriptor<T>, SearchDescriptor<T>>(
                        sd => sd.Index(indexName).Type(typeName)
                           .Query(q => q
                           .Bool(bq => bq
                            .Filter(ff => ff
                                .Bool(bb => bb.
                                    Must(ProcessFilters(filters)))))).Sort(GetSortDescriptor<T>(filters.SortCriteria, filters.SortOrder)).Size(40));
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Exception : Bikewale.BAL.BikeSearch.BuildSearchDescriptor "));
            }

            return searchDescriptor;
        }

        /// <summary>
        /// Created by : Snehal Dange on 13TH April 2018
        /// Desc: Method created to get sort query for ES
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sortCriteria"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        private static Func<SortDescriptor<T>, IPromise<IList<ISort>>> GetSortDescriptor<T>(string sortCriteria, string sortOrder) where T : class
        {
            Func<SortDescriptor<T>, IPromise<IList<ISort>>> returnVal = null;
            try
            {
                switch (sortCriteria)
                {
                    case "1":
                        Field exshowroomField = _exshowroom;
                        returnVal = a => (sortOrder == "1" ? a.Descending(exshowroomField) : a.Ascending(exshowroomField));
                        break;

                    case "2":
                        Field mileageField = _mileage;
                        returnVal = a => ((String.IsNullOrEmpty(sortOrder) || sortOrder == "0") ? a.Descending(mileageField) : a.Ascending(mileageField));
                        break;

                    default:
                        Field popularField = _popular;
                        returnVal = a => a.Descending(popularField);
                        break;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Exception : Bikewale.BAL.BikeSearch.GetSortDescriptor"));
            }
            return returnVal;
        }



        /// <summary>
        /// Created By :- Subodh Jain on 21 feb 2018
        /// Summary :- Process Filters according to req.
        /// Modified by: Dhruv Joshi
        /// Dated: 8th March 2018
        /// Description: Query for body style changed to handle string Ienumerable
        /// Modified by : Snehal Dange on 13th April 2018
        /// Desc: Added filters for makes, abs , wheels, starttype and brakes
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
                query &= FDS.Term(_bikeStatus, _modelStatus);
                query &= FDS.Term(_topVersionStatus, _versionStatus);

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
                if (filters.BodyStyle != null && filters.BodyStyle.Any())
                {
                    QueryContainer qtmp = new QueryContainer();
                    foreach (string style in filters.BodyStyle)
                    {
                        if (!String.IsNullOrEmpty(style))
                        {
                            qtmp |= FDS.Term(_bodyStyleId, SqlReaderConvertor.ToUInt32(style));
                        }
                    }
                    query &= qtmp;
                }
                if (filters.Make != null && filters.Make.Any())
                {
                    QueryContainer queryTmp = new QueryContainer();
                    foreach (uint makeId in filters.Make)
                    {
                        if (makeId > 0)
                        {
                            queryTmp |= FDS.Term(_bikeMakeId, SqlReaderConvertor.ToUInt32(makeId));
                        }
                    }
                    query &= queryTmp;
                }

                if (filters.ABS != null)
                {
                    query &= FDS.Term(_abs, filters.ABS);
                }
                if (filters.Wheels != null && filters.Wheels.Any())
                {
                    QueryContainer qtmp = new QueryContainer();
                    foreach (uint wheelType in filters.Wheels)
                    {
                        qtmp |= FDS.Term(_wheels, wheelType);
                    }
                    query &= qtmp;
                }
                if (filters.StartType != null && filters.StartType.Any())
                {
                    QueryContainer qtmp = new QueryContainer();
                    foreach (uint startType in filters.StartType)
                    {
                        qtmp |= FDS.Term(_startType, startType);
                    }
                    query &= qtmp;
                }

                if (filters.Brakes != null && filters.Brakes.Any())
                {
                    QueryContainer qtmp = new QueryContainer();
                    foreach (uint brakeType in filters.Brakes)
                    {
                        qtmp |= FDS.Term(_brakes, brakeType);
                    }
                    query &= qtmp;
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

        /// <summary>
        /// Created By : Deepak Israni on 6 March 2018
        /// Description: AutoMapper to map BikeModelDocument(ES) to BikeModelDocumentEntity(NewBikeSearch)
        /// </summary>
        /// <param name="objDocuments"></param>
        /// <returns></returns>
        private IEnumerable<BikeModelDocumentEntity> Convert(IEnumerable<BikeModelDocument> objDocuments)
        {
            Mapper.CreateMap<BikeModelDocument, BikeModelDocumentEntity>();
            Mapper.CreateMap<Bikewale.ElasticSearch.Entities.MakeEntity, Bikewale.Entities.NewBikeSearch.MakeEntity>();
            Mapper.CreateMap<Bikewale.ElasticSearch.Entities.ModelEntity, Bikewale.Entities.NewBikeSearch.ModelEntity>();
            Mapper.CreateMap<Bikewale.ElasticSearch.Entities.VersionEntity, Bikewale.Entities.NewBikeSearch.VersionEntity>();
            Mapper.CreateMap<Bikewale.ElasticSearch.Entities.ImageEntity, Bikewale.Entities.NewBikeSearch.ImageEntity>();
            Mapper.CreateMap<Bikewale.ElasticSearch.Entities.PriceEntity, Bikewale.Entities.NewBikeSearch.PriceEntity>();

            return Mapper.Map<IEnumerable<BikeModelDocument>, IEnumerable<BikeModelDocumentEntity>>(objDocuments);
        }

        /// <summary>
        /// Created By : Deepak Israni on 6 March 2018
        /// Description: AutoMapper to map PriceEntity(ES) to PriceEntity(NewBikeSearch)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private IEnumerable<Bikewale.Entities.NewBikeSearch.PriceEntity> ConvertPrice(IEnumerable<Bikewale.ElasticSearch.Entities.PriceEntity> obj)
        {
            Mapper.CreateMap<Bikewale.ElasticSearch.Entities.PriceEntity, Bikewale.Entities.NewBikeSearch.PriceEntity>();

            return Mapper.Map<IEnumerable<Bikewale.ElasticSearch.Entities.PriceEntity>, IEnumerable<Bikewale.Entities.NewBikeSearch.PriceEntity>>(obj);
        }

        /// <summary>
        /// Created By   : Prabhu Puredla on 28 sept 2018
        /// Descritption : Fetching prices from elastic index
        /// </summary>
        /// <param name="modelIds"></param>
        /// <param name="cityId"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public IEnumerable<BikeTopVersion> GetBikePriceSearchList(IEnumerable<int> modelIds, uint cityId, BikeSearchEnum source)
        {
           
            try
            {
                string indexName = GetIndexName(source);
                string typeName = GetTypeName(source);
                cityId = cityId == 0 ? uint.Parse(BWConfiguration.Instance.DefaultCity) : cityId;
                if (_client != null && modelIds != null)
                {
                    Func<SearchDescriptor<BikeTopVersion>, SearchDescriptor<BikeTopVersion>> searchDescriptor = BuildPriceSearchDescriptor<BikeTopVersion>(modelIds, cityId, indexName, typeName);

                    if (searchDescriptor != null)
                    {
                        ISearchResponse<BikeTopVersion> _result = _client.Search(searchDescriptor);
                        if (_result != null)
                        {
                            return _result.Documents;  
                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Exception : Bikewale.BAL.BikeSearch.GetBikePriceSearchList"));
            }
            return null;
        }

        /// <summary>
        /// Created By : Prabhu Puredla on 28 sept 2018
        /// Descritption : Build search descriptor to fetch Price documents in ES
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="modelIds"></param>
        /// <param name="cityId"></param>
        /// <param name="indexName"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        private Func<SearchDescriptor<T>, SearchDescriptor<T>> BuildPriceSearchDescriptor<T>(IEnumerable<int> modelIds, uint cityId, string indexName, string typeName) where T : class
        {
            Func<SearchDescriptor<T>, SearchDescriptor<T>> searchDescriptor = null;
            if (modelIds != null)
            {
                searchDescriptor = new Func<SearchDescriptor<T>, SearchDescriptor<T>>(
                        sd => sd.Index(indexName).Type(typeName)
                                    .Query(q => q
                                    .Bool(bq => bq
                                    .Filter(ff => ff
                                        .Bool(bb => bb.
                                            Must(ProcessBikes(modelIds, cityId)))))));

            }
            return searchDescriptor;
        }

        /// <summary>
        /// Created By : Prabhu Puredla on 28 sept 2018
        /// Descritption : Creates the query with given data
        /// </summary>
        /// <param name="modelIds"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        private QueryContainer ProcessBikes(IEnumerable<int> modelIds, uint cityId)
        {
            QueryContainer query = new QueryContainer();
            QueryContainerDescriptor<BikeTopVersion> FDS = new QueryContainerDescriptor<BikeTopVersion>();

            if (modelIds.Any())
            {
                QueryContainer qtmp = new QueryContainer();
                foreach (var modelId in modelIds)
                {
                    qtmp |= FDS.Term(_modelId, modelId) & FDS.Term(_cityId, cityId);
                }
                query &= qtmp;
            }
            return query;
        }
    }
}