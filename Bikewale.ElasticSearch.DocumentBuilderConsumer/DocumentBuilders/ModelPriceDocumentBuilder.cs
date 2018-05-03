using Bikewale.ElasticSearch.DocumentBuilderConsumer.Interfaces;
using Bikewale.ElasticSearch.Entities;
using Bikewale.Utility;
using Consumer;
using MySql.CoreDAL;
using Newtonsoft.Json;
using RabbitMqPublishing;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.ElasticSearch.DocumentBuilderConsumer.DocumentBuilders
{
    public class ModelPriceDocumentBuilder : IDocumentBuilder
    {
        public bool InsertDocuments(System.Collections.Specialized.NameValueCollection nvc)
        {
            string models = nvc["modelIds"];
            string cities = nvc["cityIds"];
            IList<ModelPriceDocument> indexDocs = GetDocuments(models, cities);


            if (indexDocs != null)
            {

                foreach (ModelPriceDocument doc in indexDocs)
                {
                    NameValueCollection packet = new NameValueCollection();
                    packet["indexName"] = nvc["indexName"];
                    packet["documentType"] = nvc["documentType"];
                    packet["documentId"] = doc.Id;
                    packet["operationType"] = nvc["operationType"];
                    packet["documentJson"] = JsonConvert.SerializeObject(doc);

                    PushToQueue(packet);

                    Logs.WriteInfoLog("RabbitMQExecution :Pushed job : " + packet["indexName"] + ", Document Type: " + packet["documentType"] + ", Operation Type " + packet["operationType"] + ", Document ID: " + packet["documentId"] + ", Document: " + packet["documentJson"]);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool UpdateDocuments(System.Collections.Specialized.NameValueCollection nvc)
        {
            string models = nvc["modelIds"];
            string cities = nvc["cityIds"];
            IList<ModelPriceDocument> indexDocs = GetDocuments(models, cities);


            if (indexDocs != null)
            {

                foreach (ModelPriceDocument doc in indexDocs)
                {
                    NameValueCollection packet = new NameValueCollection();
                    packet["indexName"] = nvc["indexName"];
                    packet["documentType"] = nvc["documentType"];
                    packet["documentId"] = doc.Id;
                    packet["operationType"] = nvc["operationType"];
                    packet["documentJson"] = JsonConvert.SerializeObject(doc);

                    PushToQueue(packet);

                    Logs.WriteInfoLog("RabbitMQExecution :Pushed job : " + packet["indexName"] + ", Document Type: " + packet["documentType"] + ", Operation Type " + packet["operationType"] + ", Document ID: " + packet["documentId"] + ", Document: " + packet["documentJson"]);
                }
                return true;
            }
            else
            {
                return false;
            }        
        }

        /// <summary>
        /// Created By : Deepak Israni on 9 March 2018
        /// Description: Function to push to BWEsIndexUpdater queue.
        /// </summary>
        /// <param name="nvc"></param>
        private static void PushToQueue(NameValueCollection nvc)
        {
            RabbitMqPublish publish = new RabbitMqPublish();
            publish.PublishToQueue(ConfigurationManager.AppSettings["BWEsIndexUpdaterQueue"], nvc);
        }

        public bool DeleteDocuments(System.Collections.Specialized.NameValueCollection nvc)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Created By : Deepak Israni on 3 May 2018
        /// Description: DAL method to generate an bikewalepricingindex (ES Index) document.
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        private List<ModelPriceDocument> GetDocuments(string modelIds, string cityIds)
        {
            String spName = "getmodelpriceindexbycity";

            List<ModelPriceDocument> objList = null;
            ModelPriceDocument docObj = null;
            VersionEntity verObj = null;

            uint _lastModelId = 0;
            uint _lastCityId = 0;
            uint _currentModelId = 0;
            uint _currentCityId = 0;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand(spName))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelids", DbType.String, modelIds));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityids", DbType.String, cityIds));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objList = new List<ModelPriceDocument>();
                            List<VersionEntity> versions = null;

                            while (dr.Read())
                            {
                                _currentModelId = SqlReaderConvertor.ToUInt32(dr["BikeModelId"]);
                                _currentCityId = SqlReaderConvertor.ToUInt32(dr["CityId"]);

                                if (_currentModelId != _lastModelId || _currentCityId != _lastCityId)
                                {
                                    if (docObj != null)
                                    {
                                        docObj.VersionPrice = versions;
                                        objList.Add(docObj);
                                    }

                                    docObj = new ModelPriceDocument()
                                    {
                                        Id = SqlReaderConvertor.ToUInt32(dr["BikeModelId"]) + "_" + SqlReaderConvertor.ToUInt32(dr["CityId"]),
                                        BikeModel = new ModelEntity()
                                        {
                                            ModelId = SqlReaderConvertor.ToUInt32(dr["BikeModelId"]),
                                            ModelName = Convert.ToString(dr["ModelName"]),
                                            ModelMaskingName = Convert.ToString(dr["ModelMaskingName"]),
                                            ModelStatus = GetStatus(Convert.ToBoolean(dr["IsNewModel"]), Convert.ToBoolean(dr["IsFuturisticModel"]))
                                        },
                                        BikeMake = new MakeEntity()
                                        {
                                            MakeId = SqlReaderConvertor.ToUInt32(dr["BikeMakeId"]),
                                            MakeName = Convert.ToString(dr["MakeName"]),
                                            MakeMaskingName = Convert.ToString(dr["MakeMaskingName"]),
                                            MakeStatus = GetStatus(Convert.ToBoolean(dr["IsNewMake"]), Convert.ToBoolean(dr["IsFuturisticMake"]))
                                        },
                                        City = new CityEntity()
                                        {
                                            CityId = SqlReaderConvertor.ToUInt32(dr["CityId"]),
                                            CityName = Convert.ToString(dr["CityName"]),
                                            CityMaskingName = Convert.ToString(dr["CityMaskingName"])
                                        }
                                    };

                                    versions = new List<VersionEntity>();

                                    _lastModelId = _currentModelId;
                                    _lastCityId = _currentCityId;
                                }

                                verObj = new VersionEntity()
                                {
                                    VersionId = SqlReaderConvertor.ToUInt32(dr["VersionId"]),
                                    VersionName = Convert.ToString(dr["VersionName"]),
                                    Exshowroom = SqlReaderConvertor.ToUInt32(dr["Price"]),
                                    VersionStatus = GetStatus(Convert.ToBoolean(dr["IsNewVersion"]), Convert.ToBoolean(dr["IsFuturisticVersion"]))
                                };

                                IList<PriceEntity> prices = new List<PriceEntity>();

                                prices.Add(new PriceEntity()
                                {
                                    PriceType = "Exshowroom",
                                    PriceValue = SqlReaderConvertor.ToUInt32(dr["Price"])
                                });

                                prices.Add(new PriceEntity()
                                {
                                    PriceType = "RTO",
                                    PriceValue = SqlReaderConvertor.ToUInt32(dr["RTO"])
                                });
                                prices.Add(new PriceEntity()
                                {
                                    PriceType = "Insurance",
                                    PriceValue = SqlReaderConvertor.ToUInt32(dr["Insurance"])
                                });

                                verObj.PriceList = prices;
                                verObj.Onroad = (uint)verObj.PriceList.Sum(prc => prc.PriceValue);

                                versions.Add(verObj);
                            }

                            if (docObj != null)
                            {
                                docObj.VersionPrice = versions;
                                objList.Add(docObj);
                            }

                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog(string.Format("BikewaleOpr.DAL.GetModelPriceDocument: Makeid- {0}, Cityid- {1}", modelIds, cityIds), ex);
            }

            return objList;
        }

        /// <summary>
        /// Created By : Deepak Israni on 8 March 2018
        /// Description: To get the status of make/model/version depending on they are new or futuristic.
        /// </summary>
        /// <param name="isNew"></param>
        /// <param name="isFuturistic"></param>
        /// <returns></returns>
        private static BikeStatus GetStatus(bool isNew, bool isFuturistic)
        {
            return !isNew ? (!isFuturistic ? BikeStatus.Discontinued : BikeStatus.Upcoming) : (!isFuturistic ? BikeStatus.New : BikeStatus.Invalid);
        }

    }
}
