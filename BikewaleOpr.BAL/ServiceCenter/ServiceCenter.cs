using Bikewale.Interfaces.BikeData;
using BikewaleOpr.Entities;
using BikewaleOpr.Entity.ServiceCenter;
using BikewaleOpr.Interface.BikeData;
using BikewaleOpr.Interface.ServiceCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.BAL.ServiceCenter
{
       /// <summary>		
    /// Created By:-Snehal Dange 28 July 2017		
    /// Summary:- For service center related operations		
    /// </summary>		
    public class ServiceCenter : IServiceCenter		
    {		
        private IBikeMakes _IBikeMake;		
        private IServiceCenterRepository _IServiceCenter;		
        public ServiceCenter(IBikeMakes bikeMake, IServiceCenterRepository serviceCenter)
        {		
            _IBikeMake = bikeMake;		
            _IServiceCenter = serviceCenter;		
        }		
		
		
        ///// <summary>		
        ///// Created By:-Snehal Dange 27 July 2017		
        ///// Summary:- Get bikes Make list 		
        ///// </summary>		
        ///// <param name="requestType"></param>		
        ///// <returns></returns>		
        //public IEnumerable<Entities.BikeData.BikeMakeEntityBase> GetBikeMakes(ushort requestType)
        //{		
		
        //    IEnumerable<Entities.BikeData.BikeMakeEntityBase> objMakesList = null;		
        //    try		
        //    {		
                		
        //        if(_IBikeMake != null && requestType > 0)		
        //        {		
        //            objMakesList = _IBikeMake.GetServiceCenterMakes(requestType);		
        //        }		
        //    }		
		
        //    catch (Exception ex)		
        //    {		
        //        Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "BikewaleOpr.BAL.ServiceCenterBL.GetBikeMakes");		
        //    }		
		
		
        //    return objMakesList;		
		
        //}		
		
        /// <summary>		
        /// Created By:-Snehal Dange 28 July 2017		
        /// Summary:- Get service centers cities according to makeid		
        /// </summary>		
        /// <param name="makeId"></param>		
        /// <returns></returns>		
        public IEnumerable<CityEntityBase> GetServiceCenterCities(uint makeId)
        {		
            IEnumerable<CityEntityBase> objCityList = null;		
            try		
            {		
                if (_IServiceCenter != null && makeId > 0)		
                {		
                    objCityList = _IServiceCenter.GetServiceCenterCities(makeId);		
                }		
            }		
            catch (Exception ex)		
            {		
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "BikewaleOpr.BAL.ServiceCenterBL.GetServiceCenterCities");		
               		
            }		
            return objCityList;		
        }		
		
		
        /// <summary>		
        /// Created By:-Snehal Dange 28 July 2017		
        /// Summary:- Fetch all Service Center data by make and city from Dal		
        /// </summary>		
        /// <param name="cityId"></param>		
        /// <param name="makeId"></param>		
        /// <returns></returns>		
        public ServiceCenterData GetServiceCentersByCityMake(uint cityId, uint makeId, sbyte activeStatus)
        {		
            ServiceCenterData objServiceCenterData = null;		
            try		
            {		
                if (_IServiceCenter != null && cityId > 0 && makeId > 0)		
                {		
                    objServiceCenterData = _IServiceCenter.GetServiceCentersByCityMake(cityId, makeId , activeStatus);		
                }		
            }		
            catch (Exception ex)		
            {		
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "BikewaleOpr.BAL.ServiceCenterBL.GetServiceCentersByCity");		
                		
            }		
            return objServiceCenterData;		
        }		
		
        /// <summary>		
        /// Created By:-Snehal Dange 29 July 2017		
        /// Summary:- Create new service center , passes service center details to Dal		
        /// </summary>		
        /// <returns></returns>		
        public bool AddUpdateServiceCenter(ServiceCenterCompleteData serviceCenterDetails, string updatedBy)
        {		
            bool status = false;		
          		
            try		
            {		
                if(_IServiceCenter != null && serviceCenterDetails !=null)		
                {		
                   status = _IServiceCenter.AddUpdateServiceCenter(serviceCenterDetails, updatedBy);		
                    		
                 		
                }		
            }		
            catch (Exception ex)		
            {		
		
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "BikewaleOpr.BAL.ServiceCenterBL.AddServiceCenter()");		
		
            }		
            return status;		
        }		
		
		
        /// <summary>		
        /// Created By:-Snehal Dange 29 July 2017		
        /// Summary:- Calls Dal layer to delete service center details		
        /// </summary>		
        /// <param name="serviceCenterId"></param>		
        /// <returns></returns>		
        public bool UpdateServiceCenterStatus(uint serviceCenterId, string currentUserId)
        {		
            bool status = false;		
            try		
            {		
                if(_IServiceCenter != null && serviceCenterId > 0)		
                {		
                    status = _IServiceCenter.UpdateServiceCenterStatus(serviceCenterId, currentUserId);		
                }		
		
            }		
            catch (Exception ex)		
            {		
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "BikewaleOpr.BAL.ServiceCenterBL.DeleteServiceCenter()");		
		
            }		
            return status;		
        }		
		
        /// <summary>		
        /// Created By:-Snehal Dange 29 July 2017		
        /// Summary:- Calls Dal layer to get all cities		
        /// </summary>		
      		
        /// <returns></returns>		
        public IEnumerable<CityEntityBase> GetAllCities()
        {		
            IEnumerable<CityEntityBase> objCityList = null;		
            try		
            {		
                if (_IServiceCenter != null )		
                {		
                    objCityList = _IServiceCenter.GetAllCities();		
                }		
            }		
            catch (Exception ex)		
            {		
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "BikewaleOpr.BAL.ServiceCenterBL.GetAllCities()");		
               		
            }		
            return objCityList;		
        }		
		
		
        /// <summary>		
        /// Created By:-Snehal Dange 3 August 2017		
        /// Summary:- Get service center details by Service center Id		
        /// </summary>		
        /// <param name="serviceCenterId"></param>		
        /// <returns></returns>		
		
        public ServiceCenterCompleteData GetServiceCenterDetailsbyId(uint serviceCenterId)
        {		
            ServiceCenterCompleteData objData = new ServiceCenterCompleteData();		
            try		
            {		
		
                if (_IServiceCenter != null)		
                {		
                    objData = _IServiceCenter.GetDataById(serviceCenterId);		
                }		
		
            }		
            catch (Exception ex)		
            {		
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "BikewaleOpr.BAL.ServiceCenterBL.GetServiceCenterDetailsbyId()");		
               		
            }		
            return objData;		
        }		
		
		
		
        /// <summary>		
        /// Created By:-Snehal Dange 1st August 2017		
        /// Summary:- Calls Dal layer to get state details for that city		
        /// </summary>		
		
        public StateCityEntity GetStateDetailsByCity(uint cityId)
        {		
            StateCityEntity objData = new StateCityEntity();		
            try		
            {		
                if(_IServiceCenter != null && cityId>0)		
                {		
                    objData = _IServiceCenter.GetStateDetails(cityId);		
                }		
            }		
            catch (Exception ex)		
            {		
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "BikewaleOpr.BAL.ServiceCenterBL.GetStateDetailsByCity()");		
		
            }		
            return objData;		
        }		
    }
}
