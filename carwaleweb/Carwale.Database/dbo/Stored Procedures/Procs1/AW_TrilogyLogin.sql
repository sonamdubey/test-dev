IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AW_TrilogyLogin]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AW_TrilogyLogin]
GO

	

CREATE PROCEDURE [dbo].[AW_TrilogyLogin]
	@Id				BIGINT,
	@RecordId		VARCHAR(100), 
	@LoginId		VARCHAR(50),
	@Passwd			VARCHAR(50),
	@IsActive		BIT,
	@LDTakerId		NUMERIC,
	@Status			INT OUTPUT		
 AS
	
BEGIN
	SET @Status = 0

	IF @Id = -1
		BEGIN

			SELECT ID FROM TrilogyLogin WHERE LoginId = @LoginId
			
			IF @@RowCount = 0
				BEGIN
					INSERT INTO TrilogyLogin(Id, LoginId, Password,IsActive, LDTakerId ) 
					VALUES(@RecordId,  @LoginId, @Passwd, @IsActive, @LDTakerId)
		
					SET @Status = 1 
				END
			ELSE
				SET @Status = 0
		END
	ELSE
		BEGIN
			SELECT ID FROM TrilogyLogin WHERE LoginId = @LoginId AND ID <> @Id
			
			IF @@RowCount = 0

				BEGIN
					UPDATE  TrilogyLogin 
					SET  LoginId =  @LoginId, Password = @Passwd, IsActive = @IsActive, LDTakerId = @LDTakerId
					WHERE ID = @Id
				
					SET @Status = 1 
				END
			ELSE
				SET @Status = 0
		END
END
