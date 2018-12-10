IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetCampaignIdByActualDealerId]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetCampaignIdByActualDealerId]
GO

	-- =============================================
-- Author: Vinayak
-- Create date: 07/08/2015
-- Description:	Get campaignId based on dealerId
-- =============================================
CREATE PROCEDURE [dbo].[GetCampaignIdByActualDealerId] 
	@ActualDealerId INT
	,@CampaignId INT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT @CampaignId = pq.Id
	FROM DealerLocatorConfiguration dlc WITH (NOLOCK) inner join   
	PQ_DealerSponsored pq WITH (NOLOCK) on dlc.PQ_DealerSponsoredId=pq.Id
	WHERE dlc.DealerId = @ActualDealerId
	--AND [dbo].[IsCampaignActiveBasic](pq.Id)=1
END

	
 
