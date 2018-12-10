IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[UCAlert].[NDUsedCarAlertWeeklyJob]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [UCAlert].[NDUsedCarAlertWeeklyJob]
GO

	-- =============================================
-- Author:		Manish Chourasiya
-- Create date: 23-06-2014
-- Description:	For used car alert Daily Mail 
-- =============================================
CREATE PROCEDURE [UCAlert].[NDUsedCarAlertWeeklyJob]
AS
BEGIN
   
   --This procedure will execute and add Newly Added used Cars in last week
   
  -- EXEC  [UCAlert].[SetWeeklyNewlyAddedUsedCars]
   
   --This procedure will execute another procedure UCAlert.SetUsedCarCustomersProfiles 
   --to insert data in the UCAlert.WeeklyAlerts table
   --on the basis of which customer is active and have alert frequency weekly

   EXEC [UCAlert].[NDSetUsedCarCustomersWeeklyAlertsProfile] 
   
   --Procedure to retrieve the weekly alerts data with total no of cars found.
   
 -- EXEC  [UCAlert].[GetWeeklyAlertHistory]
	  

END