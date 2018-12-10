IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetCampaignIdOnDealerId]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetCampaignIdOnDealerId]
GO

	
-- =============================================
-- Author: Vinayak
-- Create date: 30/10/2015
-- Description:	Get CampaignId on dealer id
-- exec [dbo].[GetCampaignIdOnDealerId] 20460
-- =============================================
CREATE PROCEDURE [dbo].[GetCampaignIdOnDealerId]
	-- Add the parameters for the stored procedure here
	@DealerId INT
AS
BEGIN
	SELECT P.Id
	FROM PQ_DealerSponsored P WITH (NOLOCK)
	INNER JOIN Dealers D WITH (NOLOCK) ON D.Id = P.DealerId
	WHERE P.DealerId=@DealerId
	union
	SELECT P.Id
	FROM PQ_DealerSponsored P WITH (NOLOCK)
	INNER JOIN DealerLocatorConfiguration D WITH (NOLOCK) ON P.Id = D.PQ_DealerSponsoredId
	WHERE D.DealerId=@DealerId
	END
