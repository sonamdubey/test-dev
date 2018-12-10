using Carwale.Entity.CMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Common
{
    [Serializable]
    public class PopularVideoWidget
    {
        public List<VideosEntity> Videos { get; set; }
    }
}
