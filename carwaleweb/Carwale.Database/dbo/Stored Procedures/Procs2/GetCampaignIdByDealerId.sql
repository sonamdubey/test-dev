IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetCampaignIdByDealerId]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetCampaignIdByDealerId]
GO

	

-- =============================================
-- Author:		Chetan T
-- Create date: <25/07/2016>
-- Description:	GetCampaignIdByDealerId
-- EXEC GetCampaignIdByDealerId	11743
-- =============================================
CREATE PROCEDURE [dbo].[GetCampaignIdByDealerId] @DealerId INT
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT PQ_DealerSponsoredId AS CampaignId
	FROM Dealers D WITH (NOLOCK) 
	JOIN DealerLocatorConfiguration DLC WITH (NOLOCK) ON D.ID = DLC.DealerId 
		AND DLC.IsLocatorActive = 1
		AND DLC.IsDealerLocatorPremium = 1
	WHERE DLC.DealerId = @DealerId
END

