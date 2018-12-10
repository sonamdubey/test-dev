IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCD_BuyingReportChart]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCD_BuyingReportChart]
GO

	-- =============================================
-- Author:		<Author,,Umesh Ojha>
-- Create date: <19/09/2011,,>
-- Description:	<This SP Gives the report for Expected Buying Time for car for given date range>
-- =============================================
CREATE PROCEDURE [dbo].[NCD_BuyingReportChart] 
	-- Add the parameters for the stored procedure here
(
 @dealerid int,
 @startdate datetime,
 @enddate datetime
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	declare @TtlOnewk int
	declare @TtlTwowk int
	declare @TtlOnemnt int
	declare @TtlTwomnts int
	
	Create table #tempBuying
	(
	   Car varchar(100),
	   Onewk int,
	   Twowk int,
	   Onemnt int,
	   Twomnts int
	)
	

    -- Insert statements for procedure here
    insert into #tempBuying
    
	select vw.Make+' '+vw.Model+' '+vw.Version as CAR,tbl1.wk,tbl1.twowk as '2wk',
	tbl1.onemont as '1mnt',tbl1.twomnt as '2mnt'
	from (select versionid,sum(case NI.BuyPlan when '7' then 1 else 0 end) as wk,
	sum(case BuyPlan when '14' then 1 else 0 end) as twowk,
	sum(case BuyPlan when '30' then 1 else 0 end) as onemont,
	sum(case BuyPlan when '60' then 1 else 0 end) as twomnt from NCD_Inquiries NI
	where BuyPlan is not null and DealerId=@dealerid and EntryDate between @startdate and @enddate
	group by VersionId)tbl1
	join vwMMV as vw on vw.VersionId=tbl1.VersionId 
	
	set @TtlOnewk=(select sum(Onewk) from #tempBuying)
	set @TtlTwowk=(select sum(Twowk) from #tempBuying)
	set @TtlOnemnt=(select sum(Onemnt) from #tempBuying)
	set @TtlTwomnts=(select sum(Twomnts) from #tempBuying)
	
	insert into #tempBuying (Car,Onewk,Twowk,Onemnt,Twomnts)
	values ('Total',@TtlOnewk,@TtlTwowk,@TtlOnemnt,@TtlTwomnts)
	
	select * from #tempBuying
	
	drop table #tempBuying
		
END

