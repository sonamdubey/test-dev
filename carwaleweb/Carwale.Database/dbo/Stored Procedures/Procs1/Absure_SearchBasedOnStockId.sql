IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Absure_SearchBasedOnStockId]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Absure_SearchBasedOnStockId]
GO

	

-- =============================================
-- Author		: Yuga Hatolkar
-- Created On	: 18th May, 2015
-- Description	: Get search result based on StockId
-- Modifier		: Sachin Bharti(24th July 2015)	
-- Purpose		: Add filters for CarId and Registration Number
-- =============================================
CREATE PROCEDURE [dbo].[Absure_SearchBasedOnStockId]--EXEC Absure_SearchBasedOnStockId null,'MH 04 GS 8413'

	@StockId	INT = NULL,
	@RegistrationNumber VARCHAR(10) = NULL,
	@CarId		INT = NULL
	
AS
BEGIN
	SET NOCOUNT OFF;
    
		SELECT 
			ACD.Id AS CarId,
			ACD.Make + ', ' + ACD.Model + ', ' + ACD.Version AS CarName,
			ACD.RegNumber AS RegistrationNo,
			ACD.StockId AS StockId, 
			CONVERT(VARCHAR(25),ACD.AppointmentDate,106) AS AppointmentDate,
			CONVERT(VARCHAR(25),ACD.EntryDate,106) AS RequestforInspection,
			CONVERT(VARCHAR(25),ACSM.EntryDate,106) AS AgencyAssignmentDate,
			CONVERT(VARCHAR(25),ACSM.UpdatedOn,106) AS SurveyorAssignmentDate,
			CONVERT(VARCHAR(25),ACD.SurveyDate,106) AS InspectionDoneDate,
			CONVERT(VARCHAR(25),ACD.FinalWarrantyDate,106) AS ApprovalDoneDate,
			CONVERT(VARCHAR(25),ACD.RejectedDateTime,106) AS RejectedDate,
			CONVERT(VARCHAR(25),AAW.EntryDate,106) AS WarrantyActivationRequestDate,
			CONVERT(VARCHAR(25),ACD.CancelledOn,106) AS CancelledOn,
			CONVERT(VARCHAR(25),(CASE WHEN (ACD.IsInspectionRescheduled = 1) THEN ACD.AppointmentDate ELSE CAST(NULL as DATETIME) END),106) AS RescheduledDate,
			(CASE WHEN ARCR.RejectedType = 1 THEN ARR.Reason ELSE AQC.Category + ' - ' + AQSC.SubCategory END) AS RejectedReason,
			CONVERT(VARCHAR(25),AAW.ActivationDate,106) AS WarrantyActivationDoneDate,
			CASE WHEN AWS.Status IS NULL THEN 'Request for inspection' ELSE AWS.Status END AS CurrentStatus,ACD.CancelReason AS CancelReason,
			DATEADD(DD,30,ACD.SurveyDate) AS CertificateExpiryDate, AA.Reason AS RescheduledReason--,ARCR.RejectedReason,AWS.Status
		FROM 
			AbSure_CarDetails ACD WITH(NOLOCK)
			LEFT JOIN AbSure_CarSurveyorMapping ACSM WITH(NOLOCK) ON ACD.Id = ACSM.AbSure_CarDetailsId
			LEFT JOIN  Absure_RejectedCarReasons ARCR ON ACD.Id = ARCR.Absure_CarDetailsId 
			LEFT JOIN AbSure_RejectionReasons ARR WITH(NOLOCK) ON ARCR.RejectedReason = ARR.Id 
			LEFT JOIN AbSure_ActivatedWarranty AAW WITH(NOLOCK) ON ACD.Id = AAW.AbSure_CarDetailsId        
			LEFT JOIN Absure_WarrantyStatuses AWS WITH(NOLOCK) ON AWS.Id = ISNULL(ACD.Status,0)
			LEFT JOIN Absure_RejectionType ART WITH(NOLOCK) ON ART.Id = ARCR.RejectedType AND ARCR.RejectedType = 1
			LEFT JOIN AbSure_QSubCategory AQSC WITH(NOLOCK) ON ARCR.RejectedReason = AQSC.AbSure_QSubCategoryId AND ARCR.RejectedType = 2
			LEFT JOIN AbSure_QCategory AQC WITH(NOLOCK) ON AQC.AbSure_QCategoryId = AQSC.AbSure_QCategoryId AND ARCR.RejectedType = 2
			LEFT JOIN Absure_Appointments AA WITH(NOLOCK) ON ACD.Id = AA.AbsureCarId
		WHERE 
			(@StockId IS NULL OR ACD.StockId = @StockId)
			AND (@RegistrationNumber IS NULL OR REPLACE(ACD.RegNumber, ' ', '') = @RegistrationNumber)
			AND (@CarId IS NULL OR ACD.Id = @CarId)
END
