IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCS_AddCDCompanies]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCS_AddCDCompanies]
GO

	
--THIS PROCEDURE INSERTS THE VALUES FOR THE Cities

CREATE PROCEDURE [dbo].[NCS_AddCDCompanies]
	@Id				NUMERIC,		
	@Name			VARCHAR(500),
	@Status         BIT OUTPUT
 AS
	
BEGIN
	If @Id = -1
		BEGIN
			SELECT Id FROM NCS_CDCompanies WHERE Name = @Name

			IF @@ROWCOUNT = 0
				BEGIN
					INSERT INTO NCS_CDCompanies(Name, IsActive)			
					Values(@Name, 1)	

					SET @Status = 1
				END
			ELSE
				SET @Status = 0
		END

	ELSE

		BEGIN
			SELECT Id FROM NCS_CDCompanies WHERE Name = @Name AND Id <> @Id

			IF @@ROWCOUNT = 0
				BEGIN
					UPDATE NCS_CDCompanies SET Name = @Name			
					WHERE Id = @Id	

					SET @Status = 1
				END
			ELSE
				SET @Status = 0
		END
END



