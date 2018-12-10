IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetCrossSellCampaign_V16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetCrossSellCampaign_V16]
GO

	
-- =============================================
-- Author:		Sourav Roy						EXEC GetAllVersionsOnRoadPrice 99,13
-- Create date: 30/10/2015
-- Description:	Get Cross Sell Details
-- Modified : Vicky Lund, 19/02/2016, Used Contract parameters (vwRunningcampaigns) instead of Campaign parameters
-- Modified : Vikas J, 22/02/2016, Added filter New=1 for modelid and versionid
-- Modified : Sanjay Soni, 16/03/2016, Removed TemplateId parameter
-- Modified : Chetan Thambad, 05/04/2016, Retrieved CampaignPriority from PQ_DealerSponsored and removed selecting TOP(1) campaign condition
--EXEC [dbo].[GetCrossSellCampaign_V16.4.1] 1,1,3307
--  Modified by Sanjay Soni added Top 1 on 12/05/2016
--  Modified by Chetan Thambad removed Top 1 on 13/05/2016
-- Modified by Chetan Thambad, 12/08/2016, not Selecting discontinued Version ids
-- =============================================
CREATE PROCEDURE [dbo].[GetCrossSellCampaign_V16.4.1]
	-- Add the parameters for the stored procedure here
	@CityId INT
	,@ZoneId INT
	,@TargetVersionId INT
AS
BEGIN
	SELECT CV.ID AS VersionId 
		,VRC.CampaignId
	FROM PQ_CrossSellCampaignRules CSR WITH (NOLOCK)
	INNER JOIN PQ_CrossSellCampaign CSC WITH (NOLOCK) ON CSC.Id = CSR.CrossSellCampaignId
	INNER JOIN vwRunningCampaigns VRC WITH (NOLOCK) ON VRC.CampaignId = CSC.CampaignId
	INNER JOIN CarVersions CV WITH (NOLOCK) ON CV.ID = CSR.CrossSellVersion
		AND CV.New = 1
		AND CV.IsDeleted = 0
	INNER JOIN CarModels CM WITH (NOLOCK) ON CM.Id = CV.CarModelId
		AND CM.New = 1
	INNER JOIN CarMakes CMK WITH (NOLOCK) ON CMK.ID = CM.CarMakeId
	WHERE CSR.TargetVersion = @TargetVersionId
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
							) -- zone check only for mumbai and newDelhi
						)
					OR CSR.CityId = - 1
					)
				) --City wise or Pan State
			)
END
