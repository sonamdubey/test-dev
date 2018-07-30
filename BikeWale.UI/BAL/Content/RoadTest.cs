using Bikewale.BAL.GrpcFiles;
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
    /// <summary>
    /// Author : Vivek Gupta on 18-07-2016
    /// Desc: this class created for caching and used to fetch photos
    /// </summary>
    public class RoadTest : IRoadTest
    {
        static bool _useGrpc = Convert.ToBoolean(ConfigurationManager.AppSettings["UseGrpc"]);
        static bool _logGrpcErrors = Convert.ToBoolean(ConfigurationManager.AppSettings["LogGrpcErrors"]);
        static readonly ILog _logger = LogManager.GetLogger(typeof(RoadTest));
        private IEnumerable<ModelImage> objImg = null;

        /// <summary>
        /// Author : Vivek Gupta on 18-07-2016
        /// Desc: this function moved from content/RoadTest/ViewRT.aspx.cs for caching and used to fetch photos
        /// </summary>
        /// <param name="basicId"></param>
        /// <returns></returns>
        public IEnumerable<ModelImage> GetPhotos(int basicId)
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
                        BindPhotosOldWay(basicId);
                    }

                }
                else
                {
                    BindPhotosOldWay(basicId);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.CMS.CMSController");
                objErr.SendMail();
                BindPhotosOldWay(basicId);
            }

            return objImg;
        }

        /// <summary>
        /// Author : Vivek Gupta on 18-07-2016
        /// Desc: this function moved from content/RoadTest/ViewRT.aspx.cs for caching and used to fetch photos
        /// </summary>
        /// <param name="basicId"></param>
        /// <returns></returns>
        private IEnumerable<ModelImage> BindPhotosOldWay(int basicId)
        {
            try
            {
                if (_logGrpcErrors)
                {
                    _logger.Error(string.Format("Grpc did not work for GetArticlePhotos {0}", basicId));
                }
                string _apiUrl = "webapi/image/GetArticlePhotos/?basicid=" + basicId;

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
