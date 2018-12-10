using AEPLCore.Queue;
using AEPLCore.Utils.Serializer;
using Carwale.Entity.Notifications;
using Carwale.Notifications.Interface;
using Synapse.ProtoClass;
using System.Configuration;

namespace Carwale.Notifications
{

    public class SmsLogic : ISmsLogic
    {
        private readonly IPublishManager _publishManager;
        private const string _smsQueueName = "synapse";
        private static readonly string _synapseModuleName = ConfigurationManager.AppSettings["SynapseModuleName"];
        public SmsLogic(IPublishManager publishManager)
        {
            _publishManager = publishManager;
        }
        public bool Send(SMS sms)
        {
            if (sms == null)
            {
                return false;
            }
            SendRequest request = new SendRequest
            {
                Medium = sms.IsPriority ? Medium.SmsPriority: Medium.Sms,
                Source = "CarwaleWeb",//From
                Destination = new Destination { To = sms.Mobile },
                SourceModule = sms.SourceModule.ToString(),
                Application = 1,//Carwale
                Platform = (int)sms.Platform,
                IpAddress = sms.IpAddress,
                MessageBody = sms.Message,
            };

            _publishManager.PublishMessageToQueue(_smsQueueName, new QueueMessage
            {
                ModuleName = _synapseModuleName,
                FunctionName = "Send",
                InputParameterName = "SendRequest",
                Payload = Serializer.ConvertProtobufMsgToBytes(request),
            });
            return true;
        }
    }
}
