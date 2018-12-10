IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertLandingPageCampaignModelRules]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertLandingPageCampaignModelRules]
GO
	-- =============================================
-- Author:		Shalini Nair
-- Create date: 28/07/2016
-- Description:	Insert model rules for landing page campaign
-- =============================================
CREATE PROCEDURE [dbo].[InsertLandingPageCampaignModelRules] @CampaignId INT
	,@MakeId INT
	,@ModelId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Insert statements for procedure here
	INSERT INTO LandingPageModels (
		CampaignId
		,MakeId
		,ModelId
		)
	VALUES (
		@CampaignId
		,CASE 
			WHEN @MakeId = 0
				THEN (
						SELECT top 1 CarMakeId
						FROM CarModels CM WITH (NOLOCK)
						WHERE CM.ID = @ModelId
						)
			ELSE @MakeId
			END
		,@ModelId
		)
END

