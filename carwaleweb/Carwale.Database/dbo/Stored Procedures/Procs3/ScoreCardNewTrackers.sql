IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ScoreCardNewTrackers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ScoreCardNewTrackers]
GO

	
CREATE PROCEDURE [dbo].[ScoreCardNewTrackers]
as
begin

DECLARE @TrackerDate DATE=CONVERT(varchar(11),GETDATE()-1,113) --- TrackerDate(Yesterday's date)

DECLARE @TrackerDateWS DATE=DATEADD(DAY, 2 - DATEPART(WEEKDAY, GETDATE()), CAST(GETDATE() AS DATE)); ----WeekStart Date

DECLARE @TrackerDateWE DATE=DATEADD(DAY, 8 - DATEPART(WEEKDAY, GETDATE()), CAST(GETDATE() AS DATE)); ----WeekEnd Date




-------------------UPDATE 1---------------------


Update [Carwale_com].[sc].[DailyTracker]
Set TrackerCount = 
(

Select 
		Count(Distinct z.BranchId) as TrackerCount

FROM (
select BranchId, Count(Id) as CNT from
[dbo].TC_ActionsPlatformTrack
where Cast(ActionDate as Date) between @TrackerDateWS and @TrackerDate
and TC_ActionId in (15,14,13,16,17,19,10,12,11,24)
group by BranchId
)z

)
where TrackerType='Dealers with Actions'
and TrackerDate='2015-10-04'



----------------------UPDATE 2------------------


Update [Carwale_com].[sc].[DailyTracker]
Set TrackerCount = 
(


SELECT 
	   Count(Distinct y.BranchId) as TrackerCount
	   
FROM
(

Select Distinct 
(case  when NoOfUniqueCalls*100/ (CASE WHEN NoOfUniqueLeads=0 THEN 1
ELSE NoOfUniqueLeads END )>=30 then BranchId  end) as BranchId
from

(
SELECT ISNULL(B.BranchId,A.BranchId) BranchId,
  ISNULL(A.NoOfUniqueCalls,0) NoOfUniqueCalls,
  ISNULL(B.NoOfUniqueLeads,0) NoOfUniqueLeads
FROM
(
(
SELECT TL.BranchId BranchId,
  COUNT(DISTINCT TC.TC_LeadId) NoOfUniqueCalls
FROM TC_Calls AS TC WITH (NOLOCK)
JOIN TC_InquiriesLead AS TL WITH (NOLOCK) ON TC.TC_LeadId=TL.TC_LeadId
WHERE TC.IsActionTaken=1
AND TL.TC_LeadInquiryTypeId=1
AND CONVERT(varchar(11),TC.ActionTakenOn ,113) between @TrackerDateWS and @TrackerDate
GROUP BY TL.BranchId
) A

   FULL OUTER JOIN

(
SELECT TL.BranchId BranchId,
  COUNT(DISTINCT TL.TC_LeadId) NoOfUniqueLeads
FROM  TC_Lead AS TL WITH (NOLOCK)
JOIN TC_InquiriesLead AS TCIL WITH (NOLOCK) ON TL.TC_LeadId=TCIL.TC_LeadId
where TCIL.TC_LeadInquiryTypeId=1
AND CONVERT(varchar(11),TL.LeadCreationDate ,113) between @TrackerDateWS and @TrackerDate
GROUP BY TL.BranchId
) B ON   A.BranchId=B.BranchId
)
) C
WHERE  NoOfUniqueCalls*100/(CASE WHEN NoOfUniqueLeads=0 THEN 1 ELSE
NoOfUniqueLeads END )>=30
)y



)
where TrackerType='Dealers with Autobiz Usage'
and TrackerDate='2015-10-04'






-------------------UPDATE 3--------------------------------



Update [Carwale_com].[sc].[DailyTracker]
Set TrackerCount = 
(



SELECT 
	   Count(Distinct y.BranchId) as TrackerCount
	  
FROM(

SELECT Distinct z.BranchId as BranchId
FROM(
select BranchId, Count(Id) as CNT from
[dbo].TC_ActionsPlatformTrack
where Cast(ActionDate as Date) between @TrackerDateWS and @TrackerDate
and TC_ActionId in (15,14,13,16,17,19,10,12,11,24)
group by BranchId
)z


UNION

Select Distinct 
(case  when NoOfUniqueCalls*100/ (CASE WHEN NoOfUniqueLeads=0 THEN 1
ELSE NoOfUniqueLeads END )>=30 then BranchId  end) as BranchId
from

(
SELECT ISNULL(B.BranchId,A.BranchId) BranchId,
  ISNULL(A.NoOfUniqueCalls,0) NoOfUniqueCalls,
  ISNULL(B.NoOfUniqueLeads,0) NoOfUniqueLeads
FROM
(
(
SELECT TL.BranchId BranchId,
  COUNT(DISTINCT TC.TC_LeadId) NoOfUniqueCalls
FROM TC_Calls AS TC WITH (NOLOCK)
JOIN TC_InquiriesLead AS TL WITH (NOLOCK) ON TC.TC_LeadId=TL.TC_LeadId
WHERE TC.IsActionTaken=1
AND TL.TC_LeadInquiryTypeId=1
AND CONVERT(varchar(11),TC.ActionTakenOn ,113) between @TrackerDateWS and @TrackerDate
GROUP BY TL.BranchId
) A

   FULL OUTER JOIN

(
SELECT TL.BranchId BranchId,
  COUNT(DISTINCT TL.TC_LeadId) NoOfUniqueLeads
FROM  TC_Lead AS TL WITH (NOLOCK)
JOIN TC_InquiriesLead AS TCIL WITH (NOLOCK) ON TL.TC_LeadId=TCIL.TC_LeadId
where TCIL.TC_LeadInquiryTypeId=1
AND CONVERT(varchar(11),TL.LeadCreationDate ,113) between @TrackerDateWS and @TrackerDate
GROUP BY TL.BranchId
) B ON   A.BranchId=B.BranchId
)
) C
WHERE  NoOfUniqueCalls*100/(CASE WHEN NoOfUniqueLeads=0 THEN 1 ELSE
NoOfUniqueLeads END )>=30
)y




)
where TrackerType='Unique Dealers with Actions and Autobiz Usage'
and TrackerDate='2015-10-04'

end