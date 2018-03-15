using System.Collections.Generic;

namespace BikewaleOpr.Interface.ElasticSearch
{
    /// <summary>
    /// Created by  :   Sumit Kate on 21 Feb 2018
    /// Description :   Interface for ES Index operations    
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IESIndexRepository<T>
    {
        IEnumerable<T> GetDocuments(string indexName, IEnumerable<string> documentIds);
        T GetDocument(string indexName, string documentId);
        bool InsertDocument(string queueName, string indexName, string documentType, string id, T document);
        bool UpdateDocument(string queueName, string indexName, string documentType, string id, T document);
        bool DeleteDocument(string queueName, string indexName, string documentType, string id);
    }
}
