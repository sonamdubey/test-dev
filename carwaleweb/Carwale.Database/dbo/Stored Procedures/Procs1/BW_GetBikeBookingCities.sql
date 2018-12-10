IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_GetBikeBookingCities]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_GetBikeBookingCities]
GO

	-- =============================================
-- Author:		Ashish Kamble
-- Create date: 10 May 2015
-- Description:	Proc to get the bike booking cities
-- Modified By : Ashish G. Kamble 0n 19 jun 2015
-- Modified : Old query replaced by the new query adding model id as nullable parameter
-- Modified by Manish on 06-07-2015 optimized (added with(nolock)in cities and added application id condition)
-- Modified by Manish on 09-07-2015 Use status field in place of IsDealerActive
-- Modified By : Ashish G. Kamble on 10 Feb 2016. 
-- Summary : New flag added in the query .C.IsDeleted = 0
-- =============================================
CREATE PROCEDURE [dbo].[BW_GetBikeBookingCities]		
	@BikeModelId INT = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT DISTINCT C.ID AS CityId, 
	                C.Name AS City, 
					c.IsPopular 
	from Cities C WITH (NOLOCK)
	JOIN BW_NewBikeDealerShowroomPrices AS P WITH(NOLOCK) ON P.CityId = C.ID
	JOIN Dealers AS D WITH(NOLOCK) ON D.ID = P.DealerId AND D.ApplicationId=2
	JOIN BikeVersions AS V WITH(NOLOCK) ON V.ID = P.BikeVersionId
	JOIN BikeModels AS MO WITH(NOLOCK) ON MO.ID = V.BikeModelId
	WHERE D.[Status] = 0 
	 AND (@BikeModelId IS NULL OR MO.ID = @BikeModelId) AND C.IsDeleted = 0
	ORDER BY C.IsPopular DESC, C.Name ASC
END