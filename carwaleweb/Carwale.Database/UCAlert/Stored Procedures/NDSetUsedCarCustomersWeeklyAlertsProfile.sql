IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[UCAlert].[NDSetUsedCarCustomersWeeklyAlertsProfile]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [UCAlert].[NDSetUsedCarCustomersWeeklyAlertsProfile]
GO

	-- =============================================
-- Author:		Manish Chourasiya
-- Create date: 23-06-2014
-- Description:	This procedure will execute another procedure UCAlert.NDUsedCarCustomersProfilesWeeklyAlert to insert data in the UCAlert.WeeklyAlerts table
--				on the basis of which customer is active and have alert frequency Weekly
--Modified By Manish Chourasiya on 17-09-2014 changed alert frequency from 1 to 3 since in front end there is no weekly alert.
-- Modified By: Manish on 26-08-2015 added try and exception block
-- =============================================
CREATE PROCEDURE [UCAlert].[NDSetUsedCarCustomersWeeklyAlertsProfile] 
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	Declare @rowcnt bigint, @UsedCarAlertId bigint, @LoopId bigint
	
	--	DELETE FROM ucalert.DailyAlerts WHERE CreatedOn <> CONVERT(DATE,GETDATE()); --- First execute old used car alert then new design used car alert.
	
	CREATE TABLE #tempWeeklyAlerts
	(
	   id bigint identity(1,1),
	   UsedCarAlertId bigint
	)
	
	insert into #tempWeeklyAlerts
	select UsedCarAlert_Id from UCALERT.NDUsedCarAlertCustomerList as UCA 
	where UCA.IsActive=1 and UCA.AlertFrequency=3
	
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
	  exec  UCAlert.NDUsedCarCustomersProfilesWeeklyAlert @UsedCarAlertId
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
									        'UCAlert.NDSetUsedCarCustomersWeeklyAlertsProfile',
											 ERROR_MESSAGE(),
											 'UCALERT.NDUsedCarAlertCustomerList',
											 @UsedCarAlertId,
											 GETDATE()
                                            )


	  END CATCH 
	end
	
	--select * from #tempWeeklyAlerts
	
	drop table #tempWeeklyAlerts
  	
END
