IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetDealerIdOnCampaignId]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetDealerIdOnCampaignId]
GO

	
-- =============================================
-- Author: Vinayak
-- Create date: 30/10/2015
-- Description:	Get dealerid on campaignid
-- exec [dbo].[GetDealerIdOnCampaignId] 4684
-- =============================================
create PROCEDURE [dbo].[GetDealerIdOnCampaignId]
	-- Add the parameters for the stored procedure here
	@CampaignId INT
AS
BEGIN
	SELECT P.DealerId
	FROM PQ_DealerSponsored P WITH (NOLOCK)
	WHERE P.Id=@CampaignId
	END

