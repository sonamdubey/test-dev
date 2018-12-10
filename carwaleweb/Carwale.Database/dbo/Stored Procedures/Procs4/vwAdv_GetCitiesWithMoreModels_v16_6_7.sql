IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[vwAdv_GetCitiesWithMoreModels_v16_6_7]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[vwAdv_GetCitiesWithMoreModels_v16_6_7]
GO

	
-- =============================================
-- Author:		Akansha Srivastava
-- Create date: 01.06.2016
-- Description:	Gets all the cities which have atleast 6 distinct model
-- Modified By: Mukul Bansal
-- =============================================
CREATE PROCEDURE [dbo].[vwAdv_GetCitiesWithMoreModels_v16_6_7]
@minCount INT   -- added a new parameter
	
AS
BEGIN
	SELECT CityId,CityName, COUNT(DISTINCT modelid) AS count 
	FROM vwLiveDeals With (nolock)
	GROUP BY CityId,CityName
	HAVING COUNT(DISTINCT modelid) >= @minCount
END

