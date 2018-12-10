using AutoMapper;
using Carwale.DTOs.CarData;
using Carwale.DTOs.CMS.ThreeSixtyView;
using Carwale.DTOs.OffersV1;
using Carwale.Entity;
using Carwale.Entity.AdapterModels;
using Carwale.Entity.CarData;
using Carwale.Entity.Enum;
using Carwale.Entity.Geolocation;
using Carwale.Entity.OffersV1;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.NewCars;
using Carwale.Interfaces.Offers;
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
    public class VersionPageAdapterApp : IServiceAdapterV2
    {
        private readonly ICarVersions _carVersion;
        ICarModelCacheRepository _modelCacheRepository;
        private readonly IOffersAdapter _offersAdapter;

        public VersionPageAdapterApp(ICarVersions carVersion, ICarModelCacheRepository modelCacheRepository, IOffersAdapter offersAdapter)
        {
            _carVersion = carVersion;
            _modelCacheRepository = modelCacheRepository;
            _offersAdapter = offersAdapter;
        }
        public T Get<T, U>(U input)
        {
            return (T)Convert.ChangeType(GetVersionPageDTOForApp(input), typeof(T));
        }


        CarDetailsListDTOV2 GetVersionPageDTOForApp<U>(U input)
        {
            var carDetailsList = new CarDetailsListDTOV2();
            carDetailsList.Models = new List<VersionDetailsDtoV2>();
            Dictionary<int, List<CarVersionDetails>> versionListDetails = null;
            try
            {
                CarDataAdapterInputs inputParam = (CarDataAdapterInputs)Convert.ChangeType(input, typeof(U));
                var carList = inputParam.ModelIds != null && inputParam.ModelIds.Count > 0 ? inputParam.ModelIds : inputParam.VersionIds;

                versionListDetails = inputParam.ModelIds != null && inputParam.ModelIds.Count > 0
												  ? _carVersion.GetVersionDetailsList(inputParam.ModelIds, null, inputParam.CustLocation.CityId, true, inputParam.Type)
                                                  : _carVersion.GetVersionDetailsList(null, inputParam.VersionIds, inputParam.CustLocation.CityId, true, inputParam.Type);

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
                            carModelDetailsDict.Add(versionInfo.ModelId, _modelCacheRepository.GetModelDetailsById(versionInfo.ModelId));
                        }
                        var results = Mapper.Map<List<CarVersionDetails>, List<VersionListDTO>>(versionDetails);
                        
                        foreach (var version in results)
                        {
                            var offerInput = SetOfferInputs(versionInfo, version.Id, inputParam.CustLocation);
                            version.Offer = Mapper.Map<OfferDto>(_offersAdapter.GetOffers(offerInput));
                        }

                        carDetailsList.Models.Add(new VersionDetailsDtoV2
                        {
                            VersionDetails = results,
                            ModelId = versionInfo.ModelId,
                            MakeId = versionInfo.MakeId,
                            ModelName = versionInfo.ModelName,
                            MakeName = versionInfo.MakeName,
                            ThreeSixtyAvailability = Mapper.Map<CarModelDetails, ThreeSixtyAvailabilityDTO>(carModelDetailsDict[versionInfo.ModelId]),
                            PriceBreakUpText = inputParam.CustLocation.CityId > 0 && versionInfo.PriceOverview.PriceStatus == (int)PriceBucket.HaveUserCity ? ConfigurationManager.AppSettings["ViewPriceBreakupText"] : string.Empty
                        });
                    }
                }
				if (carDetailsList.Models != null && carDetailsList.Models.Count > 0)
				{
					carDetailsList.OrpText = inputParam.CustLocation.CityId <= 0 ? ConfigurationManager.AppSettings["ShowPriceInCityText"] : string.Empty;
				}
			}
            catch(Exception ex)
            {
                Logger.LogException(ex);
            }
            return carDetailsList;

        }

        private OfferInput SetOfferInputs(CarVersionDetails versionDetails, int versionId, Location cityDetails)
        {
            var offerInput = new OfferInput
            {
                ApplicationId = (int)Application.CarWale,
                MakeId = versionDetails.MakeId,
                ModelId = versionDetails.ModelId,
                VersionId = versionId
            };

            if (cityDetails == null || cityDetails.CityId < 1)
            {
                offerInput.CityId = -1;
                offerInput.StateId = -1;
            }
            else
            {
                offerInput.CityId = cityDetails.CityId;
                offerInput.StateId = cityDetails.StateId;
            }
            return offerInput;
        }

    }
}
