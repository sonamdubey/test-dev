using System;
using System.Web;
using Carwale.UI.Common;
using System.Data;
using System.Text;
using Newtonsoft.Json;
using Carwale.BL.AutoComplete;
using System.Collections.Specialized;
using System.Collections.Generic;
using Carwale.Entity.AutoComplete;
namespace Carwale.UI.AutoComplete
{
    public class AutoComplete : IHttpHandler, System.Web.SessionState.IReadOnlySessionState
    {
        
        #if Ashwini
                string source = string.Empty, textValue = string.Empty, Params = string.Empty;

                public void ProcessRequest(HttpContext context)
                {
                    if (!string.IsNullOrEmpty(context.Request.QueryString["t"]) && !string.IsNullOrEmpty(context.Request.QueryString["s"]))
                    {
                        textValue = context.Request.QueryString["t"].ToString();
                        source = context.Request.QueryString["s"].ToString();
                        if (!string.IsNullOrEmpty(context.Request.QueryString["par"]))
                            Params = context.Request.QueryString["par"].ToString();

                        if (!string.IsNullOrEmpty(context.Request.QueryString["org"]))
                        {
                            context.Response.Write("var result =" + JsonConvert.SerializeObject(Cawale.BL.AutoComplete.GetResults(source, textValue, Params)) + ";");
                            context.Response.ContentType = "application/javascript";
                        }
                        else
                        {
                            context.Response.Write(JsonConvert.SerializeObject(Cawale.BL.AutoComplete.GetResults(source, textValue, Params)));
                            context.Response.ContentType = "text/plain; charset=utf-8";
                        }
                    }
                    else
                        context.Response.Write("query something more meaningful");
                }

                public bool IsReusable
                {
                    get
                    {
                        return false;
                    }
                }
        #endif

        public void ProcessRequest(HttpContext context)
        {
            Carwale.BL.AutoComplete.AutoComplete objAuto = new BL.AutoComplete.AutoComplete();
            NameValueCollection  nvc = new NameValueCollection();
            nvc["source"] = context.Request.QueryString["s"].ToString();
            nvc["value"] = context.Request.QueryString["t"].ToString();
            List<LabelValueOld> objPairs = new List<LabelValueOld>();

            var results = objAuto.GetResults(nvc);
            results.ForEach(item => objPairs.Add(new LabelValueOld() { Value = item.Value,Label = item.Label }));
            context.Response.Write(JsonConvert.SerializeObject(objPairs));
            context.Response.ContentType = "text/plain; charset=utf-8";
        }

        public bool IsReusable
        {
            get { throw new NotImplementedException(); }
        }
    }

    public class LabelValueOld
    {
        [JsonProperty("l")]
        public string Label { get; set; }

        [JsonProperty("v")]
        public string Value { get; set; }
    }
}