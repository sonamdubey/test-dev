using System;

namespace QuestionsAnswers.Entities
{
    /// <summary>
    /// Created by  : Sanskar Gupta on 14 June 2018
    /// Description : Enum to hold various reasons for rejection of a question.
    /// </summary>
    [Serializable]
    public enum EnumRejectionReasons
    {
        Abusive_Foul_Language = 1,
        Language_Other_Than_English,
        Incomprehensible,
        Irrelevant_Question,
        Hatred_Threat_For_Other_Users,
        Promotional_Spam
    }
}
