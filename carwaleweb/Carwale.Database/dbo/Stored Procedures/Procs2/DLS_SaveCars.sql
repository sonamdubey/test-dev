IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DLS_SaveCars]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DLS_SaveCars]
GO

	


CREATE PROCEDURE [dbo].[DLS_SaveCars]
	@Id				NUMERIC,
	@LeadId			VARCHAR(250),
	@CarVersionId	VARCHAR(150),
	@PQStatus		SMALLINT,
	@TDStatus		SMALLINT,
	@BookingStatus	SMALLINT,
	@DeliveryStatus SMALLINT,
	@LostStatus		SMALLINT,
	@Comments		VARCHAR(500),
	@UpdatedOn		DATETIME,
	@CurrentId		NUMERIC OUTPUT
 AS
	
BEGIN
	SET @CurrentId = -1
	
	IF @Id = -1
		BEGIN
			INSERT INTO DLS_CarData
			(
				LeadId, CarVersionId
			) 
			VALUES
			( 
				@LeadId, @CarVersionId
			)
				
			SET @CurrentId = SCOPE_IDENTITY() 
		END
	ELSE
		BEGIN
			UPDATE DLS_CarData 
			SET CarVersionId = @CarVersionId, PQStatus = @PQStatus, TDStatus = @TDStatus,
				DeliveryStatus = @DeliveryStatus, BookingStatus = @BookingStatus,
				LostStatus = @LostStatus, Comments = @Comments, UpdatedOn = @UpdatedOn
			WHERE Id = @Id 
			
			SET @CurrentId = @Id 
		END
END



