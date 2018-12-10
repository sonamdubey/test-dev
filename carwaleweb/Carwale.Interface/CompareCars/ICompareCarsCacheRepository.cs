using Carwale.Entity;
using Carwale.Entity.CompareCars;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.CompareCars
{
    public interface ICompareCarsCacheRepository
    {
        Tuple<Hashtable, Hashtable> GetSubCategories();

        Hashtable GetItems();

        Tuple<Hashtable, List<Color>, CarWithImageEntity> GetVersionData(int versionId);

        List<HotCarComparison> GetHotComaprisons(short _topCount);

        List<CompareCarOverview> GetCompareCarsDetails(Pagination page);

        int GetCompareCarCount();
    }
}
