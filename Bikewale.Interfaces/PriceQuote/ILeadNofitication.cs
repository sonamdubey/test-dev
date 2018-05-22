using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.Dealer;
using Bikewale.Entities.PriceQuote;
using System.Collections.Generic;

namespace Bikewale.Interfaces.PriceQuote
{
    /// <summary>
    /// Created by  :   Sumit Kate on 02 May 2016
    /// Description :   Lead Notification Interface
    /// Modified BY : Lucky Rathore on 12 May 2016
    /// Description : Signature of NotifyDealer.
    /// Modified By : Lucky Rathore on 11 July 2016.
    /// Description : parameter dealerArea added in Signature of NotifyDealer. 
    /// Modified by :   Sumit Kate on 18 Aug 2016
    /// Description :   Push Lead To Gaadi.com external API
    /// Modified by : Pratibha Verma on 27 April 2018
    /// Description : Added parameters 'additionalNumbers' and 'additionalEmails' in Signature of NotifyDealer
    /// </summary>
    public interface ILeadNofitication
    {
        void NotifyCustomer(uint pqId, string bikeName, string bikeImage, string dealerName, string dealerEmail, string dealerMobileNo, string organization, string address, string customerName, string customerEmail, List<PQ_Price> priceList, List<OfferEntity> offerList, string pinCode, string stateName, string cityName, uint totalPrice, DPQSmsEntity objDPQSmsEntity, string requestUrl, uint? leadSourceId, string versionName, double dealerLat, double dealerLong, string workingHours, string platformId = "");
        void NotifyDealer(uint pqId, string makeName, string modelName, string versionName, string dealerName, string dealerEmail, string customerName, string customerEmail, string customerMobile, string areaName, string cityName, List<PQ_Price> priceList, int totalPrice, List<OfferEntity> offerList, string imagePath, string dealerMobile, string bikeName, string dealerArea, string additionalNumbers, string additionalEmails);
        void PushtoAB(string dealerId, uint pqId, string customerName, string customerMobile, string customerEmail, string versionId, string cityId);
        bool PushLeadToGaadi(ManufacturerLeadEntity leadEntity);
    }
}
