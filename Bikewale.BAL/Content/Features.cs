using Bikewale.BAL.GrpcFiles;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Interfaces.Content;
using Bikewale.Notifications;
using Grpc.CMS;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;

namespace Bikewale.BAL.Content
{
    public class Features : IFeatures
    {
        static bool _useGrpc = Convert.ToBoolean(ConfigurationManager.AppSettings["UseGrpc"]);
        static readonly ILog _logger = LogManager.GetLogger(typeof(Features));
        static bool _logGrpcErrors = Convert.ToBoolean(ConfigurationManager.AppSettings["LogGrpcErrors"]);
        string cacheKey = "BW_ViewF";
        private ArticlePageDetails objFeature = null;
        private IEnumerable<ModelImage> objImg = null;


        /// <summary>
        /// Author : Vivek Gupta on 18-07-2016
        /// Desc: this function moved from content/features/view.aspx.cs for caching and used for feature details
        /// </summary>
        /// <param name="basicId"></param>
        /// <returns></returns>
        public ArticlePageDetails GetFeatureDetails(int basicId)
        {
            try
            {
                if (_useGrpc)
                {
                    var _objGrpcFeature = GrpcMethods.GetContentPages((ulong)basicId);

                    if (_objGrpcFeature != null)
                    {
                        objFeature = GrpcToBikeWaleConvert.ConvertFromGrpcToBikeWale(_objGrpcFeature);
                    }
                    else
                    {
                        objFeature = GetFeatureDetailsOldWay(basicId);
                    }
                }
                else
                {
                    objFeature = GetFeatureDetailsOldWay(basicId);
                }
            }
            catch (Exception err)
            {
                _logger.Error(err.Message, err);
                objFeature = GetFeatureDetailsOldWay(basicId);
            }

            return objFeature;

        }

        /// <summary>
        /// Author : Vivek Gupta on 18-07-2016
        /// Desc: this function moved from content/features/view.aspx.cs for caching
        /// </summary>
        /// <param name="basicId"></param>
        /// <returns></returns>
        public IEnumerable<ModelImage> Photos(int basicId)
        {
            try
            {
                if (_useGrpc)
                {

                    var _objGrpcArticlePhotos = GrpcMethods.GetArticlePhotos((ulong)basicId);

                    if (_objGrpcArticlePhotos != null && _objGrpcArticlePhotos.LstGrpcModelImage.Count > 0)
                    {
                        //following needs to be optimized
                        objImg = GrpcToBikeWaleConvert.ConvertFromGrpcToBikeWale(_objGrpcArticlePhotos);
                    }
                    else
                    {
                        objImg = BindPhotosOldWay(basicId);
                    }

                }
                else
                {
                    objImg = BindPhotosOldWay(basicId);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : in Photos(int basicId)");
                objErr.SendMail();
                objImg = BindPhotosOldWay(basicId);
            }

            return objImg;
        }

        /// <summary>
        /// Author : Vivek Gupta on 18-07-2016
        /// Desc: this function moved from content/features/view.aspx.cs for caching
        /// </summary>
        /// <param name="basicId"></param>
        /// <returns></returns>
        private ArticlePageDetails GetFeatureDetailsOldWay(int basicId)
        {
            try
            {

                string _apiUrl = "webapi/article/contentpagedetail/?basicid=" + basicId;

                using (Utility.BWHttpClient objClient = new Utility.BWHttpClient())
                {
                    objFeature = objClient.GetApiResponseSync<ArticlePageDetails>(Utility.APIHost.CW, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, objFeature);
                }


                if (_logGrpcErrors && objFeature!=null)
                {
                    _logger.Error(string.Format("Grpc did not work for GetFeatureDetailsOldWay {0}", basicId));
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return objFeature;
        }

        /// <summary>
        /// Author : Vivek Gupta on 18-07-2016
        /// Desc: this function moved from content/features/view.aspx.cs for caching
        /// </summary>
        /// <param name="basicId"></param>
        /// <returns></returns>
        private IEnumerable<ModelImage> BindPhotosOldWay(int basicId)
        {
            string _apiUrl = "webapi/image/GetArticlePhotos/?basicid=" + basicId;

            try
            {
                if (_logGrpcErrors)
                {
                    _logger.Error(string.Format("Grpc did not work for GetArticlePhotos {0}", basicId));
                }

                using (Utility.BWHttpClient objClient = new Utility.BWHttpClient())
                {
                    objImg = objClient.GetApiResponseSync<IEnumerable<ModelImage>>(Utility.APIHost.CW, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, objImg);
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return objImg;
        }
    }
}
