IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_SaveBookingData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_SaveBookingData]
GO

	-- =============================================
-- Author:		Vaibhav Kale
-- Create date: 10-Aug-2012
-- Description:	Saves and updates the data for skoda booking engine customer
-- Modifier:	Vaibhav K (18-Feb-2013)
--				Added Varaint & Color Code & EngineFuelTrans
--				2. Vaibhav K (25-May-2013)
--				Also if model is changed by user again reset the values for the varaint selections
--				Parameters added for cheque payment
-- =============================================
CREATE PROCEDURE [dbo].[OLM_SaveBookingData]
	-- Add the parameters for the stored procedure here
	@Type				SMALLINT,
	@OldId				NUMERIC = -1, -- Passed as -1 for 1st entry page
	@CustomerName		VARCHAR(50) = NULL,
	@Mobile				VARCHAR(15) = NULL,
	@Email				VARCHAR(50) = NULL,
	@CityId				INT = -1,
	@ModelId			INT = -1,
	@Color				VARCHAR(20) = NULL,
	@ColorCode			VARCHAR(20) = NULL,
	@Price				VARCHAR(15) = NULL,
	@VersionId			INT = -1,
	@VariantCode		VARCHAR(20) = NULL,
	@EngineFuelTrans	VARCHAR(20) = NULL,
	@FuelType			SMALLINT = NULL,
	@TransmissionType	SMALLINT = NULL,	--Default Manual Transmission
	@EngineType			VARCHAR(15) = NULL,
	@ShowroomId			INT = -1,
	@PaymentType		SMALLINT = -1,
	@ChkAddress			VARCHAR(1000) = NULL,
	@ChkCity			VARCHAR(50) = NULL,
	@ChkState			VARCHAR(50) = NULL,
	@ChkPincode			VARCHAR(10) = NULL,
	@IpAddress			VARCHAR(20) = NULL,
	@OutletName			VARCHAR(50) = NULL,
	@OutletCode			VARCHAR(50) = NULL,
	@SkodaBookingId		VARCHAR(50) = NULL,
	@IsPaymentSuccessful	BIT	= 0,
	@LeadTokenNo		VARCHAR(50) = NULL,
	@TokenDateTime		DATETIME = NULL,
	@PushSuccess		BIT = 0,
	@PushErrorMsg		VARCHAR(2000) = NULL,
	@BookingXMLSent		VARCHAR(2000) = NULL,
	@BookingXMLResult	VARCHAR(2000) = NULL,
	@NewId				NUMERIC	OUTPUT

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @PreviousModel	INT
	SET @PreviousModel = -1
	
	SET @NewId = -1

    -- Insert statements for procedure here
	IF @Type = 1 AND @OldId = -1	--For new customer entry
		BEGIN
			INSERT INTO OLM_BookingData(CustomerName,Mobile,Email,CityId,ModelId,IpAddress,IsPaymentSuccessful)
				VALUES(@CustomerName,@Mobile,@Email,@CityId,@ModelId,@IpAddress,0)
				
			SET @NewId = SCOPE_IDENTITY()
		END
		
	IF @Type = 1 AND @OldId <> -1	--If customer redirected from next step & changed original data
		BEGIN
			
			SELECT @PreviousModel = ModelId FROM OLM_BookingData WHERE Id = @OldId
			
			--If model is same as that of previous model then only update details
			--Else if model is changed then reset the variant selected and its details
			IF @PreviousModel = @ModelId
				BEGIN
					UPDATE OLM_BookingData
						SET CustomerName = @CustomerName,
							Mobile = @Mobile,
							Email = @Email,
							CityId = @CityId,
							ModelId = @ModelId
					WHERE Id = @OldId
				END
			ELSE
				BEGIN
					UPDATE OLM_BookingData
						SET CustomerName = @CustomerName,
							Mobile = @Mobile,
							Email = @Email,
							CityId = @CityId,
							ModelId = @ModelId,
							Color = NULL,
							ColorCode = NULL,
							VersionId = NULL,
							VariantCode = NULL,
							Price = NULL,
							EngineFuelTrans = NULL,
							FuelType = NULL,
							TransmissionType = NULL,
							EngineType = NULL
					WHERE Id = @OldId
				END
			
			SET @NewId = @OldId
		END
	
	IF @Type = 2	--For updating model color & version details selected by the customer 
		BEGIN
			UPDATE OLM_BookingData
				SET Color = @Color,
					ColorCode = @ColorCode,
					VersionId = @VersionId,
					VariantCode = @VariantCode,
					Price = @Price,
					EngineFuelTrans = @EngineFuelTrans,
					FuelType = @FuelType,
					TransmissionType = @TransmissionType,
					EngineType = @EngineType
			WHERE Id = @OldId
			
			SET @NewId = @OldId
		END
		
	IF @Type = 3	--For updating selected dealer(showroom) & city if changed of the customer 
	BEGIN
		UPDATE OLM_BookingData
			SET ShowroomId = @ShowroomId,
				CityId = @CityId,
				OutletName = @OutletName,
				OutletCode = @OutletCode
		WHERE Id = @OldId
		
		SET @NewId = @OldId
	END
	
	IF @Type = 4	--For updating payment type of the customer 
	BEGIN
		UPDATE OLM_BookingData
			SET PaymentType = @PaymentType	--2 for cash payment
		WHERE Id = @OldId
		
		SET @NewId = @OldId
	END
	
	IF @Type = 5	--For final confirmation after redirecting from payment gateway
	BEGIN
		UPDATE OLM_BookingData
			SET IsPaymentSuccessful = @IsPaymentSuccessful,	--1 for check and 2 for cash payment
				OutletName = @OutletName,
				SkodaBookingId = @SkodaBookingId,
				LeadTokenNo = @LeadTokenNo,
				TokenDateTime = @TokenDateTime,
				PushSuccess = @PushSuccess,
				PushErrorMsg = @PushErrorMsg
		WHERE Id = @OldId
		
		SET @NewId = @OldId
	END
	
	IF @Type = 6	--For updating payment type of the customer and cheque payment details
	BEGIN
		UPDATE OLM_BookingData
			SET PaymentType = @PaymentType,		--1 for cheque
				ChkAddress = @ChkAddress,
				ChkCity = @ChkCity,
				ChkState = @ChkState,
				ChkPincode = @ChkPincode
		WHERE Id = @OldId
		
		SET @NewId = @OldId
	END
END
