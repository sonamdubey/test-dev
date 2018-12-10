using Carwale.Entity;
using Carwale.Entity.Accessories.Tyres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.Accessories.Tyres
{
    public interface ITyresBL
    {
        TyreList GetTyresByCarModels(string carModelIds, int pageNumber, int pageSize);
        VersionTyres GetTyresByCarVersion(int carVersionId, int pageNumber, int pageSize);
        TyreDetailSummary GetTyreDataByItemId(int itemId);
        TyreList GetTyresByBrandId(int brandId, int pageNumber, int pageSize);
        List<int> CheckForTyres(List<int> versionIds);
    }
}
