using Carwale.Entity;
using Carwale.Entity.CompareCars;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.CompareCars
{
    public interface ICompareCarsRepository
    {
        Tuple<Hashtable, Hashtable> GetSubCategories();

        Hashtable GetItems();

        Tuple<Hashtable, List<Color>, CarWithImageEntity> GetVersionData(int versionId);

        List<HotCarComparison> GetHotComaprisons(short _topCount);

        List<CompareCarOverview> GetCompareCarsDetails(Pagination page);

        int GetCompareCarCount();

        DataSet GetCarModels(int MakeId,int MakeId2, int MakeId3, int MakeId4,int type);

        DataSet GetCarVersionsDataForCompare(int version1, int version2);

        DataSet GetVersionsListForComapre(string ModelId);
    }
}
