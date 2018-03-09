using Bikewale.ElasticSearch.DocumentBuilderConsumer.DocumentBuilders;
using Bikewale.ElasticSearch.DocumentBuilderConsumer.Interfaces;
using Consumer;
using log4net;
using RabbitMqPublishing;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Bikewale.ElasticSearch.DocumentBuilderConsumer
{
    /// <summary>
    /// Created By : Deepak Israni on 7 March 2018
    /// Description: Consumer to consume messages related to updates in OPR and then pushing messages to update respective ES Indexes.
    /// </summary>
    class Program
    {
        private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().Name);
        private static int NO_OF_RETRIES;
        private static IDictionary<String, IDocumentBuilder> DOCBUILDERS;

        //Operations
        private const string OPERATION_INSERT = "insert";
        private const string OPERATION_UPDATE = "update";
        private const string OPERATION_DELETE = "delete";

        /// <summary>
        /// Created By : Deepak Israni on 7 March 2018
        /// Description: Constructor to initialize value of NO_OF_RETRIES
        /// </summary>
        static Program()
        {
            try
            {
                NO_OF_RETRIES = Convert.ToInt32(ConfigurationManager.AppSettings["RetryCount"]);
                DOCBUILDERS = new Dictionary<String, IDocumentBuilder>()
                {
                    {ConfigurationManager.AppSettings["BikeIndex"], new ModelIndexDocumentBuilder()}
                };
            }
            catch (Exception ex)
            {
                logger.Error("An Error Occured while fetching NO_OF_RETRIES from AppSettings/Creating Dictionary", ex);
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
        /// Created By : Deepak Israni on 7 March 2018
        /// Description: Function responsible for receiving the message from the queue and passing the nvc forward.
        /// This function should return `false` for the message to be published to DeadLetterQueue and `true` when the the message should be acknowledged/rejected.
        /// </summary>
        /// <param name="nvc"></param>
        /// <returns></returns>
        static bool RabbitMQExecution(NameValueCollection nvc)
        {
            String id = nvc["ids"];
            String a = nvc["indexName"];
            String b = nvc["documentType"];
            String c = nvc["operationType"];
            try
            {
                if (nvc != null
                            && nvc.HasKeys()
                            && !String.IsNullOrEmpty(nvc["ids"])
                            && !String.IsNullOrEmpty(nvc["indexName"])
                            && !String.IsNullOrEmpty(nvc["documentType"])
                            && !String.IsNullOrEmpty(nvc["operationType"]))
                {
                    logger.Info("RabbitMQExecution :Received job : " + nvc["indexName"]);
                    return PerformDocumentOperations(nvc);
                }
                else
                {
                    logger.Info("NVC missing data in certain keys.");
                    return true;
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error while performing operation for " + nvc["indexName"], ex);
                return false;
            }
        }

        /// <summary>
        /// Created By : Deepak Israni on 7 March 2018
        /// Description: Function responsible for delegating the operation requested. This function gets the appropriate reference object and calls the appropriate operation function.
        /// </summary>
        /// <param name="queueMessage"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        private static bool PerformDocumentOperations(NameValueCollection queueMessage, int c = 0)
        {
            if (c >= 3)
            {
                return true;
            }

            IDocumentBuilder docBuilder = null;
            bool isOperationSuccessful = false;

            //Get reference of respective IndexDocumentBuilder
            if (DOCBUILDERS.ContainsKey(queueMessage["indexName"]))
            {
                docBuilder = DOCBUILDERS[queueMessage["indexName"]];
            }

            if (docBuilder != null)
            {
                try
                {
                    //Perform document related operations
                    switch (queueMessage["operationType"].ToLower())
                    {
                        case OPERATION_INSERT:
                            isOperationSuccessful = docBuilder.InsertDocuments(queueMessage);
                            break;
                        case OPERATION_UPDATE:
                            isOperationSuccessful = docBuilder.UpdateDocuments(queueMessage);
                            break;
                        case OPERATION_DELETE:
                            isOperationSuccessful = docBuilder.DeleteDocuments(queueMessage);
                            break;
                        default:
                            logger.Error(string.Format("ERROR. Message : Unsupported operationType_{0}", queueMessage["operationType"]));
                            break;
                    }//end switch
                }//end try
                catch (Exception ex)
                {
                    logger.Error("Exception occured while pushing the document.", ex);
                }//end catch
                finally
                {
                    if (isOperationSuccessful)
                    {
                        logger.Info(string.Format("Documents successfully sent to IndexUpdaterConsumer."));
                    }
                    else
                    {
                        logger.Error(string.Format("Error! Documents couldn't be sent to IndexUpdaterConsumer."));
                        isOperationSuccessful = PerformDocumentOperations(queueMessage, ++c);
                    }
                }//end finally
            }//end null check
            else
            {
                logger.Info("No refrence object found for " + queueMessage["indexName"]);
                return true;
            }

            return isOperationSuccessful;
        }

    }
}
