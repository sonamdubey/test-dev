IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetPaidCrossSellRules]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetPaidCrossSellRules]
GO

	

-- =============================================
-- Author:		Vicky Lund
-- Create date: 10/05/2016
-- EXEC [GetPaidCrossSellRules] 296
-- =============================================
CREATE PROCEDURE GetPaidCrossSellRules @CampaignId INT
AS
BEGIN
	SELECT PCSCR.Id
		,CASE 
			WHEN PCSCR.StateId = - 1
				THEN 'All States'
			ELSE S.NAME
			END [State]
		,CASE 
			WHEN PCSCR.CityId = - 1
				THEN 'All Cities'
			ELSE C.NAME
			END [City]
		,CASE 
			WHEN PCSCR.ZoneId = 0
				THEN '---'
			ELSE CZ.ZoneName
			END Zone
		,CV1.NAME FeaturedVersion
		,CM1.NAME FeaturedModel
		,CMa1.NAME FeaturedMake
		,CV2.NAME TargetVersion
		,CM2.NAME TargetModel
		,CMa2.NAME TargetMake
		,PCSCR.AddedOn
		,ISNULL(OU.UserName, '---') AddedBy
	FROM PQ_CrossSellCampaignRules PCSCR WITH (NOLOCK)
	INNER JOIN PQ_CrossSellCampaign PCSC WITH (NOLOCK) ON PCSC.Id = PCSCR.CrossSellCampaignId
	LEFT OUTER JOIN States S WITH (NOLOCK) ON PCSCR.StateId = S.ID
	LEFT OUTER JOIN Cities C WITH (NOLOCK) ON PCSCR.CityId = C.ID
	LEFT OUTER JOIN CityZones CZ WITH (NOLOCK) ON PCSCR.ZoneId = CZ.Id
	INNER JOIN CarVersions CV1 WITH (NOLOCK) ON PCSCR.CrossSellVersion = CV1.ID
	INNER JOIN CarModels CM1 WITH (NOLOCK) ON CV1.CarModelId = CM1.ID
	INNER JOIN CarMakes CMa1 WITH (NOLOCK) ON CM1.CarMakeId = CMa1.ID
	INNER JOIN CarVersions CV2 WITH (NOLOCK) ON PCSCR.TargetVersion = CV2.ID
	INNER JOIN CarModels CM2 WITH (NOLOCK) ON CV2.CarModelId = CM2.ID
	INNER JOIN CarMakes CMa2 WITH (NOLOCK) ON CM2.CarMakeId = CMa2.ID
	LEFT OUTER JOIN OprUsers OU WITH (NOLOCK) ON PCSCR.UpdatedBy = OU.Id
	WHERE PCSC.CampaignId = @CampaignId
END

