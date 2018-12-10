using App.PopulateDataContracts;
using AppWebApi.Common;
using Carwale.Entity.CMS;
using Carwale.Entity.CMS.Articles;
using Carwale.Interfaces.CMS.Articles;
using Carwale.Service;
using Carwale.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web;

namespace AppWebApi.Models
{
    public class NewsDetail
    {
        /*
         Author: Rakesh Yadav   
         Date Created:01 Aug 2013
         Desc: Declare Properties 
         */
       
        private string NewsId{ get; set; }
        [JsonIgnore]
        public bool ServerErrorOccured { get; set; }
        
        [JsonProperty("smallPicUrl")]
        private string SmallPicUrl { get; set; }

        [JsonProperty("largePicUrl")]
        public string LargePicUrl { get; set; }

        [JsonProperty("mediumPicUrl")]
        public string MediumPicUrl { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
        
        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("displayDate")]
        public string DisplayDate { get; set; }

        //Added by Supriya on 10/6/2014
        [JsonProperty("shareUrl")]
        public string ShareUrl { get; set; }

        [JsonProperty("tinyShareUrl")]
        public string TinyShareUrl { get; set; }

        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }

        [JsonProperty("originalPathImg")]
        public string OriginalImgPath { get; set; }

        [JsonProperty("basicId")]
        public int BasicId { get; set; }

        public List<string> Images = new List<string>();

        //[JsonProperty("htmlItems")]
        public List<HTMLItem> htmlItems = new List<HTMLItem>();
        public NewsItem prevNews = new NewsItem();
        public NewsItem nextNews = new NewsItem();
        HtmlContent htmlContent = new HtmlContent();
        ICMSContent repo = UnityBootstrapper.Resolve<ICMSContent>();

        [JsonIgnore]
        public ArticlePageDetails newspageDetails = new ArticlePageDetails();
        [JsonIgnore]
        public ArticleDetails newsDetails = new ArticleDetails();

        /*
         Author: Rakesh Yadav   
         Date Created:01 Aug 2013
         Desc: Call methods to get detailed news
         */
        public NewsDetail(string id)
        {
            NewsId = id;
            BasicId = Convert.ToInt32(id);            
            ExecuteNewsDetailProcedure(CMSAppId.Carwale);
            if (!ServerErrorOccured)
            {
                ShowDetail();
                int basicId;
                if (int.TryParse(id, out basicId)) repo.QueueUpdateView(basicId);
            }
        }

        /*
         Author: Rakesh Yadav   
         Date Created:01 Aug 2013
         Desc: Execute Stored Procedure
         */
        private void ExecuteNewsDetailProcedure(CMSAppId applicationId)
        {
            try
            {
                newspageDetails = repo.GetContentPages(new Carwale.Entity.CMS.URIs.ArticleContentURI() { BasicId = (ulong)BasicId});
                newsDetails = repo.GetContentDetails(new Carwale.Entity.CMS.URIs.ArticleContentURI() { BasicId = (ulong)BasicId });
                if (newsDetails == null || newspageDetails == null)
                    ServerErrorOccured = true;
            }
            catch(Exception err)
            {
                ServerErrorOccured = true;
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        /*
         Author: Rakesh Yadav   
         Date Created:01 Aug 2013
         Desc: Populate detailed news 
         date modified: 13 Nov 2013
         Desc : add all content to list including images
         */
        private void ShowDetail()
        {
            if (newsDetails != null)
            {
                LargePicUrl = CWConfiguration._imgHostUrl + ImageSizes._600X337 + newsDetails.OriginalImgUrl;
                Title = newsDetails.Title;
                Author = newsDetails.AuthorName;
                DisplayDate = CommonOpn.GetDate(newsDetails.DisplayDate.ToString());
                SmallPicUrl = CWConfiguration._imgHostUrl + ImageSizes._110X61 + newsDetails.OriginalImgUrl;
                MediumPicUrl = CWConfiguration._imgHostUrl + ImageSizes._210X118 + newsDetails.OriginalImgUrl;
                ShareUrl = "https://www.carwale.com" +newsDetails.ArticleUrl;
                HostUrl = CWConfiguration._imgHostUrl;
                OriginalImgPath = newsDetails.OriginalImgUrl;

                string completeContent = string.Empty;
                for (int c = 0; c < newspageDetails.PageList.Count ; c++)
                {
                    if (newspageDetails.PageList.Count > 1)
                        completeContent += String.Format("<h2>{0}</h2>", newspageDetails.PageList[c].PageName);

                    completeContent += newspageDetails.PageList[c].Content;
                }
                PopulateHtmlContent phc = new PopulateHtmlContent(completeContent, htmlContent);
                int htmlItemsCount = htmlContent.HtmlItems.ToArray().Length;
                 //add main image to list
                Images.Add(LargePicUrl);
                for (int i = 0; i < htmlItemsCount; i++)
                {
                    if (htmlContent.HtmlItems[i].Type == "Image")
                        Images.Add(htmlContent.HtmlItems[i].Content);
                    htmlItems.Add(htmlContent.HtmlItems[i]);
                }
            }

            
            if (newsDetails.NextArticle != null && newsDetails.NextArticle.BasicId > 0)
            {   
                var nextArticle = newsDetails.NextArticle;
                nextNews.Title = nextArticle.Title.Replace("&#x20B9;", "₹");
                nextNews.Author = nextArticle.AuthorName;
                nextNews.PubDate = CommonOpn.GetDate(nextArticle.DisplayDate.ToString());
                nextNews.SmallPicUrl = CWConfiguration._imgHostUrl + ImageSizes._110X61 + nextArticle.OriginalImgUrl;
                nextNews.LargePicUrl = CWConfiguration._imgHostUrl + ImageSizes._600X337 + nextArticle.OriginalImgUrl;
                nextNews.MediumPicUrl = CWConfiguration._imgHostUrl + ImageSizes._210X118 + nextArticle.OriginalImgUrl;
                nextNews.DetailUrl = CommonOpn.ApiHostUrl + "NewsDetail/?Id=" + nextArticle.BasicId.ToString();
                nextNews.HostUrl = CWConfiguration._imgHostUrl;
                nextNews.OriginalImgPath = nextArticle.OriginalImgUrl;
                nextNews.BasicId = nextArticle.BasicId;
                nextNews.Description = nextArticle.Description.Replace("&#x20B9;", "₹");
            }

            if (newsDetails.PrevArticle != null && newsDetails.PrevArticle.BasicId > 0)
            {
                var prevArticle = newsDetails.PrevArticle;                
                prevNews.Title = prevArticle.Title.Replace("&#x20B9;", "₹");
                prevNews.Author = prevArticle.AuthorName;
                prevNews.PubDate = CommonOpn.GetDate(prevArticle.DisplayDate.ToString());
                prevNews.SmallPicUrl = CWConfiguration._imgHostUrl + ImageSizes._110X61 + prevArticle.OriginalImgUrl;
                prevNews.LargePicUrl = CWConfiguration._imgHostUrl + ImageSizes._600X337 + prevArticle.OriginalImgUrl;
                prevNews.MediumPicUrl = CWConfiguration._imgHostUrl + ImageSizes._210X118 + prevArticle.OriginalImgUrl;
                prevNews.DetailUrl = CommonOpn.ApiHostUrl + "NewsDetail/?Id=" + prevArticle.BasicId.ToString();
                prevNews.HostUrl = CWConfiguration._imgHostUrl;
                prevNews.OriginalImgPath = prevArticle.OriginalImgUrl;
                prevNews.BasicId = prevArticle.BasicId;
                prevNews.Description = prevArticle.Description.Replace("&#x20B9;", "₹");
            }
        }       
    }
}