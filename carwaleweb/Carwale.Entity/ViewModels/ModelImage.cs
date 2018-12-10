using Carwale.Entity.CarData;
using Carwale.Entity.CMS.Photos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.ViewModels
{
    public class ModelImage 
    {
       
        public string HostUrl { get; set; }

        public string OriginalImgPath { get; set; }

        public CarMakeEntityBase MakeBase { get; set; }

        public CarModelEntityBase ModelBase { get; set; }

    }
}
