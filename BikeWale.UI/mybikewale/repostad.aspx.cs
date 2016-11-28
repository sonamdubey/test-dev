using Bikewale.BAL.Customer;
using Bikewale.BAL.MobileVerification;
using Bikewale.BAL.UsedBikes;
using Bikewale.Common;
using Bikewale.DAL.Customer;
using Bikewale.DAL.MobileVerification;
using Bikewale.DAL.Used;
using Bikewale.DTO.BikeBooking.Make;
using Bikewale.DTO.BikeBooking.Model;
using Bikewale.DTO.BikeBooking.Version;
using Bikewale.DTO.UsedBikes;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Customer;
using Bikewale.Entities.Used;
using Bikewale.Interfaces.Customer;
using Bikewale.Interfaces.MobileVerification;
using Bikewale.Interfaces.Used;
using Microsoft.Practices.Unity;
using System;
using System.Web;
using System.Web.UI.WebControls;

namespace Bikewale.MyBikewale
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 25 Nov 2016
    /// Summary: Repost ad page
    /// </summary>
    public class RepostSellBikeAd : System.Web.UI.Page
    {
        protected SellBikeAdDTO inquiryDTO;
        protected int inquiryId = 0;
        protected UInt64 userId;
        protected bool isAuthorized = false, isReposted = false;
        protected Button btnRepost;
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
            if (btnRepost != null)
                btnRepost.Click += new EventHandler(btnRepost_Click);
        }

        private void btnRepost_Click(object sender, EventArgs e)
        {
            IUsedBikeSeller objRepost;
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IUsedBikeSellerRepository, UsedBikeSellerRepository>();
                container.RegisterType<IUsedBikeSeller, UsedBikeSeller>();
                objRepost = container.Resolve<IUsedBikeSeller>();
                if (objRepost.RepostSellBikeAd(inquiryId, userId))
                {
                    isReposted = true;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Form.Action = Request.RawUrl;
            if (ProcessQueryString())
            {
                GetInquiryDetails(inquiryId);
            }
        }

        /// <summary>
        /// Created By : Sajal Gupta on 22/10/2016
        /// Description : Function to fetch inquiryDetails.
        /// </summary>
        protected bool GetInquiryDetails(int inquiryId)
        {
            isAuthorized = false;
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
                    container.RegisterType<IUsedBikeSellerRepository, UsedBikeSellerRepository>();
                    container.RegisterType<ISellBikes, SellBikes>();
                    obj = container.Resolve<ISellBikes>();
                    if (obj != null)
                    {
                        SellBikeAd inquiryDetailsObject = obj.GetById(inquiryId, userId);
                        if (inquiryDetailsObject != null)
                        {
                            inquiryDTO = ConvertToDto(inquiryDetailsObject);
                            if (inquiryDTO != null)
                                inquiryDTO.ManufacturingYear = (DateTime)inquiryDTO.ManufacturingYear;
                        }
                        isAuthorized = inquiryDetailsObject == null ? false : true;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.used.sell.default.CheckIsCustomerAuthorized()");
                objErr.SendMail();
            }
            return isAuthorized;
        }

        /// <summary>
        /// Created By : Sajal Gupta on 24/10/2016
        /// Description : Function to convert entity to DTO.
        /// </summary>
        private SellBikeAdDTO ConvertToDto(SellBikeAd inquiryDetailsObject)
        {
            AutoMapper.Mapper.CreateMap<Bikewale.Entities.BikeData.BikeMakeEntityBase, BBMakeBase>();
            AutoMapper.Mapper.CreateMap<Bikewale.Entities.BikeData.BikeModelEntityBase, BBModelBase>();
            AutoMapper.Mapper.CreateMap<BikeVersionEntityBase, BBVersionBase>();
            AutoMapper.Mapper.CreateMap<Bikewale.Entities.Used.SellAdStatus, Bikewale.DTO.UsedBikes.SellAdStatus>();
            AutoMapper.Mapper.CreateMap<SellBikeAdOtherInformation, SellBikeAdOtherInformationDTO>();
            AutoMapper.Mapper.CreateMap<SellerEntity, SellerDTO>();
            AutoMapper.Mapper.CreateMap<BikePhoto, Bikewale.DTO.Used.Search.BikePhoto>();
            AutoMapper.Mapper.CreateMap<SellBikeAd, SellBikeAdDTO>();
            return AutoMapper.Mapper.Map<SellBikeAdDTO>(inquiryDetailsObject);
        }

        /// <summary>
        /// Created By:  Sangram Nandkhile 25 Nov 2016
        /// Description : Process Query String
        /// </summary>
        private bool ProcessQueryString()
        {
            // Check if user is logged in or not
            if (CurrentUser.UserId > 0)
            {
                userId = CurrentUser.UserId;
            }
            else // If user is not logged in, redirect user to login page
            {
                NotAuthorizedRedirect();
                return false;
            }
            try
            {
                string strInquiryId = Request.QueryString["id"];
                if (!string.IsNullOrEmpty(strInquiryId))
                {
                    int.TryParse(strInquiryId, out inquiryId);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "RepostSellBikeAd.ProcessQueryString()");
                objErr.SendMail();
            }
            return false;
        }

        private void NotAuthorizedRedirect()
        {
            Response.Redirect(String.Format("/users/login.aspx?ReturnUrl=/used/inquiry/{0}/repost/", inquiryId));
            HttpContext.Current.ApplicationInstance.CompleteRequest();
            this.Page.Visible = false;
        }
    }
}