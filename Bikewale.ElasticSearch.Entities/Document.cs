
namespace Bikewale.ElasticSearch.Entities
{
    /// <summary>
    /// Created by  :   Sumit Kate on 19 Feb 2018
    /// Description :   Base class for all ES Document
    /// All Documents must inherite from this class
    /// </summary>
    public abstract class Document
    {
        public string Id { get; set; }
    }
}
