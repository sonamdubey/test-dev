IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ESM_SaveCategory]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ESM_SaveCategory]
GO

	
/*  
 This procedure created on 13 Jan 2010 by Sentil  
 for update and save for ESM_Category   
*/ 

CREATE PROCEDURE [dbo].[ESM_SaveCategory]
(
	@Category AS VARCHAR(50),
	@type AS SMALLINT,
	@IsActive AS BIT,
	@UpdatedOn AS DATETIME,
	@ID AS NUMERIC(18,0),
	@UpdatedBy AS NUMERIC(18,0)
)
AS
BEGIN

	IF(@ID = -1)  
		BEGIN  
		   INSERT INTO ESM_Category ( Category, type, IsActive, UpdatedOn, UpdatedBy )  
					VALUES( @Category, @type, @IsActive, @UpdatedOn, @UpdatedBy)  
		END  
	ELSE  
		BEGIN  
			UPDATE ESM_Category SET   
			Category = @Category, type = @type, IsActive = @IsActive, UpdatedOn = @UpdatedOn,UpdatedBy = @UpdatedBy  
			WHERE id = @ID  
		END   

--SELECT * FROM ESM_Category
END
