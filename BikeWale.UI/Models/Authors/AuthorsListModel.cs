
using Bikewale.Interfaces.Authors;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.Models.Authors
{
    public class AuthorsListModel
    {
        private readonly IAuthors _Authors = null;
        public AuthorsListModel(IAuthors Authors)
        {
            _Authors = Authors;
        }
        public AuthorsListVM GetData()
        {
            AuthorsListVM _objAuthorsList = null;
            try
            {
                _objAuthorsList = new AuthorsListVM();
                _objAuthorsList.AuthorsList =  _Authors.GetAuthorsListViaGrpc();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.Authors.AuthorsListModel");
            }
            return _objAuthorsList;
        }
    }
}