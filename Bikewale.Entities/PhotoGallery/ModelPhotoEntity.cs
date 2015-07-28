using Bikewale.Entities.BikeData;
using System;

namespace Bikewale.Entities.PhotoGallery
{
    /// <summary>
    /// Created By : Sadhana Upadhyay on 1 July 2014
    /// Summary : Entity for model photo gallery
    /// </summary>
    public class ModelPhotoEntity
    {
        public uint ImageId { get; set; }
        public string HostUrl { get; set; }
        public string ImagePathThumbnail { get; set; }
        public string ImagePathLarge { get; set; }
        public string ImageCategory { get; set; }
        public int BasicId { get; set; }
        public string Caption { get; set; }
        public string ArticleTitle { get; set; }
        public string ArticleUrl { get; set; }

        private BikeModelEntityBase _objModel = new BikeModelEntityBase();
        public BikeModelEntityBase ModelBase { get { return _objModel; } set { _objModel = value; } }

        private BikeMakeEntityBase _objMake = new BikeMakeEntityBase();
        public BikeMakeEntityBase MakeBase { get { return _objMake; } set { _objMake = value; } }
    }
}
