IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[reports].[GetVerification]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [reports].[GetVerification]
GO

	-- =============================================
-- Author:		Avishkar Meshram
-- Create date: Ocy 09 2011
-- Description:	Get Daily Verification Report
-- [reports].[GetVerification] '2011-10-01'
-- =============================================
CREATE PROCEDURE [reports].[GetVerification]
@date date
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
select OU.UserName,
sum(case when CL.LeadStatusId=2 then 1 else 0 end) as "Verified",
sum(case when CL.LeadStatusId=3 then 1 else 0 end) as "Fake",
sum(case when CL.LeadStatusId=4 then 1 else 0 end) as "NotInterested",
sum(case when CC.Disptype=1 then 1 else 0 end) as "Connected",
sum(case when CC.CallType=1 then 1 else 0 end) as "FreshCalls",
sum(case when CC.CallType=2 then 1 else 0 end) as "PendingCalls",
sum(case when CC.CallType=2 then 1 when CallType=1 then 1 else 0 end) as "TotalCalls"
from CRM_Calls as CC
  join CRM_leads as CL on CL.ID=CC.LeadId
join OprUsers as OU on CC.CallerId=OU.Id
where OU.IsActive=1
and convert(varchar(8),CC.ActionTakenOn,112)=convert(varchar(8),@date,112)
and CallType in (1,2)
--and LeadStageId=1
group by OU.UserName
order by OU.UserName

END

