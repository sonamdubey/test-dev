IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_UpdateBWDealerContractCampaign]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_UpdateBWDealerContractCampaign]
GO

	
-- =============================================
-- Created on	:	Sumit Kate on 18 Mar 2016
-- Description	:	Updates the TC_ContractCampaignMapping and maps campaign with contract for BW
-- =============================================
CREATE PROCEDURE dbo.BW_UpdateBWDealerContractCampaign @CampaignId INT
	,@ContractId INT
AS
BEGIN
	UPDATE TC_ContractCampaignMapping
	SET ContractStatus = 1
		,CampaignId = @CampaignId
	WHERE ContractId = @ContractId
		AND ApplicationId = 2
END
