IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_InsertCampaignRule]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_InsertCampaignRule]
GO

	
-- =============================================
-- Created on	:	Sumit Kate on 18 Mar 2016
-- Description	:	Insert BW_PQ_CampaignRule Table
-- Parameters	:
--	@CampaignId					:	Campaign Id
--	@DealerId					:	Dealer Id
--	@CityId						:	City Id
--	@ModelId					:	Model Id(Nullable)
--		If Model Id is null then Rule is added for the all the models available with the dealer.
--	@StateId					:	State Id
--	@UserId						:	User Id
--	@MakeId						:	Make Id
-- =============================================
CREATE PROCEDURE [dbo].[BW_InsertCampaignRule] @CampaignId INT
	,@CityId INT
	,@DealerId INT	
	,@StateId INT
	,@MakeId INT
	,@UserID INT
	,@ModelId VARCHAR(500) = NULL
AS
BEGIN
	SELECT @DealerId = DealerId
	FROM BW_PQ_DealerCampaigns WITH (NOLOCK)
	WHERE Id = @CampaignId

	IF @ModelId IS NOT NULL
	BEGIN
		INSERT INTO BW_PQ_CampaignRules (
			CampaignId
			,CityId
			,MakeId
			,ModelId
			,IsActive
			,EnteredBy
			)
		SELECT 
			@CampaignId
			,@CityId
			,@MakeId
			,models.ListMember
			,1
			,@UserID
		FROM fnSplitCSV(@ModelId) models
	END
	ELSE
	BEGIN
		INSERT INTO BW_PQ_CampaignRules (
			ModelId
			,CampaignId
			,CityId
			,MakeId
			,IsActive
			,EnteredBy
			)
		SELECT DISTINCT BMO.ID AS ModelID
			,@CampaignId
			,@CityId
			,@MakeId
			,1
			,@UserID
		FROM BikeVersions BV WITH (NOLOCK)
		INNER JOIN BikeModels BMO WITH (NOLOCK) ON BMO.ID = BV.BikeModelId
			AND BMO.IsDeleted = 0
			AND BMO.New = 1
		INNER JOIN BikeMakes M WITH (NOLOCK) ON M.ID = BMO.BikeMakeId
			AND M.ID = @MakeId
		ORDER BY BMO.ID
	END
END
