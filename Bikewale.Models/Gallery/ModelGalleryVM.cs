using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Entities.Videos;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.Models.Gallery
{
    public class ModelGalleryVM
    {
        public IEnumerable<BikeVideoEntity> VideosList { get; set; }
        public IEnumerable<ColorImageBaseEntity> ImageList { get; set; }
        public BikeMakeEntityBase Make { get; set; }
        public BikeModelEntityBase Model { get; set; }
        public BikeInfoVM BikeInfo { get; set; }
        public string ImagesJSON { get; set; }
        public string VideosJSON { get; set; }
        public string BikeName { get; set; }

        public uint SelectedColorImageId { get; set; }
        public uint SelectedImageId { get; set; }
        public string ReturnUrl { get; set; }
        public bool IsUpcoming { get; set; }
        public bool IsDiscontinued { get; set; }

        public bool IsVideosAvailable { get { return VideosList != null && VideosList.Any(); } }
        public bool IsImagesAvailable { get { return ImageList != null && ImageList.Any(); } }
        public bool IsBikeInfoRequired { get { return BikeInfo != null && BikeInfo.BikeInfo != null; } }
        public bool IsImagesJSONAvailable { get { return !string.IsNullOrEmpty(ImagesJSON); } }
        public bool IsVideosJSONAvailable { get { return !string.IsNullOrEmpty(VideosJSON); } }

        public int VideosCount { get { return (IsVideosAvailable ? VideosList.Count() : 0); } }
        public int ImagesCount { get { return (IsImagesAvailable ? ImageList.Count() : 0); } }
    }
}
