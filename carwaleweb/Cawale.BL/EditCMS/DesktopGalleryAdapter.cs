using Carwale.DTOs.CarData;
using Carwale.DTOs.CMS.Media;
using Carwale.Entity;
using Carwale.Entity.CarData;
using Carwale.Entity.CMS;
using Carwale.Entity.CMS.Articles;
using Carwale.Entity.CMS.URIs;
using Carwale.Entity.Enum;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.CMS;
using Carwale.Interfaces.CMS.Articles;
using Carwale.Interfaces.CMS.Photos;
using Carwale.Interfaces.NewCars;
using Carwale.Notifications.Logs;
using Carwale.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Carwale.BL.EditCMS
{
    public class DesktopGalleryAdapter : IServiceAdapterV2
    {
        private readonly ICarModels _carModelsBl;
        private readonly IPhotos _photosCacheRepo;
        private readonly ICarModelCacheRepository _modelCache;
        private readonly ICMSContent _contentBL;
        private readonly IMediaBL _mediaBL;
        private readonly IVideosBL _videosBL;

        public DesktopGalleryAdapter(ICarModels carModelsBl, IPhotos photosCacheRepo, ICarModelCacheRepository modelCache, ICMSContent contentBL, IMediaBL mediaBL, IVideosBL videosBL)
        {
            _carModelsBl = carModelsBl;
            _photosCacheRepo = photosCacheRepo;
            _modelCache = modelCache;
            _contentBL = contentBL;
            _mediaBL = mediaBL;
            _videosBL = videosBL;
        }

        public T Get<T, U>(U input)
        {
            return (T)Convert.ChangeType(GetGalleryDTO(input), typeof(T));
        }

        private PhotoGalleryDTO_V2 GetGalleryDTO<U>(U queryParameters)
        {
            try
            {
                Dictionary<string, string> queryString = (Dictionary<string, string>)Convert.ChangeType(queryParameters, typeof(U));

                ModelPhotosBycountURI galleryUri = new ModelPhotosBycountURI();
                galleryUri.ApplicationId = (int)CMSAppId.Carwale;
                int modelId;
                if (!int.TryParse(queryString["modelId"], out modelId) && queryString["cat"] != "videos")                    
                    return null;
                galleryUri.ModelId = modelId;

                PhotoGalleryDTO_V2 galleryDTO = new PhotoGalleryDTO_V2();
                galleryDTO.GalleryState = new GalleryStateDTO();
                if (galleryUri.ModelId > 0)
                    galleryDTO.ModelDetails = _modelCache.GetModelDetailsById(galleryUri.ModelId);
                else
                    galleryDTO.ModelDetails = new CarModelDetails();
                if (queryString.Keys.Contains("cat") && queryString["cat"] == "videos")
                {
                    galleryDTO.GalleryState.ActiveSection = GallerySections.VideosSection;
                    galleryDTO.ModelVideos = new List<Video>();
                    int basicId = queryString.Keys.Contains("basicId") ? int.Parse(queryString["basicId"]) : 0;
                    if (galleryUri.ModelId > 0)
                        galleryDTO.ModelVideos = _videosBL.GetVideosByModelId(galleryUri.ModelId, CMSAppId.Carwale, 1, -1);
                    else if (basicId > 0)
                        galleryDTO.ModelVideos.Add(_videosBL.GetVideoByBasicId(basicId, CMSAppId.Carwale));
                    

                    if (basicId > 0)
                    {
                        int activeVideoIndex = galleryDTO.ModelVideos.FindIndex(video => video.BasicId == basicId);

                        if (activeVideoIndex > 0)
                        {
                            Video currentVideo = galleryDTO.ModelVideos[activeVideoIndex];
                            galleryDTO.ModelVideos.RemoveAt(activeVideoIndex);
                            galleryDTO.ModelVideos.Insert(0, currentVideo);
                        }
                    }
                }
                else
                {
                    galleryUri.CategoryIdList = CMSContentType.RoadTest.ToString("D") + "," + CMSContentType.Images.ToString("D");

                    galleryDTO.ModelImages = _photosCacheRepo.GetModelPhotosByCount(galleryUri);
                    galleryDTO.ModelColors = _modelCache.GetModelColorsByModel(galleryUri.ModelId);
                    galleryDTO.ShowModelColors = _mediaBL.IsModelColorPhotosPresent(galleryDTO.ModelColors);

                    if (!(queryString.Keys.Contains("filter") && queryString["filter"] == "colors") && (galleryDTO.ModelImages == null || galleryDTO.ModelImages.Count == 0))
                        return galleryDTO;

                    ushort count = 0;
                    ushort.TryParse(ConfigurationManager.AppSettings["ExpertReviewsDesktop_Count"], out count);

                    List<ArticleSummary> expertReviews = _contentBL.GetExpertReviewByModel(galleryDTO.ModelDetails.ModelId, count);
              	    galleryDTO.ModelVideos = _videosBL.GetVideosByModelId(galleryUri.ModelId, CMSAppId.Carwale, 1, -1);
                    galleryDTO.ModelDetails.VideoCount = galleryDTO.ModelVideos != null ? galleryDTO.ModelVideos.Count : 0;
                    galleryDTO.SubNavigation = _carModelsBl.GetModelQuickMenu(galleryDTO.ModelDetails, null, false, (expertReviews != null && expertReviews.Count > 0), string.Empty, string.Empty);
                    galleryDTO.SubNavigation.Page = Pages.ImageLanding;
                    galleryDTO.SubNavigation.ShowColorsLink = galleryDTO.ShowModelColors;

                    Tuple<bool, bool> galleryFlags = SetInteriorExteriorFlag(galleryDTO);

                    galleryDTO.IsExteriorPresent = galleryFlags.Item1;
                    galleryDTO.IsInteriorPresent = galleryFlags.Item2;

                    if (queryString.Keys.Contains("filter"))
                    {
                        switch (queryString["filter"])
                        {
                            case "interior":
                                galleryDTO.GalleryState.ActiveFilter = GalleryFilters.InteriorPhotos;
                                galleryDTO.ModelImages = galleryDTO.ModelImages.Where(x => x.MainImgCategoryId == (int)ImageTypes.Interior).ToList();
                                break;
                            case "exterior":
                                galleryDTO.GalleryState.ActiveFilter = GalleryFilters.ExteriorPhotos;
                                galleryDTO.ModelImages = galleryDTO.ModelImages.Where(x => x.MainImgCategoryId == (int)ImageTypes.Exterior).ToList();
                                break;
                            case "colors":
                                galleryDTO.GalleryState.ActiveFilter = GalleryFilters.Colors;
                                break;
                            default:
                                galleryDTO.GalleryState.ActiveFilter = GalleryFilters.AllPhotos;
                                break;
                        }
                    }
                    else
                        galleryDTO.GalleryState.ActiveFilter = GalleryFilters.AllPhotos;

                    if (galleryDTO.GalleryState.ActiveFilter == GalleryFilters.AllPhotos)
                    {
                        galleryDTO.ModelVideos = _videosBL.GetVideosByModelId(galleryUri.ModelId, CMSAppId.Carwale, 1, -1);
                    }

                    galleryDTO.BreadcrumbEntitylist = BindBreadCrumb(galleryDTO);

                    galleryDTO.GalleryState.ActiveSlideIndex = queryString.Keys.Contains("imageName") ? GetCurrentImageIndex(galleryDTO, queryString["imageName"]) : 0;

                    if (galleryDTO.GalleryState.ActiveFilter == GalleryFilters.Colors)
                    {
                        if (galleryDTO.GalleryState.ActiveSlideIndex > 0)
                        {
                            ModelColors currentColorImage = galleryDTO.ModelColors[galleryDTO.GalleryState.ActiveSlideIndex];
                            galleryDTO.ModelColors.RemoveAt(galleryDTO.GalleryState.ActiveSlideIndex);
                            galleryDTO.ModelColors.Insert(0, currentColorImage);
                            galleryDTO.GalleryState.ActiveSlideIndex = 0;
                        }
                        if (!queryString.Keys.Contains("imageName"))
                        {
                            galleryDTO.ModelColors = galleryDTO.ModelColors.OrderBy(color => string.IsNullOrEmpty(color.OriginalImgPath)).ToList();
                        }
                        galleryDTO.ModelColors = galleryDTO.ModelColors.Select(color => { color.HexCode = color.HexCode.Split(',')[0]; return color; }).ToList();
                    }
                    else if (queryString.Keys.Contains("imageName"))
                    {
                        galleryDTO.GalleryState.ActiveSection = GallerySections.PhotosDetail;
                    }
                }

                return galleryDTO;
            }
            catch (Exception e)
            {
                Logger.LogException(e);
                return new PhotoGalleryDTO_V2();
            }
        }

        private List<BreadcrumbEntity> BindBreadCrumb(PhotoGalleryDTO_V2 galleryDto)
        {
            string makeName = Format.FormatSpecial(galleryDto.ModelDetails.MakeName);
            List<BreadcrumbEntity> _BreadcrumbEntitylist = new List<BreadcrumbEntity>();
            _BreadcrumbEntitylist.Add(new BreadcrumbEntity { Title = string.Format("{0} Cars", galleryDto.ModelDetails.MakeName), Link = ManageCarUrl.CreateMakeUrl(makeName), Text = galleryDto.ModelDetails.MakeName });

            _BreadcrumbEntitylist.Add(new BreadcrumbEntity { Title = galleryDto.ModelDetails.ModelName, Link = ManageCarUrl.CreateModelUrl(makeName, galleryDto.ModelDetails.MaskingName), Text = galleryDto.ModelDetails.ModelName });
            _BreadcrumbEntitylist.Add(new BreadcrumbEntity { Text = (galleryDto.GalleryState.ActiveFilter == GalleryFilters.Colors ? "Colours" : "Images") });

            return _BreadcrumbEntitylist;
        }

        private Tuple<bool, bool> SetInteriorExteriorFlag(PhotoGalleryDTO_V2 galleryDTO)
        {
            bool showInterior = false, showExterior = false;
            try
            {
                if (galleryDTO != null)
                {
                    int length = galleryDTO.ModelImages.Count;
                    if (length == 1)
                    {
                        showInterior = galleryDTO.ModelImages[0].MainImgCategoryId == (int)ImageTypes.Interior;
                        showExterior = galleryDTO.ModelImages[0].MainImgCategoryId == (int)ImageTypes.Exterior;
                    }
                    else
                    {
                        for (int i = 0; i < length / 2; i++)
                        {
                            if (galleryDTO.ModelImages[i].MainImgCategoryId == (int)ImageTypes.Interior || galleryDTO.ModelImages[length - i - 1].MainImgCategoryId == (int)ImageTypes.Interior)
                                showInterior = true;
                            if (galleryDTO.ModelImages[i].MainImgCategoryId == (int)ImageTypes.Exterior || galleryDTO.ModelImages[length - i - 1].MainImgCategoryId == (int)ImageTypes.Exterior)
                                showExterior = true;
                            if (showInterior && showExterior)
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return new Tuple<bool, bool>(showExterior, showInterior);
        }

        private int GetCurrentImageIndex(PhotoGalleryDTO_V2 galleryDTO, string imageName)
        {
            if (!String.IsNullOrWhiteSpace(imageName))
            {
                string imageId = string.Empty;
                int index = imageName.LastIndexOf('-');

                if (index < 0)
                    imageId = imageName;
                else
                    imageId = imageName.Substring(index + 1);

                int imgId = RegExValidations.IsPositiveNumber(imageId) ? Convert.ToInt32(imageId) : 0;

                return (galleryDTO.GalleryState.ActiveFilter == GalleryFilters.Colors) ? galleryDTO.ModelColors.FindIndex(color => color.ColorId == imgId) : galleryDTO.ModelImages.FindIndex(image => image.ImageId == imgId);
            }
            return 0;
        }
    }
}
