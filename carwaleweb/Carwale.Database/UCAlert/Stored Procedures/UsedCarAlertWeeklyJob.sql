IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[UCAlert].[UsedCarAlertWeeklyJob]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [UCAlert].[UsedCarAlertWeeklyJob]
GO

	-- =============================================
-- Author:		Avishkar
-- Create date: 12-6-2012
-- Description:	SP for Weekly Job
-- Modified by Manish on 18-08-2014 added used car email alert sp for new design.
-- =============================================
CREATE PROCEDURE [UCAlert].[UsedCarAlertWeeklyJob]
AS
BEGIN
   
   --This procedure will execute and add Newly Added used Cars in last week
   
   EXEC  [UCAlert].[SetWeeklyNewlyAddedUsedCars]
   
   --This procedure will execute another procedure UCAlert.SetUsedCarCustomersProfiles 
   --to insert data in the UCAlert.WeeklyAlerts table
   --on the basis of which customer is active and have alert frequency weekly

   EXEC [UCAlert].[SetUsedCarCustomersWeeklyAlertsProfile] 

   EXEC [UCAlert].[NDUsedCarAlertWeeklyJob]  --added by Manish on 18-08-2014 for used car email alert sp for new design.
   
   --Procedure to retrieve the weekly alerts data with total no of cars found.
   
  EXEC  [UCAlert].[GetWeeklyAlertHistory]
	  

END
