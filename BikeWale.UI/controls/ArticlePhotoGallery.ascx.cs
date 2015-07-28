using Bikewale.Common;
using Bikewale.Entity.CMS.Photos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.Controls
{
    /// <summary>
    /// Created By : Ashwini Todkar on 1 Oct 2014
    /// Control to show gallry of article photos 
    /// </summary>
    public class ArticlePhotoGallery : UserControl
    {
        public int BasicId { get; set; }
        public List<ModelImage> ModelImageList { get; set; }
        protected Repeater rptPhotos;

       // protected void Page_Load(object sender, EventArgs e)
       // {
           // GetArticlePhotos();
       // }



        /// <summary>
        /// Written By : Ashwini Todkar on 24 Sept 2014
        /// Method to fetch photos of a article from api asynchronously
        /// </summary>

        //private async void GetArticlePhotos()
        //{
        //    try
        //    {
                ////using (var client = new HttpClient())
                ////{

                //    //sets the base URI for HTTP requests
                //    string _cwHostUrl = "http://172.16.1.74:9090/";//ConfigurationManager.AppSettings["cwApiHostUrl"];
                //    //client.BaseAddress = new Uri(_cwHostUrl);
                //    string _apiUrl = "webapi/image/GetArticlePhotos/?basicid=" + 1;
                //    string _requestType = "application/json";
                //    List<ModelImage> objImg = null;
                //    //sets the Accept header to "application/json", which tells the server to send data in JSON format.
                //    //client.DefaultRequestHeaders.Accept.Clear();
                //    //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //    // Send HTTP GET requests 
                //    //HttpResponseMessage response = await client.GetAsync("http://172.16.1.74:9090/webapi/image/GetArticlePhotos/?basicid=" + BasicId);
                //    objImg = await BWHttpClient.GetApiResponse<List<ModelImage>>(_cwHostUrl, _requestType, _apiUrl, objImg);

                //    if (objImg != null)
                //BindPhotos(ModelImageList);

                    //response.EnsureSuccessStatusCode();    // Throw if not a success code.

                    //if (response.IsSuccessStatusCode) //Check 200 OK Status
                    //{
                    //    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    //    {
                    //        var objModelImg = await response.Content.ReadAsAsync<ModelImage>();
                    //    }
                    //}
                //}
            //}
            //catch (Exception err)
            //{
            //    Trace.Warn(err.Message);
            //    ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}
       // }

        public void BindPhotos()
        {
            rptPhotos.DataSource = ModelImageList;
            rptPhotos.DataBind();
        }
    }
}