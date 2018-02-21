using Bikewale.DAL.CoreDAL;
using Consumer;
using log4net;
using Nest;
using Newtonsoft.Json.Linq;
using RabbitMqPublishing;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Reflection;

namespace Bikewale.ElasticSearch.IndexUpdaterConsumer
{
    class Program
    {

        private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().Name);
        private static ElasticClient client = ElasticSearchInstance.GetInstance();
        static void Main(string[] args)
        {
            //string queueName = ConfigurationManager.AppSettings["QueueName"].ToUpper();
            //string consumerName = ConfigurationManager.AppSettings["ConsumerName"].ToString();
            //string retryCount = ConfigurationManager.AppSettings["RetryCount"];
            //string rabbitMsgTTL = ConfigurationManager.AppSettings["RabbitMsgTTL"];
            log4net.Config.XmlConfigurator.Configure();
            try
            {
                Logs.WriteInfoLog("Started at : " + DateTime.Now);

                RabbitMqPublish _RabbitMQPublishing = new RabbitMqPublish();
                
                _RabbitMQPublishing.RunCousumer(RabbitMQExecution, ConfigurationManager.AppSettings["QueueName"]);

                //IndexUpdateConsumer consumer = new IndexUpdateConsumer(queueName, consumerName, retryCount, rabbitMsgTTL);
                //consumer.ProcessMessages();                
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("Exception : " + ex.Message);
            }
            finally
            {
                Logs.WriteInfoLog("End at : " + DateTime.Now);
            }
        }

        /// <summary>
        /// Function responsible for receiving the message from queue
        /// </summary>
        /// <param name="nvc"></param>
        /// <returns></returns>
        static bool RabbitMQExecution(NameValueCollection nvc)
        {
            try
            {
                if (nvc != null
                            && nvc.HasKeys()
                            && !String.IsNullOrEmpty(nvc["indexName"])
                            && !String.IsNullOrEmpty(nvc["documentType"])
                            && !String.IsNullOrEmpty(nvc["documentId"])
                            && !String.IsNullOrEmpty(nvc["operationType"]))
                {
                    string indexName = nvc["indexName"];
                    string documentType = nvc["documentType"];
                    string documentJson = "";

                    if (!string.Equals(nvc["operationType"], "delete", StringComparison.OrdinalIgnoreCase))
                    {
                        documentJson = nvc["documentJson"];
                    }

                    string documentId = nvc["documentId"];

                    string operationType = nvc["operationType"];
                    logger.Info("RabbitMQExecution :Received job : " + nvc["indexName"]);

                    return InsertOrUpdateDocumentInIndex(indexName, documentId, documentType, documentJson, operationType);

                }
                else return true;
            }//end try
            catch (Exception ex)
            {
                logger.Error("Error while performing operation for " + nvc["indexName"], ex);
                return false;
            }//end catch
        }//end try


        /// <summary>
        /// This method is used to Insert/Update/Delete a document in an Elastic Search Index
        /// </summary>
        /// <param name="indexName"></param>
        /// <param name="document"></param>
        /// <returns></returns>
        private static bool InsertOrUpdateDocumentInIndex(string indexName, string documentId, string documentType, string documentJson, string operationType)
        {
            bool doesDocumentExist = client.DocumentExists(new Nest.DocumentExistsRequest(indexName, documentType, documentId)).Exists;
            bool isOperationSuccessful = false;
            string esResponse = "";

            if (doesDocumentExist)
            {
                Logs.WriteInfoLog(String.Format("Document with ID = {0} exists => UPDATE/DELETE IT", documentId));
            }
            else
            {
                Logs.WriteInfoLog(String.Format("Document with ID = {0} does not exist => INSERT IT", documentId));
            }

            switch (operationType)
            {
                case ("delete"):
                    if (!doesDocumentExist)
                    {
                        Logs.WriteErrorLog(string.Format("IndexName : {0}, DocumentId = {1}, OperationType = {2}, DoesDocumentExist = {3}", indexName, documentId, operationType, doesDocumentExist));
                    }
                    else
                    {
                        var esDeleteResponse = client.Delete(new DeleteRequest(indexName, documentType, documentId));
                        isOperationSuccessful = esDeleteResponse.IsValid;
                        esResponse = esDeleteResponse.ToString();
                    }
                    break;

                case ("insert"):

                case ("update"):
                    JObject documentJObject = JObject.Parse(documentJson);
                    var esIndexResponse = client.Index(documentJObject, i => i
                                  .Index(indexName)
                                  .Type(documentType)
                                  .Id(documentId)
                                  );
                    isOperationSuccessful = esIndexResponse.IsValid;
                    esResponse = esIndexResponse.ToString();
                    break;

                default:
                    Logs.WriteErrorLog(string.Format("IndexName : {0}, DocumentId = {1}, DoesDocumentExist = {2}, Message : OperationType Not Mentioned / Invalid", indexName, documentId, doesDocumentExist));
                    break;
            }

            if (isOperationSuccessful)
            {
                Logs.WriteInfoLog(string.Format("IndexName : {0}, DocumentId = {1}, DoesDocumentExist = {2}, OperationType : {3}, EsResponse : {4}", indexName, documentId, doesDocumentExist, operationType, esResponse));
            }
            else
            {
                Logs.WriteErrorLog(string.Format("IndexName : {0}, DocumentId = {1}, DoesDocumentExist = {2}, OperationType : {3}, EsResponse : {4}", indexName, documentId, doesDocumentExist, operationType, esResponse));
            }

            return isOperationSuccessful;

        }
    }
}

