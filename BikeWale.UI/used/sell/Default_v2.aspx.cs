using Bikewale.BAL.Customer;
using Bikewale.BAL.MobileVerification;
using Bikewale.BAL.UsedBikes;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.DAL.BikeData;
using Bikewale.DAL.Customer;
using Bikewale.DAL.Location;
using Bikewale.DAL.MobileVerification;
using Bikewale.DAL.Used;
using Bikewale.DTO.BikeBooking.Make;
using Bikewale.DTO.BikeBooking.Model;
using Bikewale.DTO.BikeBooking.Version;
using Bikewale.DTO.Customer;
using Bikewale.DTO.UsedBikes;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Customer;
using Bikewale.Entities.Location;
using Bikewale.Entities.Used;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Customer;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.MobileVerification;
using Bikewale.Interfaces.Used;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;

namespace Bikewale.Used.Sell
{
    public class Default_v2 : System.Web.UI.Page
    {
        protected IEnumerable<Bikewale.Entities.BikeData.BikeMakeEntityBase> objMakeList = null;
        private IBikeMakesCacheRepository<int> _makesRepository;
        protected List<CityEntityBase> objCityList = null;
        protected string userId = null;
        protected bool isEdit = false;
        protected ulong inquiryId = 0;
        protected bool isAuthorized = false;
        protected SellBikeAd inquiryDetailsObject = null;
        protected SellBikeAdDTO inquiryDTO;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindMakes();
            BindCities();
            BindUserId();
            CheckIsEdit();
        }

        /// <summary>
        /// Created By : Sajal Gupta on 20/10/2016
        /// Description : Function to bind userId.
        /// </summary>
        protected void BindUserId()
        {
            try
            {
                if (CurrentUser.Id != null)
                {
                    userId = CurrentUser.Id;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.used.sell.default.BindUserId()");
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Created By : Sajal Gupta on 20/10/2016
        /// Description : Function to bind makes.
        /// </summary>
        protected void BindMakes()
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeMakesCacheRepository<int>, BikeMakesCacheRepository<BikeMakeEntity, int>>()
                             .RegisterType<ICacheManager, MemcacheManager>()
                             .RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>()
                            ;
                    _makesRepository = container.Resolve<IBikeMakesCacheRepository<int>>();
                    objMakeList = _makesRepository.GetMakesByType(EnumBikeType.Used);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.used.sell.default.BindMakes()");
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Created By : Sajal Gupta on 20/10/2016
        /// Description : Function to bind Cities (both registered at and current city).
        /// </summary>
        protected void BindCities()
        {
            ICity _city = new CityRepository();
            try
            {
                objCityList = _city.GetAllCities(EnumBikeType.All);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.used.sell.default.BindCities()");
                objErr.SendMail();
            }
        }

        protected void CheckIsEdit()
        {
            try
            {
                if (Request.QueryString["id"] != null)
                {
                    isEdit = true;
                    inquiryId = Convert.ToUInt64(Request.QueryString["id"]);
                    GetInquiryDetails();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.used.sell.default.CheckIsEdit()");
                objErr.SendMail();
            }
        }

        protected void GetInquiryDetails()
        {
            try
            {
                ISellBikes obj;
                
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<ICustomerRepository<CustomerEntity, UInt32>, CustomerRepository<CustomerEntity, UInt32>>();
                    container.RegisterType<ICustomer<CustomerEntity, UInt32>, Customer<CustomerEntity, UInt32>>();
                    container.RegisterType<IMobileVerificationRepository, MobileVerificationRepository>();
                    container.RegisterType<IMobileVerification, MobileVerification>();                    
                    container.RegisterType<IUsedBikeBuyerRepository, UsedBikeBuyerRepository>();
                    container.RegisterType<ISellBikesRepository<SellBikeAd, int>, SellBikesRepository<SellBikeAd, int>>();
                    container.RegisterType<ISellBikes, SellBikes>();
                    obj = container.Resolve<ISellBikes>();
                }

                SellBikeAd inquiryDetailsObject = obj.GetById((int)inquiryId, Convert.ToUInt64(userId));
                inquiryDTO = ConvertToDto(inquiryDetailsObject);

                if (inquiryDetailsObject == null)
                {
                    isAuthorized = false;
                }
                else
                {
                    isAuthorized = true;
                }

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.used.sell.default.CheckIsCustomerAuthorized()");
                objErr.SendMail();
            }

        }

        private SellBikeAdDTO ConvertToDto(SellBikeAd inquiryDetailsObject)
        {
            AutoMapper.Mapper.CreateMap<Bikewale.Entities.BikeData.BikeMakeEntityBase, BBMakeBase>();
            AutoMapper.Mapper.CreateMap<Bikewale.Entities.BikeData.BikeModelEntityBase, BBModelBase>();
            AutoMapper.Mapper.CreateMap<BikeVersionEntityBase, BBVersionBase>();
            AutoMapper.Mapper.CreateMap<Bikewale.Entities.Used.SellAdStatus, Bikewale.DTO.UsedBikes.SellAdStatus>();
            AutoMapper.Mapper.CreateMap<SellBikeAdOtherInformation, SellBikeAdOtherInformationDTO>();
            AutoMapper.Mapper.CreateMap<SellerEntity, SellerDTO>();
            AutoMapper.Mapper.CreateMap<SellBikeAd, SellBikeAdDTO>();
            return AutoMapper.Mapper.Map<SellBikeAdDTO>(inquiryDetailsObject);
        }
    }
}