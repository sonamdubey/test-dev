using System;
using System.Collections.Generic;
using Bikewale.DTO.Videos;
using Bikewale.Entities.Authors;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Entities.PWA.Articles;
using Bikewale.Entities.Videos;
using Bikewale.Utility;
using EditCMSWindowsService.Messages;
using Google.Protobuf.Collections;
using log4net;

namespace Bikewale.BAL.GrpcFiles
{
    /// <summary>
    /// Author: Prasad Gawde
    /// Summary: This class is responsible for converting the GRPC Object into the Format Bikewale needs it to be in for further processing
    /// </summary>
    public static class GrpcToBikeWaleConvert
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
                    curArt.AuthorMaskingName = item.AuthorMaskingName;
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
					curArt.SubCategory = item.SubCategory;
					curArt.SubCategoryId = item.SubCategoryId;
                    dataNew.Articles.Add(curArt);
                }
                return dataNew;
            }
            catch (Exception e)
            {
                log.Error(e);
                throw;
            }

        }

        /// Modified by :   Sangram Nandkhile on 01 Dec 2017
        /// Description :   logic to bind Formatted Date and shareurl
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
                    DateTime displayDate = ParseDateObject(curGrpcArticleSummary.DisplayDate);
                    curArticleSummary = new ArticleSummary()
                    {
                        ArticleUrl = curGrpcArticleSummary.ArticleBase.ArticleUrl,
                        AuthorName = curGrpcArticleSummary.AuthorName,
                        BasicId = curGrpcArticleSummary.ArticleBase.BasicId,
                        CategoryId = (ushort)curGrpcArticleSummary.CategoryId,
                        Description = curGrpcArticleSummary.Description.Replace("&#x20B9;", "₹"),
                        DisplayDate = displayDate,
                        FormattedDisplayDate = displayDate.ToString("dd MMMM yyyy"),
                        ShareUrl = Bikewale.Utility.CMSShareUrl.ReturnShareUrl((ushort)curGrpcArticleSummary.CategoryId, curGrpcArticleSummary.ArticleBase.BasicId, curGrpcArticleSummary.ArticleBase.ArticleUrl),
                        FacebookCommentCount = curGrpcArticleSummary.FacebookCommentCount,
                        HostUrl = curGrpcArticleSummary.HostUrl,
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
                throw;
            }
        }

        public static DateTime ParseDateObject(string strDateValue)
        {
            DateTime outValue;

            if
                (DateTime.TryParse(strDateValue, out outValue))
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
                throw;
            }
        }

        /// <summary>
        /// Created by  : Vivek Singh Tomar on 10th Jan 2018
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static List<ModelImages> ConvertFromGrpcToBikeWale(GrpcModelsImageList data)
        {
            if(data == null)
            {
                return new List<ModelImages>();
            }
            try
            {
                List<ModelImages> retData = new List<ModelImages>();
                foreach (var model in data.LstGrpcModelImaegs)
                {
                    List<ModelImage> modelImage = new List<ModelImage>();
                    foreach (var curGrpcModelImage in model.LstGrpcModelImage)
                    {
                        var curModelImage = new ModelImage()
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
                        modelImage.Add(curModelImage);
                    }

                    retData.Add(new ModelImages
                    {
                        ModelId = model.ModelId,
                        RecordCount = model.RecordCount,
                        ModelImage = modelImage
                    });

                }

                return retData;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            return new List<ModelImages>();
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
            if (grpcAtricleDet != null && grpcAtricleDet.ArticleSummary != null)
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
                    throw;
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
                curArt.DisplayDate = ParseDateObject(data.DisplayDate);
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
                throw;
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
                        VehiclTagsList = ConvertFromGrpcToBikeWale(grpcAtricleDet.VehiclTagsList),
                        AuthorMaskingName = artSummary.AuthorMaskingName
                    };
                    return bwArticleDetails;
                }
                catch (Exception e)
                {
                    log.Error(e);
                    throw;
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
                throw;
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
                throw;
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
                    string displayDate = ParseDateObject(curGrpcVideo.DisplayDate).ToString("dd MMMM yyyy");
                    curVid = new VideoBase()
                    {
                        BasicId = Convert.ToUInt32(curGrpcVideo.BasicId),
                        Description = curGrpcVideo.Description,
                        DisplayDate = displayDate,
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
                throw;
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
                        Views = Convert.ToUInt32(curGrpcVideo.Views),
                        ModelId = Convert.ToUInt32(curGrpcVideo.ModelId)
                    };
                }
                catch (Exception e)
                {
                    log.Error(e);
                    throw;
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

        /// <summary>
        /// Created by : Ashutosh Sharma on 20-Sep-2017
        /// Description :  Method to convert grpc author list data to Bikewale author list entity.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static List<AuthorEntityBase> ConvertFromGrpcToBikeWale(GrpcAuthorList data)
        {
            if (data == null) return (new List<AuthorEntityBase>());
            try
            {
                var result = new List<AuthorEntityBase>();
                foreach (var author in data.LstGrpcAuthor)
                {
                    result.Add(
                            new AuthorEntityBase()
                            {
                                AuthorName = author.AuthorName,
                                Designation = author.Designation,
                                HostUrl = author.HostUrl,
                                ImageName = author.ImageName,
                                MaskingName = author.MaskingName,
                                ProfileImage = author.ProfileImage,
                                ShortDescription = author.ShortDescription,
                                AuthorId = author.AuthorId
                            }
                        );
                }
                return result;
            }
            catch (Exception e)
            {
                log.Error(e);
                throw;
            }
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 19th Sep 2017
        /// Summary : Convert Author Details from grpcdata to entity 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static AuthorEntity ConvertFromGrpcToBikeWale(GrpcAuthor data)
        {
            try
            {
                if (data != null)
                {
                    return new AuthorEntity()
                    {
                        AuthorName = data.AuthorName,
                        AuthorFirstName = data.AuthorName.IndexOf(" ") > -1 ? data.AuthorName.Substring(0, data.AuthorName.IndexOf(" ")) : data.AuthorName,
                        Designation = data.Designation,
                        HostUrl = data.HostUrl,
                        ImageName = data.ImageName,
                        MaskingName = data.MaskingName,
                        ProfileImage = data.ProfileImage,
                        ShortDescription = data.ShortDescription,
                        EmailId = data.EmailId,
                        FacebookProfile = data.FacebookProfile,
                        FullDescription = data.FullDescription,
                        GooglePlusProfile = data.GooglePlusProfile,
                        LinkedInProfile = data.LinkedInProfile,
                        TwitterProfile = data.TwitterProfile
                    };
                }
            }
            catch (Exception e)
            {
                log.Error(e);
                throw;
            }
            return null;
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 19th Sep 2017
        /// Summary : Convert Content list from grpc message to entity
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IList<ArticleSummary> ConvertFromGrpcToBikeWale(GrpcAuthorContentList data)
        {
            try
            {
                if (data != null)
                {
                    IList<ArticleSummary> result = new List<ArticleSummary>();
                    foreach (var content in data.LstGrpcAuthorContent)
                    {
                        result.Add
                        (
                            new ArticleSummary()
                            {
                                BasicId = Convert.ToUInt64(content.BasicId),
                                CategoryId = Convert.ToUInt16(content.CategoryId),
                                Title = content.Title,
                                ArticleUrl = content.Url
                            });
                    }
                    return result;
                }
            }
            catch (Exception e)
            {
                log.Error(e);
            }
            return new List<ArticleSummary>();
        }

        #region PWA Conversion code

        public static PwaContentBase ConvertFromGrpcToBikeWalePwa(GrpcCMSContent data)
        {
            if (data == null)
                return null;

            try
            {
                PwaContentBase dataNew = new PwaContentBase();
                dataNew.RecordCount = data.RecordCount;
                dataNew.Articles = new List<PwaArticleSummary>();

                foreach (var item in data.Articles.LstGrpcArticleSummary)
                {
                    var curArt = ConvertFromGrpcToBikeWalePwa(item);
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

        public static PwaArticleDetails ConvertFromGrpcToBikeWalePwa(GrpcArticleDetails inpDet)
        {
            var outDetails = new PwaArticleDetails();
            if (inpDet != null && inpDet.ArticleSummary != null && inpDet.ArticleSummary.ArticleBase.BasicId > 0)
            {
                var artBase = inpDet.ArticleSummary.ArticleBase;
                var artSummary = inpDet.ArticleSummary;
                var dateObj = ParseDateObject(artSummary.DisplayDate);
                var catId = (int)artSummary.CategoryId;
                var basicId = (int)artBase.BasicId;

                outDetails.ArticleUrl = GetArticleUrl(catId, artBase.ArticleUrl, basicId);
                outDetails.BasicId = (ulong)basicId;
                outDetails.Title = artBase.Title.Replace("&#x20B9;", "₹");
                outDetails.AuthorName = artSummary.AuthorName;
                outDetails.AuthorMaskingName = artSummary.AuthorMaskingName;
                outDetails.DisplayDate = dateObj.ToString("MMM dd, yyyy");
                outDetails.DisplayDateTime = dateObj.ToString("MMM dd, yyyy hh:mm tt");
                outDetails.HostUrl = artSummary.HostUrl;
                outDetails.Content = inpDet.Content;
                outDetails.PrevArticle = ConvertFromGrpcToBikeWalePwa(inpDet.PrevArticle);
                outDetails.NextArticle = ConvertFromGrpcToBikeWalePwa(inpDet.NextArticle);
                outDetails.CategoryId = (ushort)catId;
                outDetails.CategoryName = GetContentCategory(catId);
                outDetails.LargePicUrl = artSummary.LargePicUrl;
                outDetails.SmallPicUrl = artSummary.SmallPicUrl;
                outDetails.ArticleApi = string.Format("api/pwa/cms/id/{0}/page/", basicId);
                outDetails.ShareUrl = ReturnShareUrl(inpDet.ArticleSummary);
            }
            return outDetails;
        }

        public static PwaArticleSummary ConvertFromGrpcToBikeWalePwa(GrpcArticleSummary inpSum)
        {
            PwaArticleSummary outSummary = null;
            if (inpSum != null)
            {
                var artBase = inpSum.ArticleBase;
                var basicId = artBase.BasicId;
                var dateObj = ParseDateObject(inpSum.DisplayDate);
                if (basicId > 0)
                {
                    outSummary = new PwaArticleSummary();
                    string catName = GetContentCategory((int)inpSum.CategoryId);
                    outSummary.ArticleUrl = GetArticleUrl((int)inpSum.CategoryId, artBase.ArticleUrl, (int)basicId);
                    outSummary.ArticleApi = string.Format("api/pwa/cms/id/{0}/page/", basicId);
                    outSummary.AuthorName = inpSum.AuthorName;
                    outSummary.Description = inpSum.Description;
                    outSummary.BasicId = basicId;
                    outSummary.Title = artBase.Title.Replace("&#x20B9;", "₹");
                    outSummary.CategoryId = (ushort)inpSum.CategoryId;
                    outSummary.CategoryName = catName;
                    outSummary.DisplayDate = dateObj.ToString("MMM dd, yyyy");
                    outSummary.DisplayDateTime = dateObj.ToString("MMM dd, yyyy hh:mm tt");
                    outSummary.HostUrl = inpSum.HostUrl;
                    outSummary.SmallPicUrl = inpSum.SmallPicUrl;
                    outSummary.LargePicUrl = inpSum.LargePicUrl;
                }
            }
            return outSummary;
        }

        private static string ReturnShareUrl(GrpcArticleSummary articleSummary)
        {
            string shareUrl = string.Empty;
            if (articleSummary != null && articleSummary.ArticleBase != null)
            {
                string _bwHostUrl = BWConfiguration.Instance.BwHostUrlForJs;
                EnumCMSContentType contentType = (EnumCMSContentType)articleSummary.CategoryId;
                var artBase = articleSummary.ArticleBase;


                switch (contentType)
                {
                    case EnumCMSContentType.News:
                    case EnumCMSContentType.AutoExpo2016:
                        shareUrl = string.Format("{0}/news/{1}-{2}.html", _bwHostUrl, artBase.BasicId, artBase.ArticleUrl);
                        break;
                    case EnumCMSContentType.Features:
                        shareUrl = string.Format("{0}/features/{1}-{2}/", _bwHostUrl, artBase.ArticleUrl, artBase.BasicId);
                        break;
                    case EnumCMSContentType.RoadTest:
                        shareUrl = string.Format("{0}/expert-reviews/{1}-{2}.html", _bwHostUrl, artBase.ArticleUrl, artBase.BasicId);
                        break;
                    case EnumCMSContentType.SpecialFeature:
                        shareUrl = string.Format("{0}/features/{1}-{2}/", _bwHostUrl, artBase.ArticleUrl, artBase.BasicId);
                        break;
                    default:
                        break;
                }
            }
            return shareUrl;
        }

        private static string GetContentCategory(int contentType)
        {
            string _category = string.Empty;
            EnumCMSContentType _contentType = (EnumCMSContentType)contentType;

            switch (_contentType)
            {
                case EnumCMSContentType.AutoExpo2016:
                case EnumCMSContentType.News:
                    _category = "NEWS";
                    break;
                case EnumCMSContentType.Features:
                case EnumCMSContentType.SpecialFeature:
                    _category = "FEATURES";
                    break;
                case EnumCMSContentType.ComparisonTests:
                case EnumCMSContentType.RoadTest:
                    _category = "EXPERT REVIEWS";
                    break;
                case EnumCMSContentType.TipsAndAdvices:
                    _category = "Bike Care";
                    break;
                default:
                    break;
            }
            return _category;
        }

        private static string GetArticleUrl(int contentType, string url, int basicid)
        {
            string articleUrl = string.Empty;
            EnumCMSContentType _contentType = (EnumCMSContentType)contentType;

            switch (_contentType)
            {
                case EnumCMSContentType.AutoExpo2016:
                case EnumCMSContentType.News:
                    articleUrl = string.Format("/m/news/{0}-{1}.html", basicid, url);
                    break;
                case EnumCMSContentType.Features:
                case EnumCMSContentType.SpecialFeature:
                    articleUrl = string.Format("/m/features/{0}-{1}/", url, basicid);
                    break;
                case EnumCMSContentType.ComparisonTests:
                case EnumCMSContentType.RoadTest:
                    articleUrl = string.Format("/m/expert-reviews/{0}-{1}.html", url, basicid);
                    break;
                case EnumCMSContentType.TipsAndAdvices:
                    articleUrl = string.Format("/m/bike-care/{0}-{1}.html", url, basicid);
                    break;
                case EnumCMSContentType.Videos:
                    articleUrl = string.Format("/m/videos/{0}-{1}/", url, basicid);
                    break;
                default:
                    break;
            }
            return articleUrl;
        }

        public static List<PwaArticleSummary> ConvertFromGrpcToBikeWalePwa(IEnumerable<GrpcArticleSummary> inpSumList)
        {

            var pwaArticleSummaryList = new List<PwaArticleSummary>();

            foreach (var inpSummary in inpSumList)
            {
                pwaArticleSummaryList.Add(ConvertFromGrpcToBikeWalePwa(inpSummary));
            }
            return pwaArticleSummaryList;
        }

        #endregion
    }
}