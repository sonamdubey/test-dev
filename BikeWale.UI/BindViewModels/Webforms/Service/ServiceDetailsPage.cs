
using Bikewale.BAL.ServiceCenter;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Cache.ServiceCenter;
using Bikewale.Common;
using Bikewale.DAL.BikeData;
using Bikewale.DAL.ServiceCenter;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.service;
using Bikewale.Entities.ServiceCenters;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.ServiceCenter;
using Microsoft.Practices.Unity;
using System;
namespace Bikewale.BindViewModels.Webforms.Service
{
    /// <summary>
    /// Created by : Sangram Nandkhile on 16 Nov 2016
    /// Desc: Created ViewModel Service Center details
    /// </summary>
    public class ServiceDetailsPage
    {
        public ServiceCenterCompleteData objServiceCenterData = null;
        public string MakeName = string.Empty,
                        MaskingNumber = string.Empty,
                        MakeMaskingName = string.Empty,
                        CityMaskingName = string.Empty,
                        CityName = string.Empty,
                        ServiceCity = string.Empty,
                        ClientIP = string.Empty;
        public uint campaignId, cityId, serviceCenterId, makeId;
        public BikeMakeEntityBase objBikeMakeEntityBase;

        public bool BindServiceCenterData(uint serviceCenterId)
        {
            bool isDataReturned = false;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IServiceCenter, ServiceCenter<ServiceCenterLocatorList, int>>()
                    .RegisterType<IServiceCenterCacheRepository, ServiceCenterCacheRepository>()
                    .RegisterType<IServiceCenterRepository<ServiceCenterLocatorList, int>, ServiceCenterRepository<ServiceCenterLocatorList, int>>()
                    .RegisterType<ICacheManager, MemcacheManager>();
                    var objServiceCenter = container.Resolve<IServiceCenter>();
                    objServiceCenterData = objServiceCenter.GetServiceCenterDataById(serviceCenterId);

                    if (objServiceCenterData != null)
                    {
                        CityName = objServiceCenterData.Name;
                        cityId = objServiceCenterData.CityId;
                        makeId = objServiceCenterData.MakeId;
                        MakeName = GetMakeNameByMakeId(objServiceCenterData.MakeId);
                        ServiceCity = objServiceCenterData.CityName;
                        CityMaskingName = objServiceCenterData.CityMaskingName;
                        ClientIP = CommonOpn.GetClientIP();
                        isDataReturned = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, string.Format("BindServiceCenterData for serviceCenterId : {0} ", serviceCenterId));
                objErr.SendMail();
            }
            return isDataReturned;
        }

        /// <summary>
        /// Created by : SAJAL GUPTA on 08-11-2016
        /// Description: Method to get make name by makeId.
        /// </summary>
        /// <param name="cityMaskingName"></param>
        private string GetMakeNameByMakeId(uint makeId)
        {
            string curMakename = string.Empty;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>();
                    var makesRepository = container.Resolve<IBikeMakes<BikeMakeEntity, int>>();
                    objBikeMakeEntityBase = makesRepository.GetMakeDetails(makeId.ToString());
                }

                if (objBikeMakeEntityBase != null)
                {
                    curMakename = objBikeMakeEntityBase.MakeName;
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "ServiceDetailsPage.GetMakeNameByMakeId()");
                objErr.SendMail();
            }
            return curMakename;
        }

        public MakeMaskingResponse GetMakeResponse(string makeMaskingName)
        {
            MakeMaskingResponse response = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeMakesCacheRepository<int>, BikeMakesCacheRepository<BikeMakeEntity, int>>()
                          .RegisterType<ICacheManager, MemcacheManager>()
                          .RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>()
                         ;
                    var objCache = container.Resolve<IBikeMakesCacheRepository<int>>();

                    response = objCache.GetMakeMaskingResponse(makeMaskingName);
                }
            }
            catch (Exception ex)
            {

                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "ServiceDetailsPage.GetMakeResponse()");
                objErr.SendMail();
            }
            return response;
        }
    }
}