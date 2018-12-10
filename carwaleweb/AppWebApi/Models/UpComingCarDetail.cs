using App.PopulateDataContracts;
using AppWebApi.Common;
using Carwale.DAL.CarData;
using Carwale.Entity.CMS;
using Carwale.Interfaces.CMS.Articles;
using Carwale.Service;
using Carwale.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace AppWebApi.Models
{
    public class UpComingCarDetail : IDisposable
    {
        private bool serverErrorOccured = false;
        [JsonIgnore]
        public bool ServerErrorOccured
        {
            get { return serverErrorOccured; }
            set { serverErrorOccured = value; }
        }

        private string id = "10";
        [JsonProperty("id")]
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        private string makeId = "-1";
        [JsonIgnore]
        public string MakeId
        {
            get { return makeId; }
            set { makeId = value; }
        }

        private string carName = "";
        [JsonProperty("carName")]
        public string CarName
        {
            get { return carName; }
            set { carName = value; }
        }

        private string expectedLaunch = "";
        [JsonProperty("expectedLaunch")]
        public string ExpectedLaunch
        {
            get { return expectedLaunch; }
            set { expectedLaunch = value; }
        }

        private string estimatedPrice = "";
        [JsonProperty("estimatedPrice")]
        public string EstimatedPrice
        {
            get { return estimatedPrice; }
            set { estimatedPrice = value; }
        }

        //private string reviewContent = "";
        //[JsonProperty("reviewContent")]
        //public string ReviewContent
        //{
        //    get { return reviewContent; }
        //    set { reviewContent = value; }
        //}

        private string upcomingCarContent = "";
        [JsonProperty("upcomingCarContent")]
        public string UpcomingCarContent
        {
            get { return upcomingCarContent; }
            set { upcomingCarContent = value; }
        }

        private string prevUrl = "";
        [JsonProperty("prevUrl")]
        public string PrevUrl
        {
            get { return prevUrl; }
            set { prevUrl = value; }
        }

        private string nextUrl = "";
        [JsonProperty("nextUrl")]
        public string NextUrl
        {
            get { return nextUrl; }
            set { nextUrl = value; }
        }

        private string smallpicUrl = "";
        [JsonProperty("smallPicUrl")]
        public string SmallpicUrl
        {
            get { return smallpicUrl; }
            set { smallpicUrl = value; }
        }

        private string largePicUrl = "";
        [JsonProperty("largePicUrl")]
        public string LargePicUrl
        {
            get { return largePicUrl; }
            set { largePicUrl = value; }
        }
        [JsonProperty("originalImgPath")]
        public string OriginalImgPath { get; set; }

        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }

        //Added by supriya on 11/6/2014
        private string shareUrl = "";
        [JsonProperty("shareUrl")]
        public string ShareUrl
        {
            get { return shareUrl; }
            set { shareUrl = value; }
        }

        [JsonProperty("tinyShareUrl")]
        public string TinyShareUrl { get; set; }

        DataSet ds = new DataSet();
        
        public List<string> images = new List<string>();
        public List<HTMLItem> htmlItems = new List<HTMLItem>();
        HtmlContent htmlContent = new HtmlContent();
        CarSynopsisEntity carsynopsis = new CarSynopsisEntity();

        /*
         Author: Rakesh Yadav
         Date Created: 20 Oct 2013
         Desc: Get upcomming car data
         */
        public UpComingCarDetail(string id,string makeId = "-1")
        {
            Id = id;
            MakeId = makeId;

            ExecuteUpCommingCarDetailsProcedure();

            if (!ServerErrorOccured)
            {
                PopulateUpCommingCarData();
                GetNextPrevUrl();
            }
        }

        /*
         Author: Rakesh Yadav
         Date Created: 16 Oct 2013
         Desc: Fetch upcomming car data
         */
        private void ExecuteUpCommingCarDetailsProcedure()
        {        
            CarModelsRepository modelRepo = new CarModelsRepository();
            ds = modelRepo.GetUpcomingCarsForOldVersionApp(Convert.ToInt32(Id), Convert.ToInt32(MakeId));
            ICMSContent _cmsContent = UnityBootstrapper.Resolve<ICMSContent>();
            int modelid =Convert.ToInt32(ds.Tables[0].Rows[0]["ModelId"]);
            carsynopsis = _cmsContent.GetCarSynopsis(modelid, (int)Carwale.Entity.Enum.Application.CarWale);               
        }

        /*
         Author: Rakesh Yadav
         Date Created: 16 Oct 2013
         Desc: Populate Upcomming cars data
         */
        private void PopulateUpCommingCarData()
        {

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dRow = ds.Tables[0].Rows[0];

                CarName = dRow["MakeName"].ToString() + " " + dRow["ModelName"].ToString();
                SmallpicUrl =ImageSizes.CreateImageUrl(dRow["HostUrl"].ToString(), ImageSizes._110X61, dRow["OriginalImgPath"].ToString());
                LargePicUrl = ImageSizes.CreateImageUrl(dRow["HostUrl"].ToString(),ImageSizes._210X118, dRow["OriginalImgPath"].ToString());
                OriginalImgPath = dRow["OriginalImgPath"].ToString();
                HostUrl = dRow["HostUrl"].ToString();
                EstimatedPrice = dRow["EstimatedPriceMin"].ToString() + " - " + dRow["EstimatedPriceMax"].ToString() + "lakh";
                ExpectedLaunch = dRow["ExpectedLaunch"].ToString();
                UpcomingCarContent = carsynopsis.Content;
                ShareUrl = "https://www.carwale.com/" + CommonOpn.FormatSpecial(dRow["MakeName"].ToString()) + "-cars/" + dRow["MaskingName"].ToString() + "/"; 
                PopulateHtmlContent phc = new PopulateHtmlContent(UpcomingCarContent, htmlContent);

                int htmlItemsCount = htmlContent.HtmlItems.ToArray().Length;

                for (int i = 0; i < htmlItemsCount; i++)
                {
                    if (htmlContent.HtmlItems[i].Type == "Image")
                        images.Add(htmlContent.HtmlItems[i].Content);

                    htmlItems.Add(htmlContent.HtmlItems[i]);
                }
                
            }
        }

        /*
         Author: Rakesh Yadav
         Date Created: 16 Oct 2013
         Desc: Make next and prev page url
         */
        private void GetNextPrevUrl()
        {
            string listOfIds = "", prevId = "", nextId = "";
            if (ds.Tables[1].Rows.Count > 0) 
            {
                DataRow dRow = ds.Tables[1].Rows[0];
                listOfIds = dRow[0].ToString() + ",";
            }

            string[] separator = new string[] { "," + Id + "," };
            string[] splited = listOfIds.Split(separator, StringSplitOptions.None);

            if (splited.Length == 2)
            {
                if (splited[0] != "")
                {
                    string[] firstPart = splited[0].Split(',');
                    prevId = firstPart[firstPart.Length - 1];
                }

                if (prevId != "")
                {
                    PrevUrl = CommonOpn.ApiHostUrl + "UpComingCarDetail?id=" + prevId;
                    if (MakeId != "-1")
                        PrevUrl += "&makeId=" + MakeId;
                }
                if (splited[1] != "")
                {
                    string[] secondPart = splited[1].Split(',');
                    nextId = secondPart[0];
                }

                if (nextId != "")
                {
                    NextUrl = CommonOpn.ApiHostUrl + "UpComingCarDetail?id=" + nextId;
                    if (MakeId != "-1")
                        NextUrl += "&makeId=" + MakeId;
                }
            }
        }

        public void Dispose() {
            this.ds.Dispose();
        }
    }
}