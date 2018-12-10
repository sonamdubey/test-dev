using Carwale.BL.Customers;
using Carwale.DAL.CoreDAL;
using Carwale.DAL.Customers;
using Carwale.Entity;
using Carwale.Entity.Customers;
using Carwale.Interfaces;
using Carwale.Notifications;
using Carwale.Notifications.Logs;
using Carwale.Service.Filters;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Carwale.Service.Controllers.Users
{
    public class LoginController : ApiController
    {
        /// <summary>
        /// Returns Customer Details if valid AccessToken(from google or facebook) is supplied with required arguements.
        /// Registers the Customer if First time.
        /// </summary>
        /// <param name="inputs">AccessToken</param>
        /// <param name="inputs">SocialPlatform</param>
        /// <param name="inputs">FbId(if SocialPlatform is 'f' for facebook)</param>
        /// <returns>Customer Details JSON as http response</returns>
        [HttpGet]
        [ActionName("social")]
        [AuthenticateBasic]
        [PlatformRequired]
        public HttpResponseMessage Social([FromUri] LoginInputs inputs)
        {
            int custId=-1;

            var response = new HttpResponseMessage();
            if (inputs.AccessToken != null )
                {
                    if (inputs.SocialPlatform == "g")
                    {
                        GoogleUserInfo verificationResponse = CustomerSecurity.googleTokenValidate(inputs.AccessToken);
                        if (verificationResponse.Id != "" && verificationResponse.Id != "-1")
                        {
                            CustomerOnRegister customer = RegisterCustomer(verificationResponse.Name, "", verificationResponse.Email, "", "", verificationResponse.Id, verificationResponse.VerifiedEmail);
                            verificationResponse.CustomerId = customer.CustomerId;
                            custId = Convert.ToInt32(verificationResponse.CustomerId);

                            response.Content = new StringContent("{\"customerId\":" + custId
                                                                + ",\"email\":\"" + verificationResponse.Email + "\",\"isEmailVerified\":" + verificationResponse.VerifiedEmail.ToString().ToLower() + ",\"name\":\"" + verificationResponse.Name + "\",\"oauth\":\"" + customer.OAuth 
                                                                + "\",\"status\":200}");
                            response.StatusCode = HttpStatusCode.OK;
                            return response;
                        }
                        else
                        {
                            response.Content = new StringContent("{\"message\":\"Unauthorized\",\"status\":401}");
                            response.StatusCode = HttpStatusCode.Unauthorized;
                            return response;
                        }
                    }
                    else if (inputs.SocialPlatform == "f" && inputs.FbId!=null)
                    {
                        FBGraph verificationResponse = CustomerSecurity.getFBGraph(inputs.FbId, inputs.AccessToken);
                        if (verificationResponse.Id != "-1" && verificationResponse.Id == inputs.FbId)
                        {
                            CustomerOnRegister customer = RegisterCustomer(verificationResponse.Name, "", verificationResponse.Email, "", verificationResponse.Id, "", verificationResponse.Verified);
                            verificationResponse.CustomerId = customer.CustomerId;
                            custId = Convert.ToInt32(verificationResponse.CustomerId);

                            response.Content = new StringContent("{\"customerId\":" + custId
                                                                + ",\"email\":\"" + verificationResponse.Email + "\",\"isEmailVerified\":" + verificationResponse.Verified.ToString().ToLower() + ",\"name\":\"" + verificationResponse.Name + "\",\"oauth\":\"" + customer.OAuth
                                                                + "\",\"status\":200}");
                            response.StatusCode = HttpStatusCode.OK;
                            return response;
                        }
                        else
                        {
                            response.Content = new StringContent("{\"message\":\"Unauthorized\",\"status\":401}");
                            response.StatusCode = HttpStatusCode.Unauthorized;
                            return response;
                        }
                    }
                    else
                    {
                        response.Content = new StringContent("{\"message\":\"Please Enter correct 'SocialPlatform' ,and 'FbId' if facebook\",\"status\":400}");
                        response.StatusCode = HttpStatusCode.BadRequest;
                        return response;
                    }
            }
            response.Content = new StringContent("{\"message\":\"Bad Request,token required\",\"status\":400}");
            response.StatusCode = HttpStatusCode.BadRequest;
            return response;
        }

        /// <summary>
        /// This api returns Customer Details when correct credentials are supplied
        /// </summary>
        /// <param name="inputs">email</param>
        /// <param name="inputs">pass</param>
        /// <returns>Customer Details JSON as http response</returns>
        [HttpGet]
        [ActionName("cw")]
        [AuthenticateBasic]
        [PlatformRequired]
        public HttpResponseMessage Cw([FromUri] LoginInputs inputs)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            if(inputs.Email!=null)
            {
                if (inputs.Password != null)
                {
                    ICustomerBL<Customer, CustomerOnRegister> customerRepo = new CustomerActions<Customer, CustomerOnRegister>();
                    string oauth = CustomerSecurity.getAccessToken(20);  
                    Customer customer = customerRepo.GetCustomer(inputs.Email, inputs.Password,oauth);
                    if (customer.CustomerId == "-1" || customer.CustomerId == "")
                    {
                        response.Content = new StringContent("{\"message\":\"Unauthorized\",\"status\":401}");
                        response.StatusCode = HttpStatusCode.Unauthorized;
                        return response;
                    }

                    response.Content = new StringContent("{\"customerId\":" + customer.CustomerId + 
                                                        ",\"name\":\"" +  customer.Name + 
                                                        "\",\"isEmailVerified\":" +  customer.IsEmailVerified.ToString().ToLower() +
                                                        ",\"oauth\":\""+customer.OAuth+
                                                        "\",\"status\":200}");
                    response.StatusCode = HttpStatusCode.OK;
                    return response;

                }
                response.Content = new StringContent("{\"message\":\"Bad Request,pass required\",\"status\":400}");
                response.StatusCode = HttpStatusCode.BadRequest;
                return response;
            }
            else
            {
                response.Content = new StringContent("{\"message\":\"Bad Request,email required\",\"status\":400}");
                response.StatusCode = HttpStatusCode.BadRequest;
                return response;
            }
        }

        [HttpGet]
        [ActionName("passchange")]
        [AuthenticateBasic]
        [PlatformRequired]
        public HttpResponseMessage PassChange([FromUri] LoginInputs inputs)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            if (inputs.NewPassword!=null)
            {
                ICustomerBL<Customer, CustomerOnRegister> customerRepo = new CustomerActions<Customer, CustomerOnRegister>();
                if (inputs.AccessToken!=null)
                {
                    Customer customer = customerRepo.GetCustomerByAccessToken(inputs.AccessToken);//customerRepo.GetCustomerByAccessToken(inputs.AccessToken);
                    if(customer.CustomerId!="-1")
                    {
                        if (customerRepo.ResetPassword(inputs.AccessToken, inputs.NewPassword))
                        {
                            customer = customerRepo.GetCustomerById(customer.CustomerId);
                            response.Content = new StringContent("{\"changed\":true,\"oauth\":\""+customer.OAuth+"\",\"status\":200}");
                            response.StatusCode = HttpStatusCode.OK;
                            return response;
                        }
                        else //failure in reset pass
                        {
                            response.Content = new StringContent("{\"changed\":false,\"message\":\"Something went wrong. Could not change password, try again later.\",\"status\":500}");
                            response.StatusCode = HttpStatusCode.InternalServerError;
                            return response;
                        }
                    }//customer id -1
                    else
                    {
                        response.Content = new StringContent("{\"message\":\"Invalid Token\",\"status\":400}");
                        response.StatusCode = HttpStatusCode.BadRequest;
                        return response;
                    }
                }//accesstoken empty
                else if(inputs.Email!=null && inputs.Password!=null)
                {
                    Customer customer= customerRepo.GetCustomer(inputs.Email);
                    if(customer.CustomerId!="-1")
                    {
                        if (customerRepo.ResetPassword(customer.CustomerId,inputs.Password,inputs.NewPassword))
                        {
                            customer = customerRepo.GetCustomer(inputs.Email);
                            response.Content = new StringContent("{\"changed\":true,\"oauth\":\""+customer.OAuth+"\",\"status\":200}");
                            response.StatusCode = HttpStatusCode.OK;
                            return response;
                        }
                        else //failure in reset pass
                        {
                            response.Content = new StringContent("{\"changed\":false,\"message\":\"Something went wrong. Could not change password, try again later.\",\"status\":500}");
                            response.StatusCode = HttpStatusCode.InternalServerError;
                            return response;
                        }
                    }//customerid -1
                    else
                    {
                        response.Content = new StringContent("{\"message\":\"Invalid email\",\"status\":400}");
                        response.StatusCode = HttpStatusCode.BadRequest;
                        return response;
                    }
                }//empty email or password
                else
                {
                    response.Content = new StringContent("{\"message\":\"Invalid parameters\",\"status\":400}");
                    response.StatusCode = HttpStatusCode.BadRequest;
                    return response;
                }
            }//empty newpassword
            else
            {
                response.Content = new StringContent("{\"message\":\"Bad Request,'NewPassword' required\",\"status\":400}");
                response.StatusCode = HttpStatusCode.BadRequest;
                return response;
            }
        }

        /// <summary>
        /// API checks validity of OAuth Key, Returns Customer details when found valid
        /// </summary>
        /// <param name="inputs">OAuth</param>
        /// <returns>Customer Details JSON as http response</returns>
        [HttpGet]
        [ActionName("check")]
        [AuthenticateBasic]
        [PlatformRequired]
        public HttpResponseMessage Check([FromUri] LoginInputs inputs)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            if (inputs.OAuth != null)
            {
                Customer customer = new Customer();
                ICustomerRepository<Customer, CustomerOnRegister> _custrepo = UnityBootstrapper.Resolve<ICustomerRepository<Customer, CustomerOnRegister>>();
                customer = _custrepo.GetCustomerFromAuthKey(inputs.OAuth);

                try
                {
                    if (customer==null || string.IsNullOrWhiteSpace(customer.CustomerId) || customer.CustomerId == "-1")
                    {
                        response.Content = new StringContent("{\"message\":\"Invalid OAuth\",\"status\":401}");
                        response.StatusCode = HttpStatusCode.Unauthorized;
                        return response;
                    }
                }
                catch (Exception err)
                {
                    var objErr = new ExceptionHandler(err, "Social Login Check Method");
                    objErr.LogException();
                } // catch Exception

                response.Content = new StringContent("{\"customerId\":" + customer.CustomerId
                                                    + ",\"email\":\"" + customer.Email
                                                    + "\",\"isEmailVerified\":" + customer.IsEmailVerified.ToString().ToLower()
                                                    + ",\"name\":\"" + customer.Name
                                                    + "\",\"status\":200}");
                response.StatusCode = HttpStatusCode.OK;
                return response;
            }
            else
            {
                response.Content = new StringContent("{\"message\":\"Bad Request,OAuth required\",\"status\":400}");
                response.StatusCode = HttpStatusCode.BadRequest;
                return response;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("signup")]
        [AuthenticateBasic]
        [PlatformRequired]
        public HttpResponseMessage Signup([FromUri] LoginInputs inputs)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);
            string email, name, password;
            //string imgurlthumb,imgurllarge;
            string reEmail = @"^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,6}$";
            string reName = @"^[A-Za-z ]+$";


            if(
                !string.IsNullOrWhiteSpace(nvc["email"])
                &&!string.IsNullOrWhiteSpace(nvc["name"])
                &&!string.IsNullOrWhiteSpace(nvc["pass"])
                &&!string.IsNullOrWhiteSpace(nvc["confpass"])
                &&nvc["pass"].ToString().Length>5
                &&nvc["pass"].ToString()==nvc["confpass"].ToString()
                &&Regex.IsMatch(nvc["email"].ToString().ToLower(), reEmail)
                && Regex.IsMatch(nvc["name"].ToString(), reName)
                )
            {
                //if (!string.IsNullOrWhiteSpace(nvc["imgurlthumb"])) imgurlthumb = nvc["imgurlthumb"].ToString();
                //if (!string.IsNullOrWhiteSpace(nvc["imgurllarge"])) imgurllarge = nvc["imgurllarge"].ToString();
                email = nvc["email"].ToString();
                name = nvc["name"].ToString();
                password = nvc["pass"].ToString();
                try
                {
                    CustomerOnRegister customer = new CustomerOnRegister();
                    customer=RegisterCustomer(name, password, email, "");
                    if(customer.StatusOnRegister=="O")
                    {
                        response.Content = new StringContent("{\"message\":\"User already registered\",\"isRegistered\":false,\"status\":200}");
                        response.StatusCode = HttpStatusCode.BadRequest;
                        return response;
                    }
                    else if(customer.StatusOnRegister=="N")
                    {
                        response.Content = new StringContent("{\"customerId\":"+customer.CustomerId+",\"oauth\":\""+customer.OAuth+"\",\"isRegistered\":true,\"status\":200}");
                        response.StatusCode = HttpStatusCode.OK;
                        return response;
                    }
                }
                catch (Exception err)
                {
                    var objErr = new ExceptionHandler(err, "Social Login 'SignUp' Method");
                    objErr.LogException();
                }
            }
            else
            {
                response.Content = new StringContent("{\"message\":\"Bad Request\",\"isRegistered\":false,\"status\":400}");
                response.StatusCode = HttpStatusCode.BadRequest;
                return response;
            }


            response.Content = new StringContent("{\"changed\":false,\"message\":\"Something went wrong.\",\"isRegistered\":false,\"status\":500}");
            response.StatusCode = HttpStatusCode.BadRequest;
            return response;
        }


        /// <summary>
        /// Used For Registering the user for carwale. for social ,both registering and login
        /// </summary>
        /// <param name="customerName"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <param name="mobile"></param>
        /// <param name="fbId"></param>
        /// <param name="gplusId"></param>
        /// <param name="isOpenUserVerified"></param>
        /// <returns></returns>
        public CustomerOnRegister RegisterCustomer(string customerName, string password, string email, string mobile, string fbId = "", string gplusId = "", bool isOpenUserVerified = false)
        {
            if (customerName == "" || email == "" || (password == "" && fbId == "" && gplusId == ""))
            {
                return new CustomerOnRegister() { CustomerId = "", OAuth = "" };
            }


            CustomerOnRegister customer = new CustomerOnRegister();
            ICustomerBL<Customer, CustomerOnRegister> customerRepo = new CustomerActions<Customer, CustomerOnRegister>();
            try
            {
                customer = customerRepo.CreateCustomer(new Customer()
                {
                    Name = customerName,
                    Email = email,
                    Mobile = !string.IsNullOrEmpty(mobile) ? mobile : "",
                    Password = password,
                    FacebookId = fbId,
                    GoogleId = gplusId,
                    openUserVerified = isOpenUserVerified
                });
                return customer;
            }
            catch (Exception err)
            {
                HttpContext objTrace = HttpContext.Current;
                ErrorClass objErr = new ErrorClass(err, objTrace.Request.ServerVariables["URL"]);
                objErr.SendMail();
                return new CustomerOnRegister() { CustomerId = "-1", OAuth = "" };
            } 
        }

    }
}
