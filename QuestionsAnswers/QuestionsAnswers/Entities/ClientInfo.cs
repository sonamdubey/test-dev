using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionsAnswers.Entities
{
    /// <summary>
    /// Created By : Deepak Israni on 9 July 2018
    /// Description : Entity to store client related information required for storing information.
    /// </summary>
    public class ClientInfo
    {
        public ushort ApplicationId { get; set; }
        public ushort PlatformId { get; set; }
        public ushort SourceId { get; set; }
        public string ClientIp{ get; set; }
    }
}
