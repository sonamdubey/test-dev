IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[UCAlert].[SetUsedCarCustomersDailyAlertsProfile]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [UCAlert].[SetUsedCarCustomersDailyAlertsProfile]
GO

	-- =============================================
-- Author:		Ashish G. Kamble
-- Create date: 18/1/2012
-- Description:	This procedure will execute another procedure UCAlert.SetUsedCarCustomersProfiles to insert data in the UCAlert.DailyAlerts table
--				on the basis of which customer is active and have alert frequency Daily

--Modified by: Manish on 07-03-2014 added Truncate ucalert.DailyAlerts statement since every new data will be inserted and no need to take old data.
-- Modified By: Manish on 19-08-2015 added try and exception block
-- =============================================
CREATE PROCEDURE [UCAlert].[SetUsedCarCustomersDailyAlertsProfile] 
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	


	Declare @rowcnt bigint, @UsedCarAlertId bigint, @LoopId bigint
	
	TRUNCATE TABLE ucalert.DailyAlerts  -----Truncate statement added by manish on 07-03-2014 since every new data will be inserted and no need to take old data.

	CREATE TABLE #tempDailyAlerts
	(
	   id bigint identity(1,1),
	   UsedCarAlertId bigint
	)
	
	insert into #tempDailyAlerts
	select UsedCarAlert_Id from UCAlert.UserCarAlerts as UCA 
	where UCA.IsActive=1 and UCA.AlertFrequency=2
	
	set @rowcnt=@@rowcount
	set @LoopId=1
	
	while (@rowcnt>0)
	begin
	  BEGIN TRY
		  select @UsedCarAlertId = t.UsedCarAlertId 
		  from #tempDailyAlerts as t
		  where t.id=@LoopId
	  
		  set @LoopId=@LoopId+1
		  set @rowcnt=@rowcnt-1
		  --print 'exec  UCAlert.SetUsedCarCustomersProfiles  '+ cast(@UsedCarAlertId as varchar(20))
		  exec  UCAlert.UsedCarCustomersProfilesDailyAlert @UsedCarAlertId
	  END TRY
	  BEGIN CATCH
	   INSERT INTO CarWaleWebSiteExceptions (
			                                ModuleName,
											SPName,
											ErrorMsg,
											TableName,
											FailedId,
											CreatedOn)
						            VALUES ('Used Car Daily alert email',
									        'UCAlert.SetUsedCarCustomersDailyAlertsProfile',
											 ERROR_MESSAGE(),
											 'UCAlert.UserCarAlerts',
											 @UsedCarAlertId,
											 GETDATE()
                                            )
  

	  END CATCH

	end
	
	--select * from #tempDailyAlerts
	
	drop table #tempDailyAlerts
  
	
END

