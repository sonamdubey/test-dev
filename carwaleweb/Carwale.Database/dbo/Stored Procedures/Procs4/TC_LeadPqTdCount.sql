IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_LeadPqTdCount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_LeadPqTdCount]
GO

	-- Modified By: Nilesh Utture on 19th June, 2013 Data is only populated based on the users(@ReportingUsersList) who report to the logged in user
-- Modified By: Nilesh Utture on 18th July, 2013 Changed size 0f @ReportingUsersList to MAX	
-- Modified By: Nilesh Utture 0n 31st July, 2013 Shown the Lead count based on Lead creation date, and PQ, TD on the basis of their event creation date
-- Modified By: Nilima More  0n 23rd May, 2016,added condition for new car Inquiry,L.TC_LeadInquiryTypeId=3 .
CREATE  PROCEDURE [dbo].[TC_LeadPqTdCount] 
@ModelId	NUMERIC (18,0),
@Versionid	NUMERIC (18,0),
@BranchId	NUMERIC (18,0),
@fuelType	INT,
@StartDate	DATETIME,
@EndDate	DATETIME,	
@ReportingUsersList VARCHAR (MAX) = '-1' -- '-1': No user is reporting to logged in user, -- Modified By: Nilesh Utture on 18th July, 2013 Changed size 0f @ReportingUsersList to MAX	
										 --	NULL: in case if logged in user is Admin, 
										 -- Else comma separated user List
AS 
BEGIN
		--- query to get total  inquiries
	SELECT CONVERT(CHAR(3),DATENAME(MM,L.CreatedDate)) AS Mnth,COUNT(DISTINCT L.TC_InquiriesLeadId) totalLead, 
	       YEAR(L.CreatedDate) AS Year, CONVERT(VARCHAR(5),DAY(L.CreatedDate)) AS Day, CONVERT(VARCHAR(3),CONVERT(VARCHAR(3), DATENAME(MM, L.CreatedDate), 100)) AS MonthId,
	       MONTH(L.CreatedDate) AS Month 
	FROM  TC_NewCarInquiries I WITH (NOLOCK) 
		INNER JOIN TC_InquiriesLead L WITH (NOLOCK) ON I.TC_InquiriesLeadId=L.TC_InquiriesLeadId --AND L.TC_LeadInquiryTypeId=3 
		INNER JOIN vwMMV V WITH(NOLOCK) on I.VersionId=V.VersionId
	WHERE L.BranchId = @BranchId 
		AND (L.TC_UserId IN	(SELECT ListMember AS UserId FROM fnSplitCSV(@ReportingUsersList)) OR @ReportingUsersList IS NULL) -- Modified By: Nilesh Utture on 19th June, 2013 
		AND (@ModelId IS NULL OR V.ModelId=@ModelId) 	
		AND (@Versionid IS NULL OR V.VersionId=@Versionid)
		AND(@fuelType IS NULL OR V.CarFuelType=@fuelType)
		AND L.CreatedDate BETWEEN @StartDate AND @EndDate -- Modified By: Nilesh Utture 0n 31st July, 2013 
		AND L.TC_LeadInquiryTypeId=3  -- Modified By: Nilima More  0n 23rd May, 2016. 
	GROUP BY MONTH(L.CreatedDate) ,YEAR(L.CreatedDate),DATENAME(MM,L.CreatedDate),
		CONVERT(VARCHAR(5),DAY(L.CreatedDate)) , CONVERT(VARCHAR(3),CONVERT(VARCHAR(3), DATENAME(MM, L.CreatedDate), 100))

	--- query to get total priceQuote Completed
	SELECT CONVERT(CHAR(3),DATENAME(MM,NCI.PQDate)) AS Mnth,COUNT(DISTINCT NCI.TC_NewCarInquiriesId) PriceQuote,
	YEAR(NCI.PQDate) AS Year,CONVERT(VARCHAR(5),DAY(NCI.PQDate)) AS Day, CONVERT(VARCHAR(3),CONVERT(VARCHAR(3), DATENAME(MM, NCI.PQDate), 100)) AS MonthId,
	       MONTH(NCI.PQDate) AS Month 
	FROM TC_NewCarInquiries NCI WITH(NOLOCK)
		INNER JOIN TC_InquiriesLead IL WITH(NOLOCK) ON IL.TC_InquiriesLeadId = NCI.TC_InquiriesLeadId
		INNER JOIN vwMMV V WITH(NOLOCK) ON NCI.VersionId=V.VersionId
	WHERE IL.TC_LeadInquiryTypeId=3 
		AND Year(NCI.PQDate) = YEAR(GETDATE()) 
		AND NCI.PQStatus= 25 -- for PQ completed
		AND IL.BranchId = @BranchId 
		AND (IL.TC_UserId IN	(SELECT ListMember AS UserId FROM fnSplitCSV(@ReportingUsersList)) OR @ReportingUsersList IS NULL) -- Modified By: Nilesh Utture on 19th June, 2013 
		AND ( @ModelId IS NULL OR V.ModelId=@ModelId) 
		AND (@Versionid IS NULL OR V.VersionId=@Versionid)
		AND(@fuelType IS NULL OR V.CarFuelType=@fuelType)
		AND NCI.PQDate BETWEEN @StartDate AND @EndDate -- Modified By: Nilesh Utture 0n 31st July, 2013 
	GROUP BY DATENAME(MM,NCI.PQDate),YEAR(NCI.PQDate),MONTH(NCI.PQDate),CONVERT(VARCHAR(5),DAY(NCI.PQDate)) , CONVERT(VARCHAR(3),CONVERT(VARCHAR(3), DATENAME(MM, NCI.PQDate), 100)) 

	--- query for Completed test Drives
	SELECT CONVERT(CHAR(3),DATENAME(MM,NCI.TDDate)) AS Mnth,COUNT(DISTINCT NCI.TC_NewCarInquiriesId) TestDrive,
	YEAR(NCI.TDDate) AS Year,CONVERT(VARCHAR(5),DAY(NCI.TDDate)) AS Day, CONVERT(VARCHAR(3),CONVERT(VARCHAR(3), DATENAME(MM, NCI.TDDate), 100)) AS MonthId,
	       MONTH(NCI.TDDate) AS Month 
	FROM TC_NewCarInquiries NCI WITH(NOLOCK)
		INNER JOIN TC_InquiriesLead IL WITH(NOLOCK) ON IL.TC_InquiriesLeadId = NCI.TC_InquiriesLeadId
		INNER JOIN vwMMV V WITH(NOLOCK) ON NCI.VersionId=V.VersionId
	WHERE IL.TC_LeadInquiryTypeId=3 
		AND Year(NCI.TDDate) = YEAR(GETDATE()) 
		AND NCI.TDStatus= 28-- for TD Completed
		AND IL.BranchId = @BranchId 
		AND (IL.TC_UserId IN	(SELECT ListMember AS UserId FROM fnSplitCSV(@ReportingUsersList)) OR @ReportingUsersList IS NULL) -- Modified By: Nilesh Utture on 19th June, 2013 
		AND ( @ModelId IS NULL OR V.ModelId=@ModelId) 
		AND (@Versionid IS NULL OR V.VersionId=@Versionid)
		AND(@fuelType IS NULL OR V.CarFuelType=@fuelType)
		AND NCI.TDDate BETWEEN @StartDate AND @EndDate -- Modified By: Nilesh Utture 0n 31st July, 2013 
	GROUP BY DATENAME(MM,NCI.TDDate),YEAR(NCI.TDDate),MONTH(NCI.TDDate),CONVERT(VARCHAR(5),DAY(NCI.TDDate)) , CONVERT(VARCHAR(3),CONVERT(VARCHAR(3), DATENAME(MM, NCI.TDDate), 100)) 
END