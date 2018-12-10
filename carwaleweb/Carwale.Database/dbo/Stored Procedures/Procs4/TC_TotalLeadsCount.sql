IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TotalLeadsCount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TotalLeadsCount]
GO

	--	Author	:	Sachin Bharti(12th March 2013)
--	Purpose	:	To get counts of all inquieries leads on basis of
--				Day , Eagerness, Source and Car Models
-- Modified By Manish on 17-06-2013  for talling the count of reports
-- Modified By	:	Sachin Bharti(21st June 2013) Added Date Range filter
-- Modified By: Nilesh Utture on 04th July, 2013 Added select statement to get Employee Lead
-- Modified By: Nilesh Utture on 09th July, 2013 Added U.lvl <> 0 as root user should not be shown
-- Modified By: Nilesh Utture on 12th July, 2013 Added IsActive condition to Select statement
-- Modified By: Nilesh Utture on 18th July, 2013 Changed size 0f @ReportingUsersList to MAX
-- Modified By: Nilesh Utture on 25th July, 2013 11:49 AM BookedLead AND Lost Lead are shown on basis of event creation date
--				AND Booking data is shown Inquiry wise instead of Lead wise
-- Modified By: Nilesh Utture on 29th July, 2013 Changed INNER JOIN TO LEFT JOIN 	
-- Modified BY: Nilesh Utture on 1st August, 2013 Fetched source wise count from Inquiries Lead TABLE, Fetched Pending TD COUNT based on TDDATE, Added count for Delivered Leads TD Completed	
-- Modified By: Nilesh Utture on 5th August, 2013 Added InquiryTypeId = 3 condition	
-- Modified By: Nilesh Utture on 6th August, 2013 Delivered car will not be included in Bookings
-- Modified By: Nilesh Utture on 26th August, 2013 Live bookings will not be based on booking date, Added TotalBookedLeads in SELECT
-- Modified By: Manish Chourasiya on 10-09-2013 added condition (tc_leadstageid<>3 )in live booked leads 
-- Modified By: Nilesh Utture on 19-09-2013 Added Retail, Pending Deliveries, Changed logic of live bookings, Pending TD's and Pending Followup's
-- Modified By: Manish Chourasiya on 23-09-2013 for live booking lead add condition "AND ISNULL(TCNCI.CarDeliveryStatus,0)<>77 "
-- Modified by: Deepak on 16-10-2013 for resolving the issue of not loading report. Exact reason not identified under monitoring.
-- Modifid By Vivek Gupta on 15-07-2014, Added query for getting rejected inquiries and differentiating Lost inquiries with them
-- Modified By Vivek Gupta on 26-08-2014, Added queries to get inquiries date wise of its creation date
-- Modified By Vivek Gupta on 05-11-2-14, Added inner join in place of left join in tc_inquirieslead and tc_lead
-- Modified By : Tejashree Patil on 13 Nov 2014, Added @ApplicationId to identify application and joined with vwAllMMV view.
-- Modified By Vivek Gupta on 11-02-2015, replaced TCIL.TC_LeadId with TCIL.TC_inquiriesleadid
--Modified by: Afrose on 7th July 2015, modified counts of total leads on basis of Source from TC_InquirySource to TIGS.GroupSourceName
--Modified By : Ashwini Dhamankar on Dec 17,2015 (handled null condition in lvl of user table)
--Modified By : Ashwini Dhamankar on Jan 25,2016 (Removed condition of MostInterested in booking and totalbooking status) 
--Modified By : Ashwini Dhamankar on April 4,2016 (Added constaint of bookingstatus=32 for TotalBookedLead)
--Modified By : Ashwini Dhamankar on May 16,2016 (Added constraint of  ISNULL(TCNCI.CarDeliveryStatus,0)<>77) for pending followup
--Modified By : Nilima More On Sept 16,2016 (modified pending followup condition)
--Modified By : Nilima More On Sept 21,2016 (added business type condition)
--Modified By : Nilima More On Sept 22,2016,commented isleadactive condition from customerDetails join.
--Modified By : Nilima More On Oct 14,2016,replaced convert to Date range with one new variable @TodaysStart .
--EXEC TC_TotalLeadsCount 5,'2016-07-01','2016-07-25',NULL,1
--Modified By : Nilima More On Nov 2,2016,replaced value  00:00:01 to  00:00:00 for  @TodaysStart variable.
--===============================================================================================================================

CREATE PROCEDURE [dbo].[TC_TotalLeadsCount]
@BRANCHID	INT,
@FromDate	DateTime = NULL,
@EndDate	DateTime = NULL,
@ReportingUsersList VARCHAR(MAX),-- = '-1'  -- '-1': No user is reporting to logged in user, -- Modified By: Nilesh Utture on 18th July, 2013 Changed size to MAX
										 --	NULL: in case if logged in user is Admin, 
										 -- Else comma separated user List 
@ApplicationId TINYINT = 1-- Modified By : Tejashree Patil on 13 Nov 2014, default @ApplicationId is 1 carwale
AS
BEGIN
		
	------Below code is added by Deepak on 16-10-2013 for resolving the issue of not loading report. Exact reason not identified under monitoring----
	     DECLARE @TempUsers Table(UserId INT)
		 DECLARE @TodaysStart DATETIME = CONVERT(DATETIME, CONVERT(VARCHAR(10), GETDATE(), 120) + ' 00:00:00')

	
		IF (@ReportingUsersList IS NOT NULL)
		  BEGIN 
			INSERT INTO @TempUsers (UserId)
			SELECT ListMember AS UserId FROM fnSplitCSV(@ReportingUsersList)
		  END 
	--------------------------------------------------------------------------------------------------------------------------
							
		-- To get the counts of total leads on Day wise basis
		SELECT DAY(TIL.CreatedDate)AS DAY ,COUNT(DISTINCT TIL.TC_InquiriesLeadId) AS Total ,MONTH(TIL.CreatedDate) AS Month
		FROM TC_InquiriesLead TIL WITH(NOLOCK)
		WHERE TIL.TC_LeadInquiryTypeId = 3 and TIL.BranchId = @BranchId --LeadInquiryTypeId is 3 for New Car inquiries only 
		AND (TIL.TC_UserId IN (SELECT UserId FROM @TempUsers) OR @ReportingUsersList IS NULL) -- Modified By: Nilesh Utture on 19th June, 2013
		AND TIL.CreatedDate BETWEEN @FromDate AND @EndDate
		GROUP BY DAY(TIL.CreatedDate),MONTH(TIL.CreatedDate) 
		
		
		-- To get the counts of total leads on basis of Eagerness
		SELECT ISNULL(TIS.Status,'Not yet set') AS Status,COUNT(DISTINCT TIL.TC_InquiriesLeadId) AS TOTAL 
		FROM TC_InquiriesLead TIL WITH(NOLOCK)
		LEFT JOIN TC_InquiryStatus TIS WITH(NOLOCK) ON TIS.TC_InquiryStatusId = TIL.TC_InquiryStatusId
		WHERE  TIL.CreatedDate BETWEEN @FromDate AND @EndDate
		AND TIL.TC_LeadInquiryTypeId = 3 and TIL.BranchId = @BranchId --LeadInquiryTypeId is 3 for New Car inquiries only 
		AND (TIL.TC_UserId IN (SELECT UserId FROM @TempUsers) OR @ReportingUsersList IS NULL) -- Modified By: Nilesh Utture on 19th June, 2013
		GROUP BY TIS.Status
		ORDER BY TIS.Status
	
		-- To get the counts of total leads on basis of Source
		SELECT TIGS.GroupSourceName ,COUNT(DISTINCT TIL.TC_InquiriesLeadId) AS Total
		FROM TC_InquiriesLead TIL WITH(NOLOCK)
		--INNER JOIN TC_Lead TNI WITH(NOLOCK) ON TNI.TC_LeadId = TIL.TC_LeadId
		INNER JOIN TC_InquirySource TIS WITH(NOLOCK) ON TIS.Id = TIL.InqSourceId  -- Modified BY: Nilesh Utture on 1st August, 2013 
		INNER JOIN TC_InquiryGroupSource AS TIGS WITH(NOLOCK)ON TIS.TC_InquiryGroupSourceId=TIGS.TC_InquiryGroupSourceId
		WHERE  TIL.CreatedDate BETWEEN @FromDate AND @EndDate
		AND TIL.TC_LeadInquiryTypeId = 3 and TIL.BranchId = @BranchId --LeadInquiryTypeId is 3 for New Car inquiries only 
		AND (TIL.TC_UserId IN (SELECT UserId FROM @TempUsers) OR @ReportingUsersList IS NULL) -- Modified By: Nilesh Utture on 19th June, 2013
		GROUP BY TIGS.TC_InquiryGroupSourceId, TIGS.GroupSourceName
		ORDER BY TIGS.TC_InquiryGroupSourceId, TIGS.GroupSourceName

		-- To get the counts of total leads on basis Car Models
		SELECT VW.Model,COUNT(DISTINCT TIL.TC_InquiriesLeadId) AS Total
		FROM TC_InquiriesLead TIL WITH(NOLOCK)
		INNER JOIN TC_NewCarInquiries TNI WITH(NOLOCK) ON TNI.TC_InquiriesLeadId = TIL.TC_InquiriesLeadId
		--INNER JOIN vwMMV VW WITH(NOLOCK) ON VW.VersionId = TNI.VersionId
		INNER JOIN vwAllMMV VW WITH(NOLOCK) ON VW.VersionId = TNI.VersionId AND VW.ApplicationId = @ApplicationId-- Modified By : Tejashree Patil on 13 Nov 2014,
		WHERE  TIL.CreatedDate BETWEEN @FromDate AND @EndDate
		AND TIL.TC_LeadInquiryTypeId = 3 and TIL.BranchId = @BranchId --LeadInquiryTypeId is 3 for New Car inquiries only 
		AND (TIL.TC_UserId IN (SELECT UserId FROM @TempUsers) OR @ReportingUsersList IS NULL) -- Modified By: Nilesh Utture on 19th June, 2013
		GROUP BY VW.Model
		ORDER BY VW.Model
		

		
		-- Modified By: Nilesh Utture on 04th July, 2013
		-- Modified By: Nilesh Utture on 25th July, 2013 11:49 AM 
		SELECT REPLACE(U.UserName,'''','') UserName,U.lvl,U.Id, DAY(TCIL.CreatedDate) AS Day, MONTH(TCIL.CreatedDate) AS Month, U.NodeCode,
			COUNT(DISTINCT (CASE WHEN TCIL.CreatedDate BETWEEN @FromDate AND @EndDate AND ISNULL(TCNCI.MostInterested,0) = 1 THEN TCNCI.TC_NewCarInquiriesId END)) AS LeadCount,
			COUNT(DISTINCT (CASE WHEN TCIL.TC_LeadStageId<>3 AND ISNULL(TCIL.TC_LeadDispositionID,0)<>4  AND TCIL.TC_InquiryStatusId=1 AND TCIL.CreatedDate BETWEEN @FromDate AND @EndDate AND ISNULL(TCNCI.MostInterested,0) = 1 THEN TCNCI.TC_NewCarInquiriesId END )) AS ActiveHotLead,
			COUNT(DISTINCT (CASE WHEN TCIL.TC_LeadStageId<>3 AND ISNULL(TCIL.TC_LeadDispositionID,0)<>4  AND TCIL.TC_InquiryStatusId=2 AND TCIL.CreatedDate BETWEEN @FromDate AND @EndDate AND ISNULL(TCNCI.MostInterested,0) = 1 THEN TCNCI.TC_NewCarInquiriesId END )) AS ActiveWarmLead,
			COUNT(DISTINCT (CASE WHEN TCIL.TC_LeadStageId<>3 AND ISNULL(TCIL.TC_LeadDispositionID,0)<>4  AND TCIL.TC_InquiryStatusId=3 AND TCIL.CreatedDate BETWEEN @FromDate AND @EndDate AND ISNULL(TCNCI.MostInterested,0) = 1 THEN TCNCI.TC_NewCarInquiriesId END )) AS ActiveNormalLead,
			COUNT(DISTINCT (CASE WHEN TCIL.TC_LeadStageId<>3 AND ISNULL(TCIL.TC_LeadDispositionID,0)<>4  AND ISNULL(TCIL.TC_InquiryStatusId,0) = 0 AND TCIL.CreatedDate BETWEEN @FromDate AND @EndDate AND ISNULL(TCNCI.MostInterested,0) = 1 THEN TCNCI.TC_NewCarInquiriesId END )) AS ActiveNotSet,
			COUNT(DISTINCT (CASE WHEN TCNCI.TDStatus = 28 AND ISNULL(TCNCI.MostInterested,0) = 1 AND TCNCI.TDDate BETWEEN @FromDate AND @EndDate THEN TCNCI.TC_NewCarInquiriesId END )) AS TDCompleted,
			-----Modified by Manish added condition TC_LeadStageId<> 3 on 10-09-2013----------------------------------
			-------- Modified By: Nilesh Utture on 19-09-2013, showing cummulative Live Booking's till @EndDate
			COUNT(DISTINCT (CASE WHEN  ISNULL(TCNCI.CarDeliveryStatus,0)<>77 AND TCIL.TC_LeadStageId<>3 AND  TCNCI.TC_LeadDispositionId IN (4,41) AND NB.BookingStatus = 32 AND NB.InvoiceDate IS NULL AND NB.BookingDate BETWEEN @FromDate AND @EndDate THEN TCNCI.TC_NewCarInquiriesId END )) AS BookedLead, -- Modified By: Nilesh Utture on 6th August, 2013 Delivered car will not be included in Bookings
			COUNT(DISTINCT (CASE WHEN  TCNCI.TC_LeadDispositionId IN (4,41) AND TCNCI.BookingStatus = 32 AND TCNCI.BookingDate BETWEEN @FromDate AND @EndDate THEN TCNCI.TC_NewCarInquiriesId END )) AS TotalBookedLead, -- Modified By: Nilesh Utture on 26th August, 2013
			-------- Modified By: Nilesh Utture on 19-09-2013, showing all Retails's and pending deliveries count
			COUNT(DISTINCT (CASE WHEN  NB.InvoiceDate IS NOT NULL AND ISNULL(TCNCI.MostInterested,0) = 1 AND NB.InvoiceDate BETWEEN @FromDate AND @EndDate THEN TCNCI.TC_NewCarInquiriesId END )) AS Retail,
			COUNT(DISTINCT (CASE WHEN  TCIL.TC_LeadStageId<>3 AND NB.InvoiceDate IS NOT NULL AND ISNULL(TCNCI.CarDeliveryStatus,0)<>77 AND NB.InvoiceDate <= @EndDate AND ISNULL(TCNCI.MostInterested,0) = 1 THEN TCNCI.TC_NewCarInquiriesId END )) AS PendingDeliveries,
			COUNT(DISTINCT (CASE WHEN  TCNCI.CarDeliveryStatus = 77 AND  ISNULL(TCNCI.MostInterested,0) = 1 AND TCNCI.CarDeliveryDate BETWEEN @FromDate AND @EndDate THEN TCNCI.TC_NewCarInquiriesId END )) AS DeliveredLead,
			COUNT(DISTINCT (CASE WHEN (TCIL.TC_LeadStageId=3 AND  TCIL.TC_LeadDispositionID<>4 AND TCIL.TC_LeadDispositionID NOT IN(1,3,70,71,74))AND TL.LeadClosedDate BETWEEN @FromDate AND @EndDate AND ISNULL(TCNCI.MostInterested,0) = 1 THEN TCNCI.TC_NewCarInquiriesId END )) AS Lost, -- Modifid By Vivek Gupta on 15-07-2014
			COUNT(DISTINCT (CASE WHEN (TCIL.TC_LeadStageId=3 AND TCIL.TC_LeadDispositionID IN(1,3,70,71,74))AND TL.LeadClosedDate BETWEEN @FromDate AND @EndDate AND ISNULL(TCNCI.MostInterested,0) = 1 THEN TCNCI.TC_NewCarInquiriesId END )) AS Rejected, -- Modifid By Vivek Gupta on 15-07-2014
			-------- Modified By: Nilesh Utture on 19-09-2013, showing cummulative Pending Followup's till @EndDate
			COUNT(DISTINCT (CASE WHEN (TCAC.ScheduledOn < @TodaysStart )AND ISNULL(TCIL.TC_LeadDispositionId, 0) <> 4 THEN TCAC.TC_LeadId END )) AS PendingFollowUp, ---Ashwini Dhamankar on May 16,2016 (Added constraint of  ISNULL(TCNCI.CarDeliveryStatus,0)<>77) for pending followup
			-------- Modified By: Nilesh Utture on 19-09-2013, showing cummulative Pending TD's till @EndDate
			COUNT(DISTINCT (CASE WHEN  TCNCI.TDStatus <> 27 AND TCNCI.TDStatus <> 28 AND TCNCI.TDDate <= @EndDate AND ISNULL(TCNCI.MostInterested,0) = 1 THEN TCNCI.TC_NewCarInquiriesId END)) AS PendingTestDrive -- Modified BY: Nilesh Utture on 1st August, 2013 
			--COUNT(DISTINCT (CASE WHEN TCTD.TDDate<CONVERT(DATE,GETDATE()) AND ((TCTD.TDStatus<>27 AND TCTD.TDStatus<>28) OR TCTD.TDStatus IS NULL) AND TCTD.TDStatusDate BETWEEN @FromDate AND @EndDate  THEN TCIL.TC_LeadId END )) AS PendingTestDrive
			
			,COUNT (DISTINCT (CASE WHEN TCAC.TC_CallTypeId = 1 AND TCAC.ScheduledOn BETWEEN @FromDate AND @EndDate THEN TCAC.TC_CallsId END)) AS NoFollowUps
   				  FROM DEALERS as D WITH (NOLOCK)	
				  INNER JOIN TC_CustomerDetails AS C WITH (NOLOCK) ON C.BranchId = D.ID
   				  INNER JOIN TC_Lead AS TL WITH(NOLOCK) ON  C.id = TL.TC_CustomerId -- Modified By: Nilesh Utture on 29th July, 2013	
				  INNER JOIN TC_InquiriesLead AS TCIL WITH (NOLOCK) ON TL.TC_LeadId = TCIL.TC_LeadId AND TCIL.TC_LeadInquiryTypeId = 3/*AND TCIL.CreatedDate BETWEEN @FromDate AND @EndDate*/ AND (TCIL.TC_UserId IN (SELECT UserId FROM @TempUsers) OR @ReportingUsersList IS NULL)
				  INNER JOIN TC_NewCarInquiries AS TCNCI WITH (NOLOCK) ON TCIL.TC_InquiriesLeadId=TCNCI.TC_InquiriesLeadId
				  INNER JOIN TC_Users U WITH (NOLOCK) ON TCIL.TC_UserId = U.Id /*AND  D.IsDealerActive= 1*/  AND U.BranchId = @BranchId AND ISNULL(U.lvl,0) <> 0 /*AND U.IsActive = 1*/ AND (U.Id IN (SELECT UserId FROM @TempUsers) OR @ReportingUsersList IS NULL)
				  LEFT JOIN TC_TaskLists AS TCAC WITH (NOLOCK) ON TCAC.TC_LeadId=TCIL.TC_LeadId
				  LEFT JOIN TC_NewCarBooking AS NB WITH (NOLOCK) ON NB.TC_NewCarInquiriesId = TCNCI.TC_NewCarInquiriesId
				--WHERE TCIL.TC_LeadInquiryTypeId = 3 -- Modified By: Nilesh Utture on 5th August, 2013
				--LEFT JOIN TC_TDCalendar AS TCTD WITH (NOLOCK) ON  TCNCI.TC_TDCalendarId=TCTD.TC_TDCalendarId 
				WHERE D.ID = @BranchId
				GROUP BY U.UserName,U.Id, U.lvl, DAY(TCIL.CreatedDate), MONTH(TCIL.CreatedDate),U.NodeCode
				ORDER BY U.NodeCode ASC

				



			 -- Below code is same as above but leads count is coming inquiries entry date specifically--
			 
			SELECT REPLACE(U.UserName,'''','') UserName,U.lvl,U.Id, DAY(TCIL.CreatedDate) AS Day, MONTH(TCIL.CreatedDate) AS Month, U.NodeCode,
			COUNT(DISTINCT (CASE WHEN TCIL.CreatedDate BETWEEN @FromDate AND @EndDate AND ISNULL(TCNCI.MostInterested,0) = 1 THEN TCNCI.TC_NewCarInquiriesId END)) AS LeadCount,
			COUNT(DISTINCT (CASE WHEN TCIL.TC_LeadStageId<>3 AND ISNULL(TCIL.TC_LeadDispositionID,0)<>4  AND TCIL.TC_InquiryStatusId=1 AND TCIL.CreatedDate BETWEEN @FromDate AND @EndDate AND ISNULL(TCNCI.MostInterested,0) = 1 THEN TCNCI.TC_NewCarInquiriesId END )) AS ActiveHotLead,
			COUNT(DISTINCT (CASE WHEN TCIL.TC_LeadStageId<>3 AND ISNULL(TCIL.TC_LeadDispositionID,0)<>4  AND TCIL.TC_InquiryStatusId=2 AND TCIL.CreatedDate BETWEEN @FromDate AND @EndDate AND ISNULL(TCNCI.MostInterested,0) = 1 THEN TCNCI.TC_NewCarInquiriesId END )) AS ActiveWarmLead,
			COUNT(DISTINCT (CASE WHEN TCIL.TC_LeadStageId<>3 AND ISNULL(TCIL.TC_LeadDispositionID,0)<>4  AND TCIL.TC_InquiryStatusId=3 AND TCIL.CreatedDate BETWEEN @FromDate AND @EndDate AND ISNULL(TCNCI.MostInterested,0) = 1 THEN TCNCI.TC_NewCarInquiriesId END )) AS ActiveNormalLead,
			COUNT(DISTINCT (CASE WHEN TCIL.TC_LeadStageId<>3 AND ISNULL(TCIL.TC_LeadDispositionID,0)<>4  AND ISNULL(TCIL.TC_InquiryStatusId,0) = 0 AND TCIL.CreatedDate BETWEEN @FromDate AND @EndDate AND ISNULL(TCNCI.MostInterested,0) = 1 THEN TCNCI.TC_NewCarInquiriesId END )) AS ActiveNotSet,
			COUNT(DISTINCT (CASE WHEN TCNCI.TDStatus = 28 AND ISNULL(TCNCI.MostInterested,0) = 1 AND TCNCI.TDDate BETWEEN @FromDate AND @EndDate THEN TCNCI.TC_NewCarInquiriesId END )) AS TDCompleted,
			-----Modified by Manish added condition TC_LeadStageId<> 3 on 10-09-2013----------------------------------
			-------- Modified By: Nilesh Utture on 19-09-2013, showing cummulative Live Booking's till @EndDate
			COUNT(DISTINCT (CASE WHEN  ISNULL(TCNCI.CarDeliveryStatus,0)<>77 AND TCIL.TC_LeadStageId<>3 AND  TCNCI.TC_LeadDispositionId IN (4,41) AND NB.BookingStatus = 32 AND NB.InvoiceDate IS NULL  AND NB.BookingDate BETWEEN @FromDate AND @EndDate THEN TCNCI.TC_NewCarInquiriesId END )) AS BookedLead, -- Modified By: Nilesh Utture on 6th August, 2013 Delivered car will not be included in Bookings
			COUNT(DISTINCT (CASE WHEN  TCNCI.TC_LeadDispositionId IN (4,41) AND TCNCI.BookingStatus = 32 AND TCNCI.BookingDate BETWEEN @FromDate AND @EndDate THEN TCNCI.TC_NewCarInquiriesId END )) AS TotalBookedLead, -- Modified By: Nilesh Utture on 26th August, 2013
			-------- Modified By: Nilesh Utture on 19-09-2013, showing all Retails's and pending deliveries count
			COUNT(DISTINCT (CASE WHEN  NB.InvoiceDate IS NOT NULL AND  ISNULL(TCNCI.MostInterested,0) = 1 AND NB.InvoiceDate BETWEEN @FromDate AND @EndDate THEN TCNCI.TC_NewCarInquiriesId END )) AS Retail,
			COUNT(DISTINCT (CASE WHEN  TCIL.TC_LeadStageId<>3 AND NB.InvoiceDate IS NOT NULL AND ISNULL(TCNCI.CarDeliveryStatus,0)<>77 AND NB.InvoiceDate <= @EndDate  AND ISNULL(TCNCI.MostInterested,0) = 1 THEN TCNCI.TC_NewCarInquiriesId END )) AS PendingDeliveries,
			COUNT(DISTINCT (CASE WHEN  TCNCI.CarDeliveryStatus = 77 AND ISNULL(TCNCI.MostInterested,0) = 1 AND TCNCI.CarDeliveryDate BETWEEN @FromDate AND @EndDate THEN TCNCI.TC_NewCarInquiriesId END )) AS DeliveredLead,
			COUNT(DISTINCT (CASE WHEN (TCIL.TC_LeadStageId=3 AND  TCIL.TC_LeadDispositionID<>4 AND TCIL.TC_LeadDispositionID NOT IN(1,3,70,71,74))AND TL.LeadClosedDate BETWEEN @FromDate AND @EndDate AND ISNULL(TCNCI.MostInterested,0) = 1 THEN TCNCI.TC_NewCarInquiriesId END )) AS Lost, -- Modifid By Vivek Gupta on 15-07-2014
			COUNT(DISTINCT (CASE WHEN (TCIL.TC_LeadStageId=3 AND TCIL.TC_LeadDispositionID IN(1,3,70,71,74))AND TL.LeadClosedDate BETWEEN @FromDate AND @EndDate AND ISNULL(TCNCI.MostInterested,0) = 1 THEN TCNCI.TC_NewCarInquiriesId END )) AS Rejected--, -- Modifid By Vivek Gupta on 15-07-2014
			-------- Modified By: Nilesh Utture on 19-09-2013, showing cummulative Pending Followup's till @EndDate
			--COUNT(DISTINCT (CASE WHEN TCAC.ScheduledOn <= @EndDate THEN TCIL.TC_LeadId END )) AS PendingFollowUp, 
			-------- Modified By: Nilesh Utture on 19-09-2013, showing cummulative Pending TD's till @EndDate
			--COUNT(DISTINCT (CASE WHEN  TCNCI.TDStatus <> 27 AND TCNCI.TDStatus <> 28 AND TCNCI.TDDate <= @EndDate THEN TCNCI.TC_NewCarInquiriesId END)) AS PendingTestDrive -- Modified BY: Nilesh Utture on 1st August, 2013 
			--COUNT(DISTINCT (CASE WHEN TCTD.TDDate<CONVERT(DATE,GETDATE()) AND ((TCTD.TDStatus<>27 AND TCTD.TDStatus<>28) OR TCTD.TDStatus IS NULL) AND TCTD.TDStatusDate BETWEEN @FromDate AND @EndDate  THEN TCIL.TC_LeadId END )) AS PendingTestDrive
   				  FROM DEALERS as D WITH (NOLOCK)		
				INNER JOIN TC_CustomerDetails AS C WITH (NOLOCK) ON C.BranchId = D.ID	
				INNER JOIN TC_Lead AS TL WITH(NOLOCK) ON C.Id = TL.TC_LeadId -- Modified By: Nilesh Utture on 29th July, 2013	
				INNER JOIN TC_InquiriesLead AS TCIL WITH (NOLOCK) ON TL.TC_LeadId = TCIL.TC_LeadId  AND TCIL.TC_LeadInquiryTypeId = 3 AND TCIL.CreatedDate BETWEEN @FromDate AND @EndDate AND (TCIL.TC_UserId IN (SELECT UserId FROM @TempUsers) OR @ReportingUsersList IS NULL)																					   -- Modified By: Nilesh Utture on 09th July, 2013 and 12th July, 2013
				INNER JOIN TC_NewCarInquiries AS TCNCI WITH (NOLOCK) ON TCIL.TC_InquiriesLeadId=TCNCI.TC_InquiriesLeadId
				INNER JOIN TC_Users U WITH (NOLOCK) ON D.ID = U.BranchId /*AND  D.IsDealerActive= 1*/ AND D.ID = @BranchId AND U.BranchId = @BranchId AND ISNULL(U.lvl,0) <> 0 /*AND U.IsActive = 1*/ AND (U.Id IN (SELECT UserId FROM @TempUsers) OR @ReportingUsersList IS NULL)
				LEFT JOIN TC_TaskLists AS TCAC WITH (NOLOCK) ON TCAC.TC_LeadId=TCIL.TC_LeadId
				LEFT JOIN TC_NewCarBooking AS NB WITH (NOLOCK) ON NB.TC_NewCarInquiriesId = TCNCI.TC_NewCarInquiriesId
				--WHERE TL.TC_BusinessTypeId = 3 --Modified by Nilima More On 21st Sept 2016
				--WHERE TCIL.TC_LeadInquiryTypeId = 3 -- Modified By: Nilesh Utture on 5th August, 2013
				--LEFT JOIN TC_TDCalendar AS TCTD WITH (NOLOCK) ON  TCNCI.TC_TDCalendarId=TCTD.TC_TDCalendarId 
				GROUP BY U.UserName,U.Id, U.lvl, DAY(TCIL.CreatedDate), MONTH(TCIL.CreatedDate),U.NodeCode
				ORDER BY U.NodeCode ASC

END