IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCS_AddCDClassification]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCS_AddCDClassification]
GO

	
--THIS PROCEDURE INSERTS THE VALUES FOR THE Cities

CREATE PROCEDURE [dbo].[NCS_AddCDClassification]
	@Id				NUMERIC,		
	@CarMakeId		NUMERIC,
	@Name			VARCHAR(100),
	@Status         BIT OUTPUT
 AS
	
BEGIN
	If @Id = -1
		BEGIN
			SELECT Id FROM NCS_CDClassification WHERE Name = @Name AND CarMakeId = @CarMakeId

			IF @@ROWCOUNT = 0
				BEGIN
					INSERT INTO NCS_CDClassification(CarmakeId, Name, IsActive)			
					Values(@CarmakeId, @Name, 1)	

					SET @Status = 1
				END
			ELSE
				SET @Status = 0
		END

	ELSE

		BEGIN
			SELECT Id FROM NCS_CDClassification WHERE Name = @Name AND CarMakeId = @CarMakeId AND Id <> @Id

			IF @@ROWCOUNT = 0
				BEGIN
					UPDATE NCS_CDClassification SET Name = @Name			
					WHERE Id = @Id	

					SET @Status = 1
				END
			ELSE
				SET @Status = 0
		END
END


