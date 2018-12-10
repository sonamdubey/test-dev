IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[UCAlert].[SetUsedCarCustomersWeeklyAlertsProfile]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [UCAlert].[SetUsedCarCustomersWeeklyAlertsProfile]
GO

	-- =============================================
-- Author:		Ashish G. Kamble
-- Create date: 18/1/2012
-- Description:	This procedure will execute another procedure UCAlert.SetUsedCarCustomersProfiles to insert data in the UCAlert.WeeklyAlerts table
--				on the basis of which customer is active and have alert frequency weekly
-- Modified By: Manish on 26-08-2015 added try and exception block
-- =============================================
CREATE PROCEDURE [UCAlert].[SetUsedCarCustomersWeeklyAlertsProfile] 
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	Declare @rowcnt bigint, @UsedCarAlertId bigint, @LoopId bigint
	
	Truncate table UCAlert.WeeklyAlerts
	
	CREATE TABLE #tempWeeklyAlerts
	(
	   id bigint identity(1,1),
	   UsedCarAlertId bigint
	)
	
	insert into #tempWeeklyAlerts
	select UsedCarAlert_Id from UCAlert.UserCarAlerts as UCA 
	where UCA.IsActive=1 and UCA.AlertFrequency=1
	
	set @rowcnt=@@rowcount
	set @LoopId=1
	
	while (@rowcnt>0)
	begin
	   BEGIN TRY
	  select @UsedCarAlertId = t.UsedCarAlertId 
	  from #tempWeeklyAlerts as t
	  where t.id=@LoopId
	  
	  set @LoopId=@LoopId+1
	  set @rowcnt=@rowcnt-1
	  --print 'exec  UCAlert.UsedCarCustomersProfilesWeeklyAlert  '+ cast(@UsedCarAlertId as varchar(20))
	  exec  UCAlert.UsedCarCustomersProfilesWeeklyAlert @UsedCarAlertId
	  END TRY
	  BEGIN CATCH
	   INSERT INTO CarWaleWebSiteExceptions (
			                                ModuleName,
											SPName,
											ErrorMsg,
											TableName,
											FailedId,
											CreatedOn)
						            VALUES ('Used Car Weekly alert email',
									        'UCAlert.SetUsedCarCustomersWeeklyAlertsProfile',
											 ERROR_MESSAGE(),
											 'UCAlert.UserCarAlerts',
											 @UsedCarAlertId,
											 GETDATE()
                                            )
  

	  END CATCH

	end
	
	--select * from #tempWeeklyAlerts
	
	drop table #tempWeeklyAlerts
  
	
END

