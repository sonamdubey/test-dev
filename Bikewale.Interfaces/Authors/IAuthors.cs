using Bikewale.Entities.Authors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Interfaces.Authors
{
    /// <summary>
    /// Created by: Ashutosh Sharma on 20-Sep-2017
    /// Description : Define methods related to authors.
    /// </summary>
    public interface IAuthors
    {
        IEnumerable<AuthorEntityBase> GetAuthorsList();
    }
}
