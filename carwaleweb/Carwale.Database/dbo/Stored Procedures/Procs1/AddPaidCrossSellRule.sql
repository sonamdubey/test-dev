IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AddPaidCrossSellRule]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AddPaidCrossSellRule]
GO

	


-- =============================================
-- Author:		Vicky Lund
-- Create date: 19/05/2016
-- EXEC [AddPaidCrossSellRule]
-- =============================================
CREATE PROCEDURE AddPaidCrossSellRule @CampaignId INT
	,@TargetCar INT
	,@FeaturedCar INT
	,@StateId INT
	,@CityId INT
	,@ZoneId INT
	,@AddedBy INT
AS
BEGIN
	DECLARE @CrossSellCampaignId INT

	SELECT TOP 1 @CrossSellCampaignId = PCSC.Id
	FROM PQ_CrossSellCampaign PCSC WITH (NOLOCK)
	WHERE PCSC.CampaignId = @CampaignId
		AND PCSC.IsActive = 1

	IF (@CrossSellCampaignId IS NULL)
	BEGIN
		INSERT INTO PQ_CrossSellCampaign (
			IsActive
			,UpdatedOn
			,UpdatedBy
			,CampaignId
			,CampaignName
			,StartDate
			,EndDate
			,AddedOn
			)
		VALUES (
			1
			,GETDATE()
			,@AddedBy
			,@CampaignId
			,NULL
			,NULL
			,NULL
			,GETDATE()
			)

		SET @CrossSellCampaignId = SCOPE_IDENTITY()
	END

	INSERT INTO PQ_CrossSellCampaignRules (
		CrossSellCampaignId
		,CityId
		,TargetVersion
		,CrossSellVersion
		,ZoneId
		,TemplateId
		,StateId
		,UpdatedOn
		,UpdatedBy
		,AddedOn
		)
	VALUES (
		@CrossSellCampaignId
		,@CityId
		,@TargetCar
		,@FeaturedCar
		,@ZoneId
		,- 1
		,CASE 
			WHEN @StateId = 0
				THEN (
						SELECT StateId
						FROM Cities C WITH (NOLOCK)
						WHERE C.ID = @CityId
						)
			ELSE @StateId
			END
		,GETDATE()
		,@AddedBy
		,GETDATE()
		)
END


