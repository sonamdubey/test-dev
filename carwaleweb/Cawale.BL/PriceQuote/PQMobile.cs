using System;
using System.Collections.Generic;
using Carwale.Interfaces.PriceQuote;
using Carwale.Interfaces.CarData;
using Carwale.Entity.PriceQuote;
using Carwale.Interfaces.Dealers;
using Carwale.Interfaces;
using Carwale.Interfaces.Geolocation;
using Carwale.DTOs;
using Carwale.DTOs.PriceQuote;
using System.Web;
using Carwale.Utility;

namespace Carwale.BL.PriceQuote
{
    public class PQMobileWeb<PQMobileT> : IPQ<PQMobile>
    {
        private readonly IPQRepository _pqRepo;
        private readonly ICarVersionRepository _versionsRepo;
        private readonly IDealerSponsoredAdRespository _newCarDealer;
        private readonly IQueueService<PriceQuoteEntity> _queueService;
        private readonly IPQCacheRepository _cachedRepo;
        

        public PQMobileWeb(IPQRepository pqRepo, 
            ICarVersionRepository versionsRepo, 
            IDealerSponsoredAdRespository newCarDealer, 
            IQueueService<PriceQuoteEntity> queueService,
            IPQCacheRepository cachedRepo)
        {
            _pqRepo = pqRepo;
            _versionsRepo = versionsRepo;
            _newCarDealer = newCarDealer;
            _queueService = queueService;
            _cachedRepo = cachedRepo;
        }

        /// <summary>
        /// For getting PriceQuote,All CarVersions,Dealers deatails And All carDetails based on multiple pqids
        /// </summary>
        /// <param name="pqIds">comma separated pqIds(Inquiry Ids)</param>
        /// <returns>A  List ot type PQ</returns>
        /// WrittrnBy : ASHISH Verma on 15/06/2014
        public List<PQMobile> GetPQByIds(string pqIdsFromQs)
        {
            var pqListForIds = new List<PQMobile>();
            var pqIds = pqIdsFromQs.Split(',');

            foreach (var pqId in pqIds)
            {               
                var pq = _pqRepo.GetPQById(Convert.ToUInt32(pqId));

                pqListForIds.Add(new PQMobile()
                {
                    PQId = pq.PQId,
                    PriceQuoteList = pq.PriceQuoteList,
                    MakeId = pq.MakeId,
                    MakeName = pq.MakeName,
                    ModelId = pq.ModelId,
                    ModelName = pq.ModelName,
                    VersionId = pq.VersionId,
                    VersionName = pq.VersionName,
                    CityId = pq.CityId,
                    ZoneId = pq.ZoneId,
                    MaskingName = pq.MaskingName,
                    SponsoredDealer = _newCarDealer.GetSponsoredDealer(pq.ModelId, pq.CityId,pq.ZoneId),
                    CarVersions = _versionsRepo.GetCarVersionsByType("new", pq.ModelId),                    
                    LargePic = pq.LargePic,
                    CityName = pq.CityName,
                    ZoneName = pq.ZoneName,
                    OnRoadPrice = pq.OnRoadPrice
                });
            }

            return pqListForIds;
        }

        /// <summary>
        /// For getting PriceQuote,All CarVersions,Dealers deatails And All carDetails based on customer datails
        /// </summary>
        /// <param name="pqInputes">Customer Details</param>
        /// <returns>A  List ot type PQ</returns>
        /// /// WrittrnBy : Ashish Verma on 15/06/2014
        public List<PQMobile> GetPQ(PQInput pqInputes)
        {
            var pqList = new List<PQMobile>();
            //getting and assigning User Ip
            pqInputes.ClientIp = UserTracker.GetUserIp();
            
            var pq = _cachedRepo.GetPQ(pqInputes);
            
            pqList.Add(new PQMobile()
            {
                PQId = pq.PQId,
                PriceQuoteList = pq.PriceQuoteList,
                MakeId = pq.MakeId,
                MakeName = pq.MakeName,
                ModelId = pq.ModelId,
                ModelName = pq.ModelName,
                VersionId = pq.VersionId,
                VersionName = pq.VersionName,
                CityId = pq.CityId,
                MaskingName = pq.MaskingName,
                ZoneId = pq.ZoneId,
                SponsoredDealer = _newCarDealer.GetSponsoredDealer(pq.ModelId, pq.CityId, pq.ZoneId),
                CarVersions = _versionsRepo.GetCarVersionsByType("new", pq.ModelId),
                LargePic = pq.LargePic,
                CityName=pq.CityName,
                ZoneName = pq.ZoneName,
                OnRoadPrice=pq.OnRoadPrice
            });

            // If valid quote generated, push data to queue to initiate PPQ(Post Price Quote Process)
            if (pq.PQId > 0)
            {
                //commented by vikas j on 13/10/2014 as PQ taken should not og to CRM anly form fill be pushed.
                //PostPQProcess.SendDataToRabbitMQ(_queueService, pqInputes, pq.PQId);

                //Function For Tracking cust info and saving cust info to PQ_ClientInfo table 
                //Written By : Ashish Verma on 15/06/2014
                _pqRepo.TrackClientInfo(GetClientInfo(pq.PQId,pqInputes));
            }
            return pqList;
        }

        /// <summary>
        /// For Getting Client Info object
        /// </summary>
        /// <param name="pqInputes">Customer Details and PQid</param>
        /// <returns>Client info object of type PQUserInfoTrackEntity</returns>
        /// /// WrittrnBy : Ashish Verma on 19/09/2014
        private PQUserInfoTrackEntity GetClientInfo(ulong PQId, PQInput pqInputes)
        {
            var userTrackingObj = new PQUserInfoTrackEntity()
               {
                   PQId = PQId,
                   ClientIp = pqInputes.ClientIp,
                   AspSessoinId = UserTracker.GetAspSessionCookie(),
                   CWCookievalue = UserTracker.GetSessionCookie(),
                   EntryDate = DateTime.Now.ToString()
               };
            return userTrackingObj;
        }

    }
}
