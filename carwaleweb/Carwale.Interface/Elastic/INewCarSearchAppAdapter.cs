using Carwale.DTOs.CarData;
using Carwale.Entity.CarData;
using Carwale.Entity.Classified;
using Carwale.Entity.ElasticEntities;
using Carwale.Entity.NewCarFinder;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Carwale.Interfaces.Elastic
{
    public interface INewCarSearchAppAdapter
    {
        DTOs.Search.Version.NewCarSearchDTO GetVersions(NameValueCollection queryString);
        DTOs.Search.Model.NewCarSearchDTO GetModels(NameValueCollection queryString);
        DTOs.Search.Model.NewCarFinderDto GetNCFModels(NameValueCollection queryString);
        List<BodyTypesDto> GetBodyTypes(NameValueCollection queryString);
        List<FuelTypesDto> GetFuelTypes(NameValueCollection queryString);
		AllFiltersDTO GetAllFilters(int sourceId);
        MakeFilter GetMakeList(NameValueCollection queryString);
    }
}
