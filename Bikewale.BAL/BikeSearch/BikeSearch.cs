
using Bikewale.DAL.CoreDAL;
using Bikewale.ElasticSearch.Entities;
using Bikewale.Entities.NewBikeSearch;
using Bikewale.Interfaces.NewBikeSearch;
using Bikewale.Notifications;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Bikewale.BAL.BikeSearch
{
    public class BikeSearch : IBikeSearch
    {
        private static readonly string _displacement = "displacement";
        private static readonly string _exshowroom = "exshowroom";
        private static readonly string _bikeMakeId = "bikeMake.makeId";
        private static readonly string _mileage = "mileage";
        private static readonly string _bodyStyleId = "bodyStyleId";

        /// <summary>
        /// Created By :-
        /// Description :- GetBike search result according to filter passed,source and noofRecords
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filters"></param>
        /// <param name="source"></param>
        /// <param name="noOfRecords"></param>
        /// <returns></returns>
        public IEnumerable<BikeModelDocument> GetBikeSearch(SearchFilters filters, BikeSearchEnum source, int noOfRecords = 0)
        {
            IEnumerable<BikeModelDocument> suggestionList = null;

            try
            {
                suggestionList = GetBikeSearchList(filters, source);


                if (suggestionList != null && noOfRecords > 0)
                {
                    suggestionList = suggestionList.Take(noOfRecords);
                }


            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.BAL.BikeSearch.GetBikeSearch");
            }
            return suggestionList;

        }
        /// <summary>
        /// Created By :-
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
                ElasticClient client = ElasticSearchInstance.GetInstance();

                if (client != null)
                {
                    Func<SearchDescriptor<BikeModelDocument>, SearchDescriptor<BikeModelDocument>> searchDescriptor = new Func<SearchDescriptor<BikeModelDocument>, SearchDescriptor<BikeModelDocument>>(
                           sd => sd.Index(indexName).Type("bikemodeldocument")
                                    .Query(q => q
                                    .Bool(bq => bq
                                     .Filter(ff => ff
                                         .Bool(bb => bb.
                                             Must(ProcessFilters(filters)))))));
                    ISearchResponse<BikeModelDocument> _result = client.Search(searchDescriptor);

                }
            }
            catch (Exception)
            {

                throw;
            }
            return suggestionList;


        }

        /// <summary>
        /// Created By :- 
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
                if (filters.MinDisplacement > 0 || filters.MaxDisplacement > 0)
                {
                    query &= FDS.Range(RangeQuery<BikeModelDocument>(filters.MinDisplacement, filters.MaxDisplacement, _displacement));
                }
                if (filters.MaxPrice > 0 || filters.MinPrice > 0)
                {
                    query &= FDS.Range(RangeQuery<BikeModelDocument>(filters.MinPrice, filters.MaxPrice, _exshowroom));
                }
                if (filters.MakeId > 0)
                {
                    query &= FDS.Term(_bikeMakeId, filters.MakeId);
                }
                if (filters.MinMileage > 0 || filters.MaxMileage > 0)
                {
                    query &= FDS.Range(RangeQuery<BikeModelDocument>(filters.MinMileage, filters.MaxMileage, _mileage));
                }
                if (filters.BodyStyle > 0)
                {
                    query &= FDS.Term(_bodyStyleId, filters.BodyStyle);
                }

            }
            catch (Exception)
            {

                throw;
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
        private Func<NumericRangeQueryDescriptor<T>, INumericRangeQuery> RangeQuery<T>(double min, double max, string fieldName) where T : class
        {

            return v => v.Field(new Field(fieldName)).GreaterThanOrEquals(min).LessThanOrEquals(max);
        }


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
                        indexName = "";
                        break;
                    default:
                        indexName = "";
                        break;
                }
            }
            catch (Exception)
            {

                throw;
            }
            return indexName;
        }

    }
}
