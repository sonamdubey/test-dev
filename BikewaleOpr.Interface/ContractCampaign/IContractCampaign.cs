﻿using BikewaleOpr.Entities.ContractCampaign;
using System.Collections.Generic;

namespace BikewaleOpr.Interface.ContractCampaign
{
    /// <summary>
    /// Created By : Sushil Kumar on 14th July 2016
    /// Description : Interface for contract campaign mapping
    /// </summary>
    public interface IContractCampaign
    {
        IEnumerable<MaskingNumber> GetAllMaskingNumbers(uint dealerId);
        bool IsCCMapped(uint dealerId, uint contractId, uint campaignId);
        bool RelaseMaskingNumbers(uint dealerId, int userId, string maskingNumbers);
        bool AddCampaignContractData(ContractCampaignInputEntity _ccInputs);
    }
}
