IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AddHouseCrossSellRule]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AddHouseCrossSellRule]
GO

	
  

-- =============================================
-- Author:		Vicky Lund
-- Create date: 18/05/2016
-- EXEC [AddHouseCrossSellRule]
-- =============================================
CREATE PROCEDURE AddHouseCrossSellRule @CampaignId INT
	,@TargetCar INT
	,@FeaturedCar INT
	,@StateId INT
	,@CityId INT
	,@ZoneId INT
	,@AddedBy INT
AS
BEGIN
	DECLARE @FeaturedAdRuleId INT

	INSERT INTO FeaturedAdRules (
		FeaturedAdId
		,StateId
		,CityId
		,ZoneId
		,TargetVersion
		,FeaturedVersion
		,AddedOn
		,AddedBy
		)
	VALUES (
		@CampaignId
		,CASE 
			WHEN @StateId = 0
				THEN (
						SELECT StateId
						FROM Cities C WITH (NOLOCK)
						WHERE C.ID = @CityId
						)
			ELSE @StateId
			END
		,@CityId
		,@ZoneId
		,@TargetCar
		,@FeaturedCar
		,GETDATE()
		,@AddedBy
		)

	SET @FeaturedAdRuleId = SCOPE_IDENTITY()

	INSERT INTO FeaturedAdRulesLogs (
		FeaturedAdRuleId
		,FeaturedAdId
		,StateId
		,CityId
		,ZoneId
		,TargetVersion
		,FeaturedVersion
		,AddedOn
		,AddedBy
		,Remarks
		)
	SELECT FAR.Id
		,FAR.FeaturedAdId
		,FAR.StateId
		,FAR.CityId
		,FAR.ZoneId
		,FAR.TargetVersion
		,FAR.FeaturedVersion
		,FAR.AddedOn
		,FAR.AddedBy
		,'Row Inserted'
	FROM FeaturedAdRules FAR WITH (NOLOCK)
	WHERE FAR.Id = @FeaturedAdRuleId
END

   
