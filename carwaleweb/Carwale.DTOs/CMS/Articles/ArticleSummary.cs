using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.CMS.Articles
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 13 Aug 2014
    /// </summary>
    public class ArticleSummary : ArticleBase
    {
        public ushort CategoryId { get; set; }
        public string HostUrl { get; set; }
        public string LargePicUrl { get; set; }
        public string SmallPicUrl { get; set; }
        public string OriginalImgUrl { get; set; }

        public string Description { get; set; }
        public string AuthorName { get; set; }
        public DateTime DisplayDate { get; set; }
        public uint Views { get; set; }
        public bool IsSticky { get; set; }
        public uint FacebookCommentCount { get; set; }

        public string MakeName { get; set; }  //added by Shalini on 16/10/14
        public string MaskingName { get; set; }  //added by Shalini on 16/10/14
        public string SubCategory { get; set; } //added by Shalini on 03/11/14
        public string ModelName { get; set; }
        public string FormattedDisplayDate { get; set; } //added by Shalini on 18/11/14

        public string AuthorMaskingName { get; set; } //added by natesh and removed from ArticleDetails
        public bool IsFeatured { get; set; } //Added by Sumit Kate on 16 feb 2016

        public string CategoryMaskingName { get; set;} //Added by Ajay Singh on 27 april 2014 to show category name at news landing page
        public string Tags { get; set; }
    }
}
