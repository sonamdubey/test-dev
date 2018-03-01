
using Bikewale.DAL.CoreDAL;
using Bikewale.ElasticSearch.Entities;
using Bikewale.Entities.NewBikeSearch;
using Bikewale.Interfaces.NewBikeSearch;
using Bikewale.Notifications;
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
        private static readonly string _displacement = "displacement";
        private static readonly string _exshowroom = "topVersion.exshowroom";
        private static readonly string _bikeMakeId = "bikeMake.makeId";
        private static readonly string _mileage = "mileage";
        private static readonly string _bodyStyleId = "bodyStyleId";
        private static readonly string _cityId = "city.cityId";
        private static readonly string _bikeStatus = "bikeModel.modelStatus";
        private static readonly string _topVersionStatus = "topVersion.versionStatus";
        private static readonly byte _ModelStatus = 1;// by defaut all new bikes status
        private static readonly byte _VersionStatus = 1;

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
                if (filters.CityId > 1)
                {
                    IEnumerable<BikeModelDocument> objBikeListWithCityPrice = null;

                    var bikeList = Task.Factory.StartNew(() => objBikeList.Bikes = GetBikeSearchList(filters, BikeSearchEnum.BikeList));
                    var bikeListWithCityPrice = Task.Factory.StartNew(() => objBikeListWithCityPrice = GetBikeSearchList(filters, BikeSearchEnum.PriceList));


                    Task.WaitAll(bikeList, bikeListWithCityPrice);

                    if (objBikeList.Bikes != null && objBikeList.Bikes.Any())
                    {
                        for (int index = 0; index < objBikeList.Bikes.Count(); index++)
                        {
                            objBikeList.Bikes.ElementAt(index).TopVersion = objBikeListWithCityPrice.ElementAt(index).TopVersion;
                        }
                    }
                }
                else
                {
                    objBikeList.Bikes = GetBikeSearchList(filters, BikeSearchEnum.BikeList);
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
                if (filters.Price != null && filters.Price.Any())
                {
                    query &= Range(filters.Price, _exshowroom);
                }
                if (filters.CityId > 0)
                {
                    query &= FDS.Term(_cityId, filters.CityId);
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
    }
}
