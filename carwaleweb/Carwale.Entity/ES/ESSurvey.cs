using System;
using System.Collections.Generic;

namespace Carwale.Entity.ES
{
    [Serializable]
    public class ESSurveyEnity
    {
        public List<ESSurveyQuestions> Questions { get; set; }
        public ESSurveyCustomerResponse Customer { get; set; }
        public ESSurveyCampaign Campaign { get; set; }
    }
}
