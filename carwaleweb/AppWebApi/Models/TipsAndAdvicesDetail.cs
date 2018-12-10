using App.PopulateDataContracts;
using AppWebApi.Common;
using Carwale.DTOs.Autocomplete;
using Carwale.Entity.CMS;
using Carwale.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using Microsoft.Practices.Unity;
using Carwale.Service;
using Carwale.Interfaces.CMS.Articles;

namespace AppWebApi.Models
{
    public class TipsAndAdvicesDetail
    {
        private bool prValid = false;
        private string pagesCountWithPhoto = "0";
        private bool serverErrorOccured = false;
        [JsonIgnore]
        public bool ServerErrorOccured
        {
            get { return serverErrorOccured; }
            set { serverErrorOccured = value; }
        }

        private string BasicId { get; set; }
        private string priority = "1";
        private string Priority
        {
            get { return priority; }
            set { priority = value; }
        }

        private string subCatId = "-1";
        private string SubCatId
        {
            get { return subCatId; }
            set { subCatId = value; }
        }

        private string PageContent { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("authorName")]
        public string AuthorName { get; set; }
        [JsonProperty("displayDate")]
        public string DisplayDate { get; set; }
        [JsonProperty("showGallery")]
        public bool ShowGallery { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        //public string Url { get; set; }

        [JsonProperty("shareUrl")]
        public string ShareUrl { get; set; }

        [JsonProperty("tinyShareUrl")]
        public string TinyShareUrl { get; set; }

        public List<HTMLItem> htmlItems = new List<HTMLItem>();
        HtmlContent htmlContent = new HtmlContent();
        public List<LabelValueDTO> tipsAndAdvicePages = new List<LabelValueDTO>();
        public List<Photo> images = new List<Photo>();
        public TipsAdvice nextTipsAdvices = new TipsAdvice();
        public TipsAdvice prevTipsAdvices = new TipsAdvice();
        /*
         Author: Rakesh Yadav
         Date Created: 16 Oct 2013
         Desc: Load Tips and advices 
         */
        public TipsAndAdvicesDetail(string basicId, string priority, string subCatId)
        {
            if (basicId != null && basicId != "")
                BasicId = basicId;
            if (priority != null && priority != "")
                Priority = priority;

            if (subCatId != null && subCatId != "")
                SubCatId = subCatId;
            NewsDetail nd = new NewsDetail(basicId);
                GetTipsAdvicesDetails(nd);
                LoadTipsAdvicesPages(nd);

                if (priority != null && priority != "")
                    prValid = true;

                if (prValid && Priority == pagesCountWithPhoto)
                    LoadTipsAdvicesPhotos(nd);
                //else
                LoadTipsAdvicesPageContent(nd);
                LoadNextPrevUrl(nd);
        }

        /*
         Author: Rakesh Yadav
         Date Created: 16 Oct 2013
         Desc: load tips and advices details 
         */
        private void GetTipsAdvicesDetails(NewsDetail nd)
        {
            try
            {
                Title = nd.Title;
                AuthorName = nd.Author;
                ShowGallery = nd.Images.Count > 0 ? true : false;
                Description = "";
                ShareUrl = nd.ShareUrl;
                DisplayDate = nd.DisplayDate;
            }
            catch (Exception err)
            {
                ServerErrorOccured = true;
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        /*
         Author: Rakesh Yadav
         Date Created: 16 Oct 2013
         Desc: Load list of pages and url to get page content 
         */
        private void LoadTipsAdvicesPages(NewsDetail nd)
        {
            LabelValueDTO d;
            try
            {
                if (nd.newspageDetails != null && nd.newspageDetails.PageList != null)
                {
                    foreach (var item in nd.newspageDetails.PageList)
                    {
                        if (!string.IsNullOrEmpty(item.PageName))
                        {
                            d = new LabelValueDTO();
                            d.Label = item.PageName;
                            d.Value = CommonOpn.ApiHostUrl + "TipsAndAdvicesDetail/?subCatid=" + SubCatId + "&basicId=" + BasicId + "&priority=" + item.Priority;
                            tipsAndAdvicePages.Add(d);
                        }
                    }
                    if (ShowGallery)
                    {
                        d = new LabelValueDTO();
                        d.Label = "Photos";
                        d.Value = CommonOpn.ApiHostUrl + "TipsAndAdvicesDetail/?subCatid=" + SubCatId + "&basicId=" + BasicId + "&priority=" + (tipsAndAdvicePages.Count + 1);
                        tipsAndAdvicePages.Add(d);
                    }
                }
                pagesCountWithPhoto = tipsAndAdvicePages.Count.ToString();
            }
            catch (Exception err)
            {
                ServerErrorOccured = true;
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        /*
         Author: Rakesh Yadav
         Date Created: 16 Oct 2013
         Desc:  Load photos if available
         */
        private void LoadTipsAdvicesPhotos(NewsDetail nd)
        {
            try
            {
                Photo img;
                img = new Photo();
                img.SmallPicUrl = ImageSizes.CreateImageUrl(nd.HostUrl, ImageSizes._160X89, nd.OriginalImgPath);
                img.LargePicUrl = ImageSizes.CreateImageUrl(nd.HostUrl, ImageSizes._600X337,nd.OriginalImgPath);
                img.HostUrl = nd.HostUrl;
                img.OriginalImgPath = nd.OriginalImgPath;
                images.Add(img);
            }
            catch (Exception err)
            {
                ServerErrorOccured = true;
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        /*
         Author: Rakesh Yadav
         Date Created: 16 Oct 2013
         Desc: load page wise content
         */
        private void LoadTipsAdvicesPageContent(NewsDetail nd)
        {
            try
            {
                htmlItems.AddRange(nd.htmlItems);
                foreach (var item in nd.htmlItems)
                {
                    if (item.Type=="Heading")
                    {
                        htmlItems.Remove(item);
                    }
                }
            }
            catch (Exception err)
            {
                ServerErrorOccured = true;
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        private void LoadNextPrevUrl(NewsDetail nd)
        {
            try
            {
                if (nd.nextNews != null)
                {
                    var nxtnews = nd.nextNews;
                    nextTipsAdvices.Title = nxtnews.Title;
                    nextTipsAdvices.Author = nxtnews.Author;
                    nextTipsAdvices.PubDate = nxtnews.PubDate;
                    nextTipsAdvices.Description = nxtnews.Description;
                    nextTipsAdvices.DetailUrl = CommonOpn.ApiHostUrl + "TipsAndAdvicesDetail/?subCatid=" + SubCatId + "&basicId=" + nxtnews.BasicId + "&priority=1";
                }

                if (nd.prevNews != null)
                {
                    var prvnews = nd.prevNews;
                    prevTipsAdvices.Title = prvnews.Title;
                    prevTipsAdvices.Author = prvnews.Author;
                    prevTipsAdvices.Description = prvnews.Description;
                    prevTipsAdvices.PubDate = prvnews.PubDate;
                    prevTipsAdvices.DetailUrl = CommonOpn.ApiHostUrl + "TipsAndAdvicesDetail/?subCatid=" + SubCatId + "&basicId=" + prvnews.BasicId + "&priority=1";
                }                
            }
            catch (Exception err)
            {
                ServerErrorOccured = true;
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }
    }
}