using Carwale.DTOs.Classified.Stock;
using Carwale.DTOs.Classified.Stock.Ios;
using Carwale.DTOs.Elastic;
using Carwale.Entity.Classified;
using Carwale.Entity.Elastic;
using Carwale.Interfaces.Elastic;
using Carwale.DAL.CoreDAL;
using Carwale.Notifications;
using log4net;
using Microsoft.Practices.Unity;
using Nest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Carwale.DTOs.Classified;
using Carwale.Entity.CarData;

namespace Carwale.BL.Elastic
{
    public class ElasticSearchManager : IElasticSearchManager
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Class For Managing All the operation in ElasticSearch like Add, Delete, Search, Update
        /// Added By Jugal on 03 Dec 2014
        /// </summary>

        private readonly IESOperations esOperations;
        private readonly IUnityContainer _container;
        //ElasticClient clientElastic;
        public string indexElastic { get; set; }

        public ElasticSearchManager(IUnityContainer container, IESOperations _esOperations)
        {
            _container = container;
            esOperations = _esOperations;
        }

        public ElasticSearchManager()
        {

        }
        /// <summary>
        /// Search results in ElasticSearch Index.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="esIndex"></param>
        /// <returns></returns>
        public T SearchIndex<T>(string esIndex, FilterInputs filterInputs)
        {
            ElasticClient clientElastic = ElasticClientInstance.GetInstance();

            T t = default(T);

            T results = GetResults<T>(clientElastic, filterInputs);    //Modified By : Sadhana Upadhyay on 10 Mar 2015

            t = (T)Convert.ChangeType((results), typeof(T));

            return t;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TInput"></typeparam>
        /// <param name="esIndex"></param>
        /// <param name="tInput"></param>
        /// <returns></returns>
        public T SearchIndexProfileRecommendation<T, TInput>(TInput tInput, int recommendationsCount)
        {
            ElasticClient clientElastic = ElasticClientInstance.GetInstance();
            T t = default(T);
            T results = GetElasticResults<T, TInput>(clientElastic, tInput, recommendationsCount);

            t = (T)Convert.ChangeType((results), typeof(T));

            return t;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 10 Mar 2015
        /// Summary : To get Search result
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// 
        public T GetResults<T>(ElasticClient clientElastic, FilterInputs filterInputs)
        {
            T t = default(T);

            if (typeof(T) == typeof(ResultsFiltersPagerAndroid))
                t = (T)Convert.ChangeType(esOperations.GetElasticResultsAndroid(clientElastic, filterInputs), typeof(T));

            else if (typeof(T) == typeof(ResultsFiltersPagerDesktop))
                t = (T)Convert.ChangeType(esOperations.GetElasticResultsDesktop(clientElastic, filterInputs), typeof(T));

            else if (typeof(T) == typeof(StockResultsMobile))
                t = (T)Convert.ChangeType(esOperations.GetElasticResultsMobile(clientElastic, filterInputs), typeof(T));

            else if (typeof(T) == typeof(FilterCountsAndroid))
                t = (T)Convert.ChangeType(esOperations.GetFilterResultAndroid(clientElastic, filterInputs), typeof(T));

            else if (typeof(T) == typeof(StockResultsAndroid))
                t = (T)Convert.ChangeType(esOperations.GetResultsDataAndroid(clientElastic, filterInputs), typeof(T));

            else if (typeof(T) == typeof(StockResultIos))
                t = (T)Convert.ChangeType(esOperations.GetResultsDataIos(clientElastic, filterInputs), typeof(T));
            //Added By : Sadhana Upadhyay on 28 May 2015 for filter Count for iOS platform
            else if (typeof(T) == typeof(FilterCountIos))
                t = (T)Convert.ChangeType(esOperations.GetFilterResultIos(clientElastic, filterInputs), typeof(T));

            else if (typeof(T) == typeof(ResultsRecommendation))
                t = (T)Convert.ChangeType(esOperations.GetRecommendationResults(clientElastic, filterInputs), typeof(T));

            return t;
        }


        public T GetElasticResults<T, TInput>(ElasticClient clientElastic, TInput tInput, int recommendationsCount)
        {
            T t = default(T);

            if (typeof(T) == typeof(List<StockBaseEntity>))
                t = (T)Convert.ChangeType(esOperations.GetRecommendationsForProfileId(clientElastic, tInput, recommendationsCount), typeof(T));
            return t;
        }

        /// <summary>
        /// Call the ESStockOperations GetTotalStock method for getting the total stock count
        /// </summary>
        /// <param name="esIndex"></param>
        /// <returns>total stock count</returns>
        public int GetTotalStockCount(string esIndex, FilterInputs filterInputs)
        {
            ElasticClient clientElastic = ElasticClientInstance.GetInstance();
            return esOperations.GetTotalStockCount(clientElastic, filterInputs);
        }

        public int GetStocksCountByField(string esIndex, FilterInputs filterInputs, string field, double fieldValue, bool greaterThanFieldValue)
        {
            ElasticClient clientElastic = ElasticClientInstance.GetInstance();
            return esOperations.GetStocksCountByField(clientElastic, filterInputs, field, fieldValue, greaterThanFieldValue);
        }

        /// <summary>
        /// Call the ESStockOperations GetTotalStock method for getting the total stock count
        /// </summary>
        /// <param name="esIndex"></param>
        /// <returns>total stock count</returns>
        public List<CarMakeEntityBase> GetAllMakes(string esIndex)
        {
            ElasticClient clientElastic = ElasticClientInstance.GetInstance();
            return esOperations.GetAllMakes(clientElastic);
        }
    }
}
