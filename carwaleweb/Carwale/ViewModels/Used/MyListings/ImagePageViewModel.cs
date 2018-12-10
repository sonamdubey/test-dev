using Carwale.Entity.Enum;
using Carwale.Entity.Classified;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Carwale.UI.ViewModels.Used.MyListings
{
    public class ImagePageViewModel
    {
        public List<CarPhoto> PhotoList { get; set; }
        public Platform Source { get; set; }
    }
}