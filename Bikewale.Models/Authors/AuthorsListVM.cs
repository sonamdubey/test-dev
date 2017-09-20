using Bikewale.Entities.Authors;
using Bikewale.Entities.CMS.Articles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Models.Authors
{
    public class AuthorsListVM : ModelBase
    {
        public IEnumerable<AuthorEntityBase> AuthorsList { get; set; }
        public CMSContent ArticlesList { get; set; }
    }
}
