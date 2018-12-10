IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_BookingSummary]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_BookingSummary]
GO

	
-- =============================================
-- Author:		<Chetan Navin>
-- Create date: <3 Feb 2013>
-- Description:	<Get Booking Releated Data DayWise,SourceWise and CarWise>
-- Modified By: Nilesh Utture on 19th June, 2013 Data is only populated based on the users(@ReportingUsersList) who report to the logged in user
-- Modified By: Nilesh Utture on 18th July, 2013 Changed size 0f @ReportingUsersList to MAX	
-- Modified By: Nilesh Utture on 6th August, 2013 Delivered car will not be included in Bookings
-- Modified By: Nilesh Utture on 26th August, 2013 Fetched Total bookings count, instead of live bookings
-- Modified By: Khushaboo Patil on 12th May 2015 added @applicationId
-- Modified By: Afrose on 7th July 2015, changed source wise bookings from TC_InquirySource to TIGS.GroupSourceName
-- =============================================
CREATE  PROCEDURE [dbo].[TC_BookingSummary] 
	@BranchId INT, 
	@FromDate	DATETIME = NULL,
	@ToDate		DATETIME = NULL,
	@ReportingUsersList VARCHAR(MAX) = '-1',  -- '-1': No user is reporting to logged in user, -- Modified By: Nilesh Utture on 18th July, 2013 Changed size 0f @ReportingUsersList to MAX
											 --	NULL: in case if logged in user is Admin, 
											 -- Else comma separated user List 
	@applicationId	TINYINT	= 1
AS

BEGIN
	--SET NOCOUNT ON
	                  
	    --Day wise Booking Counts
			SELECT DAY(TNC.BookingDate) AS [Day], COUNT(TNC.TC_NewCarInquiriesId) AS Bookings,MONTH(TNC.BookingDate) AS Month
			FROM TC_NewCarInquiries TNC WITH (NOLOCK)
			JOIN TC_InquiriesLead TIL WITH (NOLOCK) ON TNC.TC_InquiriesLeadId = TIL.TC_InquiriesLeadId
			WHERE TNC.BookingStatus = 32 /*AND ISNULL(TNC.CarDeliveryStatus,0) <>77*/  AND BranchId = @BranchId AND TNC.BookingDate BETWEEN @FromDate AND @ToDate -- Delivered car will not be included in Bookings
			AND (TIL.TC_UserId IN (SELECT ListMember AS UserId FROM fnSplitCSV(@ReportingUsersList)) OR @ReportingUsersList IS NULL) -- Modified By: Nilesh Utture on 19th June, 2013
			GROUP BY DAY(TNC.BookingDate),MONTH(TNC.BookingDate) 

		--Source wise Booking Counts
			SELECT TIGS.GroupSourceName AS Source, COUNT(TNC.TC_NewCarInquiriesId) AS Bookings 
			FROM TC_NewCarInquiries TNC WITH (NOLOCK)
			JOIN TC_InquiriesLead TIL WITH (NOLOCK) ON TNC.TC_InquiriesLeadId = TIL.TC_InquiriesLeadId
			JOIN TC_InquirySource TIS WITH (NOLOCK) ON TIS.Id = TNC.TC_InquirySourceId
			JOIN TC_InquiryGroupSource AS TIGS WITH(NOLOCK)ON TIS.TC_InquiryGroupSourceId=TIGS.TC_InquiryGroupSourceId
			WHERE TNC.BookingStatus = 32 /*AND ISNULL(TNC.CarDeliveryStatus,0) <>77*/ AND TIL.BranchId = @BranchId AND TNC.BookingDate BETWEEN @FromDate AND @ToDate -- Delivered car will not be included in Bookings
			AND (TIL.TC_UserId IN (SELECT ListMember AS UserId FROM fnSplitCSV(@ReportingUsersList)) OR @ReportingUsersList IS NULL) -- Modified By: Nilesh Utture on 19th June, 2013
			GROUP BY TIGS.TC_InquiryGroupSourceId, TIGS.GroupSourceName

		--CarModel wise Booking Counts
			SELECT CV.Model AS ModelName, COUNT(TNC.TC_NewCarInquiriesId) AS Bookings 
			FROM TC_NewCarInquiries as TNC WITH (NOLOCK)
			JOIN TC_InquiriesLead TIL WITH (NOLOCK) ON TIL.TC_InquiriesLeadId = TNC.TC_InquiriesLeadId
			JOIN vwAllMMV CV WITH (NOLOCK) ON TNC.VersionId = CV.VersionId AND CV.ApplicationId = @applicationId
			WHERE TNC.BookingStatus=32 /*AND ISNULL(TNC.CarDeliveryStatus,0) <>77*/ AND BranchId = @BranchId AND TNC.BookingDate BETWEEN @FromDate AND @ToDate -- Delivered car will not be included in Bookings
			AND (TIL.TC_UserId IN (SELECT ListMember AS UserId FROM fnSplitCSV(@ReportingUsersList)) OR @ReportingUsersList IS NULL) -- Modified By: Nilesh Utture on 19th June, 2013
			GROUP BY CV.Model
		
END


/****** Object:  StoredProcedure [dbo].[TC_ActiveLeadsCount]    Script Date: 15 07 2015 17:37:38 ******/
SET ANSI_NULLS ON
