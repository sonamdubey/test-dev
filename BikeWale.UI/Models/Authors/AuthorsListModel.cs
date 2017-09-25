using Bikewale.Interfaces.EditCMS;
using Bikewale.Interfaces.Authors;
using Bikewale.Notifications;
using System;
using System.Linq;
using Bikewale.Utility;

namespace Bikewale.Models.Authors
{
    /// <summary>
    /// Created by: Ashutosh Sharma on 20-Sep-2017
    /// Description : Model class for authors list page.
    /// </summary>
    public class AuthorsListModel
    {
        private readonly IAuthors _Authors = null;
        private readonly IArticles _Articles = null;
        public AuthorsListModel(IAuthors Authors, IArticles Articles)
        {
            _Authors = Authors;
            _Articles = Articles;
        }

        /// <summary>
        /// Created by: Ashutosh Sharma on 20-Sep-2017
        /// Description : Method to get data for author details page
        /// </summary>
        /// <returns></returns>
        public AuthorsListVM GetData()
        {
            AuthorsListVM _objAuthorsList = null;
            try
            {
                _objAuthorsList = new AuthorsListVM();
                _objAuthorsList.AuthorsList =  _Authors.GetAuthorsList();
                BindPopularNewsWidget(_objAuthorsList);
                BindPageMetas(_objAuthorsList.PageMetaTags);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.Authors.AuthorsListModel");
            }
            return _objAuthorsList;
        }

        /// <summary>
        /// Created by: Ashutosh Sharma on 20-Sep-2017
        /// Description : Method to bind popular news widget on author page.
        /// </summary>
        /// <param name="objAuthorsList"></param>
        private void BindPopularNewsWidget(AuthorsListVM objAuthorsList)
        {
            try
            {
                string categoryId = Convert.ToString((int)Entities.CMS.EnumCMSContentType.News);
                int startIndex = 1;
                int endIndex = 3;
                objAuthorsList.ArticlesList = _Articles.GetArticlesByCategoryList(categoryId, startIndex, endIndex);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.Authors.AuthorsListModel");
            }
        }


        /// <summary>
        /// Created by: Ashutosh Sharma on 20-Sep-2017
        /// Description : Method to bind page metas
        /// </summary>
        /// <param name="pageMetaTags"></param>
        private void BindPageMetas(PageMetaTags pageMetaTags)
        {
            try
            {
                pageMetaTags.Title = "List of authors | BikeWale";
                pageMetaTags.Description = "BikeWale has a credible team of authors covering the entire spectrum of two wheeler industry. Check out the list of all authors at BikeWale.";
                pageMetaTags.CanonicalUrl = string.Format("{0}/authors", BWConfiguration.Instance.BwHostUrl);
                pageMetaTags.AlternateUrl= string.Format("{0}/m/authors", BWConfiguration.Instance.BwHostUrl);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.Authors.AuthorsListModel");
            }
        }
    }
}