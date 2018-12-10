IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertPQSponsoredAdViewLog]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertPQSponsoredAdViewLog]
GO
	-- =============================================
-- Author:		Ashish Verma
-- Create date: 22/09/2014
-- Description: Insert PQSponsoredAdViewLog

-- =============================================
CREATE PROCEDURE [dbo].[InsertPQSponsoredAdViewLog] --[dbo].[PQ_GetDealerSponsorship] 28,1,null
	-- Add the parameters for the stored procedure here
	@PQId NUMERIC,
	@CampaignId  INT,
	@PlatformId INT
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

   Insert Into PQ_DealerSponsoredAd_ViewLogs(PQId,CampaignId,PlatformId) values (@PQId,@CampaignId,@PlatformId)

END




