IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_SaveCarBookingData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_SaveCarBookingData]
GO

	CREATE PROCEDURE [dbo].[CRM_SaveCarBookingData]
	
	@CarBookingId			Numeric,
	@CarBasicDataId			Numeric,
	@BookingStatusId		Int,
	@Color					VarChar(100),
	@RegisterPersonName		VarChar(200),
	@UpdatedById			Numeric,
	@Comments				VarChar(1000),
	@NIFeedback				Bit,
	@NoFeedbackContact		Bit,

	@BookingRequestDate		DateTime,
	@BookingDate			DateTime,
	@CreatedOn				DateTime,
	@UpdatedOn				DateTime,
	@currentId				Numeric OutPut
				
 AS
	
BEGIN
	SET @currentId = -1
	
	UPDATE CRM_CarBookingData
	SET BookingStatusId = @BookingStatusId, Color = @Color, RegisterPersonName = @RegisterPersonName,
		Comments = @Comments, UpdatedBy = @UpdatedById,BookingRequestDate = @BookingRequestDate,
		BookingDate = @BookingDate, UpdatedOn = @UpdatedOn, NIFeedback = @NIFeedback, NoFeedbackContact = @NoFeedbackContact
	WHERE CarBasicDataId = @CarBasicDataId
	
	IF @@ROWCOUNT = 0
		BEGIN
			INSERT INTO CRM_CarBookingData
			(
				CarBasicDataId, BookingStatusId, Color, RegisterPersonName, UpdatedBy, 
				BookingRequestDate, BookingDate, CreatedOn, UpdatedOn, Comments, NIFeedback, NoFeedbackContact
			)
			VALUES
			(
				@CarBasicDataId, @BookingStatusId, @Color, @RegisterPersonName, @UpdatedById, 
				@BookingRequestDate, @BookingDate, @CreatedOn, @UpdatedOn, @Comments, @NIFeedback, @NoFeedbackContact
			)

			SET @currentId = Scope_Identity()
		END
	ELSE
		BEGIN
			SET @currentId = @CarBookingId
		END
END












