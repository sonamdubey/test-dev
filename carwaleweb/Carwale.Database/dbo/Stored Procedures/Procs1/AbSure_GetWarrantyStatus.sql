IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_GetWarrantyStatus]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_GetWarrantyStatus]
GO

	-- =============================================
-- Author:		Ashwini Dhamankar
-- Create date: Feb 9,2015
-- Description:	To fetch status of warranty
-- Modified By: Ashwini Dhamankar on Feb 24,2015, Added join Of TC_Users
-- exec [AbSure_GetWarrantyStatus] 610672,5
-- Modified By Tejashree Patil on 13 March 2015, Selected only IsActive = 1 cars.
-- Modified By : Ashwini Dhamankar on Sep 11,2015(Called function Absure_GetMasterCarId to handle duplicate car condition)
-- Modified By : Ashwini Dhamankar on Sep 30,2015 (Added @RegNumber as parameter)
-- Modified By : Ashwini Dhamankar on Oct 28,2015 (Called function Absure_GetMasterCarId only if car is duplicate)
-- Modified By : Ashwini Dhamankar on Nov 3,2015 (Fetched DuplicateCarId)
-- Modified By : Ashwini Dhamankar on Nov 4,2015 (Handled condition if duplicate car is sold)
-- =============================================
CREATE PROCEDURE [dbo].[AbSure_GetWarrantyStatus] 
	@StockId BIGINT,
    @BranchId BIGINT ,
	@RegNumber VARCHAR(50) = NULL
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @CarId BIGINT,@Status INT,@CancelReason VARCHAR(250),@DuplicateCarId BIGINT,@IsDuplicateCar BIT = 0,@CancelledReason VARCHAR(250),@IsDuplicateCarSold BIT = 0
	SELECT @CancelledReason = Reason FROM AbSure_ReqCancellationReason WITH(NOLOCK) WHERE Id = 7;

	IF((SELECT TOP 1 ISNULL(ACD.IsSoldOut,0) FROM AbSure_CarDetails ACD WITH(NOLOCK) WHERE StockId = @StockId AND ACD.IsActive = 1 ORDER BY Id DESC) = 1)
	BEGIN
		SELECT TOP 1 @CarId = Id FROM AbSure_CarDetails WITH(NOLOCK) WHERE StockId = @StockId AND IsActive = 1  ORDER BY Id DESC  --master car after sell
		SELECT TOP 1 @DuplicateCarId = DuplicateCarId FROM Absure_ActivatedWarranty WITH(NOLOCK) WHERE Absure_CardetailsId = @CarId ORDER BY Absure_CardetailsId DESC  -- DuplicateCarId From Absure_ActivatedWarranty
		IF(@DuplicateCarId <> @CarId)
		BEGIN
		  SET @IsDuplicateCarSold = 1
		END
	END

	SELECT	@RegNumber = ACD.RegNumber,@CarId = ACD.Id,    --Dealer may add wrong regno while adding stock so fetch corerct regno from absure_cardetails
			@Status = ACD.Status, @CancelReason = ACD.CancelReason               
	FROM	AbSure_CarDetails ACD WITH(NOLOCK)
	WHERE	StockId = @StockId AND ACD.IsActive = 1

	IF(@IsDuplicateCarSold = 1)
	BEGIN
		SET @Status = 3 
		SET @CancelReason = @CancelledReason 
		SET @CarId = @DuplicateCarId
	END

	--IF EXISTS (SELECT AbSure_StockRegNumberMappingId FROM AbSure_StockRegNumberMapping WITH(NOLOCK) WHERE RegistrationNumber = @RegNumber)
	--BEGIN 
	IF(@Status = 3 AND  @CancelReason = @CancelledReason)
	BEGIN
		IF(@IsDuplicateCarSold <> 1)
		BEGIN
			SET @DuplicateCarId = @CarId
		END
		SET @IsDuplicateCar = 1
		SELECT @CarId = dbo.Absure_GetMasterCarId(@RegNumber,@CarId)  
		
		SELECT	@BranchId = DealerId,@StockId = StockId
		FROM	AbSure_CarDetails ACD WITH(NOLOCK)
		WHERE   ACD.Id = @CarId
	END

	--END
	--ELSE 
	--BEGIN
	--   SELECT @MasterCarId =  Id
	--   FROM AbSure_CarDetails 
	--   WHERE RegNumber = @RegNumber
	--END

	SELECT		ACD.Id AS AbSure_CarDetailsId,ACSM.AbSure_CarSurveyorMappingId,ACSM.EntryDate,ACD.FinalWarrantyTypeId,AWT.Warranty,ACD.SurveyDate,
				DATEDIFF(DAY,GETDATE(),ISNULL(ACD.SurveyDate,GETDATE()) + 30) AS RemainingDays,	
				CONVERT(VARCHAR,DATEADD(DAY,30,ISNULL(ACD.SurveyDate,GETDATE())),106) AS CertificateExpiryDate,ISNULL(ACD.Status,0) AS Status,
	CASE WHEN	((ACD.RegNumber = @RegNumber ) AND ACD.DealerId = @BranchId AND ACD.IsSurveyDone IS NULL AND  
				 ISNULL(ACD.Status,0) <> 3 AND ((ACSM1.AbSure_CarSurveyorMappingId IS NULL) OR (ACSM.AbSure_CarSurveyorMappingId IS NOT NULL AND U.IsAgency = 1))) 
				 THEN 1 ELSE 0 END IsWarrantyRequested,
				--(@StockId <> ACSM.TC_StockId) OR (ACSM.TC_UserId = -1) AND ACD.Status <> 3) THEN 1 ELSE 0 END IsWarrantyRequested,
	CASE WHEN   ((ACD.RegNumber = @RegNumber ) AND ACD.DealerId = @BranchId AND ACD.IsSurveyDone IS NULL AND
				ACSM.TC_UserId <> -1 AND ISNULL(ACD.Status,0) <> 3 AND ISNULL(U.IsAgency,0) <> 1) THEN 1 ELSE 0 END IsSurveyorAssigned,
	CASE WHEN	((ACD.RegNumber = @RegNumber) AND ACD.DealerId = @BranchId AND ACD.IsSurveyDone = 1 AND  
				ACD.FinalWarrantyTypeId IS NULL AND ACD.FinalWarrantyDate IS NULL AND ISNULL(ACD.IsRejected,0) = 0 AND 
				DATEDIFF(DAY,ACD.SurveyDate,GETDATE()) <= 30) THEN 1 ELSE 0 END IsSurveyDone ,
	CASE WHEN   ((ACD.RegNumber = @RegNumber) AND ACD.DealerId = @BranchId AND ACD.IsSurveyDone = 1 AND
				ACD.FinalWarrantyTypeId IS NOT NULL AND FinalWarrantyDate IS NOT NULL AND 
				ISNULL(ACD.IsRejected,0) = 0 and ACD.IsSoldOut <> 1 AND DATEDIFF(DAY,ACD.SurveyDate,GETDATE()) <= 30) THEN 1 ELSE 0 END IsWarrantyNotSold,   --- 1 if warranty is not sold 
	CASE WHEN   ((ACD.RegNumber = @RegNumber ) AND DealerId = @BranchId AND ACD.IsSurveyDone = 1 AND
				ACD.FinalWarrantyTypeId IS NOT NULL AND FinalWarrantyDate IS NOT NULL AND
				ISNULL(ACD.IsRejected,0) = 0 and ACD.IsSoldOut = 1 AND DATEDIFF(DAY,ACD.SurveyDate,GETDATE()) <= 30) THEN 1 ELSE 0 END IsWarrantySold,  --1 if warranty sold
	CASE WHEN   ((ACD.RegNumber = @RegNumber ) AND DealerId = @BranchId AND ACD.IsSurveyDone = 1 AND ISNULL(ACD.IsRejected,0) = 1) THEN 1 ELSE 0 END IsRejected,
	CASE WHEN   ((ACD.RegNumber = @RegNumber )  AND DealerId = @BranchId AND ACD.IsSurveyDone = 1  AND DATEDIFF(DAY,ACD.SurveyDate,GETDATE()) > 30) THEN 1 ELSE 0 END IsWarrantyExpired--,	--removed isSoldOut condition	
	--CASE WHEN   (ACD.StockId = @StockId AND DealerId = @BranchId AND ACD.Status = 9) THEN 1 ELSE 0 END IsOnHold,	
	,ISNULL(ACD.AbSureWarrantyActivationStatusesId,0) AS AbSureWarrantyActivationStatusesId,
	CASE WHEN	@IsDuplicateCar = 1 THEN @DuplicateCarId ELSE ACD.Id END DuplicateCarId

	FROM		AbSure_CarDetails ACD WITH(NOLOCK)
	LEFT JOIN	AbSure_CarSurveyorMapping ACSM WITH(NOLOCK) ON ACSM.AbSure_CarDetailsId = ACD.Id
	LEFT JOIN   TC_Users U WITH(NOLOCK) ON  U.Id = ACSM.TC_UserId  
	LEFT JOIN	AbSure_CarSurveyorMapping ACSM1 WITH(NOLOCK) ON ACSM1.AbSure_CarDetailsId = ACD.Id AND ACD.RegNumber IN (@RegNumber)
	LEFT JOIN	AbSure_WarrantyTypes AWT WITH(NOLOCK) ON AWT.AbSure_WarrantyTypesId = ACD.FinalWarrantyTypeId
	WHERE       ((ACD.RegNumber = @RegNumber AND ACD.Id = @CarId) AND ACD.DealerId = @BranchId 
				AND ISNULL(ACD.IsActive,1) = 1) -- Modified By Tejashree Patil on 13 March 2015, Selected only IsActive = 1 cars.


END


