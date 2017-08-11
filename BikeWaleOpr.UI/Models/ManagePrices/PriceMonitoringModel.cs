using Bikewale.Notifications;
using BikewaleOpr.DALs.Location;
using BikewaleOpr.Entities.BikeData;
using BikewaleOpr.Entity.BikePricing;
using BikewaleOpr.Interface.BikeData;
using BikewaleOpr.Interface.BikePricing;
using BikewaleOpr.Interface.Location;
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
                priceMonitoringVM.BikeMakes = _makesRepo.GetMakes("NEW");
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
            try
            {
                if (makeId > 0 && stateId > 0)
                {
                    priceMonitoringVM = new PriceMonitoringVM();
                    priceMonitoringVM.BikeMakes = _makesRepo.GetMakes("NEW");
                    priceMonitoringVM.States = _locationRepo.GetStates();

                    priceMonitoringVM.MakeId = makeId;
                    priceMonitoringVM.StateId = stateId;
                    priceMonitoringVM.PriceMonitoringEntity = _pricesRepo.GetPriceMonitoringDetails(makeId, 0, stateId);

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
            try
            {
                if (makeId > 0 && modelId > 0 && stateId > 0)
                {
                    priceMonitoringVM = new PriceMonitoringVM();
                    priceMonitoringVM.BikeMakes = _makesRepo.GetMakes("NEW");
                    priceMonitoringVM.States = _locationRepo.GetStates();

                    priceMonitoringVM.MakeId = makeId;
                    priceMonitoringVM.StateId = stateId;
                    priceMonitoringVM.ModelId = modelId;
                    priceMonitoringVM.PriceMonitoringEntity = _pricesRepo.GetPriceMonitoringDetails(makeId, modelId, stateId);

                    if (priceMonitoringVM.BikeMakes != null && priceMonitoringVM.States != null)
                    {
                        BikeMakeEntityBase bikeMake = priceMonitoringVM.BikeMakes.FirstOrDefault(c => c.MakeId == priceMonitoringVM.MakeId);
                        BikeModelEntityBase bikeModel =  priceMonitoringVM.PriceMonitoringEntity.BikeModelList.FirstOrDefault(c => c.Id == priceMonitoringVM.ModelId);
                        Entities.StateEntityBase state = priceMonitoringVM.States.FirstOrDefault(c => c.StateId == priceMonitoringVM.StateId);
                        if (bikeMake != null && bikeModel != null &&state != null)
                        {
                            priceMonitoringVM.PageTitle = string.Format(pageTitle + " - {0} ({1}) - {2}", bikeMake.MakeName, bikeModel.Name, state.StateName);
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
    }
}