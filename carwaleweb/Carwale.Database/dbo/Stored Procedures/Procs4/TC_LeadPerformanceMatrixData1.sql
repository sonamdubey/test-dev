IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_LeadPerformanceMatrixData1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_LeadPerformanceMatrixData1]
GO

	-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <5th Feb 2016>
-- Description:	<Get dealer lead delivered count against other dealers in same city>
--TC_LeadPerformanceMatrixData1 5
-- =============================================
CREATE PROCEDURE [dbo].[TC_LeadPerformanceMatrixData1]
	@BranchID INT 
AS
BEGIN
	DECLARE @tblDealerMakes TABLE (MakeId INT)
	DECLARE @tblLeadsData TABLE (DealerId INT,InquiriesLeadId INT,CityId INT,CreatedDate DATETIME)
	DECLARE @startdate DATETIME = CONVERT(DATETIME, DATEDIFF(DAY, 0,DATEADD(m, -4, GETDATE() - DATEPART(d, GETDATE()) + 1))),
			@enddate DATETIME = DATEADD(MONTH, DATEDIFF(MONTH, -1, GETDATE())-1, -1) ,
			@CityID int

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


	INSERT INTO @tblLeadsData(DealerId,InquiriesLeadId,CityId,CreatedDate)
	SELECT  D.ID AS DealerId,IL.TC_InquiriesLeadId,D.CityId,DATEADD(MONTH, DATEDIFF(MONTH, 0, IL.CreatedDate), 0) CreatedDate
	FROM	TC_InquiriesLead IL WITH(NOLOCK) 
			INNER JOIN TC_NewCarInquiries NI WITH(NOLOCK) ON NI.TC_InquiriesLeadId = IL.TC_InquiriesLeadId
			INNER JOIN Dealers D WITH(NOLOCK) ON D.ID = IL.BranchId
			INNER JOIN vwAllMMV VW WITH(NOLOCK) ON VW.ApplicationId = D.ApplicationId AND VW.VersionId = NI.VersionId
	WHERE	IL.CreatedDate BETWEEN  @startdate AND  @enddate AND IL.ContractId IS NOT NULL
			AND VW.MakeId IN (SELECT MakeId FROM @tblDealerMakes);

	WITH CTE AS--Dealer count
		(
			SELECT COUNT(DISTINCT TAB.DealerId) TotalDealers,CreatedDate FROM (
				SELECT	DISTINCT DealerId,CreatedDate
				FROM	@tblLeadsData
				WHERE	CityId = @CityID 
				GROUP BY CreatedDate ,DealerId)TAB
			GROUP BY CreatedDate
		),
		CTE1 AS--TotalLeadsDelivered count
		(
			SELECT  count(DISTINCT InquiriesLeadId) TotalLeadsDelivered,CreatedDate
			FROM @tblLeadsData
			WHERE CityId = @CityID
			GROUP BY CreatedDate
		),
		CTE2 AS--DealerLeadsDelivered count
		(
			SELECT count(DISTINCT InquiriesLeadId) DealerLeadsDelivered,CreatedDate
			FROM @tblLeadsData
			WHERE DealerId = @BRANCHID
			GROUP BY CreatedDate
		)
		--IndustryStd base values
		SELECT  C1.TotalLeadsDelivered/C.TotalDealers IndustryStd,
		C.CreatedDate AS CreationDate, Month(C.CreatedDate) Month ,ISNULL(C2.DealerLeadsDelivered,0)DealerData
		FROM CTE C WITH(NOLOCK)
		LEFT JOIN CTE1 C1 WITH(NOLOCK) ON C.CreatedDate = C1.CreatedDate
		LEFT JOIN CTE2 C2 WITH(NOLOCK)  ON C2.CreatedDate = C.CreatedDate
		ORDER BY C.CreatedDate DESC
END
