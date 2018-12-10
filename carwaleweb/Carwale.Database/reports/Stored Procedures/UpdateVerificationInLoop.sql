IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[reports].[UpdateVerificationInLoop]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [reports].[UpdateVerificationInLoop]
GO

	-- =============================================
-- Author:		Avishkar Meshram
-- Create date: Ocy 09 2011
-- Description:	Update Verification In Loop stats
-- =============================================
CREATE PROCEDURE [reports].[UpdateVerificationInLoop]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
Create Table #NCS_Pool
(
   FLCGroupName varchar	(250),
   NCS_PoolCount int,
   NCS_PoolResources smallint
 )
 
 Declare @currentTime datetime
 set @currentTime=getdate()
 
 if (convert(varchar, @currentTime, 114)<'11:00:00:000')
 begin 
 insert into #NCS_Pool
 select FLC.Name,
       COUNT(CL.ID) as LeadInPool,
       COUNT(distinct OU.Id) as Resources
from CRM_leads as CL
 join CRM_Calls as CC on CL.ID=CC.LeadId
 join CRM_ADM_FLCGroups as FLC on CL.GroupId=FLC.Id
 join OprUsers as OU on CC.CallerId=OU.Id
where CL.LeadStageId=1 
and CL.Owner=-1
and OU.IsActive=1
and convert(varchar(8),CL.CreatedOn,112)>=convert(varchar(8),GETDATE()-1,112)
and ((convert(varchar, CL.CreatedOn, 114) between '19:00:00:999' and '24:00:00:000')
    or (convert(varchar, CL.CreatedOn, 114) between '00:00:00:001' and '11:00:00:000'))
group by FLC.Name

insert into NCS_PoolHistory
select @currentTime,'11:00 AM',FLCGroupName,NCS_PoolCount,NCS_PoolResources
from #NCS_Pool

end

 if (convert(varchar, @currentTime, 114) between '11:00:00:001' and '12:00:00:000') 
 begin
 insert into #NCS_Pool
 select FLC.Name,
       COUNT(CL.ID) as LeadInPool,
       COUNT(distinct OU.Id) as Resources
from CRM_leads as CL
 join CRM_Calls as CC on CL.ID=CC.LeadId
 join CRM_ADM_FLCGroups as FLC on CL.GroupId=FLC.Id
 join OprUsers as OU on CC.CallerId=OU.Id
where CL.LeadStageId=1 
and CL.Owner=-1
and OU.IsActive=1
and convert(varchar(8),CL.CreatedOn,112)=convert(varchar(8),GETDATE()-1,112)
and convert(varchar, CL.CreatedOn, 114) between '11:00:00:001' and '12:00:00:000'
group by FLC.Name

insert into NCS_PoolHistory
select @currentTime,'12:00 AM',FLCGroupName,NCS_PoolCount,NCS_PoolResources
from #NCS_Pool

end

if (convert(varchar, @currentTime, 114) between '12:00:00:001' and '13:00:00:000') 
 begin
 insert into #NCS_Pool
 select FLC.Name,
       COUNT(CL.ID) as LeadInPool,
       COUNT(distinct OU.Id) as Resources
from CRM_leads as CL
 join CRM_Calls as CC on CL.ID=CC.LeadId
 join CRM_ADM_FLCGroups as FLC on CL.GroupId=FLC.Id
 join OprUsers as OU on CC.CallerId=OU.Id
where CL.LeadStageId=1 
and CL.Owner=-1
and OU.IsActive=1
and convert(varchar(8),CL.CreatedOn,112)=convert(varchar(8),GETDATE()-1,112)
and convert(varchar, CL.CreatedOn, 114) between  '12:00:00:001' and '13:00:00:000'
group by FLC.Name

insert into NCS_PoolHistory
select @currentTime,'01:00 PM',FLCGroupName,NCS_PoolCount,NCS_PoolResources
from #NCS_Pool

end

if (convert(varchar, @currentTime, 114) between '13:00:00:001' and '14:00:00:000') 
 begin
 insert into #NCS_Pool
 select FLC.Name,
       COUNT(CL.ID) as LeadInPool,
       COUNT(distinct OU.Id) as Resources
from CRM_leads as CL
 join CRM_Calls as CC on CL.ID=CC.LeadId
 join CRM_ADM_FLCGroups as FLC on CL.GroupId=FLC.Id
 join OprUsers as OU on CC.CallerId=OU.Id
where CL.LeadStageId=1 
and CL.Owner=-1
and OU.IsActive=1
and convert(varchar(8),CL.CreatedOn,112)=convert(varchar(8),GETDATE()-1,112)
and convert(varchar, CL.CreatedOn, 114) between  '13:00:00:001' and '14:00:00:000'
group by FLC.Name

insert into NCS_PoolHistory
select @currentTime,'02:00 PM',FLCGroupName,NCS_PoolCount,NCS_PoolResources
from #NCS_Pool

end

if (convert(varchar, @currentTime, 114) between '14:00:00:001' and '15:00:00:000') 
 begin
 insert into #NCS_Pool
 select FLC.Name,
       COUNT(CL.ID) as LeadInPool,
       COUNT(distinct OU.Id) as Resources
from CRM_leads as CL
 join CRM_Calls as CC on CL.ID=CC.LeadId
 join CRM_ADM_FLCGroups as FLC on CL.GroupId=FLC.Id
 join OprUsers as OU on CC.CallerId=OU.Id
where CL.LeadStageId=1 
and CL.Owner=-1
and OU.IsActive=1
and convert(varchar(8),CL.CreatedOn,112)=convert(varchar(8),GETDATE()-1,112)
and convert(varchar, CL.CreatedOn, 114) between  '14:00:00:001' and '15:00:00:000'
group by FLC.Name

insert into NCS_PoolHistory
select @currentTime,'03:00 PM',FLCGroupName,NCS_PoolCount,NCS_PoolResources
from #NCS_Pool

end

if (convert(varchar, @currentTime, 114) between '15:00:00:001' and '16:00:00:000') 
 begin
 insert into #NCS_Pool
 select FLC.Name,
       COUNT(CL.ID) as LeadInPool,
       COUNT(distinct OU.Id) as Resources
from CRM_leads as CL
 join CRM_Calls as CC on CL.ID=CC.LeadId
 join CRM_ADM_FLCGroups as FLC on CL.GroupId=FLC.Id
 join OprUsers as OU on CC.CallerId=OU.Id
where CL.LeadStageId=1 
and CL.Owner=-1
and OU.IsActive=1
and convert(varchar(8),CL.CreatedOn,112)=convert(varchar(8),GETDATE()-1,112)
and convert(varchar, CL.CreatedOn, 114) between  '15:00:00:001' and '16:00:00:000'
group by FLC.Name

insert into NCS_PoolHistory
select @currentTime,'04:00 PM',FLCGroupName,NCS_PoolCount,NCS_PoolResources
from #NCS_Pool

end

if (convert(varchar, @currentTime, 114) between '16:00:00:001' and '17:00:00:000') 
 begin
 insert into #NCS_Pool
 select FLC.Name,
       COUNT(CL.ID) as LeadInPool,
       COUNT(distinct OU.Id) as Resources
from CRM_leads as CL
 join CRM_Calls as CC on CL.ID=CC.LeadId
 join CRM_ADM_FLCGroups as FLC on CL.GroupId=FLC.Id
 join OprUsers as OU on CC.CallerId=OU.Id
where CL.LeadStageId=1 
and CL.Owner=-1
and OU.IsActive=1
and convert(varchar(8),CL.CreatedOn,112)=convert(varchar(8),GETDATE()-1,112)
and convert(varchar, CL.CreatedOn, 114) between  '16:00:00:001' and '17:00:00:000'
group by FLC.Name

insert into NCS_PoolHistory
select @currentTime,'05:00 PM',FLCGroupName,NCS_PoolCount,NCS_PoolResources
from #NCS_Pool

end

if (convert(varchar, @currentTime, 114) between '17:00:00:001' and '18:00:00:000') 
 begin
 insert into #NCS_Pool
 select FLC.Name,
       COUNT(CL.ID) as LeadInPool,
       COUNT(distinct OU.Id) as Resources
from CRM_leads as CL
 join CRM_Calls as CC on CL.ID=CC.LeadId
 join CRM_ADM_FLCGroups as FLC on CL.GroupId=FLC.Id
 join OprUsers as OU on CC.CallerId=OU.Id
where CL.LeadStageId=1 
and CL.Owner=-1
and OU.IsActive=1
and convert(varchar(8),CL.CreatedOn,112)=convert(varchar(8),GETDATE()-1,112)
and convert(varchar, CL.CreatedOn, 114) between  '17:00:00:001' and '18:00:00:000'
group by FLC.Name

insert into NCS_PoolHistory
select @currentTime,'06:00 PM',FLCGroupName,NCS_PoolCount,NCS_PoolResources
from #NCS_Pool

END

if (convert(varchar, @currentTime, 114) between '18:00:00:001' and '19:00:00:000') 
 begin
 insert into #NCS_Pool
 select FLC.Name,
       COUNT(CL.ID) as LeadInPool,
       COUNT(distinct OU.Id) as Resources
from CRM_leads as CL
 join CRM_Calls as CC on CL.ID=CC.LeadId
 join CRM_ADM_FLCGroups as FLC on CL.GroupId=FLC.Id
 join OprUsers as OU on CC.CallerId=OU.Id
where CL.LeadStageId=1 
and CL.Owner=-1
and OU.IsActive=1
and convert(varchar(8),CL.CreatedOn,112)=convert(varchar(8),GETDATE()-1,112)
and convert(varchar, CL.CreatedOn, 114) between  '18:00:00:001' and '19:00:00:000'
group by FLC.Name

insert into NCS_PoolHistory
select @currentTime,'07:00 PM',FLCGroupName,NCS_PoolCount,NCS_PoolResources
from #NCS_Pool

end

drop table #NCS_Pool

END
