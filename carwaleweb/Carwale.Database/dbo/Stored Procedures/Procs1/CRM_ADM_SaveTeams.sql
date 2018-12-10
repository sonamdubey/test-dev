IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_ADM_SaveTeams]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_ADM_SaveTeams]
GO

	CREATE PROCEDURE [dbo].[CRM_ADM_SaveTeams]

	@Id			NUMERIC,
	@Name		VARCHAR(150),
	@IsActive	BIT,
	@IsNcd		BIT,
	@CreatedOn	DATETIME,
	@UpdatedOn	DATETIME,
	@UpdatedBy	NUMERIC,
	@Status		INT OUTPUT		
 AS
	
BEGIN
	SET @Status = 0

	IF @Id = -1
		BEGIN

			SELECT ID FROM CRM_ADM_Teams WHERE Name = @Name
			
			IF @@RowCount = 0
				BEGIN
					INSERT INTO CRM_ADM_Teams
					(
						Name, IsActive,IsNcd, CreatedOn, UpdatedOn, UpdatedBy
					) 
					VALUES
					( 
						@Name, @IsActive,@IsNcd, @CreatedOn, @UpdatedOn, @UpdatedBy
					)
		
					SET @Status = 1 
				END
			ELSE
				SET @Status = 0
		END
	ELSE
		BEGIN

			SELECT ID FROM CRM_ADM_Teams WHERE Name = @Name AND  ID <> @Id
			
			IF @@RowCount = 0

				BEGIN

					UPDATE CRM_ADM_Teams SET 
						Name = @Name, IsActive = @IsActive,IsNcd = @IsNcd,
						UpdatedOn = @UpdatedOn, UpdatedBy = @UpdatedBy
					 WHERE ID = @Id
				
					SET @Status = 1 

				END
			ELSE
				SET @Status = 0
		END
END

