using Bikewale.ElasticSearch.Entities;
using Nest;
using System;

namespace Bikewale.ElasticSearch.Indexes
{
    /// <summary>
    /// Created by  :   Sumit Kate on 19 Feb 2018
    /// Description :   ES Index Factory interface which defines all the operation 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IIndexFactory<T> where T : Document
    {
        bool CreateIndex<T>(string indexName, Func<MappingsDescriptor, IPromise<IMappings>> mapping);
        bool DeleteIndex(string indexName);
    }
}
