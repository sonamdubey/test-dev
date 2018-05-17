using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using ApiGatewayLibrary;
using GatewayWebservice;
using Google.Protobuf;
using Bikewale.Notifications;

namespace Bikewale.BAL.ApiGateway.ApiGatewayHelper
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 2 Apr 2018
    /// Summary : Class have implementation of methods to interact with APIGateway
    /// </summary>
    public class ApiGatewayCaller : IApiGatewayCaller
    {
        /// <summary>
        /// Response from APIGateway will be stored in this property
        /// </summary>
        private OutputRequest _outRequest;

        /// <summary>
        /// Property have list of calls added to APIGateway
        /// </summary>
        private readonly CallAggregator _aggregator;

        /// <summary>
        /// Property holds list of callback functions which will be called after response is returned from APIGateway.
        /// </summary>
        private readonly ICollection<Action<IApiGatewayCaller>> _callbackActionList;

        /// <summary>
        /// Constructor to initialize all the properties and dependencies.
        /// </summary>
        public ApiGatewayCaller()
        {
            _aggregator = new CallAggregator();
            _outRequest = new OutputRequest();
            _callbackActionList = new List<Action<IApiGatewayCaller>>();
        }

        /// <summary>
        /// Function Adds a call to the APIGateway Agregator
        /// </summary>
        /// <param name="module">Module name for microservice</param>
        /// <param name="methondName">Used to execute given method in a module</param>
        /// <param name="message">Input parameters to the method</param>
        /// <returns>Returns true when operation is successfull</returns>
        bool IApiGatewayCaller.Add(string module, string methodName, IMessage message)
        {
            bool isSuccess = false;

            try
            {
                _aggregator.AddCall(module, methodName, message);
                isSuccess = true;
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Bikewale.BAL.ApiGatewayHelper.ApiGatewayCaller.Add => module - {0} : methodName - {1}", module, methodName), ex);
            }

            return isSuccess;
        }

        /// <summary>
        /// Function Adds a call to the APIGateway Agregator.
        /// </summary>
        /// <param name="module">Module name for microservice</param>
        /// <param name="methondName">Used to execute given method in a module</param>
        /// <param name="message">Input parameters to the method</param>
        /// <param name="callback">Callback function to convert grpc message to bikewale entities. This function will be executed after APIGateway returns result.</param>
        /// <returns>Returns index of callback function</returns>
        ushort IApiGatewayCaller.Add(string module, string methodName, IMessage message, Action<IApiGatewayCaller> callback)
        {
            ushort callbackIndex = 0;

            try
            {

                _aggregator.AddCall(module, methodName, message);

                _callbackActionList.Add(callback);

                callbackIndex = (ushort)(_callbackActionList.Count - 1);
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Bikewale.BAL.ApiGatewayHelper.ApiGatewayCaller.Add with callback => module - {0} : methodName - {1}", module, methodName), ex);
            }

            return callbackIndex;
        }

        /// <summary>
        /// Function Adds a call to the APIGateway Agregator.
        /// </summary>
        /// <param name="module">Module name for microservice</param>
        /// <param name="methondName">Used to execute given method in a module</param>
        /// <param name="message">Input parameters to the method</param>
        /// <param name="identifier">Identifier for group of methods added in APIGateway caller. Identifier can be used to get data.</param>
        /// <returns>Returns true when operation is successfull</returns>
        bool IApiGatewayCaller.Add(string module, string methodName, IMessage message, string identifier)
        {
            bool isSuccess = false;

            try
            {
                _aggregator.AddCall(module, methodName, message, identifier);
                isSuccess = true;
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Bikewale.BAL.ApiGatewayHelper.ApiGatewayCaller.Add => module - {0} : methodName - {1} : identifier - {2}", module, methodName, identifier), ex);
            }

            return isSuccess;
        }

        /// <summary>
        /// Function to Call APIGateway. It also saves response from APIGateway
        /// </summary>
        void IApiGatewayCaller.Call()
        {
            try
            {
                _outRequest = _aggregator.GetResultsFromGateway();

                if (_outRequest == null || _outRequest.OutputMessages == null || _outRequest.OutputMessages.Count <= 0)
                {
                    ErrorClass.LogError(new Exception("API Gateway output is null."), "Bikewale.BAL.ApiGatewayHelper.ApiGatewayCaller.Call");
                }
                InvokeCallbackFunctions();
            }
            catch (Exception ex)
            {
                throw new Exception("Bikewale.BAL.ApiGatewayHelper.ApiGatewayCaller.Call", ex);
            }
            finally
            {
                if (_outRequest == null && _outRequest.OutputMessages != null && _outRequest.OutputMessages.Count > 0)
                {
                    _outRequest.OutputMessages.Clear();
                }
                _callbackActionList.Clear();
            }
        }

        /// <summary>
        /// Function to call all callback functions asyncronously. These functions convert GRPC messages to Bikewale entities
        /// </summary>
        private void InvokeCallbackFunctions()
        {
            try
            {
                if (_callbackActionList.Count <= 0)
                    return;

                TaskFactory factory = Task.Factory;

                var mainTask = factory.StartNew(() =>
                {
                    foreach (var actionItem in _callbackActionList)
                    {
                        factory.StartNew(() =>
                        {
                            actionItem.Invoke(this);
                        }, TaskCreationOptions.AttachedToParent);
                    }
                });

                mainTask.Wait();
            }
            catch (Exception ex)
            {
                throw new Exception("Bikewale.BAL.ApiGatewayHelper.ApiGatewayCaller.InvokeCallbackFunctions", ex);
            }
        }

        /// <summary>
        /// Function to get response from an aggregated call at given index. This function must be called after Call() method in this class
        /// </summary>
        /// <typeparam name="T">Specify GRPC message type.  Data will be converted into this type. T should inherit from IMesage interface.</typeparam>
        /// <param name="index">index at which data is present in response.</param>
        /// <returns>Response from APIGatway will be converted into this(T) type.</returns>
        T IApiGatewayCaller.GetResponse<T>(ushort index)
        {
            if (_outRequest == null || _outRequest.OutputMessages == null)
                throw new Exception("Bikewale.BAL.ApiGatewayHelper.ApiGatewayCaller.GetResponse =>_outRequest or _outRequest.OutputMessages Response from apigateway is null");

            if (index < _outRequest.OutputMessages.Count)
            {
                if (String.IsNullOrWhiteSpace(_outRequest.OutputMessages[index].Exception))
                {
                    return Utilities.ConvertBytesToMsg<T>(_outRequest.OutputMessages[index].Payload);
                }
                else
                {
                    throw new Exception(string.Format("Bikewale.BAL.ApiGatewayHelper.ApiGatewayCaller.GetResponse => Exception Code: {0} Exception: {1}",
                        _outRequest.OutputMessages[index].ExceptionCode, _outRequest.OutputMessages[index].Exception));
                }
            }
            else
            {
                throw new Exception("Bikewale.BAL.ApiGatewayHelper.ApiGatewayCaller.GetResponse => The provided index is out of range");
            }
        }

        /// <summary>
        /// Function to get reponse for the given identifier
        /// </summary>
        /// <param name="outputRequest">List of reponse objects</param>
        /// <param name="Identifier">Identifier for group of methods added in APIGateway caller. Identifier used to get data from APIGatway response.</param>
        /// <returns>Complete response object from APIGateway</returns>
        IEnumerable<ApiGatewayResponse> IApiGatewayCaller.GetResponse(OutputRequest outputRequest, string identifier)
        {
            ICollection<ApiGatewayResponse> objResponse = null;

            try
            {
                if (outputRequest == null || identifier == null)
                    return null;

                OutputRequest apiResponse = CallAggregator.GetResultsForIdentifier(outputRequest, identifier);

                if (apiResponse == null || apiResponse.OutputMessages == null)
                    return null;

                objResponse = new List<ApiGatewayResponse>();

                var output = apiResponse.OutputMessages;
                int outputCnt = output.Count;

                for (int i = 0; i < outputCnt; i++)
                {
                    ApiGatewayResponse response = new ApiGatewayResponse();

                    if (string.IsNullOrWhiteSpace(output[i].Exception))
                    {
                        response.Payload = output[i].Payload;
                    }
                    else
                    {
                        response.Exception = output[i].Exception;
                        response.ExceptionCode = output[i].ExceptionCode;
                    }
                    objResponse.Add(response);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Bikewale.BAL.ApiGatewayHelper.ApiGatewayCaller.GetResponse", ex);
            }

            return objResponse;
        }


        /// <summary>
        /// Property to get complete response object from APIGateway
        /// </summary>
        IEnumerable<ApiGatewayResponse> IApiGatewayCaller.GetAllResponse
        {
            get
            {
                ICollection<ApiGatewayResponse> objResponse = null;

                try
                {
                    if (_outRequest == null || _outRequest.OutputMessages == null)
                        return null;

                    var output = _outRequest.OutputMessages;
                    int outputCnt = output.Count;

                    objResponse = new List<ApiGatewayResponse>();

                    for (int i = 0; i < outputCnt; i++)
                    {
                        ApiGatewayResponse response = new ApiGatewayResponse();

                        if (string.IsNullOrWhiteSpace(output[i].Exception))
                        {
                            response.Payload = output[i].Payload;
                        }
                        else
                        {
                            response.Exception = output[i].Exception;
                            response.ExceptionCode = output[i].ExceptionCode;
                        }
                        objResponse.Add(response);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Bikewale.BAL.ApiGatewayHelper.ApiGatewayCaller.GetAllResponse", ex);
                }

                return objResponse;
            }
        }

    }	// class
}	// namespace
