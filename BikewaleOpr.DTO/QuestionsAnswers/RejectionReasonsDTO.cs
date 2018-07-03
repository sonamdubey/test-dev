using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.DTO.QuestionsAnswers
{
    /// <summary>
    /// Created by  : Sanskar Gupta on 21 June 2018
    /// Description : DTO for `RejectionReasons`
    /// </summary>
    public enum RejectionReasonsDTO
    {
        Abusive_Foul_Language = 1,
        Language_Other_Than_English = 2,
        Incomprehensible = 3,
        Irrelevant_Question = 4,
        Hatred_Threat_For_Other_Users = 5,
        Promotional_Spam = 6
    }
}
