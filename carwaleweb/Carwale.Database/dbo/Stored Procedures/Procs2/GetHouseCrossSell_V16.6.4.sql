IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetHouseCrossSell_V16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetHouseCrossSell_V16]
GO

	-- =============================================
-- Author:		Vicky Lund
-- Create date: 04/04/2016
-- EXEC [GetHouseCrossSell] 1,1,4548
-- Modified by Sanjay Soni added Top 1 on 12/05/2016
-- Modified by Chetan Thambad removed Top 1 on 13/05/2016
-- Modified by Vicky Lund, 04/07/2016, Selected distinct Version ids
-- Modified by Chetan Thambad, 12/08/2016, Selected new Version ids
-- =============================================
CREATE PROCEDURE [dbo].[GetHouseCrossSell_V16.6.4] @CityId INT
	,@ZoneId INT
	,@TargetVersionId INT
AS
BEGIN
	SELECT DISTINCT FAR.FeaturedVersion
	FROM FeaturedAd FA WITH (NOLOCK)
	INNER JOIN FeaturedAdRules FAR WITH (NOLOCK) ON FA.Id = FAR.FeaturedAdId
	INNER JOIN campaigncategory CC WITH (NOLOCK) ON FA.CampaignCategoryId = CC.Id
	INNER JOIN CarVersions CV WITH (NOLOCK) ON FAR.FeaturedVersion = CV.ID 
		AND FAR.TargetVersion = @TargetVersionId
		AND CC.Id = 11
		AND FA.IsActive = 1
		AND CV.New = 1
		AND CV.IsDeleted = 0
		AND (
			(FAR.StateId = - 1) --Pan India
			OR (
				FAR.StateId = (
					SELECT StateId
					FROM Cities WITH (NOLOCK)
					WHERE Id = @CityId
					)
				AND (
					(
						FAR.CityId = @CityId
						AND (
							ISNULL(FAR.ZoneId, 0) = ISNULL(@ZoneId, 0)
							OR @CityId NOT IN (1, 10)
							) -- zone check only for mumbai and newDelhi
						)
					OR FAR.CityId = - 1
					)
				) --City wise or Pan State
			)
END
