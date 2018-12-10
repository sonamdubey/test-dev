using Carwale.DAL.CoreDAL;
using Carwale.DAL.CoreDAL.MySql;
using Carwale.Entity.IPToLocation;
using Carwale.Interfaces.IPToLocation;
using Carwale.Notifications;
using Carwale.Notifications.Logs;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DAL.IPToLocation
{
    public class IPToLocationRepository : RepositoryBase,IIPToLocationRepository
    {
        private static readonly string IpToLocationIndexName = ConfigurationManager.AppSettings["IpToLocationIndexName"].ToString();
        private static readonly string IpToLocationTypeName = ConfigurationManager.AppSettings["IpToLocationTypeName"].ToString();
        public IPToLocationEntity GetIPToLocation(ulong ipNumber)
        {
            IPToLocationEntity objIPToLocationEntity = null;
            try
            {
                var client = ElasticClientInstance.GetInstance();
                var response = client.Search<IPToLocationEntity>(req => req.Index(IpToLocationIndexName).Type(IpToLocationTypeName).Size(1)
                        .Query(q => q
                            .Bool(bo => bo
                                .Must(mu => mu
                                    .Range(rng => rng.Field("ip_to").GreaterThanOrEquals(ipNumber > 0 ? ipNumber : 0))
                                    ,mu1=>mu1.
                                    Range(rng1 => rng1.Field("ip_from").LessThanOrEquals(ipNumber > 0 ? ipNumber : 0))))));

                if(response != null && response.Documents != null && response.Documents.Count() > 0)
                    objIPToLocationEntity = response.Documents.ElementAt(0);
                
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "IPToLocationRepository.GetIPToLocation()");
                objErr.LogException();
            }
            return objIPToLocationEntity;
        }
    }
}
