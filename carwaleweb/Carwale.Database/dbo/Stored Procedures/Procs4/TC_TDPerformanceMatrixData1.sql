IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TDPerformanceMatrixData1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TDPerformanceMatrixData1]
GO

	-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <5th Feb 2016>
-- Description:	<Get dealer TD done count against other dealers in same city>
--TC_TDPerformanceMatrixData 5
-- =============================================
CREATE PROCEDURE [dbo].[TC_TDPerformanceMatrixData1]
	@BranchID INT 
AS
BEGIN

	DECLARE @tblDealerMakes TABLE (MakeId INT)
	DECLARE @tblTDData TABLE (DealerId INT,NewCarInquiriesId INT,CityId INT,TDDate DATETIME)
	DECLARE @startdate DATETIME = Convert(DateTime, DATEDIFF(DAY, 0,DATEADD(M, -4, GETDATE() - DATEPART(D, GETDATE()) + 1))),
	@enddate datetime = DATEADD(MONTH, DATEDIFF(MONTH, -1, GETDATE())-1, -1) ,
	@CityID int
	SET @enddate = DATEADD(MS, -2, DATEADD(D, 1, CONVERT(DATETIME2, @enddate)))
	--GET CITY OF DEALER
	SELECT @CityID = CITYID FROM Dealers WITH(NOLOCK) WHERE ID = @BranchID

	--GET DEALER MAKES
	INSERT INTO @tblDealerMakes (MakeId)
	SELECT MakeId FROM
	TC_DealerMakes DM WITH (NOLOCK) 
	WHERE DM.DealerId = @BranchID

	INSERT INTO @tblTDData(DealerId,NewCarInquiriesId,CityId,TDDate)
	SELECT DISTINCT  D.ID, NI.TC_NewCarInquiriesId,D.CityId,DATEADD(MONTH, DATEDIFF(month, 0, NI.TDDate), 0)TDDate
	FROM  TC_InquiriesLead IL  WITH(NOLOCK)
		  INNER JOIN TC_NewCarInquiries NI WITH(NOLOCK) ON NI.TC_InquiriesLeadId = IL.TC_InquiriesLeadId
		  INNER JOIN Dealers D WITH(NOLOCK) ON D.ID = IL.BranchId
		  INNER JOIN vwAllMMV VW WITH(NOLOCK) ON VW.ApplicationId = D.ApplicationId AND VW.VersionId = NI.VersionId
	WHERE NI.TDDate BETWEEN  @startdate AND  @enddate AND TDStatus = 28 AND IL.ContractId IS NOT NULL 
		  AND VW.MakeId IN (SELECT MakeId FROM @tblDealerMakes);

	WITH CTE AS -- DEALERS COUNT
	(
		SELECT COUNT(TAB.DealerId) TotalDealers,TDDate FROM (
			SELECT DISTINCT  DealerId,TDDate
			FROM @tblTDData
			WHERE CityId = @CityID
			GROUP BY  TDDate ,DealerId)TAB 
		GROUP BY TDDate
	),
	CTE1 AS -- TotalTDDone COUNT
	(
		SELECT count(DISTINCT NewCarInquiriesId) TotalTDDone, TDDate
		FROM @tblTDData
		WHERE CityId = @CityID
		GROUP BY  TDDate
	),
	CTE2 AS -- Total Dealers TDDone
	(
		SELECT count(DISTINCT NewCarInquiriesId) TotalDealersTDDone, TDDate
		FROM @tblTDData
		WHERE DealerId = @BRANCHID
		GROUP BY TDDate
	)

	-- Industry Standard Values
	SELECT  C1.TotalTDDone/C.TotalDealers IndustryStd, 
	C.TDDate AS CreationDate, Month(C.TDDate) Month ,ISNULL(C2.TotalDealersTDDone,0)DealerData
	FROM CTE C WITH(NOLOCK)
	LEFT JOIN CTE1 C1 WITH(NOLOCK) ON C.TDDate = C1.TDDate
	LEFT JOIN CTE2 C2 WITH(NOLOCK) ON C2.TDDate = C.TDDate
	ORDER BY C.TDDate DESC
END