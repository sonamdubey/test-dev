using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Forums
{
   public  class UserProfile
    {
        public string AvtOriginalImgPath { get; set; }      
        public string RealOriginalImgPath { get;set;}
        public string HandleName {get;set;}
        public string AboutMe {get;set;}   
        public string Signature  { get;set;}
        public string AvtarPhoto { get;set;}  
        public string RealPhoto{ get;set;}
        public string StatusId { get; set; }
        public string HostURL { get; set; }
        public string ThumbNailUrl { get; set; }
        public bool IsUpdated { get; set; }
    }
}
