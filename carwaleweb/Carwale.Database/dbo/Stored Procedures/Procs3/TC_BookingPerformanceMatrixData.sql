IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_BookingPerformanceMatrixData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_BookingPerformanceMatrixData]
GO

	-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <8th Feb 2016>
-- Description:	<Get dealer bookings count against other dealers in same city>
--TC_BookingPerformanceMatrixData1 5
-- Modified By : Nilima More On 11 th Feb 2016 (Added Condition "Applicationod =1")
-- =============================================
CREATE PROCEDURE [dbo].[TC_BookingPerformanceMatrixData]
	@BranchID INT 
AS
BEGIN
	DECLARE @tblDealerMakes TABLE (MakeId INT)
	DECLARE @tblTDData TABLE (DealerId INT,NewCarInquiriesId INT,CityId INT,BookingDate DATETIME)
	DECLARE @startdate DATETIME = CONVERT(DATETIME, DATEDIFF(DAY, 0,DATEADD(m, -4, GETDATE() - DATEPART(d, GETDATE()) + 1))),
			@CityID int,
			@enddate DATETIME = DATEADD(MONTH, DATEDIFF(MONTH, -1, GETDATE())-1, -1)
			
	SET @enddate = DATEADD(MS, -2, DATEADD(D, 1, CONVERT(DATETIME2, @enddate)))
	--GET CITY OF DEALER
	SELECT	@CityID = CITYID 
	FROM	Dealers WITH(NOLOCK) 
	WHERE	ID = @BranchID

	--GET DEALER MAKES
	INSERT INTO @tblDealerMakes (MakeId)
	SELECT MakeId FROM
	TC_DealerMakes DM WITH (NOLOCK) 
	WHERE DM.DealerId = @BranchID

	INSERT INTO @tblTDData(DealerId,NewCarInquiriesId,CityId,BookingDate)
	SELECT DISTINCT  D.ID, NI.TC_NewCarInquiriesId ,D.CityId,DATEADD(month, DATEDIFF(month, 0, NI.BookingDate), 0)BookingDate
	FROM  TC_InquiriesLead IL  WITH(NOLOCK)
		  INNER JOIN TC_NewCarInquiries NI WITH(NOLOCK) ON NI.TC_InquiriesLeadId = IL.TC_InquiriesLeadId
		  INNER JOIN Dealers D WITH(NOLOCK) ON D.ID = IL.BranchId
		  INNER JOIN vwAllMMV VW WITH(NOLOCK) ON VW.ApplicationId = D.ApplicationId AND VW.VersionId = NI.VersionId
	WHERE NI.BookingDate BETWEEN  @startdate AND  @enddate AND BookingStatus = 32 AND IL.ContractId IS NOT NULL 
		  AND VW.MakeId IN (SELECT MakeId FROM @tblDealerMakes) AND D.ApplicationId =1 AND D.ID NOT IN (3838,4271,16538);

	WITH CTE AS -- DEALERS COUNT
	(
		SELECT COUNT(TAB.DealerId) TotalDealers,BookingDate FROM (
			SELECT DISTINCT  DealerId,BookingDate
			FROM @tblTDData
			WHERE CityId = @CityID
			GROUP BY  BookingDate ,DealerId)TAB 
		GROUP BY BookingDate
	),
	CTE1 AS -- TotalBookingsDone COUNT
	(
		SELECT COUNT(DISTINCT NewCarInquiriesId) TotalBookingsDone, BookingDate
		FROM @tblTDData
		WHERE CityId = @CityID
		GROUP BY  BookingDate
	),
	CTE2 AS -- Dealers Total Bookings COUNT
	(
		SELECT COUNT(DISTINCT NewCarInquiriesId) TotalDealersBookingsDone, BookingDate
		FROM @tblTDData
		WHERE DealerId = @BRANCHID
		GROUP BY BookingDate
	)
	SELECT  C1.TotalBookingsDone/C.TotalDealers IndustryStd, 
	C.BookingDate AS CreationDate, Month(C.BookingDate) Month ,ISNULL(C2.TotalDealersBookingsDone,0)DealerData
	FROM CTE C WITH(NOLOCK)
	LEFT JOIN CTE1 C1 WITH(NOLOCK) ON C.BookingDate = C1.BookingDate
	LEFT JOIN CTE2 C2 WITH(NOLOCK) ON C2.BookingDate = C.BookingDate
	ORDER BY C.BookingDate DESC
END
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

