IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_SaveCarBookingLog]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_SaveCarBookingLog]
GO

	


CREATE PROCEDURE [dbo].[CRM_SaveCarBookingLog]

	@Id						Numeric,
	@CBDId					Numeric,
	@CreatedBy				Numeric,
	@UpdatedBy				Numeric,
	@BookingNPDispositionId	Numeric,
	
	@IsBookingRequested		Bit,
	@IsBookingCompleted		Bit,
	@IsBookingNotPossible	Bit,
	@IsPriorBooking			Bit,
	
	@BookingRequestDate		DateTime,
	@BookingCompleteDate	DateTime,
	@UpdatedOn				DateTime,
	@CurrentId				Numeric OutPut,	
	------------------------Added by AMIT KUMAR on 31st 0ct 2012 ------------------
	@Color					varchar(100),
	@Comments				varchar(1000),
	@RegisterPersonName		varchar(200),
	@NIFeedback				Bit,
	@NoFeedbackContact		Bit
	-------------------------------------------------------------------------------
				
 AS
	
BEGIN
	SET @CurrentId = -1
	
	IF @Id = -1 AND @IsBookingRequested = 1

		BEGIN

			INSERT INTO CRM_CarBookingLog
			(
				CBDId, IsBookingRequested, BookingRequestDate, CreatedBy, BookingNPDispositionId,
				-------------Added By Amit Kumar---------------
				Color,Comments,NoFeedbackContact,
				NIFeedback,RegisterPersonName
				------------------------------------------------
			)
			VALUES
			(
				@CBDId, @IsBookingRequested, @BookingRequestDate, @CreatedBy, @BookingNPDispositionId,
				-------------Added By Amit Kumar---------------
				@Color,@Comments,@NoFeedbackContact,
				@NIFeedback,@RegisterPersonName
				------------------------------------------------
			)
			
			SET @CurrentId = SCOPE_IDENTITY()
		
		END
		
	ELSE
		
		BEGIN
			IF @IsBookingRequested = 1
				BEGIN
					UPDATE CRM_CarBookingLog 
					SET BookingRequestDate = @BookingRequestDate, UpdatedBy = @UpdatedBy,
						UpdatedOn = @UpdatedOn, BookingNPDispositionId = @BookingNPDispositionId,
					-------------Added By Amit Kumar on 31st oct 2012---------------
					Color=@Color,Comments=@Comments,RegisterPersonName=@RegisterPersonName,
					NIFeedback=@NIFeedback,NoFeedbackContact=@NoFeedbackContact
					
					----------------------------------------------------------------	
					WHERE Id = @Id
					
					SET @CurrentId = @Id
				END
				
			IF @IsBookingCompleted = 1
				BEGIN
					UPDATE CRM_CarBookingLog 
					SET IsBookingCompleted = 1, IsBookingNotPossible = 0, 
						BookingCompleteDate = ISNULL(@BookingCompleteDate,@UpdatedOn), UpdatedBy = @UpdatedBy,
						UpdatedOn = @UpdatedOn, BookingNPDispositionId = @BookingNPDispositionId,
						-------------Added By Amit Kumar on 31st oct 2012---------------
						Color=@Color,Comments=@Comments,RegisterPersonName=@RegisterPersonName,
						NIFeedback=@NIFeedback,NoFeedbackContact=@NoFeedbackContact
						
						----------------------------------------------------------------
					WHERE Id = @Id
					
					SET @CurrentId = @Id
				END
			
			IF @IsBookingNotPossible = 1
				BEGIN
					UPDATE CRM_CarBookingLog 
					SET IsBookingCompleted = 0, IsBookingNotPossible = 1, 
						BookingCompleteDate = ISNULL(@BookingCompleteDate,@UpdatedOn), UpdatedBy = @UpdatedBy,
						UpdatedOn = @UpdatedOn, BookingNPDispositionId = @BookingNPDispositionId,
						-------------Added By Amit Kumar on 31st oct 2012---------------
						Color=@Color,Comments=@Comments,RegisterPersonName=@RegisterPersonName,
						NIFeedback=@NIFeedback,NoFeedbackContact=@NoFeedbackContact
						
						----------------------------------------------------------------
					WHERE Id = @Id
					
					SET @CurrentId = @Id
				END
				
			IF @IsPriorBooking = 1
				BEGIN
					UPDATE CRM_CarBookingLog 
					SET BookingCompleteDate = ISNULL(@BookingCompleteDate,@UpdatedOn), IsPriorBooking = 1,
						IsBookingCompleted = 0, IsBookingNotPossible = 0, UpdatedBy = @UpdatedBy, BookingNPDispositionId = @BookingNPDispositionId,
						UpdatedOn = @UpdatedOn,
						-------------Added By Amit Kumar on 31st oct 2012---------------
						Color=@Color,Comments=@Comments,RegisterPersonName=@RegisterPersonName,
						NIFeedback=@NIFeedback,NoFeedbackContact=@NoFeedbackContact
						
						----------------------------------------------------------------
					WHERE Id = @Id
					
					SET @CurrentId = @Id
				END
		END
END















