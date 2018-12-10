IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[UCAlert].[NDSetUsedCarCustomersDailyAlertsProfile]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [UCAlert].[NDSetUsedCarCustomersDailyAlertsProfile]
GO

	-- =============================================
-- Author:		Manish Chourasiya
-- Create date: 18-06-2014
-- Description:	This procedure will execute another procedure UCAlert.NDSetUsedCarCustomersProfiles to insert data in the UCAlert.DailyAlerts table
--				on the basis of which customer is active and have alert frequency Daily
-- Modified By: Manish on 19-08-2015 added try and exception block
-- =============================================
CREATE PROCEDURE [UCAlert].[NDSetUsedCarCustomersDailyAlertsProfile] 
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	Declare @rowcnt bigint, @UsedCarAlertId bigint, @LoopId bigint
	
	--DELETE FROM ucalert.DailyAlerts WHERE CreatedOn <> CONVERT(DATE,GETDATE()); --- First execute old used car alert then new design used car alert.


	CREATE TABLE #tempDailyAlerts
	(
	   id bigint identity(1,1),
	   UsedCarAlertId bigint
	)
	
	insert into #tempDailyAlerts
	select UsedCarAlert_Id from UCALERT.NDUsedCarAlertCustomerList as UCA 
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
	  exec  UCAlert.NDUsedCarCustomersProfilesDailyAlert @UsedCarAlertId
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
									        'UCAlert.NDSetUsedCarCustomersDailyAlertsProfile',
											 ERROR_MESSAGE(),
											 'UCALERT.NDUsedCarAlertCustomerList',
											 @UsedCarAlertId,
											 GETDATE()
                                            )


	  END CATCH 


	end
	
	--select * from #tempDailyAlerts
	
	drop table #tempDailyAlerts
  
	
END



