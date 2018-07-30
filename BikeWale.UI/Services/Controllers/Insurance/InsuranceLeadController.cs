using Bikewale.DAL.Insurance;
using Bikewale.DTO.Insurance;
using Bikewale.Entities.Customer;
using Bikewale.Entities.Insurance;
using Bikewale.Interfaces.Customer;
using Bikewale.Notifications;
using Bikewale.Service.Utilities;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.Insurance
{
    /// <summary>
    /// Created BY : Lucky Rathore
    /// Date : 20 November 2015
    /// Description : API to handle User Insurance Detail.
    /// Modified by :   Sumit Kate on 18 May 2016
    /// Description :   Extend from CompressionApiController instead of ApiController 
    /// </summary>
    public class InsuranceLeadController : CompressionApiController//ApiController
    {
        private readonly ICustomerAuthentication<CustomerEntity, UInt32> _objAuthCustomer = null;
        private readonly ICustomer<CustomerEntity, UInt32> _objCustomer = null;

        /// <summary>
        /// Description : Initialize mentioned Objects
        /// </summary>
        /// <param name="objAuthCustomer"></param>
        /// <param name="objCustomer"></param>
        public InsuranceLeadController(ICustomerAuthentication<CustomerEntity, UInt32> objAuthCustomer, ICustomer<CustomerEntity, UInt32> objCustomer)
        {
            _objAuthCustomer = objAuthCustomer;
            _objCustomer = objCustomer;
        }

        /// <summary>
        /// Created By : Lucky Rathore
        /// Date : 20 November 2015
        /// Description : To submit detail of user for bike Insurance via post method.
        /// </summary>
        /// <param name="insuranceLead"></param>
        /// <returns></returns>
        [ResponseType(typeof(IHttpActionResult))]
        [HttpPost]
        public IHttpActionResult POST([FromBody] Bikewale.Entities.Insurance.InsuranceLead insuranceLead)
        {
            CustomerEntity objCust = null;
            //Submission at Client Side            
            string _apiUrl = "/api/insurance/quote/";

            //http Header Parameter
            IDictionary<string, string> _headerParameters = new Dictionary<string, string>();
            _headerParameters.Add("clientid", "5");
            _headerParameters.Add("platformid", "2");

            ClientResponse response = null;
            PostInsuranceDetail detail = new PostInsuranceDetail();

            detail.Name = insuranceLead.CustomerName;
            detail.Email = insuranceLead.Email;
            detail.Mobile = insuranceLead.Mobile;
            detail.CityName = insuranceLead.CityName;
            detail.Price = insuranceLead.ClientPrice;
            detail.CarPurchaseDate = insuranceLead.BikeRegistrationDate;
            detail.CityId = insuranceLead.CityId;
            detail.MakeId = insuranceLead.MakeId;
            detail.ModelId = insuranceLead.ModelId;
            detail.VersionId = insuranceLead.VersionId;
            detail.InsuranceNew = (insuranceLead.InsurancePolicyType == 1) ? true : false;

            try
            {
                //Register User
                if (!_objAuthCustomer.IsRegisteredUser(insuranceLead.Email, insuranceLead.Mobile))
                {
                    objCust = new CustomerEntity() { CustomerName = detail.Name, CustomerEmail = detail.Email, CustomerMobile = detail.Mobile, ClientIP = insuranceLead.ClientIP, SourceId = insuranceLead.LeadSourceId };
                    insuranceLead.CustomerId = _objCustomer.Add(objCust);
                }
                else
                {
                    insuranceLead.CustomerId = _objCustomer.GetByEmailMobile(insuranceLead.Email, insuranceLead.Mobile).CustomerId;
                }

                //else 
                //{
                //    insuranceLead.CustomerId = _objCustomer.GetByEmail(insuranceLead.Email).CustomerId; 
                //}
                //Send at client Plicy boss side
                using (BWHttpClient objClient = new BWHttpClient())
                {
                    //response = objClient.PostSync<PostInsuranceDetail, ClientResponse>(BWConfiguration.Instance.CwApiHostUrl, BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, detail, _headerParameters);
                    response = objClient.PostSync<PostInsuranceDetail, ClientResponse>(APIHost.CW, BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, detail, _headerParameters);
                }
                //setting client reponse
                if (response != null)
                {
                    insuranceLead.SubmitStatus = response.ConfirmationStatus;
                    insuranceLead.SubmitStatusId = response.UniqueId;
                }
                //To save data.
                if (insuranceLead != null)
                {
                    InsuranceLeadRespositry lead = new InsuranceLeadRespositry();
                    bool status = lead.SaveLeadDetail(insuranceLead);
                    return Ok(status);
                }
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, "Exception : Bikewale.Service.Controllers.Insurance.SaveLeadOnClient");
               
            }

            //Data Submitt on our side
            try
            {

            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, "Exception : Bikewale.Service.Controllers.Insurance.InsuranceLeadController.POST");
               
                return InternalServerError();
            }
            return NotFound();
        }



    }
}
