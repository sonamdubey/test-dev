using AutoMapper;
using Bikewale.DTO.Videos;
using Bikewale.Entities.Videos;
using Bikewale.Notifications;
using EditCMSWindowsService.Messages;
using System;
using System.Collections.Generic;

namespace Bikewale.Service.AutoMappers.Videos
{
    public class VideosMapper
    {
        internal static List<VideoBase> Convert(List<BikeVideoEntity> objVideoList)
        {
            return Mapper.Map<List<BikeVideoEntity>, List<VideoBase>>(objVideoList);
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 09th Oct 2017
        /// Summary : Convert video grpc data to dto
        /// </summary>
        /// <param name="objVideoList"></param>
        /// <returns></returns>
        internal static DTO.Videos.v2.VideosList ConvertV2(GrpcVideosList objVideoList)
        {
            DTO.Videos.v2.VideosList retData = new DTO.Videos.v2.VideosList();
            try
            {
                DTO.Videos.v2.VideoBase curVid;

                ICollection<DTO.Videos.v2.VideoBase> lstVideos = new List<DTO.Videos.v2.VideoBase>();
                foreach (var curGrpcVideo in objVideoList.LstGrpcVideos)
                {
                    curVid = new DTO.Videos.v2.VideoBase()
                    {
                        DisplayDate = curGrpcVideo.DisplayDate,
                        Likes = System.Convert.ToUInt32(curGrpcVideo.Likes),
                        VideoId = curGrpcVideo.VideoId,
                        VideoTitle = curGrpcVideo.VideoTitle,
                        VideoTitleUrl = curGrpcVideo.VideoTitleUrl,
                        VideoUrl = curGrpcVideo.VideoUrl,
                        Views = System.Convert.ToUInt32(curGrpcVideo.Views)
                    };

                    lstVideos.Add(curVid);
                }
                retData.Videos = lstVideos;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Service.AutoMappers.Videos.VideosMapper: ConvertV2");
               
            }

            return retData;
        }
    }
}