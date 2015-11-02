﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.PriceQuote.MobileVerification
{
    /// <summary>
    /// Mobile Verification resend output entity
    /// Author  :   Sumit Kate
    /// Date    :   21 Aug 2015
    /// </summary>
    public class PQResendMobileVerificationOutput
    {
        [JsonProperty("isSuccess")]
        public bool IsSuccess { get; set; }

        [JsonProperty("noOfAttempts")]
        public sbyte NoOfAttempts { get; set; }
    }
}
