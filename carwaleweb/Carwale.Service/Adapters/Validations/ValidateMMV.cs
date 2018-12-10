using Carwale.Interfaces.CarData;
using Carwale.Interfaces.Validations;

namespace Carwale.Service.Adapters.Validations
{
    public class ValidateMmv : IValidateMmv
    {
        private readonly ICarVersionCacheRepository _versionCache;
        private readonly ICarModelCacheRepository _modelCache;

        public ValidateMmv(ICarVersionCacheRepository versionCache, ICarModelCacheRepository modelCache)
        {
            _versionCache = versionCache;
            _modelCache = modelCache;
        }

        public bool IsModelVersionValid(int versionId)
        {
            if (versionId > 0)
            {
                var versionDetails = _versionCache.GetVersionDetailsById(versionId);
                if (versionDetails.New == 1 && !versionDetails.IsDeleted && IsModelValid(versionDetails.ModelId))
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsModelValid(int modelId)
        {
            if (modelId > 0)
            {
                var modelDetails = _modelCache.GetModelDetailsById(modelId);
                if (modelDetails.New && !modelDetails.IsDeleted)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
