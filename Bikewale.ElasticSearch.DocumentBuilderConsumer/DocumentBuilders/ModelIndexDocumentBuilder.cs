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

namespace Bikewale.ElasticSearch.DocumentBuilderConsumer.DocumentBuilders
{
    public class ModelIndexDocumentBuilder : IDocumentBuilder
    {
        /// <summary>
        /// Created By : Deepak Israni on 8 March 2018
        /// Description: Function to get newly created documents and push them to BWEsIndexUpdater's queue for insertion of new documents in ES Index.
        /// </summary>
        /// <param name="nvc"></param>
        /// <returns></returns>
        public bool InsertDocuments(NameValueCollection nvc)
        {
            NameValueCollection inputs = new NameValueCollection();
            inputs["modelids"] = nvc["ids"];
            IEnumerable<BikeModelDocument> indexDocs = GetDocuments(inputs);

            if (indexDocs != null)
            {
                foreach (BikeModelDocument doc in indexDocs)
                {
                    NameValueCollection packet = new NameValueCollection();
                    packet["indexName"] = nvc["indexName"];
                    packet["documentType"] = nvc["documentType"];
                    packet["documentId"] = doc.Id;
                    packet["operationType"] = nvc["operationType"];
                    packet["documentJson"] = JsonConvert.SerializeObject(doc);

                    PushToQueue(packet);
                }
                return true;
            }
            else
            {
                return false;
            }
            
        }

        /// <summary>
        /// Created By : Deepak Israni on 8 March 2018
        /// Description: Function to get recently updated documents and push them to BWEsIndexUpdater's queue for updation of entries in ES Index.
        /// </summary>
        /// <param name="nvc"></param>
        /// <returns></returns>
        public bool UpdateDocuments(NameValueCollection nvc)
        {
            NameValueCollection inputs = new NameValueCollection();
            inputs["modelids"] = nvc["ids"];
            IEnumerable<BikeModelDocument> indexDocs = GetDocuments(inputs);

            if (indexDocs != null)
            {
                foreach (BikeModelDocument doc in indexDocs)
                {
                    NameValueCollection packet = new NameValueCollection();
                    packet["indexName"] = nvc["indexName"];
                    packet["documentType"] = nvc["documentType"];
                    packet["documentId"] = doc.Id;
                    packet["operationType"] = nvc["operationType"];
                    packet["documentJson"] = JsonConvert.SerializeObject(doc);

                    PushToQueue(packet);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Created By : Deepak Israni on 8 March 2018
        /// Description: Function to push the model ids of recently deleted models to BWEsIndexUpdater's queue for deletion in the ES Index.
        /// </summary>
        /// <param name="nvc"></param>
        /// <returns></returns>
        public bool DeleteDocuments(NameValueCollection nvc)
        {
            string[] models = nvc["ids"].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            foreach (String model in models)
            {
                NameValueCollection packet = new NameValueCollection();
                packet["indexName"] = nvc["indexName"];
                packet["documentType"] = nvc["documentType"];
                packet["documentId"] = model;
                packet["operationType"] = nvc["operationType"];

                PushToQueue(packet);
            }
            return true;
        }


        /// <summary>
        /// Created By : Deepak Israni on 8 March 2018
        /// Description: To create index documents for specific modelids.
        /// </summary>
        /// <param name="inputParameters"></param>
        /// <returns></returns>
        private IEnumerable<BikeModelDocument> GetDocuments(NameValueCollection inputParameters)
        {
            string[] _pricestypes = { "RTO", "Insurance", "Exshowroom" };
            String spName = "getnewmodelforbikeindex";

            IList<BikeModelDocument> objList = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand(spName))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelids", DbType.String, inputParameters["modelids"]));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objList = new List<BikeModelDocument>();
                            uint weight_count = 1;
                            while (dr.Read())
                            {
                                IList<PriceEntity> objPrices = new List<PriceEntity>();

                                //price components
                                for (int i = 0; i < _pricestypes.Length; i++)
                                {
                                    objPrices.Add(new PriceEntity()
                                    {
                                        PriceType = _pricestypes[i],
                                        PriceValue = SqlReaderConvertor.ToUInt32(dr[_pricestypes[i]])
                                    });
                                }

                                objList.Add(new BikeModelDocument()
                                    {
                                        //doc id and Weight
                                        Id = Convert.ToString(dr["ModelId"]),
                                        Weight = weight_count++,

                                        //make details
                                        BikeMake = new MakeEntity()
                                        {
                                            MakeId = SqlReaderConvertor.ToUInt32(dr["MakeId"]),
                                            MakeName = Convert.ToString(dr["MakeName"]),
                                            MakeMaskingName = Convert.ToString(dr["MakeMaskingName"]),
                                            MakeStatus = GetStatus(SqlReaderConvertor.ToBoolean(dr["IsNewMake"]), Convert.ToBoolean(dr["IsFuturisticMake"]))
                                        },

                                        //model details
                                        BikeModel = new ModelEntity()
                                        {
                                            ModelId = SqlReaderConvertor.ToUInt32(dr["ModelId"]),
                                            ModelName = Convert.ToString(dr["ModelName"]),
                                            ModelMaskingName = Convert.ToString(dr["ModelMaskingName"]),
                                            ModelStatus = GetStatus(SqlReaderConvertor.ToBoolean(dr["IsNewModel"]), Convert.ToBoolean(dr["IsFuturisticModel"]))
                                        },

                                        //top version
                                        TopVersion = new VersionEntity()
                                        {
                                            VersionId = SqlReaderConvertor.ToUInt32(dr["TopVersionId"]),
                                            VersionName = Convert.ToString(dr["VersionName"]),
                                            Mileage = SqlReaderConvertor.ToUInt32(dr["Mileage"]),
                                            KerbWeight = SqlReaderConvertor.ToUInt32(dr["KerbWeight"]),
                                            Displacement = SqlReaderConvertor.ParseToDouble(dr["Displacement"]),
                                            Power = SqlReaderConvertor.ParseToDouble(dr["Power"]),
                                            PriceList = objPrices,
                                            Exshowroom = SqlReaderConvertor.ToUInt32(dr["Exshowroom"]),
                                            Onroad = SqlReaderConvertor.ToUInt32(dr["RTO"]) + SqlReaderConvertor.ToUInt32(dr["Insurance"]) + SqlReaderConvertor.ToUInt32(dr["Exshowroom"]),
                                            VersionStatus = GetStatus(SqlReaderConvertor.ToBoolean(dr["IsNewVersion"]), SqlReaderConvertor.ToBoolean(dr["IsFuturisticVersion"]))
                                        },

                                        BikeName = Convert.ToString(dr["MakeName"]) + " " + Convert.ToString(dr["ModelName"]),
                                        BodyStyleId = SqlReaderConvertor.ToUInt32(dr["BodyStyleId"]),

                                        //bike media/reviews
                                        BikeImage = new ImageEntity()
                                        {
                                            ImageURL = Convert.ToString(dr["ImageURL"]),
                                            HostURL = Convert.ToString(dr["HostURL"])
                                        },
                                        ImageCount = SqlReaderConvertor.ToUInt32(dr["ImageCount"]),
                                        VideosCount = SqlReaderConvertor.ToUInt32(dr["VideosCount"]),
                                        ExpertReviewsCount = SqlReaderConvertor.ToUInt32(dr["ExpertReviewsCount"]),
                                        UserReviewsCount = SqlReaderConvertor.ToUInt32(dr["UserReviewsCount"]),
                                        ReviewRatings = SqlReaderConvertor.ParseToDouble(dr["ReviewRatings"]),
                                        RatingsCount = SqlReaderConvertor.ToUInt32(dr["RatingsCount"]),
                                       
                                    });
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("Exception at Bikewale.ElasticSearch.DocumentBuilderConsumer.DocumentBuilders: GetDocuments() ", ex);
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

    }
}
