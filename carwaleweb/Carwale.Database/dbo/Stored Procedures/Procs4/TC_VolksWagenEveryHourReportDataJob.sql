IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_VolksWagenEveryHourReportDataJob]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_VolksWagenEveryHourReportDataJob]
GO

	
-- =============================================
-- Author:		Manish
-- Create date: 19-06-2013
-- Description: This SP will truncate the table VolksWagenHourlyReportData and insert the last three hour latest data
-- Modified By Manish on 01-07-2013  for replicating the logic of dashboard reports
-- Modified By Manish on 03-07-2013  for adding column dealerid
-- Modified By Manish on 24-07-2013  for considering only current month data
-- Modified By Manish on 19-08-2013  for optimization of query by changing the data fetching process
-- Modified By: Manish Chourasiya on 13-11-2014 changed to add dealer id condition now this houly reports will use for Shaman Used Cars.
-- =============================================
CREATE PROCEDURE [dbo].[TC_VolksWagenEveryHourReportDataJob]
AS 
BEGIN 

DECLARE @TmpTable TABLE ([Organization] [varchar](100),
	                     [TotalUsers] [int],
	                     [TotalNoOfLoginsToday] [int],
	                     [TotalNoOfLogins] [int],
	                     [TotalInquiryAddedToday] [int],
	                     [TotalInquiryAdded] [int] ,
	                     [TotalUniqueInquiryAdded] [int],
						 [TotalFollowupsToday] [int] ,
						 [TotalFollowups] [int] ,
						 [TotalUniqueFollowups] [int],
						 [TotalPendingFollowups] [int],
	                     [TotalUniquePendingFollowups] [int],
						 [TotalTodaysBookedCar] [int] ,
						 [TotalBookedCar] [int] ,
						 [TDCompleted] [int],
						 [TDCancelled] [int],
						 [TDBooked] [int],
						 [DealerId] [int])
 
 INSERT INTO @TmpTable
										(Organization,
										TotalUsers,
										TotalNoOfLoginsToday,
										TotalNoOfLogins,
										TotalInquiryAddedToday,
										TotalInquiryAdded,
										TotalUniqueInquiryAdded,
										TotalFollowupsToday,
										TotalFollowups,
										TotalUniqueFollowups,
										TotalPendingFollowups,
										TotalUniquePendingFollowups,
										TotalTodaysBookedCar,
										TotalBookedCar,
										TDCompleted,
										TDCancelled,
										TDBooked,
										DealerId)
		
SELECT  Organization,
		TotalUsers,
		[Total No of Logins Today],
		[Total No of Logins],
		TotalInquiryAddedToday,
		TotalInquiryAdded,
		TotalUniqueInquiryAdded,
		TotalFollowupsToday,
		TotalFollowups,
		TotalUniqueFollowups,
		TotalPendingFollowups,
		TotalUniquePendingFollowups,
		isnull(TotalTodaysBookedCar,0) TotalTodaysBookedCar,
		isnull(TotalBookedCar,0) TotalBookedCar ,
		isnull(TDCompleted,0) TDCompleted,
		isnull(TDCancelled,0) TDCancelled,
		isnull(TDBooked,0) TDBooked,
		a.[DealerId]  DealerId 
		 FROM 
(SELECT LBS.DealerId AS [DealerId],
        LBS.Organization,
COUNT(DISTINCT (CASE WHEN (LBS.ScheduledOn<GETDATE() ) THEN LBS.TC_LeadId END ))  TotalPendingFollowups,
COUNT(DISTINCT (CASE WHEN (LBS.ScheduledOn<GETDATE() ) THEN LBS.TC_LeadId END )) TotalUniquePendingFollowups,
COUNT(DISTINCT (CASE WHEN CONVERT(DATE,LBS.BookingDate)=CONVERT(DATE,GETDATE()) AND LBS.TC_LeadDispositionID=4 and LBS.BookingStatus=32  THEN LBS.TC_NewCarInquiriesId  END))  TotalTodaysBookedCar,
COUNT(DISTINCT (CASE WHEN MONTH(LBS.BookingDate)= MONTH(GETDATE()) AND LBS.TC_LeadDispositionID=4 and LBS.BookingStatus=32 THEN LBS.TC_NewCarInquiriesId END ))  TotalBookedCar
FROM TC_LeadBasedSummary AS LBS WITH (NOLOCK)
WHERE LBS.DealerId=50     --- Condition added by Manish on 13-11-2014 
GROUP BY LBS.DealerId,LBS.Organization) A
LEFT JOIN
(SELECT  D.ID AS [DealerId],
        COUNT(DISTINCT TCU.ID) [TotalUsers],
		COUNT(DISTINCT(CASE WHEN CONVERT(DATE,TCUL.LoggedTime)=CONVERT(DATE,GETDATE()) THEN TCUL.Id END)) [Total No of Logins Today], 
		COUNT(DISTINCT(CASE WHEN MONTH(TCUL.LoggedTime) = MONTH(GETDATE()) THEN TCUL.Id END)) [Total No of Logins]
FROM DEALERS            AS D WITH (NOLOCK)
--INNER JOIN TC_BrandZone AS TBZ WITH (NOLOCK) ON D.TC_BrandZoneId = TBZ.TC_BrandZoneId AND TBZ.MakeId = 20 AND  D.IsDealerActive= 1
JOIN TC_Users           AS TCU WITH (NOLOCK)  ON TCU.BranchId=D.ID   
LEFT JOIN TC_UsersLog   AS TCUL WITH (NOLOCK) ON TCUL.UserId=TCU.Id
WHERE D.ID=50  AND D.IsDealerActive=1       --- Condition added by Manish on 13-11-2014 
GROUP BY D.ID)  B   ON A.[DealerId]=B.[DealerId]
LEFT JOIN
(SELECT D.ID  AS [DealerId],
COUNT(DISTINCT (CASE WHEN TCC.IsActionTaken=1 AND CONVERT(DATE,TCC.ActionTakenOn)=CONVERT(DATE,GETDATE()) THEN TCC.TC_CallsId END ) ) TotalFollowupsToday,
COUNT(DISTINCT (CASE WHEN TCC.IsActionTaken=1 AND MONTH(TCC.ActionTakenOn)=MONTH(GETDATE()) THEN TCC.TC_CallsId END ) ) TotalFollowups,
COUNT(DISTINCT (CASE WHEN TCC.IsActionTaken=1 AND MONTH(TCC.ActionTakenOn)=MONTH(GETDATE()) THEN TCC.TC_LeadId END ) ) TotalUniqueFollowups,
COUNT(DISTINCT (CASE WHEN actionComments='Inquiry Added' AND CONVERT(DATE,TCC.CreatedOn)=CONVERT(DATE,GETDATE())  THEN TCC.TC_CallsId END ) ) TotalInquiryAddedToday,
COUNT(DISTINCT (CASE WHEN actionComments='Inquiry Added' AND MONTH(TCC.CreatedOn)=MONTH(GETDATE()) THEN TCC.TC_CallsId END ) ) TotalInquiryAdded,
COUNT(DISTINCT (CASE WHEN actionComments='Inquiry Added' AND MONTH(TCC.CreatedOn)=MONTH(GETDATE()) THEN TCC.TC_LeadId END ) ) TotalUniqueInquiryAdded
FROM DEALERS                AS D WITH (NOLOCK)
--INNER JOIN TC_BrandZone     AS TBZ WITH (NOLOCK) ON D.TC_BrandZoneId = TBZ.TC_BrandZoneId AND TBZ.MakeId = 20 AND  D.IsDealerActive= 1
LEFT  JOIN TC_InquiriesLead AS TCIL WITH (NOLOCK) ON D.ID=TCIL.BranchId
LEFT  JOIN TC_Calls         AS TCC WITH (NOLOCK) ON TCIL.TC_LeadId=TCC.TC_LeadId
WHERE D.ID=50  AND D.IsDealerActive=1    --- Condition added by Manish on 13-11-2014 
GROUP BY D.ID) C ON A.[DealerId]=C.[DealerId]
LEFT JOIN
(SELECT TCL.BRANCHID  AS [DealerId],
				COUNT(DISTINCT(CASE WHEN TCDL.TC_LeadDispositionId=28 AND MONTH(TCDL.EventCreatedOn) = MONTH(GETDATE()) THEN TCDL.TC_LeadId END)) AS TDCompleted,
				COUNT(DISTINCT(CASE WHEN TCDL.TC_LeadDispositionId=27 AND MONTH(TCDL.EventCreatedOn) = MONTH(GETDATE())  THEN TCDL.TC_LeadId END)) AS TDCancelled,
				COUNT(DISTINCT(CASE WHEN (TCDL.TC_LeadDispositionId=39 OR TCDL.TC_LeadDispositionId=29) AND MONTH(TCDL.EventCreatedOn) = MONTH(GETDATE())  THEN TCDL.TC_LeadId END)) AS TDBooked
		 FROM  TC_DispositionLog AS TCDL  WITH (NOLOCK)
		 JOIN  TC_Lead AS TCL  WITH (NOLOCK) ON TCL.TC_LeadId=TCDL.TC_LeadId
		 JOIN  TC_LeadDisposition AS TCLD   WITH (NOLOCK) ON TCLD.TC_LeadDispositionId=TCDL.TC_LeadDispositionId
		 WHERE TCL.BranchId=50    --- Condition added by Manish on 13-11-2014 
		 group by TCL.BRANCHID)  D ON A.[DealerId]=D.[DealerId]
 


               TRUNCATE TABLE TC_VolksWagenHourlyReportData;


               INSERT INTO TC_VolksWagenHourlyReportData
										(Organization,
										TotalUsers,
										TotalNoOfLoginsToday,
										TotalNoOfLogins,
										TotalInquiryAddedToday,
										TotalInquiryAdded,
										TotalUniqueInquiryAdded,
										TotalFollowupsToday,
										TotalFollowups,
										TotalUniqueFollowups,
										TotalPendingFollowups,
										TotalUniquePendingFollowups,
										TotalTodaysBookedCar,
										TotalBookedCar,
										TDCompleted,
										TDCancelled,
										TDBooked,
										DealerID)
							SELECT 
							            Organization,
										TotalUsers,
										TotalNoOfLoginsToday,
										TotalNoOfLogins,
										TotalInquiryAddedToday,
										TotalInquiryAdded,
										TotalUniqueInquiryAdded,
										TotalFollowupsToday,
										TotalFollowups,
										TotalUniqueFollowups,
										TotalPendingFollowups,
										TotalUniquePendingFollowups,
										TotalTodaysBookedCar,
										TotalBookedCar,
										TDCompleted,
										TDCancelled,
										TDBooked,
										DealerId
						    
						    FROM        @TmpTable
                             

END
/****** Object:  StoredProcedure [dbo].[TC_VolksWagenDailyUsageReport]    Script Date: 11/13/2014 11:23:50 AM ******/
SET ANSI_NULLS ON
