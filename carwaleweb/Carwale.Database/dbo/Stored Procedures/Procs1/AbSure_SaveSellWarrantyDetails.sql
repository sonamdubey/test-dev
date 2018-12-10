IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_SaveSellWarrantyDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_SaveSellWarrantyDetails]
GO

	-- =============================================
-- Author:        Ruchira Patil
-- Create date: 8th Jan 2015
-- Description:    To Save Sell Warranty Details
-- Modified By : Vinay Kumar Prajapati 14 jan 2015 after successfull warranty sell make IsSoldOut 1
-- Modified By : Ashwini Dhamankar on Feb 23,2015 , Added WarrantyTypeId parameter and fetched WarrantyType from AbSure_ActivatedWarranty
-- Modified By : Yuga Hatolkar on Mar 5, 2015, Added EngineNo and ChassisNo
-- Modified By : Ruchira Patil on 20th Mar 2015 (To insert data in AbSure_Policy against the recently added carId)
-- Modified By: Yuga Hatolkar on March 23rd, 2015, Added Parameter @EligibleModelFor and fetched data respectively.
-- Modified By : Ashwini Dhamankar on Sep 14,2015, Called function Absure_GetMasterCarId to handle duplicate car condition
-- Modified By : Kartik Rathod on 23 sept 2015, Update Absure_Cardetails table to set the StockId of Duplicate car to Origional car
-- Modified By : Kartik rathod on 21 Oct 2015, Update Absure_Cardetails table to set the AbSureWarrantyActivationStatusesId = 2
-- Modified By : Kartik Rathod on 23 Oct 2015, Added @IsWarrantyActivationPending variable (At time of the sell warranty form submitting the all details goes into the Absure_WarrantyActivationPending Table)
--                                             (only after details verified and approved by olm side, car will create entry in the ActivatedWarranty TAble)
-- Modified By : Ashwini Dhamankar on Oct 27,2015 (Added @DuplicateCarId ,@IsDuplicateCar,@Status ,@CancelReason)
-- Modified By : AShwini Dhamankar on Nov 2,2015 (Called AbSure_ChangeCertification for OriginalStockId to update details of that stock in livelistings)
-- Modified By : Ashwini Dhamankar on Nov 4,2015 (Added @IsDuplicateCarSold and passed it to AbSure_ChangeCertification)
-- Modified By : Chetan Navin on Feb 26 2016 (handled saving of cartrade warranty)
-- Modified By : Chetan Navin on Apr 18 2016 (Added parameter isCartrade to pass in function Absure_GenerateWarrantyPolicyNo)
-- =============================================
CREATE PROCEDURE [dbo].[AbSure_SaveSellWarrantyDetails]
    @CarId                INT,
    @CustName            VARCHAR(150),
    @Address            VARCHAR(500),
    @Mobile                VARCHAR(20),
    @AlternatePhone        VARCHAR(20),
    @Email                VARCHAR(50),
    @Model                VARCHAR(100),
    @MakeYear            DATETIME,
    @RegNumber            VARCHAR(50),
    @RegistrationDate    DATETIME,
    @Kilometer            BIGINT,
    @WarrantyStartDate  DATETIME,
    @WarrantyEndDate    DATETIME,
    @DealerId            INT,
    @UserId                INT,
    @ServiceTaxValue    DECIMAL(18,2),
    @WarrantyTypeId     INT,
    @EngineNo            VARCHAR(50) = NULL,
    @ChassisNo            VARCHAR(50) = NULL,
    @EligibleModelFor    TINYINT = 1,  -- 1: By default Warranty
    @IsWarrantyActivationPending BIT = NULL,       -- @IsWarrantyActivationPending =1 When dealer sell warranty AND  @IsWarrantyActivationPending=0 when olm is approving the sell warranty
	@ListingId             INT = NULL
AS
BEGIN
	IF(@CarId IS NOT NULL)
	BEGIN	
    DECLARE @StockId BIGINT,@OriginalStockId BIGINT,@DuplicateCarId BIGINT,@IsDuplicateCar BIT = 0,@Status INT,@CancelReason VARCHAR(50),
			@CancelledReason VARCHAR(50),@IsDuplicateCarSold BIT = 0,@PolicyNo	VARCHAR(50)  

	SELECT @CancelledReason = Reason FROM AbSure_ReqCancellationReason WITH(NOLOCK) WHERE Id = 7;

    SELECT  @StockId = StockId , @DuplicateCarId = ACD.Id,
            @Status = ACD.Status,@CancelReason = ACD.CancelReason
    FROM    AbSure_CarDetails ACD WITH(NOLOCK)
    WHERE   ACD.Id = @CarId			
	
	IF(@Status = 3 AND @CancelReason = @CancelledReason)
		BEGIN
			SET @IsDuplicateCar = 1 
			SET @IsDuplicateCarSold = 1
		END

		--IF EXISTS (SELECT AbSure_StockRegNumberMappingId FROM AbSure_StockRegNumberMapping WITH(NOLOCK) WHERE RegistrationNumber = @RegNumber)
	IF(@IsDuplicateCar = 1)
		BEGIN
		SELECT @CarId = dbo.Absure_GetMasterCarId(@RegNumber,@CarId) 
		
		END

	SELECT  @OriginalStockId = StockId           --StockId of car which is getting sold
		FROM    AbSure_CarDetails ACD WITH(NOLOCK)
		WHERE   ACD.Id = @CarId
			
			
	IF( @IsWarrantyActivationPending = 0)
	BEGIN
		IF NOT EXISTS(SELECT 1 FROM AbSure_WarrantyActivationPending WITH(NOLOCK) WHERE IsActive = 1 AND IsCarTradeWarranty = 1 AND AbSure_CarDetailsId = @CarId)
		BEGIN
			SELECT AbSure_ActivatedWarrantyID FROM AbSure_ActivatedWarranty WITH(NOLOCK) WHERE AbSure_CarDetailsId = @CarId
			IF @@ROWCOUNT = 0 -- Added By Deepak on 13th May 2015 because of duplicate records in database
				BEGIN
      
					INSERT INTO AbSure_ActivatedWarranty
								(AbSure_CarDetailsId,CustName,Address,Mobile,AlternatePhone,Email,Model,MakeYear,RegNumber,RegistrationDate,Kilometer,
								WarrantyStartDate,WarrantyEndDate,DealerId,UserId,WarrantyTypeId, EngineNo, ChassisNo,OriginalStockId,DuplicateCarId)
						VALUES  (@CarId,@CustName,@Address,@Mobile,@AlternatePhone,@Email,@Model,@MakeYear,@RegNumber,@RegistrationDate,@Kilometer,
								@WarrantyStartDate,@WarrantyEndDate,@DealerId,@UserId,@WarrantyTypeId, @EngineNo, @ChassisNo,@OriginalStockId,@DuplicateCarId)

					INSERT INTO AbSure_Policy(AbSure_CarId) VALUES (@CarId)

					--Get amount to be deducted modelwise and warrantywise
					DECLARE @ModelId INT = NULL, @ModelAmount DECIMAL(18,2), @GoldSalesCost DECIMAL(18,2), @SilverSalesCost DECIMAL(18,2)

					SELECT    @ModelId = V.ModelId,
							@ModelAmount =    /*CASE CD.FinalWarrantyTypeId WHEN 1 THEN EM.GoldPrice ELSE
											CASE CD.FinalWarrantyTypeId WHEN 2 THEN EM.SilverPrice END END,*/
											CASE AW.WarrantyTypeId WHEN 1 THEN EM.GoldPrice ELSE
											CASE AW.WarrantyTypeId WHEN 2 THEN EM.SilverPrice END END,
							@GoldSalesCost = EM.GoldSalesCost,
							@SilverSalesCost = EM.SilverSalesCost
					FROM    vwMMV V WITH(NOLOCK)
							INNER JOIN AbSure_CarDetails CD WITH(NOLOCK) ON CD.VersionId = V.VersionId
							INNER JOIN AbSure_EligibleModels EM WITH(NOLOCK) ON EM.ModelId = V.ModelId
							INNER JOIN AbSure_WarrantyTypes WT WITH(NOLOCK) ON WT.AbSure_WarrantyTypesId = CD.FinalWarrantyTypeId
							INNER JOIN AbSure_ActivatedWarranty AW WITH(NOLOCK) ON AW.AbSure_CarDetailsId = CD.Id
					WHERE    CD.Id = @CarId
							AND CD.IsSurveyDone = 1
							AND ISNULL(CD.IsRejected,0) = 0
							AND WT.IsActive = 1
							AND (
										( @EligibleModelFor = 1 AND ISNULL(EM.IsEligibleWarranty,1) = 1) OR
										( @EligibleModelFor = 2 AND EM.IsEligibleCertification = 1) OR
										( @EligibleModelFor = 3 AND (EM.IsEligibleCertification = 1 AND EM.IsEligibleWarranty = 1))
									)

					IF @@ROWCOUNT > 0
          
						BEGIN
              
							IF @ModelAmount > 0
								BEGIN
									--Transaction related changes
									EXECUTE AbSure_Transaction
											@DealerId,
											@CarId,
											@ModelAmount,
											@ServiceTaxValue,
											@UserId,
											@GoldSalesCost,
											@SilverSalesCost

								--	After successfull transaction of specific car warranty then make it soldOut(IsSoldOut =true) 
								UPDATE	AbSure_CarDetails 
								SET		IsSoldOut=1 
								WHERE	Id= @CarId	
								/*start - Modified by   :	Ruchira Patil on 20th Mar 2015 (To update policy no)*/
								SELECT @PolicyNo = [dbo].[Absure_GenerateWarrantyPolicyNo](@CarId,1,0) -- 1 is for dealer
		
								UPDATE AbSure_CarDetails SET PolicyNo = @PolicyNo WHERE Id = @CarId
								/*end*/	
								END
							ELSE
								INSERT INTO AbSure_Trans_FailedTransactions(DealerId, CarId, DebitAmount, ServiceTaxValue, UserId, GoldSalesCost, SilverSalesCost)
																VALUES(@DealerId, @CarId, @ModelAmount, @ServiceTaxValue, @UserId, @GoldSalesCost, @SilverSalesCost)
						END
					ELSE
						INSERT INTO AbSure_Trans_FailedTransactions(DealerId, CarId, DebitAmount, ServiceTaxValue, UserId, GoldSalesCost, SilverSalesCost)
																VALUES(@DealerId, @CarId, @ModelAmount, @ServiceTaxValue, @UserId, @GoldSalesCost, @SilverSalesCost)
				END

				UPDATE AbSure_CarDetails SET IsActive = 0 WHERE StockId = @StockId       --Modified By : Ashwini Dhamankar on Oct 29,2015 (To make isactive = 0 )

				UPDATE AbSure_CarDetails SET StockId = @StockId,IsActive = 1,                        --Modified By : Kartik rathod on 23 sept 2015, Update Absure_Cardetails table to set the StockId of Duplicate car to Origional car
											AbSureWarrantyActivationStatusesId = 2        -- Modified By : Kartik rathod on 21 Oct 2015, Update Absure_Cardetails table to set the WarrantyActivationStatus = 1
										WHERE Id = @CarId

				IF(@OriginalStockId IS NOT NULL)
				BEGIN
					EXECUTE AbSure_ChangeCertification @OriginalStockId,NULL,NULL,@IsDuplicateCarSold
				END

				IF(@IsDuplicateCar = 1 AND @StockId IS NOT NULL)    -- As we are updating stockid of master car, update sellinquiry and livelisting for updated stockid
				BEGIN
					EXECUTE AbSure_ChangeCertification @StockId,NULL,NULL
				END
			END
		ELSE
		BEGIN
		--For cartrade warranty activation
			INSERT INTO AbSure_ActivatedWarranty
			(AbSure_CarDetailsId,CustName,Address,Mobile,AlternatePhone,Email,Model,MakeYear,RegNumber,RegistrationDate,Kilometer,
			WarrantyStartDate,WarrantyEndDate,DealerId,UserId,WarrantyTypeId, EngineNo, ChassisNo,OriginalStockId,DuplicateCarId,IsCarTradeWarranty)
			VALUES  (@CarId,@CustName,@Address,@Mobile,@AlternatePhone,@Email,@Model,@MakeYear,@RegNumber,@RegistrationDate,@Kilometer,
			@WarrantyStartDate,@WarrantyEndDate,@DealerId,@UserId,@WarrantyTypeId, @EngineNo, @ChassisNo,@OriginalStockId,@DuplicateCarId,1)
			
			INSERT INTO AbSure_Policy(AbSure_CarId,IsCarTradeWarranty) VALUES (@CarId,1)

			--Get amount to be deducted modelwise and warrantywise

			SELECT    @ModelId = V.ModelId,
					@ModelAmount =    /*CASE CD.FinalWarrantyTypeId WHEN 1 THEN EM.GoldPrice ELSE
									CASE CD.FinalWarrantyTypeId WHEN 2 THEN EM.SilverPrice END END,*/
									EM.GoldPrice, 
					@GoldSalesCost = EM.GoldSalesCost,
					@SilverSalesCost = EM.SilverSalesCost
			FROM    vwMMV V WITH(NOLOCK)
					INNER JOIN TC_CarTradeCertificationRequests CD WITH(NOLOCK) ON CD.Variant = V.Version
					INNER JOIN AbSure_EligibleModels EM WITH(NOLOCK) ON EM.ModelId = V.ModelId
					INNER JOIN AbSure_ActivatedWarranty AW WITH(NOLOCK) ON AW.AbSure_CarDetailsId = CD.TC_CarTradeCertificationRequestId
			WHERE   CD.TC_CarTradeCertificationRequestId = @CarId 

			IF @@ROWCOUNT > 0
          
				BEGIN
              
					IF @ModelAmount > 0
						BEGIN
							--Transaction related changes
							EXECUTE AbSure_Transaction
									@DealerId,
									@CarId,
									@ModelAmount,
									@ServiceTaxValue,
									@UserId,
									@GoldSalesCost,
									@SilverSalesCost
							
							
							/*start - Modified by   :	Ruchira Patil on 20th Mar 2015 (To update policy no)*/
							SELECT @PolicyNo = [dbo].[Absure_GenerateWarrantyPolicyNo](@CarId,1,1) -- 1 is for dealer
		
							/* Update certification request details table with policy no */
							UPDATE TC_CarTradeCertificationRequests SET PolicyNo = @PolicyNo 
							WHERE TC_CarTradeCertificationRequestId = @CarId
							/*end*/	
						END
					ELSE
						INSERT INTO AbSure_Trans_FailedTransactions(DealerId, CarId, DebitAmount, ServiceTaxValue, UserId, GoldSalesCost, SilverSalesCost)
														VALUES(@DealerId, @CarId, @ModelAmount, @ServiceTaxValue, @UserId, @GoldSalesCost, @SilverSalesCost)
				END
			ELSE
				INSERT INTO AbSure_Trans_FailedTransactions(DealerId, CarId, DebitAmount, ServiceTaxValue, UserId, GoldSalesCost, SilverSalesCost)
														VALUES(@DealerId, @CarId, @ModelAmount, @ServiceTaxValue, @UserId, @GoldSalesCost, @SilverSalesCost)
		
			SELECT @OriginalStockId = ListingId
			FROM TC_CarTradeCertificationRequests WITH(NOLOCK)
			WHERE TC_CarTradeCertificationRequestId = @CarId

			UPDATE TC_CarTradeCertificationRequests SET CertificationStatus = 7      --Set to indicate that verification is done
			WHERE TC_CarTradeCertificationRequestId = @CarId

			IF(@OriginalStockId IS NOT NULL)
			BEGIN
				--update sellinquiry and livelisting for updated stockid 
				EXECUTE AbSure_ChangeCertification @OriginalStockId,NULL,NULL,NULL
			END
		END
	END

	IF( @IsWarrantyActivationPending = 1)                                                --At time of the sell warranty form submitting the all details goes into the Absure_WarrantyActivationPending Table
		BEGIN
			UPDATE AbSure_ActivatedWarrantyLog SET    IsActive = 0
												WHERE AbSure_CarDetailsId = @CarId

			INSERT INTO AbSure_ActivatedWarrantyLog
						(AbSure_CarDetailsId,CustName,Address,Mobile,AlternatePhone,Email,EngineNo,ChassisNo,IsActive)
			VALUES        (@CarId,@CustName,@Address,@Mobile,@AlternatePhone,@Email, @EngineNo, @ChassisNo,1)

			UPDATE AbSure_WarrantyActivationPending SET        IsActive = 0                                --Before creating new entry for carId,all the old entries of that car must have IsActive=0
													WHERE    AbSure_CarDetailsId = @CarId

			INSERT INTO AbSure_WarrantyActivationPending                                                    --
						(AbSure_CarDetailsId,CustName,Address,Mobile,AlternatePhone,Email,Model,MakeYear,RegNumber,RegistrationDate,Kilometer,
						WarrantyStartDate,WarrantyEndDate,DealerId,UserId,WarrantyTypeId, EngineNo, ChassisNo,OriginalStockId,IsActive,Absure_WarrantyActivationStatusesId,DuplicateCarId, EntryDate)
			VALUES        (@CarId,@CustName,@Address,@Mobile,@AlternatePhone,@Email,@Model,@MakeYear,@RegNumber,@RegistrationDate,@Kilometer,
						@WarrantyStartDate,@WarrantyEndDate,@DealerId,@UserId,@WarrantyTypeId, @EngineNo, @ChassisNo,@OriginalStockId,1,1,@DuplicateCarId, GETDATE())            --isActive=1 & Absure_WarrantyActivationStatusesId=1
																																			--here all dealer for sell warranty Intialise the Warranty Request(Absure_WarrantyActivationStatusesId=1)

			UPDATE        AbSure_CarDetails    SET AbSureWarrantyActivationStatusesId = 1                     --(Request Initialise for sell warranty)
						WHERE Id = @CarId

		END
	END
	ELSE
	--For cartrade warranty request
	BEGIN
		IF( @IsWarrantyActivationPending = 1)                                                --At time of the sell warranty form submitting the all details goes into the Absure_WarrantyActivationPending Table
		BEGIN
			SELECT @CarId = TL.TC_CarTradeCertificationRequestId
			FROM TC_CarTradeCertificationLiveListing TL WITH(NOLOCK)
			WHERE ListingId = @ListingId

			INSERT INTO AbSure_WarrantyActivationPending                                                    --
						(AbSure_CarDetailsId,CustName,Address,Mobile,AlternatePhone,Email,Model,MakeYear,RegNumber,RegistrationDate,Kilometer,
						WarrantyStartDate,WarrantyEndDate,DealerId,UserId,WarrantyTypeId, EngineNo, ChassisNo,OriginalStockId,IsActive,Absure_WarrantyActivationStatusesId,DuplicateCarId, EntryDate,IsCarTradeWarranty)
			VALUES        (@CarId,@CustName,@Address,@Mobile,@AlternatePhone,@Email,@Model,@MakeYear,@RegNumber,@RegistrationDate,@Kilometer,
						@WarrantyStartDate,@WarrantyEndDate,@DealerId,@UserId,@WarrantyTypeId, @EngineNo, @ChassisNo,@OriginalStockId,1,1,@DuplicateCarId, GETDATE(),1)            --isActive=1 & Absure_WarrantyActivationStatusesId=1
																								--here all dealer for sell warranty Intialise the Warranty Request(Absure_WarrantyActivationStatusesId=1)
			
			
			
			UPDATE TC_CarTradeCertificationRequests	SET CertificationStatus = 6                     --(Request Initialise for sell warranty)
			WHERE TC_CarTradeCertificationRequestId = @CarId
		END
	END
END
