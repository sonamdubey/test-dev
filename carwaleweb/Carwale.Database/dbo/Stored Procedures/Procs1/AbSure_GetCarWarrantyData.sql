IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_GetCarWarrantyData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_GetCarWarrantyData]
GO

	
-- =====================================================================================
-- Author:		Vinay Kumar Prajapati
-- Create date: 9th Jan 2015
-- Description:	Get All car Warranty Data whose survey has been done.
-- exec AbSure_GetCarWarrantyData 5,1,1,10
-- Modified By Vivek Gupta on 2-2-2015, changed join condition with tc_stock
-- Modified By Ruchira Patil on 9th Feb 2015 (to filter data with all possible status:
-- status 1- All
-- status 2- Warranty Requested - Pending to Assign Surveyor
-- status 3- Surveyor Assigned - Pending for Inspection
-- status 4- Inspection done - Pending for Approval
-- status 5- Approved Warranties
-- status 6- Rejected Warranties
-- status 7- Sold Warranties
-- status 8- Expired Warranties
-- status 9- Cancelled Warranties 
-- status 11- Approval on Hold
-- status 12 - Verification Pending
-- Modified By : Suresh Prajapati on 21st April, 2015
-- Description : Added left join on Absure_InspectionFeedback for IsFeedbackGiven information
-- Modified By : Ruchira Patil on 25th May, 2015 (to show the warranty which is activated while selling it in sold warranties)
-- Modified By : Nilima More,15th sept 2015
-- Description : Added new status for doubtfull cases.
-- Modified By : Ashwini Dhamankar on 16 sept 2015 , retrive original car deatails 
-- Modified By : Kartik rathod on 21 Sept 2015 , added new column 'OriginalCarId' to get original car id of duplicate car & added new column IsDuplicateCar.
-- EXEC [AbSure_GetCarWarrantyData] 5,5,null,null
-- Modified By- Kartik RAthod on 20 Oct 2015 , added new status Verification Pending
-- Modified by : Kartik Rathod on 23 Oct, 2015 ,added AbSureWarrantyActivationStatusesId condition for status 5,7,12 in case of Verification Pending from olm team
-- =======================================================================================================================================
CREATE PROCEDURE [dbo].[AbSure_GetCarWarrantyData]
@DealerId		 INT,
@Status			 INT,
@FromIndex       INT, 
@ToIndex         INT
AS
BEGIN
--	  WITH Cte1 
--           AS (
--	  SELECT  TS.Id AS StockId, ACD.Id, ACD.Make +'-'+ACD.Model+'-'+ACD.Version  AS Car , ACD.RegNumber ,ACD.Kilometer ,
--	  AWT.Warranty,DATEDIFF(DAY,GETDATE(),ISNULL(ACD.SurveyDate,GETDATE()) + 30) AS RemainingDays, ACD.SurveyDate,
--	  IsSoldOut,ACD.FinalWarrantyTypeId AS FinalWarrantyTypeId,IsRejected,DATEDIFF(DAY,isnull(ACD.SurveyDate,GETDATE()),GETDATE()) AS DateDifference
--	  FROM AbSure_CarDetails AS ACD WITH(NOLOCK)
--	  LEFT JOIN AbSure_WarrantyTypes AS AWT WITH(NOLOCK) ON AWT.AbSure_WarrantyTypesId=COALESCE(ACD.FinalWarrantyTypeId,ACD.AbSure_WarrantyTypesId)
--	  LEFT JOIN TC_Stock AS TS WITH(NOLOCK) ON TS.Id=ACD.StockId  --TS.VersionId=ACD.VersionId  
--	  WHERE 
--	  ACD.isrejected=0 AND ACD.issurveydone=1 AND DATEDIFF(DAY,isnull(ACD.SurveyDate,GETDATE()),GETDATE()) <= 30 
--	  AND 
--	  TS.BranchId=@DealerId 
--	  AND (IsSoldOut IS NULL OR IsSoldOut = 0)
--	  )

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
		--SELECT TS.Id AS StockId, ACD.Id AS Id, ACD.Make +' - '+ACD.Model+' - '+ACD.Version  AS Car , ACD.RegNumber AS RegNumber ,ACD.Kilometer AS Kilometer ,
		--ISNULL(AWT.Warranty,'-') AS Warranty,ISNULL(CONVERT(VARCHAR,ACD.IsSurveyDone),'') AS IsSurveyDone,
		--CASE WHEN AWT.Warranty IS NULL OR IsSoldOut = 1  THEN '-' ELSE CONVERT(VARCHAR, DATEDIFF(DAY,GETDATE(),ISNULL(ACD.SurveyDate,GETDATE()) + 30),121) END AS RemainingDays, 
		--ISNULL(CONVERT(varchar, ACD.SurveyDate, 106), '-') AS SurveyDate,CONVERT(varchar,ACD.CancelledOn,106) AS CancelledDate,
		--CONVERT(varchar,DATEADD(dd,30, ACD.SurveyDate),106) AS ExpiredDate,
		--IsRejected,DATEDIFF(DAY,isnull(ACD.SurveyDate,GETDATE()),GETDATE()) AS DateDifference,IsSoldOut,ACD.FinalWarrantyTypeId AS FinalWarrantyTypeId,
		--CASE WHEN F.Absure_InspectionFeedbackId IS NOT NULL THEN 'True' ELSE 'False' END IsFeedbackGiven
		--FROM AbSure_CarDetails AS ACD WITH(NOLOCK)
		--LEFT JOIN AbSure_CarSurveyorMapping AS ACSM WITH(NOLOCK) ON ACSM.AbSure_CarDetailsId = ACD.Id
		--LEFT JOIN TC_Users U WITH(NOLOCK) ON U.Id = ACSM.TC_UserId 
		--LEFT JOIN AbSure_WarrantyTypes AS AWT WITH(NOLOCK) ON AWT.AbSure_WarrantyTypesId = ACD.FinalWarrantyTypeId -- COALESCE(ACD.FinalWarrantyTypeId,ACD.AbSure_WarrantyTypesId)
		--LEFT JOIN TC_Stock AS TS WITH(NOLOCK) ON TS.Id=ACD.StockId
		--LEFT JOIN Absure_InspectionFeedback F WITH(NOLOCK) ON ACD.Id= F.AbSure_CarDetailsId 
		--WHERE --TS.BranchId=@DealerId
		--		ACD.DealerId = @DealerId
		--AND 
		--((@Status = 1)
		--OR (
		--@Status = 2 AND ACD.IsSurveyDone IS NULL 
		--AND ACD.Id NOT IN (SELECT AbSure_CarDetailsId FROM AbSure_CarSurveyorMapping ACSM WITH(NOLOCK)LEFT JOIN TC_Users U WITH(NOLOCK) ON U.Id = ACSM.TC_UserId WHERE (U.IsAgency <> 1)) 
		--AND (ACD.Status <> 3 OR ACD.Status IS NULL)
		--)
		--OR (@Status = 3 AND ACSM.TC_UserId <> -1 AND (ISNULL(U.IsAgency,0) <> 1 ) AND ACD.IsSurveyDone IS NULL AND (ACD.Status <> 3 OR ACD.Status IS NULL))
		--OR (@Status = 4 AND ACD.IsSurveyDone = 1 AND ACD.FinalWarrantyTypeId IS NULL AND ACD.IsRejected=0 AND (IsSoldOut <> 1))
		--OR (@Status = 5 AND ACD.IsRejected=0 AND DATEDIFF(DAY,ISNULL(ACD.SurveyDate,GETDATE()),GETDATE()) <= 30 AND (IsSoldOut <> 1) AND ACD.FinalWarrantyTypeId IS NOT NULL)
		--OR (@Status = 6 AND ACD.IsRejected=1)
		--OR (@Status = 7 AND ACD.IsRejected=0 AND ACD.FinalWarrantyTypeId IS NOT NULL AND ACD.AbSure_WarrantyTypesId IS NOT NULL AND IsSoldOut = 1)
		--OR (@Status = 8 AND ACD.IsRejected=0 AND DATEDIFF(DAY,ISNULL(ACD.SurveyDate,GETDATE()),GETDATE()) > 30 AND (IsSoldOut <> 1) AND (ACD.FinalWarrantyTypeId IS NOT NULL OR ACD.AbSure_WarrantyTypesId IS NOT NULL))
		--OR (@Status = 9 AND ACD.Status = 3))
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
		)

      SELECT *, ROW_NUMBER() OVER (ORDER BY Id ) NumberForPaging INTO   #TblTemp FROM   Cte1 
	  SELECT * FROM #TblTemp WHERE (@FromIndex IS NULL AND @ToIndex IS NULL) OR (NumberForPaging  BETWEEN @FromIndex AND @ToIndex )
	
	  SELECT COUNT(*) AS RecordCount 
	  FROM #TblTemp 

      DROP TABLE #TblTemp 
	  DROP TABLE #OriginalCarTbl

END
