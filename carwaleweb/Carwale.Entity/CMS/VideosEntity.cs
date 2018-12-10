using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CMS
{

    [Serializable]
    public class VideosEntity
    {
        public string Title {get; set;}
        public int Views { get; set; }
        public int Likes { get; set; }
        public string Description { get; set; }
        public string VideoId { get; set; }
        public string VideoSrc {get; set;}
        public DateTime PublishDate { get; set; }
        public bool IsSticky { get; set; }
        public int Popularity { get; set; }
        public string MakeName { get; set; }
        public string ModelName { get; set; }
        public string MaskingName { get; set; }
        public string SubCatName { get; set; }
        public int SubCatId { get; set; }
        public string BasicId { get; set; }

    }
}
