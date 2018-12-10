IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertLandingPageCampaignCityRules]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertLandingPageCampaignCityRules]
GO

	-- =============================================
-- Author:		Shalini Nair
-- Create date: 28/07/2016
-- Description:	Insert city rules for landing page campaigns
-- =============================================
CREATE PROCEDURE [dbo].[InsertLandingPageCampaignCityRules] @CampaignId INT
	,@StateId INT
	,@CityId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Insert statements for procedure here
	INSERT INTO LandingPageCities (
		CampaignId
		,StateId
		,CityId
		)
	VALUES (
		@CampaignId
		,CASE 
			WHEN @StateId = 0
				THEN (
						SELECT top 1 ct.StateId
						FROM Cities ct WITH (NOLOCK)
						WHERE ct.ID = @CityId
						)
			ELSE @StateId
			END
		,@CityId
		)
END

