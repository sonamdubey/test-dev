IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_ShamanDailyUsageReport]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_ShamanDailyUsageReport]
GO

	
-- =============================================
-- Author:		Manish
-- Create date: 19-06-2013
-- Description: Report for VW AutoBiz panel Usage 
-- Modified by Manish on 28-06-2013 for adding Test Drive columns
-- =============================================
CREATE PROCEDURE [dbo].[TC_ShamanDailyUsageReport]	
AS
BEGIN
--Subject :VW AutoBiz panel Usage 19-06-2013 3 PM

SELECT a.Organization,
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
		isnull(TotalTodaysBookedCar,0),
		isnull(TotalBookedCar,0) 
		--isnull(TDCompleted,0) ,
		--isnull(TDCancelled,0),
		--isnull(TDBooked,0)  
		 FROM 
		(select D.Organization,D.ID,
		COUNT(DISTINCT TCU.ID) [TotalUsers],
		COUNT(DISTINCT(CASE WHEN CONVERT(DATE,TCUL.LoggedTime)=CONVERT(DATE,GETDATE()) THEN TCUL.Id END)) [Total No of Logins Today], 
		COUNT(DISTINCT TCUL.Id) [Total No of Logins], 
		COUNT(DISTINCT (CASE WHEN actionComments='Inquiry Added' AND CONVERT(DATE,TCC.CreatedOn)=CONVERT(DATE,GETDATE())  THEN TCC.TC_CallsId END ) ) TotalInquiryAddedToday,
		COUNT(DISTINCT (CASE WHEN actionComments='Inquiry Added' THEN TCC.TC_CallsId END ) ) TotalInquiryAdded,
		COUNT(DISTINCT TCIL.TC_LeadId) TotalUniqueInquiryAdded,
		COUNT(DISTINCT (CASE WHEN TCC.IsActionTaken=1 AND CONVERT(DATE,TCC.ActionTakenOn)=CONVERT(DATE,GETDATE()) THEN TCC.TC_CallsId END ) ) TotalFollowupsToday,
		COUNT(DISTINCT (CASE WHEN TCC.IsActionTaken=1 THEN TCC.TC_CallsId END ) ) TotalFollowups,
		COUNT(DISTINCT (CASE WHEN TCC.IsActionTaken=1 THEN TCC.TC_LeadId END ) ) TotalUniqueFollowups,
		 COUNT(DISTINCT (CASE WHEN (TCAC.ScheduledOn<GETDATE() ) THEN TCAC.TC_CallsId END ))  TotalPendingFollowups,
		 COUNT(DISTINCT (CASE WHEN (TCAC.ScheduledOn<GETDATE() ) THEN TCAC.TC_LeadId END )) TotalUniquePendingFollowups
		FROM DEALERS as D WITH (NOLOCK) 
		join TC_Users AS TCU WITH (NOLOCK)  ON TCU.BranchId=D.ID   --AND TCU.IsActive=1
		LEFT JOIN  TC_Calls  AS TCC WITH (NOLOCK) ON  TC_UsersId=TCU.Id
		LEFT JOIN TC_ActiveCalls AS TCAC WITH (NOLOCK) ON TCC.TC_CallsId=TCAC.TC_CallsId
		LEFT JOIN TC_InquiriesLead AS TCIL WITH (NOLOCK) ON TCC.TC_LeadId=TCIL.TC_LeadId
		LEFT JOIN TC_UsersLog AS TCUL WITH (NOLOCK) ON TCUL.UserId=TCU.Id
		WHERE D.Status = 0  AND D.ID IN(50)
		GROUP BY D.Organization,D.ID
		--order by COUNT(DISTINCT TCUL.Id) desc
		) A
		LEFT JOIN
		(SELECT D.Organization ,D.ID,
		COUNT(DISTINCT (CASE WHEN CONVERT(DATE,BookingDate)=CONVERT(DATE,GETDATE()) THEN TCN.TC_NewCarInquiriesId END))  TotalTodaysBookedCar,
		  COUNT(DISTINCT (CASE WHEN  TCIL.TC_LeadDispositionID=4 THEN TCIL.TC_LeadId END ))  TotalBookedCar
		FROM TC_NewCarInquiries AS TCN WITH (NOLOCK) ,TC_InquiriesLead AS TCIL WITH (NOLOCK),Dealers AS D WITH (NOLOCK)
		 WHERE TCN.TC_InquiriesLeadId=TCIL.TC_InquiriesLeadId 
		  AND D.ID=TCIL.BranchId
		 AND BookingStatus=32
		 AND D.Status = 0
		 AND D.ID IN(50)
		 GROUP BY D.Organization,D.ID) B
		ON 
		 A.ID=B.ID
		--LEFT JOIN 
		-- ( SELECT TCL.BRANCHID,
		--		COUNT(DISTINCT(CASE WHEN TCDL.TC_LeadDispositionId=28   THEN TCDL.TC_LeadId END)) AS TDCompleted,
		--		COUNT(DISTINCT(CASE WHEN TCDL.TC_LeadDispositionId=27   THEN TCDL.TC_LeadId END)) AS TDCancelled,
		--		COUNT(DISTINCT(CASE WHEN TCDL.TC_LeadDispositionId=39 OR TCDL.TC_LeadDispositionId=29  THEN TCDL.TC_LeadId END)) AS TDBooked
		-- FROM  TC_DispositionLog AS TCDL  WITH (NOLOCK)
		-- JOIN  TC_Lead AS TCL  WITH (NOLOCK) ON TCL.TC_LeadId=TCDL.TC_LeadId
		-- JOIN  TC_LeadDisposition AS TCLD   WITH (NOLOCK) ON TCLD.TC_LeadDispositionId=TCDL.TC_LeadDispositionId
		-- group by TCL.BRANCHID) C
		-- ON A.ID=C.BranchId 
 ORDER BY [Total No of Logins] DESC

END