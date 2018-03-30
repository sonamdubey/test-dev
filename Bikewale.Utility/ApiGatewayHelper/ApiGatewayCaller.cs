using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
//using AEPLCore.Logging;
using ApiGatewayLibrary;
using GatewayWebservice;
using Google.Protobuf;

namespace Bikewale.Utility.ApiGatewayHelper
{
	public class ApiGatewayCaller : IApiGatewayCaller
	{
		private OutputRequest _outRequest;
		private readonly CallAggregator _aggregator;

		public ApiGatewayCaller()
		{
			_aggregator = new CallAggregator();
			_outRequest = new OutputRequest();
		}		

		public bool Add(string module, string methodName, IMessage message)
		{
			try
			{
				_aggregator.AddCall(module, methodName, message);
				return true;
			}
			catch (Exception ex)
			{
				//Logger.LogError(String.Format("ApiGatewayCaller.Add method for module : {0}, method {1}", module, methodName), ex);
				throw ex;
			}
		}

		public bool Add(string module, string methodName, IMessage message, string identifier)
		{
			try
			{
				_aggregator.AddCall(module, methodName, message, identifier);
				return true;
			}
			catch (Exception ex)
			{
				//Logger.LogError(String.Format("ApiGatewayCaller.Add method for module : {0}, method : {1}, identifier : {2}", module, methodName, identifier), ex);
				throw ex;				
			}
		}

		public void Call()
		{
			_outRequest = _aggregator.GetResultsFromGateway();
		}

		public T GetResponse<T>(int index) where T : IMessage
		{
			if (index >= _outRequest.OutputMessages.Count)
			{
				throw new GateWayException("The provided index is out of range");
			}
			if (String.IsNullOrWhiteSpace(_outRequest.OutputMessages[index].Exception))
			{
				return Utilities.ConvertBytesToMsg<T>(_outRequest.OutputMessages[index].Payload);
			}
			else
			{
				throw new GateWayException(string.Format("Exception Code: {0} Exception: {1}",
					_outRequest.OutputMessages[index].ExceptionCode, _outRequest.OutputMessages[index].Exception));
			}
		}

		public IEnumerable<ApiGatewayResponse> GetResponse(OutputRequest outputRequest, string identifier)
		{
			OutputRequest objResponse = CallAggregator.GetResultsForIdentifier(outputRequest, identifier);

			IList<ApiGatewayResponse> responses = new List<ApiGatewayResponse>();

			for (int i = 0; i < objResponse.OutputMessages.Count; i++)
			{
				ApiGatewayResponse response = new ApiGatewayResponse();

				if (string.IsNullOrWhiteSpace(objResponse.OutputMessages[i].Exception))
				{
					response.Payload = objResponse.OutputMessages[i].Payload;
				}
				else
				{
					response.Exception = objResponse.OutputMessages[i].Exception;
					response.ExceptionCode = objResponse.OutputMessages[i].ExceptionCode;
				}
				responses.Add(response);
			}
			if (!responses.Any())
			{
				return null;
			}
			return responses;
		}

		public IEnumerable<ApiGatewayResponse> GetAllResponse
		{
			get
			{
				IList<ApiGatewayResponse> responses = new List<ApiGatewayResponse>();

				for (int i = 0; i < _outRequest.OutputMessages.Count; i++)
				{
					ApiGatewayResponse response = new ApiGatewayResponse();

					if (string.IsNullOrWhiteSpace(_outRequest.OutputMessages[i].Exception))
					{
						response.Payload = _outRequest.OutputMessages[i].Payload;
					}
					else
					{
						response.Exception = _outRequest.OutputMessages[i].Exception;
						response.ExceptionCode = _outRequest.OutputMessages[i].ExceptionCode;
					}
					responses.Add(response);
				}
				if (!responses.Any())
				{
					return null;
				}
				return responses;
			}
		}

		[Serializable]//Follow ISerialize pattern 
					  // Important: This attribute is NOT inherited from Exception, and MUST be specified 
					  // otherwise serialization will fail with a SerializationException stating that
					  // "Type X in Assembly Y is not marked as serializable."
		public class GateWayException : Exception
		{
			public GateWayException()
			{
			}

			public GateWayException(string message) : base(message)
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
}
