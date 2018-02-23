using Bikewale.DAL.CoreDAL;
using Consumer;
using log4net;
using Nest;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Reflection;

namespace Bikewale.ElasticSearch.IndexUpdaterConsumer
{
    /// <summary>
    /// Created by  : Sanskar Gupta on 21 Feb 2018
    /// Description : Consumer to consume messages about ES document Insertion/Updation/Deletion from the `BWEsDocumentQueue`.
    /// </summary>
    class Program
    {

        private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().Name);
        private static ElasticClient client = ElasticSearchInstance.GetInstance();
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            try
            {
                Logs.WriteInfoLog("Queue Started at : " + DateTime.Now);

                RabbitMqPublishing.RabbitMqPublish _RabbitMQPublishing = new RabbitMqPublishing.RabbitMqPublish();

                _RabbitMQPublishing.RunCousumer(RabbitMQExecution, ConfigurationManager.AppSettings["QueueName"]);
             
            }
            catch (Exception ex)
            {
                logger.Error("Exception : " + ex.Message);
            }
            finally
            {
                logger.Info("Queue Ended at : " + DateTime.Now);
            }
        }

        /// <summary>
        /// Function responsible for receiving the message from queue.
        /// This function should return `false` for the message to be published to DeadLetterQueue and `true` otherwise.
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

                    logger.Info("RabbitMQExecution :Received job : " + nvc["indexName"]);

                    return InsertOrUpdateDocumentInIndex(nvc);

                }
                else
                {
                    return true;
                }
            }//end try
            catch (Exception ex)
            {
                logger.Error("Error while performing operation for " + nvc["indexName"], ex);
                return false;
            }//end catch
        }


        /// <summary>
        /// This method is used to Insert/Update/Delete a document in an Elastic Search Index
        /// </summary>
        /// <param name="queueMessage"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        private static bool InsertOrUpdateDocumentInIndex(NameValueCollection queueMessage, int c = 0)
        {
            if (c >= 3) {
                return true;
            }

            string indexName = queueMessage["indexName"];
            string documentType = queueMessage["documentType"];
            string documentJson = null;

            if (!string.Equals(queueMessage["operationType"], "delete", StringComparison.OrdinalIgnoreCase))
            {
                documentJson = queueMessage["documentJson"];
            }

            string documentId = queueMessage["documentId"];

            string operationType = queueMessage["operationType"];


            bool isOperationSuccessful = false;

            string esResponse = "System didn't interact with ES.";

            try
            {

                switch (operationType)
                {
                    case ("delete"):
                        bool doesDocumentExist = client.DocumentExists(new Nest.DocumentExistsRequest(indexName, documentType, documentId)).Exists;
                        if (!doesDocumentExist)
                        {
                            logger.Error(string.Format("IndexName : {0}, DocumentId = {1}, OperationType = {2}, Document Does Not Exist", indexName, documentId, operationType));
                            isOperationSuccessful = true;
                        }
                        else
                        {
                            var esDeleteResponse = client.Delete(new DeleteRequest(indexName, documentType, documentId));
                            isOperationSuccessful = esDeleteResponse.IsValid;
                            esResponse = esDeleteResponse.DebugInformation;
                        }
                        break;

                    case ("update"):

                    case ("insert"):
                        JObject documentJObject = JObject.Parse(documentJson);
                        var esIndexResponse = client.Index(documentJObject, i => i
                                      .Index(indexName)
                                      .Type(documentType)
                                      .Id(documentId)
                                      );
                        isOperationSuccessful = esIndexResponse.IsValid;
                        esResponse = esIndexResponse.DebugInformation;
                        break;

                    default:
                        logger.Info(string.Format("IndexName : {0}, DocumentId = {1}, Message : Unsupported operationType", indexName, documentId));
                        break;
                }

            }
            catch (Exception e)
            {
                logger.Error("Exception occured while updating the document", e);
                isOperationSuccessful = InsertOrUpdateDocumentInIndex(queueMessage, ++c);
            }
            finally
            {
                if (isOperationSuccessful)
                {
                    logger.Info(string.Format("IndexName : {0}, DocumentId = {1}, OperationType : {2}, EsResponse : {3}", indexName, documentId, operationType, esResponse));
                }
                else
                {
                    logger.Error(string.Format("IndexName : {0}, DocumentId = {1}, OperationType : {2}, EsResponse : {3}", indexName, documentId, operationType, esResponse));
                    isOperationSuccessful = InsertOrUpdateDocumentInIndex(queueMessage, ++c);
                }
            }
            return isOperationSuccessful;
            

        }

    }
}

