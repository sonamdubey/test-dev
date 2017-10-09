﻿using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Entities.PhotoGallery;
using Bikewale.Entities.Videos;
using Bikewale.Models.Gallery;
using System.Collections.Generic;

namespace Bikewale.Models.Photos
{
    public class PhotosPageVM : ModelBase
    {
        public ModelPhotoGalleryEntity PhotoGallery { get; set; }
        public IEnumerable<BikeVideoEntity> ModelVideos { get; set; }
        public IEnumerable<ColorImageBaseEntity> ModelImages { get; set; }
        public ImageBaseEntity ModelImage { get; set; }
        public BikeMakeEntityBase Make { get; set; }
        public BikeModelEntityBase Model { get; set; }
        public BikeModelEntity objModel { get; set; }
        public BikeInfoVM BikeInfo { get; set; }
        public RecentVideosVM Videos { get; set; }
        public SimilarBikesWithPhotosVM SimilarBikes { get; set; }
        public ModelGalleryVM ModelGallery { get; set; }

        public uint TotalPhotos { get; set; }
        public uint GridPhotoCount { get; set; }
        public uint NonGridPhotoCount { get; set; }
        public uint GridSize { get; set; }
        public uint NoOfGrid { get; set; }

        public string BikeName { get; set; }
        public bool IsPopupOpen { get; set; }


    }
}
