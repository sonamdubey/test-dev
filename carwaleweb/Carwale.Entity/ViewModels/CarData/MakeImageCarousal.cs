using Carwale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.ViewModels.CarData
{
    public class ImageCarousal
    {
        public string Title { get; set; }
        
        public string MakeName
        {
            get { return ModelPhotos.IsNotNullOrEmpty() ? ModelPhotos[0].MakeName : ""; }
        }

        public string LandingUrl { get; set; }
        

        public List<ModelImageCarousal> ModelPhotos { get; set; }
    }
}
