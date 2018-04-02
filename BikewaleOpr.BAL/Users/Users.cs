using Bikewale.Notifications;
using Bikewale.Utility;
using BikewaleOpr.Entity.Users;
using BikewaleOpr.Interface.Users;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BikewaleOpr.BAL.Users
{
    /// <summary>
    /// Author  : Kartik rathod on 30 march 18
    /// Desc    : BAL class for Users
    /// </summary>
    public class Users : IUsers
    {
        
        private const string googleApiTokenInfoUrl = "/oauth2/v3/tokeninfo?id_token={0}";
        private const string userDetailsApi = "/api/userdetails/get?loginId={0}";

        /// <summary>
        /// Author  : Kartik rathod on 30 march 18
        /// Desc    : Google authentication with passed id_token provides authorised emailid
        /// </summary>
        /// <param name="id_token">provided token through google signin api</param>
        /// <returns>valid email id</returns>
        public string GoogleApiAuthentication(string id_token)
        {
            string email = string.Empty;
            
            try
            {
                string requestUri = string.Format(googleApiTokenInfoUrl, id_token);
                Dictionary<string, string> userData = null;

                using (Bikewale.Utility.BWHttpClient objClient = new Bikewale.Utility.BWHttpClient())
                {
                    userData = objClient.GetApiResponseSync<Dictionary<string, string>>(APIHost.GoogleApi, BWConfiguration.Instance.APIRequestTypeJSON, requestUri, userData);
                }

                if (userData != null && userData["email_verified"] == "true" && userData["hd"] == "carwale.com")
                {
                    string emailAdd = userData["email"];
                    if (!string.IsNullOrEmpty(emailAdd))
                    {
                        email = emailAdd.Split('@')[0];
                    }
                        
                }
            }
            catch(Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikewaleOpr.BAL.Users.GoogleApiAuthentication"));
            }
            return email;
        }

        /// <summary>
        /// Author  : Kartik rathod on 30 march 18
        /// Desc    : get opr users details though api/userdetails/get api
        /// </summary>
        /// <param name="loginId">opr loginid column</param>
        /// <returns>UserDetailsEntity</returns>
        public UserDetailsEntity GetUserDetails(string loginId)
        {
            UserDetailsEntity objUserDetailsEntity = null;
            try
            {
                string requestUri = string.Format(userDetailsApi, loginId);
                using (Bikewale.Utility.BWHttpClient objClient = new Bikewale.Utility.BWHttpClient())
                {
                    objUserDetailsEntity = objClient.GetApiResponseSync<UserDetailsEntity>(APIHost.CWOPR, BWConfiguration.Instance.APIRequestTypeJSON, requestUri, objUserDetailsEntity);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikewaleOpr.BAL.Users.GetUserDetails loginId - " + loginId));
            }
            return objUserDetailsEntity;
        }
    }
}
