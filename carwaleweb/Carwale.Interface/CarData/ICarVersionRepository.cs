using Carwale.Entity;
using Carwale.Entity.CarData;
using Carwale.Entity.Common;
using Carwale.Entity.CompareCars;
using System;
using System.Collections.Generic;

namespace Carwale.Interfaces
{
    public interface ICarVersionRepository
    {
        List<CarVersionEntity> GetCarVersionsByType(string type, int modelId, UInt16? year = null, bool isCriticalRead = false);
        List<CarVersions> GetVersionSummaryByModel(int modelId, Status status, bool isCriticalRead = false);
        List<ValuationVersions> GetValuationVersion(int Year, int ModelId, bool isCriticalRead = false);
        CarVersionDetails GetVersionDetailsById(int versionId, bool isCriticalRead = false);
        List<CarVersions> GetOtherCarVersionsOfModel(int versionId, bool isCriticalRead = false);
        int GetDefaultVersionId(int cityId, int modelId, bool isCriticalRead = false);
        List<VersionSubSegment> GetVersionSpecs(bool isCriticalRead = false);
        VersionMaskingResponse GetVersionInfoFromMaskingName(string maskingName, int modelId, int versionId, bool isCriticalRead = false);
        List<Color> GetVersionColors(int versionId, bool isCriticalRead = false);
        int GetVersionCountByModel(int modelId, bool isCriticalRead = false);
    }
}
