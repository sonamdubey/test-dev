IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_GetStocksToLink]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_GetStocksToLink]
GO

	-- =============================================
-- Author:		Tejashree Namdeo Patil.
-- Create date: 9 September 2015
-- Description:	To  load stock to link with car
-- EXEC [AbSure_GetStocksToLink] 968
-- Modified By : Kartik Rathod 16 Sept added changes for Pagination.
-- Modified by : Kartik Rathod 7 oct 2015 Fetch only those model who having IsEligibleWarranty=1 
-- =============================================
CREATE PROCEDURE [dbo].[AbSure_GetStocksToLink] 
@DealerId	INT,
@FromIndex INT = NULL,
@ToIndex INT = NULL
AS
BEGIN

	DECLARE @CancelledReasonId	TINYINT = 7, 
			@CancelledReason	VARCHAR(250)

	SELECT  @CancelledReason = Reason
	FROM	AbSure_ReqCancellationReason WITH (NOLOCK)
	WHERE	Id = @CancelledReasonId;

	/*
	All Dealerwise Stocks that are		
		1) Not Requested for inspection
		2) Requested stocks which are not yet inspected
	*/
	WITH CTE
	AS(			
	SELECT	V.Make, V.Model, V.Version, ST.Id StockId, '' CarId,ST.RegNo RegNumber, ST.EntryDate
	FROM	TC_Stock ST WITH (NOLOCK)
			INNER JOIN vwAllMMV V WITH (NOLOCK) ON V.VersionId = ST.VersionId AND V.ApplicationId = 1
			INNER JOIN AbSure_EligibleModels AE WITH(NOLOCK) ON AE.ModelId = V.ModelId
	WHERE	ST.BranchId = @DealerId
			AND ST.IsActive = 1
			AND (ST.IsWarrantyRequested = 0 OR ST.IsWarrantyRequested IS NULL)
			AND ST.StatusId = 1 
			AND ISNULL(AE.IsEligibleWarranty,0) = 1
			AND AE.IsActive = 1
	UNION
	SELECT	CD.Make, CD.Model, Cd.Version, CD.StockId StockId, CONVERT(VARCHAR,CD.Id) CarId,CD.RegNumber, NULL EntryDate
	FROM	AbSure_CarDetails CD WITH (NOLOCK)
			INNER JOIN TC_Stock ST WITH (NOLOCK) ON CD.StockId = ST.Id  
			INNER JOIN vwAllMMV V WITH (NOLOCK) ON V.VersionId = ST.VersionId AND V.ApplicationId = 1 
            INNER JOIN AbSure_EligibleModels AE WITH(NOLOCK) ON AE.ModelId = V.ModelId
	WHERE	CD.StockId IS NOT NULL 
			AND CD.DealerId = @DealerId
			AND (
				 ((CD.Status IN (1,2,9) OR (CD.Status=4 AND CD.IsSoldOut = 0 )) AND DATEDIFF(DAY,CD.SurveyDate,GETDATE()) >30)
				 OR
				 (CD.Status IN (5,6))
				 OR
				 (CD.Status = 3 AND CD.CancelReason <> @CancelledReason) 
				)
			AND CD.IsActive = 1
			AND ISNULL(AE.IsEligibleWarranty,0) = 1    
			AND AE.IsActive = 1 
			AND ST.StatusId = 1            
	)
	
	SELECT *, ROW_NUMBER() OVER(ORDER BY EntryDate DESC) AS NumberForPaging INTO #TblTemp
		FROM CTE 
		

		SELECT * FROM #TblTemp
		WHERE  
        (@FromIndex IS NULL AND @ToIndex IS NULL)
        OR
		(NumberForPaging BETWEEN @FromIndex AND @ToIndex)
		

		SELECT COUNT(CarId) AS RecordCount
        FROM   #TblTemp
		
		DROP TABLE #TblTemp 
END