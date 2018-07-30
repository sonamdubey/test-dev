using Bikewale.Entities.Authors;
using Bikewale.Entities.CMS.Articles;
using System.Collections.Generic;

namespace Bikewale.Interfaces.Authors
{
    /// <summary>
    /// Created by: Ashutosh Sharma on 20-Sep-2017
    /// Description : Define methods related to authors.
    /// </summary>
    public interface IAuthors
    {
        IEnumerable<AuthorEntityBase> GetAuthorsList(int applicationId);
        AuthorEntity GetAuthorDetailsViaGrpc(int authorId);
        IEnumerable<ArticleSummary> GetArticlesByAuthorViaGrpc(int authorId, int applicationId, string categoryList);
        IEnumerable<AuthorEntityBase> GetAllOtherAuthors(int authorId, int applicationId);
    }
}
