IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCS_FAAddZone]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCS_FAAddZone]
GO

	
CREATE PROCEDURE [dbo].[NCS_FAAddZone]
	@Id					NUMERIC,
	@FAId				NUMERIC,
	@ZoneName			VARCHAR(100),
	@Status				BIT OUTPUT
 AS
	
BEGIN
	IF @Id = -1 --Insertion
		BEGIN
			SELECT ID FROM NCS_FAZones 
			WHERE FAId = @FAId AND ZoneName = @ZoneName

			IF @@ROWCOUNT = 0

				BEGIN

					INSERT INTO NCS_FAZones
					(	FAId, ZoneName, IsActive
					)	
				
					Values
					(	@FAId, @ZoneName, 1
					)	

					SET @Status = 1

				END	

			ELSE
				
				SET @Status = 0
		END

	ELSE
		BEGIN
			
			UPDATE NCS_FAZones
			SET ZoneName = @ZoneName
			WHERE Id = @Id
				
			SET @Status = 1
		END	
END




