IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_LoadInspectedCarDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_LoadInspectedCarDetails]
GO

	
-- =============================================
-- Author:		Tejashree Namdeo Patil.
-- Create date: 9 September 2015
-- Description:	To load dealerwise inspection tab details which are added by add car feature i.e. null stockId
-- EXEC AbSure_LoadInspectedCarDetails  968,null,null
-- Modified by : Kartik Rathod on 15sept,2015 added changes for pagination,added IsDuplicateCar,IsCertificateExpired
-- Modified By : Kartik Rathod on 27 oct 2015, fetch only those car whose entry not in Absure_ActivatedWarranty Table.
-- =============================================
CREATE PROCEDURE [dbo].[AbSure_LoadInspectedCarDetails]
@DealerId	INT,
@FromIndex   INT = NULL, 
@ToIndex     INT = NULL

AS
BEGIN

	DECLARE @CancelledReasonId	TINYINT = 7, 
			@CancelledReason	VARCHAR(250)

	SELECT  @CancelledReason = Reason
	FROM	AbSure_ReqCancellationReason WITH (NOLOCK)
	WHERE	Id = @CancelledReasonId;

	/*
	All Dealerwise cars that are		
		1) Added by "Add Car" Feature from App (StockId IS NULL) 
		2) Non inspected Cars Also Cancelled cars because of "Duplicate Cars" Reason
		3) OrigionalCarId is the carId of already inspected car in case of duplicate car  
		4)all cars in the verification pending should not come for linking stock
	*/

		SELECT	ACD.Id AS CarId, 
		dbo.Absure_GetMasterCarId(ACD.RegNumber,ACD.Id) AS OriginalCarId,
		ACD1.AbSureWarrantyActivationStatusesId AS OriginalAbSureWarrantyActivationStatusesId
		INTO #OriginalCarTbl 
		FROM AbSure_CarDetails ACD WITH(NOLOCK)																				-- ACD is the data of duplicate car
			LEFT JOIN AbSure_CarDetails ACD1 WITH(NOLOCK) ON ACD1.Id = dbo.Absure_GetMasterCarId(ACD.RegNumber,ACD.Id)      -- here ACD1 is the Table with Original Cars Data
		WHERE ACD.DealerId = @DealerId AND ACD.Status = 3 AND ACD.CancelReason = @CancelledReason		
	
	--select * from #OriginalCarTbl
	
	;WITH CTE
	AS(
	SELECT	CD.Id CarId,
			CASE WHEN CD.Status = 3 AND CD.CancelReason LIKE @CancelledReason THEN dbo.Absure_GetMasterCarId(CD.RegNumber,CD.Id) ELSE CD.Id  END OriginalCarId,
			CASE WHEN CD.CancelReason = @CancelledReason THEN 'True' ELSE 'False' END IsDuplicateCar, 
			CD.Make, CD.Model, CD.Version, CD.RegNumber, CD.Status,
			CASE WHEN  DATEDIFF(DAY,CD.SurveyDate,GETDATE()) >30 THEN 1 ELSE 0 END IsCertificateExpired,
			ROW_NUMBER() OVER(ORDER BY CD.EntryDate DESC) AS NumberForPaging
	FROM	AbSure_CarDetails CD  WITH (NOLOCK)
			LEFT JOIN AbSure_ActivatedWarranty AW WITH (NOLOCK) ON CD.Id = AW.AbSure_CarDetailsId
			LEFT JOIN #OriginalCarTbl OT WITH(NOLOCK) ON CD.Id = OT.CarId
	WHERE	CD.StockId IS NULL
			AND AW.AbSure_CarDetailsId IS NULL									--fetch only those car whose entry not in Absure_ActivatedWarranty Table.
			AND (
					(CD.Status = 3 AND CD.IsCancelled = 1 AND CD.CancelReason LIKE @CancelledReason AND OT.OriginalCarId NOT IN (SELECT AbSure_CarDetailsId FROM AbSure_ActivatedWarranty WITH(NOLOCK) WHERE AbSure_CarDetailsId = OT.OriginalCarId)  AND ISNULL(OT.OriginalAbSureWarrantyActivationStatusesId,0) <> 1) 
					OR
					(CD.IsSurveyDone = 1)
				)
			AND CD.DealerId = @DealerId
			AND CD.IsActive=1
			AND (ISNULL(CD.AbSureWarrantyActivationStatusesId,0) <> 1)
	)
	SELECT * INTO #TblTemp
		FROM CTE

		SELECT * FROM #TblTemp
		WHERE  
        (@FromIndex IS NULL AND @ToIndex IS NULL)
        OR
		(NumberForPaging BETWEEN @FromIndex AND @ToIndex)

		SELECT COUNT(CarId) AS RecordCount
        FROM   #TblTemp
		
		DROP TABLE #TblTemp 
		DROP TABLE #OriginalCarTbl


END