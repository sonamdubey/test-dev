using Carwale.Entity.Classified;

namespace Carwale.Interfaces.Classified.Search
{
    public interface IMetaKeywordsSearch
    {
        MetaKeywords GetMetaKeywordsSearchPage(string makeName, string rootName, string cityName, int pageNo, int totalPages, bool isMsite = false);
    }
}
