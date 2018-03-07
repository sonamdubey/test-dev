using Bikewale.ElasticSearch.DocumentBuilderConsumer.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Bikewale.ElasticSearch.DocumentBuilderConsumer.DocumentBuilders
{
    public class ModelIndexDocumentBuilder : IDocumentBuilder
    {

        public bool InsertDocuments(NameValueCollection nvc)
        {
            return true;
        }

        public bool UpdateDocuments(NameValueCollection nvc)
        {
            return true;
        }

        public bool DeleteDocuments(NameValueCollection nvc)
        {
            return true;
        }

        public IEnumerable<T> GetDocuments<T>(NameValueCollection inputParameters) where T : class
        {
            throw new NotImplementedException();
        }

    }
}
