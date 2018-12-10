using Carwale.DTOs.CMS;
using Carwale.Entity.CMS.Articles;
using Carwale.Entity.CMS.URIs;
using Carwale.Entity.ViewModels;
using Carwale.Interfaces.CMS.Articles;
using Carwale.Interfaces.Home;
using Carwale.Notifications;
using Carwale.UI.Common;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using System.Web.SessionState;
using System.Linq;
using Carwale.Interfaces.CMS;
using Carwale.Entity;
using Carwale.Entity.CMS;
using Carwale.Interfaces.CarData;
using Carwale.UI.ClientBL;
using Carwale.UI.Filters;
using Carwale.BL.CMS;
using AutoMapper;
using Carwale.Interfaces;
using Carwale.DTOs.CMS.Media;
using Carwale.DTOs.CMS.Articles;
using Carwale.Entity.CMS.Media;
using Carwale.Entity.Enum;
using Carwale.Utility;

namespace Carwale.Controllers
{
    [SessionState(SessionStateBehavior.Disabled)]
    public class NewsListingController : Controller
    {
        private readonly IUnityContainer _container;
        private readonly ICMSContent _contentBL;
        private readonly IVideosBL _videosBL;
        private readonly ICarModels _carModelsBL;
        private readonly ICarModelCacheRepository _carModelsCache;
        private readonly ICarMakesCacheRepository _carMakesCache;
        private readonly IMediaBL _media;
        private ushort pageNumber = 1;
        private ushort maxPageNumber;
        public string category = "news";
        private readonly ushort PageSize = 9;
        public readonly List<int> PhotosCategories = new List<int>(new int[]{(int)CMSContentType.Images});
        public readonly List<int> VideosCategories = new List<int>(new int[] { (int)CMSContentType.Videos });
        private bool isRedirectRequired = false;
        private bool isVideoRedirectRequired = false;


        public NewsListingController(IUnityContainer container, ICMSContent contentBL, IVideosBL videosBL, ICarModels carModelsBL, ICarMakesCacheRepository carMakesCache, ICarModelCacheRepository carModelsCache, IMediaBL media)
        {
            _container = container;
            _contentBL = contentBL;
            _videosBL = videosBL;
            _carModelsBL = carModelsBL;
            _carMakesCache = carMakesCache;
            _carModelsCache = carModelsCache;
            _media = media;
        }

        public ActionResult Index()
        {
            NewsListingDTO Model = new NewsListingDTO();
            try
            {
                category = this.HttpContext.Request.QueryString["cat"] ?? "news";
                Model.category = category;
                pageNumber = Convert.ToUInt16(this.HttpContext.Request.QueryString["page"] ?? "1");
                
                if (this.HttpContext.Request.QueryString["model"] != null)
                {
                    var modelMaskingName = this.HttpContext.Request.QueryString["model"];
                    var cmr = _carModelsBL.FetchModelIdFromMaskingName(modelMaskingName, string.Empty);
                    if(cmr.IsValid)
                    {
                        var details = _carModelsCache.GetModelDetailsById(cmr.ModelId);
                        Model.model = new Car() { name = modelMaskingName, id = cmr.ModelId, displayName = details.ModelName };
                        Model.make = new Car() { id = details.MakeId, name = details.MakeName.ToLower().Replace(" ", ""), displayName = details.MakeName };
                    }
                }
                else if (!String.IsNullOrWhiteSpace(this.HttpContext.Request.QueryString["makeId"]))
                {
                    var details = _carMakesCache.GetCarMakeDetails(Convert.ToInt32(this.HttpContext.Request.QueryString["makeId"]));
                    Model.make = new Car() { id = Convert.ToInt32(this.HttpContext.Request.QueryString["makeId"]),name=Carwale.Utility.Format.FormatURL(details.MakeName),displayName=details.MakeName };
                }
                
                Model.ExpertCategories = CWConfiguration.ExpertCategories;
                Model.FeatureCategories = CWConfiguration.FeatureCategories;
                Model.NewsCategories = CWConfiguration.NewsCategories;
                int cityId;

                int.TryParse(CookiesCustomers.MasterCityId.ToString(),out cityId);

                List<int> All = new List<int>();

                //aggregation of categoryIds for main news page
                All.AddRange(CWConfiguration.ExpertCategories);
                All.AddRange(CWConfiguration.FeatureCategories);
                All.AddRange(CWConfiguration.NewsCategories);//yes news has its categories itself, but features and expert reviews were added on later               

                ushort startIndex = (ushort)((pageNumber - 1) * PageSize + 1); //page x startindex
                ushort endIndex = (ushort)((pageNumber + 1) * PageSize); // page x+1 endindex

                string newsCategoryId = string.Empty;

                string carText = "Car ";

                if (Model.make != null && Model.model != null)
                {
                    carText = Model.make.displayName + " " + Model.model.displayName + " ";
                    isRedirectRequired = true;
                    isVideoRedirectRequired = true;
                }
                else if( Model.make !=null ){
                    carText = Model.make.displayName + " cars ";
                }

                switch (category) {
                    case "all": newsCategoryId = string.Join(",", All.ToArray());
                        ViewBag.Title = carText+"News, Auto News India ";
                        ViewBag.Description = "Check out Latest car News, Auto Launch Updates and Expert Views on Indian Car Industry at CarWale";
                        ViewBag.H1 = "Car News";
                        break;
                    case "expert-reviews": newsCategoryId = string.Join(",", CWConfiguration.ExpertCategories.ToArray());
                        ViewBag.Title = carText+"Road tests, First drives, Expert Reviews of New Cars in India ";
                        ViewBag.Description = "Road testing a car is the only way to know true capabilities of a car. Read our Expert Car Reviews, Car Comparison & Road Tests to know how cars perform on various aspects.";
                        ViewBag.H1 = "Expert Reviews";
                        break;
                    case "features": newsCategoryId = string.Join(",", CWConfiguration.FeatureCategories.ToArray());
                        ViewBag.Title = carText+"Special Reports - Stories, Specials & Travelogues ";
                        ViewBag.Description = "Special Reports section of CarWale brings specials, stories, travelogues and much more.";
                        ViewBag.H1 = "Special Reports";
                        break;
                    case "images": newsCategoryId = string.Join(",", PhotosCategories.ToArray());
                        ViewBag.Title = carText+"photos and images ";
                        ViewBag.Description = "Check all exterior and interior images of cars ";
                        ViewBag.H1 = "Images";
                        Model.IsGallery = true;
                        break;
                    case "videos": newsCategoryId = string.Join(",", VideosCategories.ToArray());
                        ViewBag.Title = carText+"Videos, Expert Video Reviews with Road Test & Car Comparison ";
                        ViewBag.Description = "Check latest car videos, Watch Carwale Expert's take on latest Cars - Features, performance, price, fuel economy, handling and more.";
                        ViewBag.H1 = "Videos";
                        Model.IsGallery = true;
                        break;
                    default: newsCategoryId = string.Join(",", All.ToArray());
                        ViewBag.Title = carText+"News, Auto News India ";
                        ViewBag.Description = "Check out Latest Car News, Auto Launch Updates and Expert Views on Indian Car Industry at CarWale";
                        ViewBag.H1 = "Car News";
                        break;
                }

                Model.pageNumber = pageNumber;
                Model.HasPrevPage = pageNumber != 1;

                if (Request.Url.Query.EndsWith("m=1"))
                {
                    ViewBag.isMobile = true;
                }
                else if (Request.ServerVariables["HTTP_X_REWRITE_URL"] != null)
                {
                    ViewBag.isMobile = DeviceDetectionManager.IsMobile(this.HttpContext);
                    if (ViewBag.isMobile) return Redirect("/m" + Request.ServerVariables["HTTP_X_REWRITE_URL"]);
                }

                

                switch (category)
                {
                    case "images":
                        Model.MediaPage1  = Mapper.Map<Media, MediaDTO>(_media.GetMediaListing(new ArticleByCatURI() { ApplicationId = 1, CategoryIdList = newsCategoryId, StartIndex = startIndex, EndIndex = endIndex, MakeId = Model.make == null ? 0 : Model.make.id, ModelId = Model.model == null ? 0 : Model.model.id }));
                        if (Model.MediaPage1.Photos == null || Model.MediaPage1.Photos.ImageRecordCount <= 0)
                        {
                            if (isVideoRedirectRequired)
                            {
                                return Redirect(String.Format("{0}/images/?make={1}", ViewBag.isMobile ? "/m" : "", Model.make.name));
                            }
                            else
                            {
                                return Redirect(String.Format("{0}/images/", ViewBag.isMobile ? "/m" : ""));
                            }
                        }

                        Model.HasNextPage = Model.MediaPage1.Photos.ImageRecordCount > endIndex - PageSize;
                        maxPageNumber = (ushort) Math.Ceiling(Model.MediaPage1.Photos.ImageRecordCount/(double)PageSize);
                        break;
                    case "videos":
                        Model.MediaPage1 = Mapper.Map<Media, MediaDTO>(_media.GetMediaListing(new ArticleByCatURI() { ApplicationId = 1, CategoryIdList = newsCategoryId, StartIndex = startIndex, EndIndex = endIndex, MakeId = Model.make == null ? 0 : Model.make.id, ModelId = Model.model == null ? 0 : Model.model.id, GetAllMedia = true }));
                        if ( Model.MediaPage1.Videos.VideoRecordCount < 1)
                        {
                            if (isRedirectRequired)
                            {
                                return Redirect(String.Format("{0}/{1}-cars/videos/", ViewBag.isMobile?"/m" : "",Model.make.name));
                            }
                            else
                            {
                                return Redirect(String.Format("{0}/videos/", ViewBag.isMobile ? "/m" : ""));
                            }
                        }
                        Model.HasNextPage = Model.MediaPage1.Videos.VideoRecordCount > endIndex - PageSize;
                        maxPageNumber = (ushort)Math.Ceiling(Model.MediaPage1.Videos.VideoRecordCount / (double)PageSize);
                        break;
                    default:
                        Model.Page1 = Mapper.Map<Carwale.Entity.CMS.Articles.CMSContent, Carwale.DTOs.CMS.Articles.CMSContentDTOV2>(_contentBL.GetContentListByCategory(new ArticleByCatURI() { ApplicationId = 1, CategoryIdList = newsCategoryId, StartIndex = startIndex, EndIndex = endIndex,PageNo=pageNumber,PageSize=PageSize, MakeId = Model.make == null ? 0 : Model.make.id, ModelId = Model.model == null ? 0 : Model.model.id }));
                        if (Model.Page1.Articles.Count <= 0)
                        {
                            if(isRedirectRequired) 
                            {
                                return Redirect(String.Format("/{0}-cars/{1}/", Model.make.name, category));
                            }
                            else
                            {
                                return Redirect(String.Format("/{0}/",category));
                            }
                        }
                        Model.HasNextPage = Model.Page1.RecordCount > endIndex - PageSize;
                        maxPageNumber = (ushort)Math.Ceiling(Model.Page1.RecordCount / (double)PageSize);
                        break;
                }

                List<CMSSubCategoryV2> content = _contentBL.GetContentSegment(new CommonURI { ApplicationId = 1, MakeId = Model.make == null ? 0 : Model.make.id, ModelId = Model.model == null ? 0 : Model.model.id }, (int)Platform.CarwaleDesktop, true);
                
                Model.Segments = Mapper.Map<List<CMSSubCategoryV2>, List<ContentSegmentDTO>>(content);

                Model = SetUpPagination(Model,maxPageNumber);

                return View("~/Views/Editorial/NewsListing.cshtml", Model);
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "NewsListingController.Index()\n Exception : " + ex.Message);
                objErr.LogException();
            }
            return Redirect("/");
        }

        private NewsListingDTO SetUpPagination(NewsListingDTO Model, ushort maxPageNumber)
        {
            int paginationStart = Model.pageNumber;
            for (int i = 1; i <= 2; i++)
            {
                if (paginationStart - 1 >= 1) paginationStart--;
                else break;
            }

            Model.Pages = new List<int>();

            for (int i = paginationStart; i < paginationStart + 5; i++)
            {
                if (i <= maxPageNumber)
                {
                    Model.Pages.Add(i);
                }
                else
                {
                    break;
                }
            }

            return Model;
        }

    }
}