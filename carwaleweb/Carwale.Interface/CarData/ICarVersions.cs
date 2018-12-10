using Carwale.DTOs.CarData;
using Carwale.DTOs.NewCars;
using Carwale.Entity;
using Carwale.Entity.CarData;
using Carwale.Interfaces.PriceQuote;
using System;
using System.Collections.Generic;

namespace Carwale.Interfaces.CarData
{
    public interface ICarVersions
    {
        List<CarVersions> GetCarVersions(int modelId, Status status);
        int[] ProcessVersionsData(int modelId, List<NewCarVersionsDTO> versionsList);
        CarFuel GetCarFuel(int versionId);
        List<CarVersionEntity> GetCarVersionsByType(string type, int modelId, int cityId, UInt16? year = null);
        Dictionary<int, List<CarVersionDetails>> GetVersionDetailsList(List<int> modelList, List<int> versionList, int cityid, bool orp = false, string type = "New");
        PqVersionCitiesEntity PqVersionsAndCities(int modelId);
        List<NewCarVersionsDTO> MapCarVersionDtoWithCarVersionEntity(int modelId, int cityId, bool isVersionsPassed = false, List<CarVersions> carVersionList = null);
        List<NewCarVersionsDTO> MapUpcomingVersionDTOWithEntity(int modelId, int cityId, bool isCityPage = false);
        List<CarVersionDetails> GetSelectedVersionDetails(string compareVersions);
        VersionMaskingNameValidation FetchVersionInfoFromMaskingName(string modelMaskingName, string versionMaskingName, string modelId, string versionId, bool isMsite, string makeMaskingName = null);
        List<List<Carwale.Entity.CompareCars.Color>> GetVersionsColors(List<int> versionId);
        bool CheckTyresExists(int versionId);
    }
}
