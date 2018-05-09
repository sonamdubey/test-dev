
using Bikewale.DTO.PriceQuote.BikeBooking;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.Customer;
using Bikewale.Entities.PriceQuote;
using Bikewale.Entities.Dealer;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.Customer;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.Lead;
using Bikewale.Interfaces.MobileVerification;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Notifications;
using Bikewale.ManufacturerCampaign.Interface;
using RabbitMqPublishing;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;
using Bikewale.ManufacturerCampaign.Entities;

namespace Bikewale.BAL.Lead
{
    /// <summary>
    /// Created by : Snehal Dange on 2nd May 2018
    /// Description : To manage dealer and manufacture leads related methods
    /// </summary>
    public class LeadProcess : ILead
    {
        private readonly ICustomerAuthentication<CustomerEntity, UInt32> _objAuthCustomer = null;
        private readonly ICustomer<CustomerEntity, UInt32> _objCustomer = null;
        private readonly IDealerPriceQuote _objDealerPriceQuote = null;
        private readonly IMobileVerificationRepository _mobileVerRespo = null;
        private readonly IMobileVerificationCache _mobileVerCacheRepo = null;
        private readonly IMobileVerification _mobileVerification = null;
        private readonly IDealer _objDealer = null;
        private readonly IPriceQuote _objPriceQuote = null;
        private readonly ILeadNofitication _objLeadNofitication = null;
        private readonly Bikewale.Interfaces.AutoBiz.IDealers _objAutobizDealer = null;
        private readonly IManufacturerCampaignRepository _manufacturerCampaignRepo = null;
        public bool IsPQCustomerDetailWithPQ { get; set; }
        CustomerEntity objCust = null;


        public LeadProcess(
            ICustomerAuthentication<CustomerEntity, UInt32> objAuthCustomer,
            ICustomer<CustomerEntity, UInt32> objCustomer,
            IDealerPriceQuote objDealerPriceQuote,
            IMobileVerificationRepository mobileVerRespo,
            IMobileVerification mobileVerificetion,
            IDealer objDealer,
            IPriceQuote objPriceQuote, ILeadNofitication objLeadNofitication, IMobileVerificationCache mobileVerCacheRepo, Bikewale.Interfaces.AutoBiz.IDealers objAutobizDealer, IManufacturerCampaignRepository manufacturerCampaignRepo)
        {
            _objAuthCustomer = objAuthCustomer;
            _objCustomer = objCustomer;
            _objDealerPriceQuote = objDealerPriceQuote;
            _mobileVerRespo = mobileVerRespo;
            _mobileVerification = mobileVerificetion;
            _objDealer = objDealer;
            _objPriceQuote = objPriceQuote;
            _objLeadNofitication = objLeadNofitication;
            _mobileVerCacheRepo = mobileVerCacheRepo;
            _objAutobizDealer = objAutobizDealer;
            _manufacturerCampaignRepo = manufacturerCampaignRepo;
        }


        public PQCustomerDetailOutputEntity ProcessPQCustomerDetailInput(Entities.PriceQuote.PQCustomerDetailInput pqInput, System.Collections.Specialized.NameValueCollection requestHeaders)
        {
            PriceQuoteParametersEntity pqParam = null;
            DPQ_SaveEntity entity = null;
            BookingPageDetailsEntity objBookingPageDetailsEntity = null;
            PQCustomerDetail pqCustomer = null;
            PQCustomerDetailOutputEntity pqCustomerDetailEntity = null;
            bool isSuccess = false;
            sbyte noOfAttempts = 0;
            try
            {
                if (pqInput != null && (pqInput.PQId > 0) && (Convert.ToUInt32(pqInput.VersionId) > 0))
                {
                    pqParam = new PriceQuoteParametersEntity();
                    pqParam.VersionId = Convert.ToUInt32(pqInput.VersionId);

                    _objPriceQuote.UpdatePriceQuote(pqInput.PQId, pqParam);
                    entity = CheckRegisteredUser(pqInput, requestHeaders);

                    isSuccess = _objDealerPriceQuote.SaveCustomerDetail(entity);

                    var numberList = _mobileVerCacheRepo.GetBlockedNumbers();

                    if (numberList != null && !numberList.Contains(pqInput.CustomerMobile))
                    {
                        objBookingPageDetailsEntity = _objDealerPriceQuote.FetchBookingPageDetails(Convert.ToUInt32(pqInput.CityId), Convert.ToUInt32(pqInput.VersionId), pqInput.DealerId);

                        pqCustomer = _objDealerPriceQuote.GetCustomerDetails(pqInput.PQId);
                        objCust = pqCustomer.objCustomerBase;

                        pqCustomerDetailEntity = NotifyCustomerAndDealer(pqInput, requestHeaders);
                        pqCustomerDetailEntity.Dealer = objBookingPageDetailsEntity.Dealer;
                        pqCustomerDetailEntity.NoOfAttempts = noOfAttempts;

                    }


                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.BAL.Lead.LeadProcess.ProcessPQCustomerDetailInput"));
            }
            return pqCustomerDetailEntity;
        }

     
       public PQCustomerDetailOutputEntity ProcessPQCustomerDetailInputV1(PQCustomerDetailInput pqInput, System.Collections.Specialized.NameValueCollection requestHeaders)
        {
            PriceQuoteParametersEntity objPQEntity = null;
            DPQ_SaveEntity entity = null;
            Bikewale.DTO.PriceQuote.v2.PQCustomerDetailOutput output = null;
            PQCustomerDetailOutputEntity pqCustomerDetailEntity = null;
            bool isSuccess = false;
            bool isVerified = false;
            string password = string.Empty, salt = string.Empty, hash = string.Empty;
            string bikeName = String.Empty;
            string imagePath = String.Empty;
            string versionName = string.Empty;

            CustomerEntity objCust = null;
            PQCustomerDetail pqCustomer = null;
            sbyte noOfAttempts = -1;
            UInt64 pqId = default(UInt64);

            try
            {
                if (pqInput != null && !String.IsNullOrEmpty(pqInput.CustomerEmail) && !String.IsNullOrEmpty(pqInput.CustomerMobile))
                {
                    objPQEntity = new PriceQuoteParametersEntity();
                    objPQEntity.CityId = Convert.ToUInt32(pqInput.CityId);

                    if (requestHeaders != null)
                    {
                        string platformId = requestHeaders["platformId"];
                        if (platformId == "3")
                        {
                            objPQEntity.SourceId = Convert.ToUInt16(Bikewale.DTO.PriceQuote.PQSources.Android);
                            objPQEntity.DeviceId = pqInput.DeviceId;
                        }
                    }

                    objPQEntity.DeviceId = pqInput.DeviceId;
                    objPQEntity.PQLeadId = pqInput.LeadSourceId;
                    objPQEntity.VersionId = Convert.ToUInt32(pqInput.VersionId);
                    objPQEntity.DealerId = pqInput.DealerId;
                    pqId = _objPriceQuote.RegisterPriceQuote(objPQEntity);
                    entity = CheckRegisteredUser(pqInput, requestHeaders);

                    isSuccess = _objDealerPriceQuote.SaveCustomerDetail(entity);

                    var numberList = _mobileVerCacheRepo.GetBlockedNumbers();

                    if (numberList != null && !numberList.Contains(pqInput.CustomerMobile))
                    {
                        //Don't mark mobile verified for pq
                        //isVerified = _objDealerPriceQuote.UpdateIsMobileVerified(input.PQId);
                        isVerified = true;// Set Verified to true to push the lead into AB for un-verified leads as well
                        pqCustomer = _objDealerPriceQuote.GetCustomerDetails(Convert.ToUInt32(pqId));
                        objCust = pqCustomer.objCustomerBase;

                        pqCustomerDetailEntity = NotifyCustomerAndDealer(pqInput, requestHeaders);
                        pqCustomerDetailEntity.NoOfAttempts = noOfAttempts;
                        pqCustomerDetailEntity.PQId = pqId;

                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.BAL.Lead.LeadProcess.ProcessPQCustomerDetailInputV1"));
            }

            return pqCustomerDetailEntity;
        }

        /// <summary>
        /// Modified by : Sanskar Gupta on 09 May 2018
        /// Description : Optimized the code using object caching and added null checks wherever required.
        /// </summary>
        /// <param name="pqInput"></param>
        /// <param name="requestHeaders"></param>
        /// <returns></returns>
        private PQCustomerDetailOutputEntity NotifyCustomerAndDealer(Bikewale.Entities.PriceQuote.PQCustomerDetailInput pqInput, System.Collections.Specialized.NameValueCollection requestHeaders)
        {
            PQCustomerDetailOutputEntity output = null;
            try
            {
                PQ_DealerDetailEntity dealerDetailEntity = null;

                string apiValue = string.Empty;
                uint exShowroomCost = 0;
                UInt32 TotalPrice = 0;
                uint bookingAmount = 0;
                string bikeName = String.Empty;
                string imagePath = String.Empty;
                string versionName = string.Empty;
                bool isVerified = true;// Set Verified to true to push the lead into AB for un-verified leads as well


                PQParameterEntity objParam = new PQParameterEntity();
                objParam.CityId = Convert.ToUInt32(pqInput.CityId);
                objParam.DealerId = Convert.ToUInt32(pqInput.DealerId);
                objParam.VersionId = Convert.ToUInt32(pqInput.VersionId);
                dealerDetailEntity = _objAutobizDealer.GetDealerDetailsPQ(objParam);


                if (dealerDetailEntity != null && dealerDetailEntity.objQuotation != null)
                {
                    if (dealerDetailEntity.objBookingAmt != null)
                    {
                        bookingAmount = dealerDetailEntity.objBookingAmt.Amount;
                    }

                    bool isShowroomPriceAvail = false, isBasicAvail = false;

                    foreach (var item in dealerDetailEntity.objQuotation.PriceList)
                    {
                        //Check if Ex showroom price for a bike is available CategoryId = 3 (exshowrrom)
                        if (item.CategoryId == 3)
                        {
                            isShowroomPriceAvail = true;
                            exShowroomCost = item.Price;
                        }

                        //if Ex showroom price for a bike is not available  then set basic cost for bike price CategoryId = 1 (basic bike cost)
                        if (!isShowroomPriceAvail && item.CategoryId == 1)
                        {
                            exShowroomCost += item.Price;
                            isBasicAvail = true;
                        }

                        if (item.CategoryId == 2 && !isShowroomPriceAvail)
                            exShowroomCost += item.Price;

                        TotalPrice += item.Price;
                    }

                    if (isBasicAvail && isShowroomPriceAvail)
                        TotalPrice = TotalPrice - exShowroomCost;

                    imagePath = Bikewale.Utility.Image.GetPathToShowImages(dealerDetailEntity.objQuotation.OriginalImagePath, dealerDetailEntity.objQuotation.HostUrl, Bikewale.Utility.ImageSize._210x118);
                    bikeName = dealerDetailEntity.objQuotation.objMake.MakeName + " " + dealerDetailEntity.objQuotation.objModel.ModelName + " " + dealerDetailEntity.objQuotation.objVersion.VersionName;
                    versionName = dealerDetailEntity.objQuotation.objVersion.VersionName;
                    var platformId = "";
                    if (requestHeaders != null)
                    {
                        platformId = requestHeaders["platformId"];
                    }

                    apiValue = (IsPQCustomerDetailWithPQ ? "api/PQCustomerDetail" : "api/v2/PQCustomerDetail");

                    NewBikeDealers dealer = dealerDetailEntity.objDealer;

                    DPQSmsEntity objDPQSmsEntity = new DPQSmsEntity();
                    objDPQSmsEntity.CustomerMobile = objCust.CustomerMobile;
                    objDPQSmsEntity.CustomerName = objCust.CustomerName;
                    objDPQSmsEntity.DealerMobile = dealer != null ? dealer.PhoneNo : string.Empty;
                    objDPQSmsEntity.DealerName = dealer != null ? dealer.Organization : string.Empty;
                    objDPQSmsEntity.Locality = dealer != null ? dealer.Address : string.Empty;
                    objDPQSmsEntity.BookingAmount = bookingAmount;
                    objDPQSmsEntity.BikeName = String.Format("{0} {1} {2}", dealerDetailEntity.objQuotation.objMake.MakeName, dealerDetailEntity.objQuotation.objModel.ModelName, dealerDetailEntity.objQuotation.objVersion.VersionName);
                    objDPQSmsEntity.DealerArea = dealer != null && dealer.objArea != null ? dealer.objArea.AreaName : string.Empty;
                    objDPQSmsEntity.DealerAdd = dealer != null ? dealer.Address : string.Empty;
                    objDPQSmsEntity.DealerCity = dealer != null ? dealer.objCity.CityName : string.Empty;
                    objDPQSmsEntity.OrganisationName = dealer != null ? dealer.Organization : string.Empty;

                    if (dealer != null)
                    {
                        _objLeadNofitication.NotifyCustomer(pqInput.PQId, bikeName, imagePath, dealer.Name,
                           dealer.EmailId, dealer.PhoneNo, dealer.Organization,
                           dealer.Address, objCust.CustomerName, objCust.CustomerEmail,
                           dealerDetailEntity.objQuotation.PriceList, dealerDetailEntity.objOffers, dealer.objArea.PinCode,
                           dealer.objState.StateName, dealer.objCity.CityName, TotalPrice, objDPQSmsEntity,
                           apiValue, pqInput.LeadSourceId, versionName, dealer.objArea.Latitude, dealer.objArea.Longitude,
                           dealer.WorkingTime, platformId);

                        _objLeadNofitication.NotifyDealer(pqInput.PQId, dealerDetailEntity.objQuotation.objMake.MakeName, dealerDetailEntity.objQuotation.objModel.ModelName, dealerDetailEntity.objQuotation.objVersion.VersionName,
                            dealer.Name, dealer.EmailId, objCust.CustomerName, objCust.CustomerEmail, objCust.CustomerMobile, objCust.AreaDetails.AreaName, objCust.cityDetails.CityName, dealerDetailEntity.objQuotation.PriceList, Convert.ToInt32(TotalPrice), dealerDetailEntity.objOffers, imagePath, dealer.PhoneNo, bikeName, objDPQSmsEntity.DealerArea);
                    }
                    
                    if (isVerified)
                    {
                        _objPriceQuote.SaveBookingState(pqInput.PQId, PriceQuoteStates.LeadSubmitted);
                        _objLeadNofitication.PushtoAB(pqInput.DealerId.ToString(), pqInput.PQId, objCust.CustomerName, objCust.CustomerMobile, objCust.CustomerEmail, pqInput.VersionId, pqInput.CityId);
                    }
                    output = new Entities.PriceQuote.PQCustomerDetailOutputEntity();
                    output.IsSuccess = isVerified;

                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.BAL.Lead.LeadProcess.NotifyCustomerAndDealer"));
            }
            return output;
        }

        
        /// <summary>
        /// Created By : Deepak Israni on 4 May 2018
        /// Description: BAL function to process manufacturer leads.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="headers"></param>
        public uint ProcessESLead(ManufacturerLeadEntity input, NameValueCollection headers)
        {
            uint leadId = 0;

            try
            {
                String utma = headers["UTMA"];
                String utmz = headers["UTMZ"];
                //String platformId = headers["PlatformId"];

                if (input.CityId > 0 && input.VersionId > 0 && input.PQId > 0 && !String.IsNullOrEmpty(input.Name) && !String.IsNullOrEmpty(input.Mobile) && input.DealerId > 0)
                {
                    CustomerEntity objCust = GetCustomerEntity(input.Name, input.Mobile, input.Email);

                    ES_SaveEntity leadInfo = new ES_SaveEntity
                    {
                        DealerId = input.DealerId,
                        PQId = input.PQId,
                        CustomerId = objCust.CustomerId,
                        CustomerName = input.Name,
                        CustomerEmail = input.Email,
                        CustomerMobile = input.Mobile,
                        LeadSourceId = input.LeadSourceId,
                        UTMA = utma,
                        UTMZ = utmz,
                        DeviceId = input.DeviceId,
                        CampaignId = input.CampaignId,
                        LeadId = input.LeadId
                    };

                    input.LeadId = leadId = _manufacturerCampaignRepo.SaveManufacturerCampaignLead(leadInfo);

                    if (leadId > 0)
                    {
                        IEnumerable<String> numberList = _mobileVerCacheRepo.GetBlockedNumbers();

                        if (numberList != null && !numberList.Contains(input.Mobile))
                        {
                            PushToLeadConsumer(input);

                            if (input.CampaignId == Utility.BWConfiguration.Instance.KawasakiCampaignId)
                            {
                                SMSKawasaki(input);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.BAL.Lead.ProcessESLead : " + Newtonsoft.Json.JsonConvert.SerializeObject(input));
            }

            return leadId;
        }

        /// <summary>
        /// Created By : Deepak Israni on 4 May 2018
        /// Description: Checks if customer exists and if not creates a new customer entity.
        /// </summary>
        /// <param name="customerName"></param>
        /// <param name="mobile"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        private CustomerEntity GetCustomerEntity(string customerName, string mobile, string email)
        {
            CustomerEntity objCust = null;

            if (!_objAuthCustomer.IsRegisteredUser(email, mobile))
            {
                objCust = new CustomerEntity() { CustomerName = customerName, CustomerEmail = email, CustomerMobile = mobile, ClientIP = "" };
                objCust.CustomerId = _objCustomer.Add(objCust);
            }
            else
            {
                objCust = _objCustomer.GetByEmailMobile(email, mobile);

                objCust.CustomerName = customerName;
                objCust.CustomerEmail = !String.IsNullOrEmpty(email) ? email : objCust.CustomerEmail;
                objCust.CustomerMobile = mobile;

                _objCustomer.Update(objCust);
            }

            return objCust;
        }
        
        /// <summary>
        /// Created By : Deepak Israni on 4 May 2018
        /// Description: Pushes lead to Lead Processing Consumer.
        /// </summary>
        /// <param name="input"></param>
        private static void PushToLeadConsumer(ManufacturerLeadEntity input)
        {
            NameValueCollection objNVC = new NameValueCollection();

            objNVC.Add("pqId", input.PQId.ToString());
            objNVC.Add("dealerId", input.DealerId.ToString());
            objNVC.Add("customerName", input.Name);
            objNVC.Add("customerEmail", input.Email);
            objNVC.Add("customerMobile", input.Mobile);
            objNVC.Add("versionId", input.VersionId.ToString());
            objNVC.Add("pincodeId", input.PinCode.ToString());
            objNVC.Add("cityId", input.CityId.ToString());
            objNVC.Add("leadType", "2");
            objNVC.Add("manufacturerDealerId", input.ManufacturerDealerId.ToString());
            objNVC.Add("manufacturerLeadId", input.LeadId.ToString());

            RabbitMqPublish objRMQPublish = new RabbitMqPublish();
            objRMQPublish.PublishToQueue(Bikewale.Utility.BWConfiguration.Instance.LeadConsumerQueue, objNVC);
        }

        private void SMSKawasaki(ManufacturerLeadEntity objLead)
        {
            DPQSmsEntity objDPQSmsEntity = new DPQSmsEntity();
            objDPQSmsEntity.CustomerMobile = objLead.Mobile;
            objDPQSmsEntity.CustomerName = objLead.Name;
            objDPQSmsEntity.DealerName = objLead.ManufacturerDealer;
            SendEmailSMSToDealerCustomer.SendSMSToCustomer(objLead.PQId, string.Empty, objDPQSmsEntity, DPQTypes.KawasakiCampaign);
        }


        private DPQ_SaveEntity CheckRegisteredUser(Entities.PriceQuote.PQCustomerDetailInput input, System.Collections.Specialized.NameValueCollection requestHeaders)
        {
            DPQ_SaveEntity entity = null;
            try
            {
                if (input != null)
                {

                    if (!_objAuthCustomer.IsRegisteredUser(input.CustomerEmail, input.CustomerMobile))
                    {
                        objCust = new CustomerEntity() { CustomerName = input.CustomerName, CustomerEmail = input.CustomerEmail, CustomerMobile = input.CustomerMobile, ClientIP = input.ClientIP };
                        UInt32 CustomerId = _objCustomer.Add(objCust);
                    }
                    else
                    {
                        var objCustomer = _objCustomer.GetByEmailMobile(input.CustomerEmail, input.CustomerMobile);
                        objCust = new CustomerEntity()
                        {
                            CustomerId = objCustomer.CustomerId,
                            CustomerName = input.CustomerName,
                            CustomerEmail = input.CustomerEmail = !String.IsNullOrEmpty(input.CustomerEmail) ? input.CustomerEmail : objCustomer.CustomerEmail,
                            CustomerMobile = input.CustomerMobile
                        };
                        _objCustomer.Update(objCust);
                    }
                    entity = new DPQ_SaveEntity()
                    {
                        DealerId = input.DealerId,
                        PQId = input.PQId,
                        CustomerName = input.CustomerName,
                        CustomerEmail = input.CustomerEmail,
                        CustomerMobile = input.CustomerMobile,
                        ColorId = null,
                        UTMA = requestHeaders["utma"],
                        UTMZ = requestHeaders["utmz"],
                        DeviceId = input.DeviceId,
                        LeadSourceId = input.LeadSourceId
                    };

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.BAL.Lead.LeadProcess.CheckRegisteredUser"));
            }
            return entity;
        }
    }
}
