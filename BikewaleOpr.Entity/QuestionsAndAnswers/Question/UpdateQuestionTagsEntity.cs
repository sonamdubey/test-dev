using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.Entity.QuestionsAndAnswers.Question
{
    /// <summary>
    /// Created by  : Sanskar Gupta on 14 June 2018
    /// Description : Entity to hold data related to UpdateTags API
    /// </summary>
    /// 
    [Serializable]
    public class UpdateQuestionTagsEntity
    {
        public uint ModeratorId { get; set; }
        public List<uint> OldTags { get; set; }
        public List<string> NewTags { get; set; }
        public uint ModelId { get; set; }
    }
}
