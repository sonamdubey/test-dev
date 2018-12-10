using AEPLCore.Cache;
using Carwale.DAL.CoreDAL;
using Carwale.DAL.Security;
using Carwale.Entity.MobileAppAlerts;
using Carwale.Entity.Security;
using Carwale.Interfaces;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Linq;
using AEPLCore.Cache.Interfaces;

namespace Carwale.Service.Filters
{

    public class ApiAuthorizationAttribute : AuthorizationFilterAttribute
    {
        ICacheManager _memcache = new CacheManager();
        ISecurityRepository<string> _securityRepo = new SecurityRepository<string>();
        HttpActionContext _actionContext { get; set; }
        string _timeStampString { get; set; }
        string _authenticationString { get; set; }
        string _userName { get; set; }
        string _passWord { get; set; }
        string _contentHash { get; set; }
        string _baseString { get; set; }


        public override void OnAuthorization(HttpActionContext actionContext)
        {
            _actionContext = actionContext;
            bool isAuthenticated = IsAuthenticated();

            if (!isAuthenticated)
            {
                var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                actionContext.Response = response;
            }
        }

        private bool IsAuthenticated()
        {
            var headers = _actionContext.Request.Headers;

            _timeStampString = GetHttpRequestHeader(headers, "ts").ToString();
            

            if (string.IsNullOrEmpty(_timeStampString))
                return false;

            _authenticationString = GetHttpRequestHeader(headers, "Authorization").Replace("basic","").Trim();
            if (string.IsNullOrEmpty(_authenticationString))
                return false;


           _userName = GetHttpRequestHeader(headers, "un");
           if (string.IsNullOrEmpty(_userName))
                return false;

            _contentHash = GetHttpRequestHeader(_actionContext.Request.Content.Headers, "Content-MD5");
            if (string.IsNullOrEmpty(_contentHash))
                return false;

            _passWord = _memcache.GetFromCache<string>(_userName, TimeSpan.FromDays(1), () => _securityRepo.GetPassword(_userName));
            _baseString = BuildBaseString();

            return VerifyRequest();
        }

        private string ComputeHash()
        {            
            byte[] key = Encoding.UTF8.GetBytes(Sha1Hash(_passWord));            

            using (var hmac = new HMACSHA256(key))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(_baseString));
                return Convert.ToBase64String(hash);
            }
        }

        private static string GetHttpRequestHeader(HttpHeaders headers, string headerName)
        {
            try
            {
                if (!headers.Contains(headerName))
                    return string.Empty;
                return ((string[])(headers.GetValues(headerName)))[0];
            }
            catch (Exception ex) 
            {
                throw ex;
            }
        }

        private string BuildBaseString()
        {
            string methodType = _actionContext.Request.Method.Method;
            var absolutePath = GetAbsoluteURIWithoutPort();
            string message = String.Join("\n", methodType, _contentHash, _timeStampString,_userName, absolutePath);
            return message;
        }

        private string GetAbsoluteURIWithoutPort()
        {
            var absolutePath = _actionContext.Request.RequestUri.AbsoluteUri.ToLower();
            var urlBuilder = new UriBuilder(absolutePath);
            urlBuilder.Port = -1;

            return urlBuilder.Uri.ToString();
        }

        private  bool VerifyRequest()
        {
            if (string.IsNullOrEmpty(_passWord))
                return false;

            var verifiedHash = ComputeHash();
            if (_authenticationString != null && _authenticationString.Equals(verifiedHash))
                return true;

            return false;
        }

        string Sha1Hash(string password)
        {
            return string.Join("", SHA1CryptoServiceProvider.Create().ComputeHash(Encoding.UTF8.GetBytes(password)).Select(x => x.ToString("x2")));
        }
    }
}
