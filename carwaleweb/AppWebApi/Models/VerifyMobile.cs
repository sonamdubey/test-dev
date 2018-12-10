using AppWebApi.Common;
using Carwale.Entity.CustomerVerification;
using Carwale.Entity.Enum;
using Carwale.Interfaces.Classified.Leads;
using Carwale.Interfaces.CustomerVerification;
using Carwale.Service;
using Carwale.Utility;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using RabbitMqPublishing;
using System;
using System.Text.RegularExpressions;
using System.Web;

namespace AppWebApi.Models
{
    public class VerifyMobile
    {
        private string MobileNo { get; set; }
        private string CWICode { get; set; }
        private string SourceId { get; set; }

        private bool isVerified = false;
        //[JsonProperty("isVerified")]
        [JsonIgnore]
        public bool IsVerified
        {
            get { return isVerified; }
            set { isVerified = value; }
        }

        [JsonProperty("responseCode")]
        public string ResponseCode { get; set; }
        [JsonProperty("responseMessage")]
        public string ResponseMessage { get; set; }

        private bool serverErrorOccured = false;
        [JsonIgnore]
        public bool ServerErrorOccured
        {
            get { return serverErrorOccured; }
            set { serverErrorOccured = value; }
        }

        /*
         Author: Rakesh Yadav
         Date Created: 12 Oct 2013
         Desc: Verify user mobile 
         */
        public VerifyMobile(string mobileNo, string cwiCode, Platform source, string clientTokenId)
        {
            ICustomerVerificationRepository customerVerificationRepo;
            ICustomerVerification customerVerificationBL;
            using (IUnityContainer container = UnityBootstrapper.Resolver.GetContainer())
            {
                customerVerificationRepo = container.Resolve<ICustomerVerificationRepository>();
                customerVerificationBL = container.Resolve<ICustomerVerification>();
            }
            string email = "";
            MobileNo = mobileNo;
            CWICode = cwiCode;
            SourceId = source.ToString("D");
            //Generate buyerEmail if not Present
            if (email == "")
            {
                email = mobileNo + "@unknown.com";             //Generate Email 
            }
            if (CWICode != null && MobileNo != null && IsValidMobile(MobileNo))
            {
                if (SourceId.Equals("74"))   //SourceId= 74(Android)
                {
                    SourceId = "5";  //SourceId= 5(Android Buyer process)
                }
                else if (SourceId.Equals("83"))  //SourceId= 74(IOS)
                {
                    SourceId = "9"; //SourceId= 9(IOS Buyer Process)
                }
                isVerified = customerVerificationRepo.CheckVerification(MobileNo, CWICode, string.Empty, email, clientTokenId, int.Parse(SourceId));
                if (isVerified)
                {
                    ResponseCode = "1";
                    ResponseMessage = "OK";
                    
                    customerVerificationBL.SendToVerificationQueue(new MobileVerification
                    {
                        Mobile = MobileNo,
                        Source = source,
                        ClientId = (source == Platform.CarwaleAndroid || source == Platform.CarwaleiOS) ? HttpContextUtils.GetHeader<string>("IMEI") : HttpContextUtils.GetHeader<string>("User-Agent"),
                        VerificationTime = DateTime.Now
                    });
                }
                else
                {
                    ResponseCode = "3";
                    ResponseMessage = "Invalid code";
                }
            }
            else
            {
                ResponseCode = "2";
                ResponseMessage = "(Invalid Data)";
            }
        }       

        /*
         Author: Rakesh Yadav
         Date Created: 16 Oct 2013
         Desc: Check if valid mobile no or not
         */
        private bool IsValidMobile(string mobile)
        {
            bool retVal = false;
            try
            {
                //check with the regular expression
                if (Regex.IsMatch(mobile, @"^[0-9]+$") == true)
                {
                    //check its length
                    if (mobile.Length == 10)
                    {
                        retVal = true;
                    }
                    else
                    {
                        retVal = false;
                    }
                }
                else
                {
                    retVal = false;
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
                retVal = false;
            }

            return retVal;
        }
    }
}
