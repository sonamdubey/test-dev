IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[trk_audit]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[trk_audit]
GO

	CREATE procedure  trk_audit
as
select ts.mastertaskno,tm.Descr,
     ts.startdate,em.*
from trk_MasterTasks AS tm
join trk_Category as tc on tc.CategoryId=tm.CategoryId
--join OprUsers as ou on ou.Id=tu.TaskOwner
join trk_SubTasks as ts on tm.TaskNo=ts.MasterTaskNo
join trk_TaskUsers as tu on tu.TaskNo=ts.taskno
join emptp as em on em.oprid=tu.TaskOwner
where ts.startdate < em.JoinedOn


select ts.mastertaskno,tm.Descr,
     ts.ActualEndDate,em.Leftondate
from trk_MasterTasks AS tm
join trk_Category as tc on tc.CategoryId=tm.CategoryId
--join OprUsers as ou on ou.Id=tu.TaskOwner
join trk_SubTasks as ts on tm.TaskNo=ts.MasterTaskNo
join trk_TaskUsers as tu on tu.TaskNo=ts.taskno
join emptp as em on em.oprid=tu.TaskOwner
where em.Leftondate<ts.ActualEndDate 

select ts.mastertaskno,tm.Descr,
     ts.ActualEndDate,ts.StartDate
from trk_MasterTasks AS tm
join trk_Category as tc on tc.CategoryId=tm.CategoryId
--join OprUsers as ou on ou.Id=tu.TaskOwner
join trk_SubTasks as ts on tm.TaskNo=ts.MasterTaskNo
join trk_TaskUsers as tu on tu.TaskNo=ts.taskno
where ts.StartDate>ts.ActualEndDate   

select ts.mastertaskno,tm.Descr,
     ts.ActualEndDate,ts.StartDate
from trk_MasterTasks AS tm
join trk_Category as tc on tc.CategoryId=tm.CategoryId
--join OprUsers as ou on ou.Id=tu.TaskOwner
join trk_SubTasks as ts on tm.TaskNo=ts.MasterTaskNo
join trk_TaskUsers as tu on tu.TaskNo=ts.taskno
where ts.StartDate>ts.ActualEndDate 