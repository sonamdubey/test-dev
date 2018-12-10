IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[vwAdv_GetCitiesWithMoreModels]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[vwAdv_GetCitiesWithMoreModels]
GO

	-- =============================================
-- Author:		Akansha Srivastava
-- Create date: 01.06.2016
-- Description:	Gets all the cities which have atleast 6 distinct model
-- =============================================
CREATE PROCEDURE [dbo].[vwAdv_GetCitiesWithMoreModels]
	
AS
BEGIN
	SELECT CityId,CityName, COUNT(DISTINCT modelid) AS count 
	FROM vwLiveDeals With (nolock)
	GROUP BY CityId,CityName
	HAVING COUNT(DISTINCT modelid) > 2
END
