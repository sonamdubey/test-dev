IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetAllRunningCampaignDealer]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetAllRunningCampaignDealer]
GO

	

-- =============================================
-- Author:		Sanjay Soni
-- Create date: 10/08/2016
-- Description:	Get All Running Campaign Dealer
-- =============================================
CREATE PROCEDURE [dbo].[GetAllRunningCampaignDealer] 
AS
BEGIN
    SELECT P.Id AS CampaignId, DealerId, D.Organization AS DealerName FROM PQ_DealerSponsored P
	INNER JOIN Dealers D with(nolock) on D.Id = P.DealerId
	WHERE P.Id IN (SELECT DISTINCT CampaignId FROM vwRunningCampaigns) 
END

