IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[UCAlert].[NDUsedCarAlertDailyJob]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [UCAlert].[NDUsedCarAlertDailyJob]
GO

	-- =============================================
-- Author:		Manish Chourasiya
-- Create date: 18-06-2014
-- Description:	For used car alert Daily Mail 
-- =============================================
CREATE PROCEDURE [UCAlert].[NDUsedCarAlertDailyJob]
AS
BEGIN
   
   --This procedure will execute and add Newly Added used Cars 
   
 --  EXEC  UCAlert.SetDailyNewlyAddedUsedCars  
   
   --This procedure will execute another procedure UCAlert.UsedCarCustomersProfilesDailyAlert
   --to insert data in the UCAlert.DailyAlerts table
   --on the basis of which customer is active and have alert frequency Daily

   EXEC UCAlert.NDSetUsedCarCustomersDailyAlertsProfile 
  
   
   --Procedure to retrieve the Daily alerts data with total no of cars found.
   
  -- EXEC [UCAlert].[GetDailyAlertsHistory]
	  

END
