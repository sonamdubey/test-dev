using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.Interface.ManufacturerCampaign
{
    /// <summary>
    /// Created by : Sajal Gupta on 31/08/2016
    /// Description : This interface contains declaration of function to relase masking number;
    /// </summary>
   public  interface IManufacturerReleaseMaskingNumber
    {
        bool ReleaseNumber(uint dealerId, int campaignId, string maskingNumber, int userId);
    }
}
