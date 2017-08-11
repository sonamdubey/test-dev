﻿
using AutoMapper;
using Bikewale.Notifications;
using BikewaleOpr.DALs.Location;
using BikewaleOpr.DTO.BikeData;
using BikewaleOpr.Entities.BikeData;
using BikewaleOpr.Entity.BikeData;
using BikewaleOpr.Entity.BikePricing;
using BikewaleOpr.Interface.BikeData;
using BikewaleOpr.Interface.BikePricing;
using BikewaleOpr.Interface.Location;
using BikewaleOpr.Service.AutoMappers.BikeData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BikewaleOpr.Models.ManagePrices
{
    /// <summary>
    /// Created By: Ashutosh Sharma on 31-07-2017
    /// Discription : Model for Price monitoring report page.
    /// </summary>
    public class PriceMonitoringModel
    {
        private readonly IBikeMakes _makesRepo = null;
        private readonly IShowroomPricesRepository _pricesRepo = null;
        private readonly ILocation _locationRepo = null;
        string pageTitle = "Price Monitoring Report";

        public PriceMonitoringModel(IBikeMakes makesRepo, IShowroomPricesRepository pricesRepo, ILocation locationRepo)
        {
            _makesRepo = makesRepo;
            _pricesRepo = pricesRepo;
            _locationRepo = locationRepo;
        }

        public PriceMonitoringVM getData()
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
                ErrorClass objErr = new ErrorClass(ex, "PriceMonitoringModel.getData");
            }
            return priceMonitoringVM;
        }


        public PriceMonitoringVM getData(uint makeId, uint stateId)
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
                        BikeMakeEntityBase bikeMake =  priceMonitoringVM.BikeMakes.FirstOrDefault(c => c.MakeId == priceMonitoringVM.MakeId);
                        Entities.StateEntityBase state =  priceMonitoringVM.States.FirstOrDefault(c => c.StateId == priceMonitoringVM.StateId);
                        if (bikeMake != null && state != null)
                        {
                            priceMonitoringVM.PageTitle = string.Format(pageTitle + "- {0} ({1})", bikeMake.MakeName, state.StateName);
                        } 
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("PriceMonitoringModel.getData._makeId:{0}_stateId:{1}", makeId, stateId));
            }
            return priceMonitoringVM;
        }

        public PriceMonitoringVM getData(uint makeId, uint modelId, uint stateId)
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
                        ModelBase bikeModel =  priceMonitoringVM.BikeModelList.FirstOrDefault(c => c.ModelId == priceMonitoringVM.ModelId);
                        Entities.StateEntityBase state = priceMonitoringVM.States.FirstOrDefault(c => c.StateId == priceMonitoringVM.StateId);
                        if (bikeMake != null && bikeModel != null &&state != null)
                        {
                            priceMonitoringVM.PageTitle = string.Format(pageTitle + " - {0} ({1}) - {2}", bikeMake.MakeName, bikeModel.ModelName, state.StateName);
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, string.Format("PriceMonitoringModel.getData._makeId:{0}_modelId:{1}_stateId:{2}", makeId, modelId,stateId));
            }
            return priceMonitoringVM;
        }

        private IEnumerable<ModelBase> Convert(IEnumerable<BikeModelEntityBase> objModels)
        {
            Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
            return Mapper.Map<IEnumerable<BikeModelEntityBase>, IEnumerable<ModelBase>>(objModels);
        }
    }
}