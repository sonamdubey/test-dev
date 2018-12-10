using Carwale.Entity.Author;
using Carwale.Entity.CMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.Author
{
    public interface IAuthorRepository
    {
        AuthorEntity GetAuthorDetails(int authorId);
        List<AuthorList> GetAuthorsList();
        List<ExpertReviews> GetExpertReviewsByAuthor(int authorId, CMSAppId applicationId);
        List<NewsEntity> GetNewsByAuthor(int authorId, CMSAppId applicationId);
        List<AuthorList> GetAllOtherAuthors(int authorId);
    }
}
