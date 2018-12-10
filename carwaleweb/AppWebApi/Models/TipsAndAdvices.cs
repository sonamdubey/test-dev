using AppWebApi.Common;
using Carwale.Entity.CMS;
using Carwale.Entity.CMS.Articles;
using Carwale.Entity.CMS.URIs;
using Carwale.Interfaces.CMS.Articles;
using Carwale.Service;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;

namespace AppWebApi.Models
{
    public class TipsAndAdvices
    {
        private bool serverErrorOccured = false;
        [JsonIgnore]
        public bool ServerErrorOccured
        {
            get { return serverErrorOccured; }
            set { serverErrorOccured = value; }
        }
        private string SubCatId { get; set; }
        private int pageNo = 1;
        [JsonIgnore]
        public int PageNo
        {
            get { return pageNo; }
            set { pageNo = value; }
        }

        private int pageSize = 10;
        [JsonIgnore]
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value; }
        }

        [JsonIgnore]
        public int StartIndex
        {
            get { return (PageNo - 1) * PageSize + 1; }
        }

        [JsonIgnore]
        public int LastIndex
        {
            get { return StartIndex + PageSize - 1; }
        }

        [JsonProperty("nextPageUrl")]
        public string NextPageUrl { get; set; }

        private int TipsAndAdviceCount { get; set; }

        public List<TipsAndAdvicesCategory> tipsAdvicesCategories = new List<TipsAndAdvicesCategory>();
        public List<TipsAdvice> tipsAdvices = new List<TipsAdvice>();

        /*
         Author: Rakesh Yadav
         Date Created: 16 Oct 2013
         Desc: Get different categories and list of tips and advices
         */
        public TipsAndAdvices(string subCatId,string pageNo,string pageSize)
        {
            SubCatId = subCatId;

            if (pageNo != "0" && pageNo != null && pageNo != "")
                PageNo = Convert.ToInt32(pageNo);

            if (pageSize != "0" && pageSize != null && pageSize != "")
                PageSize = Convert.ToInt32(pageSize);

            LoadTipsAdvicesCategory();
            LoadTipsAdvices();
            //GetTipsAndAdviceCount();

            if ((Convert.ToInt32(PageNo) * Convert.ToInt32(PageSize)) >= TipsAndAdviceCount)
                NextPageUrl = "";
            else
                NextPageUrl = CommonOpn.ApiHostUrl + "TipsAndAdvices/?subCatId=" + SubCatId + "&pageNo=" + (PageNo + 1) + "&pageSize=" + PageSize;

        }

        /*
         Author: Rakesh Yadav
         Date Created: 16 Oct 2013
         Desc: Populate categories of tips and advices and generate ulr to get list of tips and advices for that criteria
         */
        private void LoadTipsAdvicesCategory()
        {
            var c = new TipsAndAdvicesCategory();
            c.Label = "All tips & advice";
            c.Value = "-1";
            c.Url = CommonOpn.ApiHostUrl + "TipsAndAdvices/?subCatId=" + c.Value + "&pageNo=1&pageSize=" + PageSize;
            c.ImageUrl = GetSubCatUrl("-1");
            tipsAdvicesCategories.Add(c);

            var categoryId = 5;
            try {
                using (var unityContainer = UnityBootstrapper.Resolver.GetContainer())
                {

                    var _cmsRepo = unityContainer.Resolve<ICMSContent>();
                    var subCategories = _cmsRepo.GetCMSSubCategories(categoryId);
                    foreach (CMSSubCategory subCat in subCategories)
                    {
                        var cat = new TipsAndAdvicesCategory();
                        cat.Label = subCat.SubCategoryName;
                        cat.Value = subCat.SubCategoryId.ToString();
                        cat.Url = CommonOpn.ApiHostUrl + "TipsAndAdvices/?subCatId=" + subCat.SubCategoryId + "&pageNo=1&pageSize=" + PageSize;
                        cat.ImageUrl = GetSubCatUrl(subCat.SubCategoryId.ToString());
                        tipsAdvicesCategories.Add(cat);
                    }
                }
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
         Date Created: 16 Oct 2013
         Desc: Load tips nad advices
         */
        private void LoadTipsAdvices()
        {
            var categoryId = 5;
            try
            {
                using (var unityContainer = UnityBootstrapper.Resolver.GetContainer())
                {
                    ushort subCatId = 0;
                    if (Convert.ToInt16(SubCatId) > 0)
                        subCatId = Convert.ToUInt16(SubCatId);

                    var _cmsRepo = unityContainer.Resolve<ICMSContent>();
                    var contentQuery = new ArticleBySubCatURI()
                    {
                        ApplicationId = (int)CMSAppId.Carwale,
                        CategoryIdList = categoryId.ToString(),
                        SubCategoryId = subCatId,
                        StartIndex = Convert.ToUInt16(StartIndex),
                        EndIndex = Convert.ToUInt16(LastIndex)
                    };

                    var cmsContents=_cmsRepo.GetContentListBySubCategory(contentQuery);

                    foreach(ArticleSummary articleSumary in cmsContents.Articles)
                    {
                        var tipAdvice = new TipsAdvice();
                        tipAdvice.Author=articleSumary.AuthorName;
                        tipAdvice.Description = GetDescription(articleSumary.Description);
                        tipAdvice.DetailUrl = CommonOpn.ApiHostUrl + "TipsAndAdvicesDetail/?subCatid=" + SubCatId + "&basicId=" + articleSumary.BasicId + "&priority=1";
                        tipAdvice.PubDate = CommonOpn.GetDate(articleSumary.DisplayDate.ToString());
                        tipAdvice.Title=articleSumary.Title;
                        tipsAdvices.Add(tipAdvice);
                    }
                    TipsAndAdviceCount = Convert.ToInt32(cmsContents.RecordCount);
                }
            }
            catch (Exception err)
            { 
                ServerErrorOccured = true;
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        private string GetDescription(string desc)
        {
            if (desc.Length > 121)
            {
                desc = desc.Substring(0, 120);
                desc = desc.Substring(0, desc.LastIndexOf(" ")) + " ....";
            }
            return desc;
        }
            
        private string GetSubCatUrl(string catId)
        {
            string retVal = "";
            //string HostUrl = CommonOpn.ApiHostUrl.Replace("/api/", "/images/app/");
            string HostUrl = ConfigurationManager.AppSettings["staticUrl"] != "" ? "https://img1.carwale.com" : "";
            HostUrl += "/images/app/";

            switch (Convert.ToInt32(catId))
            {
                case -1: retVal = "all-icon.png";     /*http://192.168.1.235/images/all-icon.png */
                    break;

                case 26: retVal = "Car insurance.png";     /*http://192.168.1.235/images/all-icon.png */
                         break;

                case 27: retVal = "New car purchase.png";
                         break;

                case 28: retVal = "Used car purchase.png";
                         break;

                case 29: retVal = "Car care.png";
                         break;

                case 30: retVal = "Driving a Car.png";
                         break;

                case 31: retVal = "Safety and security.png";
                         break;

                case 32: retVal = "Car loan.png";
                         break;

                case 39: retVal = "Tyres & Wheels.png";
                         break;

                //default: retVal = "all-icon.png";
                //         break;
            }

            retVal = HostUrl + retVal;

            return retVal;
        }
    }
}