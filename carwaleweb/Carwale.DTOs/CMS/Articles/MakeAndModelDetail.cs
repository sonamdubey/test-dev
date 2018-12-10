using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.CMS.Articles
{
    /// <summary>
    /// created by natesh on 13/11/14 
    /// entity for make and model name and id available for roadtest and comparison test
    /// </summary>
    public class MakeAndModelDetail
    {
        public string Text { get; set; }
        public uint Value { get; set; }
        public string MaskingName { get; set; }
    }
}
