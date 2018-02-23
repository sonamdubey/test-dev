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

        private const string ES_OPERATION_INSERT = "insert";
        private const string ES_OPERATION_UPDATE = "update";
        private const string ES_OPERATION_DELETE = "delete";

        private static ElasticClient client;
        private static int NO_OF_RETRIES;

        static Program(){
            try
            {
                client = ElasticSearchInstance.GetInstance();
                NO_OF_RETRIES = Convert.ToInt32(ConfigurationManager.AppSettings["RetryCount"]);
            }
            catch (Exception e)
            {
                logger.Error("An Error Occured while trying to get ES Instance / fetching NO_OF_RETRIES from AppSettings", e);
            }
        }

        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            try
            {
                Logs.WriteInfoLog("Consumer Started at : " + DateTime.Now);

                RabbitMqPublishing.RabbitMqPublish _RabbitMQPublishing = new RabbitMqPublishing.RabbitMqPublish();

                _RabbitMQPublishing.RunCousumer(RabbitMQExecution, ConfigurationManager.AppSettings["QueueName"]);
             
            }
            catch (Exception ex)
            {
                logger.Error("Exception : " + ex.Message);
            }
            finally
            {
                logger.Info("Consumer Ended at : " + DateTime.Now);
            }
        }

        /// <summary>
        /// Function responsible for receiving the message from queue.
        /// This function should return `false` for the message to be published to DeadLetterQueue and `true` when the the message should be acknowledged/rejected.
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
            if (c >= NO_OF_RETRIES) {
                return true;
            }

            string indexName = queueMessage["indexName"];
            string documentType = queueMessage["documentType"];
            string documentJson = null;
            string documentId = queueMessage["documentId"];
            string operationType = queueMessage["operationType"];
            bool isOperationSuccessful = false;
            string esResponse = string.Empty;

            logger.Info(string.Format("IndexName : {0}, DocumentId = {1}, DocumentType : {2}, OperationType : {3}", indexName, documentId, documentType, operationType));

            if (!string.Equals(operationType, ES_OPERATION_DELETE, StringComparison.OrdinalIgnoreCase))
            {
                if (queueMessage["documentJson"] != null)
                {
                    documentJson = queueMessage["documentJson"];
                }
                else
                {
                    logger.Error(string.Format("ERROR : Document JSON Not Passed, OperationType = {0}", operationType));
                    return true;
                }
            }

            try
            {

                switch (operationType)
                {
                    case ES_OPERATION_DELETE:
                        bool doesDocumentExist = client.DocumentExists(new Nest.DocumentExistsRequest(indexName, documentType, documentId)).Exists;
                        if (!doesDocumentExist)
                        {
                            logger.Error(string.Format("ERROR : Document Does Not Exist, OperationType = {0}", operationType));
                            isOperationSuccessful = true;
                        }
                        else
                        {
                            var esDeleteResponse = client.Delete(new DeleteRequest(indexName, documentType, documentId));
                            if (esDeleteResponse != null) { 
                                isOperationSuccessful = esDeleteResponse.IsValid;
                                esResponse = esDeleteResponse.DebugInformation;
                            }
                        }
                        break;

                    case ES_OPERATION_UPDATE:

                    case ES_OPERATION_INSERT:
                        JObject documentJObject = JObject.Parse(documentJson);
                        var esIndexResponse = client.Index(documentJObject, i => i
                                      .Index(indexName)
                                      .Type(documentType)
                                      .Id(documentId)
                                      );
                        if (esIndexResponse != null)
                        {
                            isOperationSuccessful = esIndexResponse.IsValid;
                            esResponse = esIndexResponse.DebugInformation;
                        }
                        break;

                    default:
                        logger.Error(string.Format("ERROR. Message : Unsupported operationType_{0}", operationType));
                        break;
                }

            }
            catch (Exception e)
            {
                logger.Error("Exception occured while updating the document", e);
            }
            finally
            {
                if (isOperationSuccessful)
                {
                    logger.Info(string.Format("ES Operation Successfully processed, Response : {0}", esResponse));
                }
                else
                {
                    logger.Error(string.Format("Error! ES Operation can't be processed, Response : {0}", esResponse));
                    isOperationSuccessful = InsertOrUpdateDocumentInIndex(queueMessage, ++c);
                }
            }
            return isOperationSuccessful;
            

        }

    }
}

