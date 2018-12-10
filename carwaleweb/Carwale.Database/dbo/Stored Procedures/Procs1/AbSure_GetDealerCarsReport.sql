IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_GetDealerCarsReport]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_GetDealerCarsReport]
GO

	-- =============================================
-- Author:		Tejashree Patil
-- Create date: 9 Jun 2015
-- Description:	Get Car details of Dealer surveyor report.
-- =============================================

CREATE PROCEDURE [dbo].[AbSure_GetDealerCarsReport]
	@DealerId	INT		 = NULL,
	@Status		TINYINT	 = NULL,	-- 1-agency assn pending	2-surveyor assn pending			3-inspection pending		4-approval pending
									-- 5-Warranties Approved	6-Warranty Activation Pending	7-Rescheduled-with reason	8-Warranty Activated
									-- 9-eligibility			10-cancelled cars				11-rejected cars			12-expired certificate
	@Type		INT		 = NULL,	-- 1-Real Time 2-Consolidated
	@StartDate	DATETIME = NULL,	--'2014-06-10',
	@EndDate	DATETIME = NULL		--'2015-06-06'
AS
BEGIN
	DECLARE @Date DATETIME = GETDATE()
	/* Update Status Field in Absure_CarDetails Page
		0	OR NULL				- Inspection initiated by dealer but no action till now from AXA
		1	SurveyDone			- IF UPDATE(IsSurveyDone)		AND IsSurveyDone = 1		THEN 1 ELSE 0
		2	Rejected			- IF UPDATE(IsRejected)			AND IsRejected = 1			THEN 2
		3	Cancelled			- IF UPDATE(IsCancelled)		AND IsCancelled = 1			THEN 3
		4	Accepted			- IF UPDATE(FinalWarrantyType)	AND FinalWarrantyType > 0	THEN 4
		5	Surveyor Assigned	- Update Through SP where there is surveyor assignment	
		6	Agency Assigned		- Update Through SP where there is agency assignment	
		7	Certificate Expired	- One Automated running SP, Update Status only if warranty has not been activated
		8	Warranty Activated	- Call SP on Warranty activation Form.
	*/
	IF ISNULL(NULL,@Type) = 1  --Real Time
	BEGIN

		SELECT		ACD.StockId,ACD.Id AS AbSure_CarDetailsId,ACD.RegNumber,(ACD.Make + ' ' + ACD.Model + ' ' + ACD.Version) AS Car,ACD.VersionId,
					ACD.Kilometer,ACD.EntryDate,ACD.Status, AP.Reason AS ReschedulingReason
		FROM		AbSure_CarDetails ACD WITH(NOLOCK) 
					LEFT JOIN	AbSure_CarSurveyorMapping CSM	WITH(NOLOCK)  ON ACD.Id = CSM.AbSure_CarDetailsId 
					LEFT JOIN	AbSure_ActivatedWarranty AW		WITH (NOLOCK) ON ACD.ID = AW.AbSure_CarDetailsId
					LEFT JOIN	Absure_Appointments AP			WITH (NOLOCK) ON ACD.ID = AP.AbsureCarId
		WHERE		ACD.DealerId = @DealerId
					AND (
							   (@Status = 1 AND ACD.STATUS IS NULL				AND CAST(ACD.EntryDate AS DATE) = CAST(@Date AS DATE))   --agency pending i.e warranty requested
							OR (@Status = 2 AND ACD.STATUS = 6					AND CAST(CSM.EntryDate AS DATE) = CAST(@Date AS DATE))  --surveyor pending
							OR (@Status = 3 AND ACD.STATUS = 5					AND CAST(CSM.UpdatedOn AS DATE) = CAST(@Date AS DATE) )  --inspection pending
							OR (@Status = 4 AND ACD.IsSurveyDone=1				AND ACD.FinalWarrantyDate IS NULL 
											AND ISNULL(ACD.IsRejected,0)= 0		AND CAST(ACD.SurveyDate AS DATE) = CAST(@Date AS DATE))   --approval pending
							OR (@Status = 5 AND ACD.Status = 4					AND (CAST(ACD.FinalWarrantyDate AS DATE) = CAST(@Date AS DATE)))  -- warranty approved
							OR (@Status = 6 AND ISNULL(AW.IsActivated,0) = 0	AND CAST(AW.EntryDate AS DATE) = CAST(@Date AS DATE))   --warranty activation pending
							OR (@Status = 7 AND ACD.IsInspectionRescheduled = 1 AND CAST(AP.EntryDate AS DATE) = CAST(@Date AS DATE))  --rescheduled-with reason
							OR (@Status = 8 AND ACD.Status = 8					AND CAST(AW.ActivationDate AS DATE) = CAST(@Date AS DATE))
						)   --warranty activated
					
	END
	ELSE --Consolidated
	BEGIN
		SELECT	ACD.StockId,ACD.Id AS AbSure_CarDetailsId,ACD.RegNumber,(ACD.Make + ' ' + ACD.Model + ' ' + ACD.Version) AS Car,ACD.VersionId,
		ACD.Kilometer,ACD.EntryDate,ACD.Status,ACD.CancelReason CancelReason, dbo.AbSure_GetRejectionReasons(ACD.Id) RejectionReasons
		FROM		AbSure_CarDetails ACD WITH(NOLOCK) 
		INNER JOIN	TC_Stock ST WITH(NOLOCK) on ST.BranchId = ACD.DealerId AND ST.Id = ACD.StockId
		INNER JOIN	CarVersions V WITH(NOLOCK) ON  V.ID = ST.VersionId  
		INNER JOIN	AbSure_EligibleModels AE WITH(NOLOCK) ON AE.ModelId = V.CarModelId
		INNER JOIN  TC_CarCondition CC WITH(NOLOCK) ON CC.StockId = ST.Id	
		LEFT JOIN	AbSure_ActivatedWarranty AW WITH (NOLOCK) ON ACD.ID = AW.AbSure_CarDetailsId
	
		WHERE		ACD.DealerId = @DealerId
					AND (
							   (@Status = 5 AND ACD.Status = 4 AND (CAST(ACD.FinalWarrantyDate AS DATE) BETWEEN CAST(@StartDate AS DATE) AND CAST(@EndDate AS DATE))) --warranty approved
							OR (@Status = 8 AND ACD.Status = 8  AND  (CAST(AW.ActivationDate AS DATE) BETWEEN CAST(@StartDate AS DATE) AND CAST(@EndDate AS DATE)))  --warranty activated
							OR (@Status = 9 AND ST.Kms <= 98000 AND (DATEDIFF(MONTH,ST.MakeYear,GETDATE()) >= 20 AND DATEDIFF(MONTH,ST.MakeYear,GETDATE()) <= 84) 
										AND V.CarFuelType IN (1,2,3) AND CC.Owners <> 0 AND AE.IsActive = 1 AND ISNULL(AE.IsEligibleWarranty,1) = 1 
										AND	CAST(ST.EntryDate AS DATE) BETWEEN CAST(@StartDate AS DATE) and CAST(@EndDate AS DATE))          --eligible
							OR (@Status = 10 AND (CAST(ACD.CancelledOn AS DATE) BETWEEN CAST(@StartDate AS DATE) AND CAST(@EndDate AS DATE)))   --cancelled
							OR (@Status = 11 AND ACD.STATUS = 2 AND (CAST(ACD.RejectedDateTime AS DATE) BETWEEN CAST(@StartDate AS DATE) AND CAST(@EndDate AS DATE)))  --rejected 
							OR (@Status = 12 AND ACD.STATUS = 7 AND (CAST(DATEADD(DD,30,ACD.SurveyDate) AS DATE) BETWEEN CAST(@StartDate AS DATE) AND CAST(@EndDate AS DATE))) --expired
						)
	END
END