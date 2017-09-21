using Bikewale.Entities;
using Bikewale.Entities.Authors;
using Bikewale.Entities.CMS;
using Bikewale.Interfaces.Authors;
using Bikewale.Interfaces.EditCMS;
using Bikewale.Notifications;
using Bikewale.Utility;
using System;
using System.Linq;

namespace Bikewale.Models.Authors
{
    /// <summary>
    /// Created by : Vivek Singh Tomar on 20th Sep 2017
    /// Summary : Model page for author details page
    /// </summary>
    public class AuthorsDetailsPageModel
    {
        private readonly string _authorMaskingName;
        private int _authorId;
        private readonly IAuthors _authors = null;
        private readonly IArticles _articles = null;
        private readonly  IAuthorsCacheRepository _authorsCacheRepository;
        public StatusCodes status;
        public AuthorsMaskingReponse objResponse;
        public bool IsMobile { get; set; }

        public AuthorsDetailsPageModel(IAuthors authors, IArticles articles, IAuthorsCacheRepository authorsCacheRepository, string authorMaskingName)
        {
            _authors = authors;
            _articles = articles;
            _authorsCacheRepository = authorsCacheRepository;
            _authorMaskingName = authorMaskingName;
            ProcessQuery(_authorMaskingName);
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 20th Sep 2017
        /// Summary : Bind Author Details Page
        /// </summary>
        /// <returns></returns>
        public AuthorDetailsPageVM GetData()
        {
            AuthorDetailsPageVM objAuthorDetails = new AuthorDetailsPageVM();
            try
            {
                objAuthorDetails.Author = _authors.GetAuthorDetailsViaGrpc(_authorId);
                objAuthorDetails.NewsList = _authors.GetArticlesByAuthorViaGrpc(_authorId, Convert.ToInt32(BWConfiguration.Instance.ApplicationId), string.Format("{0}", (int)EnumCMSContentType.News));
                objAuthorDetails.ExpertReviewsList = _authors.GetArticlesByAuthorViaGrpc(_authorId, Convert.ToInt32(BWConfiguration.Instance.ApplicationId), string.Format("{0},{1}", (int)EnumCMSContentType.ComparisonTests, (int)EnumCMSContentType.RoadTest));
                objAuthorDetails.OtherAuthors = _authors.GetAllOtherAuthors(_authorId, Convert.ToInt32(BWConfiguration.Instance.ApplicationId));
                BindPopularNewsWidget(objAuthorDetails);
                BindPageMetaTags(objAuthorDetails);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.Authors.AuthorsDetailsPageModel: GetData");
            }
            return objAuthorDetails;
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 20th Sep 2017
        /// Summary : Get PopularNews for author details page
        /// </summary>
        /// <param name="objAuthorsDetails"></param>
        private void BindPopularNewsWidget(AuthorDetailsPageVM objAuthorsDetails)
        {
            try
            {
                string categoryId = Convert.ToString((int)EnumCMSContentType.News);
                int startIndex = 1;
                int endIndex = 50;
                objAuthorsDetails.ArticlesList = _articles.GetArticlesByCategoryList(categoryId, startIndex, endIndex);
                if (objAuthorsDetails.ArticlesList != null)
                {
                    objAuthorsDetails.ArticlesList.Articles = objAuthorsDetails.ArticlesList.Articles.OrderBy(c => c.Views).Take(3).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.Authors.AuthorsDetailsPageModel: BindPopularNewsWidget");
            }
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 20th Sep 2017
        /// Summary : Process author Masking Name passed in query string
        /// </summary>
        /// <param name="authorMaskingName"></param>
        private void ProcessQuery(string authorMaskingName)
        {
            objResponse = _authorsCacheRepository.GetAuthorsMaskingResponse(authorMaskingName);
            if(objResponse != null && objResponse.StatusCode == 200)
            {
                status = StatusCodes.ContentFound;
                _authorId = objResponse.AuthorId;
            }
            else
            {
                status = StatusCodes.ContentNotFound;
            }
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 20th Sep 2017
        /// Summary : Bind Page Meta Tags for author details page
        /// </summary>
        /// <param name="objAuthorDetails"></param>
        private void BindPageMetaTags(AuthorDetailsPageVM objAuthorDetails)
        {
            try
            {
                objAuthorDetails.PageMetaTags.Title = string.Format("{0}, {1} | BikeWale", objAuthorDetails.Author.AuthorName, objAuthorDetails.Author.Designation);
                objAuthorDetails.PageMetaTags.Description = string.Format("{0} is {1} at BikeWale. Check out his bio, latest stories and connect with him on various social platforms!", objAuthorDetails.Author.AuthorName, objAuthorDetails.Author.Designation);
                objAuthorDetails.PageMetaTags.CanonicalUrl = string.Format("{0}/authors/{1}/",BWConfiguration.Instance.BwHostUrl, _authorMaskingName );
                objAuthorDetails.PageMetaTags.AlternateUrl = string.Format("{0}/mauthors/{1}/", BWConfiguration.Instance.BwHostUrl, _authorMaskingName);
            }
            catch(Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.Authors.AuthorsDetailsPageModel: BindPageMetaTags");
            }
        }

    }
}