﻿using Bikewale.Entities.BikeBooking;
using System.Collections.Generic;

namespace Bikewale.Interfaces.PriceQuote
{
    /// <summary>
    /// Created by  :   Sumit Kate on 02 May 2016
    /// Description :   Lead Notification Interface
    /// Modified BY : Lucky Rathore on 12 May 2016
    /// Description : Signature of NotifyDealer.
    /// </summary>
    public interface ILeadNofitication
    {
        void NotifyCustomer(uint pqId, string bikeName, string bikeImage, string dealerName, string dealerEmail, string dealerMobileNo, string organization, string address, string customerName, string customerEmail, List<PQ_Price> priceList, List<OfferEntity> offerList, string pinCode, string stateName, string cityName, uint totalPrice, Bikewale.Entities.PriceQuote.DPQSmsEntity objDPQSmsEntity, string requestUrl, uint? leadSourceId, string platformId = "", uint isInsuranceFree = 0);
        void NotifyDealer(uint pqId, string makeName, string modelName, string versionName, string dealerName, string dealerEmail, string customerName, string customerEmail, string customerMobile, string areaName, string cityName, List<PQ_Price> priceList, int totalPrice, List<OfferEntity> offerList, string imagePath, string dealerMobile, string bikeName);
        void PushtoAB(string dealerId, uint pqId, string customerName, string customerMobile, string customerEmail, string versionId, string cityId);
    }
}
