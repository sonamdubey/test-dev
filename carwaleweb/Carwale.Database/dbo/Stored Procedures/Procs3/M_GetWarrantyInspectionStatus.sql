IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[M_GetWarrantyInspectionStatus]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[M_GetWarrantyInspectionStatus]
GO

	
-- =============================================
-- Author      : Suresh Prajapati
-- Create date : 14th July, 2015
-- Description : To get Summary and Dealer data of warranty inspection status
-- EXEC M_GetWarrantyInspectionStatus 3,NULL
-- =============================================
CREATE PROCEDURE [dbo].[M_GetWarrantyInspectionStatus] @OprUserId INT = NULL
	,@ReportingUserId INT = NULL
AS
BEGIN
	
	DECLARE @TempRealtimeDataTbl TABLE (
		UserId INT
		,DealerId INT
		,SurveyorPending INT
		,InspectionPending INT
		,WarrantyActivated INT
		)

	INSERT INTO @TempRealtimeDataTbl (
		--UserId
		DealerId
		,SurveyorPending
		,InspectionPending
		,WarrantyActivated
		)
	SELECT --Tbl1.UserId
		Tbl1.DealerId
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
		WHERE OU.ID IN (
				SELECT UsersId
				FROM [dbo].[Fn_DCRM_GetChildUsersIncludingParent](@OprUserId)
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
		WHERE OU.ID IN (
				SELECT UsersId
				FROM [dbo].[Fn_DCRM_GetChildUsersIncludingParent](ISNULL(@ReportingUserId, @OprUserId))
				)
			AND CAST(AW.ActivationDate AS DATE) = CAST(GETDATE() AS DATE)
		GROUP BY UD.DealerId
		) AS Tbl2 ON Tbl1.DealerId = Tbl2.DealerId

	DECLARE @InspectionStatus AS TABLE (
		OprUserId INT
		,Inspectionpending INT
		,SurveyorPending INT
		,WarrantyActivated INT
		,AgencyPending INT
		,ApprovalPending INT
		,WarrantiesApproved INT
		,WarrantyActivationPending INT
		,Rescheduled INT
		)

	INSERT INTO @InspectionStatus (
		OprUserId
		,Inspectionpending
		,SurveyorPending
		,WarrantyActivated
		,AgencyPending
		,ApprovalPending
		,WarrantiesApproved
		,WarrantyActivationPending
		,Rescheduled
		)
	SELECT
		@OprUserId
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
	--LEFT JOIN @DirectReportingUserId AS
	WHERE OU.ID IN (
			SELECT UsersId
			FROM [dbo].[Fn_DCRM_GetChildUsersIncludingParent](@OprUserId)
			)
	--AND UD.RoleId IN (
	--	3
	--	,5
	--	)
	GROUP BY UD.DealerId
		,TCD.InspectionPending
		,TCD.SurveyorPending
		,TCD.WarrantyActivated

	SELECT OprUserId
		,SUM(Inspectionpending) AS Inspectionpending
		,SUM(SurveyorPending) AS SurveyorPending
		,SUM(WarrantyActivated) AS WarrantyActivated
		,SUM(AgencyPending) AS AgencyPending
		,SUM(ApprovalPending) AS ApprovalPending
		,SUM(WarrantiesApproved) AS WarrantiesApproved
		,SUM(WarrantyActivationPending) AS WarrantyActivationPending
		,SUM(Rescheduled) AS Rescheduled
	FROM @InspectionStatus
	GROUP BY OprUserId
		--END
END
