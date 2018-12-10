using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Carwale.Service.Filters {
    public class EntityTagContentHashAttribute : ActionFilterAttribute {
        private IEnumerable<string> _receivedEntityTags;

        public override void OnActionExecuting(HttpActionContext actionContext) {

            var conditions = actionContext.Request.Headers.IfNoneMatch;

            if (conditions != null)
            {
                _receivedEntityTags = conditions.Select(t => t.Tag.Trim('"'));
            }
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext) {
            var objectContent = actionExecutedContext.Response.Content as HttpContent;

            if (objectContent == null) return;

            var computedEntityTag = ComputeHash(objectContent.ReadAsStringAsync().Result);

            if (_receivedEntityTags.Contains(computedEntityTag))
            {
                actionExecutedContext.Response.StatusCode = HttpStatusCode.NotModified;
                actionExecutedContext.Response.Content = null;
            }

            actionExecutedContext.Response.Headers.ETag = new EntityTagHeaderValue("\"" + computedEntityTag + "\"", true);
        }

        private static string ComputeHash(object instance) {
            var cryptoServiceProvider = new MD5CryptoServiceProvider();
            var serializer = new DataContractSerializer(instance.GetType());

            using (var memoryStream = new MemoryStream())
            {
                serializer.WriteObject(memoryStream, instance);
                cryptoServiceProvider.ComputeHash(memoryStream.ToArray());

                return String.Join("", cryptoServiceProvider.Hash.Select(c => c.ToString("x2")));
            }
        }
    }
}
