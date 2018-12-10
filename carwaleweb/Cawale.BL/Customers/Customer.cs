using Carwale.DAL.Customers;
using Carwale.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.Interfaces;
using Carwale.Notifications.MailTemplates;
using System.Web;
using Carwale.Utility;
using System.Web.Security;
using Carwale.DAL.CoreDAL;
using System.Data.SqlClient;
using System.Data;
using Carwale.Notifications;
using Carwale.Entity.Enum;

namespace Carwale.BL.Customers
{
    public class CustomerActions<TEntity, TOut> : ICustomerBL<TEntity, TOut>
        where TEntity : Customer
        where TOut : CustomerOnRegister
    {
        ICustomerRepository<Customer, CustomerOnRegister> CustDal;

        public CustomerActions()
        {
            CustDal = new CustomerRepository<Customer, CustomerOnRegister>();
        }

        //general functions for customers start
        public TOut CreateCustomer(TEntity customer)
        {
            bool isARR = false;
            if (string.IsNullOrWhiteSpace(customer.Email))
                return (TOut)new CustomerOnRegister() { CustomerId = "0" };

            customer.SecurityKey = CustomerSecurity.GetRandomKey();
            if (string.IsNullOrWhiteSpace(customer.Password))
            {
                customer.Password = CustomerSecurity.GetPassword();
                isARR = true;
            }

            if (string.IsNullOrWhiteSpace(customer.OAuth))  
                customer.OAuth = CustomerSecurity.getAccessToken(20);

            customer.PasswordSaltHashStr = PBKDF2.PasswordHash.CreateHash(customer.Password);

            CustomerOnRegister custOut = CustDal.Create(customer);

            if (custOut.CustomerId != "-1" && custOut.StatusOnRegister == "N")
            {
                new CustomerRegistrationTemplate(customer.Name, customer.Email, customer.Password, custOut.CustomerId, isARR);
            }

            return (TOut)custOut;
        }

        public TEntity GetCustomer(string email)
        {
            return (TEntity)CustDal.GetCustomerFromEmail(email);
        }

        public TEntity GetCustomerById(string CustomerId)
        {
            return (TEntity)CustDal.GetCustomerFromCustomerId(CustomerId);
        }

        public TEntity GetCustomer(string email, string password,string oauth="")
        {
            Customer customer = CustDal.GetCustomerFromEmail(email, oauth);

            if (customer.CustomerId == "-1" || !PBKDF2.PasswordHash.ValidatePassword(password, customer.PasswordSaltHashStr))
            {
                customer = new Customer();
                customer.CustomerId = "-1";
            }

            return (TEntity)customer;
        }

        public bool UpdateCustomerDetails(TEntity customer)
        {
            return CustDal.Update(customer);
        }
        //general functions for customers end

        //password change functions start
        public bool ResetPassword(string customerId, string oldPassword, string newPassword)
        {
            bool retval = false;

            Customer customer = CustDal.GetCustomerFromCustomerId(customerId);

            if (customer.CustomerId != "-1" && PBKDF2.PasswordHash.ValidatePassword(oldPassword, customer.PasswordSaltHashStr))
            {
                string newoauth = CustomerSecurity.getAccessToken(20);  

                retval = CustDal.ResetPassword(customerId, PBKDF2.PasswordHash.CreateHash(newPassword), newoauth);
                
            }
            
            return retval;
        }
        
        public bool GenPasswordChangeAT(string email)
        {
            bool resp = false;

            string AccesToken = CustomerSecurity.getAccessToken(21);

            if (CustDal.SavePasswordChangeAT(email, AccesToken) != "-1")
            {
                resp = true;
                Customer cust = CustDal.GetCustomerFromEmail(email);
                new CustomerPasswordResetTemplate(cust.Email, cust.Name, AccesToken);
            }

            return resp;
        }

        public bool ResetPassword(string AccessToken, string newPassword)
        {
            bool returnVal = false;
            int MinutesDiff;
            string customerId = CustDal.GetCustomerIdByAccessToken(AccessToken, out MinutesDiff);

            if (IsMinutesDiffValid(MinutesDiff) && customerId != "-1")
            {
                string newoauth = CustomerSecurity.getAccessToken(20);
                returnVal = CustDal.ResetPassword(customerId, PBKDF2.PasswordHash.CreateHash(newPassword), newoauth);
            }
            return returnVal;
        }

        public TEntity GetCustomerByAccessToken(string AccessToken)
        {
            int MinutesDiff;
            string customerId = CustDal.GetCustomerIdByAccessToken(AccessToken, out MinutesDiff);
            if (IsMinutesDiffValid(MinutesDiff) && customerId != "-1")
                return (TEntity)CustDal.GetCustomerFromCustomerId(customerId);
            else
                return (TEntity)(new Customer(){CustomerId = "-1"});
        }
        
        static int _minutesDiff = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["resetlinkvalidityinmins"]?? "1440");

        static bool IsMinutesDiffValid(int MinutesDiff)
        {
            if (MinutesDiff >= 0 && MinutesDiff <= _minutesDiff)
                return true;
            else
                return false;
        }        
        //password change functions end

        //user session and rememberme functions start
        public void CreateRememberMeSession(string customerId)
        {
            CustomerRememberMe custrm = new CustomerRememberMe()
            {
                CustomerId = customerId,
                Identifier = CustomerSecurity.getRandomString(20),
                AccessToken = CustomerSecurity.getRandomString(20),
                IPAddress = ipaddr,
                UserAgent = usragent
            };
            if (CustDal.CreateRememberMeSession(custrm))//save rememberme details in db
            {
                //create rememberme cookie
                CreateRememberMeCookie(custrm, CustDal.GetCustomerFromCustomerId(custrm.CustomerId));
            }
        }

        //use this method to authenticate user by rememberme cookie
        public bool UseActiveRememberMeSession()
        {
            bool retval = false;
            CustomerRememberMe custrm = ValidateRememberMeCookie();

            if (custrm.CustomerId != "-1")
            {
                custrm.IPAddress = ipaddr;
                custrm.UserAgent = usragent;
                string resp = "N";
                if (custrm.Identifier != "-1")//update session details in database (for new rememberme cookies)
                {
                    //assign new accesstoken
                    string newAccessToken = CustomerSecurity.getRandomString(20);
                    resp = CustDal.UseActiveRememberMeSession(custrm, newAccessToken);
                    custrm.AccessToken = newAccessToken;                   
                }
                else if (custrm.Identifier == "-1" && custrm.AccessToken == "-1")//create session details in databse (for old rememberme cookies)
                {
                    custrm.Identifier = CustomerSecurity.getRandomString(20);
                    custrm.AccessToken = CustomerSecurity.getRandomString(20);
                    if (CustDal.CreateRememberMeSession(custrm))
                        resp = "Y";
                }
                if (resp == "Y")//proceed when resp is YES
                {
                    Customer cust = CustDal.GetCustomerFromCustomerId(custrm.CustomerId);
                    if (cust.CustomerId != "-1")
                    {
                        //update remmberme cookie with new accesstoken
                        CreateRememberMeCookie(custrm, cust);
                        StartSession(cust.Name, cust.CustomerId, cust.Email, cust.IsEmailVerified);
                        retval = true;
                    }
                }
            }

            if (!retval)
                ClearRememberMeCookie();
            return retval;
        }

        public bool EndRememberMeSession(string customerId, string identifier)
        {
            bool retval = false;
            
            string Identifier = CustomTripleDES.DecryptTripleDES(HttpUtility.UrlDecode(identifier));
            retval = CustDal.EndRememberMeSession(customerId, Identifier);

            return retval;
        }

        static string RememberMeCookieName = "RememberMe";

        static void CreateRememberMeCookie(CustomerRememberMe custrm, Customer cust)
        {
            //userId~userName~Email~Identifier~AccessToken~isEmailVerified 
            string credentials = HttpUtility.UrlEncode(CustomTripleDES.EncryptTripleDES(custrm.CustomerId)) + "~"
                                        + HttpUtility.UrlEncode(CustomTripleDES.EncryptTripleDES(cust.Name)) + "~"
                                        + HttpUtility.UrlEncode(CustomTripleDES.EncryptTripleDES(cust.Email)) + "~"
                                        + HttpUtility.UrlEncode(CustomTripleDES.EncryptTripleDES(custrm.Identifier)) + "~"
                                        + HttpUtility.UrlEncode(CustomTripleDES.EncryptTripleDES(custrm.AccessToken)) + "~"
                                        + HttpUtility.UrlEncode(CustomTripleDES.EncryptTripleDES(cust.IsEmailVerified.ToString()));

            HttpCookie rememberCookie = new HttpCookie(RememberMeCookieName);
            rememberCookie.Value = credentials;
            rememberCookie.Expires = DateTime.Now.AddYears(1);
            HttpContext.Current.Response.Cookies.Add(rememberCookie);
        }

        static void ClearRememberMeCookie()
        {
            HttpCookie rememberMe1 = HttpContext.Current.Request.Cookies.Get(RememberMeCookieName);
            if (rememberMe1 != null)
            {
                rememberMe1.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(rememberMe1);
            }
        }

        static CustomerRememberMe ValidateRememberMeCookie()
        {
            HttpCookie rememberMe = HttpContext.Current.Request.Cookies.Get("RememberMe");
            string userName = "", userId = "-1", email = "", identifier = "", accesToken = "", isEmailVerified = "";
            string[] credentials;

            CustomerRememberMe custrm = new CustomerRememberMe() { CustomerId = userId};
            if (!string.IsNullOrWhiteSpace(rememberMe.Value))
            {
                credentials = rememberMe.Value.Split('~');

                try
                {
                    userId = CustomTripleDES.DecryptTripleDES(HttpUtility.UrlDecode(credentials[0]));
                    userName = CustomTripleDES.DecryptTripleDES(HttpUtility.UrlDecode(credentials[1]));
                    email = CustomTripleDES.DecryptTripleDES(HttpUtility.UrlDecode(credentials[2]));
                    identifier = CustomTripleDES.DecryptTripleDES(HttpUtility.UrlDecode(credentials[3]));
                    accesToken = CustomTripleDES.DecryptTripleDES(HttpUtility.UrlDecode(credentials[4]));
                    isEmailVerified = CustomTripleDES.DecryptTripleDES(HttpUtility.UrlDecode(credentials[5]));

                    custrm = new CustomerRememberMe() { CustomerId = userId, Identifier = identifier, AccessToken = accesToken };
                }
                catch (Exception err)
                {
                    custrm = new CustomerRememberMe() { CustomerId = "-1" };
                    var objErr = new ExceptionHandler(err, "ValidateRememberMeCookie()");
                    objErr.LogException();
                }
            }
            
            return custrm;
        }

        static void StartSession(string userName, string userId, string email, bool isEmailVerified)
        {
            //create a ticket and add it to the cookie
            FormsAuthenticationTicket ticket;
            //now add the id and the role to the ticket, concat the id and role, separated by ',' 
            ticket = new FormsAuthenticationTicket(
                        1,
                        userName,
                        DateTime.Now,
                        DateTime.Now.AddDays(365),
                        false,
                        userId + ":" + email + ":" + isEmailVerified.ToString()
                    );

            //add the ticket into the cookie
            HttpCookie objCookie;
            objCookie = new HttpCookie(".ASPXAUTH");
            objCookie.Value = FormsAuthentication.Encrypt(ticket);
            HttpContext.Current.Response.Cookies.Add(objCookie);

            // delete auction cookie if not exists
            // it will be reassign
            DeleteAuctionCookie();
        }        

        static void DeleteAuctionCookie()
        {
            //Delete auction cookies if exists
            if (HttpContext.Current.Request.Cookies["CookieBidderId"] != null)
            {
                HttpContext.Current.Response.Cookies["CookieBidderId"].Expires = DateTime.Now.AddYears(-1);
            }
        }

        public void UpdateSourceId(EnumTableType source, string id)
        {
            CustDal.UpdateSourceId(source, id);
        }

        static string ipaddr
        {
            get
            {
                return !string.IsNullOrWhiteSpace(HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"]) ?
                    HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"]
                    :
                    HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
        }

        static string usragent { get { return HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"]; } }

        //user session and rememberme functions end
    }
}
