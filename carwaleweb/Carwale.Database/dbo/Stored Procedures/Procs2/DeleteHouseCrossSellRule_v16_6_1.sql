IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DeleteHouseCrossSellRule_v16_6_1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DeleteHouseCrossSellRule_v16_6_1]
GO

	-- =============================================
-- Author:		Vicky Lund
-- Create date: 10/05/2016
-- EXEC [DeleteHouseCrossSellRule] '1,2'
-- =============================================
CREATE PROCEDURE [dbo].[DeleteHouseCrossSellRule_v16_6_1] @RuleIds VARCHAR(MAX)
	,@UpdatedBy INT
AS
BEGIN
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
		,GETDATE()
		,@UpdatedBy
		,'Row Deleted'
	FROM FeaturedAdRules FAR WITH (NOLOCK)
	WHERE FAR.Id IN (
			SELECT ListMember
			FROM dbo.fnSplitCSVMAx(@RuleIds)
			)

	DELETE
	FROM FeaturedAdRules
	WHERE Id IN (
			SELECT ListMember
			FROM dbo.fnSplitCSVMAx(@RuleIds)
			)
END

