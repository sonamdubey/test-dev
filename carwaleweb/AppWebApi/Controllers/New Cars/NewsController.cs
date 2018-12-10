using AppWebApi.Common;
using AutoMapper;
using Carwale.DTOs;
using Carwale.Entity;
using Carwale.Entity.CMS.Articles;
using Carwale.Entity.CMS.URIs;
using Carwale.Interfaces.CMS.Articles;
using Carwale.Notifications;
using Carwale.Service;
using Carwale.Service.Filters;
using Carwale.Utility;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace AppWebApi.Controllers
{
    public class NewsListingController : ApiController
    {
        static NewsListingController() {
            Mapper.CreateMap<ArticleSummary, NewsItemDTOEntity>().ForMember(src => src.CWNewsDetailUrl, dest => dest.MapFrom(y => CommonOpn.GetArticleUrlForApp(y.CategoryId.ToString(), y.ArticleUrl.ToString(), y.BasicId.ToString(), y.MaskingName.ToString(), y.MakeName.ToString())))
                                                         .ForMember(src => src.DetailUrl, dest => dest.MapFrom(y => ConfigurationManager.AppSettings["WebApiHostUrl"].ToString() + "NewsDetail?Id=" + y.BasicId))
                                                         .ForMember(src => src.DisplayDate, dest => dest.MapFrom(y => (Convert.ToDateTime(y.DisplayDate)).ConvertDateToDays()))
                                                         .ForMember(src => src.LargeImageUrl, dest => dest.MapFrom(y => y.HostUrl + y.LargePicUrl))
                                                         .ForMember(src => src.SmallImageUrl, dest => dest.MapFrom(y => y.HostUrl + y.SmallPicUrl))
                                                         .ForMember(src => src.ThumbNailImageUrl, dest => dest.MapFrom(y => y.HostUrl + y.SmallPicUrl))
                                                         .ForMember(src => src.Description, dest => dest.MapFrom(y => Format.RemoveHtmlTags(y.Description)))
                                                         .ForMember(src => src.HostUrl, dest => dest.MapFrom(y => y.HostUrl))
                                                         .ForMember(src => src.OriginalImgPath, dest => dest.MapFrom(y => y.OriginalImgUrl))
                                                         .ForMember(src => src.CategoryId, dest => dest.MapFrom(y => y.CategoryId))
                                                         .ForMember(src => src.MakeName, dest => dest.MapFrom(y => y.MakeName))
                                                         .ForMember(src => src.ModelMaskingName, dest => dest.MapFrom(y => y.MaskingName))
                                                         .ForMember(src => src.CategoryId, dest => dest.MapFrom(y => y.CategoryId))
                                                         .ForMember(src => src.CategoryMaskingName, dest => dest.MapFrom(y => y.CategoryMaskingName));
        }

        /// <summary>
        /// Populates the news list based on pageNo & pageSize passed
        /// Written By : Supriya on 2/6/2014
        /// </summary>
        /// <returns></returns>
        [AuthenticateBasic]
        public HttpResponseMessage Get()
        {
            var response = new HttpResponseMessage();

            try
            {
                NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);

                if (!RegExValidations.IsPositiveNumber(nvc["pageNo"].ToString()) || !RegExValidations.IsPositiveNumber(nvc["pageSize"].ToString()))
                {
                    response.Content = new StringContent("Bad Request");
                    return response;
                }

                int pageNo = Convert.ToInt32(nvc["pageNo"]);
                int pageSize = Convert.ToInt32(nvc["pageSize"]);

                int startIndex = (pageNo - 1) * pageSize + 1;
                int endIndex = pageNo * pageSize;

                int totalRecords;

                ContentFilters year = new ContentFilters();
                year.Year = DateTime.Now.Year;

                IUnityContainer container = UnityBootstrapper.Resolver.GetContainer();

                var newsList = container.Resolve<ICMSContent>();

                var queryParam = new ArticleByCatURI() { ApplicationId = 1, CategoryIdList = "1,2,6,8,19,22", StartIndex = Convert.ToUInt16(startIndex), EndIndex = Convert.ToUInt16(endIndex) };

                var content = newsList.GetContentListByCategory(queryParam);
                var contentList = content.Articles;
                totalRecords = Convert.ToInt32(content.RecordCount);

                List<NewsItemDTOEntity> objNewsItemDTOEntity = Mapper.Map<IList<ArticleSummary>, List<NewsItemDTOEntity>>(contentList);

                var objNewsDTO = new NewsDTOEntity();
                objNewsDTO.newsItems = objNewsItemDTOEntity;

                if ((pageNo * pageSize) >= totalRecords)
                    objNewsDTO.NextPageUrl = "";
                else
                    objNewsDTO.NextPageUrl = ConfigurationManager.AppSettings["WebApiHostUrl"].ToString() + "Newslisting?pageNo=" + (pageNo + 1) + "&pageSize=" + pageSize;

                response.Content = new StringContent(JsonConvert.SerializeObject(objNewsDTO));
            }
            catch (Exception ex)
            {

                //ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                //objErr.SendMail();
                ExceptionHandler objErr = new ExceptionHandler(ex, "NewsListingController");
                objErr.LogException();
            }

            return response;
        }
    }
}