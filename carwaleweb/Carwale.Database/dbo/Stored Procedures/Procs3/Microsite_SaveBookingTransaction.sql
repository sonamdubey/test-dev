IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_SaveBookingTransaction]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_SaveBookingTransaction]
GO

	-- =============================================

-- Author:		Vaibhav Kale

-- Create date: 26 Mar 2015

-- Description:	Save or Update Booking Details from Dealer Booking Engines
-- Modified by : Rakesh Yadav on 15 Apr Insert and update AutoBizInqId
-- Modified by : Rakesh Yadav on 24 April 2015 delete selected offers if version is changed since offer is version specific 
-- Modified by : Rakesh Yadav on 29 May 2015 Added IsPaymentSuccessFull		
-- =============================================

CREATE PROCEDURE [dbo].[Microsite_SaveBookingTransaction]

	-- Add the parameters for the stored procedure here

	@BookingTransactionId			NUMERIC(18,0) OUTPUT,

	@DealerId			NUMERIC(18,0) = NULL,

	@MakeId				INT = NULL,

	@ModelId			INT = NULL,

	@OfferId			INT = NULL,

	@CustomerName		VARCHAR(100) = NULL,

	@CustomerMobile		VARCHAR(20) = NULL,

	@CustomerEmail		VARCHAR(100) = NULL,

	@CustomerStateId	INT = NULL,

	@CustomerCityId		INT = NULL,
	
	@CustomerAddress	VARCHAR(1000) = NULL,

	@FuelTypeId			INT = NULL,

	@TransmissionTypeId	INT = NULL,

	@VersionId			INT = NULL,

	@VersionPrice		VARCHAR(15) = NULL,

	@Color				VARCHAR(20) = NULL,

	@OutletId			INT = NULL,

	@PaymentType		INT = NULL,

	@PaymentMode		INT = NULL,

	@PaymentAmount		INT = NULL,

	@PGTransactionId	NUMERIC(18,0) = NULL,

	@PaymentDate		DATETIME = NULL,

	@ClientIp			VARCHAR(20) = NULL,

	@UserAgent			VARCHAR(50) = NULL,

	@HostName			VARCHAR(50) = NULL,

	@AutobizInqId		BIGINT = NULL,

	@BookingAmount INT = NULL,
	@PickUpDateTime DATETIME = NULL,
	@AutoBizResponse VARCHAR(500) = NULL,
	@IsPaymentSuccessfull bit=NULL
AS

BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from

	-- interfering with SELECT statements.

	SET NOCOUNT ON;



    -- Insert statements for procedure here



	IF @BookingTransactionId IS NULL OR @BookingTransactionId = -1

		BEGIN

			INSERT INTO Microsite_BookingTransaction

			(

				DealerId, MakeId, ModelId, OfferId, CustomerName, CustomerMobile, CustomerEmail, CustomerStateId, CustomerCityId, CustomerAddress,

				FuelTypeId, TransmissionTypeId, VersionId, VersionPrice, Color, OutletId, PaymentType, PaymentMode, PaymentAmount,

				PGTransactionId, PaymentDate, ClientIp, UserAgent, HostName, AutoBizInqId,IsPaymentSuccessfull

			)

			VALUES

			(

				@DealerId, @MakeId, @ModelId, @OfferId, @CustomerName, @CustomerMobile, @CustomerEmail, @CustomerStateId, @CustomerCityId, @CustomerAddress,

				@FuelTypeId, @TransmissionTypeId, @VersionId, @VersionPrice, @Color, @OutletId, @PaymentType, @PaymentMode, @PaymentAmount,

				@PGTransactionId, @PaymentDate, @ClientIp, @UserAgent, @HostName, @AutobizInqId,@IsPaymentSuccessfull

			)



			SET @BookingTransactionId = SCOPE_IDENTITY()



		END

	ELSE

		BEGIN

			DECLARE @OldModelId	INT, @OldVersionId INT
			
			SELECT @OldModelId = MBT.ModelId, @OldVersionId=MBT.VersionId 
			FROM Microsite_BookingTransaction MBT WITH(NOLOCK)
			WHERE Id = @BookingTransactionId
			
			UPDATE Microsite_BookingTransaction 

				SET 

				DealerId			= ISNULL(@DealerId, DealerId),

				MakeId				= ISNULL(@MakeId, MakeId),

				ModelId				= ISNULL(@ModelId, ModelId),

				OfferId				= ISNULL(@OfferId, OfferId),

				CustomerName		= ISNULL(@CustomerName, CustomerName),

				CustomerMobile		= ISNULL(@CustomerMobile, CustomerMobile),

				CustomerEmail		= ISNULL(@CustomerEmail, CustomerEmail),

				CustomerStateId		= ISNULL(@CustomerStateId, CustomerStateId),

				CustomerCityId		= ISNULL(@CustomerCityId, CustomerCityId),

				CustomerAddress		= ISNULL(@CustomerAddress, CustomerAddress),

				FuelTypeId			= ISNULL(@FuelTypeId, FuelTypeId),

				TransmissionTypeId	= ISNULL(@TransmissionTypeId, TransmissionTypeId),

				VersionId			= ISNULL(@VersionId, VersionId),

				VersionPrice		= ISNULL(@VersionPrice, VersionPrice),

				Color				= ISNULL(@Color, Color),

				OutletId			= ISNULL(@OutletId, OutletId),

				PaymentType			= ISNULL(@PaymentType, PaymentType),

				PaymentMode			= ISNULL(@PaymentMode, PaymentMode),

				PaymentAmount		= ISNULL(@PaymentAmount, PaymentAmount),

				PGTransactionId		= ISNULL(@PGTransactionId, PGTransactionId),

				PaymentDate			= ISNULL(@PaymentDate, PaymentDate),

				ModifiedDate		= GetDate(),

				ClientIp			= ISNULL(@ClientIp, ClientIp),

				UserAgent			= ISNULL(@UserAgent, UserAgent),

				HostName			= ISNULL(@HostName, HostName),

				AutoBizInqId		= ISNULL(@AutobizInqId, AutoBizInqId),
				BookingAmount = ISNULL(@BookingAmount,BookingAmount),
				PickUpDate = ISNULL(@PickUpDateTime,PickUpDate),
				AutoBizResponse = ISNULL(@AutoBizResponse,AutoBizResponse),
				IsPaymentSuccessfull= ISNULL(@IsPaymentSuccessfull,IsPaymentSuccessfull)
			WHERE Id = @BookingTransactionId


			--Vaibhav K 24-Apr-2015
			--If model is changed from old existing model then reset further transaction
			IF (@ModelId IS NOT NULL AND @OldModelId <> @ModelId)
				BEGIN
					UPDATE Microsite_BookingTransaction 
						SET VersionId = @VersionId,--set versionid if model is changed due to change of offer,
							VersionPrice = NULL,
							FuelTypeId = NULL,
							TransmissionTypeId = NULL,
							Color = NULL
					WHERE Id = @BookingTransactionId

					DELETE FROM Microsite_BookingTransactionOffers 
					WHERE Microsite_BookingTransactionId=@BookingTransactionId
				END

			IF(@VersionId IS NOT NULL AND @OldVersionId<>@VersionId)
			BEGIN
				DELETE FROM Microsite_BookingTransactionOffers 
				WHERE Microsite_BookingTransactionId=@BookingTransactionId
			END
		END

END