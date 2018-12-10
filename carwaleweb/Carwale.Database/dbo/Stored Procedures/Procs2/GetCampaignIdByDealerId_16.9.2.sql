IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetCampaignIdByDealerId_16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetCampaignIdByDealerId_16]
GO

	
-- =============================================
-- Author:		Chetan T
-- Create date: <25/07/2016>
-- Description:	GetCampaignIdByDealerId
-- EXEC GetCampaignIdByDealerId	6678
-- =============================================
CREATE PROCEDURE [dbo].[GetCampaignIdByDealerId_16.9.2] @DealerId INT
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT PQ_DealerSponsoredId AS CampaignId
	FROM Dealers D WITH (NOLOCK) 
	JOIN DealerLocatorConfiguration DLC WITH (NOLOCK) ON D.ID = DLC.DealerId 
		WHERE DLC.DealerId = @DealerId
END

