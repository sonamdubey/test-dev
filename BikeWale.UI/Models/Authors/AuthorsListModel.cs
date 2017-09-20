using Bikewale.Interfaces.EditCMS;
using Bikewale.Interfaces.Authors;
using Bikewale.Notifications;
using System;
using System.Linq;

namespace Bikewale.Models.Authors
{
    public class AuthorsListModel
    {
        private readonly IAuthors _Authors = null;
        private readonly IArticles _Articles = null;
        public AuthorsListModel(IAuthors Authors, IArticles Articles)
        {
            _Authors = Authors;
            _Articles = Articles;
        }
        public AuthorsListVM GetData()
        {
            AuthorsListVM _objAuthorsList = null;
            try
            {
                _objAuthorsList = new AuthorsListVM();
                _objAuthorsList.AuthorsList =  _Authors.GetAuthorsList();
                BindPopularNewsWidget(_objAuthorsList);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.Authors.AuthorsListModel");
            }
            return _objAuthorsList;
        }


        private void BindPopularNewsWidget(AuthorsListVM objAuthorsList)
        {
            try
            {
                string categoryId = Convert.ToString((int)Entities.CMS.EnumCMSContentType.News);
                int startIndex = 1;
                int endIndex = 50;
                objAuthorsList.ArticlesList = _Articles.GetArticlesByCategoryList(categoryId, startIndex, endIndex);
                if (objAuthorsList.ArticlesList != null)
                {
                    objAuthorsList.ArticlesList.Articles = objAuthorsList.ArticlesList.Articles.OrderBy(c => c.Views).Take(3).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.Authors.AuthorsListModel");
            }
        }
    }
}