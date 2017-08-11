using Bikewale.Entities.BikeData;
using Bikewale.Notifications;
using BikewaleOpr.Entities;
using BikewaleOpr.Entity.ServiceCenter;
using BikewaleOpr.Interface.BikeData;
using BikewaleOpr.Interface.ServiceCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BikewaleOpr.Models.ServiceCenter
{
    /// <summary>		
    /// Written By : Snehal Dange  on 27 July 2017		
    /// Summary : Model for Service Center Page in Opr		
    /// </summary>		
    public class ServiceCenterPageModel
    {
        ServiceCenterPageVM viewModel = null;

        StateCityEntity objStateCityData = null;
        private readonly IServiceCenter _serviceCenter = null;
        private readonly IBikeMakes _makes = null;


        public ServiceCenterPageModel(IServiceCenter serviceCenter)
        {
            _serviceCenter = serviceCenter;
        }

        public ServiceCenterPageModel(IServiceCenter serviceCenter, IBikeMakes makes )
        {
            _serviceCenter = serviceCenter;
            _makes = makes;

        }

        /// <summary>		
        /// <summary>		
        /// Written By : Snehal Dange  on 27 July 2017		
        /// Summary : GetData() to get list of makes and city to be bind on front page		
        /// </summary>		
        /// </summary>		
        /// <returns></returns>		
        public ServiceCenterPageVM GetData()
        {
            viewModel = new ServiceCenterPageVM();
            viewModel.Makes = GetBikeMakes();
            viewModel.AllCityList = GetAllCityList();
            return viewModel;
        }



        /// <summary>		
        /// <summary>		
        /// Written By : Snehal Dange  on 27 July 2017		
        /// Summary : GetFormData() returns city and state data which need to be loaded when form is displayed		
        /// </summary>		
        /// <param name="cityId"></param>		
        /// <param name="makeId"></param>		
        /// <param name="makeName"></param>		
        /// <returns></returns>		
        public ServiceCenterCompleteDataVM GetFormData(uint cityId, uint makeId, string makeName)
        {
            ServiceCenterCompleteDataVM formModel = new ServiceCenterCompleteDataVM();
            objStateCityData = null;
            try
            {
                objStateCityData = GetStateDetailsByCityId(cityId);

                if (cityId > 0 && makeId > 0)
                {
                    formModel.MakeName = makeName;
                    formModel.details.MakeId = makeId;
                    formModel.details.Location = objStateCityData;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikewaleOpr.Models.ServiceCenter.ServiceCenterPageModel.GetFormData()");
            }
            return formModel;
        }

        /// <summary>		
        /// <summary>		
        /// Written By : Snehal Dange  on 28 July 2017		
        /// Summary : GetBikeMakes() returns list of MakeName and makeId .		
        /// </summary>		
        /// <returns></returns>		
        public IEnumerable<Entities.BikeData.BikeMakeEntityBase> GetBikeMakes()
        {
            IEnumerable<Entities.BikeData.BikeMakeEntityBase> serviceCenterMakeList = null;

            try
            {
                if (_serviceCenter != null)
                {
                    serviceCenterMakeList = _makes.GetMakes((ushort)EnumBikeType.ServiceCenter);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikewaleOpr.Models.ServiceCenter.ServiceCenterPageModel.GetBikeMakes()");
            }

            return serviceCenterMakeList;


        }

        /// <summary>		
        /// <summary>		
        /// Written By : Snehal Dange  on 28 July 2017		
        /// Summary : GetAllCityList() returns list of all cities		
        /// </summary>		
        /// <returns></returns>		
        public IEnumerable<CityEntityBase> GetAllCityList()
        {
            IEnumerable<CityEntityBase> allCityList = null;
            try
            {
                if (_serviceCenter != null)
                {
                    allCityList = _serviceCenter.GetAllCities();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikewaleOpr.Models.ServiceCenter.ServiceCenterPageModel.GetAllCityList()");
            }

            return allCityList;


        }


        /// <summary>		
        /// <summary>		
        /// Written By : Snehal Dange  on 29 July 2017		
        /// Summary : GetServiceCenterDetailsById() returns all service center details while editing the form		
        /// </summary>		
        /// <param name="ServiceCenterId"></param>		
        /// <returns></returns>		
        public ServiceCenterCompleteData GetServiceCenterDetailsById(uint ServiceCenterId)
        {
            ServiceCenterCompleteData details = null;

            try
            {
                details = _serviceCenter.GetServiceCenterDetailsbyId(ServiceCenterId);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikewaleOpr.Models.ServiceCenter.ServiceCenterPageModel.GetServiceCenterDetailsById()");
            }


            return details;
        }


        /// <summary>		
        /// <summary>		
        /// Written By : Snehal Dange  on 1 August 2017		
        /// Summary : GetStateDetailsByCityId() gives State and City details for a CityId		
        /// </summary>		
        /// <param name="cityId"></param>		
        /// <returns></returns>		
        public StateCityEntity GetStateDetailsByCityId(uint cityId)
        {
            StateCityEntity objModel = null;
            try
            {

                if (_serviceCenter != null && cityId > 0)
                {
                    objModel = _serviceCenter.GetStateDetailsByCity(cityId);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikewaleOpr.Models.ServiceCenter.ServiceCenterPageModel.GetStateDetailsByCityId()");
            }

            return objModel;
        }
    }
}