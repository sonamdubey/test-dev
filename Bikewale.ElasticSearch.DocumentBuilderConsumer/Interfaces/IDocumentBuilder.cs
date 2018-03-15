using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Bikewale.ElasticSearch.DocumentBuilderConsumer.Interfaces
{
    public interface IDocumentBuilder
    {
        bool InsertDocuments(NameValueCollection nvc);
        bool UpdateDocuments(NameValueCollection nvc);
        bool DeleteDocuments(NameValueCollection nvc);
    }
}
