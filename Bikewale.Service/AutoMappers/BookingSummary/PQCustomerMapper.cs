﻿using AutoMapper;
using Bikewale.DTO.PriceQuote.CustomerDetails;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.Service.AutoMappers.BookingSummary
{
    public class PQCustomerMapper
    {
        internal static DTO.PriceQuote.CustomerDetails.PQCustomer Convert(Entities.BikeBooking.PQCustomerDetail objCustomer)
        {
            Mapper.CreateMap<CustomerEntityBase, PQCustomerBase>();
            Mapper.CreateMap<VersionColor, PQColor>();
            Mapper.CreateMap<PQCustomerDetail, PQCustomer>();
            return Mapper.Map<PQCustomerDetail, PQCustomer>(objCustomer);
        }
    }
}