using Carwale.Interfaces.CarData;
using Carwale.Interfaces.NewCars;
using System;
using System.Collections.Generic;
using Carwale.DTOs.CarData;
using AutoMapper;
using Carwale.Entity.CarData;
using Carwale.Utility;
using Carwale.Notifications;

namespace Carwale.BL.NewCars
{
    /// <summary>
    /// Created By:Shalini
    /// </summary>
    public class CompareCarAdapterApp : IServiceAdapterV2
    {
        private readonly ICarDataLogic _compareCarsBL;
        private readonly ICarVersionCacheRepository _carVersionCacheRepository;
        private readonly ICarVersions _carVersionBl;

        public CompareCarAdapterApp(ICarDataLogic compareCarsBL, ICarVersionCacheRepository carVersionCacheRepository, ICarVersions carVersionBl)
        {
            _compareCarsBL = compareCarsBL;
            _carVersionCacheRepository = carVersionCacheRepository;
            _carVersionBl = carVersionBl;
        }

        public T Get<T, U>(U input)
        {
            return (T)Convert.ChangeType(GetCCarDataDtoForApp<U>(input), typeof(T));
        }

        private CCarDataDto GetCCarDataDtoForApp<U>(U input)
        {
            try
            {
                List<int> versionIds = (List<int>)Convert.ChangeType(input, typeof(U));
                var versions = _compareCarsBL.GetCombinedCarDataOldApp(versionIds);
                var cardetails = new List<Carwale.DTOs.CarData.CarWithImageEntityDTO>();
                foreach (int version in versionIds)
                {
                    var versiondetails = _carVersionCacheRepository.GetVersionDetailsById(version);
                    cardetails.Add(versiondetails != null ? new CarWithImageEntityDTO
                    {
                        HostURL = versiondetails.HostURL,
                        Image = CWConfiguration._imgHostUrl + ImageSizes._210X118 + versiondetails.OriginalImgPath ?? string.Empty,
                        ImageSmall = CWConfiguration._imgHostUrl + ImageSizes._110X61 + versiondetails.OriginalImgPath ?? string.Empty,
                        MakeId = versiondetails.MakeId,
                        MakeName = versiondetails.MakeName,
                        MaskingName = versiondetails.VersionMasking,
                        ModelId = versiondetails.ModelId,
                        ModelName = versiondetails.ModelName,
                        OriginalImgPath = versiondetails.OriginalImgPath,
                        VersionId = versiondetails.VersionId,
                        VersionName = versiondetails.VersionName
                    } : new CarWithImageEntityDTO
                    {
                        VersionId = version
                    });
                }


                CCarDataDto data = new CCarDataDto
                {
                    Specs = Mapper.Map<List<SubCategory>>(versions.Specifications),
                    Features = Mapper.Map<List<SubCategory>>(versions.Features),
                    OverView = Mapper.Map<List<Item>>(versions.Overview),
                    ValidVersionIds = versionIds,
                    Colors = Mapper.Map<List<List<Carwale.DTOs.CarData.Color>>>(_carVersionBl.GetVersionsColors(versionIds)),
                    CarDetails = cardetails,
                    FeaturedVersionId = -1
                };
                data.CarDetails.ForEach(item => item.Versions = Mapper.Map<List<CarVersionEntity>, List<Versions>>(_carVersionCacheRepository.GetCarVersionsByType("Compare", item.ModelId)));
                data.CarDetails.ForEach(item => item.Price = Format.GetPrice(item.Price.ToNullSafeString()));

                return data;
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CompareCarAdapterApp.GetCCarDataDtoForApp()");
                objErr.LogException();
            }
            return null;
        }
    }
}
