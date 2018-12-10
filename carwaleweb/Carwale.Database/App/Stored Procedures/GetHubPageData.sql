IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[App].[GetHubPageData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [App].[GetHubPageData]
GO

	
-- ====================================================================
-- Author:		Supriya
-- Create date: 10/05/2012
-- Description:	SP for fetching Top Make,UpcomingCars,RoadTest and News
-- ====================================================================

CREATE PROCEDURE [App].[GetHubPageData] 

AS
BEGIN

	SET NOCOUNT ON;

  	Execute app.GetTopMakes
  	--Procedure to fetch top 12 Makes 
  	
  	Execute app.GetTopUpcomingCars
  	--Procedure to fetch top 8 Upcoming Cars
  	
  	Execute App.GetTopRoadTests
  	--Procedure to fetch top 7 RoadTests
  	
  	Execute App.GetTopNews
  	--Procedure to fetch top 10 News
END
