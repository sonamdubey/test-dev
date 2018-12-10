using Carwale.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Linq;

namespace Carwale.Service.Filters
{
    public class AutocompleteQSFilterAttribute : System.Web.Http.Filters.ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext == null)
            {
                actionContext = new HttpActionContext();
            }
            string qs = actionContext.Request.RequestUri.Query;

            if (!string.IsNullOrWhiteSpace(qs))
            {
                NameValueCollection nvc = System.Web.HttpUtility.ParseQueryString(qs);
                string source = nvc["source"];
                string val = nvc["value"];

                if (string.IsNullOrWhiteSpace(source) || string.IsNullOrWhiteSpace(val))
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, "Bad Request");
            }
            else
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, "Bad Request");
        }
    }
    public class NewCarFinderQSFilterAttribute : System.Web.Http.Filters.ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if(actionContext == null)
            {
                actionContext = new HttpActionContext();
            }
            string qs = actionContext.Request.RequestUri.Query;
             
            if (!string.IsNullOrWhiteSpace(qs))
            {
                NameValueCollection nvc = System.Web.HttpUtility.ParseQueryString(qs);
                if (
                    (string.IsNullOrWhiteSpace(nvc["countOnly"]) || nvc["countOnly"].Equals("false")) && (string.IsNullOrWhiteSpace(nvc["pageNo"]) || string.IsNullOrWhiteSpace(nvc["pageSize"]))
                    )
                    {
                        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, "Bad Request");
                    }
            }
            else
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, "Bad Request");
        }

    }
    public class BodyTypesQSFilterAttribute : System.Web.Http.Filters.ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext == null)
            {
                actionContext = new HttpActionContext();
            }
            string qs = actionContext.Request.RequestUri.Query;

            if (!string.IsNullOrWhiteSpace(qs))
            {
                NameValueCollection nvc = System.Web.HttpUtility.ParseQueryString(qs);
                if (!CommonValidation.IsValidBudget(nvc["budget"]))
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, "Bad Request");
                }
            }
           
        }     
    }
    public class FuelTypesQSFilterAttribute : System.Web.Http.Filters.ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext == null)
            {
                actionContext = new HttpActionContext();
            }
            string qs = actionContext.Request.RequestUri.Query;

            if (!string.IsNullOrWhiteSpace(qs))
            {
                List<int> bodyStyleIds = new List<int>();
                NameValueCollection nvc = System.Web.HttpUtility.ParseQueryString(qs);
                if (!string.IsNullOrWhiteSpace(nvc["bodyStyleIds"]) && RegExValidations.ValidateCommaSeperatedNumbers(nvc["bodyStyleIds"]))
                {
                    bodyStyleIds = nvc["bodyStyleIds"].Split(',').Select(int.Parse).Where(x => x >0).ToList();
                }
                if ((!CommonValidation.IsValidBudget(nvc["budget"])) || (!string.IsNullOrWhiteSpace(nvc["bodyStyleIds"]) && bodyStyleIds.Count <=0))
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, "Bad Request");
                }
            }

        }
    }

}