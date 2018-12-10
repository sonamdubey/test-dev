IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetCrossSellCampaign_V15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetCrossSellCampaign_V15]
GO

	
-- =============================================
-- Author:		Sourav Roy						EXEC GetAllVersionsOnRoadPrice 99,13
-- Create date: 30/10/2015
-- Description:	Get Cross Sell Details
-- Modified : Vicky Lund, 19/02/2016, Used Contract parameters (vwRunningcampaigns) instead of Campaign parameters
-- Modified : Vikas J, 22/02/2016, Added filter New=1 for modelid and versionid
-- =============================================
CREATE PROCEDURE [dbo].[GetCrossSellCampaign_V15.10.4]
	-- Add the parameters for the stored procedure here
	@CityId INT
	,@ZoneId INT
	,@TargetVersionId INT
AS
BEGIN
	SELECT TOP (1) CMK.NAME AS CrossSellMakeName
		,CM.NAME AS CrossSellModelName
		,CV.NAME AS CrossSellVersionName
		,CSR.CrossSellVersion AS CrossSellVersionId
		,CV.CarModelId AS CrossSellModelId
		,SDT.TemplateId AS TemplateId
		,SDT.TemplateName
		,CSC.CampaignId
	FROM PQ_CrossSellCampaignRules CSR WITH (NOLOCK)
	INNER JOIN PQ_CrossSellCampaign CSC WITH (NOLOCK) ON CSC.Id = CSR.CrossSellCampaignId
	INNER JOIN PQ_SponsoredDealeAd_Templates SDT WITH (NOLOCK) ON CSR.TemplateId = SDT.TemplateId
	INNER JOIN vwRunningCampaigns VRC WITH (NOLOCK) ON VRC.CampaignId = CSC.CampaignId
	INNER JOIN CarVersions CV WITH (NOLOCK) ON CV.ID = CSR.CrossSellVersion
		AND CV.New = 1
	INNER JOIN CarModels CM WITH (NOLOCK) ON CM.Id = CV.CarModelId
		AND CM.New = 1
	INNER JOIN CarMakes CMK WITH (NOLOCK) ON CMK.ID = CM.CarMakeId
	WHERE SDT.PlatformId = 1
		AND SDT.CategoryId = 2
		AND (
			(CSR.StateId = - 1) --Pan India
			OR (
				CSR.StateId = (
					SELECT StateId
					FROM Cities WITH (NOLOCK)
					WHERE Id = @CityId
					)
				AND (
					(
						CSR.CityId = @CityId
						AND (
							ISNULL(CSR.ZoneId, 0) = ISNULL(@ZoneId, 0)
							OR @CityId NOT IN (
								1
								,10
								)
							)
						)
					OR CSR.CityId = - 1
					)
				) --City wise or Pan State
			)
		AND CSR.TargetVersion = @TargetVersionId
END
