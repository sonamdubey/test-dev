
using System;
namespace QuestionsAnswers.Entities
{

    /// <summary>
    /// Created by  : Sanskar Gupta on 06 June 2018
    /// Description : Enum to hold the values for Moderation status
    /// </summary>
    [Serializable]
    public enum EnumModerationStatus
    {
        New = 1,
        Approved,
        Rejected
    }
}
