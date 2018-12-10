IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[UCAlert].[SetUsedCarCustomersDailyAlertsProfile_Test]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [UCAlert].[SetUsedCarCustomersDailyAlertsProfile_Test]
GO

	-- =============================================
-- Author:		Ashish G. Kamble
-- Create date: 18/1/2012
-- Description:	This procedure will execute another procedure UCAlert.SetUsedCarCustomersProfiles to insert data in the UCAlert.DailyAlerts table
--				on the basis of which customer is active and have alert frequency Daily
-- =============================================
CREATE PROCEDURE [UCAlert].[SetUsedCarCustomersDailyAlertsProfile_Test] 
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	Declare @rowcnt bigint, @UsedCarAlertId bigint, @LoopId bigint
	
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
	  
	  select @UsedCarAlertId = t.UsedCarAlertId 
	  from #tempDailyAlerts as t
	  where t.id=@LoopId
	  
	  set @LoopId=@LoopId+1
	  set @rowcnt=@rowcnt-1
	  --print 'exec  UCAlert.SetUsedCarCustomersProfiles  '+ cast(@UsedCarAlertId as varchar(20))
	  exec  UCAlert.UsedCarCustomersProfilesDailyAlert_Test @UsedCarAlertId
	end
	
	--select * from #tempDailyAlerts
	
	drop table #tempDailyAlerts
  
	
END
