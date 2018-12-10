IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_VaildateCar]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_VaildateCar]
GO

	-- =============================================
-- Author:		Ruchira Patil
-- Create date: 1st Sept 2015
-- Description:	To validate car against the Registration number for add car feature
-- EXEC AbSure_VaildateCar 'CARWALE001',5,1103
-- =============================================
CREATE PROCEDURE [dbo].[AbSure_VaildateCar] 
	@RegNum		VARCHAR(50),
	@DealerId	INT = NULL ,
	@CarId		INT = NULL
AS
BEGIN
	DECLARE @IsActive BIT,@IsSurveyDone BIT,@Status INT = 0,@IsSoldOut BIT,@IsCancelled BIT,@SurveyDate DATETIME,@ExistingCarId INT,@WarrantyEndDate DATETIME,
	@ExistingDealerId INT,@IsNewCar INT = 0,@IsSameDealer INT = 0,@IsWarrantyExpired INT = 0,@CancelledReason VARCHAR(100),@IsDuplicateCar INT = 0

	SELECT @CancelledReason = Reason FROm AbSure_ReqCancellationReason WITH (NOLOCK) WHERE Id = 7 -- Cancellation reason of duplicate car

	SELECT		@ExistingCarId = ACD.Id, @IsActive = ACD.IsActive,@IsSurveyDone = ACD.IsSurveyDone, @Status = ACD.Status, 
				@IsSoldOut = ACD.IsSoldOut, @IsCancelled = ACD.IsCancelled, @SurveyDate = ACD.SurveyDate, @ExistingDealerId = ACD.DealerId,@WarrantyEndDate = AW.WarrantyEndDate
	FROM		AbSure_CarDetails ACD WITH (NOLOCK)
	LEFT JOIN	AbSure_ActivatedWarranty AW WITH (NOLOCK) on AW.AbSure_CarDetailsId = ACD.Id
	WHERE		REPLACE(REPLACE(ACD.RegNumber, ' ', ''), '-','') = REPLACE(REPLACE(@RegNum, ' ', ''), '-','') 
				AND ACD.DealerId NOT IN (3838,4271,11392,11894,12150) -- testing dealers,quikr dealers,camp\BTL,abSure.in
				--AND ACD.DealerId NOT IN (11392,11894,12150) -- testing dealers,quikr dealers,camp\BTL,abSure.in
				AND ISNULL(Status,0) NOT IN (5,6,0)--Agency assignment pending,surveyor assignment pending,not inspected
				AND (Status <> 3 AND (ACD.CancelReason NOT LIKE @CancelledReason or ACD.CancelReason is null))

	IF EXISTS(SELECT Id FROM AbSure_CarDetails ACD WITH (NOLOCK)
			  WHERE (ACD.CancelReason LIKE @CancelledReason) AND DealerId = @DealerId 
			  AND REPLACE(REPLACE(ACD.RegNumber, ' ', ''), '-','') = REPLACE(REPLACE(@RegNum, ' ', ''), '-',''))
	SET @IsDuplicateCar=1 -- if the duplicate car is again requested for inspection, marked it as same dealer and dont allow inspection

	--SELECT @ExistingCarId Id, @IsActive IsActive,@IsSurveyDone IsSurveyDone, @Status Status, @IsSoldOut IsSoldOut, @IsCancelled IsCancelled, @SurveyDate SurveyDate,@ExistingDealerId ExistingDealerId,@WarrantyEndDate WarrantyEndDate,@IsDuplicateCar IsDuplicateCar
		
	IF(@ExistingCarId IS NOT NULL AND LEN(@ExistingCarId) > 0)
	BEGIN
		IF(
		(@ExistingDealerId = ISNULL(@DealerId,0) AND 
		((((@Status = 8 OR (@Status = 4 AND @IsSoldOut= 1)) AND DATEDIFF(DAY,ISNULL(@WarrantyEndDate,GETDATE()),GETDATE()) < 0) OR ((@Status IN (1,9) OR (@Status = 4 AND @IsSoldOut = 0)) AND DATEDIFF(DAY,ISNULL(@SurveyDate,GETDATE()),GETDATE()) <= 30))
		AND @Status <> 3)) -- if the dealers are same checked if the warranty is expired or check if certificate is expired
		OR (@IsDuplicateCar = 1 AND 
			((@Status = 8 AND DATEDIFF(DAY,ISNULL(@WarrantyEndDate,GETDATE()),GETDATE()) < 0) -- warranty expired
				OR (@Status IN (1,4,9) AND DATEDIFF(DAY,ISNULL(@SurveyDate,GETDATE()),GETDATE()) <= 30) --certificate expired
			)) -- if the dealer is the one where the car is already marked as duplicate
		)
		BEGIN 
			SET @IsSameDealer = 1
			SET @IsNewCar=0
			IF(@CarId IS NOT NULL AND LEN(@CarId) > 0 AND @CarId <> @ExistingCarId)
				SET @IsWarrantyExpired = 1 -- When dealer is same and car is already inspected and the current request for inspection is through stock(having carid),cancel it and tag it as duplicate car
			ELSE
				SET @IsWarrantyExpired = 0 -- When dealer is same,dont cancel it and dont allow inspection
			SELECT '' DealerId, '' DealerName,@IsNewCar IsNewCar,@IsSameDealer IsSameDealer,@IsWarrantyExpired IsWarrantyExpired
		END
		ELSE
		BEGIN
			IF(@IsSurveyDone = 1 AND 
			(((@Status IN (1,9) OR (@Status = 4 AND @IsSoldOut = 0)) AND DATEDIFF(DAY,ISNULL(@SurveyDate,GETDATE()),GETDATE()) <= 30) --Approved Warranty
			OR ((@Status = 8 OR (@Status = 4 AND @IsSoldOut= 1)) AND DATEDIFF(DAY,ISNULL(@WarrantyEndDate,GETDATE()),GETDATE()) < 0) ))-- Activated Warranty and warranty not expired
			
			BEGIN
				SET @IsNewCar=0
				SET @IsSameDealer = 0
				IF ((@Status = 8 OR (@Status = 4 AND @IsSoldOut= 1)) AND @CarId IS NULL)
					SET @IsWarrantyExpired = 0 -- When iswarrantyexpired=0, no record inserted(dont insert record in case of warranty sold and not expired,linking not possible)
				ELSE
					SET @IsWarrantyExpired = 1 -- When iswarrantyexpired=1,record inserted
				SELECT D.ID DealerId, D.Organization DealerName,@IsNewCar IsNewCar,@IsSameDealer IsSameDealer,@IsWarrantyExpired IsWarrantyExpired
				FROM AbSure_CarDetails ACD WITH (NOLOCK)
				INNER JOIN Dealers D WITH (NOLOCK) ON D.ID = ACD.DealerId
				WHERE ACD.Id = @ExistingCarId 
			END

			IF((DATEDIFF(DAY,ISNULL(@SurveyDate,GETDATE()),GETDATE()) > 30 AND ISNULL(@IsSoldOut,0) = 0) -- Expired Certificates
				OR (@Status IN (2,3)) --Cancelled or Rejected 
				--OR (@Status IN (5,6)) -- Surveyor/Agency assigned
				OR((@Status = 8 OR (@Status = 4 AND @IsSoldOut= 1)) AND DATEDIFF(DAY,ISNULL(@WarrantyEndDate,GETDATE()),GETDATE()) > 0) -- Activated Warranty and warranty expired
			) 
			BEGIN
				SET @IsNewCar=1
				SET @IsSameDealer = 0
				SET @IsWarrantyExpired = 1
				SELECT '' DealerId, '' DealerName,@IsNewCar IsNewCar,@IsSameDealer IsSameDealer,@IsWarrantyExpired IsWarrantyExpired
			END
		END
	END
	ELSE
	BEGIN
		SET @IsNewCar=1
		SET @IsSameDealer = 0
		SET @IsWarrantyExpired = 1
		SELECT '' DealerId, '' DealerName,@IsNewCar IsNewCar,@IsSameDealer IsSameDealer,@IsWarrantyExpired IsWarrantyExpired
	END
END
