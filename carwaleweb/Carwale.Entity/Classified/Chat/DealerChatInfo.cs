using System;

namespace Carwale.Entity.Classified.Chat
{
    [Serializable]
    public class DealerChatInfo
    {
        public int Id { get; set; }
        public string MobileNo { get; set; }
        public string ChatUserToken { get; set; }
    }
}
