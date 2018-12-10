using Carwale.Entity.ElasticEntities;
using Nest;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Carwale.Interfaces.Elastic
{
    public interface INewCarElasticSearch
    {
        ElasticCarData GetModels(NewCarSearchInputs inputs, bool isNcf = false);
        ElasticCarData GetVersions(NewCarSearchInputs inputs,bool isOrp = false);
        NewCarSearchInputs GetElasticInputs(NameValueCollection queryString);
        ElasticCarData GetModelsV2(NewCarSearchInputs inputs);
        NCFElasticCarData GetNCFModels(NewCarSearchInputs inputs);
        ElasticCarData GetModelsAndVersions(NewCarSearchInputs inputs, bool isNcf = false);
        ISearchResponse<CarBaseEntity> GetBodyTypes(NewCarSearchInputs inputs);
        ISearchResponse<CarBaseEntity> GetFuelTypes(NewCarSearchInputs inputs);
        List<ElasticModel> GetModelList(NewCarSearchInputs inputs);
    }
}
