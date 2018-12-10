IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_GetTransactionsforDealer]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_GetTransactionsforDealer]
GO

	-- =======================================================================================
-- Author	   : Ruchira Patil
-- Create date : 27th April 2015
-- Description : Get credit and debit transactions and umber of inspections for a dealer
-- Modified By : Suresh Prajapati on 05th Nov, 2015
-- Description : Added DealerTypeId check for fetching dealer transaction details
-- =======================================================================================
CREATE PROCEDURE [dbo].[AbSure_GetTransactionsforDealer] -- 5,'03/13/2015','05/06/2015'
	@DealerId INT
	,@StartDate DATETIME
	,@EndDate DATETIME
	--,@FromIndex       INT = NULL, 
	--   @ToIndex         INT = NULL
AS
BEGIN
	DECLARE @OldClosingBal INT
	DECLARE @DealerTypeId INT = NULL

	SELECT @DealerTypeId = TC_DealerTypeId
	FROM Dealers WITH (NOLOCK)
	WHERE ID = @DealerId
		AND IsDealerActive = 1
		AND IsDealerDeleted = 0

	SELECT TOP 1 @OldClosingBal = ClosingAmount
	FROM AbSure_Trans_Logs ATL WITH (NOLOCK)
	LEFT JOIN AbSure_Trans_Debits ATD WITH (NOLOCK) ON ATL.TransactionId = ATD.Id
		AND ATL.TransactionType = 2
	LEFT JOIN AbSure_Trans_Credits ATC WITH (NOLOCK) ON ATL.TransactionId = ATC.Id
		AND ATL.TransactionType = 1
	WHERE cast(logdate AS DATE) < cast(@StartDate AS DATE)
		AND (
			ATD.DealerId = @DealerId
			OR ATC.DealerId = @DealerId
			)
	ORDER BY atl.ID DESC

	--;WITH Cte1 
	--         AS (
	--TRANSACTIONS
	IF (ISNULL(@DealerTypeId, 0) <> 1) -- I.E NOT UCD DEALER, HENCE FETCH DEALER TRASACTION  
	BEGIN
		SELECT CASE 
				WHEN @OldClosingBal IS NULL
					THEN 0
				ELSE @OldClosingBal
				END AS OldOpeningBal
			,ATL.ClosingAmount ClosingBalance
			,ISNULL(CAST(ATC.CreditAmount AS VARCHAR(50)), '-') CreditAmount
			,ISNULL(CONVERT(VARCHAR, ATC.CreditDate, 107), '-') CreditDate
			,ISNULL(CAST(ATD.DebitedAmount AS VARCHAR(50)), '-') DebitedAmount
			,ISNULL(CONVERT(VARCHAR, ATD.DebitDate, 107), '-') DebitDate
			,ISNULL(CAST(ATD.DiscountValue AS VARCHAR(50)), '-') DiscountValue
			,ISNULL(CAST(ATD.ServiceTaxValue AS VARCHAR(50)), '-') ServiceTaxValue
			,ISNULL(ATD.FinalDebitedAmount, 0) FinalDebitedAmount
			,ISNULL(CAST(ACD.StockId AS VARCHAR(50)), '-') StockId
			,ISNULL(WT.Warranty, '-') Warranty
			,CONVERT(VARCHAR, ATL.LogDate, 107) LogDate
			,DENSE_RANK() OVER (
				PARTITION BY CAST(ATL.LogDate AS DATE) ORDER BY AtL.Id
				) SerialNo
			,RANK() OVER (
				ORDER BY CAST(ATL.LogDate AS DATE)
				) RowNo
		FROM AbSure_Trans_Logs ATL WITH (NOLOCK)
		LEFT JOIN AbSure_Trans_Debits ATD WITH (NOLOCK) ON ATL.TransactionId = ATD.Id
			AND ATL.TransactionType = 2
		LEFT JOIN AbSure_Trans_Credits ATC WITH (NOLOCK) ON ATL.TransactionId = ATC.Id
			AND ATL.TransactionType = 1
		LEFT JOIN AbSure_CarDetails ACD WITH (NOLOCK) ON ACD.Id = ATD.CarId
		LEFT JOIN AbSure_WarrantyTypes WT WITH (NOLOCK) ON WT.AbSure_WarrantyTypesId = ACD.FinalWarrantyTypeId
		WHERE (
				ATD.DealerId = @DealerId
				OR ATC.DealerId = @DealerId
				)
			AND CAST(ATL.LogDate AS DATE) BETWEEN CAST(@StartDate AS DATE)
				AND CAST(@EndDate AS DATE)
		ORDER BY CAST(LogDate AS DATE)
	END

	--)              
	----  This is used for pagination 
	--SELECT *, ROW_NUMBER() OVER (ORDER BY CAST(LogDate AS DATE) ) NumberForPaging INTO #TblTemp1 FROM   Cte1 
	--SELECT * FROM #TblTemp1 WHERE (@FromIndex IS NULL AND @ToIndex IS NULL) OR (NumberForPaging  BETWEEN @FromIndex AND @ToIndex )
	--SELECT COUNT(*) AS RecordCount 
	--FROM #TblTemp1 
	--DROP TABLE #TblTemp1 
	--COST INCURRED
	DECLARE @ServiceTaxDateChange DATETIME = '2015-06-01'

	SELECT COUNT(CD.Id) Inspections
		,CONVERT(VARCHAR, CD.SurveyDate, 107) SurveyDate
		,CD.DealerId
		,CASE 
			WHEN CONVERT(VARCHAR, COUNT(CD.Id), 107) IS NULL
				THEN '0'
					--ELSE ISNULL(CONVERT(VARCHAR,COUNT(CD.Id) * 500, 107),0) + ' + ' + CONVERT(VARCHAR,CAST(12.36*ISNULL( COUNT(CD.Id)*500,0)/100 AS DECIMAL(8,2)),107)
			WHEN CAST(CD.SurveyDate AS DATE) >= @ServiceTaxDateChange
				THEN ISNULL(COUNT(CD.Id) * 500, 0) + CAST(14.00 * ISNULL(COUNT(CD.Id) * 500, 0) / 100 AS DECIMAL(8, 2))
			ELSE ISNULL(COUNT(CD.Id) * 500, 0) + CAST(12.36 * ISNULL(COUNT(CD.Id) * 500, 0) / 100 AS DECIMAL(8, 2))
			END CostIncurred
	FROM AbSure_CarDetails CD WITH (NOLOCK)
	WHERE CD.DealerId = @DealerId
		AND CD.IsSurveyDone = 1
		AND CD.FinalWarrantyTypeId IS NULL
		AND CAST(CD.SurveyDate AS DATE) BETWEEN CAST(@StartDate AS DATE)
			AND CAST(@EndDate AS DATE)
	GROUP BY CONVERT(VARCHAR, CD.SurveyDate, 107)
		,CD.DealerId
		,CAST(CD.SurveyDate AS DATE)
	ORDER BY CAST(CONVERT(VARCHAR, CD.SurveyDate, 107) AS DATE)
END
