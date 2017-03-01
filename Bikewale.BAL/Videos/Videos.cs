using Bikewale.BAL.GrpcFiles;
using Bikewale.Entities.Videos;
using Bikewale.Interfaces.Videos;
using Bikewale.Notifications;
using Bikewale.Utility;
using EditCMSWindowsService.Messages;
using Grpc.CMS;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.BAL.Videos
{
    /// <summary>
    /// Created By : Sushil Kumar  on 18th Febrauary 2016
    /// Summary : Bussiness logic to get videos 
    /// </summary>
    public class Videos : IVideos
    {
        private readonly IVideos videosRepository = null;
        static string _cwHostUrl;
        static string _requestType;
        static string _applicationid;

        static bool _logGrpcErrors = Convert.ToBoolean(Bikewale.Utility.BWConfiguration.Instance.LogGrpcErrors);
        static readonly ILog _logger = LogManager.GetLogger(typeof(Videos));
        static bool _useGrpc = Convert.ToBoolean(Bikewale.Utility.BWConfiguration.Instance.UseGrpc);

        public Videos()
        {
            _cwHostUrl = BWConfiguration.Instance.CwApiHostUrl;
            _applicationid = BWConfiguration.Instance.ApplicationId;
            _requestType = BWConfiguration.Instance.APIRequestTypeJSON;

        }

        /// <summary>
        /// Created By : Sushil Kumar K
        /// Created On : 18th February 2016
        /// Description : To egt BIke Videos by Category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IEnumerable<BikeVideoEntity> GetVideosByCategory(EnumVideosCategory categoryId, ushort totalCount)
        {
            return GetVideosByCategoryIdViaGrpc((int)categoryId, totalCount);
        }

        /// <summary>
        /// Author: Prasad Gawde
        /// </summary>
        /// <returns></returns>
        private IEnumerable<BikeVideoEntity> GetVideosByCategoryIdViaGrpc(int categoryId, ushort totalCount)
        {
            IEnumerable<BikeVideoEntity> videoDTOList = null;
            try
            {
                if (_useGrpc)
                {
                    var _objVideoList = GrpcMethods.GetVideosBySubCategory((uint)categoryId, 1, totalCount);

                    if (_objVideoList != null)
                    {
                        videoDTOList = GrpcToBikeWaleConvert.ConvertFromGrpcToBikeWale(_objVideoList.LstGrpcVideos);
                    }
                    else
                    {
                        videoDTOList = GetVideosByCategoryOldWay(categoryId, totalCount);
                    }
                }
                else
                {
                    videoDTOList = GetVideosByCategoryOldWay(categoryId, totalCount);
                }
            }
            catch (Exception err)
            {
                _logger.Error(err.Message, err);
                videoDTOList = GetVideosByCategoryOldWay(categoryId, totalCount);
            }

            return videoDTOList;
        }

        public IEnumerable<BikeVideoEntity> GetVideosByCategoryOldWay(int categoryId, ushort totalCount)
        {
            IEnumerable<BikeVideoEntity> objVideosList = null;
            try
            {

                string _apiUrl = String.Format("/api/v1/videos/category/{0}/?appId=2&pageNo=1&pageSize={1}", categoryId, totalCount);

                using (BWHttpClient objclient = new BWHttpClient())
                {
                    objVideosList = objclient.GetApiResponseSync<IEnumerable<BikeVideoEntity>>(APIHost.CW, _requestType, _apiUrl, objVideosList);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return objVideosList;
        }


        /// <summary>
        /// Created By : Sushil Kumar K
        /// Created On : 18th February 2016
        /// Description : To get Bike Videos by Category/Categories
        /// </summary>
        /// <param name="categoryIdList"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public BikeVideosListEntity GetVideosBySubCategory(string categoryIdList, ushort pageNo, ushort pageSize, VideosSortOrder? sortOrder = null)
        {
            BikeVideosListEntity objVideosList = null;
            try
            {
                if (_useGrpc)
                {
                    if (!sortOrder.HasValue)
                        sortOrder = VideosSortOrder.MostPopular;

                    int startIndex, endIndex;
                    Bikewale.Utility.Paging.GetStartEndIndex(pageSize, pageNo, out startIndex, out endIndex);

                    var _objVideoList = GrpcMethods.GetVideosBySubCategories(categoryIdList, (uint)startIndex, (uint)endIndex, sortOrder.Value);

                    if (_objVideoList != null && _objVideoList.TotalRecords > 0)
                    {
                        var pageCount = (int)Math.Ceiling((double)_objVideoList.TotalRecords / (double)pageSize);

                        if (pageNo < pageCount)
                            _objVideoList.NextPageUrl = String.Format("api/v1/videos/subcategory/{0}/?appId={1}&pageNo={2}&pageSize={3}&sortCategory={4}", categoryIdList, 2, pageNo + 1, pageSize, sortOrder.ToString());

                        if (pageNo > 1 && pageNo <= pageCount)
                            _objVideoList.PrevPageUrl = String.Format("api/v1/videos/subcategory/{0}/?appId={1}&pageNo={2}&pageSize={3}&sortCategory={4}", categoryIdList, 2, pageNo - 1, pageSize, sortOrder.ToString());

                        objVideosList = GrpcToBikeWaleConvert.ConvertFromGrpcToBikeWale(_objVideoList);
                    }
                    else
                    {
                        objVideosList = GetVideosBySubCategoryOldWay(categoryIdList, pageNo, pageSize, sortOrder);
                    }
                }
                else
                {
                    objVideosList = GetVideosBySubCategoryOldWay(categoryIdList, pageNo, pageSize, sortOrder);
                }
            }
            catch (Exception err)
            {
                _logger.Error(err.Message, err);
                objVideosList = GetVideosBySubCategoryOldWay(categoryIdList, pageNo, pageSize, sortOrder);
            }

            return objVideosList;
        }

        public BikeVideosListEntity GetVideosBySubCategoryOldWay(string categoryIdList, ushort pageNo, ushort pageSize, VideosSortOrder? sortOrder = null)
        {
            BikeVideosListEntity objVideosList = null;

            try
            {
                string _apiUrl = string.Empty;
                if (sortOrder.HasValue)
                {
                    _apiUrl = String.Format("/api/v1/videos/subcategory/{0}/?appId=2&pageNo={1}&pageSize={2}&sortCategory={3}", categoryIdList, pageNo, pageSize, sortOrder);
                }
                else
                {
                    _apiUrl = String.Format("/api/v1/videos/subcategory/{0}/?appId=2&pageNo={1}&pageSize={2}", categoryIdList, pageNo, pageSize);
                }


                using (BWHttpClient objclient = new BWHttpClient())
                {
                    objVideosList = objclient.GetApiResponseSync<BikeVideosListEntity>(APIHost.CW, _requestType, _apiUrl, objVideosList);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return objVideosList;
        }

        /// <summary>
        /// Created By : Sushil Kumar K
        /// Created On : 18th February 2016
        /// Description : To get Bike Similar Videos  based on videoBasic Id
        /// </summary>
        /// <param name="videoId"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IEnumerable<BikeVideoEntity> GetSimilarVideos(uint videoId, ushort totalCount)
        {
            return GetSimilarVideosViaGrpc(videoId, totalCount);
        }

        /// <summary>
        /// Author: Prasad Gawde
        /// </summary>
        /// <returns></returns>
        private IEnumerable<BikeVideoEntity> GetSimilarVideosViaGrpc(uint videoId, ushort totalCount)
        {
            IEnumerable<BikeVideoEntity> videoDTOList = null;
            try
            {
                if (_useGrpc)
                {
                    GrpcVideosList _objVideoList;

                    _objVideoList = GrpcMethods.GetSimilarVideos((int)videoId, totalCount);

                    if (_objVideoList != null && _objVideoList.LstGrpcVideos.Count > 0)
                    {
                        videoDTOList = GrpcToBikeWaleConvert.ConvertFromGrpcToBikeWale(_objVideoList.LstGrpcVideos);
                    }
                    else
                    {
                        videoDTOList = GetSimilarVideosOldWay(videoId, totalCount);
                    }
                }
                else
                {
                    videoDTOList = GetSimilarVideosOldWay(videoId, totalCount);
                }
            }
            catch (Exception err)
            {
                _logger.Error(err.Message, err);
                videoDTOList = GetSimilarVideosOldWay(videoId, totalCount);
            }

            return videoDTOList;
        }

        public IEnumerable<BikeVideoEntity> GetSimilarVideosOldWay(uint videoId, ushort totalCount)
        {
            IEnumerable<BikeVideoEntity> objVideosList = null;
            try
            {
                string _apiUrl = String.Format("/api/v1/videos/{0}/similar/?appId=2&topCount={1}", videoId, totalCount);
                using (BWHttpClient objclient = new BWHttpClient())
                {
                    objVideosList = objclient.GetApiResponseSync<IEnumerable<BikeVideoEntity>>(APIHost.CW, _requestType, _apiUrl, objVideosList);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return objVideosList;
        }

        /// <summary>
        /// Created By : Sushil Kumar K
        /// Created On : 18th February 2016
        /// Description : To get video details based on videoBasic Id
        /// </summary>
        /// <param name="videoBasicId"></param>
        /// <returns></returns>
        public BikeVideoEntity GetVideoDetails(uint videoBasicId)
        {
            return GetVideoDetailsViaGrpc(videoBasicId);
        }

        /// <summary>
        /// Author: Prasad Gawde
        /// </summary>
        /// <returns></returns>
        private BikeVideoEntity GetVideoDetailsViaGrpc(uint videoId)
        {
            BikeVideoEntity videoDet = null;
            try
            {
                if (_useGrpc)
                {

                    var _objVideo = GrpcMethods.GetVideoByBasicId((int)videoId);

                    if (_objVideo != null)
                    {
                        videoDet = GrpcToBikeWaleConvert.ConvertFromGrpcToBikeWale(_objVideo);
                    }
                    else
                    {
                        videoDet = GetVideoDetailsOldWay(videoId);
                    }
                }
                else
                {
                    videoDet = GetVideoDetailsOldWay(videoId);
                }
            }
            catch (Exception err)
            {
                _logger.Error(err.Message, err);
                videoDet = GetVideoDetailsOldWay(videoId);
            }

            return videoDet;
        }

        private BikeVideoEntity GetVideoDetailsOldWay(uint videoBasicId)
        {
            BikeVideoEntity objVideo = null;
            try
            {
                string _apiUrl = String.Format("/api/v1/videos/{0}/?appId=2", videoBasicId);

                using (BWHttpClient objclient = new BWHttpClient())
                {
                    objVideo = objclient.GetApiResponseSync<BikeVideoEntity>(APIHost.CW, _requestType, _apiUrl, objVideo);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return objVideo;
        }

        /// <summary>
        /// Created By : Lucky Rathore
        /// Created On : 1st March 2016
        /// Description : To get Bike Videos by Bike Make Id 
        /// </summary>
        /// <param name="makeID"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IEnumerable<BikeVideoEntity> GetVideosByMakeModel(ushort pageNo, ushort pageSize, uint makeId, uint? modelId = null)
        {
            return GetVideosByMakeModelViaGrpc(pageNo, pageSize, makeId, modelId);
        }

        /// <summary>
        /// Author: Prasad Gawde
        /// </summary>
        /// <returns></returns>
        private IEnumerable<BikeVideoEntity> GetVideosByMakeModelViaGrpc(ushort pageNo, ushort pageSize, uint makeId, uint? modelId = null)
        {
            IEnumerable<BikeVideoEntity> videoDTOList = null;
            try
            {
                if (_useGrpc)
                {
                    GrpcVideosList _objVideoList;

                    int startIndex, endIndex;
                    Bikewale.Utility.Paging.GetStartEndIndex((int)pageSize, (int)pageNo, out startIndex, out endIndex);

                    if (modelId.HasValue)
                    {
                        _objVideoList = GrpcMethods.GetVideosByModelId((int)modelId.Value, (uint)startIndex, (uint)endIndex);
                    }
                    else
                    {
                        _objVideoList = GrpcMethods.GetVideosByMakeId((int)makeId, (uint)startIndex, (uint)endIndex);
                    }


                    if (_objVideoList != null && _objVideoList.LstGrpcVideos.Count > 0)
                    {
                        videoDTOList = GrpcToBikeWaleConvert.ConvertFromGrpcToBikeWale(_objVideoList.LstGrpcVideos);
                    }
                    else
                    {
                        videoDTOList = GetVideosByMakeModelOldWay(pageNo, pageSize, makeId, modelId);
                    }
                }
                else
                {
                    videoDTOList = GetVideosByMakeModelOldWay(pageNo, pageSize, makeId, modelId);
                }
            }
            catch (Exception err)
            {
                _logger.Error(err.Message, err);
                videoDTOList = GetVideosByMakeModelOldWay(pageNo, pageSize, makeId, modelId);
            }

            return videoDTOList;
        }

        public IEnumerable<BikeVideoEntity> GetVideosByMakeModelOldWay(ushort pageNo, ushort pageSize, uint makeId, uint? modelId = null)
        {
            //BikeVideosListEntity objVideosList = null;
            IEnumerable<BikeVideoEntity> objVideosList = null;
            try
            {
                string _apiUrl = string.Empty;
                if (modelId.HasValue)
                {
                    _apiUrl = String.Format("/api/v1/videos/model/{0}/?appId=2&pageNo={1}&pageSize={2}", modelId, pageNo, pageSize);
                }
                else
                {
                    _apiUrl = String.Format("/api/v1/videos/make/{0}/?appId=2&pageNo={1}&pageSize={2}", makeId, pageNo, pageSize);
                }

                using (BWHttpClient objclient = new BWHttpClient())
                {
                    objVideosList = objclient.GetApiResponseSync<IEnumerable<BikeVideoEntity>>(APIHost.CW, _requestType, _apiUrl, objVideosList);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + "BikeVideosRepository.GetVideosByCategory");
                objErr.SendMail();
            }

            return objVideosList;
        }

        /// <summary>
        /// Created by: Sangram Nandkhile on 28 Feb 2017
        /// Summary: Fetch similar videos by video basic id and totalCount
        /// </summary>
        /// <param name="videoId"></param>
        /// <returns></returns>
        public IEnumerable<BikeVideoEntity> GetSimilarModelsVideos(uint videoId, ushort totalCount)
        {
            List<BikeVideoEntity> objVideosList = null;
            try
            {
                objVideosList = GetSimilarVideos(videoId, totalCount).ToList();
                int fetchedCount = objVideosList.Count;
                if (fetchedCount < totalCount)
                {
                    int remainingCount = totalCount - Convert.ToUInt16(fetchedCount);
                    if (remainingCount > 0)
                    {
                        IEnumerable<BikeVideoEntity> objCategoryVidesos = GetVideosByCategory(EnumVideosCategory.FeaturedAndLatest, (ushort)remainingCount);
                        if (objCategoryVidesos != null && objCategoryVidesos.Count() > 0)
                        {
                            objVideosList.AddRange(objCategoryVidesos);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("BikeVideosRepository.GetSimilarModelsVideos => {0}", videoId));
            }

            return objVideosList;
        }
    }
}
