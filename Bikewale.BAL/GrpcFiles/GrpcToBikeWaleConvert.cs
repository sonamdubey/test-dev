﻿using Bikewale.DTO.Videos;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Entities.Videos;
using EditCMSWindowsService.Messages;
using Google.Protobuf.Collections;
using log4net;
using System;
using System.Collections.Generic;

namespace Bikewale.BAL.GrpcFiles
{
    /// <summary>
    /// Author: Prasad Gawde
    /// Summary: This class is responsible for converting the GRPC Object into the Format Bikewale needs it to be in for further processing
    /// </summary>
    public class GrpcToBikeWaleConvert
    {
        static readonly ILog log = LogManager.GetLogger(typeof(GrpcToBikeWaleConvert));
        public static CMSContent ConvertFromGrpcToBikeWale(GrpcCMSContent data)
        {
            if (data == null)
                return null;

            try
            {
                CMSContent dataNew = new CMSContent();
                dataNew.RecordCount = data.RecordCount;
                dataNew.Articles = new List<ArticleSummary>();

                foreach (var item in data.Articles.LstGrpcArticleSummary)
                {
                    var curArt = new ArticleSummary();
                    curArt.ArticleUrl = item.ArticleBase.ArticleUrl;
                    curArt.BasicId = item.ArticleBase.BasicId;
                    curArt.Title = item.ArticleBase.Title.Replace("&#x20B9;", "₹");
                    curArt.AuthorName = item.AuthorName;
                    curArt.CategoryId = (ushort)item.CategoryId;
                    curArt.Description = item.Description.Replace("&#x20B9;", "₹");
                    curArt.DisplayDate = ParseDateObject(item.DisplayDate);
                    curArt.FacebookCommentCount = item.FacebookCommentCount;
                    curArt.HostUrl = item.HostUrl;
                    curArt.IsFeatured = false;
                    curArt.IsSticky = item.IsSticky;
                    curArt.LargePicUrl = item.LargePicUrl;
                    curArt.OriginalImgUrl = item.OriginalImgUrl;
                    curArt.SmallPicUrl = item.SmallPicUrl;
                    curArt.Views = item.Views;
                    dataNew.Articles.Add(curArt);
                }
                return dataNew;
            }
            catch (Exception e)
            {
                log.Error(e);
                throw e;
            }

        }

        public static List<ArticleSummary> ConvertFromGrpcToBikeWale(GrpcArticleSummaryList data)
        {
            if (data == null)
                return null;
            try
            {
                List<ArticleSummary> retData = new List<ArticleSummary>();
                ArticleSummary curArticleSummary;
                foreach (var curGrpcArticleSummary in data.LstGrpcArticleSummary)
                {
                    curArticleSummary = new ArticleSummary()
                    {
                        ArticleUrl = curGrpcArticleSummary.ArticleBase.ArticleUrl,
                        AuthorName = curGrpcArticleSummary.AuthorName,
                        BasicId = curGrpcArticleSummary.ArticleBase.BasicId,
                        CategoryId = (ushort)curGrpcArticleSummary.CategoryId,
                        Description = curGrpcArticleSummary.Description.Replace("&#x20B9;", "₹"),
                        DisplayDate = ParseDateObject(curGrpcArticleSummary.DisplayDate),
                        FacebookCommentCount = curGrpcArticleSummary.FacebookCommentCount,
                        HostUrl = curGrpcArticleSummary.HostUrl,
                        //IsFeatured=curGrpcArticleSummary.fedate
                        IsSticky = curGrpcArticleSummary.IsSticky,
                        LargePicUrl = curGrpcArticleSummary.LargePicUrl,
                        OriginalImgUrl = curGrpcArticleSummary.OriginalImgUrl,
                        SmallPicUrl = curGrpcArticleSummary.SmallPicUrl,
                        Title = curGrpcArticleSummary.ArticleBase.Title.Replace("&#x20B9;", "₹"),
                        Views = curGrpcArticleSummary.Views
                    };


                    retData.Add(curArticleSummary);
                }

                return retData;
            }
            catch (Exception e)
            {
                log.Error(e);
                throw e;
            }
        }

        public static DateTime ParseDateObject(string strDateValue)
        {
            //return Convert.ToDateTime(strDateValue);
            DateTime outValue;

            if
                (DateTime.TryParse(Convert.ToString(strDateValue), out outValue))
                return outValue;
            else
            {
                DateTime.TryParse(strDateValue, new System.Globalization.CultureInfo("en-IN"), System.Globalization.DateTimeStyles.AssumeLocal, out outValue);
                return outValue;
            }
        }


        public static List<ModelImage> ConvertFromGrpcToBikeWale(GrpcModelImageList data)
        {
            if (data == null)
                return null;
            try
            {
                List<ModelImage> retData = new List<ModelImage>();
                ModelImage curModelImage;
                foreach (var curGrpcModelImage in data.LstGrpcModelImage)
                {
                    curModelImage = new ModelImage()
                    {
                        AltImageName = curGrpcModelImage.AltImageName,
                        Caption = curGrpcModelImage.Caption,
                        HostUrl = curGrpcModelImage.HostUrl,
                        ImageCategory = curGrpcModelImage.ImageCategory,
                        ImageDescription = curGrpcModelImage.ImageDescription,
                        ImageId = curGrpcModelImage.ImageId,
                        ImageName = curGrpcModelImage.ImageName,
                        ImagePathLarge = curGrpcModelImage.ImagePathLarge,
                        ImagePathThumbnail = curGrpcModelImage.ImagePathThumbnail,
                        ImageTitle = curGrpcModelImage.ImageTitle,
                        MainImgCategoryId = (short)curGrpcModelImage.MainImgCategoryId,
                        MakeBase = ConvertFromGrpcToBikeWale(curGrpcModelImage.MakeBase),
                        ModelBase = ConvertFromGrpcToBikeWale(curGrpcModelImage.ModelBase),
                        OriginalImgPath = curGrpcModelImage.OriginalImgPath
                    };


                    retData.Add(curModelImage);
                }

                return retData;
            }
            catch (Exception e)
            {
                log.Error(e);
                throw e;
            }
        }

        public static BikeMakeEntityBase ConvertFromGrpcToBikeWale(GrpcCarMakeEntityBase grpcMake)
        {
            if (grpcMake != null)
            {
                BikeMakeEntityBase bwMake = new BikeMakeEntityBase()
                {
                    MakeId = grpcMake.MakeId,
                    MakeName = grpcMake.MakeName
                };
                return bwMake;
            }
            else
                return null;
        }

        public static BikeModelEntityBase ConvertFromGrpcToBikeWale(GrpcCarModelEntityBase grpcModel)
        {
            if (grpcModel != null)
            {
                BikeModelEntityBase bwModel = new BikeModelEntityBase()
                {
                    ModelId = grpcModel.ModelId,
                    ModelName = grpcModel.ModelName,
                    MaskingName = grpcModel.MaskingName
                };
                return bwModel;
            }
            else
                return null;
        }

        public static BikeVersionEntityBase ConvertFromGrpcToBikeWale(GrpcCarVersionEntity grpcVersion)
        {
            if (grpcVersion != null)
            {
                BikeVersionEntityBase bwVersion = new BikeVersionEntityBase()
                {
                    VersionId = grpcVersion.Id,
                    VersionName = grpcVersion.Name
                };
                return bwVersion;
            }
            else
                return null;
        }

        public static ArticleDetails ConvertFromGrpcToBikeWale(GrpcArticleDetails grpcAtricleDet)
        {
            if (grpcAtricleDet != null && grpcAtricleDet.ArticleSummary!=null)
            {
                try
                {
                    var artBase = grpcAtricleDet.ArticleSummary.ArticleBase;
                    var artSummary = grpcAtricleDet.ArticleSummary;

                    ArticleDetails bwArticleDetails = new ArticleDetails()
                    {
                        ArticleUrl = artBase.ArticleUrl,
                        Title = artBase.Title.Replace("&#x20B9;", "₹"),
                        BasicId = artBase.BasicId,
                        CategoryId = (ushort)artSummary.CategoryId,
                        AuthorName = artSummary.AuthorName,
                        Description = artSummary.Description.Replace("&#x20B9;", "₹"),
                        DisplayDate = ParseDateObject(artSummary.DisplayDate),
                        FacebookCommentCount = artSummary.FacebookCommentCount,
                        HostUrl = artSummary.HostUrl,
                        IsSticky = artSummary.IsSticky,
                        LargePicUrl = artSummary.LargePicUrl,
                        SmallPicUrl = artSummary.SmallPicUrl,
                        OriginalImgUrl = artSummary.OriginalImgUrl,
                        Views = artSummary.Views,
                        AuthorMaskingName = artSummary.AuthorMaskingName,
                        Content = grpcAtricleDet.Content,
                        PrevArticle = ConvertFromGrpcToBikeWale(grpcAtricleDet.PrevArticle),
                        NextArticle = ConvertFromGrpcToBikeWale(grpcAtricleDet.NextArticle),
                        TagsList = ConvertFromGrpcToBikeWale(grpcAtricleDet.TagsList),
                        VehiclTagsList = ConvertFromGrpcToBikeWale(grpcAtricleDet.VehiclTagsList)
                    };
                    return bwArticleDetails;
                }
                catch (Exception e)
                {
                    log.Error(e);
                    throw e;
                }
            }
            else
                return null;
        }


        public static ArticleSummary ConvertFromGrpcToBikeWale(GrpcArticleSummary data)
        {
            if (data == null || data.ArticleBase == null) return new ArticleSummary();

            try
            {
                var curArt = new ArticleSummary();
                curArt.ArticleUrl = data.ArticleBase.ArticleUrl;
                curArt.BasicId = data.ArticleBase.BasicId;
                curArt.Title = data.ArticleBase.Title.Replace("&#x20B9;", "₹");
                curArt.AuthorName = data.AuthorName;
                //curArt.AuthorMaskingName = data.AuthorMaskingName;
                curArt.CategoryId = (ushort)data.CategoryId;
                //curArt.CategoryMaskingName = data.CategoryMaskingName;
                curArt.Description = data.Description.Replace("&#x20B9;", "₹");
                curArt.DisplayDate = Convert.ToDateTime(data.DisplayDate);
                curArt.FacebookCommentCount = data.FacebookCommentCount;
                curArt.HostUrl = data.HostUrl;
                curArt.IsFeatured = false;
                curArt.IsSticky = data.IsSticky;
                curArt.LargePicUrl = data.LargePicUrl;
                curArt.OriginalImgUrl = data.OriginalImgUrl;
                curArt.SmallPicUrl = data.SmallPicUrl;
                curArt.Views = data.Views;
                //curArt.SubCategory = data.SubCategory;
                return curArt;
            }
            catch (Exception e)
            {
                log.Error(e);
                throw e;
            }
        }

        public static ArticlePageDetails ConvertFromGrpcToBikeWale(GrpcArticlePageDetails grpcAtricleDet)
        {
            if (grpcAtricleDet != null && grpcAtricleDet.ArticleSummary != null)
            {
                try
                {
                    var artBase = grpcAtricleDet.ArticleSummary.ArticleBase;
                    var artSummary = grpcAtricleDet.ArticleSummary;

                    ArticlePageDetails bwArticleDetails = new ArticlePageDetails()
                    {
                        ArticleUrl = artBase.ArticleUrl,
                        Title = artBase.Title.Replace("&#x20B9;", "₹"),
                        BasicId = artBase.BasicId,
                        CategoryId = (ushort)artSummary.CategoryId,
                        AuthorName = artSummary.AuthorName,
                        Description = artSummary.Description.Replace("&#x20B9;", "₹"),
                        DisplayDate = ParseDateObject(artSummary.DisplayDate),
                        FacebookCommentCount = artSummary.FacebookCommentCount,
                        HostUrl = artSummary.HostUrl,
                        IsSticky = artSummary.IsSticky,
                        LargePicUrl = artSummary.LargePicUrl,
                        SmallPicUrl = artSummary.SmallPicUrl,
                        OriginalImgUrl = artSummary.OriginalImgUrl,
                        Views = artSummary.Views,
                        MainImgCaption = grpcAtricleDet.MainImgCaption,
                        IsMainImageSet = grpcAtricleDet.IsMainImageSet,
                        PageList = ConvertFromGrpcToBikeWale(grpcAtricleDet.PageList),
                        TagsList = ConvertFromGrpcToBikeWale(grpcAtricleDet.TagsList),
                        VehiclTagsList = ConvertFromGrpcToBikeWale(grpcAtricleDet.VehiclTagsList)
                    };
                    return bwArticleDetails;
                }
                catch (Exception e)
                {
                    log.Error(e);
                    throw e;
                }
            }
            else
                return null;
        }

        public static List<VehicleTag> ConvertFromGrpcToBikeWale(RepeatedField<GrpcVehicleTag> data)
        {
            if (data == null)
                return null;
            try
            {
                List<VehicleTag> retData = new List<VehicleTag>();
                VehicleTag curVehicleTag;
                foreach (var curGrpcVehicleTag in data)
                {
                    curVehicleTag = new VehicleTag()
                    {
                        MakeBase = ConvertFromGrpcToBikeWale(curGrpcVehicleTag.MakeBase),
                        ModelBase = ConvertFromGrpcToBikeWale(curGrpcVehicleTag.ModelBase),
                        VersionBase = ConvertFromGrpcToBikeWale(curGrpcVehicleTag.VersionBase)
                    };

                    retData.Add(curVehicleTag);
                }

                return retData;
            }
            catch (Exception e)
            {
                log.Error(e);
                throw e;
            }
        }

        public static List<Page> ConvertFromGrpcToBikeWale(RepeatedField<GrpcPage> data)
        {
            if (data == null)
                return null;
            try
            {
                List<Page> retData = new List<Page>();
                Page curPage;
                foreach (var curGrpcPage in data)
                {
                    curPage = new Page()
                    {
                        Content = curGrpcPage.Content,
                        pageId = curGrpcPage.PageId,
                        PageName = curGrpcPage.PageName,
                        Priority = (ushort)curGrpcPage.Priority
                    };

                    retData.Add(curPage);
                }

                return retData;
            }
            catch (Exception e)
            {
                log.Error(e);
                throw e;
            }
        }

        public static ArticleBase ConvertFromGrpcToBikeWale(GrpcArticleBase grpcArtBase)
        {
            if (grpcArtBase != null)
            {
                ArticleBase bwArticleBase = new ArticleBase()
                {
                    ArticleUrl = grpcArtBase.ArticleUrl,
                    BasicId = grpcArtBase.BasicId,
                    Title = grpcArtBase.Title.Replace("&#x20B9;", "₹")
                };
                return bwArticleBase;
            }
            else
                return null;
        }

        public static List<string> ConvertFromGrpcToBikeWale(RepeatedField<string> lstString)
        {
            List<string> retList = new List<string>();
            if (lstString != null)
            {
                retList.AddRange(lstString);
            }
            return retList;
        }

        public static VideosList ConvertFromGrpcToBikeWale(GrpcVideosList data)
        {
            if (data == null)
                return null;

            try
            {
                VideosList retData = new VideosList();

                VideoBase curVid;

                List<VideoBase> lstVideos = new List<VideoBase>();
                foreach (var curGrpcVideo in data.LstGrpcVideos)
                {
                    curVid = new VideoBase()
                    {
                        BasicId = Convert.ToUInt32(curGrpcVideo.BasicId),
                        Description = curGrpcVideo.Description,
                        DisplayDate = curGrpcVideo.DisplayDate,
                        Duration = Convert.ToUInt32(curGrpcVideo.Duration),
                        ImagePath = curGrpcVideo.ImagePath,
                        ImgHost = curGrpcVideo.ImgHost,
                        Likes = Convert.ToUInt32(curGrpcVideo.Likes),
                        MakeName = curGrpcVideo.MakeName,
                        MaskingName = curGrpcVideo.MaskingName,
                        ModelName = curGrpcVideo.ModelName,
                        SubCatId = curGrpcVideo.SubCatId,
                        SubCatName = curGrpcVideo.SubCatName,
                        Tags = curGrpcVideo.Tags,
                        ThumbnailPath = curGrpcVideo.ThumbnailPath,
                        VideoId = curGrpcVideo.VideoId,
                        VideoTitle = curGrpcVideo.VideoTitle,
                        VideoTitleUrl = curGrpcVideo.VideoTitleUrl,
                        VideoUrl = curGrpcVideo.VideoUrl,
                        Views = Convert.ToUInt32(curGrpcVideo.Views)
                    };

                    lstVideos.Add(curVid);
                }
                retData.Videos = lstVideos;
                return retData;
            }
            catch (Exception e)
            {
                log.Error(e);
                throw e;
            }
        }

        public static List<BikeVideoEntity> ConvertFromGrpcToBikeWale(RepeatedField<GrpcVideo> data)
        {
            if (data == null)
                return null;

            List<BikeVideoEntity> lstVideos = new List<BikeVideoEntity>();
            foreach (var curGrpcVideo in data)
            {
                lstVideos.Add(ConvertFromGrpcToBikeWale(curGrpcVideo));
            }

            return lstVideos;
        }

        public static BikeVideoEntity ConvertFromGrpcToBikeWale(GrpcVideo curGrpcVideo)
        {
            if (curGrpcVideo != null)
            {
                try
                {
                    return new BikeVideoEntity()
                    {
                        BasicId = Convert.ToUInt32(curGrpcVideo.BasicId),
                        Description = curGrpcVideo.Description,
                        DisplayDate = curGrpcVideo.DisplayDate,
                        Duration = Convert.ToUInt32(curGrpcVideo.Duration),
                        ImagePath = curGrpcVideo.ImagePath,
                        ImgHost = curGrpcVideo.ImgHost,
                        Likes = Convert.ToUInt32(curGrpcVideo.Likes),
                        MakeName = curGrpcVideo.MakeName,
                        MaskingName = curGrpcVideo.MaskingName,
                        ModelName = curGrpcVideo.ModelName,
                        SubCatId = curGrpcVideo.SubCatId,
                        SubCatName = curGrpcVideo.SubCatName,
                        Tags = curGrpcVideo.Tags,
                        ThumbnailPath = curGrpcVideo.ThumbnailPath,
                        VideoId = curGrpcVideo.VideoId,
                        VideoTitle = curGrpcVideo.VideoTitle,
                        VideoTitleUrl = curGrpcVideo.VideoTitleUrl,
                        VideoUrl = curGrpcVideo.VideoUrl,
                        Views = Convert.ToUInt32(curGrpcVideo.Views)
                    };
                }
                catch (Exception e)
                {
                    log.Error(e);
                    throw e;
                }
            }
            return null;
        }

        public static BikeVideosListEntity ConvertFromGrpcToBikeWale(GrpcVideoListEntity inp)
        {
            if (inp != null)
            {
                BikeVideosListEntity retVal = new BikeVideosListEntity();
                retVal.NextPageUrl = inp.NextPageUrl;
                retVal.PrevPageUrl = inp.PrevPageUrl;
                retVal.TotalRecords = inp.TotalRecords;
                retVal.Videos = ConvertFromGrpcToBikeWale(inp.Videos.LstGrpcVideos);

                return retVal;
            }
            return null;
        }
    }
}