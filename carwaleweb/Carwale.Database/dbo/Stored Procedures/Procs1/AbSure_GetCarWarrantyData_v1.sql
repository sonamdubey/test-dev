IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_GetCarWarrantyData_v1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_GetCarWarrantyData_v1]
GO

	
-- =====================================================================================
-- Author      : Chetan Navin	
-- Create date : 25th Feb 2016
-- Description : To get car warranty data of absure/cartrade certified cars
-- EXEC AbSure_GetCarWarrantyData_v1 12150,5,NULL,NULL
-- =======================================================================================================================================
CREATE PROCEDURE [dbo].[AbSure_GetCarWarrantyData_v1]
@DealerId		 INT,
@Status			 INT,
@FromIndex       INT, 
@ToIndex         INT
AS
BEGIN
	-- Added By Kartik Rathod on 22 Sept 2015, add new column IsDuplicateCar. 
	DECLARE @CancelledReason VARCHAR(250)
	SELECT @CancelledReason = Reason FROM AbSure_ReqCancellationReason WITH(NOLOCK) WHERE Id = 7;      -- Cancellation reason of duplicate car

	-- Added By Ashwini Dhamanakar on 16 sept 2015, to get the original car details in case of duplicate cars
		-- here original car belongs to the Master Car Of the Duplicate car.
	SELECT	ACD.Id AS CarId, 
		dbo.Absure_GetMasterCarId(ACD.RegNumber,ACD.Id) AS OriginalCarId,
		ACD1.Make +' - '+ACD1.Model+' - '+ACD1.Version  AS OriginalCar,
		CASE WHEN dbo.Absure_GetMasterCarId(ACD.RegNumber,ACD.Id) IS NULL THEN 0 ELSE 1 END IsMapped,
		ACD1.Status,
		ACD1.StockId OriginalStockId, 
		ACD1.IsSurveyDone OriginalIsSurveyDone,
		ACD1.FinalWarrantyTypeId OriginalFinalWarrantyTypeId,
		ACD1.AbSure_WarrantyTypesId OriginalAbsureWarrantyId,
		ACD1.SurveyDate OriginalSurveyDate,
		ISNULL(ACD1.IsRejected,0) OriginalIsRejected,
		ACD1.IsSoldOut OriginalIsSoldOut,
		ACD1.CancelledOn OriginalCancelDate,
		CASE WHEN (ACD1.FinalWarrantyTypeId IS NULL OR ACD1.IsSoldOut = 1 ) THEN '-' ELSE CONVERT(VARCHAR, DATEDIFF(DAY,GETDATE(),ISNULL(ACD1.SurveyDate,GETDATE()) + 30),121)  END OriginalRemainingDays,
		ACD1.AbSureWarrantyActivationStatusesId AS OriginalAbSureWarrantyActivationStatusesId
		INTO #OriginalCarTbl 
		FROM AbSure_CarDetails ACD WITH(NOLOCK)																				-- ACD is the data of duplicate car
			LEFT JOIN AbSure_CarDetails ACD1 WITH(NOLOCK) ON ACD1.Id = dbo.Absure_GetMasterCarId(ACD.RegNumber,ACD.Id)      -- here ACD1 is the Table with Original Cars Data
		WHERE ACD.DealerId = @DealerId AND ACD.Status = 3 AND ACD.CancelReason = @CancelledReason							-- get original car details

		                                       

	  ;WITH Cte1
			AS (
			SELECT  DISTINCT TS.Id AS StockId,
					ACD.Id AS Id, 
					CASE WHEN ACD.CancelReason = @CancelledReason AND OT.IsMapped = 1 THEN OT.OriginalCar ELSE ACD.Make +' - '+ ACD.Model+' - '+ ACD.Version END  AS Car, 
					ACD.RegNumber AS RegNumber ,
					ACD.Kilometer AS Kilometer ,
					CASE WHEN AW.WarrantyTypeId IS NOT NULL THEN AW.WarrantyTypeId WHEN MAW.WarrantyTypeId IS NOT NULL THEN MAW.WarrantyTypeId ELSE NULL END AS WarrantyTypeId,
					--CASE @Status WHEN 7 THEN ISNULL(AWT1.Warranty,'-') ELSE ISNULL(AWT.Warranty,'-') END  AS Warranty, 
					CASE WHEN @Status=7 AND AWT1.Warranty IS NOT NULL THEN AWT1.Warranty WHEN @Status= 7 AND AWT1.Warranty IS NULL THEN MAWT1.Warranty  WHEN @Status <> 7 AND AWT.Warranty IS NOT NULL THEN AWT.Warranty WHEN @Status <> 7 AND AWT.Warranty IS NULL THEN MAWT.Warranty ELSE '-' END AS Warranty,
					CASE WHEN ACD.IsSurveyDone IS NOT NULL THEN CONVERT(VARCHAR,ACD.IsSurveyDone)  WHEN OT.OriginalIsSurveyDone IS NOT NULL THEN CONVERT(VARCHAR,OT.OriginalIsSurveyDone) ELSE '' END IsSurveyDone,
					CASE WHEN ACD.CancelReason = @CancelledReason THEN OT.OriginalRemainingDays WHEN AWT.Warranty IS NULL OR IsSoldOut = 1  THEN '-' ELSE CONVERT(VARCHAR, DATEDIFF(DAY,GETDATE(),ISNULL(ACD.SurveyDate,GETDATE()) + 30),121) END AS RemainingDays,
					CASE WHEN ACD.SurveyDate IS NOT NULL THEN CONVERT(varchar, ACD.SurveyDate, 106) WHEN OT.OriginalSurveyDate IS NOT NULL THEN CONVERT(varchar, OT.OriginalSurveyDate, 106) ELSE '-' END SurveyDate,
					CONVERT(varchar,ACD.CancelledOn,106) AS CancelledDate, 
					CASE WHEN ACD.SurveyDate IS NOT NULL THEN CONVERT(varchar,DATEADD(dd,30, ACD.SurveyDate),106) WHEN OT.OriginalSurveyDate IS NOT NULL THEN CONVERT(varchar,DATEADD(dd,30,OT.OriginalSurveyDate),106) ELSE NULL END AS ExpiredDate,
					--CASE WHEN ACD.IsRejected IS NOT NULL THEN ACD.IsRejected WHEN OT.OriginalIsRejected IS NOT NULL THEN OT.OriginalIsRejected END AS IsRejected,
					CASE WHEN ACD.CancelReason = @CancelledReason AND OT.IsMapped = 1 THEN OT.OriginalIsRejected ELSE ISNULL(ACD.Isrejected,0) END AS IsRejected,
					CASE WHEN ACD.SurveyDate IS NOT NULL THEN DATEDIFF(DAY,ACD.SurveyDate,GETDATE()) WHEN OT.OriginalSurveyDate IS NOT NULL THEN DATEDIFF(DAY,isnull(OT.OriginalSurveyDate,GETDATE()),GETDATE()) ELSE DATEDIFF(DAY,GETDATE(),GETDATE()) END AS DateDifference,
					CASE WHEN ACD.IsSoldOut = 1 OR ACD.IsSoldOut IS NULL THEN ACD.IsSoldOut WHEN OT.OriginalIsSoldOut IS NOT NULL THEN OT.OriginalIsSoldOut ELSE 0 END AS IsSoldOut,
					CASE WHEN ACD.FinalWarrantyTypeId IS NOT NULL THEN ACD.FinalWarrantyTypeId WHEN OT.OriginalFinalWarrantyTypeId IS NOT NULL THEN OT.OriginalFinalWarrantyTypeId ELSE NULL END AS FinalWarrantyTypeId,
					CASE WHEN F.Absure_InspectionFeedbackId IS NOT NULL THEN 'True' ELSE 'False' END IsFeedbackGiven,
					CASE WHEN ACD.CancelReason = @CancelledReason THEN 'True' ELSE 'False' END IsDuplicateCar,
					CASE WHEN ACD.CancelReason = @CancelledReason THEN OT.OriginalCarId ELSE ACD.Id END OriginalCarId,   --if car is with 'duplicate car' cancel reason then only take originalcarid or always pass ACD.Id as  originalcarid
					CASE WHEN ACD.CancelReason = @CancelledReason THEN OT.OriginalAbSureWarrantyActivationStatusesId ELSE ACD.AbSureWarrantyActivationStatusesId END AbSureWarrantyActivationStatusesId
					,'1' AS Product
			FROM AbSure_CarDetails AS ACD WITH(NOLOCK)
				LEFT JOIN AbSure_CarSurveyorMapping AS ACSM WITH(NOLOCK) ON ACSM.AbSure_CarDetailsId = ACD.Id
				LEFT JOIN TC_Users U WITH(NOLOCK) ON U.Id = ACSM.TC_UserId 
				LEFT JOIN #OriginalCarTbl OT WITH(NOLOCK) ON ACD.Id = OT.CarId
				LEFT JOIN AbSure_WarrantyTypes AS AWT WITH(NOLOCK) ON AWT.AbSure_WarrantyTypesId = ACD.FinalWarrantyTypeId AND @Status <> 7
				LEFT JOIN AbSure_WarrantyTypes AS MAWT WITH(NOLOCK) ON MAWT.AbSure_WarrantyTypesId = OT.OriginalFinalWarrantyTypeId AND @Status <> 7
				LEFT JOIN AbSure_ActivatedWarranty AS AW WITH(NOLOCK) ON AW.AbSure_CarDetailsId = acd.Id
				LEFT JOIN AbSure_ActivatedWarranty AS MAW WITH(NOLOCK) ON MAW.AbSure_CarDetailsId = OT.OriginalCarId
				LEFT JOIN AbSure_WarrantyTypes AS AWT1 WITH(NOLOCK) ON AWT1.AbSure_WarrantyTypesId = AW.WarrantyTypeId AND @Status = 7
				LEFT JOIN AbSure_WarrantyTypes AS MAWT1 WITH(NOLOCK) ON MAWT1.AbSure_WarrantyTypesId = MAW.WarrantyTypeId  AND @Status = 7
				LEFT JOIN TC_Stock AS TS WITH(NOLOCK) ON TS.Id=ACD.StockId
				LEFT JOIN Absure_InspectionFeedback F WITH(NOLOCK) ON ACD.Id= F.AbSure_CarDetailsId 
				LEFT JOIN AbSure_CarPhotos P WITH(NOLOCK) ON ACD.ID = P.AbSure_CarDetailsId
			
			WHERE --TS.BranchId=@DealerId
					ACD.DealerId = @DealerId
			AND 
			(
			 (@Status = 1)
			OR (
					@Status = 2 AND ACD.IsSurveyDone IS NULL 
					AND ACD.Id NOT IN (SELECT AbSure_CarDetailsId FROM AbSure_CarSurveyorMapping ACSM WITH(NOLOCK)LEFT JOIN TC_Users U WITH(NOLOCK) ON U.Id = ACSM.TC_UserId WHERE (U.IsAgency <> 1)) 
					AND (ACD.Status <> 3 OR ACD.Status IS NULL)
				)
			OR (@Status = 3 AND ACSM.TC_UserId <> -1 AND (ISNULL(U.IsAgency,0) <> 1 ) AND ACD.IsSurveyDone IS NULL AND (ACD.Status <> 3 OR ACD.Status IS NULL))
			OR (@Status = 4 AND ((ACD.IsSurveyDone = 1 AND ACD.FinalWarrantyTypeId IS NULL AND ISNULL(ACD.IsRejected,0)=0 AND (IsSoldOut <> 1) AND (ISNULL(RCImagePending,0) = 0) AND ACD.Status <> 9) OR (OT.Status = 1 AND OT.OriginalFinalWarrantyTypeId IS NULL)))
			OR (@Status = 5 AND ( (ISNULL(ACD.IsRejected,0)=0 AND DATEDIFF(DAY,ISNULL(ACD.SurveyDate,GETDATE()),GETDATE()) <= 30 AND IsSoldOut <> 1 AND ACD.FinalWarrantyTypeId IS NOT NULL AND ISNULL(ACD.AbSureWarrantyActivationStatusesId,0)<>1) OR (OT.Status = 4 AND OT.Status <> 9 AND DATEDIFF(DAY,ISNULL(OT.OriginalSurveyDate,GETDATE()),GETDATE()) <= 30 AND OT.OriginalIsSoldOut <> 1 AND ISNULL(OT.OriginalAbSureWarrantyActivationStatusesId,0) <> 1)))
			OR (@Status = 6 AND (ISNULL(ACD.IsRejected,0)=1 OR (OT.OriginalIsRejected = 1)))
			OR (@Status = 7 AND ((ISNULL(ACD.IsRejected,0)=0 AND ACD.FinalWarrantyTypeId IS NOT NULL AND ACD.AbSure_WarrantyTypesId IS NOT NULL AND IsSoldOut = 1 AND ISNULL(ACD.AbSureWarrantyActivationStatusesId,2) = 2 ) OR (OT.Status IN (4,8) AND OT.OriginalIsSoldOut = 1 AND OT.OriginalIsRejected = 0 AND ISNULL(OT.OriginalAbSureWarrantyActivationStatusesId,2)=2)))
			OR (@Status = 8 AND ((ISNULL(ACD.IsRejected,0)=0 AND ACD.FinalWarrantyTypeId IS NOT NULL AND ACD.AbSure_WarrantyTypesId IS NOT NULL AND IsSoldOut = 1 AND (ACD.FinalWarrantyTypeId IS NOT NULL OR ACD.AbSure_WarrantyTypesId IS NOT NULL) )  OR (OT.OriginalIsRejected = 0 AND DATEDIFF(DAY,ISNULL(OT.OriginalSurveyDate,GETDATE()),GETDATE()) > 30 AND (OT.OriginalIsSoldOut <> 1) AND (OT.OriginalFinalWarrantyTypeId IS NOT NULL OR OT.OriginalAbsureWarrantyId IS NOT NULL))))
			OR (@Status = 9 AND ((ACD.Status = 3 AND ACD.cancelreason <> @CancelledReason) OR OT.IsMapped=0)) --cancelled 
			OR (@Status = 10 AND ACD.IsSurveyDone = 1 AND ACD.FinalWarrantyTypeId IS NULL AND ISNULL(ACD.IsRejected,0)=0 AND (IsSoldOut IS NULL OR IsSoldOut = 0) AND ACD.RCImagePending = 1)
			OR (@Status = 11 AND (ACD.Status = 9 OR OT.Status = 9))				--onhold
			OR (@Status = 12 AND ((ACD.AbSureWarrantyActivationStatusesId = 1) OR ISNULL(OT.OriginalAbSureWarrantyActivationStatusesId,0) = 1 ))			--Verification Pending from olm team
			)
			UNION
			SELECT DISTINCT TR.ListingId AS StockId
			,TR.CarTradeCertificationId AS Id
			,TR.Make + ' ' + TR.Model + ' ' + TR.Variant AS Car
			,TR.RegistrationNo AS RegNumber 
			,TC.Mileage AS Kilometer 
			,1 AS  WarrantyTypeId
			,CASE WHEN ISNULL(TC.IsWarranty,0) = 1 AND TR.CertificationStatus = 1 THEN 'Available' 
			      WHEN TR.CertificationStatus = 8 THEN 'Comprehensive' ELSE '' END Warranty
			,CASE WHEN ISNULL(TR.CertificationStatus,0) IN(1,6,7) THEN 1 ELSE 0 END IsSurveyDone
			,CASE WHEN TC.InvCertifiedDate IS NOT NULL THEN CONVERT(VARCHAR, DATEDIFF(DAY,GETDATE(),ISNULL(TC.InvCertifiedDate,GETDATE()) + 45),121) ELSE NULL END RemainingDays
			,ISNULL(CONVERT(VARCHAR,TC.InvCertifiedDate,106),NULL) SurveyDate
			,NULL AS CancelledDate 
			,CASE WHEN TC.InvCertifiedDate IS NOT NULL THEN CONVERT(VARCHAR,DATEADD(dd,45, TC.InvCertifiedDate),106) ELSE NULL END AS ExpiredDate
			,'False' AS IsRejected
			,CASE WHEN TC.InvCertifiedDate IS NOT NULL THEN DATEDIFF(DAY,TC.InvCertifiedDate,GETDATE()) ELSE NULL END AS DateDifference
			,CASE WHEN TR.CertificationStatus = 8 THEN 1 ELSE 0 END AS IsSoldOut
			,1 AS FinalWarrantyTypeId
			,'False' IsFeedbackGiven
			,'False' AS IsDuplicateCar
			,TC.TC_CarTradeCertificationDataId AS OriginalCarId   --if car is with 'duplicate car' cancel reason then only take originalcarid or always pass ACD.Id as  originalcarid
			,NULL AS AbSureWarrantyActivationStatusesId
			,'2' AS Product
			FROM TC_CarTradeCertificationRequests TR WITH(NOLOCK)
			LEFT JOIN TC_CarTradeCertificationData TC WITH(NOLOCK) ON TR.ListingId = TC.ListingId
			WHERE TR.DealerId = @DealerId AND 
			(@Status = 1             --All
			OR                      
			(@Status = 3 AND TR.CertificationStatus = 2)        --Requested/Pending
			OR
			(@Status = 4 AND TR.CertificationStatus = 1         --Certification Done
			AND TR.ListingId IN (SELECT ListingId FROM TC_CarTradeCertificationLiveListing TL WITH(NOLOCK) WHERE TL.TC_CarTradeCertificationRequestId = TR.TC_CarTradeCertificationRequestId)
			)
			OR      --Warranty Available/Approved
			(@Status = 5 AND TC.IsWarranty = 1 AND TR.CertificationStatus = 1 AND DATEDIFF(DAY,TC.InvCertifiedDate,GETDATE()) <= 45)
			OR
			(@Status = 12 AND TR.CertificationStatus = 6 AND DATEDIFF(DAY,TC.InvCertifiedDate,GETDATE()) <= 45))
			OR     --Sold/Activated
			(@Status = 7 AND TR.CertificationStatus = 8)
			OR     --Expired
			(@Status = 8 AND TR.CertificationStatus = 1 AND DATEDIFF(DAY,TC.InvCertifiedDate,GETDATE()) > 45 )
		)

      SELECT *, ROW_NUMBER() OVER (ORDER BY Id ) NumberForPaging INTO   #TblTemp FROM   Cte1 WITH(NOLOCK)
	  SELECT * FROM #TblTemp WITH(NOLOCK) WHERE (@FromIndex IS NULL AND @ToIndex IS NULL) OR (NumberForPaging  BETWEEN @FromIndex AND @ToIndex )
	
	  SELECT COUNT(*) AS RecordCount 
	  FROM #TblTemp 

      DROP TABLE #TblTemp 
	  DROP TABLE #OriginalCarTbl
END





