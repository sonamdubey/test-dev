IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[UCAlert].[UsedCarAlertDailyJob]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [UCAlert].[UsedCarAlertDailyJob]
GO

	-- =============================================
-- Author:		Avishkar
-- Create date: 12-6-2012
-- Description:	SP for Daily Job
-- Modified by Manish on 14-08-2014 added used car email alert sp for new design.
-- =============================================
CREATE PROCEDURE [UCAlert].[UsedCarAlertDailyJob]
AS
BEGIN
   
   --This procedure will execute and add Newly Added used Cars 
   
   EXEC  UCAlert.SetDailyNewlyAddedUsedCars  
   
   --This procedure will execute another procedure UCAlert.UsedCarCustomersProfilesDailyAlert
   --to insert data in the UCAlert.DailyAlerts table
   --on the basis of which customer is active and have alert frequency Daily

   EXEC UCAlert.SetUsedCarCustomersDailyAlertsProfile 
  
   EXEC [UCAlert].[NDUsedCarAlertDailyJob]  --added by Manish on 14-08-2014 for used car email alert sp for new design.
   --Procedure to retrieve the Daily alerts data with total no of cars found.
   
   EXEC [UCAlert].[GetDailyAlertsHistory]
	  

END




