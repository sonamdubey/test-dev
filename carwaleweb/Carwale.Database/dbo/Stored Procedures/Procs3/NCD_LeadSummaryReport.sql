IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCD_LeadSummaryReport]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCD_LeadSummaryReport]
GO

	-- =============================================
-- Author:		<Author,,Umesh Ojha>
-- Create date: <19/09/2011,,>
-- Description:	<This SP Gives the report for lead summary for given date range>
-- Modified by: Chetan Kane on 11th July one more parameter added for the fetching result depending on the AssignedExecutive
-- =============================================
CREATE PROCEDURE  [dbo].[NCD_LeadSummaryReport]
	-- Add the parameters for the stored procedure here
(
 @dealerid int,
 @startdate datetime,
 @enddate datetime,
 @AssignedExecutive INT = -1
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
declare @TtlCwAcc int
declare @TtlDwAcc int
declare @TtlAcc int
declare @TtlAccWithNoActionTaken int
    
Create table #tempAccepted
(
   InquirySource	varchar(13),
   Statusid	int,
   AcceptedCount int
)

Create table #tempRejected
(
   InquirySource	varchar(13),
   Statusid	int,
   RejectedCount int
)

Create table #tempTotalAcc
(
   StatusName varchar(70),
   cwAccepted int,
   dwAccepted int,
   TotalCt int
)

Create table #tempTotalRej
(
   StatusName varchar(70),
   cwRejected int,
   dwRejected int,
   TotalCt int
)

Create table #tempTotalNotActionTaken
(  
   NotActionTakenCount int
)

declare @endDateTime datetime

set @endDateTime=(select DATEADD (mi , 1439,@enddate ))

insert into #tempAccepted
select   (case NI.InquirySource  
          when 1 then 'DealerWebSite'
          when 2 then 'CarWale' 
       end ) as InquirySource,  
       --sum((case NI.IsAccepted  when  0 then 1 else 0 end)) as Rejected,
       NS.Id,
        sum((case NI.IsAccepted  when  1 then 1 else 0 end)) as Accepted       
from NCD_Inquiries AS NI  
   join NCD_LeadStatus as NS on NI.LeadStatusId=NS.Id  
where NI.DealerId=@dealerid and NI.IsActionTaken=1
and NI.EntryDate between @startdate and @endDateTime AND (@AssignedExecutive = -1 OR NI.AssignedExecutive = @AssignedExecutive)
group by NI.InquirySource,NS.Id

insert into #tempRejected
select   (case NI.InquirySource  
          when 1 then 'DealerWebSite'
          when 2 then 'CarWale' 
       end ) as InquirySource,  
       --sum((case NI.IsAccepted  when  0 then 1 else 0 end)) as Rejected,
       NS.Id,
        sum((case NI.IsAccepted  when  0 then 1 else 0 end)) as Rejected       
from NCD_Inquiries AS NI  
   join NCD_LeadStatus as NS on NI.LeadStatusId=NS.Id  
where NI.DealerId=@dealerid and NI.IsActionTaken=1
and NI.EntryDate between @startdate and @endDateTime AND (@AssignedExecutive = -1 OR NI.AssignedExecutive = @AssignedExecutive)
group by NI.InquirySource,NS.Id

--select InquirySource,StatusName,COUNT(*) from #tempAccepted
--GROUP BY InquirySource,StatusName


insert into #tempTotalAcc 

select NL.StatusName,isnull(cwTbl.cwAc,0) as cwAccepted,isnull(dwTbl.dwAc,0) as dwAccepted,
isnull(cwTbl.cwAc,0)+isnull(dwTbl.dwAc,0) as TotalCt from NCD_LeadStatus NL
left join
(select statusid as cwSid,acceptedcount as cwAc from #tempaccepted
where InquirySource ='CarWale')cwTbl on NL.Id=cwTbl.cwSid
 left join 

(select statusid as dwSid,acceptedcount as dwAc from #tempaccepted
where InquirySource ='DealerWebSite')dwTbl on NL.id=dwTbl.dwSid
--where NL.LeadStatusType=1

set @TtlCwAcc = (select sum(cwAccepted)  from #tempTotalAcc)
set @TtlDwAcc = (select sum(dwAccepted) from #tempTotalAcc)
set @TtlAcc = (select SUM(TotalCt) from #tempTotalAcc)

insert into #tempTotalAcc (StatusName,cwAccepted,dwAccepted,TotalCt)
values ('Total',@TtlCwAcc,@TtlDwAcc,@TtlAcc)

insert into #tempTotalRej

select NL.StatusName,isnull(cwTbl.cwRc,0) as cwRejected,isnull(dwTbl.dwRc,0) as dwRejected,
isnull(cwTbl.cwRc,0)+isnull(dwTbl.dwRc,0) as TotalCt from NCD_LeadStatus NL
left join
(select statusid as cwSid,RejectedCount as cwRc from #tempRejected
where InquirySource ='CarWale')cwTbl on NL.Id=cwTbl.cwSid
 left join 

(select statusid as dwSid,RejectedCount as dwRc from #tempRejected
where InquirySource ='DealerWebSite')dwTbl on NL.id=dwTbl.dwSid

where NL.LeadStatusType=0

set @TtlCwAcc = (select sum(cwRejected)  from #tempTotalRej)
set @TtlDwAcc = (select sum(dwRejected) from #tempTotalRej)
set @TtlAcc = (select SUM(TotalCt) from #tempTotalRej)

insert into #tempTotalRej (StatusName,cwRejected,dwRejected,TotalCt)
values ('Total',@TtlCwAcc,@TtlDwAcc,@TtlAcc)

insert into #tempTotalNotActionTaken
select count(NI.Id) as NoTActionTaken       
from NCD_Inquiries AS NI    
where NI.DealerId=@dealerid and NI.IsActionTaken=0
and NI.EntryDate between @startdate and @endDateTime AND (@AssignedExecutive = -1 OR NI.AssignedExecutive = @AssignedExecutive)



set @TtlCwAcc = (select R.cwRejected+A.cwAccepted from #tempTotalRej R join #tempTotalAcc A on R.StatusName=A.StatusName where A.StatusName='Total' and R.StatusName='Total' )
set @TtlDwAcc = (select R.dwRejected+A.dwAccepted from #tempTotalRej R join #tempTotalAcc A on R.StatusName=A.StatusName where A.StatusName='Total' and R.StatusName='Total')
set @TtlAcc = (select R.TotalCt+A.TotalCt from #tempTotalRej R join #tempTotalAcc A on R.StatusName=A.StatusName where A.StatusName='Total' and R.StatusName='Total')
set @TtlAccWithNoActionTaken = (select NA.NotActionTakenCount from #tempTotalNotActionTaken NA)
--insert into #tempTotalRej (StatusName,cwRejected,dwRejected,TotalCt)
--values ('Total Leads',@TtlCwAcc,@TtlDwAcc,@TtlAcc)

select *  from #tempTotalAcc
select *  from #tempTotalRej
Select 'Total Leads' as 'StatusName',@TtlCwAcc as 'cwTtlLeads',@TtlDwAcc as 'dwTtlLeads',@TtlAcc as 'TtlLeads',@TtlAccWithNoActionTaken as 'TtlLeadsNoActionTaken'  

drop table #tempAccepted
drop table #tempRejected
drop table #tempTotalAcc
drop table #tempTotalRej

END
