using Carwale.Utility;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;

namespace Carwale.Notifications.Logs
{
    public static class ExecutionTimeLogger
    {
        static bool _logGrpcCallTimings = CWConfiguration.LogGrpcCallTimings;
        static int _grpcCallTimeLimitCheckValue = CWConfiguration.GrpcCallTimeLimitCheckValue;

        public static T LogExecutionTime<T>(Func<T> callBack)
        {
            Stopwatch sw = null;
            try
            {
                if (_logGrpcCallTimings)
                {
                    sw = Stopwatch.StartNew();
                }

                return callBack();
            }
            finally
            {
                if (_logGrpcCallTimings)
                {
                    sw.Stop();
                    if (sw.ElapsedMilliseconds > _grpcCallTimeLimitCheckValue)
                    {
                        Logger.LogInfo(callBack.Method.Name + " took " + sw.ElapsedMilliseconds);
                    }
                }
            }
        }

        public static async Task<T> LogExecutionTimeAsyc<T>(Func<Task<T>> callBack, IMessage request)
        {
            Stopwatch sw = null;
            try
            {
                if (_logGrpcCallTimings)
                {
                    sw = Stopwatch.StartNew();
                }

                return await callBack();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
            finally
            {
                if (_logGrpcCallTimings)
                {
                    sw.Stop();
                    var requestParams = JsonConvert.SerializeObject(request);
                    Logger.LogInfo(String.Format("{0} took {1} milliseconds - params {2}", callBack.Method.Name, sw.ElapsedMilliseconds, requestParams));
                }
            }
        }
    }
}
