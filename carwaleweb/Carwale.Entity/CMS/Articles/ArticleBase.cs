using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CMS.Articles
{
    /// <summary>
    /// written by Ashish Kamble
    /// </summary>
    [Serializable]
    public class ArticleBase
    {
        public ulong BasicId { get; set; }
        public string Title { get; set; }
        public string ArticleUrl { get; set; }
        public string CompleteArticleUrl { get; set; }
    }
}
