using Bikewale.ElasticSearch.Entities;
using ElasticClientManager;
using Nest;
using System;

namespace Bikewale.ElasticSearch.Indexes
{
    /// <summary>
    /// Created by  :   Sumit Kate on 19 Feb 2018
    /// Description :   Implementes IIndexFactory interface and it's methods
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class IndexFactory<T> : IIndexFactory<T> where T : Document
    {
        private ElasticClient _client;
        private readonly IndexSettings _DefaultIndexSettings = null;
        private readonly IndexState _DefaultIndexConfig = null;
        private ICreateIndexRequest _request = null;

        protected void Initialize()
        {
            _client = ElasticClientOperations.GetElasticClient();
        }

        /// <summary>
        /// Default constructor
        /// Initializes the default settings for ES Index
        /// NumberOfReplicas : 2 and NumberOfShards : 2
        /// </summary>
        public IndexFactory()
        {
            Initialize();
            _DefaultIndexSettings = new IndexSettings { NumberOfReplicas = 2, NumberOfShards = 2 };
            _DefaultIndexConfig = new IndexState { Settings = _DefaultIndexSettings };
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 19 feb 2018
        /// Description :   Constructor to initialize custom settings for a ES Index
        /// </summary>
        /// <param name="numberOfReplicas"></param>
        /// <param name="numberOfShards"></param>
        public IndexFactory(int numberOfReplicas, int numberOfShards)
        {
            if (numberOfReplicas > 0 && numberOfShards > 0)
            {
                Initialize();
                _DefaultIndexSettings = new IndexSettings { NumberOfReplicas = numberOfReplicas, NumberOfShards = numberOfShards };
                _DefaultIndexConfig = new IndexState { Settings = _DefaultIndexSettings };
            }
            else
            {
                throw new ArgumentException("numberOfReplicas and numberOfShards should be greater than 0.");
            }
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 19 Feb 2018
        /// Description :   Creates ES Index based on mapping passed as parameter
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="indexName">Name of the Index. Should be in lower case</param>
        /// <param name="mapping"></param>
        /// <returns></returns>
        public bool CreateIndex<T>(string indexName, Func<MappingsDescriptor, IPromise<IMappings>> mapping)
        {
            bool success = false;
            try
            {
                if (mapping == null)
                {
                    throw new ArgumentException("No mapping is provided");
                }
                if (indexName.ToLower() == indexName)
                {
                    ICreateIndexResponse resp = _client.CreateIndex(indexName, c => c.InitializeUsing(_DefaultIndexConfig).Mappings(mapping));
                    success = resp != null && resp.Acknowledged;
                }
                else
                {
                    throw new ArgumentException("Index Name should be in Lower case");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return success;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 19 Feb 2018
        /// Description :   Deletes as ES Index. Identified by the Name
        /// </summary>
        /// <param name="indexName"></param>
        /// <returns></returns>
        public bool DeleteIndex(string indexName)
        {
            bool success = false;
            try
            {
                var resp = _client.DeleteIndex(indexName);
                success = resp.Acknowledged;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return success;
        }
    }
}
