using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bikewale.Models.Survey
{
    public class BajajSurveyModel : ModelBase
    {
        public string ModelName { get; set; }
        public List<string> MultipleModel { get; set; }
        public List<string> InternetOptions { get; set; }
        public string OtherBike { get; set; }
        public string IsLongerFormat { get; set; }

        public string Age { get; set; }
        public string Handset { get; set; }
    }

    public class SubQuestion
    {
        public short Id { get; set; }
        public int Key { get; set; }
        public string Value { get; set; }
    }

    public class Question : SubQuestion
    { 

        public IEnumerable<SubQuestion> SubQuestions { get; set; }
    }
}