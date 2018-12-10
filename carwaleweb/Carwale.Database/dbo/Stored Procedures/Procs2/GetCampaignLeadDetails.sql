IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetCampaignLeadDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetCampaignLeadDetails]
GO

	
-- =============================================
-- Author:		Sanjay Soni
-- Create date: 17/02/2016
-- Description:	Get Campaign Lead based details based on leadId 
-- =============================================
CREATE PROCEDURE [dbo].[GetCampaignLeadDetails]
	-- Add the parameters for the stored procedure here
	@leadId BIGINT
AS
BEGIN
	SELECT Name
		,Email
		,Mobile
		,VersionId
		,CityId
		,ZoneId
		,DealerId
		,CampaignId
		,ModelHistory
	FROM PQDealerAdLeads WITH(NOLOCK)
	WHERE Id = @leadId
END

