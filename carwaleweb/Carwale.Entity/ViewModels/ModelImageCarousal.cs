using Carwale.Entity.CMS.Photos;
using Carwale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.ViewModels
{
    public class ModelImageCarousal
    {
        private List<ModelImage> _images;
        public uint RecordCount { get; set; }
        public string ImageSwiperTitle { get; set; }
        public string MakeName { get; set; }
        public string ModelName { get; set; }
        public string ModelMaskingName { get; set; }
        public string ModelImagePageUrl { get; set; }
        public List<ModelImage> Images
        {
            get { return _images; }
            set
            {
                _images = value;
                if (_images.IsNotNullOrEmpty())
                {
                    MakeName = Images[0].MakeBase.MakeName;
                    ModelName = Images[0].ModelBase.ModelName;
                    ModelMaskingName = Images[0].ModelBase.MaskingName;
                    ImageSwiperTitle = string.Format("{0} Images Swiper", MakeName);
                }
            }
        }
    }
}
