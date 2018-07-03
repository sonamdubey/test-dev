using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionsAnswers.Entities
{
    /// <summary>
    /// Created By : Deepak Israni on 12 June 2018
    /// Description: Entity to store Customer details.
    /// Modified by : Sanskar Gupta on 20 June 2018
    /// Description : Added property `IsInternalUser`
    /// </summary>
    [Serializable]
    public class Customer
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public bool IsInternalUser { get; set; }
    }
}
