IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_InsertUpdateETDisposition]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_InsertUpdateETDisposition]
GO

	
CREATE PROCEDURE [dbo].[CRM_InsertUpdateETDisposition]

	@ItemId			NUMERIC,
	@DispositonId	NUMERIC,
	@TypeId			NUMERIC,
	@EventType		NUMERIC,
	@CreatedBy		NUMERIC,
	@CreatedOn		DATETIME,
	@Status			INT OUTPUT		
 AS
	DECLARE 
		@Id		Numeric
	
BEGIN
	
	if EXISTS (SELECT ETL.Id FROM CRM_ETDispositions ETD, CRM_CarETDispositions ETL
		WHERE ETD.Id = ETL.DispositonId AND ETD.EventType = @EventType
		AND ItemId = @ItemId AND ETL.Type = @TypeId)
			BEGIN
				SELECT @Id = ETL.Id FROM CRM_ETDispositions ETD, CRM_CarETDispositions ETL
				WHERE ETD.Id = ETL.DispositonId AND ETD.EventType = @EventType
				AND ItemId = @ItemId AND ETL.Type = @TypeId
					
					BEGIN
						SET @Status = 0
						Update CRM_CarETDispositions 
						Set DispositonId = @DispositonId, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn
						Where ItemId = @ItemId AND TYPE = @TypeId AND Id = @Id
						SET @Status = 1
					END
			END
	ELSE
		BEGIN
			SET @Status = 0

			INSERT INTO CRM_CarETDispositions( ItemId, DispositonId,Type,CreatedBy,CreatedOn )
			VALUES(@ItemId,@DispositonId,@TypeId,@CreatedBy, @CreatedOn)
			SET @Status = 1
		END

END
