IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetTestDriveCounts]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetTestDriveCounts]
GO

	-- ==============================================================================================
-- Author		: Suresh Prajapati
-- Created On	: 01st April, 2016
-- Description  : 1. This SP returns the Pending Test Drive counts for specified dealer and/or user
--				  2. If @FromDate and @ToDate is same, then this case will return Pending counts for that day
--				  3. Pass @FromDate and @ToDate as Today's Date to get Scheduled Test Drive Counts for Today
-- EXEC TC_GetTestDriveCounts 5,243,'2016-04-01'
-- ==============================================================================================
CREATE PROCEDURE [dbo].[TC_GetTestDriveCounts] @BranchId INT
	,@UserId INT = NULL
	,@FromDate DATE = NULL
	,@ToDate DATE = NULL
AS
BEGIN
	SELECT DISTINCT COUNT(C.TC_TDCalendarId) AS PendingCount
	FROM TC_TDCalendar AS C WITH (NOLOCK)
	INNER JOIN TC_InquirySource S WITH (NOLOCK) ON C.TC_SourceId = S.Id
	INNER JOIN TC_CustomerDetails CD WITH (NOLOCK) ON C.TC_CustomerId = CD.Id
	INNER JOIN TC_Users U WITH (NOLOCK) ON C.TC_UsersId = U.Id
	LEFT JOIN TC_TDCars TC WITH (NOLOCK) ON TC.TC_TDCarsId = C.TC_TDCarsId
	LEFT JOIN TC_NewCarInquiries NC WITH (NOLOCK) ON NC.TC_TDCalendarId = C.TC_TDCalendarId
	WHERE C.TDStatus IN (
			29 -- Rescheduled
			,39 -- Confirmed
			)
		AND C.BranchId = @BranchId -- Filter For Dealer
		AND (
			@UserId IS NULL
			OR C.TC_UsersId = @UserId -- Filter for User (or Sales Executive)
			)
		AND (
			@FromDate IS NULL
			OR CONVERT(DATE, C.TDDate) >= @FromDate
			)
		AND (
			--@ToDate IS NULL
			--OR 
			CONVERT(DATE, C.TDDate) <= ISNULL(@ToDate, GETDATE())
			)
END

