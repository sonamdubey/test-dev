using Bikewale.Entities.Authors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Interfaces.Authors
{
    public interface IAuthors
    {
        IEnumerable<AuthorEntityBase> GetAuthorsList();
    }
}
