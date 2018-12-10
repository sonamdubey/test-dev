using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppWebApi.Models
{
    public class HtmlContent
    {
        /*
         Author:Rakesh Yadav 
         Date Created: 1 Oct 2013
         */
        public List<HTMLItem> HtmlItems { get; set; }
        public HtmlContent()
        {
            HtmlItems = new List<HTMLItem>();
        }
    }
}