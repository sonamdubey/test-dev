using Carwale.DTOs.IPToLocation;
using Carwale.Entity.IPToLocation;
using Carwale.Interfaces.Geolocation;
using Carwale.Interfaces.IPToLocation;
using Carwale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Carwale.BL.IPToLocation
{
    public class IPToLocation:IIPToLocation
    {
        protected readonly IIPToLocationCacheRepository _ipToLocationCacheRepo;
        protected readonly IGeoCitiesCacheRepository _geoCitiesCacheRepository;

        public IPToLocation(IIPToLocationCacheRepository ipToLocationCacheRepo, IGeoCitiesCacheRepository geoCitiesCacheRepository)
        {
            _ipToLocationCacheRepo = ipToLocationCacheRepo;
            _geoCitiesCacheRepository = geoCitiesCacheRepository;
        }

        public IPToLocationEntity GetCity()
        {
            ulong IpNumber;
            IPToLocationEntity objIPToLocationEntity = new IPToLocationEntity();
            try
            {
                string ipAddress = HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"];
                if (!string.IsNullOrEmpty(ipAddress))
                {
                    IpNumber = CalculateIpNumber(ipAddress);
                    if (IpNumber > 0UL)
                        return _ipToLocationCacheRepo.GetIPToLocation(IpNumber);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "IPToLocation.GetCity()");
                objErr.LogException();
            }
            return objIPToLocationEntity;
        }

        public IPToLocationDTO GetCity_v1()
        {
            IPToLocationDTO objIPToLocation = new IPToLocationDTO();
            try
            {
                IPToLocationEntity objIPToLocationEntity = GetCity();
                if(objIPToLocationEntity.CityId > 0 )
                {
                    bool isAreaAvailable = _geoCitiesCacheRepository.IsAreaAvailable(objIPToLocationEntity.CityId);
                    objIPToLocation.CityId = objIPToLocationEntity.CityId;
                    objIPToLocation.CityName = objIPToLocationEntity.CityName;
                    objIPToLocation.IsAreaAvailable = isAreaAvailable;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "IPToLocation.GetCity()");
                objErr.LogException();
            }
            return objIPToLocation;
        }

        private ulong CalculateIpNumber(string ipAddress)
        {
            ulong IpNumber = 0UL;
            try
            {
                IPAddress address;
                if (IPAddress.TryParse(ipAddress, out address))
                {
                    if (address.AddressFamily.ToString() != "InterNetworkV6")
                    {
                        string[] strArray = address.ToString().Split('.');
                        IpNumber = ulong.Parse(strArray[0]) * 16777216UL + ulong.Parse(strArray[1]) * 65536UL + ulong.Parse(strArray[2]) * 256UL + ulong.Parse(strArray[3]);
                    }
                }
            }
            catch (Exception ex)
            { 
                ExceptionHandler objErr = new ExceptionHandler(ex, "IPToLocation.CalculateIpNumber()");
                objErr.LogException();
            }
            return IpNumber;
        }
    }
}


