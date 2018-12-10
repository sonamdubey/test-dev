IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[MG_SaveRequests]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[MG_SaveRequests]
GO

	/*************************************************************************************************************
       Description: This SP(Stored Procedure) will save the request for magazine

       Objects Used in Operation: MG_Volumes, MG_Requests

       Created On: 10 Apr 2008
       Created By: Satish Sharma

	Parameters:
	Parameter		Type			Desc
	-----------		-------------		-----------
	@CustomerId		input			Id of the customer intersted buying magazine
	@PaymentMode		input			How customer paying for magazine
	@Copies			input			How many copy of magazine customer is buying
	@Status			output			Returns '1' if request successfully saved alse '0'

       History:
       
       Edited By               		 EditedOn             		 Description
       ---------                	----------               			 -----------
       Satish Sharma                	10-Apr-08 10:47 AM			Created        		
       
**********************************************************************************************************/

CREATE      PROCEDURE [dbo].[MG_SaveRequests]
		@MagRequestId	NUMERIC,
	@CustomerId		NUMERIC,
	@PaymentMode		INT,
	@Copies		INT,
	@AmountPaid		INT,
	@RequestDateTime	DateTime,
	@ShippingAddress	VarChar(200),
	@PinCode		VarChar(50),
	@DeliveryStatus		INT,
	@ID			Numeric OUTPUT
	
AS
	DECLARE @VolumeId	INT
BEGIN
	SELECT @VolumeId=VolumeId FROM MG_Volumes WHERE CurrentVolume = 1

	IF @VolumeId > 0
		BEGIN
			IF @MagRequestId = -1
			BEGIN
				INSERT INTO MG_Requests(CustomerId, VolumeId, PaymentMode, Copies, Amount, DeliveryStatus, RequestDateTime)
				VALUES(@CustomerId, @VolumeId, @PaymentMode, @Copies, @AmountPaid, @DeliveryStatus, @RequestDateTime)
				
				Set @ID = SCOPE_IDENTITY()			
			
				Update MG_ShippingAddress Set ShippingAddress = @ShippingAddress, PinCode = @PinCode Where CustomerId = @CustomerId

				IF @@ROWCOUNT = 0
				BEGIN
					INSERT INTO MG_ShippingAddress VALUES(@CustomerId, @ShippingAddress, @PinCode)		
				END
				
			END
			ELSE
			BEGIN
				UPDATE MG_Requests Set
						PaymentMode = @PaymentMode, 
						Copies = @Copies, 
						Amount = @AmountPaid, 
						RequestDateTime = @RequestDateTime
				WHERE
					ID = @MagRequestId
				
				Set @ID = @MagRequestId			
			
				Update MG_ShippingAddress Set ShippingAddress = @ShippingAddress, PinCode = @PinCode Where CustomerId = @CustomerId
				
			END


		END
	ELSE SET @ID = -1
END
