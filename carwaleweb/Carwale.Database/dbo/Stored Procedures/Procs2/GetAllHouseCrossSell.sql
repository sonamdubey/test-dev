IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetAllHouseCrossSell]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetAllHouseCrossSell]
GO

	

-- =============================================
-- Author:		Shalini Nair
-- Create date: 11/05/2016
-- Description:	To get all house cross-sell campaigns
-- =============================================
CREATE PROCEDURE [dbo].[GetAllHouseCrossSell]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Insert statements for procedure here
	SELECT FA.Id AS CampaignId
		,FA.NAME AS CampaignName
		,CONVERT(VARCHAR(10), FA.StartDate, 103) StartDate
		,CONVERT(VARCHAR(10), FA.EndDate, 103) EndDate
		,FA.IsActive
	FROM FeaturedAd FA WITH (NOLOCK)
	ORDER BY FA.StartDate DESC
END

