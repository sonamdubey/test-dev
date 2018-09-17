using ApiGatewayLibrary;
using Bikewale.BAL.GrpcFiles;
using Bikewale.Entities.Videos;
using Bikewale.Interfaces.Videos;
using Bikewale.Notifications;
using Bikewale.Utility;
using EditCMSWindowsService.Messages;
using Google.Protobuf;
using System;
using AEPLCore.Utils.Serializer;

namespace Bikewale.Models.Videos
{
    /// <summary>
    /// Created by : Aditi Srivasatva on 25 Mar 2017
    /// Summary    : Model to get videos by subcategory
    /// </summary>
    public class VideosBySubcategory
    {
        private readonly IVideos _videos = null;
        private readonly string EditCMSModuleName = Bikewale.Utility.BWConfiguration.Instance.EditCMSModuleName;
        #region Constructor
        public VideosBySubcategory(IVideos videos)
        {
            _videos = videos;
        }
        #endregion

        #region Functions
        /// <summary>
        /// Created by : Aditi Srivasatva on 25 Mar 2017
        /// Summary    : Get videos by category
        /// </summary>
        public VideosBySubcategoryVM GetData(string sectionBackgroundClass, string categoryIdList, ushort pageNo, ushort pageSize, VideosSortOrder? sortOrder = null)
        {
            VideosBySubcategoryVM objVideos = new VideosBySubcategoryVM();
            System.Diagnostics.Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                objVideos.VideoList = _videos.GetVideosBySubCategory(categoryIdList, pageNo, pageSize, sortOrder);
                objVideos.SectionTitle = VideoTitleDescription.VideoCategoryTitle(categoryIdList);
                objVideos.CategoryIdList = categoryIdList;
                objVideos.SectionBackgroundClass = sectionBackgroundClass;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.Models.Videos.VideosBySubCategory.GetData: CategoryId {0}, PageNo {1}, PageSize {2}, SortOrder {3}", categoryIdList, pageNo, pageSize, sortOrder));
            }
            finally
            {
                watch.Stop();
                long elapsedMs = watch.ElapsedMilliseconds;
                log4net.ThreadContext.Properties[String.Format("TimeTaken_{0}", categoryIdList)] = elapsedMs;
            }
            return objVideos;
        }

        public void SetWidgetDataProperties(string sectionBackgroundClass, string categoryIdList, ByteString payload, out VideosBySubcategoryVM widget)
        {
            VideosBySubcategoryVM objVideos = null;
            try
            {
                if (payload != null && !string.IsNullOrEmpty(categoryIdList))
                {
                    objVideos = new VideosBySubcategoryVM();
                    objVideos.VideoList = GrpcToBikeWaleConvert.ConvertFromGrpcToBikeWale(Serializer.ConvertBytesToMsg<GrpcVideoListEntity>(payload));
                    objVideos.SectionTitle = VideoTitleDescription.VideoCategoryTitle(categoryIdList);
                    objVideos.CategoryIdList = categoryIdList;
                    objVideos.SectionBackgroundClass = sectionBackgroundClass;
                }
                widget = objVideos;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.Videos.AddGrpcCallsToAPIGateway");
                widget = null;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryIds"></param>
        /// <param name="categoryTotalRecords"></param>
        /// <returns></returns>
        public CallAggregator AddGrpcCallsToAPIGateway(string[] categoryIds, ushort[] categoryTotalRecords)
        {
            CallAggregator ca = null;
            try
            {
                bool IsRecordsCountAvailable = categoryTotalRecords != null && categoryTotalRecords.Length > 0;
                if (categoryIds != null && categoryIds.Length > 0)
                {
                    ca = new CallAggregator();
                    for (ushort i = 0; i < categoryIds.Length; i++)
                    {
                        ca.AddCall(EditCMSModuleName, "GetVideosBySubCategories", new GrpcVideosBySubCategoriesURI()
                        {
                            ApplicationId = 2,
                            SubCategoryIds = categoryIds[i],
                            StartIndex = 1,
                            EndIndex = (uint)(IsRecordsCountAvailable ? categoryTotalRecords[i] : 9),
                            SortCategory = GrpcVideoSortOrderCategory.MostPopular
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.Videos.AddGrpcCallsToAPIGateway");
            }

            return ca;

        }



        #endregion
    }
}