IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_SaveCarDeliveryData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_SaveCarDeliveryData]
GO

	
CREATE PROCEDURE [dbo].[CRM_SaveCarDeliveryData]
	
	@CarDeliveryId			Numeric,
	@CarBasicDataId			Numeric,
	@DeliveryStatusId		Int,
	@DealerId				Numeric,
	@ChasisNumber			VarChar(50),
	@EngineNumber			VarChar(50),
	@Color					VarChar(100),
	@RegistrationNumber		VarChar(50),
	@DeliveryComments		VarChar(1000),
	@UpdatedById			Numeric,

	@ExpectedDeliveryDate	DateTime,
	@ActualDeliveryDate		DateTime,
	@CreatedOn				DateTime,
	@UpdatedOn				DateTime,
	@ContactPerson			VarChar(50),
	@Contact				VarChar(50),
	@currentId				Numeric OutPut
				
 AS
BEGIN
	SET @currentId = -1
	
	UPDATE CRM_CarDeliveryData 
	SET DeliveryStatusId = @DeliveryStatusId, DealerId = @DealerId, ChasisNumber = @ChasisNumber,
		EngineNumber = @EngineNumber, Color = @Color, RegistrationNumber = @RegistrationNumber,
		DeliveryComments = @DeliveryComments, UpdatedBy = @UpdatedById, UpdatedOn = @UpdatedOn,
		ExpectedDeliveryDate = @ExpectedDeliveryDate, ActualDeliveryDate = @ActualDeliveryDate,
		ContactPerson = @ContactPerson, Contact = @Contact
	WHERE CarBasicDataId = @CarBasicDataId
	
	IF @@ROWCOUNT = 0
		BEGIN

			INSERT INTO CRM_CarDeliveryData
			(
				CarBasicDataId, DeliveryStatusId, DealerId, ChasisNumber, EngineNumber, Color, 
				RegistrationNumber, DeliveryComments, UpdatedBy, ExpectedDeliveryDate,
				ActualDeliveryDate, CreatedOn, UpdatedOn,
				ContactPerson, Contact
			)
			VALUES
			(
				@CarBasicDataId, @DeliveryStatusId, @DealerId, @ChasisNumber, @EngineNumber, @Color,
				@RegistrationNumber, @DeliveryComments, @UpdatedById, @ExpectedDeliveryDate,
				@ActualDeliveryDate, @CreatedOn, @UpdatedOn,
				@ContactPerson, @Contact
			)
			
			SET @currentId = Scope_Identity()

		END

	ELSE

		BEGIN
			SET @currentId = @CarDeliveryId
		END
END


