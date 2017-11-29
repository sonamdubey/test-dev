using Bikewale.Entities.Authors;
using Bikewale.Entities.CMS.Articles;
using System.Collections.Generic;

namespace Bikewale.Models.Authors
{
    /// <summary>
    /// Created by : Ashutosh Sharma on 20-Sep-2017
    /// Description :  ViewModel for authors list.
    /// </summary>
    public class AuthorsListVM : ModelBase
    {
        public IEnumerable<AuthorEntityBase> AuthorsList { get; set; }
        public CMSContent ArticlesList { get; set; }
    }
}
