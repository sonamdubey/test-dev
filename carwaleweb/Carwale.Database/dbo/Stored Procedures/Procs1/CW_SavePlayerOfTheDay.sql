IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CW_SavePlayerOfTheDay]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CW_SavePlayerOfTheDay]
GO

	-- =============================================
-- Author      : Chetan Navin
-- Create date : 18 Feb 2015
-- Description : To insert and updated social feed data in database.
-- Module      : Product CRM Masters
-- =============================================
CREATE PROCEDURE [dbo].[CW_SavePlayerOfTheDay]
	@Id					BIGINT , 
	@WinnerName			VARCHAR(100),
	@WinnerComments		VARCHAR(MAX) = '',
	@PrizeText   		VARCHAR (100),
	@Day            	INT,
	@IsActive           BIT = 1
AS
BEGIN
	
    IF (@Id = -1)
		BEGIN
			INSERT INTO CW_PlayerOfTheDay(WinnerName,PrizeText,Day,IsActive, WinnerComments) 
			VALUES (@WinnerName,@PrizeText,@Day,@IsActive, @WinnerComments)
		    
		    RETURN 1
		END
		
	ELSE
		BEGIN
			UPDATE CW_PlayerOfTheDay SET WinnerName = @WinnerName,PrizeText = @PrizeText,Day = @Day,IsActive = @IsActive, WinnerComments = @WinnerComments, ModifiedDate = GETDATE()
			WHERE Id = @Id
			       
			RETURN 2      
		END
END

