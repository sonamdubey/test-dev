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
        private static bool isESInstanceInitialized;
        private const string ES_OPERATION_INSERT = "insert";
        private const string ES_OPERATION_UPDATE = "update";
        private const string ES_OPERATION_DELETE = "delete";
        private const string ES_OPERATION_PARTIALUPDATE = "partialupdate";

        private static ElasticClient client;
        private static int NO_OF_RETRIES;

        static Program(){
            log4net.Config.XmlConfigurator.Configure();
            try
            {
                client = ElasticSearchInstance.GetInstance();
                isESInstanceInitialized = true;
                NO_OF_RETRIES = Convert.ToInt32(ConfigurationManager.AppSettings["RetryCount"]);
            }
            catch (Exception e)
            {
                Logs.WriteErrorLog("An Error Occured while trying to get ES Instance / fetching NO_OF_RETRIES from AppSettings", e);
            }
        }

        static void Main(string[] args)
        {

            //If ES Instance is not initialized
            if (!isESInstanceInitialized)
            {
                return;
            }
            try
            {
                Logs.WriteInfoLog("Consumer Started at : " + DateTime.Now);

                RabbitMqPublishing.RabbitMqPublish _RabbitMQPublishing = new RabbitMqPublishing.RabbitMqPublish();

                _RabbitMQPublishing.RunCousumer(RabbitMQExecution, ConfigurationManager.AppSettings["QueueName"]);
             
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("Exception : " + ex.Message);
            }
            finally
            {
                Logs.WriteInfoLog("Consumer Ended at : " + DateTime.Now);
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

                    Logs.WriteInfoLog("RabbitMQExecution :Received job : " + nvc["indexName"]);

                    return InsertOrUpdateDocumentInIndex(nvc);

                }
                else
                {
                    return true;
                }
            }//end try
            catch (Exception ex)
            {
                Logs.WriteErrorLog("Error while performing operation for " + nvc["indexName"], ex);
                return false;
            }//end catch
        }


        /// <summary>
        /// This method is used to Insert/Update/Delete a document in an Elastic Search Index
        /// Modified by : Ashutosh Sharma on 31 Mar 2018.
        /// Description : Added case to paritally update a document. No need to pass whole document in "documentJson" field of "queueMessage".
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

            Logs.WriteInfoLog(string.Format("IndexName : {0}, DocumentId = {1}, DocumentType : {2}, OperationType : {3}", indexName, documentId, documentType, operationType));

            if (!string.Equals(operationType, ES_OPERATION_DELETE, StringComparison.OrdinalIgnoreCase))
            {
                if (queueMessage["documentJson"] != null)
                {
                    documentJson = queueMessage["documentJson"];
                }
                else
                {
                    Logs.WriteErrorLog(string.Format("ERROR : Document JSON Not Passed, OperationType = {0}", operationType));
                    return true;
                }
            }

            try
            {
                JObject documentJObject;
                switch (operationType)
                {
                    case ES_OPERATION_DELETE:
                        bool doesDocumentExist = client.DocumentExists(new Nest.DocumentExistsRequest(indexName, documentType, documentId)).Exists;
                        if (!doesDocumentExist)
                        {
                            Logs.WriteErrorLog(string.Format("ERROR : Document Does Not Exist, OperationType = {0}", operationType));
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
                        documentJObject = JObject.Parse(documentJson);
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
                    case ES_OPERATION_PARTIALUPDATE:
                        documentJObject = JObject.Parse(documentJson);
                        var esUpdateDocResponse = client.Update<DocumentPath<JObject>, object>(documentId, 
                            d => d.Index(indexName)
                                .Type(documentType)
                                .Doc(documentJObject)
                                .RetryOnConflict(5));
                        if (esUpdateDocResponse != null)
                        {
                            isOperationSuccessful = esUpdateDocResponse.IsValid;
                            esResponse = esUpdateDocResponse.DebugInformation;
                        }
                        break;
                    default:
                        Logs.WriteErrorLog(string.Format("ERROR. Message : Unsupported operationType_{0}", operationType));
                        break;
                }

            }
            catch (Exception e)
            {
                Logs.WriteErrorLog("Exception occured while updating the document", e);
            }
            finally
            {
                if (isOperationSuccessful)
                {
                    Logs.WriteInfoLog(string.Format("ES Operation Successfully processed, Response : {0}", esResponse));
                }
                else
                {
                    Logs.WriteErrorLog(string.Format("Error! ES Operation can't be processed, Response : {0}", esResponse));
                    isOperationSuccessful = InsertOrUpdateDocumentInIndex(queueMessage, ++c);
                }
            }
            return isOperationSuccessful;
            

        }

    }
}

