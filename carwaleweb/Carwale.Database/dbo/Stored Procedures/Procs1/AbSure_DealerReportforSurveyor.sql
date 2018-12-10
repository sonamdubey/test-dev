IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_DealerReportforSurveyor]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_DealerReportforSurveyor]
GO

	-- =============================================
-- Author:		Ruchira Patil
-- Create date: 25th May 2015
-- Description:	To get the dealer report for a surveyor
-- exec AbSure_DealerReportforSurveyor 42,1,null,null
-- =============================================
CREATE PROCEDURE [dbo].[AbSure_DealerReportforSurveyor] @UserId INT
	,@Type INT
	,--1-REALTIME REPORT || 2-CONSOLIDATED REPORT
	@StartDate DATETIME = NULL
	,@EndDate DATETIME = NULL
AS
BEGIN
	IF @Type = 1 --REALTIME REPORT
	BEGIN
		DECLARE @TempRealtimeDataTbl TABLE (
			DealerId INT
			,SurveyorPending INT
			,InspectionPending INT
			,WarrantyActivated INT
			)

		INSERT INTO @TempRealtimeDataTbl (
			DealerId
			,SurveyorPending
			,InspectionPending
			,WarrantyActivated
			)
		SELECT Tbl1.DealerId
			,Tbl1.SurveyorPending
			,Tbl1.InspectionPending
			,Tbl2.WarrantyActivated
		FROM (
			SELECT UD.DealerId AS DealerId
				,SUM(CASE 
						WHEN ACD.STATUS = 6
							AND CAST(CSM.EntryDate AS DATE) = CAST(GETDATE() AS DATE)
							THEN 1
						ELSE 0
						END) AS SurveyorPending
				,SUM(CASE 
						WHEN ACD.STATUS = 5
							AND CAST(CSM.UpdatedOn AS DATE) = CAST(GETDATE() AS DATE)
							THEN 1
						ELSE 0
						END) AS InspectionPending
			FROM OprUsers OU WITH (NOLOCK)
			INNER JOIN DCRM_ADM_UserDealers UD WITH (NOLOCK) ON UD.UserId = OU.Id
			LEFT JOIN AbSure_CarDetails ACD WITH (NOLOCK) ON ACD.DealerId = UD.DealerId
			INNER JOIN AbSure_CarSurveyorMapping CSM WITH (NOLOCK) ON ACD.Id = CSM.AbSure_CarDetailsId
			WHERE OU.ID = @UserId
				AND UD.RoleId IN (
					3
					,5
					)
			GROUP BY UD.DealerId
			) AS Tbl1
		JOIN (
			SELECT UD.DealerId AS DealerId
				,SUM(CASE ACD.STATUS
						WHEN 8
							THEN 1
						ELSE 0
						END) AS WarrantyActivated
			FROM OprUsers OU WITH (NOLOCK)
			INNER JOIN DCRM_ADM_UserDealers UD WITH (NOLOCK) ON UD.UserId = OU.Id
			LEFT JOIN AbSure_CarDetails ACD WITH (NOLOCK) ON ACD.DealerId = UD.DealerId
			INNER JOIN AbSure_ActivatedWarranty AW WITH (NOLOCK) ON AW.AbSure_CarDetailsId = ACD.Id
			WHERE OU.ID = @UserId
				AND UD.RoleId IN (
					3
					,5
					)
				AND CAST(AW.ActivationDate AS DATE) = CAST(GETDATE() AS DATE)
			GROUP BY UD.DealerId
			) AS Tbl2 ON Tbl1.DealerId = Tbl2.DealerId

		SELECT DISTINCT UD.DealerId DealerId
			,ISNULL(TCD.InspectionPending, 0) InspectionPending
			,ISNULL(TCD.SurveyorPending, 0) SurveyorPending
			,ISNULL(TCD.WarrantyActivated, 0) WarrantyActivated
			,SUM(CASE 
					WHEN ACD.STATUS IS NULL
						AND CAST(ACD.EntryDate AS DATE) = CAST(GETDATE() AS DATE)
						THEN 1
					ELSE 0
					END) AgencyPending
			,SUM(CASE 
					WHEN (
							ACD.IsSurveyDone = 1
							AND ACD.FinalWarrantyDate IS NULL
							AND ISNULL(ACD.IsRejected, 0) = 0
							)
						AND CAST(ACD.SurveyDate AS DATE) = CAST(GETDATE() AS DATE)
						THEN 1
					ELSE 0
					END) ApprovalPending
			,SUM(CASE 
					WHEN ACD.STATUS = 4
						AND (CAST(ACD.FinalWarrantyDate AS DATE) = CAST(GETDATE() AS DATE))
						THEN 1
					ELSE 0
					END) WarrantiesApproved
			,SUM(CASE 
					WHEN ISNULL(AW.IsActivated, 0) = 0
						AND CAST(AW.EntryDate AS DATE) = CAST(GETDATE() AS DATE)
						THEN 1
					ELSE 0
					END) WarrantyActivationPending
			,SUM(CASE 
					WHEN ACD.IsInspectionRescheduled = 1
						AND CAST(AP.EntryDate AS DATE) = CAST(GETDATE() AS DATE)
						THEN 1
					ELSE 0
					END) Rescheduled
		FROM OprUsers OU WITH (NOLOCK)
		INNER JOIN DCRM_ADM_UserDealers UD WITH (NOLOCK) ON UD.UserId = OU.Id
		LEFT JOIN AbSure_CarDetails ACD WITH (NOLOCK) ON ACD.DealerId = UD.DealerId
		LEFT JOIN AbSure_ActivatedWarranty AW WITH (NOLOCK) ON ACD.ID = AW.AbSure_CarDetailsId
		LEFT JOIN Absure_Appointments AP WITH (NOLOCK) ON ACD.ID = AP.AbsureCarId
		LEFT JOIN @TempRealtimeDataTbl TCD ON TCD.DealerId = ACD.DealerId
		WHERE OU.ID = @UserId
			AND UD.RoleId IN (
				3
				,5
				)
		GROUP BY UD.DealerId
			,TCD.InspectionPending
			,TCD.SurveyorPending
			,TCD.WarrantyActivated
	END
	ELSE -- CONSOLIDATED REPORT
	BEGIN
		DECLARE @TempConsolidatedDataTbl TABLE (
			DealerId INT
			,EligibleCar INT
			,WarrantyActivated INT
			)

		INSERT INTO @TempConsolidatedDataTbl (
			DealerId
			,EligibleCar
			,WarrantyActivated
			)
		SELECT Tbl1.DealerId
			,Tbl1.EligibleCar
			,Tbl2.WarrantyActivated
		FROM (
			SELECT UD.DealerId DealerId
				,COUNT(ST.Id) EligibleCar
			FROM OprUsers OU WITH (NOLOCK)
			INNER JOIN DCRM_ADM_UserDealers UD WITH (NOLOCK) ON UD.UserId = OU.Id
			INNER JOIN TC_Stock ST WITH (NOLOCK) ON ST.BranchId = UD.DealerId
			INNER JOIN CarVersions V WITH (NOLOCK) ON V.ID = ST.VersionId
			INNER JOIN AbSure_EligibleModels AE WITH (NOLOCK) ON AE.ModelId = V.CarModelId
			INNER JOIN TC_CarCondition CC WITH (NOLOCK) ON CC.StockId = ST.Id
			WHERE OU.ID = @UserId
				AND UD.RoleId IN (
					3
					,5
					)
				AND ST.Kms <= 98000
				AND (
					DATEDIFF(MONTH, ST.MakeYear, GETDATE()) >= 20
					AND DATEDIFF(MONTH, ST.MakeYear, GETDATE()) <= 84
					)
				AND V.CarFuelType IN (
					1
					,2
					,3
					)
				AND CC.Owners <> 0
				AND AE.IsActive = 1
				AND ISNULL(AE.IsEligibleWarranty, 1) = 1
				AND CAST(ST.EntryDate AS DATE) BETWEEN CAST(@StartDate AS DATE)
					AND CAST(@EndDate AS DATE)
			GROUP BY UD.DealerId
			) AS Tbl1
		JOIN (
			SELECT UD.DealerId DealerId
				,SUM(CASE ACD.STATUS
						WHEN 8
							THEN 1
						ELSE 0
						END) AS WarrantyActivated
			FROM OprUsers OU WITH (NOLOCK)
			INNER JOIN DCRM_ADM_UserDealers UD WITH (NOLOCK) ON UD.UserId = OU.Id
			LEFT JOIN AbSure_CarDetails ACD WITH (NOLOCK) ON ACD.DealerId = UD.DealerId
			INNER JOIN AbSure_ActivatedWarranty AW WITH (NOLOCK) ON AW.AbSure_CarDetailsId = ACD.Id
			WHERE OU.ID = @UserId
				AND UD.RoleId IN (
					3
					,5
					)
				AND CAST(AW.ActivationDate AS DATE) BETWEEN CAST(@StartDate AS DATE)
					AND CAST(@EndDate AS DATE)
			GROUP BY UD.DealerId
			) AS Tbl2 ON Tbl1.DealerId = Tbl2.DealerId

		SELECT DISTINCT UD.DealerId DealerId
			,ISNULL(TCD.EligibleCar, 0) EligibleCar
			,ISNULL(TCD.WarrantyActivated, 0) WarrantyActivated
			,SUM(CASE 
					WHEN CAST(ACD.EntryDate AS DATE) BETWEEN CAST(@StartDate AS DATE)
							AND CAST(@EndDate AS DATE)
						THEN 1
					ELSE 0
					END) RequestedCars
			,SUM(CASE 
					WHEN ACD.STATUS = 4
						AND (
							CAST(ACD.FinalWarrantyDate AS DATE) BETWEEN CAST(@StartDate AS DATE)
								AND CAST(@EndDate AS DATE)
							)
						THEN 1
					ELSE 0
					END) WarrantiesApproved
			,SUM(CASE 
					WHEN ACD.STATUS = 3
						AND (
							CAST(ACD.CancelledOn AS DATE) BETWEEN CAST(@StartDate AS DATE)
								AND CAST(@EndDate AS DATE)
							)
						THEN 1
					ELSE 0
					END) Cancelled
			,SUM(CASE 
					WHEN ACD.STATUS = 2
						AND (
							CAST(ACD.RejectedDateTime AS DATE) BETWEEN CAST(@StartDate AS DATE)
								AND CAST(@EndDate AS DATE)
							)
						THEN 1
					ELSE 0
					END) Rejected
			,SUM(CASE 
					WHEN ACD.STATUS = 7
						AND (
							CAST(DATEADD(DD, 30, ACD.SurveyDate) AS DATE) BETWEEN CAST(@StartDate AS DATE)
								AND CAST(@EndDate AS DATE)
							)
						THEN 1
					ELSE 0
					END) Expired
		FROM OprUsers OU WITH (NOLOCK)
		INNER JOIN DCRM_ADM_UserDealers UD WITH (NOLOCK) ON UD.UserId = OU.Id
		LEFT JOIN AbSure_CarDetails ACD WITH (NOLOCK) ON ACD.DealerId = UD.DealerId
		LEFT JOIN @TempConsolidatedDataTbl TCD ON TCD.DealerId = ACD.DealerId
		WHERE OU.ID = @UserId
			AND UD.RoleId IN (
				3
				,5
				)
		GROUP BY UD.DealerId
			,TCD.EligibleCar
			,TCD.WarrantyActivated
	END
END
