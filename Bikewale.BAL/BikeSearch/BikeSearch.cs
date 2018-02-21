
using Bikewale.DAL.CoreDAL;
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
        private static string _displacement = "displacement";
        private static string _exshowroom = "exshowroom";
        private static string _bikeMakeId = "bikeMake.makeId";
        private static string _mileage = "mileage";
        private static string _bodyStyleId = "bodyStyleId";

        public IEnumerable<SuggestOption<T>> GetBikeSearch<T>(SearchFilters filters, BikeSearchEnum source, int noOfRecords = 0) where T : class
        {
            IEnumerable<SuggestOption<T>> suggestionList = null;

            try
            {
                suggestionList = GetBikeSearchList<T>(filters, source);


                if (suggestionList != null && noOfRecords > 0)
                {
                    suggestionList = suggestionList.Take(noOfRecords);
                }


            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.BAL.ElasticSearchManger.GetAutoSuggestResult");
            }
            return suggestionList;

        }
        private IEnumerable<SuggestOption<T>> GetBikeSearchList<T>(SearchFilters filters, BikeSearchEnum source) where T : class
        {
            IEnumerable<SuggestOption<T>> suggestionList = null;

            try
            {
                string indexName = GetIndexName(source);
                ElasticClient client = ElasticSearchInstance.GetInstance();

                if (client != null)
                {
                    Func<SearchDescriptor<T>, SearchDescriptor<T>> searchDescriptor = new Func<SearchDescriptor<T>, SearchDescriptor<T>>(
                           sd => sd.Index(indexName).Type("bikemodeldocument")
                                    .Query(q => q
                                    .Bool(bq => bq
                                     .Filter(ff => ff
                                         .Bool(bb => bb.
                                             Must(ProcessFilters<T>(filters)))))));
                    ISearchResponse<T> _result = client.Search<T>(searchDescriptor);

                }
            }
            catch (Exception)
            {

                throw;
            }
            return suggestionList;


        }
        private QueryContainer ProcessFilters<T>(SearchFilters filters) where T : class
        {
            QueryContainer query = new QueryContainer();
            QueryContainerDescriptor<T> FDS = new QueryContainerDescriptor<T>();
            try
            {
                if (filters.MinDisplacement > 0 || filters.MaxDisplacement > 0)
                {
                    query &= FDS.Range(RangeQuery<T>(filters.MinDisplacement, filters.MaxDisplacement, _displacement));
                }
                if (filters.MaxPrice > 0 || filters.MinPrice > 0)
                {
                    query &= FDS.Range(RangeQuery<T>(filters.MinPrice, filters.MaxPrice, _exshowroom));
                }
                if (filters.MakeId > 0)
                {
                    query &= FDS.Term(_bikeMakeId, filters.MakeId);
                }
                if (filters.MinMileage > 0 || filters.MaxMileage > 0)
                {
                    query &= FDS.Range(RangeQuery<T>(filters.MinMileage, filters.MaxMileage, _mileage));
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
