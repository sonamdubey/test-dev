IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_SalesPerformance]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_SalesPerformance]
GO

	-- =============================================
-- Author		: Chetan Navin
-- Create date	: 15 Feb 2013
-- Description	: Get Sales data Executive wise , Source wise and Model wise	
-- Modified By	: Nilesh Utture on 19th June, 2013 Data is only populated based on the users(@ReportingUsersList) who report to the logged in user
-- Modified By	: Sachin Bharti(24th June 2013) Adding filter on Date range	
-- Modified by  : Nilesh utture on 12th July, 2013 Added is active condition
-- Modified By: Nilesh Utture on 18th July, 2013 Changed size 0f @ReportingUsersList to MAX	
-- Modified By: Nilesh Utture on 2nd August, 2013 changed Active Leads count(Not Showing booked Leads as ACtive one's)
-- Modified By: Nilesh Utture on 7th August, 2013 Made Booking Count event date based and bbokings are bookings less deliveries.
-- Modified By: Nilesh Utture on 26th August, 2013 Fetched Total bookings count, instead of live bookings
-- Modified By: Afrose, Selecting group source from TC_InquiryGroupSource instead of TC_InquirySource.
-- Modified By : Nilima More On 26th July 2016 ,added condition for application id.
-- =============================================
CREATE  PROCEDURE [dbo].[TC_SalesPerformance] 
	-- Add the parameters for the stored procedure here
	@BranchId INT ,
	@FromDate	DATETIME = NULL,
	@ToDate		DATETIME = NULL,
	@ApplicationId SMALLINT,
	@Type    TINYINT ,
	@ReportingUsersList VARCHAR(MAX) = '-1'  -- '-1': No user is reporting to logged in user, -- Modified By: Nilesh Utture on 18th July, 2013 Changed size 0f @ReportingUsersList to MAX	 
											 --	NULL: in case if logged in user is Admin, 
											 -- Else comma separated user List 
AS
BEGIN
	SET NOCOUNT ON;
	
    --Type 1 for Source wise TotalLeads,ActiveLeads and Booking count.
    IF ( @Type = 1 AND @ApplicationId=1)   
	 
        SELECT  TIGS.GroupSourceName AS Roles,
		COUNT(DISTINCT(CASE WHEN TCIL.CreatedDate BETWEEN @FromDate AND @ToDate THEN TCIL.TC_LeadId END)) AS TotalLeads ,
		COUNT(DISTINCT(CASE WHEN TCIL.TC_LeadStageId<>3 AND ISNULL(TCIL.TC_LeadDispositionID,0)<>4 AND TCIL.CreatedDate BETWEEN @FromDate AND @ToDate THEN TCIL.TC_LeadId END)) AS ActiveLeads, -- Modified By: Nilesh Utture on 2nd August, 2013
		COUNT(DISTINCT(CASE WHEN TNI.BookingStatus=32 /*AND ISNULL(TNI.CarDeliveryStatus,0) <> 77*/ AND TNI.BookingDate BETWEEN @FromDate AND @ToDate THEN TNI.TC_NewCarInquiriesId END)) AS Booked
		FROM TC_Lead AS TCL WITH(NOLOCK)
		JOIN TC_InquiriesLead AS TCIL WITH(NOLOCK) ON TCL.TC_LeadId=TCIL.TC_LeadId
		AND TCIL.TC_LeadInquiryTypeId = 3 AND TCIL.BranchId = @BranchId
		AND (TCIL.TC_UserId IN (SELECT ListMember AS UserId FROM fnSplitCSV(@ReportingUsersList)) OR @ReportingUsersList IS NULL) -- Modified By: Nilesh Utture on 19th June, 2013
		JOIN TC_NewCarInquiries TNI WITH(NOLOCK) ON TCIL.TC_InquiriesLeadId =TNI.TC_InquiriesLeadId 
		JOIN TC_InquirySource TIS WITH (NOLOCK) ON TIS.Id = TCIL.InqSourceId
		INNER JOIN TC_InquiryGroupSource AS TIGS WITH(NOLOCK)ON TIS.TC_InquiryGroupSourceId=TIGS.TC_InquiryGroupSourceId
		--WHERE TCL.LeadCreationDate BETWEEN @FromDate AND @ToDate
		WHERE IsVisibleCW=@ApplicationId
		GROUP BY TIGS.TC_InquiryGroupSourceId, TIGS.GroupSourceName

		IF ( @Type = 1 AND @ApplicationId=2)   
	 
        SELECT  TIGS.GroupSourceName AS Roles,
		COUNT(DISTINCT(CASE WHEN TCIL.CreatedDate BETWEEN @FromDate AND @ToDate THEN TCIL.TC_LeadId END)) AS TotalLeads ,
		COUNT(DISTINCT(CASE WHEN TCIL.TC_LeadStageId<>3 AND ISNULL(TCIL.TC_LeadDispositionID,0)<>4 AND TCIL.CreatedDate BETWEEN @FromDate AND @ToDate THEN TCIL.TC_LeadId END)) AS ActiveLeads, -- Modified By: Nilesh Utture on 2nd August, 2013
		COUNT(DISTINCT(CASE WHEN TNI.BookingStatus=32 /*AND ISNULL(TNI.CarDeliveryStatus,0) <> 77*/ AND TNI.BookingDate BETWEEN @FromDate AND @ToDate THEN TNI.TC_NewCarInquiriesId END)) AS Booked
		FROM TC_Lead AS TCL WITH(NOLOCK)
		JOIN TC_InquiriesLead AS TCIL WITH(NOLOCK) ON TCL.TC_LeadId=TCIL.TC_LeadId
		AND TCIL.TC_LeadInquiryTypeId = 3 AND TCIL.BranchId = @BranchId
		AND (TCIL.TC_UserId IN (SELECT ListMember AS UserId FROM fnSplitCSV(@ReportingUsersList)) OR @ReportingUsersList IS NULL) -- Modified By: Nilesh Utture on 19th June, 2013
		JOIN TC_NewCarInquiries TNI WITH(NOLOCK) ON TCIL.TC_InquiriesLeadId =TNI.TC_InquiriesLeadId 
		JOIN TC_InquirySource TIS WITH (NOLOCK) ON TIS.Id = TCIL.InqSourceId
		INNER JOIN TC_InquiryGroupSource AS TIGS WITH(NOLOCK)ON TIS.TC_InquiryGroupSourceId=TIGS.TC_InquiryGroupSourceId
		--WHERE TCL.LeadCreationDate BETWEEN @FromDate AND @ToDate
		WHERE IsVisibleBW=@ApplicationId
		GROUP BY TIGS.TC_InquiryGroupSourceId, TIGS.GroupSourceName
		
	--Type 2 for Model wise TotalLeads,ActiveLeads and Booking count.			
	IF ( @Type = 2)
	    SELECT CV.Model AS Roles,
		COUNT(DISTINCT(CASE WHEN TCIL.CreatedDate BETWEEN @FromDate AND @ToDate THEN TCIL.TC_LeadId END)) AS TotalLeads ,
		COUNT(DISTINCT(CASE WHEN TCIL.TC_LeadStageId<>3 AND ISNULL(TCIL.TC_LeadDispositionID,0)<>4 AND TCIL.CreatedDate BETWEEN @FromDate AND @ToDate THEN TCIL.TC_LeadId END)) AS ActiveLeads, -- Modified By: Nilesh Utture on 2nd August, 2013
		COUNT(DISTINCT(CASE WHEN TNI.BookingStatus=32 /*AND ISNULL(TNI.CarDeliveryStatus,0) <> 77*/ AND TNI.BookingDate BETWEEN @FromDate AND @ToDate THEN TNI.TC_NewCarInquiriesId END)) AS Booked
		FROM TC_Lead AS TCL WITH(NOLOCK)
		JOIN TC_InquiriesLead AS TCIL WITH(NOLOCK) ON TCL.TC_LeadId=TCIL.TC_LeadId
		AND TCIL.TC_LeadInquiryTypeId = 3 AND TCIL.BranchId = @BranchId
		AND (TCIL.TC_UserId IN (SELECT ListMember AS UserId FROM fnSplitCSV(@ReportingUsersList)) OR @ReportingUsersList IS NULL) -- Modified By: Nilesh Utture on 19th June, 2013
		JOIN TC_NewCarInquiries TNI WITH(NOLOCK) ON TCIL.TC_InquiriesLeadId =TNI.TC_InquiriesLeadId 
		--JOIN vwMMV CV WITH (NOLOCK) ON TNI.VersionId = CV.VersionId
		JOIN DEALERS D WITH(NOLOCK) ON D.ID = TCIL.BranchId
		JOIN vwAllMMV CV WITH (NOLOCK) ON TNI.VersionId = CV.VersionId AND D.ApplicationId = CV.ApplicationId  --Modified By : Nilima More On 26th July 2016 ,added condition for application id.
		--WHERE TCL.LeadCreationDate BETWEEN @FromDate AND @ToDate
		GROUP BY CV.Model
		
	--Type 2 for Executive wise TotalLeads,ActiveLeads and Booking count.				
	IF ( @Type = 3)
		SELECT TU.UserName AS Roles,
		COUNT(DISTINCT(CASE WHEN TCIL.CreatedDate BETWEEN @FromDate AND @ToDate THEN TCIL.TC_LeadId END)) AS TotalLeads ,
		COUNT(DISTINCT(CASE WHEN TCIL.TC_LeadStageId<>3 AND ISNULL(TCIL.TC_LeadDispositionID,0)<>4 AND TCIL.CreatedDate BETWEEN @FromDate AND @ToDate THEN TCIL.TC_LeadId END)) AS ActiveLeads, -- Modified By: Nilesh Utture on 2nd August, 2013
		COUNT(DISTINCT(CASE WHEN TNI.BookingStatus=32 /*AND ISNULL(TNI.CarDeliveryStatus,0) <> 77*/ AND TNI.BookingDate BETWEEN @FromDate AND @ToDate THEN TNI.TC_NewCarInquiriesId END)) AS Booked
		FROM TC_Lead AS TCL WITH(NOLOCK)
		JOIN TC_InquiriesLead AS TCIL WITH(NOLOCK) ON TCL.TC_LeadId=TCIL.TC_LeadId
		AND TCIL.TC_LeadInquiryTypeId = 3 AND TCIL.BranchId = @BranchId
		AND (TCIL.TC_UserId IN (SELECT ListMember AS UserId FROM fnSplitCSV(@ReportingUsersList)) OR @ReportingUsersList IS NULL) -- Modified By: Nilesh Utture on 19th June, 2013
		JOIN TC_NewCarInquiries TNI WITH(NOLOCK) ON TCIL.TC_InquiriesLeadId =TNI.TC_InquiriesLeadId 
		JOIN TC_Users TU WITH(NOLOCK) ON TCIL.TC_UserId = TU.Id AND TU.IsActive = 1 -- Modified by Nilesh utture on 12th July, 2013 Added is active condition
		--WHERE TCL.LeadCreationDate BETWEEN @FromDate AND @ToDate
		GROUP BY TU.UserName		
				
END


/****** Object:  StoredProcedure [dbo].[TC_ActiveLeadsCount]    Script Date: 7/26/2016 3:52:08 PM ******/
SET ANSI_NULLS ON
