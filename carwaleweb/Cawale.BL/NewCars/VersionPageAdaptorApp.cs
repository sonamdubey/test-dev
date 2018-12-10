using AutoMapper;
using Carwale.DTOs.CarData;
using Carwale.DTOs.CMS.ThreeSixtyView;
using Carwale.Entity.AdapterModels;
using Carwale.Entity.CarData;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.NewCars;
using Carwale.Notifications.Logs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Carwale.BL.NewCars
{
    public class VersionPageAdaptorApp : IServiceAdapterV2
    {
        private readonly ICarVersions _carVersion;
        public VersionPageAdaptorApp(ICarVersions carVersion)
        {
            _carVersion = carVersion;
        }
        public T Get<T, U>(U input)
        {
            return (T)Convert.ChangeType(GetVersionPageDTOForApp(input), typeof(T));
        }


        CarDetailsListDTOV2 GetVersionPageDTOForApp<U>(U input)
        {
            var carDetailsList = new CarDetailsListDTOV2();
            carDetailsList.Models = new List<VersionDetailsDTO>();
            Dictionary<int, List<CarVersionDetails>> versionListDetails = null;
            try
            {
                CarDataAdapterInputs inputParam = (CarDataAdapterInputs)Convert.ChangeType(input, typeof(U));
                var carList = inputParam.ModelIds != null && inputParam.ModelIds.Count > 0 ? inputParam.ModelIds : inputParam.VersionIds;
                versionListDetails = inputParam.ModelIds != null && inputParam.ModelIds.Count > 0 ? _carVersion.GetVersionDetailsList(inputParam.ModelIds, null, inputParam.CustLocation.CityId, true)
                                                  : _carVersion.GetVersionDetailsList(null, inputParam.VersionIds, inputParam.CustLocation.CityId, true);

                Dictionary<int, CarModelDetails> carModelDetailsDict = new Dictionary<int, CarModelDetails>();
                foreach (var car in carList)
                {
                    List<CarVersionDetails> versionDetails;
                    versionListDetails.TryGetValue(car, out versionDetails);

                    CarVersionDetails versionInfo = null;
                    if (versionDetails != null && versionDetails.Count > 0)
                    {
                        versionInfo = versionDetails.First();
                    }
                    if (versionInfo != null && versionInfo.ModelId > 0)
                    {
                        if (!carModelDetailsDict.ContainsKey(versionInfo.ModelId))
                        {
                            carModelDetailsDict.Add(versionInfo.ModelId, _carVersion.modelCacheRepository.GetModelDetailsById(versionInfo.ModelId));
                        }
                        var results = Mapper.Map<List<CarVersionDetails>, List<VersionListDTO>>(versionDetails);
                        carDetailsList.Models.Add(new VersionDetailsDTO
                        {
                            VersionDetails = results,
                            ModelId = versionInfo.ModelId,
                            MakeId = versionInfo.MakeId,
                            ModelName = versionInfo.ModelName,
                            MakeName = versionInfo.MakeName,
                            ThreeSixtyAvailability = Mapper.Map<CarModelDetails, ThreeSixtyAvailabilityDTO>(carModelDetailsDict[versionInfo.ModelId])
                        });
                    }

                    carDetailsList.OrpText = inputParam.CustLocation.CityId <= 0 ? ConfigurationManager.AppSettings["ShowPriceInCityText"] : string.Empty;
                }
            }
            catch(Exception ex)
            {
                Logger.LogException(ex);
            }
            return carDetailsList;

        }
    }
}
