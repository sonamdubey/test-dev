using AutoMapper;
using Carwale.DTOs.Accessories.Tyres;
using Carwale.Entity.Accessories.Tyres;
using Carwale.Entity.CarData;
using Carwale.Entity.Common;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.NewCars;
using Carwale.Notifications.Logs;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Carwale.BL.Accessories.Tyres
{
    public class TyresAdapterMobileSearch : IServiceAdapterV2
    {
        private readonly TyresBL _tyres;
        private readonly ICarVersionCacheRepository _carVersionCache;
        private readonly ICarModelCacheRepository _carModelCache;

        public TyresAdapterMobileSearch(TyresBL tyres, ICarVersionCacheRepository carVersionCache, ICarModelCacheRepository carModelCache)
        {
            _tyres = tyres;
            _carVersionCache = carVersionCache;
            _carModelCache = carModelCache;
        }

        public T Get<T, U>(U input)
        {
            return (T)Convert.ChangeType(GetTyresData(input), typeof(T));
        }

        private TyresDTO GetTyresData<U>(U input)
        {
            TyresDTO tyreDtoMobile = new TyresDTO();
            tyreDtoMobile.CarVersions = new List<CarModelsVersionsList>();

            try
            {
                TyresSearchInput inputs = (TyresSearchInput)Convert.ChangeType(input, typeof(U));
                
                string[] modelIds = inputs.CMIds.Split(',');

                foreach (var strModelId in modelIds)
                {
                    int modelId;
                    Int32.TryParse(strModelId, out modelId);
                    var modelData = new CarModelsVersionsList();

                    var modelBasicInfo = _carModelCache.GetModelDetailsById(modelId);
                    if (modelBasicInfo.MakeId > 0)
                    {
                        modelData.MakeId = modelBasicInfo.MakeId;
                        modelData.MakeName = modelBasicInfo.MakeName;
                        modelData.ModelId = modelId;
                        modelData.ModelName = modelBasicInfo.ModelName;
                        modelData.DisplayName = Regex.Split(modelBasicInfo.ModelName, @"\[")[0];
                        modelData.HostUrl = modelBasicInfo.HostUrl;
                        modelData.OriginalImgPath = modelBasicInfo.OriginalImage;

                        modelData.Versions = new List<IdName>();
                        modelData.Versions = Mapper.Map<List<CarVersionEntity>, List<IdName>>(_carVersionCache.GetCarVersionsByType("nonfuturistic", modelId));

                        tyreDtoMobile.CarVersions.Add(modelData);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return tyreDtoMobile;
        }

    }
}
