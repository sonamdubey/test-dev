using ApiGatewayLibrary;
using GatewayWebservice;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using Carwale.Notifications.Logs;
using AEPLCore.Utils.Serializer;
using System.Threading.Tasks;


namespace Carwale.DAL.ApiGateway
{
    /// <summary>
    /// ApiGatewayCaller Class to 
    /// 1. Add calls
    /// 2. Get Response
    /// </summary>
    public class ApiGatewayCaller : IApiGatewayCaller
    {
        /// <summary>
        /// To Store Gateway Response.
        /// </summary>
        private OutputRequest _outRequest;

        /// <summary>
        /// Dictionary is used to maintain response for particular requestId
        /// </summary>
        private readonly Dictionary<string, OutputMessage> _gatewayResponseCollection;

        /// <summary>
        /// Call Aggregator used for Adding Calls in ApiGateway Library.
        /// </summary>
        private readonly CallAggregator _gatewayCallAggregator;

        /// <summary>
        /// Stores CallBack Methods For respective Adapters.
        /// </summary>
        private readonly ICollection<Action<IApiGatewayCaller>> _callbackActionList;

        /// <summary>
        /// To Avoid redundant calls added in aggregatot hashset is maintained.
        /// </summary>
        private readonly HashSet<string> _requestIdList;

        /// <summary>
        /// To store the count of calls added by Existing Extension method inorder to maintain response with respective index.
        /// </summary>
        private int _indexAsRequestId;

        /// <summary>
        /// Constructor for initialise all the dependencies
        /// </summary>
        public ApiGatewayCaller()
        {
            _outRequest = new OutputRequest();
            _gatewayCallAggregator = new CallAggregator();
            _gatewayResponseCollection = new Dictionary<string, OutputMessage>();
            _callbackActionList = new List<Action<IApiGatewayCaller>>();
            _requestIdList = new HashSet<string>();
        }

        /// <summary>
        /// Function to Add Call in gateway aggregator
        /// Internally it calls Add function using Identifier
        /// </summary>
        /// <param name="module">Module Name of respective Service</param>
        /// <param name="methodName">Method Name of respective Service,which should be called</param>
        /// <param name="message">Input request required to add respective call</param>
        /// <returns></returns>
        public bool Add(string module, string methodName, IMessage message)
        {
            Add(module, methodName, message, _indexAsRequestId.ToString());
            _indexAsRequestId++;
            return true;
        }

        /// <summary>
        /// Function to Add Call in gateway aggregator
        /// </summary>
        /// <param name="module">Module Name of respective Service</param>
        /// <param name="methodName">Method Name of respective Service,which should be called</param>
        /// <param name="message">Input request required to add respective call</param>
        /// <param name="requestId">string added for identifying respective call</param>
        /// <returns>true if call added successfully</returns>
        public bool Add(string module, string methodName, IMessage message, string requestId)
        {
            if (!_requestIdList.Contains(requestId))
            {
                _gatewayCallAggregator.AddCall(module, methodName, message, requestId);
                _requestIdList.Add(requestId);
            }
            return true;
        }

        /// <summary>
        /// 1.Function to Add Call in gateway aggregator
        /// 2.Add Callback to callbackActionList.
        /// </summary>
        /// <param name="module">Module Name of respective Service</param>
        /// <param name="methodName">Method Name of respective Service,which should be called</param>
        /// <param name="message">Input request required to add respective call</param>
        /// <param name="requestId">string added for identifying respective call</param>
        /// <param name="callback">Invoked when response is recieved and build respective response</param>
        /// <returns></returns>
        public bool Add(string module, string methodName, IMessage message, string requestId, Action<IApiGatewayCaller> callback)
        {
            if (!_requestIdList.Contains(requestId))
            {
                _gatewayCallAggregator.AddCall(module, methodName, message, requestId);
                AddCallback(callback);
                _requestIdList.Add(requestId);
            }
            return true;
        }

        /// <summary>
        /// Method to invoke all the callback methods 
        /// </summary>
        private void InvokeCallbackFunctions()
        {
            if (_callbackActionList.Count <= 0)
            {
                return;
            }
            foreach (var actionItem in _callbackActionList)
            {
                actionItem.Invoke(this);
            }
        }

        private bool ValidGatewayResponse(OutputRequest outputRequest)
        {
            if (outputRequest == null || outputRequest.OutputMessages == null || outputRequest.OutputMessages.Count == 0)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// This Method will 
        /// 1. Get Response from aggregator.
        /// 2. Create Dictionary for mapping identifier and respective response.
        /// 3. Invoke all callback function added in apigateway caller.
        /// 4. clear outputrequest and callbackactionlist.
        /// </summary>
        public void Call()
        {
            try 
            {
                _outRequest = _gatewayCallAggregator.GetResultsFromGateway();
                if (!ValidGatewayResponse(_outRequest))
                {
                    return;
                }
                var output = _outRequest.OutputMessages;
                for (int i = 0; i < output.Count; i++)
                {
                    if (!String.IsNullOrWhiteSpace(output[i].Identifier) && !_gatewayResponseCollection.ContainsKey(output[i].Identifier))
                    {
                        _gatewayResponseCollection.Add(output[i].Identifier, output[i]);
                    }
                }
                InvokeCallbackFunctions();
                _outRequest.OutputMessages.Clear();
                _callbackActionList.Clear();
            }
            catch(Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        public async Task CallAsync()
        {
            _outRequest = await _gatewayCallAggregator.GetResultsFromGatewayAsync();
        }

        /// <summary>
        /// Function to get response based on index 
        /// </summary>
        /// <typeparam name="T">Response Type</typeparam>
        /// <param name="index">position of  particular call added in caller</param>
        /// <returns>IMessage</returns>
        public T GetResponse<T>(int index) where T : IMessage
        {
            if (index >= _indexAsRequestId)
            {
                Logger.LogError(new GateWayException("The provided index is out of range"));
                return default(T);
            }

            if ((_gatewayResponseCollection.ContainsKey(index.ToString())) && String.IsNullOrWhiteSpace(_gatewayResponseCollection[index.ToString()].Exception))
            {
                return Serializer.ConvertBytesToMsg<T>(_gatewayResponseCollection[index.ToString()].Payload);
            }
            else
            {
                Logger.LogError(new GateWayException(string.Format("Exception Code: {0} Exception: {1}",
                   _gatewayResponseCollection[index.ToString()].ExceptionCode, _gatewayResponseCollection[index.ToString()].Exception)));
                return default(T);
            }
        }

        /// <summary>
        /// Function to get response based on identifier added during Add call
        /// </summary>
        /// <typeparam name="T">Response Type</typeparam>
        /// <param name="identifier">string added for identifyingrespective call</param>
        /// <returns></returns>
        public T GetResponse<T>(string requestId) where T : IMessage
        {
            if (string.IsNullOrEmpty(requestId))
            {
                Logger.LogError(new GateWayException("Invalid Identifier"));
            }

            if ((_gatewayResponseCollection.ContainsKey(requestId)) && String.IsNullOrWhiteSpace(_gatewayResponseCollection[requestId].Exception))
            {
                return Serializer.ConvertBytesToMsg<T>(_gatewayResponseCollection[requestId].Payload);
            }
            else
            {
                Logger.LogError(new GateWayException(string.Format("Exception Code: {0} Exception: {1}",
                    _gatewayResponseCollection[requestId].ExceptionCode, _gatewayResponseCollection[requestId].Exception)));
                return default(T);
            }
        }

        /// <summary>
        /// Method for addind a callback action in callback List
        /// </summary>
        /// <param name="callback"></param>
        public void AddCallback(Action<IApiGatewayCaller> callback)
        {
            _callbackActionList.Add(callback);
        }

    }
}