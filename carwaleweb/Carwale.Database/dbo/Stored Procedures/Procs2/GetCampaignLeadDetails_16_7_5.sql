IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetCampaignLeadDetails_16_7_5]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetCampaignLeadDetails_16_7_5]
GO

	-- =============================================
-- Author:		Sanjay Soni
-- Create date: 17/02/2016
-- Description:	Get Campaign Lead based details based on leadId 
-- EXEC [GetCampaignLeadDetails_16_7_5] 21320
-- =============================================
create PROCEDURE [dbo].[GetCampaignLeadDetails_16_7_5]
	-- Add the parameters for the stored procedure here
	@leadId INT
AS
BEGIN
	SELECT PAL.[Name]
		,PAL.Email
		,PAL.Mobile
		,PAL.VersionId
		,PAL.CityId
		,CASE 
			WHEN PAL.ZoneId IS NOT NULL
				AND PAL.ZoneId != 0
				THEN CZ.ZoneName
			ELSE C.[Name]
			END CityName
		,PAL.ZoneId
		,PAL.DealerId
		,PAL.CampaignId
		,PAL.ModelHistory
	FROM PQDealerAdLeads PAL WITH (NOLOCK)
	INNER JOIN Cities C WITH (NOLOCK) ON PAL.CityId = C.ID
		AND C.IsDeleted = 0
	LEFT OUTER JOIN CityZones CZ WITH (NOLOCK) ON PAL.ZoneId = CZ.Id
		AND CZ.IsActive = 1
	WHERE PAL.Id = @leadId
END
