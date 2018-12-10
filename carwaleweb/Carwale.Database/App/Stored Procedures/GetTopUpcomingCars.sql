IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[App].[GetTopUpcomingCars]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [App].[GetTopUpcomingCars]
GO

	
-- =================================================
-- Author:		Supriya
-- Create date: 10/01/2012
-- Description:	SP for fetching Top 8 Upcoming Cars 
-- =================================================

CREATE PROCEDURE [App].[GetTopUpcomingCars]
	
AS
BEGIN

	SET NOCOUNT ON;
	
	SELECT 
		Top 8 ECL.ID, MK.Name MakeName, Mo.Name AS ModelName, ECL.ExpectedLaunch, EstimatedPriceMin, EstimatedPriceMax, Mo.HostUrl, Mo.SmallPic,Mo.LargePic, ECL.CWConfidence, ECL.UpdatedDate
		FROM ExpectedCarLaunches ECL 
		INNER JOIN CarModels Mo ON ECL.CarModelId = Mo.ID 
		INNER JOIN CarMakes MK ON MK.ID = Mo.CarMakeId 
		WHERE ECL.IsLaunched = 0 
		ORDER BY ECL.LaunchDate 
		
END
