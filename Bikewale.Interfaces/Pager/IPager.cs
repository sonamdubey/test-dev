using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Entities.Pager;

namespace Bikewale.Interfaces.Pager
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 8 May 2014
    /// </summary>
    public interface IPager
    {
        T GetPager<T>(PagerEntity pagerDetails) where T : PagerOutputEntity, new();
        void GetStartEndIndex(int pageSize, int currentPageNo, out int startIndex, out int endIndex);
        int GetTotalPages(int totalRecords, int pageSize);
    }
}
