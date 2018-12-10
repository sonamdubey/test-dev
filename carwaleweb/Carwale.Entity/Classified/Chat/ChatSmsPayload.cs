using FluentValidation.Attributes;
using System;

namespace Carwale.Entity.Classified.Chat
{
    [Validator(typeof(ChatSmsPayloadValidator))]
    public class ChatSmsPayload
    {
        public string Key { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Message { get; set; }
        public DateTime MessageSentTime { get; set; }

        private long _timeStamp;
        public long TimeStamp
        {
            get
            {
                return _timeStamp;
            }
            set
            {
                MessageSentTime = new DateTime(1970, 1, 1).AddMilliseconds(value);
                _timeStamp = value;
            }
        }
        public long ReceiverLastSeenAtTime { get; set; }
        public bool ReceiverConnected { get; set; }
    }
}
