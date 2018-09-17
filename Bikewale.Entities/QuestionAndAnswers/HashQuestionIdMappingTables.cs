using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.QuestionAndAnswers
{
    /// <summary>
    /// Created by: Dhruv Joshi
    /// Dated: 10th August 2018
    /// Description: Wrapper class containing hashtables for both way (questionid-hash) mapping 
    /// </summary>
    [Serializable]
    public class HashQuestionIdMappingTables
    {
        public Hashtable HashToQuestionIdMapping { get; set; }
        public Hashtable QuestionIdToHashMapping { get; set; }
    }
}
