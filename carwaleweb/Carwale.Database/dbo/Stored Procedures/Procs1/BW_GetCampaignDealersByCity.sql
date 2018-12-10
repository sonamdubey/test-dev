IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_GetCampaignDealersByCity]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_GetCampaignDealersByCity]
GO

	
-- =============================================
-- Author:		Sumit Kate
-- Create date: 09 May 2016
-- Description:	Returns the list of Dealers for a city based on Subscription Model
--	@CityId				:	City Id
-- e.g. EXEC [dbo].[BW_GetCampaignDealersByCity] 1
-- =============================================
CREATE PROCEDURE [dbo].[BW_GetCampaignDealersByCity] 
	@CityId INT
AS
BEGIN
	IF (ISNULL(@CityId,0) > 0)
	BEGIN
		SELECT DISTINCT
			d.ID,
			d.Lattitude,
			d.Longitude
		FROM Dealers d WITH(NOLOCK)
		INNER JOIN TC_ContractCampaignMapping ccm WITH(NOLOCK)
		ON d.id = ccm.DealerId AND d.IsDealerActive = 1 AND d.IsDealerDeleted = 0 AND ccm.ContractStatus = 1
		INNER JOIN BW_PQ_DealerCampaigns dc WITH(NOLOCK)
		ON ccm.CampaignId = dc.Id AND ccm.ContractId = dc.ContractId AND ccm.DealerId = dc.DealerId AND dc.IsActive = 1
		INNER JOIN BW_PQ_CampaignRules cr WITH(NOLOCK)
		ON dc.Id = cr.CampaignId AND cr.IsActive = 1 AND cr.CityId = @CityId
	END
END
