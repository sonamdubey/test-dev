
using Bikewale.BAL.ApiGateway.Adapters.SpamFilter;
using Bikewale.BAL.ApiGateway.ApiGatewayHelper;
using Bikewale.BAL.ApiGateway.Entities.SpamFilter;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.Customer;
using Bikewale.Entities.Dealer;
using Bikewale.Entities.PriceQuote;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.Customer;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.Lead;
using Bikewale.Interfaces.MobileVerification;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.ManufacturerCampaign.Entities;
using Bikewale.ManufacturerCampaign.Interface;
using Bikewale.Notifications;
using RabbitMqPublishing;
using System;
using System.Web;
using System.Collections.Specialized;
using Bikewale.Utility;
using Bikewale.BAL.Bhrigu;
using log4net;
using Bikewale.Entities.MaskingNumber;
using Newtonsoft.Json;
using Bikewale.Interfaces.MaskingNumber;
using Bikewale.DTO.PriceQuote;
using Bikewale.Interfaces.Location;

namespace Bikewale.BAL.Lead
{
    /// <summary>
    /// Created by : Snehal Dange on 2nd May 2018
    /// Description : To manage dealer and manufacture leads related methods
    /// Modified by : Kartik Rathod on 19 nov 2019
    /// Description : _spamSentinentalScore changed value from 7 to 90
    /// </summary>
    public class LeadProcess : ILead
    {
        #region Class Level Variables
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
        private readonly IApiGatewayCaller _apiGatewayCaller;
        private readonly ushort _spamSentinentalScore = 90;
        static ILog _logger = LogManager.GetLogger("SpamScoreLogger");
        private readonly IMaskingNumberDl _maskingNumberDl ;

        private const float SPAM_SCORE_THRESHOLD = 0.0f;
        #endregion


        #region Constructor
        public LeadProcess(
            ICustomerAuthentication<CustomerEntity, UInt32> objAuthCustomer,
            ICustomer<CustomerEntity, UInt32> objCustomer,
            IDealerPriceQuote objDealerPriceQuote,
            IMobileVerificationRepository mobileVerRespo,
            IMobileVerification mobileVerificetion,
            IDealer objDealer,
            IPriceQuote objPriceQuote, ILeadNofitication objLeadNofitication, IMobileVerificationCache mobileVerCacheRepo, Bikewale.Interfaces.AutoBiz.IDealers objAutobizDealer, IManufacturerCampaignRepository manufacturerCampaignRepo,
            IApiGatewayCaller apiGatewayCaller,IMaskingNumberDl maskingNumberDl)
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
            _apiGatewayCaller = apiGatewayCaller;
            _maskingNumberDl = maskingNumberDl;
        }
        #endregion


        /// <summary>
        /// Modified by : Sanskar Gupta on 09 May 2018
        /// Description : Removed unused variables such as `isVerified` and the DTO
        /// Modified by : Monika Korrapati on 20 Aug 2018
        /// Description : Added NVC object of tracking data and Bhrigu tracking method 'PushDataToBhrigu'
        /// Modified by : Monika Korrapati on 18 Sept 2018
        /// Description : Added Null checks for pqCustomer, objCust, PageUrl
        /// Modified by : Kartik Rathod on 19 nov 2019
        /// Desc        : Remove bhrigu tracking code and called common method BhriguTracking
        /// </summary>
        public PQCustomerDetailOutputEntity ProcessPQCustomerDetailInputWithPQ(Entities.PriceQuote.PQCustomerDetailInput pqInput, System.Collections.Specialized.NameValueCollection requestHeaders)
        {
            PriceQuoteParametersEntity pqParam = null;
            DPQ_SaveEntity entity = null;
            BookingPageDetailsEntity objBookingPageDetailsEntity = null;
            PQCustomerDetail pqCustomer = null;
            PQCustomerDetailOutputEntity pqCustomerDetailEntity = null;
            sbyte noOfAttempts = 0;
            CustomerEntity objCust = null;
            var request = HttpContext.Current.Request;

            try
            {
                if (pqInput != null && (pqInput.PQId > 0))
                {
                    pqParam = new PriceQuoteParametersEntity();
                    pqParam.VersionId = Convert.ToUInt32(pqInput.VersionId);

                    entity = CheckRegisteredUser(pqInput, requestHeaders, ref objCust);

                    pqInput.LeadId = _objDealerPriceQuote.SaveCustomerDetailByPQId(entity);

                    if (entity != null && entity.IsAccepted && pqInput.LeadId > 0) //if the details are not abusive and lead saved successfully
                    {
                        objBookingPageDetailsEntity = _objDealerPriceQuote.FetchBookingPageDetails(Convert.ToUInt32(pqInput.CityId), Convert.ToUInt32(pqInput.VersionId), pqInput.DealerId);

                        pqCustomer = _objDealerPriceQuote.GetCustomerDetailsByLeadId(pqInput.LeadId);
                        objCust = pqCustomer != null ? pqCustomer.objCustomerBase : null;

                        pqCustomerDetailEntity = objCust != null ? NotifyCustomerAndDealer(pqInput, requestHeaders, objCust, false) : new PQCustomerDetailOutputEntity();
                        pqCustomerDetailEntity.Dealer = pqCustomerDetailEntity.IsSuccess && objBookingPageDetailsEntity != null ? objBookingPageDetailsEntity.Dealer : null;
                        if (entity.SpamScore == _spamSentinentalScore)
                        {
                            _logger.Warn(String.Format("Spam Score null for LeadId : {0}", pqInput.LeadId), null);
                        }
                    }
                    else
                    {
                        pqCustomerDetailEntity = new PQCustomerDetailOutputEntity();
                    }
                    pqCustomerDetailEntity.NoOfAttempts = noOfAttempts;
                    pqCustomerDetailEntity.IsSuccess = true;

                    GlobalCityAreaEntity LocationEntity = GlobalCityArea.GetGlobalCityArea();

                    NameValueCollection objNVC = new NameValueCollection();
                    objNVC.Add("leadId", Convert.ToString(pqInput.LeadId));
                    objNVC.Add("leadSourceId", Convert.ToString((ushort)pqInput.LeadSourceId));
                    objNVC.Add("platformId", Convert.ToString(pqInput.PlatformId));
                    objNVC.Add("versionId", Convert.ToString(pqInput.VersionId));
                    objNVC.Add("dealerId", Convert.ToString(pqInput.DealerId));
                    objNVC.Add("appVersion", requestHeaders["appVersion"]);
                    objNVC.Add("campaignId", Convert.ToString(pqInput.CampaignId));
                    objNVC.Add("action", entity != null && entity.IsAccepted ? "Accepted" : "Rejected");
                    objNVC.Add("name", pqInput.CustomerName);
                    objNVC.Add("mobile", pqInput.CustomerMobile);
                    objNVC.Add("email", pqInput.CustomerEmail);

                    BhriguLeadTracking(objNVC, request, LocationEntity);

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.BAL.Lead.LeadProcess.ProcessPQCustomerDetailInputWithPQ(), pqInput : {0}", Newtonsoft.Json.JsonConvert.SerializeObject(pqInput)));
            }
            return pqCustomerDetailEntity;
        }

        /// <summary>
        /// Created by  : Pratibha Verma on 26 June 2018
        /// Description : changes PQId data type and added leadId in entities
        /// Modified by : Monika Korrapati on 20 Aug 2018
        /// Description : Added NVC object of tracking data and Bhrigu tracking method 'PushDataToBhrigu'
        /// Modified by : Monika Korrapati on 18 Sept 2018
        /// Description : Added Null checks for pqCustomer, objCust, PageUrl
        /// Modified by : Kartik Rathod on 19 nov 2019
        /// Desc        : Remove bhrigu tracking code and called common method BhriguTracking
        /// </summary>
        /// <param name="pqInput"></param>
        /// <param name="requestHeaders"></param>
        /// <returns></returns>
        public Bikewale.Entities.PriceQuote.v2.PQCustomerDetailOutputEntity ProcessPQCustomerDetailInputWithPQV2(Bikewale.Entities.PriceQuote.v2.PQCustomerDetailInput pqInput, NameValueCollection requestHeaders)
        {
            PriceQuoteParametersEntity pqParam = null;
            Bikewale.Entities.BikeBooking.v2.DPQ_SaveEntity entity = null;
            BookingPageDetailsEntity objBookingPageDetailsEntity = null;
            PQCustomerDetail pqCustomer = null;
            Bikewale.Entities.PriceQuote.v2.PQCustomerDetailOutputEntity pqCustomerDetailEntity = null;
            sbyte noOfAttempts = 0;
            CustomerEntity objCust = null;
            var request = HttpContext.Current.Request;
            try
            {
                if (pqInput != null && !String.IsNullOrEmpty(pqInput.PQId))
                {
                    pqParam = new PriceQuoteParametersEntity();
                    pqParam.VersionId = Convert.ToUInt32(pqInput.VersionId);
                    
                    entity = CheckRegisteredUserV2(pqInput, requestHeaders, ref objCust);

                    pqInput.LeadId = _objDealerPriceQuote.SaveCustomerDetailByLeadId(entity);

                    if (entity != null && entity.IsAccepted && pqInput.LeadId > 0) //if the details are not abusive and lead saved sucessfully
                    {
                        objBookingPageDetailsEntity = _objDealerPriceQuote.FetchBookingPageDetails(Convert.ToUInt32(pqInput.CityId), Convert.ToUInt32(pqInput.VersionId), pqInput.DealerId);

                        pqCustomer = _objDealerPriceQuote.GetCustomerDetailsByLeadId(pqInput.LeadId);
                        objCust = pqCustomer != null ? pqCustomer.objCustomerBase : null;

                        pqCustomerDetailEntity = objCust != null ? NotifyCustomerAndDealerV2(pqInput, requestHeaders, objCust, false) : new Bikewale.Entities.PriceQuote.v2.PQCustomerDetailOutputEntity();
                        pqCustomerDetailEntity.Dealer = pqCustomerDetailEntity.IsSuccess && objBookingPageDetailsEntity != null ? objBookingPageDetailsEntity.Dealer : null;
                        if (entity.SpamScore == _spamSentinentalScore)
                        {
                            _logger.Warn(String.Format("Spam Score null for LeadId : {0}", pqInput.LeadId), null);
                        }
                    }
                    else
                    {
                        pqCustomerDetailEntity = new Bikewale.Entities.PriceQuote.v2.PQCustomerDetailOutputEntity();
                    }
                    pqCustomerDetailEntity.NoOfAttempts = noOfAttempts;
                    pqCustomerDetailEntity.IsSuccess = true;

                    GlobalCityAreaEntity LocationEntity = GlobalCityArea.GetGlobalCityArea();

                    NameValueCollection objNVC = new NameValueCollection();

                    objNVC.Add("leadId", Convert.ToString(pqInput.LeadId));
                    objNVC.Add("leadSourceId", Convert.ToString((ushort)pqInput.LeadSourceId));
                    objNVC.Add("platformId", Convert.ToString(pqInput.PlatformId));
                    objNVC.Add("versionId", Convert.ToString(pqInput.VersionId));
                    objNVC.Add("dealerId", Convert.ToString(pqInput.DealerId));
                    objNVC.Add("appVersion", requestHeaders["appVersion"]);
                    objNVC.Add("campaignId", Convert.ToString(pqInput.CampaignId));
                    objNVC.Add("action", entity != null && entity.IsAccepted ? "Accepted" : "Rejected");
                    objNVC.Add("name", pqInput.CustomerName);
                    objNVC.Add("mobile", pqInput.CustomerMobile);
                    objNVC.Add("email", pqInput.CustomerEmail);

                    BhriguLeadTracking(objNVC, request, LocationEntity);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.BAL.Lead.LeadProcess.ProcessPQCustomerDetailInputWithPQV2() , pqInput : {0}", Newtonsoft.Json.JsonConvert.SerializeObject(pqInput)));
            }
            return pqCustomerDetailEntity;
        }

        /// <summary>
        /// Modified by : Sanskar Gupta on 09 May 2018
        /// Description : Removed unused variables such as `isVerified` and the DTO, Added the check `pqInput.PQId`
        /// Modified by : Monika Korrapati on 18 Sept 2018
        /// Description : Added Null checks for pqCustomer, objCust
        /// </summary>
        /// <param name="pqInput"></param>
        /// <param name="requestHeaders"></param>
        /// <returns></returns>
        public PQCustomerDetailOutputEntity ProcessPQCustomerDetailInputWithoutPQ(Bikewale.Entities.PriceQuote.PQCustomerDetailInput pqInput, System.Collections.Specialized.NameValueCollection requestHeaders)
        {
            PriceQuoteParametersEntity objPQEntity = null;
            DPQ_SaveEntity entity = null;
            PQCustomerDetailOutputEntity pqCustomerDetailEntity = null;
            string bikeName = String.Empty;
            string imagePath = String.Empty;
            string versionName = string.Empty;

            CustomerEntity objCust = null;
            PQCustomerDetail pqCustomer = null;
            sbyte noOfAttempts = -1;
            UInt64 pqId = default(UInt64);

            try
            {
                if (pqInput != null)
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

                    if (pqInput.PQId <= 0)
                    {
                        pqId = _objPriceQuote.RegisterPriceQuote(objPQEntity);
                        pqInput.PQId = Convert.ToUInt32(pqId);
                    }

                    entity = CheckRegisteredUser(pqInput, requestHeaders, ref objCust);

                    pqInput.LeadId = _objDealerPriceQuote.SaveCustomerDetailByPQId(entity);

                    if (entity.IsAccepted) //if the details are not abusive 
                    {
                        pqCustomer = _objDealerPriceQuote.GetCustomerDetailsByLeadId(pqInput.LeadId);
                        objCust = pqCustomer != null ? pqCustomer.objCustomerBase : null;
                        pqCustomerDetailEntity = objCust != null ? NotifyCustomerAndDealer(pqInput, requestHeaders, objCust, true) : new PQCustomerDetailOutputEntity();
                        if(entity.SpamScore == _spamSentinentalScore) {
                            _logger.Warn(String.Format("SpamScore returned null for LeadId : {0}", pqInput.LeadId));
                        }
                    }
                    else
                    {
                        pqCustomerDetailEntity = new PQCustomerDetailOutputEntity();
                    }
                    pqCustomerDetailEntity.NoOfAttempts = noOfAttempts;
                    pqCustomerDetailEntity.PQId = pqId;
                    pqCustomerDetailEntity.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.BAL.Lead.LeadProcess.ProcessPQCustomerDetailInputV1"));
            }

            return pqCustomerDetailEntity;
        }

        /// <summary>
        /// Created by  : Pratibha Verma on 11 October 2018
        /// Description : new version for PQId related changes
        /// </summary>
        /// <param name="pqInput"></param>
        /// <param name="requestHeaders"></param>
        /// <returns></returns>
        public Entities.PriceQuote.v2.PQCustomerDetailOutputEntity ProcessPQCustomerDetailInputWithoutPQV2(Entities.PriceQuote.v2.PQCustomerDetailInput pqInput, NameValueCollection requestHeaders)
        {
            Entities.PriceQuote.v2.PriceQuoteParametersEntity objPQEntity = null;
            Entities.BikeBooking.v2.DPQ_SaveEntity entity = null;
            Entities.PriceQuote.v2.PQCustomerDetailOutputEntity pqCustomerDetailEntity = null;
            string bikeName = String.Empty;
            string imagePath = String.Empty;
            string versionName = string.Empty;

            CustomerEntity objCust = null;
            PQCustomerDetail pqCustomer = null;
            sbyte noOfAttempts = -1;
            string pqId = string.Empty;

            try
            {
                if (pqInput != null)
                {
                    objPQEntity = new Bikewale.Entities.PriceQuote.v2.PriceQuoteParametersEntity();
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

                    if (string.IsNullOrEmpty(pqInput.PQId))
                    {
                        pqId = _objPriceQuote.RegisterPriceQuoteV2(objPQEntity);
                        pqInput.PQId = pqId;
                    }

                    entity = CheckRegisteredUserV2(pqInput, requestHeaders, ref objCust);

                    pqInput.LeadId = _objDealerPriceQuote.SaveCustomerDetailByLeadId(entity);

                    if (entity.IsAccepted) //if the details are not abusive 
                    {
                        pqCustomer = _objDealerPriceQuote.GetCustomerDetailsByLeadId(pqInput.LeadId);
                        objCust = pqCustomer != null ? pqCustomer.objCustomerBase : null;
                        pqCustomerDetailEntity = objCust != null ? NotifyCustomerAndDealerV2(pqInput, requestHeaders, objCust, true) : new Bikewale.Entities.PriceQuote.v2.PQCustomerDetailOutputEntity();
                    }
                    else
                    {
                        pqCustomerDetailEntity = new Entities.PriceQuote.v2.PQCustomerDetailOutputEntity();
                    }
                    pqCustomerDetailEntity.NoOfAttempts = noOfAttempts;
                    pqCustomerDetailEntity.GuId = pqId;
                    pqCustomerDetailEntity.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.BAL.Lead.LeadProcess.ProcessPQCustomerDetailInputWithoutPQV2"));
            }

            return pqCustomerDetailEntity;
        }

        /// <summary>
        /// Modified by : Sanskar Gupta on 09 May 2018
        /// Description : Optimized the code using object caching (`objDPQSmsEntity` , `dealerDetailEntity.objDealer`, `dealerDetailEntity.objQuotation`), changed `var` to specific object (Eg. (var => string) platformId), added null checks wherever required and other minor code optimizations.
        /// </summary>
        /// <param name="pqInput"></param>
        /// <param name="requestHeaders"></param>
        /// <returns></returns>
        private PQCustomerDetailOutputEntity NotifyCustomerAndDealer(Bikewale.Entities.PriceQuote.PQCustomerDetailInput pqInput, System.Collections.Specialized.NameValueCollection requestHeaders, CustomerEntity objCust, bool IsPQCustomerDetailWithPQ)
        {
            PQCustomerDetailOutputEntity output = new PQCustomerDetailOutputEntity();
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
                    PQ_QuotationEntity quotation = dealerDetailEntity.objQuotation;
                    if (dealerDetailEntity.objBookingAmt != null)
                    {
                        bookingAmount = dealerDetailEntity.objBookingAmt.Amount;
                    }

                    bool isShowroomPriceAvail = false, isBasicAvail = false;

                    foreach (var item in quotation.PriceList)
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

                    imagePath = Bikewale.Utility.Image.GetPathToShowImages(quotation.OriginalImagePath, quotation.HostUrl, Bikewale.Utility.ImageSize._210x118);
                    bikeName = quotation.objMake.MakeName + " " + quotation.objModel.ModelName + " " + quotation.objVersion.VersionName;
                    versionName = quotation.objVersion.VersionName;
                    string platformId = string.Empty;
                    if (requestHeaders != null)
                    {
                        platformId = requestHeaders["platformId"];
                    }

                    apiValue = IsPQCustomerDetailWithPQ ? "api/PQCustomerDetail" : "api/v2/PQCustomerDetail";

                    NewBikeDealers dealer = dealerDetailEntity.objDealer;

                    DPQSmsEntity objDPQSmsEntity = new DPQSmsEntity
                    {
                        CustomerMobile = objCust.CustomerMobile,
                        CustomerName = objCust.CustomerName,
                        DealerMobile = dealer != null ? dealer.PhoneNo : string.Empty,
                        DealerName = dealer != null ? dealer.Organization : string.Empty,
                        Locality = dealer != null ? dealer.Address : string.Empty,
                        BookingAmount = bookingAmount,
                        BikeName = String.Format("{0} {1} {2}", quotation.objMake.MakeName, quotation.objModel.ModelName, quotation.objVersion.VersionName),
                        DealerArea = dealer != null && dealer.objArea != null ? dealer.objArea.AreaName : string.Empty,
                        DealerAdd = dealer != null ? dealer.Address : string.Empty,
                        DealerCity = dealer != null ? dealer.objCity.CityName : string.Empty,
                        OrganisationName = (dealer != null ? dealer.Organization : string.Empty)
                    };

                    if (dealer != null)
                    {
                        _objLeadNofitication.NotifyCustomer(pqInput.PQId, bikeName, imagePath, dealer.Name,
                           dealer.EmailId, dealer.PhoneNo, dealer.Organization,
                           dealer.Address, objCust.CustomerName, objCust.CustomerEmail,
                           quotation.PriceList, dealerDetailEntity.objOffers, dealer.objArea.PinCode,
                           dealer.objState.StateName, dealer.objCity.CityName, TotalPrice, objDPQSmsEntity,
                           apiValue, pqInput.LeadSourceId, versionName, dealer.objArea.Latitude, dealer.objArea.Longitude,
                           dealer.WorkingTime, platformId);

                        _objLeadNofitication.NotifyDealer(pqInput.PQId, quotation.objMake.MakeName, quotation.objModel.ModelName, quotation.objVersion.VersionName,
                            dealer.Name, dealer.EmailId, objCust.CustomerName, objCust.CustomerEmail, objCust.CustomerMobile, objCust.AreaDetails.AreaName, objCust.cityDetails.CityName, quotation.PriceList, Convert.ToInt32(TotalPrice), dealerDetailEntity.objOffers, imagePath, dealer.PhoneNo, bikeName, objDPQSmsEntity.DealerArea, dealerDetailEntity.objDealer.AdditionalNumbers, dealerDetailEntity.objDealer.AdditionalEmails);
                    }

                    if (isVerified)
                    {
                        _objLeadNofitication.PushtoAB(pqInput.DealerId.ToString(), pqInput.PQId, objCust.CustomerName, objCust.CustomerMobile, objCust.CustomerEmail, pqInput.VersionId, pqInput.CityId, String.Empty, pqInput.LeadId, pqInput.CampaignId);
                    }
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
        /// Created by  : Pratibha Verma on 26 June 2018
        /// Description : changes the input entity
        /// </summary>
        /// <param name="pqInput"></param>
        /// <param name="requestHeaders"></param>
        /// <param name="objCust"></param>
        /// <param name="IsPQCustomerDetailWithPQ"></param>
        /// <returns></returns>
        private Bikewale.Entities.PriceQuote.v2.PQCustomerDetailOutputEntity NotifyCustomerAndDealerV2(Bikewale.Entities.PriceQuote.v2.PQCustomerDetailInput pqInput, NameValueCollection requestHeaders, CustomerEntity objCust, bool IsPQCustomerDetailWithPQ)
        {
            Bikewale.Entities.PriceQuote.v2.PQCustomerDetailOutputEntity output = new Bikewale.Entities.PriceQuote.v2.PQCustomerDetailOutputEntity();
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
                    PQ_QuotationEntity quotation = dealerDetailEntity.objQuotation;
                    if (dealerDetailEntity.objBookingAmt != null)
                    {
                        bookingAmount = dealerDetailEntity.objBookingAmt.Amount;
                    }

                    bool isShowroomPriceAvail = false, isBasicAvail = false;

                    foreach (var item in quotation.PriceList)
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

                    imagePath = Bikewale.Utility.Image.GetPathToShowImages(quotation.OriginalImagePath, quotation.HostUrl, Bikewale.Utility.ImageSize._210x118);
                    bikeName = quotation.objMake.MakeName + " " + quotation.objModel.ModelName + " " + quotation.objVersion.VersionName;
                    versionName = quotation.objVersion.VersionName;
                    string platformId = string.Empty;
                    if (requestHeaders != null)
                    {
                        platformId = requestHeaders["platformId"];
                    }

                    apiValue = IsPQCustomerDetailWithPQ ? "api/v1/PQCustomerDetailWithOutPQ/" : "api/v1/PQCustomerDetail";

                    NewBikeDealers dealer = dealerDetailEntity.objDealer;

                    DPQSmsEntity objDPQSmsEntity = new DPQSmsEntity
                    {
                        CustomerMobile = objCust.CustomerMobile,
                        CustomerName = objCust.CustomerName,
                        DealerMobile = dealer != null ? dealer.PhoneNo : string.Empty,
                        DealerName = dealer != null ? dealer.Organization : string.Empty,
                        Locality = dealer != null ? dealer.Address : string.Empty,
                        BookingAmount = bookingAmount,
                        BikeName = String.Format("{0} {1} {2}", quotation.objMake.MakeName, quotation.objModel.ModelName, quotation.objVersion.VersionName),
                        DealerArea = dealer != null && dealer.objArea != null ? dealer.objArea.AreaName : string.Empty,
                        DealerAdd = dealer != null ? dealer.Address : string.Empty,
                        DealerCity = dealer != null ? dealer.objCity.CityName : string.Empty,
                        OrganisationName = (dealer != null ? dealer.Organization : string.Empty)
                    };

                    if (dealer != null)
                    {
                        _objLeadNofitication.NotifyCustomerV2(pqInput.PQId, bikeName, imagePath, dealer.Name,
                           dealer.EmailId, dealer.PhoneNo, dealer.Organization,
                           dealer.Address, objCust.CustomerName, objCust.CustomerEmail,
                           quotation.PriceList, dealerDetailEntity.objOffers, dealer.objArea.PinCode,
                           dealer.objState.StateName, dealer.objCity.CityName, TotalPrice, objDPQSmsEntity,
                           apiValue, pqInput.LeadSourceId, versionName, dealer.objArea.Latitude, dealer.objArea.Longitude,
                           dealer.WorkingTime, platformId);

                        _objLeadNofitication.NotifyDealerV2(pqInput.PQId, quotation.objMake.MakeName, quotation.objModel.ModelName, quotation.objVersion.VersionName,
                            dealer.Name, dealer.EmailId, objCust.CustomerName, objCust.CustomerEmail, objCust.CustomerMobile, objCust.AreaDetails.AreaName, objCust.cityDetails.CityName, quotation.PriceList, Convert.ToInt32(TotalPrice), dealerDetailEntity.objOffers, imagePath, dealer.PhoneNo, bikeName, objDPQSmsEntity.DealerArea, dealerDetailEntity.objDealer.AdditionalNumbers, dealerDetailEntity.objDealer.AdditionalEmails);
                    }

                    if (isVerified)
                    {
                        _objLeadNofitication.PushtoAB(pqInput.DealerId.ToString(), 0, objCust.CustomerName, objCust.CustomerMobile, objCust.CustomerEmail, Convert.ToString(pqInput.VersionId), Convert.ToString(pqInput.CityId), pqInput.PQId, pqInput.LeadId, pqInput.CampaignId);
                    }
                    output.IsSuccess = isVerified;
                    output.LeadId = pqInput.LeadId;

                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.BAL.Lead.LeadProcess.NotifyCustomerAndDealerv2"));
            }
            return output;
        }

        /// <summary>
        /// Modified by : Pratibha Verma on 2 August 2018
        /// Description : Added sourceid and clientip in the entity to be saved
        /// </summary>
        /// <param name="input"></param>
        /// <param name="requestHeaders"></param>
        /// <param name="objCust"></param>
        /// <returns></returns>
        private DPQ_SaveEntity CheckRegisteredUser(Entities.PriceQuote.PQCustomerDetailInput input, System.Collections.Specialized.NameValueCollection requestHeaders, ref CustomerEntity objCust)
        {
            DPQ_SaveEntity entity = null;
            SpamScore spamScore = null;
            float spamThreshold = 0;

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
                    spamScore = CheckSpamScore(objCust);
                    UInt16 platformId;
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
                        LeadSourceId = input.LeadSourceId,
                        PlatformId = UInt16.TryParse(requestHeaders["platformid"], out platformId) ? platformId : Convert.ToUInt16(0),
                        ClientIP = CurrentUser.GetClientIP()
                    };
                    if (spamScore != null)
                    {
                        entity.SpamScore = spamScore.Score;
                        entity.OverallSpamScore = GetSpamOverallScore(spamScore);
                        entity.IsAccepted = (spamScore.Score == spamThreshold);
                    }
                    else
                    {
                        entity.SpamScore = _spamSentinentalScore;
                        entity.OverallSpamScore = _spamSentinentalScore;
                        entity.IsAccepted = true;
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.BAL.Lead.LeadProcess.CheckRegisteredUser"));
            }
            return entity;
        }

        /// <summary>
        /// Modified by : Pratibha Verma on 2 August 2018
        /// Description : Added sourceid and clientip in the entity to be saved
        /// </summary>
        /// <param name="input"></param>
        /// <param name="requestHeaders"></param>
        /// <param name="objCust"></param>
        /// <returns></returns>
        private Bikewale.Entities.BikeBooking.v2.DPQ_SaveEntity CheckRegisteredUserV2(Bikewale.Entities.PriceQuote.v2.PQCustomerDetailInput input, NameValueCollection requestHeaders, ref CustomerEntity objCust)
        {
            Bikewale.Entities.BikeBooking.v2.DPQ_SaveEntity entity = null;
            SpamScore spamScore = null;
            float spamThreshold = 0;

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
                    spamScore = CheckSpamScore(objCust);
                    UInt16 platformId;
                    entity = new Bikewale.Entities.BikeBooking.v2.DPQ_SaveEntity()
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
                        LeadSourceId = input.LeadSourceId,
                        AreaId = input.AreaId,
                        CityId = input.CityId,
                        VersionId = input.VersionId,
                        LeadId = input.LeadId,
                        PlatformId = UInt16.TryParse(requestHeaders["platformid"], out platformId) ? platformId : input.PlatformId,
                        ClientIP = CurrentUser.GetClientIP(),
                        CampaignId = input.CampaignId
                    };
                    if (spamScore != null)
                    {
                        entity.SpamScore = spamScore.Score;
                        entity.OverallSpamScore = GetSpamOverallScore(spamScore);
                        entity.IsAccepted = (spamScore.Score == spamThreshold);
                    }
                    else
                    {
                        entity.SpamScore = _spamSentinentalScore;
                        entity.OverallSpamScore = _spamSentinentalScore;
                        entity.IsAccepted = true;
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.BAL.Lead.LeadProcess.CheckRegisteredUserV2"));
            }
            return entity;
        }

        /// <summary>
        /// Created By : Deepak Israni on 4 May 2018
        /// Description: BAL function to process manufacturer leads.
        /// Modified by : Pratibha Verma on 2 August 2018
        /// Description : Added sourceid and clientip in the entity to be saved
        /// Modified by : Monika Korrapati on 20 Aug 2018
        /// Description : Added NVC object of tracking data and Bhrigu tracking method 'PushDataToBhrigu'
        /// Modified by : Monika Korrapati on 18 Sept 2018
        /// Description : Added Null check on PageUrl
        /// Modified by : Kartik Rathod on 19 nov 2019
        /// Desc        : Remove bhrigu tracking code and called common method BhriguTracking
        /// </summary>
        /// <param name="input"></param>
        /// <param name="headers"></param>
        public uint ProcessESLead(ManufacturerLeadEntity input, NameValueCollection headers)
        {
            uint leadId = 0;
            var request = HttpContext.Current.Request;
            
            try
            {
                if (input!=null && input.CityId > 0 && input.VersionId > 0 && (input.PQId > 0 || !string.IsNullOrEmpty(input.PQGUId)) && !String.IsNullOrEmpty(input.Name) && !String.IsNullOrEmpty(input.Mobile) && input.DealerId > 0)
                {
                    CustomerEntity objCust = GetCustomerEntity(input.Name, input.Mobile, input.Email);

                    ES_SaveEntity leadInfo = new ES_SaveEntity()
                    {
                        DealerId = input.DealerId,
                        PQId = input.PQId,
                        CustomerId = objCust.CustomerId,
                        CustomerName = input.Name,
                        CustomerEmail = input.Email,
                        CustomerMobile = input.Mobile,
                        LeadSourceId = input.LeadSourceId,
                        UTMA = headers["UTMA"],
                        UTMZ = headers["UTMZ"],
                        DeviceId = input.DeviceId,
                        CampaignId = input.CampaignId,
                        LeadId = input.LeadId,
                        CityId = input.CityId,
                        VersionId = input.VersionId,
                        PQGUId = input.PQGUId,
                        PlatformId = Convert.ToUInt16(input.PlatformId),
                        ClientIP = CurrentUser.GetClientIP()
                    };

                    SpamScore spamScore = CheckSpamScore(objCust);

                    if (spamScore != null)
                    {
                        leadInfo.SpamScore = spamScore.Score;
                        leadInfo.Reason = "";
                        leadInfo.IsAccepted = !(spamScore.Score > SPAM_SCORE_THRESHOLD);
                        leadInfo.OverallSpamScore = GetSpamOverallScore(spamScore);
                    }
                    else
                    {
                        leadInfo.SpamScore = _spamSentinentalScore;
                        leadInfo.Reason = "";
                        leadInfo.OverallSpamScore = _spamSentinentalScore;
                        leadInfo.IsAccepted = true;
                    }

                    input.LeadId = leadId = _manufacturerCampaignRepo.SaveManufacturerCampaignLead(leadInfo);
                    if (spamScore == null)
                    {
                        _logger.Warn(String.Format("Spam Score null for LeadId : {0}", input.LeadId), null);
                    }
                    GlobalCityAreaEntity LocationEntity = GlobalCityArea.GetGlobalCityArea();

                    NameValueCollection objNVC = new NameValueCollection();

                    objNVC.Add("leadId", Convert.ToString(input.LeadId));
                    objNVC.Add("leadSourceId", Convert.ToString(input.LeadSourceId));
                    objNVC.Add("platformId", input.PlatformId);
                    objNVC.Add("versionId", Convert.ToString(input.VersionId));
                    objNVC.Add("dealerId", Convert.ToString(input.DealerId));
                    objNVC.Add("appVersion", headers["appVersion"]);
                    objNVC.Add("campaignId", Convert.ToString(input.CampaignId));
                    objNVC.Add("action", leadInfo != null && leadInfo.IsAccepted ? "Accepted" : "Rejected");
                    objNVC.Add("name", input.Name);
                    objNVC.Add("mobile", input.Mobile);
                    objNVC.Add("email", input.Email);

                    BhriguLeadTracking(objNVC, request, LocationEntity);


                    if (leadId > 0 && leadInfo.IsAccepted)
                    {
                        PushToLeadConsumer(input);

                        if (input.CampaignId == Utility.BWConfiguration.Instance.KawasakiCampaignId)
                        {
                            SMSKawasaki(input);
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
        private CustomerEntity GetCustomerEntity(string customerName, string mobile, string email, bool isPerformUpdate = true)
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
                
                if (isPerformUpdate)
                {
                    _objCustomer.Update(objCust);
                }
            }

            return objCust;
        }

        /// <summary>
        /// Created By : Deepak Israni on 4 May 2018
        /// Description: Pushes lead to Lead Processing Consumer.
		/// Modifier    : Kartik Rathod on 16 may 2018, added dealerName,bikename and sendLeadSMSCustomer to ManufacturerLead consumer 
        /// Modified By : Rajan Chauhan on 20 August 2018 
        /// Description : Added null check on Pincode 
        /// Modified by : Pratibha Verma on 10 October 2018
        /// Description : added emailOption in nvc object
        /// Modified by : Kartik Rathod on 19 nov 2019
        /// Desc        : added inquirySource , otherData and comments
        /// </summary>
        /// <param name="input"></param>
        /// <param name="inquirySource">39 for bikewale new lead by default</param>
        /// <param name="otherData">extra datea send to autobiz api in others field</param>
        /// <param name="comments">comments from </param>
        private static void PushToLeadConsumer(ManufacturerLeadEntity input, ushort inquirySource = 39, string otherData = "",string comments = "")
        {
            EnumEmailOptions emailOptionValue;
            UInt16 emailOption = Enum.TryParse<EnumEmailOptions>(input.EmailOption, out emailOptionValue) ? (UInt16)emailOptionValue : (UInt16)EnumEmailOptions.Optional;
            NameValueCollection objNVC = new NameValueCollection();

            objNVC.Add("pqId", Convert.ToString(input.PQId));
            objNVC.Add("pqGUId", input.PQGUId);
            objNVC.Add("dealerId", Convert.ToString(input.DealerId));
            objNVC.Add("customerName", input.Name);
            objNVC.Add("customerEmail", input.Email);
            objNVC.Add("customerMobile", input.Mobile);
            objNVC.Add("versionId", Convert.ToString(input.VersionId));
            objNVC.Add("pincodeId", string.IsNullOrEmpty(input.PinCode) ? string.Empty : input.PinCode);
            objNVC.Add("cityId", Convert.ToString(input.CityId));
            objNVC.Add("leadType", "2");
            objNVC.Add("manufacturerDealerId", Convert.ToString(input.ManufacturerDealerId));
            objNVC.Add("manufacturerLeadId", Convert.ToString(input.LeadId));
			objNVC.Add("dealerName", input.DealerName);
			objNVC.Add("bikeName", input.BikeName);
			objNVC.Add("sendLeadSMSCustomer", Convert.ToString(input.SendLeadSMSCustomer));
            objNVC.Add("emailOption", Convert.ToString(emailOption));
            objNVC.Add("campaignId", Convert.ToString(input.CampaignId));
            objNVC.Add("others", otherData);
            objNVC.Add("inquirySource", Convert.ToString(inquirySource));
            objNVC.Add("comments", comments);

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

        /// <summary>
        /// Created By : Deepak Israni on 14 May 2018
        /// Description: Function to call microservice to check whether inputs by customer are valid or invalid.
        /// </summary>
        /// <param name="customerDetails"></param>
        /// <returns></returns>
        private SpamScore CheckSpamScore(CustomerEntityBase customerDetails)
        {
            GetScoreAdapter spamFilter = new BAL.ApiGateway.Adapters.SpamFilter.GetScoreAdapter();
            spamFilter.AddApiGatewayCall(_apiGatewayCaller, customerDetails);
            _apiGatewayCaller.Call();
            SpamScore output = spamFilter.Output;
            return output;
        }

        /// <summary>
        /// Created By : Deepak Israni on 14 May 2018
        /// Description: Function to calculate  overall spam score (to get exact reason for rejection of lead).
        /// </summary>
        /// <param name="spamScore"></param>
        /// <returns></returns>
        private ushort GetSpamOverallScore(SpamScore spamScore)
        {
            float threshold = 0.0f;
            ushort ovrScore = 0;
            try
            {
                if (spamScore != null)
                {
                    if (spamScore.Name != null && spamScore.Name.Score > threshold)
                    {
                        ovrScore += (ushort)SpamDetailsEnum.Name;
                    }

                    if (spamScore.Email != null && spamScore.Email.Score > threshold)
                    {
                        ovrScore += (ushort)SpamDetailsEnum.Email;
                    }

                    if (spamScore.Number != null && spamScore.Number.Score > threshold)
                    {
                        ovrScore += (ushort)SpamDetailsEnum.Number;
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.BAL.Lead.GetSpamOverallScore");
            }
            return ovrScore;
        }

        /// <summary>
        /// Author  : Kartik Rathod on 19 nov 2019
        /// Desc    : Process Masking number lead
        /// </summary>
        /// <param name="objLead"></param>
        /// <returns></returns>
        public uint ProcessMaskingLead(MaskingNumberLeadEntity objLead)
        {
            
            try
            {
                if(objLead == null)
                {
                    return 0;
                }

                Bikewale.Entities.PriceQuote.v2.PriceQuoteParametersEntity objPQEntity = new Bikewale.Entities.PriceQuote.v2.PriceQuoteParametersEntity()
                {
                    CityId = objLead.CityId,
                    SourceId = (ushort)Bikewale.DTO.PriceQuote.PQSources.MaskingNumber,
                    CustomerName = objLead.CustomerName,
                    CustomerMobile = objLead.CustomerMobile,
                    DealerId = objLead.DealerId,
                    ModelId = objLead.ModelId,
                    VersionId = objLead.VersionId,
                    CampaignId = objLead.CampaignId,
                    PQLeadId = (ushort)PQSourceEnum.MaskingNumber
                };

                objLead.PqGuId = _objPriceQuote.RegisterPriceQuoteV2(objPQEntity);

                if (!string.IsNullOrEmpty(objLead.PqGuId))
                {
                    CustomerEntity objCust = GetCustomerEntity(objPQEntity.CustomerName, objPQEntity.CustomerMobile, objPQEntity.CustomerEmail,false);
                    objLead.CustomerId = (uint)objCust.CustomerId;
                    objLead.CustomerEmail = objCust.CustomerEmail;
                    
                    SpamScore spamScore = CheckSpamScore(objCust);

                    if (spamScore != null)
                    {
                        objLead.SpamScore = spamScore.Score;
                        objLead.IsAccepted = !(spamScore.Score > SPAM_SCORE_THRESHOLD);
                        objLead.OverallSpamScore = GetSpamOverallScore(spamScore);
                    }
                    else
                    {
                        objLead.SpamScore = _spamSentinentalScore;
                        objLead.OverallSpamScore = _spamSentinentalScore;
                        objLead.IsAccepted = true;
                    }

                    objLead.LeadId = _maskingNumberDl.SaveMaskingNumberLead(objLead);
                    
                    if (spamScore == null)
                    {
                        _logger.Warn(String.Format("Spam Score null for LeadId : {0}", objLead.LeadId), null);
                    }
                    if (objLead.LeadId > 0 && objLead.IsAccepted)
                    {
                        PushMaskingNoLeadToConsumer(objLead);
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Exception : Bikewale.BAL.MaskingNumber.MaskingNumberBl.ProcessMaskingLead() obj - {0}", JsonConvert.SerializeObject(objLead)));
            }
            return objLead.LeadId;
        }

        /// <summary>
        /// Author  : Kartik Rathod on 19 nov 2019
        /// Desc    : Pushed Masking lead to consumer
        /// </summary>
        /// <param name="objLead"></param>
        private void PushMaskingNoLeadToConsumer(MaskingNumberLeadEntity objLead)
        {
            if (objLead != null)
            {
                if (objLead.LeadTypeId == LeadTypeEnum.DS)
                {
                    _objLeadNofitication.PushtoAB(Convert.ToString(objLead.DealerId), 0, objLead.CustomerName, objLead.CustomerMobile, objLead.CustomerEmail, Convert.ToString(objLead.VersionId), Convert.ToString(objLead.CityId), objLead.PqGuId, objLead.LeadId, objLead.CampaignId,objLead.InquirySource, Newtonsoft.Json.JsonConvert.SerializeObject(objLead.Others),objLead.Comments);
                }
                else
                {
                    PushToLeadConsumer(new ManufacturerLeadEntity(){
                        PQGUId =  objLead.PqGuId,
                        DealerId = objLead.DealerId,
                        Name = objLead.CustomerName,
                        Email = objLead.CustomerEmail,
                        Mobile = objLead.CustomerMobile,
                        VersionId = objLead.VersionId,
                        CityId = objLead.CityId,
                        LeadId = objLead.LeadId,
                        SendLeadSMSCustomer = false,
                        EmailOption = "Not_Required",
                        CampaignId = objLead.CampaignId
                    }, objLead.InquirySource, Newtonsoft.Json.JsonConvert.SerializeObject(objLead.Others),objLead.Comments);
                }
            }
        }

        /// <summary>
        /// Author  : Kartik Rathod on 19 nov 2019
        /// Desc    : created common method for bhrigu call for ds es and masking number
        /// </summary>
        /// <param name="objData"></param>
        private void BhriguLeadTracking(NameValueCollection objNVC, HttpRequest request, GlobalCityAreaEntity locationEntity)
        {
            if (objNVC != null)
            {
                string PageUrl = request != null ? Convert.ToString(request.UrlReferrer) : string.Empty;
                                
                objNVC.Add("category", "NewBikesLead");
                objNVC.Add("pageUrl", PageUrl);
                objNVC.Add("clientIP", CurrentUser.GetClientIP());
                objNVC.Add("queryString", !string.IsNullOrEmpty(PageUrl) && PageUrl.Contains("?") ? PageUrl.Split('?')[1].Replace('&', '|') : string.Empty);
                objNVC.Add("userAgent", request != null ? request.UserAgent : string.Empty);
                objNVC.Add("referrer", string.Empty);
                objNVC.Add("cookieId", request!=null ? (request.Cookies["BWC"] != null ? request.Cookies["BWC"].Value : request.Headers["IMEI"]) : string.Empty);
                objNVC.Add("sessionId", "NA");
                objNVC.Add("bwtest", request!=null && request.Cookies["_bwtest"] != null ? request.Cookies["_bwtest"].Value : string.Empty);
                objNVC.Add("bwutmz", request!=null && request.Cookies["_bwutmz"] != null ? request.Cookies["_bwutmz"].Value : string.Empty);
                objNVC.Add("cwv", request!=null && request.Cookies["_cwv"] != null ? request.Cookies["_cwv"].Value : string.Empty);
                objNVC.Add("locationCity", locationEntity != null ? locationEntity.City : string.Empty);
                objNVC.Add("locationArea", locationEntity!= null ? locationEntity.Area : string.Empty);

                LeadTracking.PushDataToBhrigu(objNVC);
            }
            
        }

    }
}
