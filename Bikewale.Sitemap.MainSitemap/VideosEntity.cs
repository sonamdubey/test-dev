﻿using System.Collections.Generic;

namespace Bikewale.Sitemap.MainSitemap
{
    /// <summary>
    /// Created by  :   Sumit Kate on 31 Oct 2017
    /// Description :   Video response entity
    /// </summary>
    public class VideosResponse
    {
        public IEnumerable<VideosEntity> videos { get; set; }
    }

    /// <summary>
    /// Created by  :   Sumit Kate on 31 Oct 2017
    /// Description :   Video Entity
    /// </summary>
    public class VideosEntity
    {
        public string VideoTitle { get; set; }
        public string VideoUrl { get; set; }
        public string VideoId { get; set; }
        public uint Views { get; set; }
        public uint Likes { get; set; }
        public string Description { get; set; }
        public uint BasicId { get; set; }
        public string Tags { get; set; }
        public uint Duration { get; set; }
        public string MakeName { get; set; }
        public string ModelName { get; set; }
        public string MaskingName { get; set; }
        public string SubCatId { get; set; }
        public string SubCatName { get; set; }
        public string VideoTitleUrl { get; set; }
        public string ImgHost { get; set; }
        public string ThumbnailPath { get; set; }
        public string ImagePath { get; set; }
        public string DisplayDate { get; set; }
    }
}
