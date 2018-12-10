IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetHouseCrossSell]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetHouseCrossSell]
GO

	-- =============================================
-- Author:		Vicky Lund
-- Create date: 04/04/2016
-- EXEC [GetHouseCrossSell] 1,-1,1998
--  Modified by Sanjay Soni added Top 1 on 12/05/2016
--  Modified by Chetan Thambad removed Top 1 on 13/05/2016
-- =============================================
CREATE PROCEDURE [dbo].[GetHouseCrossSell] @CityId INT
	,@ZoneId INT
	,@TargetVersionId INT
AS
BEGIN
	SELECT FAR.FeaturedVersion 
	FROM FeaturedAd FA WITH (NOLOCK)
	INNER JOIN FeaturedAdRules FAR WITH (NOLOCK) ON FA.Id = FAR.FeaturedAdId
		AND FAR.TargetVersion = @TargetVersionId
		AND FA.IsActive = 1
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
