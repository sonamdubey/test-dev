IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetMonthlySummaryDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetMonthlySummaryDetails]
GO

	-- =============================================
-- Author:		Ashwini Dhamankar
-- Create Date: Feb 9,2016
-- Description:	Fetch monthly summary details
-- @Type :	1 - Leads Assigned
--			2 - Active Leads
--			3 -	Booked Leads
--			4 - Closed Leads
--			5 -	Booked by Co-dealers (Lost to Co-dealers)
--			6 - Booked by other brand dealers (Lost to competition brand)
--			7 - Last month bookings
--			8 - Secondlast month bookings
--			9 - Thirdlast month bookings
-- Modified by : Kritika Choudhary on 5th April 2016, modified output based on date filter, 
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetMonthlySummaryDetails]
@BranchId BIGINT,
@Type INT,
@FromDate DATETIME= NULL,
@ToDate DATETIME= NULL
AS
BEGIN
	SET NOCOUNT ON;
	SET @ToDate = DATEADD(ms, -2, DATEADD(dd, 1, DATEDIFF(dd, 0,@ToDate))) 
	;WITH CTE AS
	(
			SELECT	
			DISTINCT TCNI.TC_NewCarInquiriesId,TCIL.TC_LeadStageId AS TC_LeadStageId
			,TCNI.TC_LeadDispositionId AS TC_LeadDispositionId,TCIL.TC_LeadDispositionID AS TCILDisposition,TCNI.BookingDate AS BookingDate,TCNI.BookingStatus AS BookingStatus
			,(CMK.Name + ' ' + CM.Name + ' ' + CV.Name) AS Car,
			TCL.TC_LeadId,C.CustomerName As CustomerName
			,C.Mobile As Mobile
			,CASE WHEN TCIL.TC_LeadStageId=3 AND ((ISNULL(TCIL.TC_LeadDispositionID,0) NOT IN(1,3,4,70,71,74)) OR (ISNULL(TCIL.TC_LeadDispositionID,0) = 41 AND ISNULL(TCNI.BookingStatus,0) <> 32)) THEN TCNI.TC_LeadDispositionReason ELSE '' END AS LeadDispositionReason
			,(SELECT TOP 1 CL.ActionComments FROM TC_Calls CL WITH(NOLOCK) WHERE CL.TC_LeadId = TCIL.TC_LeadId AND CL.IsActionTaken = 1 AND ISNULL(CL.TC_CallActionId,0) <> 2 AND ISNULL(CL.CallType,0) IN (3) ORDER BY CL.TC_CallsId DESC) AS LastCallComments
			,(SELECT TOP 1 CL.ActionTakenOn FROM TC_Calls CL WITH(NOLOCK) WHERE CL.TC_LeadId = TCIL.TC_LeadId AND CL.IsActionTaken = 1 AND ISNULL(CL.TC_CallActionId,0) <> 2 AND ISNULL(CL.CallType,0) IN (3) ORDER BY CL.TC_CallsId DESC) AS ActionTime
			,TCL.LeadClosedDate AS LeadClosedDate
			,TCNI.CreatedOn CreatedOn
			,CASE	@Type	WHEN 1 THEN 'Assigned'
							WHEN 2 THEN 'Active'
							WHEN 3 THEN 'Booked'
							WHEN 4 THEN 'Closed'
							WHEN 5 THEN 'LostToCo-Dealer'
							WHEN 6 THEN 'LostToOtherBrand'
							WHEN 7 THEN 'Booked'
							WHEN 8 THEN 'Booked'
							WHEN 9 THEN 'Booked'
					END AS Status
			,@Type AS Type
			FROM TC_Lead TCL WITH(NOLOCK)
			INNER JOIN TC_InquiriesLead TCIL WITH(NOLOCK) ON TCIL.TC_LeadId = TCL.TC_LeadId AND TCIL.TC_LeadInquiryTypeId = 3 AND TCIL.BranchId = @BranchId
			INNER JOIN TC_NewCarInquiries TCNI WITH(NOLOCK) ON TCIL.TC_InquiriesLeadId = TCNI.TC_InquiriesLeadId
			LEFT JOIN TC_CustomerDetails C WITH(NOLOCK) ON C.Id = TCL.TC_CustomerId
			LEFT JOIN CarVersions CV WITH (NOLOCK) ON CV.Id = TCNI.VersionId
			LEFT JOIN CarModels CM WITH (NOLOCK) ON CM.ID = CV.CarModelId 
			LEFT JOIN CarMakes CMK WITH (NOLOCK) ON CMK.ID = CM.CarMakeId 
			WHERE 
			(	
				(@Type = 1 AND(TCIL.TC_UserId IS NOT NULL AND TCIL.CreatedDate BETWEEN @FromDate AND @ToDate))   --assigned
				OR(@Type = 2 AND (TCIL.TC_LeadStageId<>3 AND ISNULL(TCIL.TC_LeadDispositionID,0)<>4 AND TCIL.CreatedDate BETWEEN @FromDate AND @ToDate) --active
				OR(@Type = 3 AND (ISNULL(TCNI.TC_LeadDispositionId,0) IN (4,41) AND ISNULL(TCNI.BookingStatus,0) = 32 AND TCNI.BookingDate BETWEEN @FromDate AND @ToDate))  --booked
				--OR(@Type = 4 AND (((TCIL.TC_LeadStageId = 3 AND ISNULL(TCIL.TC_LeadDispositionID,0)<>4 AND ISNULL(TCIL.TC_LeadDispositionID,0) NOT IN(1,3,70,71,74)) OR (ISNULL(TCNI.TC_LeadDispositionId,0) = 4)) AND ((MONTH(TCL.LeadClosedDate) = MONTH(@ToDate) AND YEAR(TCL.LeadClosedDate) = YEAR(@ToDate)) OR (MONTH(TCNI.BookingDate) = MONTH(@ToDate) AND YEAR(TCNI.BookingDate) = YEAR(@ToDate)))
				--					OR (ISNULL(TCNI.TC_LeadDispositionId,0) = 4 AND MONTH(TCNI.BookingDate) = MONTH(@ToDate) AND YEAR(TCNI.BookingDate) = YEAR(@ToDate))
				--					))   --closed

				OR(@Type = 4 AND ((TCIL.TC_LeadStageId = 3 AND ((ISNULL(TCIL.TC_LeadDispositionID,0) NOT IN(1,3,4,70,71,74)) OR (ISNULL(TCIL.TC_LeadDispositionID,0) = 41 AND ISNULL(TCNI.BookingStatus,0) <> 32)) AND TCL.LeadClosedDate  BETWEEN @FromDate AND @ToDate) OR (ISNULL(TCNI.TC_LeadDispositionId,0) IN (4,41) AND ISNULL(TCNI.BookingStatus,0) = 32 AND TCNI.BookingDate BETWEEN @FromDate AND @ToDate))))  --closed
				OR(@Type = 5 AND (ISNULL(TCNI.TC_LeadDispositionId,0) = 65 AND  TCL.LeadClosedDate BETWEEN @FromDate AND @ToDate))  --lost to co dealer
				OR(@Type = 6 AND (ISNULL(TCNI.TC_LeadDispositionId,0) = 64 AND  TCL.LeadClosedDate BETWEEN @FromDate AND @ToDate)) -- lost to competition brand
				--OR(@Type = 7 AND (ISNULL(TCNI.TC_LeadDispositionId,0) IN (4,41) AND ISNULL(TCNI.BookingStatus,0) = 32 AND DATEDIFF(MONTH,TCNI.BookingDate, @ToDate) = 1))  --last month
				--OR(@Type = 8 AND (ISNULL(TCNI.TC_LeadDispositionId,0) IN (4,41) AND ISNULL(TCNI.BookingStatus,0) = 32 AND DATEDIFF(MONTH,TCNI.BookingDate, @ToDate) = 2))  --second last month
				--OR(@Type = 9 AND (ISNULL(TCNI.TC_LeadDispositionId,0) IN (4,41) AND ISNULL(TCNI.BookingStatus,0) = 32 AND DATEDIFF(MONTH,TCNI.BookingDate, @ToDate) = 3))  --third last month
			)
)

SELECT *, ROW_NUMBER() OVER (PARTITION BY TC_LeadId ORDER BY CreatedOn DESC) RowNum INTO #TblTemp FROM Cte
SELECT TC_LeadId,CustomerName,Mobile,Car,LeadDispositionReason,LastCallComments,ActionTime,Type,Status FROM #TblTemp WHERE
(
	(@Type NOT IN (3,4,5,6/*,7,8,9*/) AND RowNum=1)    --fetch lead
	OR(@Type IN (3,5,6/*,7,8,9*/))   -- fetch inquiries
	OR(@Type = 4 AND ((TC_LeadStageId = 3 AND ((ISNULL(TCILDisposition,0) NOT IN(1,3,4,70,71,74)) OR (ISNULL(TCILDisposition,0) = 41 AND ISNULL(BookingStatus,0) <> 32)) AND  LeadClosedDate BETWEEN @FromDate AND @ToDate AND RowNum = 1) OR ((ISNULL(TC_LeadDispositionId,0) = 4) AND BookingDate BETWEEN @FromDate AND @ToDate)))
)
DROP TABLE #TblTemp
END

-------------------------------------

