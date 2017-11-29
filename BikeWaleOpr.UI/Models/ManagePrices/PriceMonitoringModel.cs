
using AutoMapper;
using Bikewale.Notifications;
using BikewaleOpr.DTO.BikeData;
using BikewaleOpr.Entities.BikeData;
using BikewaleOpr.Entity.BikeData;
using BikewaleOpr.Entity.BikePricing;
using BikewaleOpr.Interface.BikeData;
using BikewaleOpr.Interface.Dealers;
using BikewaleOpr.Interface.Location;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BikewaleOpr.Models.ManagePrices
{
    /// <summary>
    /// Created By: Ashutosh Sharma on 31-07-2017
    /// Description : Model for Price monitoring report page.
    /// </summary>
    public class PriceMonitoringModel
    {
        private readonly IBikeMakes _makesRepo = null;
        private readonly IShowroomPricesRepository _pricesRepo = null;
        private readonly ILocation _locationRepo = null;
        private string pageTitle = "Price Monitoring Report";

        public PriceMonitoringModel(IBikeMakes makesRepo, IShowroomPricesRepository pricesRepo, ILocation locationRepo)
        {
            _makesRepo = makesRepo;
            _pricesRepo = pricesRepo;
            _locationRepo = locationRepo;
        }

        /// <summary>
        /// Created By : Ashutosh Sharma on 09-08-2017
        /// Description: Method to get bikemakes and states for price monitoring page.
        /// </summary>
        /// <returns></returns>
        public PriceMonitoringVM GetData()
        {
            PriceMonitoringVM priceMonitoringVM = null;
            try
            {
                priceMonitoringVM = new PriceMonitoringVM();
                priceMonitoringVM.BikeMakes = _makesRepo.GetMakes((ushort)EnumBikeType.New);
                priceMonitoringVM.States = _locationRepo.GetStates();
                priceMonitoringVM.PageTitle = pageTitle;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "PriceMonitoringModel.getData");
            }
            return priceMonitoringVM;
        }


        /// <summary>
        /// Created By : Ashutosh Sharma on 09-08-2017
        /// Description: Method to get bikemakes, states and price last updated details for price monitoring page.
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="stateId"></param>
        /// <returns></returns>
        public PriceMonitoringVM GetData(uint makeId, uint stateId)
        {
            PriceMonitoringVM priceMonitoringVM = null;
            PriceMonitoringEntity objPriceMonitoring = null;

            try
            {
                if (makeId > 0 && stateId > 0)
                {
                    priceMonitoringVM = new PriceMonitoringVM();
                    priceMonitoringVM.BikeMakes = _makesRepo.GetMakes((ushort)EnumBikeType.New);
                    priceMonitoringVM.States = _locationRepo.GetStates();

                    priceMonitoringVM.MakeId = makeId;
                    priceMonitoringVM.StateId = stateId;

                    objPriceMonitoring = _pricesRepo.GetPriceMonitoringDetails(makeId, 0, stateId);

                    priceMonitoringVM.BikeModelList = Convert(objPriceMonitoring.BikeModelList);
                    priceMonitoringVM.PriceLastUpdatedList = objPriceMonitoring.PriceLastUpdatedList;
                    priceMonitoringVM.BikeVersionList = objPriceMonitoring.BikeVersionList;
                    priceMonitoringVM.CityList = objPriceMonitoring.CityList;

                    if (priceMonitoringVM.BikeMakes != null && priceMonitoringVM.States != null)
                    {
                        BikeMakeEntityBase bikeMake = priceMonitoringVM.BikeMakes.FirstOrDefault(c => c.MakeId == priceMonitoringVM.MakeId);
                        Entities.StateEntityBase state = priceMonitoringVM.States.FirstOrDefault(c => c.StateId == priceMonitoringVM.StateId);
                        if (bikeMake != null && state != null)
                        {
                            priceMonitoringVM.PageTitle = string.Format("{0} - {1} ({2})", pageTitle, bikeMake.MakeName, state.StateName);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("PriceMonitoringModel.getData._makeId:{0}_stateId:{1}", makeId, stateId));
            }
            return priceMonitoringVM;
        }

        /// <summary>
        /// Created By : Ashutosh Sharma on 09-08-2017
        /// Description: Method to get bikemakes, bikemodels, states and price last updated details for price monitoring page.
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="modelId"></param>
        /// <param name="stateId"></param>
        /// <returns></returns>
        public PriceMonitoringVM GetData(uint makeId, uint modelId, uint stateId)
        {
            PriceMonitoringVM priceMonitoringVM = null;
            PriceMonitoringEntity objPriceMonitoring = null;

            try
            {
                if (makeId > 0 && modelId > 0 && stateId > 0)
                {
                    priceMonitoringVM = new PriceMonitoringVM();
                    priceMonitoringVM.BikeMakes = _makesRepo.GetMakes((ushort)EnumBikeType.New);
                    priceMonitoringVM.States = _locationRepo.GetStates();

                    priceMonitoringVM.MakeId = makeId;
                    priceMonitoringVM.StateId = stateId;
                    priceMonitoringVM.ModelId = modelId;
                    objPriceMonitoring = _pricesRepo.GetPriceMonitoringDetails(makeId, modelId, stateId);

                    priceMonitoringVM.BikeModelList = Convert(objPriceMonitoring.BikeModelList);
                    priceMonitoringVM.PriceLastUpdatedList = objPriceMonitoring.PriceLastUpdatedList;
                    priceMonitoringVM.BikeVersionList = objPriceMonitoring.BikeVersionList;
                    priceMonitoringVM.CityList = objPriceMonitoring.CityList;

                    if (priceMonitoringVM.BikeMakes != null && priceMonitoringVM.States != null)
                    {
                        BikeMakeEntityBase bikeMake = priceMonitoringVM.BikeMakes.FirstOrDefault(c => c.MakeId == priceMonitoringVM.MakeId);
                        ModelBase bikeModel = priceMonitoringVM.BikeModelList.FirstOrDefault(c => c.ModelId == priceMonitoringVM.ModelId);
                        Entities.StateEntityBase state = priceMonitoringVM.States.FirstOrDefault(c => c.StateId == priceMonitoringVM.StateId);
                        if (bikeMake != null && bikeModel != null && state != null)
                        {
                            priceMonitoringVM.PageTitle = string.Format("{0} - {1} ({2}) - {3}", pageTitle, bikeMake.MakeName, bikeModel.ModelName, state.StateName);
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                ErrorClass.LogError(ex, string.Format("PriceMonitoringModel.getData._makeId:{0}_modelId:{1}_stateId:{2}", makeId, modelId, stateId));
            }
            return priceMonitoringVM;
        }

        /// <summary>
        /// Created By : Ashutosh Sharma on 11-08-2017
        /// Description : Auto map BikeModelEntityBase to ModelBase.
        /// </summary>
        /// <param name="objModels"></param>
        /// <returns></returns>
        private IEnumerable<ModelBase> Convert(IEnumerable<BikeModelEntityBase> objModels)
        {
            Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
            return Mapper.Map<IEnumerable<BikeModelEntityBase>, IEnumerable<ModelBase>>(objModels);
        }
    }
}