using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity
{
    public class CMSFeatureDetails : CMSFeatureList
    {
        public string make {get; set;}
        public string model { get; set; }
        public string maskingName { get; set; }
        public string version { get; set; }
        public string subCategory { get; set; }
        public int modelId { get; set; }
        public int versionId { get; set; }
        public string car { get; set; }
        public string category { get; set; }
        public string fieldName { get; set; }
        public string valueType { get; set; }
        public string otherInfoValue { get; set; }
        public string pageNameforDDL { get; set; }
        public string pageName { get; set; }
        public int priority { get; set; }
        public string data { get; set; }
        public string tag { get; set; }
    }
}
