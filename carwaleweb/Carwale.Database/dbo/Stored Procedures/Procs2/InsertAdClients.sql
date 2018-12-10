IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertAdClients]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertAdClients]
GO

	

CREATE PROCEDURE [dbo].[InsertAdClients]
	@Id			BIGINT,
	@ClientName		VARCHAR(100), 
	@AgencyName		VARCHAR(100),
	@LoginId		VARCHAR(50),
	@Passwd		VARCHAR(50),
	@Status		INT OUTPUT		
 AS
	
BEGIN
	SET @Status = 0

	IF @Id = -1
		BEGIN

			SELECT ID FROM AW_AdClients WHERE LoginId = @LoginId
			
			IF @@RowCount = 0
				BEGIN
					INSERT INTO AW_AdClients(ClientName, AgencyName, LoginId, Passwd,IsActive ) 
					VALUES(@ClientName, @AgencyName, @LoginId, @Passwd, 1)
		
					SET @Status = 1 
				END
			ELSE
				SET @Status = 0
		END
	ELSE
		BEGIN
			SELECT ID FROM AW_AdClients WHERE LoginId = @LoginId AND ID <> @Id
			
			IF @@RowCount = 0

				BEGIN
					UPDATE  AW_AdClients SET ClientName = @ClientName,  AgencyName = @AgencyName, LoginId =  @LoginId,
					 Passwd = @Passwd WHERE ID = @Id
				
					SET @Status = 1 
				END
			ELSE
				SET @Status = 0
		END
END
