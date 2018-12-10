using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.ChatManagement
{
    [Serializable]
    public class ChatResponse
    {
        public bool IsChatOn { get; set; }
        public bool IsActive { get; set; }
    }
}
