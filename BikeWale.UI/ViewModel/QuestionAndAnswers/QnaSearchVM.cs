using Bikewale.Entities.Pages;
using Bikewale.Entities.PriceQuote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.Models.QuestionAndAnswers
{
    public class QnaSearchVM
    {
        public uint CityId { get; set; }
        public uint ModelId { get; set; }
        public uint VersionId { get; set; }
        public string MakeMaskingName { get; set; }
        public string ModelMaskingName { get; set; }
        public string MakeName { get; set; }
        public string ModelName { get; set; }                
        public uint PlatformId { get; set; }
        public GAPages PageName { get; set; }
        public string QnaGASource { get; set; }
    }
}