using Bikewale.Entities.Authors;
using Bikewale.Entities.CMS.Articles;
using System.Collections.Generic;

namespace Bikewale.Interfaces.Authors
{
    public interface IAuthors
    {
        IEnumerable<AuthorEntityBase> GetAuthorsListViaGrpc();
        AuthorEntity GetAuthorDetailsViaGrpc(int authorId);
        IEnumerable<ArticleSummary> GetArticlesByAuthorViaGrpc(int authorId, int applicationId, string categoryList);
        IEnumerable<AuthorEntityBase> GetAllOtherAuthors(int authorId, int applicationId);
    }
}
