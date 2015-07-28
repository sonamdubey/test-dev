using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Practices.Unity;
using Bikewale.Entities.CMS;
using Bikewale.Interfaces.CMS;
using Bikewale.BAL.CMS;
using Bikewale.Cache.Core;
using Bikewale.Interfaces.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Cache.BikeData;
using Bikewale.Interfaces.Cache;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Common;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Spire.Pdf;
using System.Threading;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using Bikewale.Interfaces.Feedback;
using Bikewale.BAL.Feedback;
using Bikewale.Notifications;
using Bikewale.Notifications.MailTemplates;
using System.Configuration;

namespace Bikewale
{
    public partial class test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Bikewale.Ajax.AjaxCommon common = new Ajax.AjaxCommon();
            //common.GetModelsNew("PriceQuote", "1");

            //string feedbackEmailTo = ConfigurationManager.AppSettings["feedbackEmailTo"];

            //Trace.Warn("feedbackEmailTo : ", feedbackEmailTo);

            //using (IUnityContainer container = new UnityContainer())
            //{
            //    container.RegisterType<IFeedback, Feedback>();
            //    IFeedback objFeedback = container.Resolve<IFeedback>();
                                                
            //    ComposeEmailBase objEmail = new FeedbackMailer("http://www.bikewale.com", "We are testing");
            //    objEmail.Send(feedbackEmailTo, "BikeWale User Feedback");                
            //}

            //var htmlContent = File.ReadAllText("E:\\work log\\BikeWale\\Online bike booking\\PQ PDF page\\ddpr_1-inline.html"); //String.Format("<body>Hello world: {0}</body>", DateTime.Now);
            //var pdfBytes = (new NReco.PdfGenerator.HtmlToPdfConverter()).GeneratePdf(htmlContent);

            //if (pdfBytes != null)
            //{
            //    Response.ContentType = "Application/pdf";
            //    Response.AddHeader("content-disposition", "attachment;filename=DetailedDealerQuotation.pdf");
            //    Response.BinaryWrite(pdfBytes);
            //    Response.End();
            //}

            //double dLat1 = 38.898556, dLon1 = -77.037852, dLat2 = 38.897147, dLon2 = -77.043934, d = 0;
            ////dLat1 = 18.935255;
            ////dLon1 = 72.836898;

            ////dLat2 = 19.1805744171143;
            ////dLon2 = 72.836051940918;

            //dLat1 = 19.184877; 
            //dLon1 = 72.835770;
            //dLat2 = 18.942250;
            //dLon2 = 72.836790;


            //Utility.Distance objDist = new Utility.Distance();

            //d = objDist.GetDistanceBetweenTwoLocations(dLat1, dLon1, dLat2, dLon2);

            //var R = 6373; // mean radius of the earth (km) at 39 degrees from the equator
            //double dLat1 = 38.898556, dLon1 = -77.037852, dLat2 = 38.897147, dLon2 = -77.043934, lat1, lon1, lat2, lon2, dlat, dlon, a, c, d;

            //// convert coordinates to radians
            //lat1 = deg2rad(dLat1);
            //lon1 = deg2rad(dLon1);
            //lat2 = deg2rad(dLat2);
            //lon2 = deg2rad(dLon2);

            //// find the differences between the coordinates
            //dlon = lon2 - lon1;
            //dlat = lat2 - lat1;
            
            //a = Math.Pow(Math.Sin(dlat / 2), 2) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Pow(Math.Sin(dlon / 2), 2);

            //c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));  // great circle distance in radians

            //d = R * c; // great circle distance in km

            
            //int recordCount = 0;

            //using (IUnityContainer container = new UnityContainer())
            //{
            //    container.RegisterType<IBikeMaskingCacheRepository<BikeModelEntity, int>, BikeModelMaskingCache<BikeModelEntity, int>>()
            //             .RegisterType<ICacheManager, MemcacheManager>()
            //             .RegisterType<IBikeModelsRepository<BikeModelEntity, int>, IBikeModelsRepository<BikeModelEntity, int>>()
            //            ;
            //    var objCache = container.Resolve<IBikeMaskingCacheRepository<BikeModelEntity, int>>();

            //    ModelMaskingResponse objResponse = objCache.GetModelMaskingResponse("xtreme");
            //}

            //RegisterCustomer objCust = new RegisterCustomer();
            //string email = objCust.DecryptPasswordToken("lCcQYUN9CnNbdR3cBrpx/UIl85FBs0V0RlrynjjrbEo=");


            //Spire.Pdf.PdfDocument doc = new Spire.Pdf.PdfDocument();
            ////String url = "http://www.london2012.com/news/articles/paralympic-torch-relay-route-revealed-1258473.html";
            //String url = "http://localhost:84/index.html";
            //url = "http://www.bikewale.com/suzuki-bikes/gixxer/";
            //url = "http://localhost:84/ddpr_1-inline.html";
            //Thread thread = new Thread(() =>
            //{ doc.LoadFromHTML(url, false, true, true); });
            //thread.SetApartmentState(ApartmentState.STA);            
            //thread.Start();
            //thread.Join();
            ////Save pdf file. 
            ////doc.LoadFromFile(this.Server.MapPath("/sample.pdf"));
            //PdfPageSettings settings = new PdfPageSettings();
            
            ////doc.SaveToFile("sample.pdf");
            //doc.SaveToHttpResponse("sample.pdf", this.Response, HttpReadType.Save);
            
            //doc.Close();
            //Launching the Pdf file.
            //System.Diagnostics.Process.Start("sample.pdf");

        } 
       
        // convert degrees to radians
	    private double deg2rad(double deg) {
		    double rad = deg * Math.PI/180; // radians = degrees * pi/180
		    return rad;
	    }
	
	
	    // round to the nearest 1/1000
	    private double round(double x) {
		    return Math.Round( x * 1000) / 1000;
	    }

        //public static Task<bool> PostAsync<T>(string hostUrl, string requestType, string apiUrl, T postObject)
        //{
        //    //bool isSuccess = false;
        //    Task<bool> t = default(Task<bool>);

        //    using (var client = new HttpClient())
        //    {
        //        // TODO - Send HTTP requests
        //        client.BaseAddress = new Uri(hostUrl); ;
        //        client.DefaultRequestHeaders.Accept.Clear();
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(requestType));
        //        // HTTP POST

        //        HttpResponseMessage response = client.PostAsJsonAsync(apiUrl, postObject).Result;

        //        if (response.IsSuccessStatusCode)
        //        {
        //            //// Get the URI of the created resource.
        //            //Uri gizmoUrl = response.Headers.Location;
                    
        //        }
        //    }
        //    return t;

        //}
    }
}