using Bikewale.BAL.GrpcFiles;
using Bikewale.Entities.CMS;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Interfaces.News;
using Bikewale.Notifications;
using Bikewale.Utility;
using Grpc.CMS;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;

namespace Bikewale.BAL.News
{
    public class News : INews
    {
        static bool _useGrpc = Convert.ToBoolean(ConfigurationManager.AppSettings["UseGrpc"]);
        private CMSContent objNews = null;
        static readonly ILog _logger = LogManager.GetLogger(typeof(News));
        static bool _logGrpcErrors = Convert.ToBoolean(ConfigurationManager.AppSettings["LogGrpcErrors"]);

        /// <summary>
        /// Author : Vivek Gupta 
        /// Date : 19-07-2016
        /// Summary : method to fetch news list and total record count from carwale api
        /// </summary>
        public CMSContent GetNews(int _startIndex, int _endIndex)
        {
            string contentTypeList = CommonApiOpn.GetContentTypesString(new List<EnumCMSContentType>() { EnumCMSContentType.News, EnumCMSContentType.AutoExpo2016 });

            try
            {
                if (_useGrpc)
                {
                    var _objGrpcArticle = GrpcMethods.GetArticleListByCategory(contentTypeList, (uint)_startIndex, (uint)_endIndex);

                    if (_objGrpcArticle != null && _objGrpcArticle.RecordCount > 0)
                    {
                        objNews = GrpcToBikeWaleConvert.ConvertFromGrpcToBikeWale(_objGrpcArticle);
                    }
                    else
                    {
                        objNews = GetNewsOldWay(_startIndex, _endIndex, contentTypeList);
                    }
                }
                else
                {
                    objNews = GetNewsOldWay(_startIndex, _endIndex, contentTypeList);
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
                objNews = GetNewsOldWay(_startIndex, _endIndex, contentTypeList);
            }

            return objNews;
        }

        /// <summary>
        /// Author : Vivek Gupta
        /// Date : 19-07-2016
        /// Desc : GRPC caching for news
        /// </summary>
        /// <param name="_startIndex"></param>
        /// <param name="_endIndex"></param>
        /// <param name="contentTypeList"></param>
        private CMSContent GetNewsOldWay(int _startIndex, int _endIndex, string contentTypeList)
        {
            try
            {
                if (_logGrpcErrors)
                {
                    _logger.Error(string.Format("Grpc did not work for News contentTypeList:{0}", contentTypeList));
                }

                string _apiUrl = String.Format("/webapi/article/listbycategory/?applicationid=2&categoryidlist={0}&startindex={1}&endindex={2}", contentTypeList, _startIndex, _endIndex);

                using (Utility.BWHttpClient objClient = new Utility.BWHttpClient())
                {
                    objNews = objClient.GetApiResponseSync<CMSContent>(Utility.APIHost.CW, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, objNews);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return objNews;
        }
    }
}
