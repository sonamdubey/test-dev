using Bikewale.Entities.Authors;
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
    }
}
