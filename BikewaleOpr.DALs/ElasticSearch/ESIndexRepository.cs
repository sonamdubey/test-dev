using Bikewale.DAL.CoreDAL;
using Bikewale.Notifications;
using BikewaleOpr.Interface.ElasticSearch;
using Nest;
using System;
using System.Collections.Specialized;
using System.Linq;

namespace BikewaleOpr.DALs.ElasticSearch
{
    /// <summary>
    /// Created by  :   Sumit Kate on 21 Feb 2018
    /// Description :   ES Index Repository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ESIndexRepository<T> : IESIndexRepository<T> where T : class
    {
        private readonly ElasticClient _client;
        /// <summary>
        /// Created by  :   Sumit Kate on 21 Feb 2018
        /// Description :   ES Index Repository
        /// </summary>
        public ESIndexRepository()
        {
            _client = ElasticSearchInstance.GetInstance();
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 21 Feb 2018
        /// Description :   Returns the ES Documents from an Index
        /// </summary>
        /// <param name="indexName"></param>
        /// <param name="documentIds"></param>
        /// <returns></returns>
        public System.Collections.Generic.IEnumerable<T> GetDocuments(string indexName, System.Collections.Generic.IEnumerable<string> documentIds)
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
        /// Created by  :   Sumit Kate on 21 Feb 2018
        /// Description :   Returns the ES Document from an Index
        /// </summary>
        /// <param name="indexName"></param>
        /// <param name="documentId"></param>
        /// <returns></returns>
        public T GetDocument(string indexName, string documentId)
        {
            T document = null;
            try
            {
                var documents = GetDocuments(indexName, new string[] { documentId });
                if (documents != null)
                {
                    document = documents.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("GetDocument({0},{1})", indexName, documentId));
            }
            return document;
        }


        public bool InsertDocument(string queueName, string indexName, string documentType, string id, T document)
        {
            bool isSuccess = false;
            try
            {
                //Call Push to Consumer for Inserting document
                NameValueCollection nvc = new NameValueCollection();
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("InsertDocument({0},{1})", indexName, id));
            }
            return isSuccess;
        }

        public bool UpdateDocument(string queueName, string indexName, string documentType, string id, T document)
        {
            bool isSuccess = false;
            try
            {
                //Call Push to Consumer for Update document
                NameValueCollection nvc = new NameValueCollection();
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("UpdateDocument({0},{1})", indexName, id));
            }
            return isSuccess;
        }

        public bool DeleteDocument(string queueName, string indexName, string documentType, string id)
        {
            bool isSuccess = false;
            try
            {
                //Call Push to Consumer for Update document
                NameValueCollection nvc = new NameValueCollection();
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("UpdateDocument({0},{1})", indexName, id));
            }
            return isSuccess;
        }
    }
}
