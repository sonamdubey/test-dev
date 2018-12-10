using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Carwale.Entity.CMS.Articles;

namespace Carwale.UI.ViewModels.NewCars
{
    public class CarSynopsisModelPage
    {
        public List<Page> PageList { get; set; }
        public bool IsFuturistic { get; set; }
        public string Description { get; set; }
        public ulong BasicId { get; set; }
        public string Heading { get; set; }
        public string ModelName { get; set; }
        public int ModelId { get; set; }
        public bool ShowH2Tag { get; set; }
        public bool IsExpertReview { get; set; }
    }
}