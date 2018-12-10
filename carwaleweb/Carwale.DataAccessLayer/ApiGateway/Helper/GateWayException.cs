using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;


namespace Carwale.DAL.ApiGateway
{
    [Serializable]//Follow ISerialize pattern 
    // Important: This attribute is NOT inherited from Exception, and MUST be specified 
    // otherwise serialization will fail with a SerializationException stating that
    // "Type X in Assembly Y is not marked as serializable."
    public class GateWayException : Exception
    {
        public GateWayException()
        {
        }

        public GateWayException(string message)
            : base(message)
        {
        }

        public GateWayException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
        // Without this constructor, deserialization will fail
        protected GateWayException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
