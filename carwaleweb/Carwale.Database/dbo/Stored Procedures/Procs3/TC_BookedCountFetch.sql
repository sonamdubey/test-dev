IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_BookedCountFetch]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_BookedCountFetch]
GO

	-- Modified By: Nilesh Utture on 19th June, 2013 Data is only populated based on the users(@ReportingUsersList) who report to the logged in user
-- Modified By	:	Sachin Bharti(21st June 2013)Added filter for Date range
-- Modified By: Nilesh Utture on 18th July, 2013 Changed size 0f @ReportingUsersList to MAX	
-- Modified By: Nilesh Utture on 31st July, 2013 Fetching count based on booking date instead of lead creation date
-- Modified By: Nilesh Utture on 6th August, 2013 Delivered car will not be included in Bookings
-- Modified By: Nilesh Utture on 26th August, 2013 Fetched Total bookings count, instead of live bookings

CREATE  PROCEDURE [dbo].[TC_BookedCountFetch]
@ModelId	NUMERIC (18,0),
@Versionid	NUMERIC (18,0),
@BranchId	NUMERIC (18,0),
@fuelType	INT,
@FromDate	DateTime = NULL,
@EndDate	DateTime = NULL,
@ReportingUsersList VARCHAR (MAX) = '-1' -- '-1': No user is reporting to logged in user, -- Modified By: Nilesh Utture on 18th July, 2013 Changed size 0f @ReportingUsersList to MAX	
										 --	NULL: in case if logged in user is Admin, 
										 -- Else comma separated user List
AS 
BEGIN
--- query for Booked car for all model
	SELECT CONVERT(CHAR(3),DATENAME(MM,NCI.BookingDate)) AS Month,COUNT(DISTINCT NCI.TC_NewCarInquiriesId) Booked,
	YEAR(NCI.BookingDate) AS Year,MONTH(NCI.BookingDate) AS MonthId, DAY(NCI.BookingDate) AS Day
	FROM TC_NewCarInquiries NCI WITH(NOLOCK)
		INNER JOIN TC_InquiriesLead IL WITH(NOLOCK) ON IL.TC_InquiriesLeadId = NCI.TC_InquiriesLeadId
		INNER JOIN CRM.vwMMV V ON NCI.VersionId=V.VersionId
	WHERE IL.TC_LeadInquiryTypeId=3 AND Year(NCI.BookingDate) = YEAR(GETDATE()) 
		AND NCI.BookingStatus= 32-- for Booking Completed
		--AND ISNULL(NCI.CarDeliveryStatus,0)<>77 -- Delivered car will not be included in Bookings -- Modified By: Nilesh Utture on 26th August, 2013
		AND IL.BranchId = @BranchId
		AND (IL.TC_UserId IN	(SELECT ListMember AS UserId FROM fnSplitCSV(@ReportingUsersList)) OR @ReportingUsersList IS NULL) -- Modified By: Nilesh Utture on 19th June, 2013
		--Modified By Sachin Bharti(21st Jun 2013 )
		AND NCI.BookingDate BETWEEN @FromDate AND @EndDate -- Modified By: Nilesh Utture on 31st July, 2013 
	GROUP BY DATENAME(MM,NCI.BookingDate),YEAR(NCI.BookingDate),MONTH(NCI.BookingDate),DAY(NCI.BookingDate)
	
	--- query for Booked car for selected model and version
	SELECT CONVERT(CHAR(3),DATENAME(MM,NCI.BookingDate)) AS Month,COUNT(DISTINCT NCI.TC_NewCarInquiriesId) BookedCar,
	YEAR(NCI.BookingDate) AS Year,MONTH(NCI.BookingDate) AS MonthId ,DAY(NCI.BookingDate) AS Day
	FROM TC_NewCarInquiries NCI WITH(NOLOCK)
		INNER JOIN TC_InquiriesLead IL WITH(NOLOCK) ON IL.TC_InquiriesLeadId = NCI.TC_InquiriesLeadId
		INNER JOIN CRM.vwMMV V ON NCI.VersionId=V.VersionId
	WHERE IL.TC_LeadInquiryTypeId=3 
		AND Year(NCI.BookingDate) = YEAR(GETDATE())
		AND NCI.BookingStatus= 32-- for Booking Completed
		--AND ISNULL(NCI.CarDeliveryStatus,0)<>77  -- Delivered car will not be included in Bookings -- Modified By: Nilesh Utture on 26th August, 2013
		AND IL.BranchId = @BranchId 
		AND (IL.TC_UserId IN	(SELECT ListMember AS UserId FROM fnSplitCSV(@ReportingUsersList)) OR @ReportingUsersList IS NULL) -- Modified By: Nilesh Utture on 19th June, 2013
		AND ( @ModelId IS NULL OR V.ModelId=@ModelId) 
		AND (@Versionid IS NULL OR V.VersionId=@Versionid) 
		AND(@fuelType IS NULL OR V.CarFuelType=@fuelType)
		--Modified By Sachin Bharti(21st Jun 2013 )
		AND NCI.BookingDate BETWEEN @FromDate AND @EndDate -- Modified By: Nilesh Utture on 31st July, 2013 
	GROUP BY DATENAME(MM,NCI.BookingDate),YEAR(NCI.BookingDate),MONTH(NCI.BookingDate),DAY(NCI.BookingDate) 
END






/****** Object:  StoredProcedure [dbo].[TC_TotalLeadsCount]    Script Date: 08/27/2013 17:40:02 ******/
SET ANSI_NULLS ON
