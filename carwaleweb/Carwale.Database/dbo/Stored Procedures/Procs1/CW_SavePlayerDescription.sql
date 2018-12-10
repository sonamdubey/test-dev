IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CW_SavePlayerDescription]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CW_SavePlayerDescription]
GO

	
-- =============================================
-- Author      : Chetan Navin
-- Create date : 18 Feb 2015
-- Description : To insert and updated social feed data in database.
-- Module      : Product CRM Masters
-- =============================================
CREATE PROCEDURE [dbo].[CW_SavePlayerDescription]
	@Id					BIGINT , 
	@PlayerName			VARCHAR(100),
	@PlayerDescription  VARCHAR (2000),
	@Day            	INT,
	@IsActive           BIT = 1
AS
BEGIN
	
    IF (@Id = -1)
		BEGIN
			INSERT INTO CW_PlayerDescription(PlayerName,PlayerDescription,Day,IsActive) 
			VALUES (@PlayerName,@PlayerDescription,@Day,@IsActive)
		    
		    RETURN 1
		END
		
	ELSE
		BEGIN
			UPDATE CW_PlayerDescription SET PlayerName = @PlayerName,PlayerDescription = @PlayerDescription,Day = @Day,IsActive = @IsActive, ModifiedDate = GETDATE()
			WHERE Id = @Id
			       
			RETURN 2      
		END
END


