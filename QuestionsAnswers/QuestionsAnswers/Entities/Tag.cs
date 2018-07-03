using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionsAnswers.Entities
{
    /// <summary>
    /// Created by  : Sanskar Gupta on 06 June 2018
    /// Description : Entity to hold properties of a tag
    /// Modified By : Deepak Israni on 12 June 2018
    /// Description : Change entity based on schema changes, to store only name and id.
    /// </summary>
    [Serializable]
    public class Tag
    {
        public uint Id { get; set; }
        public string Name { get; set; }
    }
}
