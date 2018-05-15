using Bikewale.Entities.Customer;
using Bikewale.Entities.Dealer;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.Customer;
using Bikewale.Interfaces.MobileVerification;
using Bikewale.ManufacturerCampaign.Interface;
using Bikewale.Notifications;
using Bikewale.Service.Utilities;
using RabbitMqPublishing;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.LeadsGeneration
{
    /// <summary>
    /// Craeted By  : Sushil Kumar
    /// Created On  : 23rd October 2015
    /// Summary     : To generate manufacturer Lead against the bikewale pricequote
    /// Modified by :   Sumit Kate on 18 May 2016
    /// Description :   Extend from CompressionApiController instead of ApiController 
    /// </summary>
    public class ManufacturerLeadController : CompressionApiController//ApiController
    {
        private readonly IDealerPriceQuote _objIPQ = null;
        private readonly IMobileVerificationCache _mobileVerCacheRepo = null;
        private readonly IManufacturerCampaignRepository _manufacturerCampaignRepo = null;
        private readonly ICustomerAuthentication<CustomerEntity, UInt32> _objAuthCustomer = null;
        private readonly ICustomer<CustomerEntity, UInt32> _objCustomer = null;
        /// <summary>
        /// Type Initializer
        /// </summary>
        /// <param name="objIPQ"></param>
        /// <param name="mobileVerCacheRepo"></param>
        /// <param name="manufacturerCampaignRepo"></param>
        public ManufacturerLeadController(IDealerPriceQuote objIPQ, IMobileVerificationCache mobileVerCacheRepo, IManufacturerCampaignRepository manufacturerCampaignRepo, ICustomerAuthentication<CustomerEntity, UInt32> objAuthCustomer, ICustomer<CustomerEntity, UInt32> objCustomer)
        {
            _objIPQ = objIPQ;
            _mobileVerCacheRepo = mobileVerCacheRepo;
            _manufacturerCampaignRepo = manufacturerCampaignRepo;
            _objAuthCustomer = objAuthCustomer;
            _objCustomer = objCustomer;
        }


        /// <summary>
        /// To genearate manufacturer Lead
        /// Modified By : Sadhana Upadhyay on 29 Dec 2015
        /// Summary : added utmz, utma, device Id etc
        /// Modified by :   Sumit Kate on 18 Aug 2016
        /// Description :   rearranged the data validation checks and return BadRequest if data is invalid
        /// </summary>
        /// <param name="objLead"></param>
        /// <returns></returns>
        /////uint cityId, uint versionId, string name, string email, string mobile, string pqId, UInt16? platformId, UInt16? leadSourceId, string deviceId = null
        [ResponseType(typeof(bool))]
        public IHttpActionResult Post([FromBody]ManufacturerLeadEntity objLead)
        {
            try
            {
                if (objLead != null)
                {
                    if (objLead.CityId > 0 && objLead.VersionId > 0 && objLead.PQId > 0 && !String.IsNullOrEmpty(objLead.Name) && !String.IsNullOrEmpty(objLead.Mobile) && objLead.DealerId > 0)
                    {
                        CustomerEntity objCust = null;
                        if (!_objAuthCustomer.IsRegisteredUser(objLead.Email, objLead.Mobile))
                        {
                            objCust = new CustomerEntity() { CustomerName = objLead.Name, CustomerEmail = objLead.Email, CustomerMobile = objLead.Mobile, ClientIP = "" };
                            objCust.CustomerId = _objCustomer.Add(objCust);

                        }
                        else
                        {
                            var objCustomer = _objCustomer.GetByEmailMobile(objLead.Email, objLead.Mobile);
                            objCust = new CustomerEntity()
                            {
                                CustomerId = objCustomer.CustomerId,
                                CustomerName = objLead.Name,
                                CustomerEmail = objLead.Email = !String.IsNullOrEmpty(objLead.Email) ? objLead.Email : objCustomer.CustomerEmail,
                                CustomerMobile = objLead.Mobile
                            };
                            _objCustomer.Update(objCust);
                        }

                        objLead.LeadId = _manufacturerCampaignRepo.SaveManufacturerCampaignLead(
                            objLead.DealerId,
                            objLead.PQId,
                            objCust.CustomerId,
                            objLead.Name,
                            objLead.Email,
                            objLead.Mobile,
                           objLead.LeadSourceId,
                           Request.Headers.Contains("utma") ? Request.Headers.GetValues("utma").FirstOrDefault() : String.Empty,
                           Request.Headers.Contains("utmz") ? Request.Headers.GetValues("utmz").FirstOrDefault() : String.Empty,
                           objLead.DeviceId,
                           objLead.CampaignId,
                           objLead.LeadId
                            );
                        if (objLead.LeadId > 0)
                        {
                            var numberList = _mobileVerCacheRepo.GetBlockedNumbers();

                            if (numberList != null && !numberList.Contains(objLead.Mobile))
                            {
                                NameValueCollection objNVC = new NameValueCollection();
                                objNVC.Add("pqId", objLead.PQId.ToString());
                                objNVC.Add("dealerId", objLead.DealerId.ToString());
                                objNVC.Add("customerName", objLead.Name);
                                objNVC.Add("customerEmail", objLead.Email);
                                objNVC.Add("customerMobile", objLead.Mobile);
                                objNVC.Add("versionId", objLead.VersionId.ToString());
                                objNVC.Add("pincodeId", Convert.ToString(objLead.PinCode));
                                objNVC.Add("cityId", objLead.CityId.ToString());
                                objNVC.Add("leadType", "2");
                                objNVC.Add("manufacturerDealerId", Convert.ToString(objLead.ManufacturerDealerId));
                                objNVC.Add("manufacturerLeadId", Convert.ToString(objLead.LeadId));
                                objNVC.Add("dealerName", objLead.DealerName);
                                objNVC.Add("bikeName", objLead.BikeName);

                                RabbitMqPublish objRMQPublish = new RabbitMqPublish();
                                objRMQPublish.PublishToQueue(Bikewale.Utility.BWConfiguration.Instance.LeadConsumerQueue, objNVC);
                                if (objLead.CampaignId == Utility.BWConfiguration.Instance.KawasakiCampaignId)
                                    SMSKawasaki(objLead);
                            }
                            

                            return Ok(objLead.LeadId);
                        }
                        else
                        {
                            return NotFound();
                        }
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.ManufacturerLeadController.Post : " + Newtonsoft.Json.JsonConvert.SerializeObject(objLead));
               
                return InternalServerError();
            }
        }

        private void SMSKawasaki(ManufacturerLeadEntity objLead)
        {
            DPQSmsEntity objDPQSmsEntity = new DPQSmsEntity();
            objDPQSmsEntity.CustomerMobile = objLead.Mobile;
            objDPQSmsEntity.CustomerName = objLead.Name;
            objDPQSmsEntity.DealerName = objLead.ManufacturerDealer;
            SendEmailSMSToDealerCustomer.SendSMSToCustomer(objLead.PQId, string.Empty, objDPQSmsEntity, DPQTypes.KawasakiCampaign);
        }

       
    }
}