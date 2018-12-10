using AEPLCore.Cache;
using AEPLCore.Cache.Interfaces;
using Carwale.Entity.Author;
using Carwale.Entity.CMS;
using Carwale.Interfaces;
using Carwale.Interfaces.Author;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Cache.Authors
{
    public class Author : IAuthorCacheRepository
    {
        private readonly IAuthorRepository _authorRepo;
        private readonly ICacheManager _cacheProvider;

        public Author(IAuthorRepository authorRepo, ICacheManager cacheProvider)
        {
            _authorRepo = authorRepo;
            _cacheProvider = cacheProvider;
        }

        /// <summary>
        /// Gets the Author details based on the authorId passed 
        /// Written By : Shalini on 18/07/14
        /// </summary>
        /// <param name="authorId"></param>
        /// <returns></returns>
        public AuthorEntity GetAuthorDetails(int authorId)
        {
            return _cacheProvider.GetFromCache<AuthorEntity>("AuthorDetails-" + authorId,
                CacheRefreshTime.GetInMinutes("DefaultCacheRefreshTime"),
                () => _authorRepo.GetAuthorDetails(authorId));
        }

        /// <summary>
        /// Gets the list of all authors
        /// Written By : Shalini on 18/07/14
        /// </summary>
        /// <returns></returns>
        public List<AuthorList> GetAuthorsList()
        {
            return _cacheProvider.GetFromCache<List<AuthorList>>("AuthorsList",
                CacheRefreshTime.GetInMinutes("DefaultCacheRefreshTime"),
                () => _authorRepo.GetAuthorsList());
        }

     /// <summary>
     /// Gets the list of Expert Reviews written By Author 
     /// Written By: Shalini on 30/07/14
     /// </summary>
     /// <param name="authorId"></param>
     /// <returns></returns>
        public List<ExpertReviews> GetExpertReviewsByAuthor(int authorId, CMSAppId applicationId)
        {
            return _cacheProvider.GetFromCache<List<ExpertReviews>>("ExpertReviewList-" + authorId + "_" + applicationId,
                 CacheRefreshTime.GetInMinutes("DefaultCacheRefreshTime"),
                 () => _authorRepo.GetExpertReviewsByAuthor(authorId,applicationId));
        }

        /// <summary>
        /// Gets the list of news writtten by Author
        /// Written By: Shalini on 30/07/14
        /// </summary>
        /// <param name="authorId"></param>
        /// <returns></returns>
        public List<NewsEntity> GetNewsByAuthor(int authorId, CMSAppId applicationId)
        {
            return _cacheProvider.GetFromCache<List<NewsEntity>>("NewsList-" + authorId + "_" + applicationId,
                CacheRefreshTime.GetInMinutes("DefaultCacheRefreshTime"),
                () => _authorRepo.GetNewsByAuthor(authorId, applicationId));
        }

        /// <summary>
        /// Gets the list of all other authors 
        /// Written by : Shalini on 30/07/14
        /// </summary>
        /// <param name="authorId"></param>
        /// <returns></returns>
        public List<AuthorList> GetAllOtherAuthors(int authorId)
        {
            return _cacheProvider.GetFromCache<List<AuthorList>>("OtherAuthors-" + authorId,
                CacheRefreshTime.GetInMinutes("DefaultCacheRefreshTime"),
                () => _authorRepo.GetAllOtherAuthors(authorId));
        }
    }
}
