IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_AddCarSynopsis]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_AddCarSynopsis]
GO

	CREATE PROCEDURE [dbo].[Con_AddCarSynopsis]
   @Id         NUMERIC,
    @ModelId    NUMERIC,
    @FullDescription VARCHAR(MAX),
    @SmallDescription VARCHAR(8000),
    @Pros             VARCHAR(500),
    @Cons             VARCHAR(500),
    @Looks            INT,
    @Performance            INT,
    @Fuel            INT,
    @Comfort            INT,
    @Safety            INT,
    @Interiors            INT,
    @Ride                INT,
    @Handling            INT,
    @Braking            INT,
    @Overall            INT,
    @IsActive           BIT,
    @EntryDateTime  DATETIME,
    @LastUpdated    DATETIME,
    @LastSavedId    NUMERIC OUTPUT,
	@UpdatedOn      DATETIME = NULL,
	@UpdatedBy		INT = NULL,	
	@CreatedBy		INT = NULL
 AS
   
BEGIN
     IF @Id = -1
     	BEGIN
	     SELECT ID FROM CarSynopsis WHERE ModelId = @ModelId
	     IF @@RowCount = 0
			BEGIN
			     INSERT INTO CarSynopsis
			             (
			                ModelId,FullDescription, SmallDescription, Pros, Cons, Looks, Performance,
					FuelEfficiency, Comfort, Safety, Interiors, RideQuality,
					Handling, Braking, Overall, IsActive, EntryDateTime, LastUpdated, CreatedBy
			             )
			      VALUES
			             (
			                @ModelId,@FullDescription, @SmallDescription, @Pros, @Cons, @Looks, @Performance,
					@Fuel, @Comfort, @Safety, @Interiors, @Ride, @Handling, 
					@Braking, @Overall, @IsActive, @EntryDateTime, @LastUpdated, @CreatedBy
			             )

      				SET @LastSavedId = SCOPE_IDENTITY()
			END
		ELSE	
			SET @LastSavedId = 0
        END
    ELSE
        BEGIN
		UPDATE CarSynopsis SET 
			SmallDescription = @SmallDescription,FullDescription = @FullDescription, Pros = @Pros,
			Cons =@Cons, Looks = @Looks, Performance = @Performance,
			FuelEfficiency = @Fuel, Comfort = @Comfort,Safety = @Safety, 
			Interiors = @Interiors, RideQuality = @Ride,
			Handling = @Handling, Braking = @Braking, Overall = @Overall, 
			IsActive = @IsActive, LastUpdated = @LastUpdated, UpdatedOn = @UpdatedOn,
			UpdatedBy = @UpdatedBy
		WHERE Id = @Id
            	
		SET @LastSavedId = @Id
        END
END
