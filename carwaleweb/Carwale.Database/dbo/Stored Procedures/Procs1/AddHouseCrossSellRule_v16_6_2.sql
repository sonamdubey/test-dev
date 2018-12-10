IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AddHouseCrossSellRule_v16_6_2]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AddHouseCrossSellRule_v16_6_2]
GO

	
-- =============================================
-- Author:		Vicky Lund
-- Create date: 18/05/2016
-- EXEC [AddHouseCrossSellRule]
-- Modified by: Vicky Lund, 06/07/2016, Changed AddedOn -> UpdatedOn, AddedBy -> UpdatedBy
-- Modified by: Sachin Bharti, 06/07/2016, Added new paramete @ImpressionTracker
-- =============================================
CREATE PROCEDURE [dbo].[AddHouseCrossSellRule_v16_6_2] @CampaignId INT
	,@TargetCar INT
	,@FeaturedCar INT
	,@StateId INT
	,@CityId INT
	,@ZoneId INT
	,@AddedBy INT
	,@ImpressionTracker VARCHAR(250) =NULL
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
		,UpdatedOn
		,UpdatedBy
		,CarImpressionTracker
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
		,@ImpressionTracker
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
		,UpdatedOn
		,UpdatedBy
		,Remarks
		)
	SELECT FAR.Id
		,FAR.FeaturedAdId
		,FAR.StateId
		,FAR.CityId
		,FAR.ZoneId
		,FAR.TargetVersion
		,FAR.FeaturedVersion
		,FAR.UpdatedOn
		,FAR.UpdatedBy
		,'Row Inserted'
	FROM FeaturedAdRules FAR WITH (NOLOCK)
	WHERE FAR.Id = @FeaturedAdRuleId
END

