IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[SC].[SetDailyNewTrackers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [SC].[SetDailyNewTrackers]
GO

	-- =============================================
-- Author:		Chriag Perla
-- Create date: 2015-09-24
-- Description:	Daily Tracker for New Tracker Types

---------------UPDATE LOG:-----------------------
--Modified by Chirag Perla: Removed MERGE Logic from 'Dealers with Actions', 'Dealers with Autobiz Usage' and 'Unique Dealers with Actions and Autobiz Usage' to bring in data for all dates (we would use each week's max date value in Scorecard)
--Modified by Chirag Perla on 2015-11-16:  Added a new tracker 'Active New Car Dealer Count'

-- =============================================
CREATE PROCEDURE [SC].[SetDailyNewTrackers] 
	
AS
BEGIN
	
	SET NOCOUNT ON;

DECLARE @TrackerDate DATE=CONVERT(varchar(11),GETDATE()-1,113) --- TrackerDate(Yesterday's date)

DECLARE @TrackerDateWS DATE=DATEADD(DAY, 2 - DATEPART(WEEKDAY, GETDATE()-2), CAST(GETDATE()-2 AS DATE)); ----WeekStart Date

DECLARE @TrackerDateWE DATE=DATEADD(DAY, 8 - DATEPART(WEEKDAY, GETDATE()-2), CAST(GETDATE()-2 AS DATE)); ----WeekEnd Date

    
	
--------------------------PAID DEALERS--------------------------

INSERT INTO SC.DailyTracker(TrackerType,TrackerCount,TrackerDate)

SELECT 'Paid UCDealers' AS TrackerType,
	   COUNT(ID) as TrackerCount,
	   @TrackerDate AS TrackerDate 
FROM ConsumerCreditPoints WITH (NOLOCK)
where ConsumerType=1
and PackageType Not in 
(
select IP.Id
from InquiryPointCategory as IP WITH (NOLOCK)
JOIN packages as P WITH (NOLOCK) on P.InqPtCategoryId=IP.Id
where IsDealer=1
and Amount=0
)
AND CONVERT(varchar(11),ExpiryDate,113) >= @TrackerDate



------------------------WARRANTY ACTIVATIONS---------------------

INSERT INTO SC.DailyTracker(TrackerType,TrackerCount,TrackerDate)


SELECT 'Warranty Activations' AS TrackerType,
	   Count(distinct PolicyNo) as TrackerCount,
	   @TrackerDate AS TrackerDate 
FROM AbSure_ActivatedWarranty AA  WITH (NOLOCK)
INNER JOIN AbSure_CarDetails ACD  WITH (NOLOCK)  ON AA.AbSure_CarDetailsId = ACD.Id
INNER JOIN AbSure_Trans_Debits ATD WITH (NOLOCK) ON ATD.CarId = ACD.Id
LEFT JOIN Cities C WITH (NOLOCK)                 ON ACD.OwnerCityId = C.ID
LEFT JOIN States AS S WITH (NOLOCK)              ON C.StateId = S.ID
LEFT JOIN Areas A  WITH (NOLOCK)                 ON A.ID = ACD.OwnerAreaId
LEFT JOIN TC_Users TU WITH (NOLOCK)              ON aa.UserId = tu.Id
LEFT JOIN AbSure_WarrantyTypes AWT WITH (NOLOCK) ON AA.WarrantyTypeId = AWT.AbSure_WarrantyTypesId
LEFT JOIN dealers D  WITH (NOLOCK)               ON aa.DealerId = d.ID
LEFT JOIN TC_Dealertype AS TD  WITH (NOLOCK)     ON D.TC_DealerTypeId = TD.TC_DealerTypeId AND TD.TC_DealerTypeId IN (1, 3, 5)
WHERE D.Id NOT IN (4271,3838,12150,11894)
and Cast(AA.EntryDate as Date) = @TrackerDate;




-------------------------Dealers with Actions----------------------------



INSERT INTO SC.DailyTracker(TrackerType,TrackerCount,TrackerDate)

Select 'Dealers with Actions' as TrackerType,
 Count(Distinct z.BranchId) as TrackerCount,
 @TrackerDate as TrackerDate
FROM (
select BranchId, Count(Id) as CNT 
from TC_ActionsPlatformTrack  WITH (NOLOCK)
where Cast(ActionDate as Date) between @TrackerDateWS and @TrackerDate
and TC_ActionId in (15,14,13,16,17,19,10,12,11,24)
group by BranchId
)z




------------------- Dealers with Autobiz Usage -----------------------

INSERT INTO SC.DailyTracker(TrackerType,TrackerCount,TrackerDate)

SELECT 'Dealers with Autobiz Usage' as TrackerType,
	   Count(Distinct y.BranchId) as TrackerCount,
	   @TrackerDate as TrackerDate
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





---------------------------Unique Dealers with Actions and Autobiz Usage---------------------------


INSERT INTO SC.DailyTracker(TrackerType,TrackerCount,TrackerDate)


SELECT 'Unique Dealers with Actions and Autobiz Usage' as TrackerType,
	   Count(Distinct y.BranchId) as TrackerCount,
	   @TrackerDate as TrackerDate
FROM(

SELECT Distinct z.BranchId as BranchId
FROM(
select BranchId, Count(Id) as CNT 
from TC_ActionsPlatformTrack  WITH (NOLOCK)
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





END



---------------------------Active New Car Dealer Outlets---------------------------



INSERT INTO SC.DailyTracker(TrackerType,TrackerCount,TrackerDate)



Select 'Active New Car Dealer Outlets' as TrackerType,
	   sum(outletcount) as TrackerCount,
	   @TrackerDate as TrackerDate
from DCRM_OutletCountLog as DC
join Dealers as D on D.Id=DC.DealerId
where dealertype in (2,3)
and D.IsDealerActive=1
and d.applicationid=1
and CONVERT(varchar(11),CaptureDate,113) = @TrackerDate