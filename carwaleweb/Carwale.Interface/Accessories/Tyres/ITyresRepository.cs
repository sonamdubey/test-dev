using Carwale.Entity.Accessories.Tyres;
using System.Collections.Generic;

namespace Carwale.Interfaces.Accessories.Tyres
{
    public interface ITyresRepository
    {
        VersionTyres GetTyresByVersionId(int carVersionId);
        List<TyreSummary> GetTyresByModels(string carModelIds);
        int GetBrandIdFromMaskingName(string maskingName);
        List<TyreSummary> GetTyresByBrandId(int brandId);
    }
}
