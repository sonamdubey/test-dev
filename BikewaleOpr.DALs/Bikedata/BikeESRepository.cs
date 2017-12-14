using System;
using System.Linq;
using Bikewale.DAL.CoreDAL;
using Bikewale.Notifications;
using BikewaleOpr.Entity.ElasticSearch;
using BikewaleOpr.Interface.BikeData;
using Nest;

namespace BikewaleOpr.DALs.Bikedata
{
    public class BikeESRepository : IBikeESRepository
    {
        private readonly ElasticClient _client;
        public BikeESRepository()
        {
            _client = ElasticSearchInstance.GetInstance();
        }
        
        /// <summary>
        /// Created by  : Vivek Singh Tomar on 13th Dec 2017
        /// Description : Get Bike data for given ID from ES index 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BikeList GetBikeESIndex(string id, string indexName)
        {
            BikeList bikeData = null;
            try
            {
                if (_client != null)
                {
                    var ElasticResponse = _client.Search<BikeList>(s => s
                                    .Index(indexName)
                                    .Type("bikelist")
                                    .Query(q => q
                                        .Term(t => t.Field("id")
                                        .Value(id)
                                        )
                                     )
                                  );
                    if(ElasticResponse != null && ElasticResponse.Hits != null && ElasticResponse.Hits.Count == 1)
                    {
                        bikeData = ElasticResponse.Hits.First().Source;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikeWaleOpr.DALs.Bikedata.BikeESRepository : GetBikeESIndex, Id = {0}, IndexName = {1}", id, indexName));
            }

            return bikeData;
        }

        /// <summary>
        /// Created by  : Vivek Singh Tomar on 13th Dec 2017
        /// Description : Update bike data in ES index
        /// </summary>
        /// <param name="id"></param>
        /// <param name="bike"></param>
        /// <returns></returns>
        public bool UpdateBikeESIndex(string id, string indexName, BikeList bike)
        {
            IUpdateResponse<BikeList> resp = null;

            try
            {
                resp = _client.Update<BikeList, BikeList>(id, d => d
                        .Index(indexName)
                        .Type("bikelist")
                        .Doc(bike));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikeWaleOpr.DALs.Bikedata.BikeESRepository : UpdateBikeESIndex, Id = {0}, IndexName = {1}", id, indexName));
            }

            return (resp != null && resp.Result == Result.Updated);
        }
    }
}
