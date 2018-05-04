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
            log4net.Config.XmlConfigurator.Configure();
            try
            {
                NO_OF_RETRIES = Convert.ToInt32(ConfigurationManager.AppSettings["RetryCount"]);
                DOCBUILDERS = new Dictionary<String, IDocumentBuilder>()
                {
                    {ConfigurationManager.AppSettings["BikeIndex"], new ModelIndexDocumentBuilder()},
                    {ConfigurationManager.AppSettings["BikePriceIndex"], new ModelPriceDocumentBuilder()}
                };
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("An Error Occured while fetching NO_OF_RETRIES from AppSettings/Creating Dictionary", ex);
            }

        }


        static void Main(string[] args)
        {
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
                Logs.WriteErrorLog("Consumer Ended at : " + DateTime.Now);
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
            try
            {
                if (nvc != null
                            && nvc.HasKeys()
                            && !String.IsNullOrEmpty(nvc["indexName"])
                            && !String.IsNullOrEmpty(nvc["documentType"])
                            && !String.IsNullOrEmpty(nvc["operationType"]))
                {
                    Logs.WriteInfoLog("RabbitMQExecution :Received job : " + nvc["indexName"] + ", Document Type: " + nvc["documentType"] + ", Operation Type: " + nvc["operationType"]);
                    return PerformDocumentOperations(nvc);
                }
                else
                {
                    Logs.WriteInfoLog("NVC missing data in certain keys.");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("Error while performing operation for " + nvc["indexName"], ex);
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
            if (c >= NO_OF_RETRIES)
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
                            Logs.WriteErrorLog(string.Format("ERROR. Message : Unsupported operationType_{0}", queueMessage["operationType"]));
                            break;
                    }//end switch
                }//end try
                catch (Exception ex)
                {
                    Logs.WriteErrorLog("Exception occured while pushing the document.", ex);
                }//end catch
                finally
                {
                    if (isOperationSuccessful)
                    {
                        Logs.WriteInfoLog(string.Format("Documents successfully sent to IndexUpdaterConsumer."));
                    }
                    else
                    {
                        Logs.WriteErrorLog(string.Format("Error! Documents couldn't be sent to IndexUpdaterConsumer."));
                        isOperationSuccessful = PerformDocumentOperations(queueMessage, ++c);
                    }
                }//end finally
            }//end null check
            else
            {
                Logs.WriteInfoLog("No refrence object found for " + queueMessage["indexName"]);
                return true;
            }

            return isOperationSuccessful;
        }

    }
}
