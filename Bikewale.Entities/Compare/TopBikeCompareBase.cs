using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.Compare
{
    public class TopBikeCompareBase
    {
        public int ID { get; set; }
        public UInt16 VersionId1 { get; set; }
        public UInt16 VersionId2 { get; set; }
        public UInt16 ModelId1 { get; set; }
        public UInt16 ModelId2 { get; set; }
        public string Bike1 { get; set; }
        public string Bike2 { get; set; }
        public string MakeMaskingName1 { get; set; }
        public string MakeMaskingName2 { get; set; }
        public string ModelMaskingName1 { get; set; }
        public string ModelMaskingName2 { get; set; }
        public UInt32 Price1 { get; set; }
        public UInt32 Price2 { get; set; }
        public UInt16 Review1 { get; set; }
        public UInt16 Review2 { get; set; }
        public UInt16 ReviewCount1 { get; set; }
        public UInt16 ReviewCount2 { get; set; }
        public string HostURL { get; set; }
        public string OriginalImagePath { get; set; }
    }
}
