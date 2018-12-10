IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_SaveInterestedIn]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_SaveInterestedIn]
GO

	CREATE PROCEDURE [dbo].[CRM_SaveInterestedIn]

	@Id					NUMERIC,
	@LeadId				NUMERIC,
	@ProductTypeId		SMALLINT,
	@ProductStatusId	INT,
	@ClosingProbability	INT,
	@ClosingDate		DATETIME,
	@CreatedOn			DATETIME,
	@UpdatedOn			DATETIME,
	@UpdatedBy			NUMERIC,
	@ClosingComments	VARCHAR(500),
	@IsMultipleBooking  BIT	= 0,
	@BookingCount		BIGINT = 0,
	@IIId				NUMERIC OUTPUT	
				
 AS
	
BEGIN
	SET @IIId = -1
	IF @Id = -1

		BEGIN
			
			SELECT @IIId = Id FROM CRM_InterestedIn WITH(NOLOCK) 
			WHERE LeadId = @LeadId AND ProductTypeId = @ProductTypeId
			
			IF @@ROWCOUNT = 0
				BEGIN	
					INSERT INTO CRM_InterestedIn
					(
						LeadId, ProductTypeId, ProductStatusId, ClosingProbability,
						ClosingDate, CreatedOn, UpdatedOn, UpdatedBy, ClosingComments,IsMultipleBooking,BookingCount
					)
					VALUES
					(
						@LeadId, @ProductTypeId, @ProductStatusId, @ClosingProbability, 
						@ClosingDate, @CreatedOn, @UpdatedOn, @UpdatedBy, @ClosingComments,@IsMultipleBooking,@BookingCount
					)
			
					SET @IIId = SCOPE_IDENTITY()
				END
			
		END
	
	ELSE

		BEGIN
			UPDATE CRM_InterestedIn 
			SET ProductStatusId = @ProductStatusId, ClosingProbability = @ClosingProbability,
				ClosingDate = @ClosingDate, UpdatedOn = @UpdatedOn, UpdatedBy = @UpdatedBy,
				ClosingComments = @ClosingComments, IsMultipleBooking = @IsMultipleBooking, BookingCount = @BookingCount
			WHERE Id = @Id
			
			SET @IIId = @Id
		END
END











