IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetAllHouseCrossSell_v16_6_1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetAllHouseCrossSell_v16_6_1]
GO

	

-- =============================================
-- Author:		Shalini Nair
-- Create date: 11/05/2016
-- Description:	To get all house cross-sell campaigns
-- Modifier : Sachin Bharti (6th June 2016)
-- Purpose : Added new field CategoryName
-- =============================================
CREATE PROCEDURE [dbo].[GetAllHouseCrossSell_v16_6_1]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Insert statements for procedure here
	SELECT FA.Id AS CampaignId
		,FA.Name AS CampaignName
		,CC.Name AS CampaignCategory
		,FA.CampaignCategoryId
		,CONVERT(VARCHAR(10), FA.StartDate, 103) StartDate
		,CONVERT(VARCHAR(10), FA.EndDate, 103) EndDate
		,FA.IsActive
		,FA.ExternalLinkText
		,FA.LinkClickTracker
		,FA.CarNameClickTracker
		,FA.CarImageClickTracker
	FROM FeaturedAd FA WITH (NOLOCK)
	LEFT JOIN CampaignCategory CC (NOLOCK) ON FA.CampaignCategoryId = CC.Id
	ORDER BY FA.StartDate DESC
END

