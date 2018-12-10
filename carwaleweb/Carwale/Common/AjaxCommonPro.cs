using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using AjaxPro;
using Newtonsoft.Json;
using Carwale.Interfaces;
using Carwale.BL.Customers;
using Carwale.Entity;
using Carwale.Entity.Customers;
using Carwale.UI.Common;
using Carwale.Notifications;
using Carwale.DAL.CoreDAL;

namespace CarwaleAjax
{
    public class AjaxCommonPro
    {
        // write all the Common Ajax functions come here
        // These functions will be common for entire website

        //used for writing the debug messages
        private HttpContext objTrace = HttpContext.Current;
        /*
            Function to send Password of the customer through Email
        */
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.None)]
        public string SendCustomerPwd(string email)
        {
            bool retVal = false;
            email = email.Trim();

            ICustomerBL<Customer, CustomerOnRegister> customerRepo = new CustomerActions<Customer, CustomerOnRegister>();
            retVal = customerRepo.GenPasswordChangeAT(email);

            return "200";
        }

        [AjaxMethod()]
        public string UserRegistration(string custName, string password, string email, string phone, string mobile, string cityId)
        {
            Customers cs = new Customers();
            return cs.RegisterCustomer(custName, password, email, phone, mobile, cityId, "", "");
        }

        [AjaxMethod()]
        public GoogleUserInfo GplusRegistration(string accessToken)
        {
            GoogleUserInfo verificationResponse = CustomerSecurity.googleTokenValidate(accessToken);
            verificationResponse.CustomerId = "-1";
            Customers cs = new Customers();
            //                         customerName              password    email                       phone         mobile     cityid      fbId    gplusid
            verificationResponse.CustomerId = cs.RegisterCustomer(verificationResponse.Name, "", verificationResponse.Email, "", "", "", "", verificationResponse.Id, verificationResponse.VerifiedEmail);
            return verificationResponse;
        }

        [AjaxMethod()]
        public FBGraph FbRegistration(string fbId, string accessToken)
        {
            FBGraph verificationResponse = CustomerSecurity.getFBGraph(fbId, accessToken);
            verificationResponse.CustomerId = "-1";
            if (verificationResponse.Id == fbId)
            {
                Customers cs = new Customers();
                //                         customerName             password    email                        phone      mobile     cityid      fbId    gplusid
                verificationResponse.CustomerId = cs.RegisterCustomer(verificationResponse.Name, "", verificationResponse.Email, "", "", "", fbId, "", verificationResponse.Verified);
            }
            return verificationResponse;
        }



        // function to login through ajax
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.None)]
        public bool UserLogin(string loginId, string pwd, string rememberMe, bool isDealer)
        {
            CustomersLogin login = new CustomersLogin();

            // login individual and return true if its successfull
            return login.DoLogin(loginId, pwd, rememberMe == "true" ? true : false);

        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.None)]
        public string GetCurrentUserName()
        {
            return CurrentUser.Name;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.None)]
        public string GetCurrentUserId()
        {
            return CurrentUser.Id;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.None)]
        public bool EndRememberMeSession(string identifier)
        {
            bool resp = false;

            if (CurrentUser.Id != "-1")
            {
                ICustomerBL<Customer, CustomerOnRegister> customerRepo = new CustomerActions<Customer, CustomerOnRegister>();
                resp = customerRepo.EndRememberMeSession(CurrentUser.Id, identifier);
            }

            return resp;
        }

        /*******************************************************************************************************************************************
            -- Ajax function realated to customers ends here
        **********************************************************************************************************************************************/
    }
}