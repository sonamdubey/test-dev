using Carwale.Entity;
using Carwale.Entity.CarData;
using Carwale.Entity.CentralizedCacheRefresh;
using Carwale.Entity.CompareCars;
using System;
using System.Collections.Generic;

namespace Carwale.Interfaces.CarData
{
    public interface ICarVersionCacheRepository
    {
        List<CarVersions> GetVersionSummaryByModel(int modelId, Status status);
        List<CarVersionEntity> GetCarVersionsByType(string type, int modelId, UInt16? year = null);
        CarVersionDetails GetVersionDetailsById(int versionId);
        List<CarVersions> GetOtherCarVersionsOfModel(int versionId);
        int GetDefaultVersionId(int cityId, int modelId);
        VersionMaskingResponse GetVersionInfoFromMaskingName(string maskingName, int modelId, int versionId);
        List<Color> GetVersionColors(int versionId);
        Dictionary<int, CarVersionDetails> MultiGetVersionDetails(List<int> versionList);
        bool RefreshCarVersionCache(List<VersionAttribute> versionAttributes);
        int GetVersionCountByModel(int modelId);
    }
}
