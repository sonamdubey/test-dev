IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_BookedCountLeadWiseFetch]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_BookedCountLeadWiseFetch]
GO

	--	Modified By		:	Sachin Bharti(21st June 2013) Added filter on Date range
-- Modified By: Nilesh Utture on 18th July, 2013 Changed size 0f @ReportingUsersList to MAX	
-- Modified By: Nilesh Utture on 31st July, 2013 Fetched Lead count on the basis of IL.CreatedDate
CREATE  PROCEDURE [dbo].[TC_BookedCountLeadWiseFetch]
@ModelId  NUMERIC (18,0),
@Versionid NUMERIC (18,0),
@BranchId  NUMERIC (18,0),
@FromDate	DateTime = NULL,
@EndDate	DateTime = NULL,
@fuelType  INT,

@ReportingUsersList VARCHAR(MAX) = '-1'  -- '-1': No user is reporting to logged in user, -- Modified By: Nilesh Utture on 18th July, 2013 Changed size 0f @ReportingUsersList to MAX	
										 --	NULL: in case if logged in user is Admin, 
										 -- Else comma separated user List
AS 
BEGIN
	--- query for Booked lead for all model
	--- this part of the querry is required to show the booked lead independent of model, version and fuelType
	
	SELECT CONVERT(CHAR(3),DATENAME(MM,IL.CreatedDate)) AS Month,COUNT(DISTINCT IL.TC_LeadId) Booked,
	YEAR(IL.CreatedDate) AS Year,MONTH(IL.CreatedDate) AS MonthId ,DAY(IL.CreatedDate) AS Day 
	FROM TC_NewCarInquiries NCI WITH(NOLOCK)
		INNER JOIN TC_InquiriesLead IL WITH(NOLOCK) ON IL.TC_InquiriesLeadId = NCI.TC_InquiriesLeadId
		INNER JOIN TC_Lead L WITH(NOLOCK) ON IL.TC_LeadId = L.TC_LeadId
		INNER JOIN CRM.vwMMV V ON NCI.VersionId=V.VersionId
	WHERE IL.TC_LeadInquiryTypeId=3 
		AND Year(IL.CreatedDate) = YEAR(GETDATE())
		--AND NCI.BookingStatus= 32-- for Booking Completed
		AND IL.BranchId = @BranchId
		AND (IL.TC_UserId IN (SELECT ListMember AS UserId FROM fnSplitCSV(@ReportingUsersList)) OR @ReportingUsersList IS NULL) -- Modified By: Nilesh Utture on 19th June, 2013
		--Modified By Sachin Bharti(21st Jun 2013 )
		AND IL.CreatedDate BETWEEN @FromDate AND @EndDate-- Modified By: Nilesh Utture on 31st July, 2013
	GROUP BY DATENAME(MM,IL.CreatedDate),YEAR(IL.CreatedDate),MONTH(IL.CreatedDate),DAY(IL.CreatedDate) 
	
	--- query for Booked lead for selected model and version
	
	SELECT CONVERT(CHAR(3),DATENAME(MM,IL.CreatedDate)) AS Month,COUNT(DISTINCT IL.TC_LeadId) BookedCar,
	YEAR(IL.CreatedDate) AS Year,MONTH(IL.CreatedDate) AS MonthId,DAY(IL.CreatedDate) AS Day  
	FROM TC_NewCarInquiries NCI WITH(NOLOCK)
		INNER JOIN TC_InquiriesLead IL WITH(NOLOCK) ON IL.TC_InquiriesLeadId = NCI.TC_InquiriesLeadId
		INNER JOIN TC_Lead L WITH(NOLOCK) ON IL.TC_LeadId = L.TC_LeadId
		INNER JOIN CRM.vwMMV V ON NCI.VersionId=V.VersionId
	WHERE IL.TC_LeadInquiryTypeId=3 
		AND Year(IL.CreatedDate) = YEAR(GETDATE()) 
		--AND NCI.BookingStatus= 32-- for Booking Completed
		AND IL.BranchId = @BranchId  
		AND (IL.TC_UserId IN (SELECT ListMember AS UserId FROM fnSplitCSV(@ReportingUsersList)) OR @ReportingUsersList IS NULL) -- Modified By: Nilesh Utture on 19th June, 2013
		AND ( @ModelId IS NULL OR V.ModelId=@ModelId)
		AND (@Versionid IS NULL OR V.VersionId=@Versionid) 
		AND(@fuelType IS NULL OR V.CarFuelType=@fuelType)
		--Modified By Sachin Bharti(21st Jun 2013 )
		AND IL.CreatedDate BETWEEN @FromDate AND @EndDate-- Modified By: Nilesh Utture on 31st July, 2013
	GROUP BY DATENAME(MM,IL.CreatedDate),YEAR(IL.CreatedDate),MONTH(IL.CreatedDate),DAY(IL.CreatedDate)
	
	
END






/****** Object:  StoredProcedure [dbo].[TC_InquiryDetailsNewCar]    Script Date: 08/05/2013 11:26:42 ******/
SET ANSI_NULLS ON
