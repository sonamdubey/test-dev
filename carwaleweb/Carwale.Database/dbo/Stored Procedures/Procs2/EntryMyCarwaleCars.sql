IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[EntryMyCarwaleCars]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[EntryMyCarwaleCars]
GO

	--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR mycarwalecars table
--ID, CustomerId, VersionId, MakeYear, StartingKm, CurrentKm, PurchaseDate, RegistrationNo, ChasisNo, EngineNo, IsActive

CREATE PROCEDURE [dbo].[EntryMyCarwaleCars]
	@ID			NUMERIC,		--ID. IF IT IS -1 THEN IT IS FOR INSERT, ELSE IT IS FOR UPDATE FOR THE ID 		        	
	@CustomerId		NUMERIC, 
	@VersionId		NUMERIC, 
	@MakeYear		DATETIME, 
	@StartingKm		NUMERIC, 
	@CurrentKm		NUMERIC, 
	@AverageKm		NUMERIC,
	@PurchaseDate	DATETIME, 
	@RegistrationNo	VARCHAR(50), 
	@ChasisNo		VARCHAR(50), 
	@EngineNo		VARCHAR(50),
	@EntryDate		DATETIME,
	@LastUpdatedDate DATETIME,
	@InsuranceExpiryDate 	DATETIME,
	@MyCarwaleCarId	NUMERIC OUTPUT
 AS
	
BEGIN
	
	IF @ID = -1
	BEGIN		
		SELECT @MyCarwaleCarId = Id FROM MyCarwaleCars WHERE CustomerId = @CustomerId AND VersionId = @VersionId AND 
		Month(MakeYear) = Month(@MakeYear) AND Year(MakeYear) = Year(@MakeYear)

		IF @@RowCount = 0		
			BEGIN
				--IT IS FOR THE INSERT
				INSERT INTO MyCarwaleCars
					(
						CustomerId, 		VersionId, 		MakeYear, 		StartingKm, 	
						CurrentKm, 		AverageKm,		PurchaseDate, 		RegistrationNo, 		
						ChasisNo, 		EngineNo, 		EntryDate,		LastUpdatedDate,	
						IsActive, 		InsuranceExpiryDate
					)
				VALUES
					(	
						@CustomerId, 		@VersionId, 		@MakeYear, 		@StartingKm, 	
						@CurrentKm, 		@AverageKm,		@PurchaseDate, 	@RegistrationNo, 	
						@ChasisNo, 		@EngineNo, 		@EntryDate, 		@LastUpdatedDate,	
						1,			@InsuranceExpiryDate
					)

				Set @MyCarwaleCarId = SCOPE_IDENTITY()
			END
	END
	ELSE
	BEGIN
		--IT IS FOR THE UPDATE
		UPDATE MyCarwaleCars SET 
			CurrentKm		= @CurrentKm,
			StartingKm		= @StartingKm,
			AverageKm		= @AverageKm,
			MakeYear		= @MakeYear,
			PurchaseDate	= @PurchaseDate,			
			RegistrationNo	= @RegistrationNo,
			ChasisNo		= @ChasisNo,
			EngineNo		= @EngineNo,
			LastUpdatedDate	= @LastUpdatedDate
		 WHERE 
			ID = @ID
		
		Set @MyCarwaleCarId = @ID
	END	
END