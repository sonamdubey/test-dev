IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetHouseCrossSellRules_v16_6_1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetHouseCrossSellRules_v16_6_1]
GO

	-- =============================================
-- Author:		Vicky Lund
-- Create date: 10/05/2016
-- EXEC [GetHouseCrossSellRules_v16_6_1] 28
-- =============================================
CREATE PROCEDURE [dbo].[GetHouseCrossSellRules_v16_6_1] @CampaignId INT
AS
BEGIN
	SELECT FAR.Id
		,CASE 
			WHEN FAR.StateId = - 1
				THEN 'All States'
			ELSE S.NAME
			END [State]
		,CASE 
			WHEN FAR.CityId = - 1
				THEN 'All Cities'
			ELSE C.NAME
			END [City]
		,CASE 
			WHEN FAR.ZoneId = 0
				THEN '---'
			ELSE CZ.ZoneName
			END Zone
		,CV1.NAME FeaturedVersion
		,CM1.NAME FeaturedModel
		,CMa1.NAME FeaturedMake
		,CV2.NAME TargetVersion
		,CM2.NAME TargetModel
		,CMa2.NAME TargetMake
		,FAR.UpdatedOn AS AddedOn
		,ISNULL(OU.UserName, '---') AddedBy
	FROM FeaturedAdRules FAR WITH (NOLOCK)
	INNER JOIN FeaturedAd FA WITH (NOLOCK) ON FA.Id = FAR.FeaturedAdId
	LEFT OUTER JOIN States S WITH (NOLOCK) ON FAR.StateId = S.ID
	LEFT OUTER JOIN Cities C WITH (NOLOCK) ON FAR.CityId = C.ID
	LEFT OUTER JOIN CityZones CZ WITH (NOLOCK) ON FAR.ZoneId = CZ.Id
	INNER JOIN CarVersions CV1 WITH (NOLOCK) ON FAR.FeaturedVersion = CV1.ID
	INNER JOIN CarModels CM1 WITH (NOLOCK) ON CV1.CarModelId = CM1.ID
	INNER JOIN CarMakes CMa1 WITH (NOLOCK) ON CM1.CarMakeId = CMa1.ID
	INNER JOIN CarVersions CV2 WITH (NOLOCK) ON FAR.TargetVersion = CV2.ID
	INNER JOIN CarModels CM2 WITH (NOLOCK) ON CV2.CarModelId = CM2.ID
	INNER JOIN CarMakes CMa2 WITH (NOLOCK) ON CM2.CarMakeId = CMa2.ID
	LEFT OUTER JOIN OprUsers OU WITH (NOLOCK) ON FAR.UpdatedBy = OU.Id
	WHERE FAR.FeaturedAdId = @CampaignId
END

